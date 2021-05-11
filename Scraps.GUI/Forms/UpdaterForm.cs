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
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;

using Scraps.GUI.Updater;
using Scraps.GUI.Updater.Events;

namespace Scraps.GUI.Forms
{
    public partial class UpdaterForm : Form
    {
        private readonly UpdaterService _updater;

        public UpdaterForm()
        {
            _updater = new UpdaterService();

            InitializeComponent();

            _updater.OnCheckingForUpdates += OnCheckingForUpdates;
            _updater.OnUpdateAvailable += OnUpdateAvailable;
            _updater.OnDownloadingInstaller += OnDownloadingInstaller;
            _updater.OnInstallerDownloaded += OnInstallerDownloaded;
            _updater.OnUpToDate += OnUpToDate;

            this.Shown += UpdaterForm_OnShown;
        }

        private async void UpdaterForm_OnShown(object sender, EventArgs e) => await _updater.CheckForUpdatesAsync();

        private void OnCheckingForUpdates(object sender, CheckingForUpdatesArgs e) => _UpdateStatusLabel.Text = "Checking for updates...";

        private async void OnUpdateAvailable(object sender, UpdateAvailableArgs e)
        {
            var version = e.NewVersion;
            var result = Utils.ShowInfo("Update available", string.Format("Scraps {0} is available to download.\n\nWould you like to download and run the installer?", version), MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                _UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                await _updater.DownloadUpdateAsync();
            }
            else
            {
                Close();
            }
        }

        private void OnDownloadingInstaller(object sender, DownloadingInstallerArgs e)
        {
            _UpdateStatusLabel.Text = "Downloading installer...";
            _UpdateProgressBar.Increment(75);
        }

        private async void OnInstallerDownloaded(object sender, InstallerDownloadedArgs e)
        {
            _UpdateStatusLabel.Text = "Running installer...";
            _UpdateProgressBar.Increment(100);

            await Task.Delay(500);

            string installerPath = e.DownloadPath;
            var psi = new ProcessStartInfo();
                psi.FileName = installerPath;
                psi.Arguments = "/update=yes";

            Process.Start(psi);

            Application.Exit();
        }

        private void OnUpToDate(object sender, UpToDateArgs e) => Close();
    }
}