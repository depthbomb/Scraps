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
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Serilog.Core;

using Scraps.Models;
using Scraps.Events;
using Scraps.Extensions;

namespace Scraps
{
    public class Bot
    {
        private Logger _logger;
        private Config _config;
        private HttpClient _http;
        private BotManager _manager;

        public Bot(Logger logger, Config config, HttpClient http)
        {
            _logger = logger;
            _config = config;
            _http = http;
            _manager = new BotManager(_config, _http);
        }

        public async Task Run()
        {
            Console.CursorVisible = false;

            _manager.OnLogger += OnLogger;
            _manager.OnStatusUpdate += OnStatusUpdate;
            _manager.OnAccountBanned += OnAccountBanned;
            _manager.OnCsrfTokenObtained += OnCsrfTokenObtained;

            _manager.OnRafflesWon += OnRafflesWon;

            try
            {
                await _manager.StartLoop();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex.Message);
            }
        }

        private void OnLogger(object sender, LoggerArgs e) => _logger.Write(e.Level, e.Template, e.Properties);

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

        private void OnAccountBanned(object sender, AccountBannedArgs e)
        {
            _manager.Stop();
        }

        private void OnCsrfTokenObtained(object sender, CsrfTokenObtainedArgs e) => _logger.Debug("Obtained CSRF Token ({Token}}", e.CsrfToken);

        private void OnRafflesWon(object sender, RafflesWonArgs e)
        {
            List<string> wonRaffles = e.RaffleIds;
            int numWonRaffles = wonRaffles.Count;

            _logger.Information("You've won {Number} " + "raffle".Pluralize(numWonRaffles) + " that " + "needs".Pluralize(numWonRaffles, "need") + " to be withdrawn!", numWonRaffles);

            //if (_config.EnableToastNotifications && Platform.OS == "Win32NT")
            //{
            //    if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated()) return;

            //    string appLogoOverride = Path.GetTempFileName();
            //    byte[] data = _http.GetByteArrayAsync("https://scrap.tf/apple-touch-icon.png?" + Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            //    File.WriteAllBytes(appLogoOverride, data);

            //    var viewButton = new ToastButton();
            //        viewButton.AddArgument("action", "viewRafflesWonPage");
            //        viewButton.SetContent("View Won Raffles");

            //    var dismissButton = new ToastButton();
            //        dismissButton.SetContent("Dismiss");
            //        dismissButton.SetDismissActivation();

            //    var toast = new ToastContentBuilder();
            //        toast.AddAppLogoOverride(new Uri(appLogoOverride), ToastGenericAppLogoCrop.Circle, null, false);
            //        toast.AddAttributionText(string.Format("Scraps {0}", Constants.Version.Full));
            //        toast.AddText("Items Need Withdrawing");
            //        toast.AddText(string.Format("You've won {0} {1} that {2} to be withdrawn!", wonRaffles, "raffle".Pluralize(numWonRaffles), "needs".Pluralize(numWonRaffles, "need")));
            //        toast.AddButton(viewButton);
            //        toast.AddButton(dismissButton);
            //        toast.Show();

            //    ToastNotificationManagerCompat.OnActivated += (args) =>
            //    {
            //        var parsed = ToastArguments.Parse(args.Argument);
            //        switch (parsed["action"])
            //        {
            //            case "viewRafflesWonPage":
            //                Process.Start("explorer.exe", "https://scrap.tf/raffles/won");
            //                break;
            //        }
            //    };
            //}
        }
    }
}
