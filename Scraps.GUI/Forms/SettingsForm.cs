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

using Scraps.GUI.Services;

namespace Scraps.GUI.Forms
{
    public partial class SettingsForm : Form
    {
        private bool _testingProxies = false;
        private CancellationToken _cancelToken;
        private CancellationTokenSource _cancelTokenSource;

        private readonly RaffleService _runner;
        private readonly ProxyService _proxy;

        public SettingsForm(RaffleService runner, ProxyService proxy)
        {
            _runner = runner;
            _proxy = proxy;

            InitializeComponent();
            PopulateControlValues();
            SubscribeToHelpEvents();
        }

        private void SubscribeToHelpEvents()
        {
            _CookieInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "This is where you paste your Scrap.TF cookie value. This is required for operation.");

            _SortNewToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Enabling this will make Scraps load newer raffles rather than by time remaining.");

            _ParanoidToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Enabling this will make Scraps be extra strict when checking raffles as to avoid honeypots.");

            _ToastToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Enabling this will enable toast notifications for various events.");

            _ScanDelayInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "This is where you can change the delay between scan operations (in milliseconds).");

            _PaginateDelayInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "This is where you can change the delay between loading raffle index page results (in milliseconds).");

            _JoinDelayInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "This is where you can change the delay between joining queued raffles (in milliseconds).");

            _IncrementScanDelayToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Enabling this will make Scraps increment the Scan Delay by 1 second if a scan operation resulted in no available raffles to join.");

            _ProxiesInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Here you may enter HTTP proxy addresses (+ port) that Scraps will use when operating. Separate each address by a single new line.");

            _TestProxiesButton.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Tests the current above proxies to ensure that they don't reveal your IP address.");

            _CancelProxyTestButton.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Cancels the proxy test as soon as it can.");

            _SaveButton.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo(this, "Help", "Settings changes made won't take effect until this button is clicked.");
        }

        private void PopulateControlValues()
        {
            _CookieInput.Text                 = Properties.UserConfig.Default.Cookie;
            _SortNewToggle.Checked            = Properties.UserConfig.Default.SortByNew;
            _ParanoidToggle.Checked           = Properties.UserConfig.Default.Paranoid;
            _ToastToggle.Checked              = Properties.UserConfig.Default.ToastNotifications;
            _ScanDelayInput.Value             = Properties.UserConfig.Default.ScanDelay;
            _PaginateDelayInput.Value         = Properties.UserConfig.Default.PaginateDelay;
            _JoinDelayInput.Value             = Properties.UserConfig.Default.JoinDelay;
            _IncrementScanDelayToggle.Checked = Properties.UserConfig.Default.IncrementScanDelay;
            _ProxiesInput.Text                = Properties.UserConfig.Default.Proxies;
        }

        private async void TestProxiesButton_OnClick(object sender, EventArgs e)
        {
            if (_ProxiesInput.Text.Trim().Length == 0) return;

            _cancelTokenSource = new();
            _cancelToken = _cancelTokenSource.Token;

            _testingProxies = true;
            _SaveButton.Enabled = false;
            _TestProxiesButton.Enabled = false;
            _CancelProxyTestButton.Visible = true;

            List<string> brokenProxies = new();
            string[] proxies = _ProxiesInput.Text.Split("\n");
            foreach (string proxy in proxies)
            {
                if (_cancelToken.IsCancellationRequested) break;

                bool working = await _proxy.TestProxyAsync(proxy.Trim());
                if (!working)
                {
                    brokenProxies.Add(proxy);
                }
            }

            if (brokenProxies.Count > 0)
            {
                string invalidProxies = string.Join("\n", brokenProxies);
                Utils.ShowError(this, "Proxy Test", $"The following proxies don't appear to work:\n\n{invalidProxies}");
            }
            else
            {
                Utils.ShowInfo(this, "Proxy Test", "All proxies are working!");
            }

            _testingProxies = false;
            _SaveButton.Enabled = true;
            _TestProxiesButton.Enabled = true;
            _CancelProxyTestButton.Visible = false;
            _CancelProxyTestButton.Enabled = true;
        }

        private void CancelProxyTestButton_OnClick(object sender, EventArgs e)
        {
            _CancelProxyTestButton.Enabled = false;

            _cancelTokenSource.Cancel();
        }

        private void SaveButton_OnClick(object sender, EventArgs e)
        {
            string cookie           = _CookieInput.Text.Trim();
            bool sortByNew          = _SortNewToggle.Checked;
            bool paranoid           = _ParanoidToggle.Checked;
            bool toast              = _ToastToggle.Checked;
            int scanDelay           = (int)_ScanDelayInput.Value;
            int paginateDelay       = (int)_PaginateDelayInput.Value;
            int joinDelay           = (int)_JoinDelayInput.Value;
            bool incrementScanDelay = _IncrementScanDelayToggle.Checked;
            string proxies          = _ProxiesInput.Text.Trim();

            if (string.IsNullOrEmpty(cookie)) return;
            if (cookie.Length < 360)
            {
                Utils.ShowError(this, "Invalid Cookie Length", "Your cookie value is likely invalid due to it being shorter in length than expected. If you are sure that this is incorrect then please submit an issue.");
                return;
            }
            if (cookie.Contains("scr_session"))
            {
                Utils.ShowError(this, "Invalid Cookie Value", "Your cookie value is invalid. Make sure that you input ONLY the value of the scr_session cookie.");
                return;
            }

            Properties.UserConfig.Default.Cookie = cookie;
            Properties.UserConfig.Default.SortByNew = sortByNew;
            Properties.UserConfig.Default.Paranoid = paranoid;
            Properties.UserConfig.Default.ToastNotifications = toast;
            Properties.UserConfig.Default.ScanDelay = scanDelay;
            Properties.UserConfig.Default.PaginateDelay = paginateDelay;
            Properties.UserConfig.Default.JoinDelay = joinDelay;
            Properties.UserConfig.Default.IncrementScanDelay = incrementScanDelay;
            Properties.UserConfig.Default.Proxies = proxies;
            Properties.UserConfig.Default.Save();
            Properties.UserConfig.Default.Reload();

            if (_runner is RaffleService && _runner.Running)
            {
                Utils.ShowWarning(this, "Warning", "Some changes won't go into effect until the raffle runner is restarted.");
            }

            this.Close();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_testingProxies)
            {
                e.Cancel = true;
            }

            if (_CookieInput.Text == "" && Properties.UserConfig.Default.Cookie == "")
            {
                Application.Exit();
            }
        }
    }
}
