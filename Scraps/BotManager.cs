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
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Serilog.Events;

using HtmlAgilityPack;

using Scraps.Models;
using Scraps.Events;
using Scraps.Services;
using Scraps.Constants;
using Scraps.Extensions;

namespace Scraps
{
    public class BotManager
    {
        private Config _config;
        private HttpClient _http;
        private Random _rng;
        private HtmlDocument _html;

        private string _csrfToken;

        private int _scanDelay;
        private int _rafflesJoined = 0;

        private bool _running = false;
        private bool _alertedOfWonRaffles = false;

        private List<string> _wonRaffles = new List<string>();
        private List<string> _raffleQueue = new List<string>();
        private List<string> _enteredRaffles = new List<string>();

        #region Events
        public event EventHandler<LoggerArgs> OnLogger;

        /// <summary>
        /// Raised when the account has been banned from Scrap.TF
        /// </summary>
        public event EventHandler<AccountBannedArgs> OnAccountBanned;

        /// <summary>
        /// Raised when the website CSRF token has been obtained
        /// </summary>
        public event EventHandler<CsrfTokenObtainedArgs> OnCsrfTokenObtained;

        /// <summary>
        /// Raised when the bot's displayed status is updated
        /// </summary>
        public event EventHandler<StatusUpdateArgs> OnStatusUpdate;

        /// <summary>
        /// Raised when one or more raffles have been won
        /// </summary>
        public event EventHandler<RafflesWonArgs> OnRafflesWon;
        #endregion

        public BotManager(Config config, HttpClient http)
        {
            _config = config;
            _http = http;
            _rng = new Random();
            _html = new HtmlDocument();
        }

        #region Public Methods
        /// <summary>
        /// Starts the operational loop that does all the work
        /// </summary>
        public async Task StartLoop()
        {
            if (_csrfToken.IsNullOrEmpty())
            {
                await GetCsrfToken();
            }

            _scanDelay = _config.Delays.ScanDelay;
            _running = true;

            while (_running)
            {
                SendStatus("Scanning raffles...");

                await ScanRaffles();

                if (_raffleQueue.Count > 0)
                {
                    SendStatus("Joining raffles...");

                    // Set the scan delay to our config's value in case it was modified elsewhere
                    _scanDelay = _config.Delays.ScanDelay;

                    await JoinRaffles();
                }
                else
                {
                    SendStatus("Waiting to scan...");

                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "All raffles have been entered, scanning again after {Delay} seconds", _scanDelay / 1000));

                    await Task.Delay(_scanDelay);

                    if (_config.Delays.IncrementScanDelay)
                    {
                        _scanDelay += 1000;
                    }
                }
            }
        }

        /// <summary>
        /// Stops the loop
        /// </summary>
        public void Stop()
        {
            _running = false;
        }

        /// <summary>
        /// Restarts the loop
        /// </summary>
        public void Restart()
        {
            _running = true;
            StartLoop();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Shortcut method to raise a <see cref="OnStatusUpdate"/> event
        /// </summary>
        /// <param name="message">Message to send with the event</param>
        private void SendStatus(string message) => OnStatusUpdate?.Invoke(this, new StatusUpdateArgs(_rafflesJoined, message));

        #region Operations
        /// <summary>
        /// Attempts to obtain the account CSRF token required to perform AJAX requests
        /// </summary>
        /// <returns></returns>
        private async Task GetCsrfToken()
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
                        _csrfToken = csrf.Groups[1].Value;
                        OnCsrfTokenObtained?.Invoke(this, new CsrfTokenObtainedArgs(_csrfToken));
                        tokenObtained = true;
                    }
                    else
                    {
                        if (html.Contains(Strings.SiteDown))
                        {
                            OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Site appears to be down/under maintenance. Trying again after 1 minute."));
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

        private async Task ScanRaffles()
        {
            _raffleQueue.Clear();

            OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Scanning raffles"));

            bool doneScanning = false;
            string html = await _http.GetStringAsync("https://scrap.tf/raffles");
            string lastId = string.Empty;

            while (!doneScanning)
            {
                string json = await Paginate(lastId);

                if (json == null)
                {
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
                                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Scanning next page (apex = {Apex})", lastId));

                                await Task.Delay(_config.Delays.PaginateDelay);

                                continue;
                            }
                            else
                            {
                                doneScanning = true;
                            }

                            OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Done scanning all raffles, grabbing IDs of un-entered raffles..."));

                            SendStatus("Parsing scanned data...");

                            _html.LoadHtml(html);

                            var document = _html.DocumentNode;
                            var raffleElements = document.SelectNodes(Xpaths.UnenteredRaffles);
                            if (html.Contains("ScrapTF.Raffles.WithdrawRaffle"))
                            {
                                await CheckForWonRaffles();
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
                                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Encountered an error while paginating: {Message} - Waiting 10 seconds", paginateResponse.message));

                                    await Task.Delay(10_000);
                                }
                            }
                            else
                            {
                                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Paginate response for apex {Apex} was unsuccessful", lastId.IsNullOrEmpty() ? "<empty>" : lastId));
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Failed to read pagination data: {Error}", ex.Message));
                    }
                }
            }
        }

        private async Task<string> Paginate(string apex = null)
        {
            SendStatus("Paginating...");

            string url = "https://scrap.tf/ajax/raffles/Paginate";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("start", apex),
                new KeyValuePair<string, string>("sort", "1"),
                new KeyValuePair<string, string>("puzzle", "0"),
                new KeyValuePair<string, string>("csrf", _csrfToken),
            });

            var response = await _http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Pagination returned a {Status} response instead of JSON. Waiting...", response.StatusCode));
                return null;
            }
        }

        private async Task CheckForWonRaffles()
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
                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Adding {Entered} to won raffles list", raffleId));

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

        private async Task JoinRaffles()
        {
            int entered = 0;
            int total = _raffleQueue.Count;
            int joinDelay = _config.Delays.JoinDelay;

            foreach (string raffle in _enteredRaffles)
            {
                if (_raffleQueue.Contains(raffle))
                {
                    _raffleQueue.Remove(raffle);
                }
            }

            foreach (string raffle in _raffleQueue)
            {
                SendStatus($"Joining raffle {raffle}...");

                string html = await _http.GetStringAsync($"https://scrap.tf/raffles/{raffle}");
                var hash = RegexPatterns.RaffleHashRegex.Match(html);
                var limits = RegexPatterns.RaffleLimitRegex.Match(html);
                bool hasEnded = html.Contains("data-time=\"Raffle Ended\"");

                var hp = new HoneypotChecker(html);
                    hp.Check();
                if (hp.IsHoneypot)
                {
                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Information, "Raffle {Id} is likely a honeypot: {Reason}", raffle, hp.Reason));

                    total--;
                    _enteredRaffles.Add(raffle);
                    continue;
                }

                if (hasEnded)
                {
                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Information, "Raffle {Id} has ended", raffle));

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
                        OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Information, "Raffle {Id} has too few entries", raffle));

                        total--;
                        continue;
                    }

                    if (num >= max)
                    {
                        OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Information, "Raffle {Id} is full ({Num}/{Max})", raffle, num, max));

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
                        new KeyValuePair<string, string>("csrf", _csrfToken),
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

                        OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Information, "[{Entered}/{Total}] Joined raffle {Id}", entered, total, raffle));

                        _enteredRaffles.Add(raffle);
                        _rafflesJoined++;

                        if (_config.RaffleActions.VoteInPolls)
                        {
                            var poll = RegexPatterns.RafflePollRegex.Match(html);
                            if (poll.Success)
                            {
                                string pollId = poll.Groups[1].Value;

                                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Voting in poll {Poll}", pollId));

                                var optionsMatches = RegexPatterns.RafflePollOptionRegex.Matches(html);
                                if (optionsMatches.Count > 0)
                                {
                                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Found {OptionCount} " + "option".Pluralize(optionsMatches.Count) + " in poll", optionsMatches.Count));

                                    await AnswerPoll(pollId, optionsMatches.Count);
                                }
                                else
                                {
                                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Warning, "Didn't find any options for poll"));
                                }
                            }
                        }
                    }
                    else
                    {
                        total--;
                        OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Unable to join raffle {Id}: {Message}", raffle, joinRaffleResponse.message));
                    }

                    SendStatus("Waiting...");

                    await Task.Delay(joinDelay);
                }
                else
                {
                    total--;
                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Warning, "Could not obtain hash from raffle {id}", raffle));
                }
            }

            if (entered > 0)
            {
                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Information, "Finished raffle queue"));
            }
        }

        private async Task AnswerPoll(string pollId, int numOptions)
        {
            SendStatus($"Voting in poll {pollId}");

            string url = "https://scrap.tf/ajax/viewpoll/SubmitAnswer";
            int choice = numOptions > 1 ? _rng.Next(0, numOptions) : 0;

            OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Chose poll option {Option}", numOptions + 1));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("poll", pollId),
                new KeyValuePair<string, string>("answers[]", choice.ToString()),
                new KeyValuePair<string, string>("csrf", _csrfToken),
            });

            var response = await _http.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<SubmitAnswerResponse>(json);
                if (!data.success || data.message != "Answered!")
                {
                    OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Poll answer failed: {Message}", data.message));
                }

                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Debug, "Successfully answered poll {Poll}", pollId));
            }
            else
            {
                OnLogger?.Invoke(this, new LoggerArgs(LogEventLevel.Error, "Request to answer poll failed: {StatusCode}", response.StatusCode));
            }
        }
        #endregion
        #endregion
    }
}
