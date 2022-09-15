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

using Windows.Win32.Foundation;

using Scraps.Events;
using Scraps.Controls;
using Scraps.Services;
using Scraps.Resources;

namespace Scraps.Forms;

public sealed partial class MainForm : Form
{
    private CancellationTokenSource _secondaryStatusCancelTokenSource = new();

    private readonly SettingsService _settings;
    private readonly Logger          _log = LogManager.GetCurrentClassLogger();

    public MainForm(
        MainControl mainControl,
        WonRafflesControl wonRaffles,
        SettingsControl settingsControl,
        AboutControl aboutControl,
        RaffleService runner,
        SettingsService settings,
        UpdateService update
    )
    {
        _settings = settings;
        
        runner.OnStatus          += RunnerOnStatus;
        _settings.OnSaved        += SettingsOnSaved;
        update.OnCheckingUpdates += UpdateOnCheckingUpdates;

        InitializeComponent();

        Text        = GlobalShared.WindowTitle;
        MinimumSize = new Size(900, 525);
        
        _MainWindowTabs.SizeMode = TabSizeMode.Fixed;
        _MainWindowTabs.ImageList = new ImageList
        {
            Images =
            {
                Images.terminal,
                Images.trophy,
                Images.gear,
                Images.information,
                Images.radar,
            }
        };

        AddControlToTab(mainControl, "Runner", 0);
        AddControlToTab(wonRaffles, "Won Raffles", 1);
        AddControlToTab(settingsControl, "Settings", 2);
        AddControlToTab(aboutControl, "About", 3);

        if (Args.Has("debug"))
        {
            var debugControl = new DebugControl(settings);
            AddControlToTab(debugControl, "Debug", 4);
        }
    }

    private void AddControlToTab(UserControl control, string name, int imageIndex)
    {
        control.Dock = DockStyle.Fill;

        var tab = new TabPage(name);
            tab.ImageIndex = imageIndex;
            tab.Controls.Add(control);

        _MainWindowTabs.TabPages.Add(tab);

        _log.Debug("Added control {Name} to tabs with image index {Index}", name, imageIndex);
    }

    private void RunnerOnStatus(object sender, RaffleServiceStatusArgs e)
        => _MainWindowStatus.Text = e.Message;
    
    private void SettingsOnSaved(object sender, SettingsServiceSavedArgs e)
    {
        TopMost = _settings.GetBool("AlwaysOnTop");
        
        ShowSecondaryStatus("Settings saved!");
    }

    private void UpdateOnCheckingUpdates(object sender, UpdateServiceCheckingUpdatesArgs e)
        => ShowSecondaryStatus("Checking for updates...", 500);

    private async void ShowSecondaryStatus(string message, int autoClearDelay = 1_500)
    {
        try
        {
            if (_MainWindowSecondaryStatus.Text != null)
            {
                _secondaryStatusCancelTokenSource.Cancel();
                _secondaryStatusCancelTokenSource = new CancellationTokenSource();
            }

            var cancelToken = _secondaryStatusCancelTokenSource.Token;

            _MainWindowSecondaryStatus.Text = message;
            
            await Task.Delay(autoClearDelay, cancelToken);
            
            _MainWindowSecondaryStatus.Text = null;
        }
        catch
        {
            //
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == GlobalShared.WM_SCRAPSNEXT_SHOWME)
        {
            _log.Debug("Bringing to front from second instance");

            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            Show();
            Activate();
            BringToFront();
            Native.FlashWindow((HWND)Handle, true);
        }

        base.WndProc(ref m);
    }
}