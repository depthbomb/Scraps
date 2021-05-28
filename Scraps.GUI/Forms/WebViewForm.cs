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
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Scraps.GUI.Forms
{
    public partial class WebViewForm : Form
    {
        public WebViewForm(string url, string cookies = null)
        {
            if (!IsRuntimeInstalled())
            {
                DownloadAndInstallRuntime();
            }
            else
            {
                InitializeComponent();
                InitializeBrowserAsync(url, cookies);

                _WebBrowser.NavigationCompleted += WebBrowser_OnNavigationCompleted;
                _StatusStripButton.Click += StatusStripButton_OnClick;
            }

            this.FormClosing += OnFormClosing;
        }

        private bool IsRuntimeInstalled()
        {
            try
            {
                CoreWebView2Environment.GetAvailableBrowserVersionString();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void DownloadAndInstallRuntime()
        {
            string runtimeUrl = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
            if (MessageBox.Show(this, "This feature requires the WebView2 Runtime to be installed on your system.\n\nWould you like to download and install the runtime?", "WebView2 Runtime Required", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                using (var http = new HttpClient())
                {
                    var request = http.GetAsync(runtimeUrl).Result;
                    if (request.IsSuccessStatusCode)
                    {
                        string tempFile = Path.GetTempFileName();
                        byte[] data = request.Content.ReadAsByteArrayAsync().Result;

                        File.WriteAllBytes(tempFile, data);

                        Process.Start(tempFile);

                        Application.Exit();
                    }
                    else
                    {
                        if (MessageBox.Show(this, "The runtime could not be downloaded. Would you like to manually download the runtime?", "Failed to download runtime", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        {
                            Process.Start("explorer.exe", runtimeUrl);
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                }
            }
            else
            {
                this.Close();
            }
        }

        private async void InitializeBrowserAsync(string url, string cookies)
        {
            await _WebBrowser.EnsureCoreWebView2Async(null);

            _WebBrowser.CoreWebView2.Settings.AreDevToolsEnabled = false;

            if (cookies != null)
            {
                string[] cookieList = cookies.Split(';');
                foreach (string cookie in cookieList)
                {
                    var splitCookie = cookie.Split('=');
                    string key = splitCookie[0];
                    string value = splitCookie[1];

                    // Since this form is only going to be used to open Scrap.TF pages, just go ahead and hard-code the cookie domain.
                    var webCookie = _WebBrowser.CoreWebView2.CookieManager.CreateCookie(key, value, "scrap.tf", "/");

                    _WebBrowser.CoreWebView2.CookieManager.AddOrUpdateCookie(webCookie);
                }
            }

            _WebBrowser.CoreWebView2.Navigate(url);
        }

        private void WebBrowser_OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            string url = _WebBrowser.Source.ToString();

            this.Text = url;
            _StatusStripLabel.Text = url;
        }

        private void StatusStripButton_OnClick(object sender, EventArgs e)
            => Process.Start("explorer.exe", _WebBrowser.Source.ToString());

        private void OnFormClosing(object sender, FormClosingEventArgs e) => _WebBrowser?.Dispose();
    }
}
