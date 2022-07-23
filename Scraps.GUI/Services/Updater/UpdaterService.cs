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

using Scraps.GUI.Services.Updater.Models;

namespace Scraps.GUI.Services;

public class UpdaterService : IDisposable
{
    private const string Url = "https://api.github.com/repos/depthbomb/scraps/releases/latest";

    private readonly Logger _log;
    private readonly HttpClient _http;

    private LatestRelease _latestRelease;

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

        string json = await _http.GetStringAsync(Url);

        _latestRelease = JsonSerializer.Deserialize<LatestRelease>(json);
        if (_latestRelease != null)
        {
            var latestVersion = new Version(_latestRelease.tag_name.Replace("v", ""));
            string body = _latestRelease.body;
            string releaseUrl = $"https://github.com/depthbomb/Scraps/releases/tag/{latestVersion}";
            var compare = Constants.Version.AsDotNetVersion().CompareTo(latestVersion);
            if (compare < 0)
            {
                var updateAvailablePage = new TaskDialogPage
                {
                    Caption = "Scraps",
                    Heading = "Update Available",
                    Text = $"Scraps {latestVersion} is available to download.",
                    Icon = TaskDialogIcon.ShieldBlueBar,
                    SizeToContent = true,
                    Expander = new TaskDialogExpander
                    {
                        Text = body,
                        CollapsedButtonText = "View Changelog",
                        ExpandedButtonText = "Hide Changelog"
                    },
                };
                var progressPage = new TaskDialogPage
                {
                    Caption = "Scraps",
                    Heading = "Downloading installer...",
                    ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.Marquee)
                };
                var downloadButton = new TaskDialogCommandLinkButton
                {
                    Text = "Update",
                    DescriptionText = "Download and run the installer automatically",
                    AllowCloseDialog = false,
                };
                var dismissButton = new TaskDialogCommandLinkButton
                {
                    Text = "Dismiss"
                };

                downloadButton.Click += async (s, e) =>
                {
                    _log.Debug("Downloading installer");

                    updateAvailablePage.Navigate(progressPage);

                    string tempLocation = Path.GetTempFileName();
                    string windowsAsset = _latestRelease.assets.First(a => a.name.StartsWith("scraps_setup")).browser_download_url;
                    byte[] data = await _http.GetByteArrayAsync(windowsAsset);
                    await using (var fs = new FileStream(tempLocation, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        fs.Write(data, 0, data.Length);
                    }

                    _log.Debug("Installer downloaded to {Path}", tempLocation);

                    if (progressPage.BoundDialog != null) progressPage.BoundDialog.Close();

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = tempLocation,
                        Arguments = "/update=yes"
                    });

                    Application.Exit();
                };

                updateAvailablePage.Buttons.Add(downloadButton);
                updateAvailablePage.Buttons.Add(dismissButton);
                updateAvailablePage.DefaultButton = downloadButton;

                var res = TaskDialog.ShowDialog(updateAvailablePage);
            }
        }
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}