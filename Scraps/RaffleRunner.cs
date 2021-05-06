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
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NLog;

using HtmlAgilityPack;

using Scraps.Models;
using Scraps.Events;
using Scraps.Services;
using Scraps.Constants;
using Scraps.Extensions;

namespace Scraps
{
    public class RaffleRunner
    {
        public string CsrfToken;

        private Logger _log;
        private Config _config;
        private HttpClient _http;
        private HtmlDocument _html;

        private int _scanDelay;
        private int _joinDelay;
        private int _rafflesJoined = 0;

        private bool _alertedOfWonRaffles = false;

        private List<string> _raffleQueue = new List<string>();
        private List<string> _enteredRaffles = new List<string>();

        private CancellationToken _cancelToken;
        private CancellationTokenSource _cancelTokenSource;

        #region Events
        /// <summary>
        /// Raised when the account has been banned from Scrap.TF
        /// </summary>
        public event EventHandler<AccountBannedArgs> OnAccountBanned;

        /// <summary>
        /// Raised when the CSRF token has been parsed from the site
        /// </summary>
        public event EventHandler<CsrfTokenObtainedArgs> OnCsrfTokenObtainedArgs;

        /// <summary>
        /// Raised when the bot's displayed status is updated
        /// </summary>
        public event EventHandler<StatusUpdateArgs> OnStatusUpdate;

        /// <summary>
        /// Raised when the bot receives a response from paginating the raffle index
        /// </summary>
        public event EventHandler<PaginateArgs> OnPaginate;

        /// <summary>
        /// Raised when a raffle has been successfully joined
        /// </summary>
        public event EventHandler<RaffleJoinedArgs> OnRaffleJoined;

        /// <summary>
        /// Raised when one or more raffles have been won
        /// </summary>
        public event EventHandler<RafflesWonArgs> OnRafflesWon;
        #endregion

        public RaffleRunner(Config config, HttpClient http)
        {
            _log = LogManager.GetCurrentClassLogger();
            _config = config;
            _http = http;
            _html = new HtmlDocument();

            _cancelTokenSource = new CancellationTokenSource();
            _cancelToken = _cancelTokenSource.Token;
        }

        #region Public Methods
        /// <summary>
        /// Starts the operational loop that does all the work
        /// </summary>
        public async Task StartLoopAsync()
        {
            if (CsrfToken.IsNullOrEmpty())
            {
                await GetCsrfTokenAsync();
            }

            _scanDelay = _config.Delays.ScanDelay;
            _joinDelay = _config.Delays.JoinDelay;

            while (true)
            {
                if (_cancelToken.IsCancellationRequested) _cancelToken.ThrowIfCancellationRequested();

                SendStatus("Scanning raffles...");

                await ScanRafflesAsync();

                if (_raffleQueue.Count > 0)
                {
                    SendStatus("Joining raffles...");

                    // Set the scan delay to our config's value in case it was modified elsewhere
                    _scanDelay = _config.Delays.ScanDelay;

                    await JoinRafflesAsync();
                }
                else
                {
                    SendStatus("Waiting to scan...");

                    _log.Debug("All raffles have been entered, scanning again after {Delay} seconds", _scanDelay / 1000);

                    await Task.Delay(_scanDelay);

                    if (_config.Delays.IncrementScanDelay)
                    {
                        _scanDelay += 1000;
                    }
                }
            }
        }

        public void Stop()
        {
            _log.Info("Cancel signal received, stopping raffle runner as soon as possible. Press CTRL+C again to terminate immediately.");
            _cancelTokenSource.Cancel();
        }
        #endregion

        #region Private Methods
        #region Operations
        private async Task GetCsrfTokenAsync()
        {
            bool tokenObtained = false;
            while (!tokenObtained)
            {
                SendStatus("Obtaining CSRF token...");

                string html = await _http.GetStringAsync("https://scrap.tf");
                if (html.Contains(Strings.AccountBanned))
                {
                    OnAccountBanned?.Invoke(this, new AccountBannedArgs());
                }
                else
                {
                    var csrf = RegexPatterns.CsrfRegex.Match(html);
                    if (csrf.Success)
                    {
                        CsrfToken = csrf.Groups[1].Value;
                        OnCsrfTokenObtainedArgs?.Invoke(this, new CsrfTokenObtainedArgs(CsrfToken));

                        tokenObtained = true;
                    }
                    else
                    {
                        if (html.Contains(Strings.SiteDown))
                        {
                            _log.Error("Site appears to be down/under maintenance. Trying again after 1 minute.");

                            await Task.Delay(60 * 1000);
                        }
                        else
                        {
                            throw new Exception("Unable to retreive CSRF token. Please check your cookie value.");
                        }
                    }
                }
            }
        }

        private async Task ScanRafflesAsync()
        {
            _raffleQueue.Clear();

            _log.Debug("Scanning raffles");

            bool doneScanning = false;
            string html = await _http.GetStringAsync("https://scrap.tf/raffles");
            string lastId = string.Empty;

            while (!doneScanning)
            {
                string json = await PaginateAsync(lastId);

                if (json == null)
                {
                    _log.Error("Pagination returned an invalid response instead of JSON. Waiting 10 seconds...");
                    await Task.Delay(10_000);
                }
                else
                {
                    try
                    {
                        var paginateResponse = JsonSerializer.Deserialize<PaginateResponse>(json);
                        if (paginateResponse.success)
                        {
                            html += paginateResponse.html;
                            lastId = paginateResponse.lastid;

                            if (!paginateResponse.done)
                            {
                                _log.Debug("Scanning next page (apex = {Apex})", lastId);

                                await Task.Delay(_config.Delays.PaginateDelay);

                                continue;
                            }
                            else
                            {
                                doneScanning = true;
                            }

                            _log.Debug("Done scanning all raffles, grabbing IDs of un-entered raffles...");

                            SendStatus("Parsing scanned data...");

                            _html.LoadHtml(html);

                            var document = _html.DocumentNode;
                            var raffleElements = document.SelectNodes(Xpaths.UnenteredRaffles);
                            if (html.Contains("ScrapTF.Raffles.WithdrawRaffle"))
                            {
                                await CheckForWonRafflesAsync();
                            }
                            else
                            {
                                _alertedOfWonRaffles = false;
                            }

                            foreach (var el in raffleElements)
                            {
                                string elementHtml = el.InnerHtml.Trim();
                                string raffleId = RegexPatterns.RaffleEntryRegex.Match(elementHtml).Groups[1].Value.Trim();
                                if (
                                    !raffleId.IsNullOrEmpty() &&    // For some reason `raffleId` will sometimes give us emptiness
                                    !_raffleQueue.Contains(raffleId) &&
                                    !_enteredRaffles.Contains(raffleId)
                                )
                                {
                                    SendStatus($"Adding raffle {raffleId} to queue...");

                                    _raffleQueue.Add(raffleId);
                                }
                            }
                        }
                        else
                        {
                            if (paginateResponse.message != null)
                            {
                                if (paginateResponse.message.Contains("active site ban"))
                                {
                                    OnAccountBanned?.Invoke(this, new AccountBannedArgs());
                                }
                                else
                                {
                                    _log.Error("Encountered an error while paginating: {Message} - Waiting 10 seconds", paginateResponse.message);

                                    await Task.Delay(10_000);
                                }
                            }
                            else
                            {
                                _log.Error("Paginate response for apex {Apex} was unsuccessful", lastId.IsNullOrEmpty() ? "<empty>" : lastId);
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        _log.Error("Failed to read pagination data");
                        _log.Trace(ex);
                    }
                }
            }
        }

        private async Task<string> PaginateAsync(string apex = null)
        {
            SendStatus("Paginating...");

            string sort = _config.JoinNewestFirst ? "1" : "0";
            string url = "https://scrap.tf/ajax/raffles/Paginate";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("start", apex),
                new KeyValuePair<string, string>("sort", sort),
                new KeyValuePair<string, string>("puzzle", "0"),
                new KeyValuePair<string, string>("csrf", CsrfToken),
            });

            var response = await _http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string html = await response.Content.ReadAsStringAsync();

                OnPaginate?.Invoke(this, new PaginateArgs(apex, html));

                return html;
            }
            else
            {
                return null;
            }
        }

        private async Task CheckForWonRafflesAsync()
        {
            string url = "https://scrap.tf/raffles/won";
            string html = await _http.GetStringAsync(url);
            var raffleIds = RegexPatterns.RaffleEntryRegex.Matches(html);
            List<string> wonRaffles = new List<string>();

            foreach (Match id in raffleIds)
            {
                string raffleId = id.Groups[1].Value;
                if (!_enteredRaffles.Contains(raffleId))
                {
                    _log.Debug("Adding {Entered} to won raffles list", raffleId);

                    _enteredRaffles.Add(raffleId);

                    wonRaffles.Add(raffleId);
                }
            }

            if (!_alertedOfWonRaffles)
            {
                OnRafflesWon?.Invoke(this, new RafflesWonArgs(wonRaffles));

                _alertedOfWonRaffles = true;
            }
        }

        private async Task JoinRafflesAsync()
        {
            int entered = 0;
            int total = _raffleQueue.Count;

            foreach (string raffle in _enteredRaffles)
            {
                if (_raffleQueue.Contains(raffle))
                {
                    _raffleQueue.Remove(raffle);
                }
            }

            foreach (string raffle in _raffleQueue)
            {
                // Check if we need to cancel here as well since this is the spot that would delay the cancellation the most
                if (_cancelToken.IsCancellationRequested) _cancelToken.ThrowIfCancellationRequested();

                SendStatus($"Joining raffle {raffle}...");

                string html = await _http.GetStringAsync($"https://scrap.tf/raffles/{raffle}");
                var hash = RegexPatterns.RaffleHashRegex.Match(html);
                var limits = RegexPatterns.RaffleLimitRegex.Match(html);
                bool hasEnded = html.Contains("data-time=\"Raffle Ended\"");

                var hp = new HoneypotChecker(html);
                    hp.Check();
                if (hp.IsHoneypot)
                {
                    _log.Info("Raffle {Id} is likely a honeypot: {Reason}", raffle, hp.Reason);

                    total--;
                    _enteredRaffles.Add(raffle);

                    continue;
                }

                if (hasEnded)
                {
                    _log.Info("Raffle {Id} has ended", raffle);

                    total--;
                    _enteredRaffles.Add(raffle);

                    continue;
                }

                if (limits.Success)
                {
                    int num = int.Parse(limits.Groups[1].Value);
                    int max = int.Parse(limits.Groups[2].Value);

                    if (_config.Paranoid && num < 2)
                    {
                        _log.Info("Raffle {Id} has too few entries", raffle);

                        total--;

                        continue;
                    }

                    if (num >= max)
                    {
                        _log.Info("Raffle {Id} is full ({Num}/{Max})", raffle, num, max);

                        total--;
                        _enteredRaffles.Add(raffle);

                        continue;
                    }
                }

                if (hash.Success)
                {
                    string url = "https://scrap.tf/ajax/viewraffle/EnterRaffle";
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("raffle", raffle),
                        new KeyValuePair<string, string>("captcha", ""),
                        new KeyValuePair<string, string>("hash", hash.Groups[1].Value),
                        new KeyValuePair<string, string>("flag", ""),
                        new KeyValuePair<string, string>("csrf", CsrfToken),
                    });

                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                        httpRequest.Content = content;
                        httpRequest.Headers.Referrer = new Uri($"https://scrap.tf/raffles/{raffle}");
                    var response = await _http.SendAsync(httpRequest);

                    string json = await response.Content.ReadAsStringAsync();

                    var joinRaffleResponse = JsonSerializer.Deserialize<JoinRaffleResponse>(json);
                    if (joinRaffleResponse.success)
                    {
                        entered++;

                        OnRaffleJoined?.Invoke(this, new RaffleJoinedArgs(raffle, html, entered, total));

                        _enteredRaffles.Add(raffle);
                        _rafflesJoined++;
                    }
                    else
                    {
                        _log.Error("Unable to join raffle {Id}: {Message}", raffle, joinRaffleResponse.message);
                    }

                    SendStatus("Waiting...");

                    await Task.Delay(_joinDelay);
                }
                else
                {
                    _log.Error("Could not obtain hash from raffle {Id}", raffle);
                }
            }

            if (entered > 0)
            {
                _log.Info("Finished raffle queue");
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Shortcut method to raise a <see cref="OnStatusUpdate"/> event
        /// </summary>
        /// <param name="message">Message to send with the event</param>
        private void SendStatus(string message) => OnStatusUpdate?.Invoke(this, new StatusUpdateArgs(_rafflesJoined, message));
        #endregion
        #endregion
    }
}
