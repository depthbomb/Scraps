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

using Microsoft.Web.WebView2.Core;

using Scraps.Events;
using Scraps.Services;

namespace Scraps.Controls;

public partial class WonRafflesControl : UserControl
{
    private const string SourceUrl = "https://scrap.tf/raffles/won";

    private bool _hasCookie;

    private readonly SettingsService _settings;
    private readonly Logger          _log = LogManager.GetCurrentClassLogger();
    
    public WonRafflesControl(RaffleService runner, SettingsService settings)
    {
        _settings = settings;
        
        runner.OnWithdrawalAvailable += RunnerOnWithdrawalAvailable;
        
        _settings.OnSaved += SettingsOnSaved;
        _settings.OnReset += SettingsOnReset;
        
        InitializeComponent();
        #pragma warning disable CS4014
        InitializeWebViewAsync();
        #pragma warning restore CS4014

        _WebView.NavigationStarting += WebViewOnNavigationStarting;
    }

    private async Task InitializeWebViewAsync()
    {
        try
        {
            var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: GlobalShared.DataPath);
            
            await _WebView.EnsureCoreWebView2Async(environment);

            #if RELEASE
            _WebView.CoreWebView2.Settings.AreDevToolsEnabled            = false;
            _WebView.CoreWebView2.Settings.IsStatusBarEnabled            = false;
            _WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            #endif
        }
        catch (Exception ex)
        {
            _log.Fatal(ex, "Failed to initialize WebView2");
            throw;
        }
    }

    /// <summary>
    /// Adds the user's saved cookie to WebView2
    /// </summary>
    private void AddCookie()
    {
        ClearBrowserData();
        _WebView.CoreWebView2.CookieManager.DeleteAllCookies();
        _WebView.CoreWebView2.CookieManager.AddOrUpdateCookie(
            _WebView.CoreWebView2.CookieManager.CreateCookie("scr_session", _settings.GetString("Cookie"), "scrap.tf", "/")
        );
    }

    private void NavigateToWonRafflesPage(bool addCookie = true)
    {
        if (addCookie)
        {
            AddCookie();
        }
        
        _WebView.CoreWebView2.Navigate(SourceUrl);
    }

    private void ClearBrowserData() => _WebView.CoreWebView2?.Profile.ClearBrowsingDataAsync();

    private void SetStatusFromCookie(string cookie)
    {
        if (Utils.IsValidCookie(cookie))
        {
            AddCookie();
            
            _hasCookie           = true;
            _StatusLabel.Visible = false;
            _StatusLabel.Enabled = false;
            _WebView.CoreWebView2.Navigate(SourceUrl);
        }
        else
        {
            _hasCookie           = false;
            _StatusLabel.Visible = true;
            _StatusLabel.Enabled = true;
        }
    }

    #region Service Event Subscriptions
    private void RunnerOnWithdrawalAvailable(object sender, RaffleServiceWithdrawalAvailableArgs e)
        => NavigateToWonRafflesPage(false);

    private void SettingsOnSaved(object sender, SettingsServiceSavedArgs e)
        => SetStatusFromCookie(_settings.GetString("Cookie"));

    private void SettingsOnReset(object sender, SettingsServiceResetArgs e)
        => SetStatusFromCookie(_settings.GetString("Cookie"));
    #endregion
    
    #region Control Event Subscriptions
    private void WonRafflesControl_Load(object sender, EventArgs e)
        => SetStatusFromCookie(_settings.GetString("Cookie"));
    
    private void WebViewOnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (_hasCookie)
        {
            string url = e.Uri;
            if (url != "https://scrap.tf/raffles/won")
            {
                e.Cancel = true;
            }
        }
    }
    #endregion
}
