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
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Notifications;

using NLog;

using Scraps.Models;
using Scraps.Events;
using Scraps.Constants;
using Scraps.Extensions;
using Scraps.Abstractions;

namespace Scraps
{
    public class Bot
    {
        private Logger _log;
        private Config _config;
        private HttpClient _http;
        private RaffleRunner _runner;

        public static DateTime StartTime;

        private bool _terminateImmediately = false;

        public Bot(Config config, HttpClient http)
        {
            _log = LogManager.GetCurrentClassLogger();
            _config = config;
            _http = http;
            _runner = new RaffleRunner(_config, _http);
        }

        public async Task RunAsync()
        {
            _runner.OnStatusUpdate += OnStatusUpdate;
            _runner.OnCsrfTokenObtainedArgs += OnCsrfTokenObtainedArgs;
            _runner.OnAccountBanned += OnAccountBanned;

            _runner.OnRafflesWon += OnRafflesWon;
            _runner.OnRaffleJoined += OnRaffleJoined;

            Console.CancelKeyPress += OnCancelKeyPress;

            try
            {
                await _runner.StartLoopAsync();
            }
            catch (OperationCanceledException ex)
            {
                _log.Info("Bot successfully stopped");
                _log.Trace(ex);

                Console.WriteLine();
                Console.WriteLine("Press [Enter] to exit.");
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message);
            }

            Console.ReadLine();
        }

        public async Task LoadPluginsAsync()
        {
            if (!Directory.Exists(Paths.PluginsPath))
            {
                Directory.CreateDirectory(Paths.PluginsPath);

                _log.Debug("Created missing plugins folder {Path}", Paths.PluginsPath);
            }
            else
            {
                PluginManager.LoadPlugins();

                var plugins = PluginManager.Assemblies;

                foreach (var plugin in plugins)
                {
                    try
                    {
                        var types = plugin.GetTypes();
                        var type = types[0];
                        var loadedPlugin = (PluginBase)Activator.CreateInstance(type, _config, _http, _runner);
                            loadedPlugin.Initialize();

                        _log.Info("Initialized plugin {Plugin}", type.Name);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Failed to load plugin {Plugin}: {Exception}", plugin.FullName, ex.Message);
                    }
                }
            }

            StartTime = DateTime.UtcNow;

            await Task.CompletedTask;
        }

        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (!_terminateImmediately)
            {
                e.Cancel = true;

                _terminateImmediately = true;
                _runner.Stop();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void OnStatusUpdate(object sender, StatusUpdateArgs e)
        {
            string appTitle = string.Format("Scraps - {0}", Constants.Version.Full);
            int rafflesJoined = e.RafflesJoined;
            string status = e.Status;
            string title = string.Format("{0} — {1} {2} joined this session — {3}",
                appTitle,
                rafflesJoined,
                "Raffle".Pluralize(rafflesJoined),
                status
            );

            Console.Title = title;
        }

        private void OnCsrfTokenObtainedArgs(object sender, CsrfTokenObtainedArgs e) => _log.Info("Received CSRF token");

        private void OnAccountBanned(object sender, AccountBannedArgs e)
        {
            Console.Title = "R.I.P.";
            _log.Fatal("Your account has been banned. You will need to use a different account session cookie to continue using Scraps.");
            _log.Fatal("Press [Enter] to exit.");
            Console.CursorVisible = true;
            Console.ReadLine();
            Environment.Exit(0);
        }

        private void OnRafflesWon(object sender, RafflesWonArgs e)
        {
            List<string> wonRaffles = e.RaffleIds;
            int numWonRaffles = wonRaffles.Count;

            _log.Info("You've won {Number} " + "raffle".Pluralize(numWonRaffles) + " that " + "needs".Pluralize(numWonRaffles, "need") + " to be withdrawn!", numWonRaffles);

            if (_config.EnableToastNotification && Platform.OS == "Win32NT")
            {
                if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated()) return;

                string appLogoOverride = Path.GetTempFileName();
                byte[] data = _http.GetByteArrayAsync("https://scrap.tf/apple-touch-icon.png?" + Guid.NewGuid().ToString()).GetAwaiter().GetResult();
                File.WriteAllBytes(appLogoOverride, data);

                var viewButton = new ToastButton();
                    viewButton.AddArgument("action", "viewRafflesWonPage");
                    viewButton.SetContent("View Won Raffles");

                var dismissButton = new ToastButton();
                    dismissButton.SetContent("Dismiss");
                    dismissButton.SetDismissActivation();

                var toast = new ToastContentBuilder();
                    toast.AddAppLogoOverride(new Uri(appLogoOverride), ToastGenericAppLogoCrop.Circle, null, false);
                    toast.AddAttributionText(string.Format("Scraps {0}", Constants.Version.Full));
                    toast.AddText("Items Need Withdrawing");
                    toast.AddText(string.Format("You've won {0} {1} that {2} to be withdrawn!", numWonRaffles, "raffle".Pluralize(numWonRaffles), "needs".Pluralize(numWonRaffles, "need")));
                    toast.AddButton(viewButton);
                    toast.AddButton(dismissButton);
                    toast.Show();

                ToastNotificationManagerCompat.OnActivated += (args) =>
                {
                    var parsed = ToastArguments.Parse(args.Argument);
                    switch (parsed["action"])
                    {
                        case "viewRafflesWonPage":
                            Process.Start("explorer.exe", "https://scrap.tf/raffles/won");
                            break;
                    }
                };
            }
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e) => _log.Info("[{Entered}/{Total}] Joined raffle {Id}", e.Entered, e.Total, e.RaffleId);
    }
}
