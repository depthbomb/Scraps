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

using Microsoft.Extensions.DependencyInjection;

using Scraps.Next.Forms;
using Scraps.Next.Controls;
using Scraps.Next.Services;
using Scraps.Next.Resources;

namespace Scraps.Next;

internal static class Bootstrapper
{
    public static           IServiceProvider Services { get; private set; }
    
    private static readonly Mutex Mutex = new(false, GlobalShared.MutexName);

    [STAThread]
    private static void Main(string[] args)
    {
        if (!Mutex.WaitOne(0))
        {
            var intPtr = Native.FindWindow(default, GlobalShared.WindowTitle);
            if (intPtr != IntPtr.Zero)
            {
                Native.SendMessage(intPtr, GlobalShared.WM_SCRAPSNEXT_SHOWME, default, IntPtr.Zero);
            }
            
            Application.Exit();
            return;
        }
        
        #if DEBUG
        Native.AllocConsole();
        #endif

        InitializeLogger();

        var log = LogManager.GetCurrentClassLogger();
        
        log.Debug("Startup");
        
        InitializeServices();

        var settings = Services.GetRequiredService<SettingsService>();
            settings.TryUpgrade();

        if (!settings.Get<bool>("SeenInitialDisclaimer"))
        {
            log.Debug("Showing first-time warning disclaimer");
            
            ShowDisclaimer();
            settings.Set("SeenInitialDisclaimer", true)
                    .Save(false);
        }

        Services.GetRequiredService<WebViewService>().TryInstallRuntimeAsync().Wait();
        
        Services.GetRequiredService<FlagsService>()
                .RegisterFlag("debug", "d")
                .ParseFlags(args);

        if (settings.Get<bool>("CheckUpdates"))
        {
            Services.GetRequiredService<UpdateService>()
                    .CheckUntilUpdateAvailable();
        }

        ApplicationConfiguration.Initialize();
        
        var mainForm = Services.GetRequiredService<MainForm>();
            mainForm.TopMost     =  settings.Get<bool>("AlwaysOnTop");
            mainForm.FormClosing += MainWindowOnFormClosing;
        
            
        log.Debug("Displaying MainForm");
        
        Application.Run(mainForm);
    }

    private static void InitializeLogger()
    {
        var config = new LoggingConfiguration();
        var fileTarget = new FileTarget
        {
            Layout                       = @"${longdate} │ ${pad:padding=-5:inner=${level:uppercase=true}} │ ${pad:padding=43:inner=${callsite:className=True:includeNamespace=False:includeSourcePath=False}} │ ${message}${exception}",
            ArchiveEvery                 = FileArchivePeriod.Month,
            ArchiveFileName              = "backup.{#}.zip",
            ArchiveNumbering             = ArchiveNumberingMode.Date,
            ArchiveDateFormat            = "yyyyMMddHHmm",
            EnableArchiveFileCompression = true,
            FileName                     = Path.Combine(GlobalShared.LogsPath, "Scraps.${date:format=yyyy-MM}.log"),
            CreateDirs                   = true,
            MaxArchiveFiles              = 3,
        };

        config.AddTarget("File", fileTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

        LogManager.Configuration = config;
    }

    private static void InitializeServices()
    {
        Services = new ServiceCollection()
                   // Forms
                   .AddSingleton<MainForm>()
                   // Controls
                   .AddSingleton<MainControl>()
                   .AddSingleton<WonRafflesControl>()
                   .AddSingleton<SettingsControl>()
                   .AddSingleton<AboutControl>()
                   // Services
                   .AddSingleton<FlagsService>()
                   .AddSingleton<SettingsService>()
                   .AddSingleton<RaffleService>()
                   .AddSingleton<UpdateService>()
                   .AddSingleton<WebViewService>()
                   .AddSingleton<AnnouncementService>()
                   .BuildServiceProvider();
    }

    private static void ShowDisclaimer() => Utils.ShowWarning(null, "Disclaimer", Strings.DisclaimerText);

    private static void MainWindowOnFormClosing(object sender, FormClosingEventArgs e)
    {
        #if DEBUG
        Native.FreeConsole();
        #endif
    }
}