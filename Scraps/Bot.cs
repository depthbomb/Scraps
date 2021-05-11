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
using System.Threading.Tasks;
using System.Collections.Generic;

using NLog;

using Scraps.Models;
using Scraps.Events;
using Scraps.Extensions;

namespace Scraps
{
    public class Bot
    {
        private readonly Logger _log;
        private readonly Config _config;
        private readonly HttpClient _http;
        private readonly RaffleRunner _runner;

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
            string appTitle = string.Format("Scraps - {0}", Common.Constants.Version.Full);
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
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e) => _log.Info("[{Entered}/{Total}] Joined raffle {Id}", e.Entered, e.Total, e.RaffleId);
    }
}
