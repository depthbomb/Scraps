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

using Scraps.Next.Events;
using Scraps.Next.Services;

namespace Scraps.Next.Controls;

public partial class SettingsControl : UserControl
{
    private readonly SettingsService _settings;
    private readonly RaffleService   _runner;
    
    public SettingsControl(SettingsService settings, RaffleService runner)
    {
        _settings   = settings;
        _runner     = runner;
        
        _settings.OnSaved += SettingsOnSaved;
        _settings.OnReset += SettingsOnReset;
        
        InitializeComponent();
        PopulateControlValues();
    }

    private void PopulateControlValues()
    {
        _CookieInput.Text                     = _settings.Get<string>("Cookie");
        _ScanDelayInput.Value                 = _settings.Get<int>("ScanDelay");
        _AutoIncrementScanDelayToggle.Checked = _settings.Get<bool>("IncrementScanDelay");
        _PaginateDelayInput.Value             = _settings.Get<int>("PaginateDelay");
        _JoinDelayInput.Value                 = _settings.Get<int>("JoinDelay");
        _RaffleSortByNewToggle.Checked        = _settings.Get<bool>("SortByNew");
        _ParanoidModeToggle.Checked           = _settings.Get<bool>("Paranoid");
        _TopmostToggle.Checked                = _settings.Get<bool>("AlwaysOnTop");
        _CheckUpdatesToggle.Checked           = _settings.Get<bool>("CheckUpdates");
        _FetchAnnouncementsToggle.Checked     = _settings.Get<bool>("FetchAnnouncements");
    }
    
    #region Service Event Subscriptions
    
    private void SettingsOnSaved(object sender, SettingsServiceSavedArgs e)
        => PopulateControlValues();
    
    private void SettingsOnReset(object sender, SettingsServiceResetArgs e)
        => PopulateControlValues();
    
    #endregion

    #region Control Event Subscriptions
    
    private void _CookieInput_MouseEnter(object sender, EventArgs e)
        => _CookieInput.UseSystemPasswordChar = false;

    private void _CookieInput_MouseLeave(object sender, EventArgs e)
        => _CookieInput.UseSystemPasswordChar = true;

    private void _SaveSettingsButton_Click(object sender, EventArgs e)
    {
        string cookie             = _CookieInput.Text.Trim();
        int    scanDelay          = (int)_ScanDelayInput.Value;
        bool   incrementScanDelay = _AutoIncrementScanDelayToggle.Checked;
        int    paginateDelay      = (int)_PaginateDelayInput.Value;
        int    joinDelay          = (int)_JoinDelayInput.Value;
        bool   sortByNew          = _RaffleSortByNewToggle.Checked;
        bool   paranoid           = _ParanoidModeToggle.Checked;
        bool   alwaysOnTop        = _TopmostToggle.Checked;
        bool   checkUpdates       = _CheckUpdatesToggle.Checked;
        bool   fetchAnnouncements = _FetchAnnouncementsToggle.Checked;
        
        if (cookie.Contains("scr_session"))
        {
            Utils.ShowError(null, "Invalid Cookie Value", "Your cookie value is invalid. Make sure that you input ONLY the value of the scr_session cookie.");
            return;
        }
        
        _settings
            .Set("Cookie", cookie)
            .Set("ScanDelay", scanDelay)
            .Set("IncrementScanDelay", incrementScanDelay)
            .Set("PaginateDelay", paginateDelay)
            .Set("JoinDelay", joinDelay)
            .Set("SortByNew", sortByNew)
            .Set("Paranoid", paranoid)
            .Set("AlwaysOnTop", alwaysOnTop)
            .Set("CheckUpdates", checkUpdates)
            .Set("FetchAnnouncements", fetchAnnouncements)
            .Save();

        if (_runner.Running)
        {
            Utils.ShowWarning(null, "Warning", "Some changes won't go into effect until you restart the runner.");
        }
    }

    private void _ResetSettingsButton_Click(object sender, EventArgs e)
    {
        if (_runner.Running)
        {
            Utils.ShowError(null, "Runner Active", "The runner must be stopped before you can reset your settings.");
            return;
        }
        
        var res = Utils.ShowWarning(null, "Reset Settings", "Are you sure you want to reset your settings to their default values?", MessageBoxButtons.YesNo);
        if (res != DialogResult.Yes)
        {
            return;
        }
        
        _settings.Reset();
    }
    
    #endregion
}
