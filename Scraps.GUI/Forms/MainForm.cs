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
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;

using NLog;
using NLog.Config;
using NLog.Targets;

using Scraps.GUI.Logging;
using Scraps.GUI.Extensions;
using Scraps.Common.Constants;
using Scraps.GUI.RaffleRunner;
using Scraps.Common.Announcement;
using Scraps.GUI.RaffleRunner.Events;

namespace Scraps.GUI.Forms
{
    public partial class MainForm : Form
    {
        private RaffleRunnerService _runner;

        private bool _running = false;
        private bool _exitOnCancel = false;

        public MainForm(string[] args)
        {
            ToastNotificationManagerCompat.OnActivated += Toast_OnActivated;

            InitializeComponent();

            var updater = new UpdaterForm();
                updater.ShowDialog();

            InitializeLogger();
            InitializeSettings();
            InitializeRaffleRunner();

            CheckForAnnouncement();

            if (args.Contains("--debug"))
            {
                var debugForm = new DebugForm();
                    debugForm.Show();
            }

            this.Text = string.Format("Scraps - {0}", Common.Constants.Version.Full);
            this.FormClosing += MainForm_OnClosing;
        }

        private void InitializeSettings()
        {
            if (string.IsNullOrEmpty(Properties.UserConfig.Default.Cookie))
            {
                var settingsWindow = new SettingsForm(_runner ?? null);
                    settingsWindow.ShowDialog(this);
            }
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
                FileName = Path.Combine(Paths.LOGS_PATH, "Scraps.${date:format=yyyy-MM}.log"),
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
            _runner.OnProfileNotSetUp += OnProfileNotSetUp;
            _runner.OnRafflesWon += OnRafflesWon;
            _runner.OnStopping += OnStopping;
            _runner.OnStopped += OnStopped;
        }

        private void CheckForAnnouncement()
        {
            var ann = new AnnouncementService();
            if (ann.GetAnnouncement().Result is string announcement && announcement != null)
            {
                announcement = "[Announcement] " + announcement;
                _LogWindow.AppendLine(announcement, System.Drawing.Color.GreenYellow);
            }
        }

        private void ResetStatus() => _Status.Text = " "; // Set text to a space rather than null/empty so the status strip doesn't collapse

        #region Raffle Runner Event Subscriptions
        private void OnStatus(object sender, StatusArgs e) => _Status.Text = e.Message;

        private void OnStarting(object sender, StartingArgs e)
        {
            _StartStopButton.Enabled = false;
            _StartStopButton.Text = "Starting...";
        }

        private void OnRunning(object sender, RunningArgs e)
        {
            _running = true;
            _StartStopButton.Image = Icons.Stop;
            _StartStopButton.Enabled = true;
            _StartStopButton.Text = "Stop";
        }

        private void OnAccountBanned(object sender, AccountBannedArgs e) => _runner.Cancel();

        private void OnProfileNotSetUp(object sender, ProfileNotSetUpArgs e) => _runner.Cancel();

        private void OnRafflesWon(object sender, RafflesWonArgs e)
        {
            bool enableToast = Properties.UserConfig.Default.ToastNotifications;
            if (enableToast)
            {
                string logo = Files.LOGO_FILE;
                if (!File.Exists(logo))
                {
                    using (var http = new HttpClient())
                    {
                        string url = string.Format("https://scrap.tf/apple-touch-icon.png?{0}", Guid.NewGuid());
                        byte[] data = http.GetByteArrayAsync(url).Result;
                        File.WriteAllBytes(logo, data);
                    }
                }

                string message = e.Message;
                var viewButton = new ToastButton();
                    viewButton.AddArgument("action", "viewRafflesWonPage");
                    viewButton.SetContent("View Won Raffles");

                var dismissButton = new ToastButton();
                    dismissButton.SetContent("Dismiss");
                    dismissButton.SetDismissActivation();

                var toast = new ToastContentBuilder();
                    toast.AddAppLogoOverride(new Uri(logo), ToastGenericAppLogoCrop.Circle, null, false);
                    toast.AddAttributionText(string.Format("Scraps {0}", Common.Constants.Version.Full));
                    toast.AddText("Items Need Withdrawing");
                    toast.AddText(message);
                    toast.AddButton(viewButton);
                    toast.AddButton(dismissButton);
                    toast.Show();
            }
        }

        private void OnStopping(object sender, StoppingArgs e)
        {
            _StartStopButton.Enabled = false;
            _StartStopButton.Text = "Stopping...";
        }

        private void OnStopped(object sender, StoppedArgs e)
        {
            _running = false;
            _StartStopButton.Image = Icons.Start;
            _StartStopButton.Enabled = true;
            _StartStopButton.Text = "Start";
            ResetStatus();

            if (_exitOnCancel)
            {
                Application.Exit();
            }
        }
        #endregion

        #region Control Event Subscriptions
        private void MainForm_OnClosing(object sender, FormClosingEventArgs e)
        {
            // Finish any log writing that needs to be done.
            LogManager.Flush();

            ToastNotificationManagerCompat.OnActivated -= Toast_OnActivated;
        }

        private async void StartStopButton_OnClick(object sender, EventArgs e)
        {
            if (Properties.UserConfig.Default.Cookie.IsNullOrEmpty())
            {
                var settingsWindow = new SettingsForm(null);
                    settingsWindow.ShowDialog(this);

                return;
            }

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
                catch
                {
                    ResetStatus();

                    _runner.Cancel();

                    _StartStopButton.Enabled = true;
                    _StartStopButton.Text = "Start";
                    _StartStopButton.Image = Icons.Start;
                }
            }
        }

        private void WonRafflesButton_OnClick(object sender, EventArgs e) => ShowWebViewWindow("https://scrap.tf/raffles/won", $"scr_session={Properties.UserConfig.Default.Cookie}");

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

        private void ShowWebViewWindow(string url, string cookies = null) => (new WebViewForm(url, cookies)).Show();

        private void Toast_OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            var parsed = ToastArguments.Parse(e.Argument);
            switch (parsed["action"])
            {
                case "viewRafflesWonPage":
                    Process.Start("explorer.exe", "https://scrap.tf/raffles/won");
                    break;
            }
        }
    }
}
