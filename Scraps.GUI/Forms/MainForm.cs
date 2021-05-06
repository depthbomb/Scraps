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
using System.Diagnostics;
using System.Windows.Forms;

using NLog;
using NLog.Config;
using NLog.Targets;

using Scraps.GUI.Logging;
using Scraps.GUI.Constants;
using Scraps.GUI.Extensions;
using Scraps.GUI.RaffleRunner;
using Scraps.GUI.RaffleRunner.Events;

namespace Scraps.GUI.Forms
{
    public partial class MainForm : Form
    {
        private RaffleRunnerService _runner;

        private bool _running = false;
        private bool _exitOnCancel = false;

        public MainForm()
        {
            InitializeComponent();

            InitializeLogger();
            InitializeSettings();
            InitializeRaffleRunner();

            var updater = new UpdaterForm();
                updater.ShowDialog(this);

            this.Text = string.Format("Scraps - {0}", Constants.Version.Full);
            this.FormClosing += MainForm_OnClosing;
        }

        private void InitializeSettings()
        {
            if (string.IsNullOrEmpty(Properties.UserConfig.Default.Cookie))
            {
                var settingsWindow = new SettingsForm(_runner ?? null);
                    settingsWindow.ShowDialog(this);
            }

            Properties.UserConfig.Default.Reload();
        }

        private void InitializeLogger()
        {
            var config = new LoggingConfiguration();
            var rtbTarget = new RtbTarget(_LogWindow)
            {
                Layout = "${message}${exception}"
            };
            var fileTarget = new FileTarget
            {
                Layout = @"${longdate} | ${pad:padding=5:inner=${level:uppercase=true}} | [${logger:shortName=true}] ${message}${exception}",
                ArchiveEvery = FileArchivePeriod.Month,
                ArchiveFileName = "backup.{#}.zip",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveDateFormat = "yyyyMMddHHmm",
                EnableArchiveFileCompression = true,
                FileName = Path.Combine(Paths.LogsPath, "Scraps.${date:format=yyyy-MM}.log"),
                CreateDirs = true,
                MaxArchiveFiles = 5,
            };

            config.AddTarget("RTB", rtbTarget);
            config.AddTarget("File", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, rtbTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;
        }

        private void InitializeRaffleRunner()
        {
            _runner = new();
            _runner.OnStatus += OnStatus;
            _runner.OnStarting += OnStarting;
            _runner.OnRunning += OnRunning;
            _runner.OnAccountBanned += OnAccountBanned;
            _runner.OnPaginate += OnPaginate;
            _runner.OnPaginateDone += OnPaginateDone;
            _runner.OnRafflesWon += OnRafflesWon;
            _runner.OnRaffleJoined += OnRaffleJoined;
            _runner.OnStopping += OnStopping;
            _runner.OnStopped += OnStopped;
        }

        private void ResetProgressBar()
        {
            _ProgressBar.Value = 0;
            _ProgressBar.Style = ProgressBarStyle.Continuous;
        }

        private void ResetStatus() => _Status.Text = null;

        #region Event Subscriptions
        private void OnStatus(object sender, StatusArgs e) => _Status.Text = e.Message;

        private void OnStarting(object sender, StartingArgs e)
        {
            _StartStopButton.Enabled = false;
            _ProgressBar.Style = ProgressBarStyle.Marquee;
            _StartStopButton.Text = "Starting...";
        }

        private void OnRunning(object sender, RunningArgs e)
        {
            ResetProgressBar();

            _running = true;
            _StartStopButton.Image = Icons.Stop;
            _StartStopButton.Enabled = true;
            _StartStopButton.Text = "Stop";
            _TrayIcon.Text = "Scraps is running";
        }

        private void OnAccountBanned(object sender, AccountBannedArgs e) => _runner.Cancel();

        private void OnPaginate(object sender, PaginateArgs e) => _ProgressBar.Style = ProgressBarStyle.Marquee;

        private void OnPaginateDone(object sender, PaginateDoneArgs e) => ResetProgressBar();

        private void OnRafflesWon(object sender, RafflesWonArgs e)
        {
            var wonRaffles = e.RaffleIds;
            int numWonRaffles = wonRaffles.Count;

            bool enableToast = Properties.UserConfig.Default.ToastNotifications;
            if (enableToast)
            {
                _TrayIcon.ShowBalloonTip(60_000, "Items Need Withdrawing", string.Format("You've won {0} {1} that {2} to be withdrawn!", numWonRaffles, "raffle".Pluralize(numWonRaffles), "needs".Pluralize(numWonRaffles, "need")), ToolTipIcon.Info);
                _TrayIcon.BalloonTipClicked += (object sender, EventArgs e) => Process.Start("explorer", "https://scrap.tf/raffles/won");
            }
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e)
        {
            _ProgressBar.Minimum = 0;
            _ProgressBar.Maximum = e.Total;

            // A dumb fix because visual changes to the bar's value are delayed.
            if (_ProgressBar.Value <= _ProgressBar.Maximum - 2)
            {
                _ProgressBar.Value += 2;
                _ProgressBar.Value--;
            }
            else if (_ProgressBar.Value <= _ProgressBar.Maximum)
            {
                _ProgressBar.Maximum--;
            }
        }

        private void OnStopping(object sender, StoppingArgs e)
        {
            _ProgressBar.Style = ProgressBarStyle.Marquee;
            _StartStopButton.Enabled = false;
            _StartStopButton.Text = "Stopping...";
        }

        private void OnStopped(object sender, StoppedArgs e)
        {
            ResetProgressBar();

            _running = false;
            _StartStopButton.Image = Icons.Start;
            _StartStopButton.Enabled = true;
            _StartStopButton.Text = "Start";
            _TrayIcon.Text = "Scraps";

            if (_exitOnCancel)
            {
                Application.Exit();
            }
        }
        #endregion

        #region Control Event Subscriptions
        private void MainForm_OnClosing(object sender, FormClosingEventArgs e)
        {
            // Only intercept if the raffle runner is running, otherwise just close normally.
            if (_running)
            {
                e.Cancel = true;
                _exitOnCancel = true;
                _runner.Cancel();
            }

            // Finish any log writing that needs to be done.
            LogManager.Flush();
        }

        private async void StartStopButton_OnClick(object sender, EventArgs e)
        {
            if (_running)
            {
                _runner.Cancel();
            }
            else
            {
                try
                {
                    await _runner.StartAsync();
                }
                catch(Exception ex)
                {
                    ResetStatus();
                    ResetProgressBar();

                    _runner.Cancel();

                    _StartStopButton.Enabled = true;
                    _StartStopButton.Text = "Start";
                    _StartStopButton.Image = Icons.Start;

                    Utils.ShowError("Error", ex.Message);
                }
            }
        }

        private void SettingsButton_OnClick(object sender, EventArgs e)
        {
            var settingsWindow = new SettingsForm(_runner);
                settingsWindow.ShowDialog(this);
        }

        private void InfoButton_OnClick(object sender, EventArgs e)
        {
            var aboutWindow = new AboutForm();
            aboutWindow.ShowDialog(this);
        }
        #endregion
    }
}
