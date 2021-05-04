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
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;

using NLog;

using Scraps.Models;

namespace Scraps.Services
{
    public class UpdateService : IDisposable
    {
        private bool _disposed;
        private readonly Logger _log;
        private readonly HttpClient _http;

        public UpdateService()
        {
            _log = LogManager.GetCurrentClassLogger();
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("user-agent", "Scraps - depthbomb/Scraps");
        }

        public async Task CheckForUpdates()
        {
            _log.Info("Checking for updates...");

            string uri = "https://api.github.com/repos/depthbomb/scraps/releases/latest";
            string json = await _http.GetStringAsync(uri);

            var release = JsonSerializer.Deserialize<LatestRelease>(json);
            var latestVersion = new System.Version(release.tag_name.Replace("v", ""));
            var compare = Constants.Version.AsDotNetVersion().CompareTo(latestVersion);
            if (compare < 0)
            {
                Console.WriteLine("Scraps {0} is available. Would you like to download and run the installer?", latestVersion);
                Console.WriteLine("Press Y to proceed or press N to continue without updating");
                var keyPressed = Console.ReadKey().Key;
                if (keyPressed == ConsoleKey.Y)
                {
                    Console.CursorVisible = false;
                    Console.Clear();

                    string tempLocation = Path.Combine(Path.GetTempPath(), "scraps_setup.exe");

                    _log.Info("Downloading installer...");

                    string windowsAsset = release.assets.Where(a => a.name.StartsWith("scraps_setup")).First().browser_download_url;
                    byte[] data = await _http.GetByteArrayAsync(windowsAsset);
                    using (var fs = new FileStream(tempLocation, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        fs.Write(data, 0, data.Length);
                    }

                    _log.Info("Running installer...");

                    var psi = new ProcessStartInfo();
                        psi.FileName = tempLocation;
                        psi.Arguments = "/update=yes";

                    Process.Start(psi);

                    Environment.Exit(0);
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _http.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
