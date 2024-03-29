﻿#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2021 Caprine Logic
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

using Scraps.Events;
using Scraps.Services;
using Scraps.Resources;
using Scraps.Extensions;

namespace Scraps.Controls;

public partial class MainControl : UserControl
{
    private const string StartText    = "Start";
    private const string StartingText = "Starting";
    private const string StopText     = "Stop";
    private const string StoppingText = "Stopping";
    private const string ErrorText    = "Error";

    private readonly AnnouncementService _announcement;
    private readonly RaffleService       _runner;
    private readonly SettingsService     _settings;
    private readonly Image               _startIcon = Images.control;
    private readonly Image               _stopIcon  = Images.control_stop_square;
    private readonly Image               _errorIcon = Images.exclamation__frame;

    private enum RunnerButtonState
    {
        Starting,
        Started,
        Stopping,
        Stopped,
        Error
    }

    private enum AlertState
    {
        Default,
        InvalidCookie,
        WonRaffle,
        AccountBanned
    }

    public MainControl(AnnouncementService announcement, RaffleService runner, SettingsService settings)
    {
        _announcement = announcement;
        _runner       = runner;
        _settings     = settings;
        
        _announcement.OnAnnouncementReceived += AnnouncementOnAnnouncementReceived;

        _runner.OnStarting            += RaffleOnStarting;
        _runner.OnStarted             += RaffleOnStarted;
        _runner.OnStopping            += RaffleOnStopping;
        _runner.OnStopped             += RaffleOnStopped;
        _runner.OnWithdrawalAvailable += RunnerOnWithdrawalAvailable;

        _settings.OnSaved += SettingsOnSaved;
        _settings.OnReset += SettingsOnReset;

        InitializeComponent();
        AddRtbTarget();

        _RunnerButton.Click += RunnerButtonOnClick;

        ValidateCookie(_settings.GetString("Cookie"));
    }

    private void AddRtbTarget()
    {
        var debug  = Args.Has("debug");
        var config = LogManager.Configuration;
        var target = new RtbTarget(_MainViewLog)
        {
            Layout = debug ? "[${logger:shortName=true}] ${message}${exception}" : "${message}${exception}"
        };
        
        config.AddTarget("RTB", target);
        config.LoggingRules.Add(new LoggingRule("*", debug ? LogLevel.Debug : LogLevel.Info, target));
        
        LogManager.ReconfigExistingLoggers();
    }

    private void SetRunnerButtonState(RunnerButtonState state, bool enable)
    {
        switch (state)
        {
            case RunnerButtonState.Starting:
                _RunnerButton.Text  = StartingText;
                _RunnerButton.Image = _startIcon;
                break;
            case RunnerButtonState.Started:
                _RunnerButton.Text  = StopText;
                _RunnerButton.Image = _stopIcon;
                break;
            case RunnerButtonState.Stopping:
                _RunnerButton.Text  = StoppingText;
                _RunnerButton.Image = _stopIcon;
                break;
            case RunnerButtonState.Stopped:
                _RunnerButton.Text  = StartText;
                _RunnerButton.Image = _startIcon;
                break;
            case RunnerButtonState.Error:
                _RunnerButton.Text  = ErrorText;
                _RunnerButton.Image = _errorIcon;
                break;
        }

        _RunnerButton.Enabled = enable;
    }

    private void SetAlert(AlertState state = AlertState.Default, string message = null)
    {
        switch (state)
        {
            case AlertState.InvalidCookie:
                _AlertLabel.Text      = message ?? "Invalid or missing cookie";
                _AlertLabel.ForeColor = Color.Crimson;
                break;
            case AlertState.AccountBanned:
                _AlertLabel.Text      = message ?? "Account banned";
                _AlertLabel.ForeColor = Color.Crimson;
                break;
            case AlertState.WonRaffle:
                _AlertLabel.Text      = message ?? "You've won raffles that need withdrawing";
                _AlertLabel.ForeColor = Color.SeaGreen;
                break;
            default:
            case AlertState.Default:
                _AlertLabel.Text = "";
                break;
        }
    }

    private void ValidateCookie(string cookie)
    {
        if (!_runner.Running)
        {
            if (Utils.IsValidCookie(cookie))
            {
                SetRunnerButtonState(RunnerButtonState.Stopped, true);
                SetAlert();
            }
            else
            {
                SetRunnerButtonState(RunnerButtonState.Error, false);
                SetAlert(AlertState.InvalidCookie);
            }
        }
    }
    
    #region Service Event Subscriptions
    
    private void AnnouncementOnAnnouncementReceived(object sender, AnnouncementServiceAnnouncementReceivedArgs e)
        => _MainViewLog.AppendLine($"[Announcement] {e.Announcement}", ColorTranslator.FromHtml("#eab308"));
    
    private void RaffleOnStarting(object sender, RaffleServiceStartingArgs e)
        => SetRunnerButtonState(RunnerButtonState.Starting, false);

    private void RaffleOnStarted(object sender, RaffleServiceStartedArgs e)
    {
        SetAlert();
        SetRunnerButtonState(RunnerButtonState.Started, true);
    }

    private void RaffleOnStopping(object sender, RaffleServiceStoppingArgs e)
        => SetRunnerButtonState(RunnerButtonState.Stopping, false);

    private void RaffleOnStopped(object sender, RaffleServiceStoppedArgs e)
    {
        SetAlert();
        SetRunnerButtonState(RunnerButtonState.Stopped, true);
    }
    
    private void RunnerOnWithdrawalAvailable(object sender, RaffleServiceWithdrawalAvailableArgs e)
        => SetAlert(AlertState.WonRaffle, e.Message);

    private void SettingsOnSaved(object sender, SettingsServiceSavedArgs e)
        => ValidateCookie(_settings.GetString("Cookie"));
    
    private void SettingsOnReset(object sender, SettingsServiceResetArgs e)
        => ValidateCookie(_settings.GetString("Cookie"));

    #endregion
    
    #region Control Event Subscriptions

    private async void MainControl_Load(object sender, EventArgs e)
    {
        if (_settings.GetBool("FetchAnnouncements"))
        {
            await _announcement.FetchAnnouncementsAsync();
        }
    }

    private async void _MainViewLog_LinkClicked(object sender, LinkClickedEventArgs e) => await Utils.OpenUrl(e.LinkText);
    
    private void _MainViewLog_TextChanged(object sender, EventArgs e) => _ClearButton.Enabled = _MainViewLog.Text.Length > 0;

    private async void RunnerButtonOnClick(object sender, EventArgs e)
    {
        if (_runner.Running)
        {
            _runner.Stop();
        }
        else
        {
            await _runner.StartAsync();
        }
    }

    private void _ClearButton_Click(object sender, EventArgs e) => _MainViewLog.Clear();
    #endregion
}
