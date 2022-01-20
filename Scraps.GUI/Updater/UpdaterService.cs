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

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

using Scraps.GUI.Updater.Events;
using Scraps.GUI.Updater.Models;

namespace Scraps.GUI.Updater
{
    public class UpdaterService
    {
        private const string URL = "https://api.github.com/repos/depthbomb/scraps/releases/latest";

        private readonly Logger _log;
        private readonly HttpClient _http;

        private LatestRelease _latestRelease;

        #region Events
        /// <summary>
        /// Raised when the updater begins checking for updates
        /// </summary>
        public event EventHandler<CheckingForUpdatesArgs> OnCheckingForUpdates;

        /// <summary>
        /// Raised when a new version is available to download
        /// </summary>
        public event EventHandler<UpdateAvailableArgs> OnUpdateAvailable;

        /// <summary>
        /// Raised when the installer has started to be downloaded
        /// </summary>
        public event EventHandler<DownloadingInstallerArgs> OnDownloadingInstaller;

        /// <summary>
        /// Raised when the installer has finished downloading to a path
        /// </summary>
        public event EventHandler<InstallerDownloadedArgs> OnInstallerDownloaded;

        /// <summary>
        /// Raised when there are no available updates
        /// </summary>
        public event EventHandler<UpToDateArgs> OnUpToDate;
        #endregion

        public UpdaterService()
        {
            _log = LogManager.GetCurrentClassLogger();
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("user-agent", "Scraps - depthbomb/Scraps");
        }

        /// <summary>
        /// Checks for a new version of the application and prompts the user to update if a new version is available
        /// </summary>
        /// <returns></returns>
        public async Task CheckForUpdatesAsync()
        {
            _log.Debug("Checking for updates");

            OnCheckingForUpdates?.Invoke(this, new());

            string json = await _http.GetStringAsync(URL);

            _latestRelease = JsonSerializer.Deserialize<LatestRelease>(json);
            var latestVersion = new Version(_latestRelease.tag_name.Replace("v", ""));
            var compare = Common.Constants.Version.AsDotNetVersion().CompareTo(latestVersion);
            if (compare < 0)
            {
                OnUpdateAvailable?.Invoke(this, new(latestVersion));
            }
            else
            {
                OnUpToDate?.Invoke(this, new());
            }
        }

        /// <summary>
        /// Downloads the installer to a temporary location
        /// </summary>
        /// <returns></returns>
        public async Task DownloadUpdateAsync()
        {
            OnDownloadingInstaller?.Invoke(this, new());

            string tempLocation = Path.Combine(Path.GetTempPath(), "scraps_setup.exe");

            _log.Debug("Downloading installer");

            string windowsAsset = _latestRelease.assets.Where(a => a.name.StartsWith("scraps_setup")).First().browser_download_url;
            byte[] data = await _http.GetByteArrayAsync(windowsAsset);
            using (var fs = new FileStream(tempLocation, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(data, 0, data.Length);
            }

            _log.Debug("Installer downloaded to {Path}", tempLocation);

            OnInstallerDownloaded?.Invoke(this, new(tempLocation));
        }
    }
}
