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

using Microsoft.Toolkit.Uwp.Notifications;
using Scraps.GUI.Models;
using Scraps.GUI.Logging;
using Scraps.GUI.Services;
using Scraps.GUI.Constants;
using Scraps.GUI.Extensions;
using Scraps.GUI.Services.Raffle;

namespace Scraps.GUI.Forms;

public partial class MainForm : Form
{
    private bool _running;

    private readonly LaunchOptions _options;
    private readonly RaffleService _runner;

    public MainForm(LaunchOptions options)
    {
        _options = options;

        InitializeComponent();

        InitializeLogger();
        InitializeSettings();

        _runner              =  new RaffleService(_options);
        _runner.OnStatus     += OnStatus;
        _runner.OnStarting   += OnStarting;
        _runner.OnRunning    += OnRunning;
        _runner.OnRafflesWon += OnRafflesWon;
        _runner.OnStopped    += OnStopped;

        Text        =  $"Scraps - {Constants.Version.Full}";
        FormClosing += MainForm_OnClosing;

        ToastNotificationManagerCompat.OnActivated += Toast_OnActivated;
    }

    private void InitializeSettings()
    {
        if (string.IsNullOrEmpty(Properties.UserConfig.Default.Cookie))
        {
            var settingsWindow = new SettingsForm(_runner);
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
            Layout                       = @"${longdate} | ${pad:padding=5:inner=${level:uppercase=true}} | [${logger:shortName=true}] ${message}${exception}",
            ArchiveEvery                 = FileArchivePeriod.Month,
            ArchiveFileName              = "backup.{#}.zip",
            ArchiveNumbering             = ArchiveNumberingMode.Date,
            ArchiveDateFormat            = "yyyyMMddHHmm",
            EnableArchiveFileCompression = true,
            FileName                     = Path.Combine(Paths.LOGS_PATH, "Scraps.${date:format=yyyy-MM}.log"),
            CreateDirs                   = true,
            MaxArchiveFiles              = 5,
        };

        config.AddTarget("RTB", rtbTarget);
        config.AddTarget("File", fileTarget);
        config.LoggingRules.Add(new LoggingRule("*", _options.Debug ? LogLevel.Debug : LogLevel.Info, rtbTarget));
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

        LogManager.Configuration = config;
    }

    private void ResetStatus()
        => _Status.Text = " "; // Set text to a space rather than null/empty so the status strip doesn't collapse

    #region Raffle Runner Event Subscriptions

    private void OnStatus(object sender, StatusArgs e) => _Status.Text = e.Message;

    private void OnStarting(object sender, StartingArgs e)
    {
        _StartStopButton.Enabled = false;
        _StartStopButton.Text    = "Starting...";
    }

    private void OnRunning(object sender, RunningArgs e)
    {
        _running                 = true;
        _StartStopButton.Image   = Icons.Stop;
        _StartStopButton.Enabled = true;
        _StartStopButton.Text    = "Stop";
    }

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
                    string url  = string.Format("https://scrap.tf/apple-touch-icon.png?{0}", Guid.NewGuid());
                    byte[] data = http.GetByteArrayAsync(url).Result;
                    File.WriteAllBytes(logo, data);
                }
            }

            string message    = e.Message;
            var    viewButton = new ToastButton();
            viewButton.AddArgument("action", "viewRafflesWonPage");
            viewButton.SetContent("View Won Raffles");

            var dismissButton = new ToastButton();
            dismissButton.SetContent("Dismiss");
            dismissButton.SetDismissActivation();

            var toast = new ToastContentBuilder();
            toast.AddAppLogoOverride(new Uri(logo), ToastGenericAppLogoCrop.Circle, null, false);
            toast.AddAttributionText(string.Format("Scraps {0}", Constants.Version.Full));
            toast.AddText("Items Need Withdrawing");
            toast.AddText(message);
            toast.AddButton(viewButton);
            toast.AddButton(dismissButton);
            toast.Show();
        }
    }

    private void OnStopped(object sender, StoppedArgs e)
    {
        _running                 = false;
        _StartStopButton.Image   = Icons.Start;
        _StartStopButton.Enabled = true;
        _StartStopButton.Text    = "Start";

        ResetStatus();
    }

    #endregion

    #region Control Event Subscriptions

    private async void MainForm_OnShown(object sender, EventArgs e)
    {
        if (await IsOnline())
        {
            try
            {
                if (!_options.SkipAnnouncements)
                {
                    _Status.Text = "Fetching announcements";

                    await FetchAnnouncementsAsync();
                }

                _StartStopButton.Enabled = true;

                if (_options.Silent)
                {
                    this.WindowState = FormWindowState.Minimized;
                    await _runner.StartAsync();
                }
            }
            catch { }

            ResetStatus();
        }
        else
        {
            Utils.ShowError(this, "No Internet Connection", "Could not connect to the internet. Please check your internet connection.\n\nIf you believe this is a mistake then please open up an issue on GitHub.");

            Application.Exit();
        }
    }

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
                _StartStopButton.Text    = "Start";
                _StartStopButton.Image   = Icons.Start;
            }
        }
    }

    private void WonRafflesButton_OnClick(object sender, EventArgs e)
        => Process.Start("explorer.exe", "https://scrap.tf/raffles/won");

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

    private void Toast_OnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        var parsed = ToastArguments.Parse(e.Argument);
        switch (parsed["action"])
        {
            case"viewRafflesWonPage":
                Process.Start("explorer.exe", "https://scrap.tf/raffles/won");
                break;
        }
    }

    private async Task FetchAnnouncementsAsync()
    {
        using (var ann = new AnnouncementService())
        {
            if (await ann.GetAnnouncementAsync() is string announcement && !string.IsNullOrEmpty(announcement))
            {
                string[] announcementLines = announcement.Split('\n');
                for (int i = 0; i < announcementLines.Length; i++)
                {
                    string line = announcementLines[i].Trim();
                    if (!string.IsNullOrEmpty(line))
                    {
                        _LogWindow.AppendLine($"[Announcement #{i + 1}] {line}", Color.FromArgb(251, 191, 36));
                    }
                }
            }
        }
    }

    private async Task<bool> IsOnline()
    {
        _Status.Text = "Checking connectivity";
        using (var client = new HttpClient())
        {
            try
            {
                await client.GetAsync("https://google.com/204_response");

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == Native.WM_RAFFLERUNNER_SHOWME)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            Show();
            Activate();
            BringToFront();
        }

        base.WndProc(ref m);
    }
}