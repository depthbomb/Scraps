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

using Scraps.Next.Events;
using Scraps.Next.Services;

namespace Scraps.Next.Controls;

public partial class WonRafflesControl : UserControl
{
    private const string SourceUrl = "https://scrap.tf/raffles/won";

    private readonly SettingsService _settings;
    private readonly Logger          _log = LogManager.GetCurrentClassLogger();
    
    public WonRafflesControl(RaffleService runner, SettingsService settings)
    {
        _settings = settings;
        
        runner.OnWithdrawalAvailable += RunnerOnWithdrawalAvailable;
        
        _settings.OnSaved += SettingsOnSaved;
        _settings.OnReset += SettingsOnReset;
        
        InitializeComponent();
        InitializeWebViewAsync();

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
            
            NavigateToWonRafflesPage();
        }
        catch (Exception ex)
        {
            _log.Fatal(ex, "Failed to initialize WebView2");
            throw;
        }
    }

    private void NavigateToWonRafflesPage(bool addCookie = true)
    {
        if (addCookie)
        {
            _WebView.CoreWebView2.CookieManager.AddOrUpdateCookie(
                _WebView.CoreWebView2.CookieManager.CreateCookie("scr_session", _settings.Get<string>("Cookie"), "scrap.tf", "/")
            );
        }
        
        _WebView.CoreWebView2.Navigate(SourceUrl);
    }
    
    private void RunnerOnWithdrawalAvailable(object sender, RaffleServiceWithdrawalAvailableArgs e)
        => NavigateToWonRafflesPage(false);

    private void SettingsOnSaved(object sender, SettingsServiceSavedArgs e)
        => NavigateToWonRafflesPage();
    
    private void SettingsOnReset(object sender, SettingsServiceResetArgs e)
        => NavigateToWonRafflesPage();
    
    private void WebViewOnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
    {
        string url = e.Uri;
        if (!url.StartsWith("https://scrap.tf"))
        {
            e.Cancel = true;
        }
    }
}