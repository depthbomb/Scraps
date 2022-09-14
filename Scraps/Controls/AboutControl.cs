#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2022 Caprine Logic
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

using System.Reflection;
using System.Diagnostics;

using Scraps.Services;
using Scraps.Resources;
using Scraps.Extensions;

namespace Scraps.Controls;

public partial class AboutControl : UserControl
{
    private readonly UpdateService _update;
    private readonly Logger        _log = LogManager.GetCurrentClassLogger();
    
    public AboutControl(UpdateService update)
    {
        _update = update;
        
        InitializeComponent();

        _AssemblyNameLabel.Text      = "Scraps {0}".Format(GlobalShared.FullVersion);
        _AssemblyBuildDateLabel.Text = "Built on {0}".Format(GetAssemblyBuildDate());
        _LicenseTextLabel.Text       = Strings.LicenseText;
    }

    private string GetAssemblyBuildDate()
    {
        #pragma warning disable IL3000
        string assLocation = Assembly.GetExecutingAssembly().Location;
        #pragma warning restore IL3000
        if (assLocation.IsNullOrEmpty())
        {
            assLocation = Path.Combine(AppContext.BaseDirectory, "scraps.exe");
        }
        var buildDate   = new FileInfo(assLocation).LastWriteTime;

        return buildDate.ToString("F");
    }

    private async void _RepositoryLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        => await Utils.OpenUrl("https://github.com/depthbomb/Scraps");

    private async void _SubmitIssueLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        => await Utils.OpenUrl("https://github.com/depthbomb/Scraps/issues/new/choose");

    private async void _CheckForUpdatesButton_Click(object sender, EventArgs e)
    {
        _CheckForUpdatesButton.Enabled = false;

        try
        {
            await _update.CheckForUpdatesAsync();

            if (!_update.IsUpdateAvailable)
            {
                Utils.ShowInfo(null, "Updater", "You are using the latest version of Scraps!");
            }
        }
        catch (Exception ex)
        {
            _log.Error("Failed to check for updates");
            _log.Error(ex);
            
            switch (ex)
            {
                case HttpRequestException:
                    Utils.ShowError(null, "Updater", "Failed to check for updates because of a network issue.\nCheck your internet connection and try again.");
                    break;
                default:
                    Utils.ShowError(null, "Updater", "Failed to check for updates for an unknown reason.\nPlease check for new releases on GitHub.");
                    break;
            }
        }

        _CheckForUpdatesButton.Enabled = true;
    }

    private async void _AuthorGithubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        => await Utils.OpenUrl("https://github.com/depthbomb");

    private async void _OpenLogsFolderButton_Click(object sender, EventArgs e)
        => await Utils.OpenDirectory(GlobalShared.LogsPath);
}