#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2022 Caprine Logic

/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.

/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
/// GNU General Public License for more details.

/// You should have received a copy of the GNU General Public License
/// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion License

using Scraps.GUI.Forms;
using Scraps.GUI.Models;
using Scraps.GUI.Services;
using Scraps.GUI.Constants;

namespace Scraps.GUI
{
    static class Bootstrapper
    {
        public static LaunchOptions _options;

        [STAThread]
        static async Task Main(string[] args)
        {
            using (var mutex = new Mutex(false, "RaffleRunner Windows"))
            {
                if (!mutex.WaitOne(0))
                {
                    IntPtr intPtr = Native.FindWindowByCaption(IntPtr.Zero, string.Format("Scraps - {0}", Constants.Version.Full));
                    Native.SendMessage(intPtr, Native.WM_RAFFLERUNNER_SHOWME, IntPtr.Zero, IntPtr.Zero);
                    Environment.Exit(0);
                }

                _options = ParseOptions(args);

                EnsureFileSystem();

                if (Properties.UserConfig.Default.UpgradeRequired)
                {
                    Properties.UserConfig.Default.Upgrade();
                    Properties.UserConfig.Default.UpgradeRequired = false;
                    Properties.UserConfig.Default.Save();
                }

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    if (!_options.SkipUpdates)
                    {
                        using (var updater = new UpdaterService())
                        {
                            await updater.CheckForUpdatesAsync();
                        }
                    }
                }
                catch { }

                Application.Run(new MainForm(_options));
            }
        }

        private static void EnsureFileSystem()
        {
            foreach (string path in new string[] { Paths.LOGS_PATH, Paths.DATA_PATH })
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        private static LaunchOptions ParseOptions(string[] args)
        {
            var options = new LaunchOptions
            {
                Debug = args.Any(a => a == "/Debug"),
                Silent = args.Any(a => a == "/Silent"),
                SkipUpdates = args.Any(a => a == "/SkipUpdate"),
                SkipAnnouncements = args.Any(a => a == "/SkipAnnouncements")
            };

            return options;
        }
    }
}
