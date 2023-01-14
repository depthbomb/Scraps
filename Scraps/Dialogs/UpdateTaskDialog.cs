#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2021 Caprine Logic
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Diagnostics;

using Scraps.Models;

namespace Scraps.Dialogs;

public class UpdateTaskDialog : IDisposable
{
    private readonly LatestReleaseResponse       _latestRelease;
    private readonly TaskDialogPage              _updatePage;
    private readonly TaskDialogPage              _progressPage;
    private readonly TaskDialogCommandLinkButton _downloadButton;
    private readonly TaskDialogCommandLinkButton _dismissButton;
    private readonly Logger                      _log = LogManager.GetCurrentClassLogger();
    
    public UpdateTaskDialog(LatestReleaseResponse latestRelease)
    {
        _latestRelease = latestRelease;
        _updatePage = new TaskDialogPage
        {
            Caption       = "Scraps",
            Heading       = "Update Available",
            Text          = $"Scraps {latestRelease.TagName} is available to download.",
            Icon          = TaskDialogIcon.ShieldBlueBar,
            SizeToContent = true,
            Expander = new TaskDialogExpander
            {
                Text                = latestRelease.Body,
                CollapsedButtonText = "View Changelog",
                ExpandedButtonText  = "Hide Changelog"
            },
        };
        _progressPage = new TaskDialogPage
        {
            Caption     = "Scraps",
            Heading     = "Downloading installer...",
            ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.Marquee)
        };
        _downloadButton = new TaskDialogCommandLinkButton
        {
            Text             = "Update",
            DescriptionText  = "Download and run the installer automatically",
            AllowCloseDialog = false,
        };
        _dismissButton = new TaskDialogCommandLinkButton
        {
            Text = "Dismiss"
        };
        
        _updatePage.Buttons.Add(_downloadButton);
        _updatePage.Buttons.Add(_dismissButton);
        _updatePage.DefaultButton = _downloadButton;
        
        _downloadButton.Click += DownloadButtonOnClick;
    }

    public TaskDialogButton ShowDialog()
        => TaskDialog.ShowDialog(_updatePage);

    private async void DownloadButtonOnClick(object sender, EventArgs e)
    {
        string downloadPath = Path.GetTempFileName();
        string downloadUrl  = _latestRelease.Assets.First(a => a.Name.StartsWith("scraps_setup")).BrowserDownloadUrl;
        
        _updatePage.Navigate(_progressPage);

        using (var http = new HttpClient())
        {
            _log.Debug("Downloading installer {Url}",downloadUrl);

            var res = await http.GetAsync(downloadUrl);
            if (res.IsSuccessStatusCode)
            {
                byte[] bytes = await res.Content.ReadAsByteArrayAsync();
                await using (var fs = new FileStream(downloadPath, FileMode.Create,  FileAccess.Write, FileShare.None))
                {
                    await fs.WriteAsync(bytes);
                }
                
                _log.Debug("Successfully downloaded installer to {Path}", downloadPath);

                _progressPage.BoundDialog?.Close();

                Process.Start(new ProcessStartInfo
                {
                    FileName  = downloadPath,
                    Arguments = "/update=yes"
                });
                
                Environment.Exit(0);
            }
            else
            {
                _log.Error("Unable to download release asset: {Reason}", res.ReasonPhrase);
                throw new Exception("Failed to download latest release. Please download it manually.");
            }
        }
    }

    public void Dispose()
    {
        _log.Debug("Disposing");

        _downloadButton.Click -= DownloadButtonOnClick;
    }
}
