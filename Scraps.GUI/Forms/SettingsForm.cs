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
using System.Windows.Forms;

using Scraps.GUI.RaffleRunner;

namespace Scraps.GUI.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly RaffleRunnerService _runner;

        public SettingsForm(RaffleRunnerService runner)
        {
            _runner = runner;

            InitializeComponent();
            SubscribeToHelpEvents();
            PopulateControlValues();
        }

        private void SubscribeToHelpEvents()
        {
            _CookieInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "This is where you paste your Scrap.TF cookie value. This is required for operation.");

            _SortNewToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "Enabling this will make Scraps load newer raffles rather than by time remaining.");

            _ParanoidToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "Enabling this will make Scraps be extra strict when checking raffles as to avoid honeypots.");

            _ScanDelayInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "This is where you can change the delay between scan operations (in milliseconds).");

            _PaginateDelayInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "This is where you can change the delay between loading raffle index page results (in milliseconds).");

            _JoinDelayInput.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "This is where you can change the delay between joining queued raffles (in milliseconds).");

            _IncrementScanDelayToggle.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "Enabling this will make Scraps increment the Scan Delay by 1 second if a scan operation resulted in no available raffles to join.");

            _SaveButton.HelpRequested += (object s, HelpEventArgs h)
                => Utils.ShowInfo("Help", "Settings changes made won't take effect until this button is clicked.");
        }

        private void PopulateControlValues()
        {
            _CookieInput.Text                 = Properties.UserConfig.Default.Cookie;
            _SortNewToggle.Checked            = Properties.UserConfig.Default.SortByNew;
            _ParanoidToggle.Checked           = Properties.UserConfig.Default.Paranoid;
            _ScanDelayInput.Value             = Properties.UserConfig.Default.ScanDelay;
            _PaginateDelayInput.Value         = Properties.UserConfig.Default.PaginateDelay;
            _JoinDelayInput.Value             = Properties.UserConfig.Default.JoinDelay;
            _IncrementScanDelayToggle.Checked = Properties.UserConfig.Default.IncrementScanDelay;
        }

        private void SaveButton_OnClick(object sender, EventArgs e)
        {
            string cookie           = _CookieInput.Text.Trim();
            bool sortByNew          = _SortNewToggle.Checked;
            bool paranoid           = _ParanoidToggle.Checked;
            int scanDelay           = (int)_ScanDelayInput.Value;
            int paginateDelay       = (int)_PaginateDelayInput.Value;
            int joinDelay           = (int)_JoinDelayInput.Value;
            bool incrementScanDelay = _IncrementScanDelayToggle.Checked;

            if (InputIsValid())
            {
                Properties.UserConfig.Default.Cookie = cookie;
                Properties.UserConfig.Default.SortByNew = sortByNew;
                Properties.UserConfig.Default.Paranoid = paranoid;
                Properties.UserConfig.Default.ScanDelay = scanDelay;
                Properties.UserConfig.Default.PaginateDelay = paginateDelay;
                Properties.UserConfig.Default.JoinDelay = joinDelay;
                Properties.UserConfig.Default.IncrementScanDelay = incrementScanDelay;
                Properties.UserConfig.Default.Save();
                Properties.UserConfig.Default.Reload();

                if (_runner is RaffleRunnerService && _runner.Running)
                {
                    Utils.ShowWarning("Warning", "Some changes won't go into effect until the raffle runner is restarted.", MessageBoxButtons.OK);
                }

                this.Close();
            }
        }
        
        private bool InputIsValid() => (
            !string.IsNullOrEmpty(_CookieInput.Text)
        );
    }
}
