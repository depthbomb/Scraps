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

using Scraps.Next.Services;
using Scraps.Next.Resources;
using Scraps.Next.Extensions;

namespace Scraps.Next.Controls;

public partial class AboutControl : UserControl
{
    private readonly UpdateService _update;
    
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
        string assLocation = Assembly.GetExecutingAssembly().Location;
        if (assLocation.IsNullOrEmpty())
        {
            assLocation = Path.Combine(AppContext.BaseDirectory, "scraps.exe");
        }
        var buildDate   = new FileInfo(assLocation).LastWriteTime;

        return buildDate.ToLocalTime().ToString("O");
    }

    private void _RepositoryLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        => Utils.OpenUrl("https://github.com/depthbomb/Scraps");

    private void _SubmitIssueLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        => Utils.OpenUrl("https://github.com/depthbomb/Scraps/issues/new/choose");

    private async void _CheckForUpdatesButton_Click(object sender, EventArgs e)
    {
        _CheckForUpdatesButton.Enabled = false;

        await _update.CheckForUpdatesAsync();

        if (!_update.IsUpdateAvailable)
        {
            Utils.ShowInfo(null, "Updater", "You are using the latest version of Scraps!");
        }

        _CheckForUpdatesButton.Enabled = true;
    }

    private void _AuthorGithubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        => Utils.OpenUrl("https://github.com/depthbomb");

    private void _OpenLogsFolderButton_Click(object sender, EventArgs e)
        => Process.Start("explorer", GlobalShared.LogsPath);
}