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
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;

using Scraps.Common.Constants;

namespace Scraps.GUI.Forms
{
    public partial class WebViewForm : Form
    {
        public WebViewForm(string url, string cookies = null)
        {
            if (!IsRuntimeInstalled())
            {
                Task.Run(async () => await DownloadAndInstallRuntimeAsync()).Wait();
            }

            InitializeComponent();
            InitializeBrowserAsync(url, cookies);

            _WebBrowser.NavigationStarting += WebBrowser_NavigationStarting;
            _WebBrowser.NavigationCompleted += WebBrowser_OnNavigationCompleted;
            _StatusStripButton.Click += StatusStripButton_OnClick;

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

        private async Task DownloadAndInstallRuntimeAsync()
        {
            string runtimeUrl = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
            using (var http = new HttpClient())
            {
                var request = await http.GetAsync(runtimeUrl);
                if (request.IsSuccessStatusCode)
                {
                    string tempFile = Path.GetTempFileName();
                    byte[] data = await request.Content.ReadAsByteArrayAsync();

                    File.WriteAllBytes(tempFile, data);

                    var installation = Process.Start(new ProcessStartInfo
                    {
                        FileName = tempFile,
                        Arguments = "/install",
                        Verb = "runas"
                    });

                    await installation.WaitForExitAsync();

                    File.Delete(tempFile);

                    if (installation.ExitCode != 0)
                    {
                        MessageBox.Show(this, "The runtime could not be installed. Please attempt to install it manually or create an issue on the GitHub repo.", "Failed to install runtime", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                        Application.Exit();
                    }
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

        private async void InitializeBrowserAsync(string url, string cookies)
        {
            var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: Paths.DATA_PATH);
            await _WebBrowser.EnsureCoreWebView2Async(environment);

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

        private void WebBrowser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            string url = e.Uri;
            string status = string.Format("Loading {0}", url);

            this.Text = status;
            _StatusStripLabel.Text = status;
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
