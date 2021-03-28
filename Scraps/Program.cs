#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2021  Caprine Logic

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

using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using Scraps.Models;
using Scraps.Constants;
using Scraps.Extensions;

namespace Scraps
{
    class Program
    {
        static bool _verbose;
        static Logger _logger;
        static Config _config;
        static HttpClient _http;

        static async Task Main(string[] args)
        {
            if (!HasWritePermissions())
            {
                if (Platform.IsUnix)
                {
                    Console.WriteLine("Scraps needs to be ran with elevated permissions.");
                    Console.WriteLine("Please run Scraps with sudo.");
                    Environment.Exit(1);
                }
                else
                {
                    string assembly = Process.GetCurrentProcess().MainModule.FileName;
                    Console.WriteLine(assembly);
                    var si = new ProcessStartInfo(assembly)
                    {
                        Arguments = "runas",
                    };

                    Process.Start(si);
                    Environment.Exit(0);
                }
            }

            if (IsAlreadyRunning())
            {
                Environment.Exit(1);
            }

            Console.WriteLine();
            Console.WriteLine("Scraps - Scrap.TF Raffle Bot");
            Console.WriteLine("By depthbomb - https://s.team/p/fwc-crhc");
            Console.WriteLine("Changelog available at https://github.com/depthbomb/Scraps/blob/master/CHANGELOG.md");
            Console.WriteLine();

            CheckForNewReleases();

            ParseArguments(args);

            InitializeLogger();
            InitializeSettings();
            InitializeHttpClient();

            Console.WriteLine("=".Repeat(Console.BufferWidth));
            Console.WriteLine();

            await new Bot(_logger, _config, _http).RunAsync();
        }

        static bool IsAlreadyRunning() => Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1;

        static bool HasWritePermissions()
        {
            string str = new Random().Next(10000, 99999999).ToString();
            try
            {
                File.WriteAllText(str, str);
                File.Delete(str);

                return true;
            }
            catch
            {
                return false;
            }
        }

        static void CheckForNewReleases()
        {
            Console.WriteLine("Checking for new releases...");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);

                string uri = "https://api.github.com/repos/depthbomb/scraps/releases/latest";
                string json = client.GetStringAsync(uri).GetAwaiter().GetResult();
                var currentTag = new System.Version(Constants.Version.SemVer);

                var release = JsonSerializer.Deserialize<LatestRelease>(json);
                var latestTag = new System.Version(release.tag_name.Replace("v", ""));   //  In case I'm inconsistent in tagging

                var compare = currentTag.CompareTo(latestTag);
                if (compare < 0)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("A new version of Scraps ({0}) is available to download at {1}", latestTag.ToString(), release.html_url);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine("You are running the latest version of Scraps!");
                }

                Console.WriteLine();
            }
        }

        static void ParseArguments(string[] args)
        {
            bool hasVerboseArgs = args.Contains("-v") || args.Contains("--verbose");
            bool hasOpenConfigArgs = args.Contains("-c") || args.Contains("--config");

            if (hasOpenConfigArgs)
            {
                Console.WriteLine("Opening settings file...");
                Process.Start("explorer.exe", Files.ConfigFile);
                Environment.Exit(0);
            }

            _verbose = hasVerboseArgs;
        }

        #region Initializers
        static void InitializeLogger()
        {
            bool debug;
#if DEBUG
            debug = true;
#else
            debug = _verbose;
#endif
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    restrictedToMinimumLevel: debug ? LogEventLevel.Debug : LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] {Message:l}{NewLine}{Exception}"
                 )
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine(Paths.LogsPath, "Fatal-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(Path.Combine(Paths.LogsPath, "Errors-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine(Paths.LogsPath, "Warning-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(Path.Combine(Paths.LogsPath, "Info-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.File(Path.Combine(Paths.LogsPath, "Verbose-.log"), rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        static void InitializeSettings()
        {
            var cm = new ConfigManager();
            if (cm.FileExists())
            {
                _config = cm.Read();

                if (_config.Version != App.SettingsVersion)
                {
                    Console.WriteLine("Your settings file is outdated, press [Enter] to back up the old version so you may fill out the new one.");
                    Console.ReadLine();
                    cm.Save(_config, "Settings_OLD");
                    cm.Save(new Config { Version = App.SettingsVersion });
                    if (!Platform.IsUnix)
                    {
                        SelectFile(Files.ConfigFile);
                    }
                    Console.WriteLine("Press [Enter] to continue.");
                    Console.ReadLine();
                    Console.WriteLine();

                    _config = cm.Read();
                }
            }
            else
            {
                cm.Save(new Config { Version = App.SettingsVersion });

                Console.WriteLine("A default config file has been created at {0}", Files.ConfigFile);
                Console.WriteLine("Documentation for the settings file can be found at {0}", "https://github.com/depthbomb/Scraps/blob/master/SETTINGS.md");
                Console.WriteLine("Please fill it out and press [Enter] to continue.");
                Console.ReadLine();
                Console.WriteLine();

                _config = cm.Read();
            }
        }

        static void InitializeHttpClient()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
            };

            var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);
                client.DefaultRequestHeaders.Add("cookie", "scr_session=" + _config.Cookie);

            _http = client;
        }
        #endregion

        static void SelectFile(string path)
        {
            string args = string.Format("/e, /select, \"{0}\"", path);

            var pInfo = new ProcessStartInfo()
            {
                FileName = "explorer",
                Arguments = args
            };

            Process.Start(pInfo);
        }
    }
}
