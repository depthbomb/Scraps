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

using Scraps.GUI.Models;
using Scraps.GUI.Constants;
using Scraps.GUI.Extensions;
using Scraps.GUI.Services.Raffle;
using Scraps.GUI.Services.Raffle.Exceptions;

namespace Scraps.GUI.Services
{
    public class RaffleService
    {
        public bool Running = false;

        private string _cookie;
        private string _csrfToken;
        private HttpClient _http;
        private int _scanDelay;
        private int _joinDelay;
        private bool _alertedOfWonRaffles = false;
        private CancellationToken _cancelToken;
        private CancellationTokenSource _cancelTokenSource;

        private readonly Logger _log;
        private readonly HtmlAgilityPack.HtmlDocument _html;
        private readonly LaunchOptions _options;
        private readonly List<string> _raffleQueue = new();
        private readonly List<string> _enteredRaffles = new();

        #region Events
        /// <summary>
        /// Raised when a status is sent
        /// </summary>
        public event EventHandler<StatusArgs> OnStatus;
        /// <summary>
        /// Raised when a start is requested
        /// </summary>
        public event EventHandler<StartingArgs> OnStarting;
        /// <summary>
        /// Raised when the main loop has officially started
        /// </summary>
        public event EventHandler<RunningArgs> OnRunning;
        /// <summary>
        /// Raised when the CSRF token has been parsed from the site
        /// </summary>
        public event EventHandler<CsrfTokenObtainedArgs> OnCsrfTokenObtained;
        /// <summary>
        /// Raised when the bot receives a response from paginating the raffle index
        /// </summary>
        public event EventHandler<PaginateArgs> OnPaginate;
        /// <summary>
        /// Raised when pagination has completed before processing the results
        /// </summary>
        public event EventHandler<PaginateDoneArgs> OnPaginateDone;
        /// <summary>
        /// Raised when a raffle has been successfully joined
        /// </summary>
        public event EventHandler<RaffleJoinedArgs> OnRaffleJoined;
        /// <summary>
        /// Raised when one or more raffles have been won
        /// </summary>
        public event EventHandler<RafflesWonArgs> OnRafflesWon;
        /// <summary>
        /// Raised when cancellation has succeeded
        /// </summary>
        public event EventHandler<StoppedArgs> OnStopped;
        #endregion

        public RaffleService(LaunchOptions options)
        {
            _log = LogManager.GetCurrentClassLogger();
            _html = new HtmlAgilityPack.HtmlDocument();
            _options = options;
        }

        #region Public Methods
        public async Task StartAsync()
        {
            _cancelTokenSource = new();
            _cancelToken = _cancelTokenSource.Token;

            Running = true;

            SendStatus("Starting");
            _log.Info("Starting");

            OnStarting?.Invoke(this, new());

            if (_http == null || _cookie != Properties.UserConfig.Default.Cookie)
            {
                _http?.Dispose();

                _log.Debug("Creating HTTP client");

                _cookie = Properties.UserConfig.Default.Cookie;
                var cookies = new CookieContainer();
                var handler = new HttpClientHandler
                {
                    CookieContainer = cookies,
                    UseCookies = true,
                };
                var client = new HttpClient(handler);
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Add("user-agent", Strings.USER_AGENT);
                    client.DefaultRequestHeaders.Add("cookie", "scr_session=" + _cookie);

                _http = client;
            }

            try
            {
                await GetCsrfTokenAsync();

                _scanDelay = Properties.UserConfig.Default.ScanDelay;
                _joinDelay = Properties.UserConfig.Default.JoinDelay;

                OnRunning?.Invoke(this, new());

                while (Running && !_cancelToken.IsCancellationRequested)
                {
                    SendStatus("Scanning raffles");

                    await ScanRafflesAsync();

                    OnPaginateDone?.Invoke(this, new());

                    if (_raffleQueue.Count > 0)
                    {
                        SendStatus("Joining raffles");

                        _scanDelay = Properties.UserConfig.Default.ScanDelay;

                        await JoinRafflesAsync();
                    }
                    else
                    {
                        SendStatus("Waiting to scan");

                        _log.Debug("All raffles have been entered, scanning again after {Delay} seconds", _scanDelay / 1000);

                        await Task.Delay(_scanDelay, _cancelToken);

                        if (Properties.UserConfig.Default.IncrementScanDelay)
                        {
                            _scanDelay += 1000;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is not TaskCanceledException || _options.Debug)
                {
                    if (ex is AccountBannedException)
                    {
                        _log.Error("Account banned: {Reason}", ex.Message);
                    }
                    else
                    {
                        _log.Error("{Name}{Stack}", ex.Source, ex.StackTrace);
                    }
                }

                Cancel();
            }
            finally
            {
                OnStopped?.Invoke(this, new());
                _log.Info("Stopped");
                SendStatus(null);
                Running = false;
            }
        }

        public void Cancel()
        {
            if (Running)
            {
                _cancelTokenSource.Cancel();
            }
        }
        #endregion

        #region Private Methods
        private async Task GetCsrfTokenAsync()
        {
            SendStatus("Obtaining CSRF token");

            string html = await _http.GetStringAsync("https://scrap.tf");
            if (html.Contains(Strings.ACCOUNT_BANNED))
            {
                throw await NewAccountBannedException();
            }
            else if (html.Contains(Strings.PROFILE_SET_UP))
            {
                throw new ProfileNotSetUpException("Profile is not set up to use Scrap.TF. See the website for more info.");
            }
            else
            {
                var csrf = RegexPatterns.CSRF.Match(html);
                if (csrf.Success)
                {
                    _csrfToken = csrf.Groups[1].Value;
                    OnCsrfTokenObtained?.Invoke(this, new(_csrfToken));
                }
                else
                {
                    if (html.Contains(Strings.SITE_DOWN))
                    {
                        throw new DownForMaintenanceException("Site appears to be down/under maintenance. Trying again after 1 minute.");
                    }
                    else
                    {
                        throw new Exception("Unable to retreive CSRF token. Please check your cookie value.");
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

            while (!doneScanning && !_cancelToken.IsCancellationRequested)
            {
                string json = await PaginateAsync(lastId);

                if (json == null)
                {
                    _log.Error("Pagination returned an invalid response instead of JSON. Waiting 10 seconds");
                    await Task.Delay(10_000, _cancelToken);
                }
                else
                {
                    try
                    {
                        var paginateResponse = JsonSerializer.Deserialize<PaginateResponse>(json);
                        if (paginateResponse.Success)
                        {
                            html += paginateResponse.Html;
                            lastId = paginateResponse.LastId;

                            if (!paginateResponse.Done)
                            {
                                _log.Debug("Scanning next page (apex = {Apex})", lastId);

                                await Task.Delay(Properties.UserConfig.Default.PaginateDelay, _cancelToken);
                                continue;
                            }
                            else
                            {
                                doneScanning = true;
                            }

                            _log.Debug("Done scanning all raffles, grabbing IDs of un-entered raffles");
                            _html.LoadHtml(html);

                            var document = _html.DocumentNode;
                            var raffleElements = document.SelectNodes(Xpaths.UNENTERED_RAFFLES);
                            if (html.Contains("ScrapTF.Raffles.WithdrawRaffle"))
                            {
                                CheckForWonRaffles(html);
                            }
                            else
                            {
                                _alertedOfWonRaffles = false;
                            }

                            foreach (var el in raffleElements)
                            {
                                string elementHtml = el.InnerHtml.Trim();
                                string raffleId = RegexPatterns.RAFFLE_ENTRY.Match(elementHtml).Groups[1].Value.Trim();
                                if (
                                    !raffleId.IsNullOrEmpty() &&
                                    !_raffleQueue.Contains(raffleId) &&
                                    !_enteredRaffles.Contains(raffleId)
                                )
                                {
                                    SendStatus($"Adding raffle {raffleId} to queue");

                                    _raffleQueue.Add(raffleId);
                                }
                            }
                        }
                        else
                        {
                            if (paginateResponse.Message != null)
                            {
                                if (paginateResponse.Message.Contains("active site ban"))
                                {
                                    throw await NewAccountBannedException();
                                }
                                else
                                {
                                    _log.Error("Encountered an error while paginating: {Message} - Waiting 10 seconds", paginateResponse.Message);

                                    await Task.Delay(10_000, _cancelToken);
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
            SendStatus("Paginating");

            string sort = Properties.UserConfig.Default.SortByNew ? "1" : "0";
            string url = "https://scrap.tf/ajax/raffles/Paginate";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("start", apex),
                new KeyValuePair<string, string>("sort", sort),
                new KeyValuePair<string, string>("puzzle", "0"),
                new KeyValuePair<string, string>("csrf", _csrfToken),
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

        private void CheckForWonRaffles(string html)
        {
            if (!_alertedOfWonRaffles)
            {
                var match = RegexPatterns.WON_RAFFLES_ALERT.Match(html);
                string message = match.Groups[0].Value;

                OnRafflesWon?.Invoke(this, new RafflesWonArgs(message));

                _log.Info(message);
                _alertedOfWonRaffles = true;
            }
        }

        private async Task JoinRafflesAsync()
        {
            int entered = 0;
            int total = _raffleQueue.Count;

            var queue = _raffleQueue.Where(r => !_enteredRaffles.Contains(r));
            foreach (string raffle in queue)
            {
                SendStatus($"Joining raffle {raffle}");

                string html = await _http.GetStringAsync($"https://scrap.tf/raffles/{raffle}");
                var hash = RegexPatterns.RAFFLE_HASH.Match(html);
                var limits = RegexPatterns.RAFFLE_LIMIT.Match(html);
                bool hasEnded = html.Contains("data-time=\"Raffle Ended\"");

                using (var hp = new HoneypotService())
                {
                    hp.Check(html);
                    if (hp.IsHoneypot)
                    {
                        _log.Info("Raffle {Id} is likely a honeypot: {Reason}", raffle, hp.Reason);

                        total--;
                        _enteredRaffles.Add(raffle);
                        continue;
                    }
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

                    if (Properties.UserConfig.Default.Paranoid && num < 2)
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
                        new KeyValuePair<string, string>("csrf", _csrfToken),
                    });

                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                        httpRequest.Content = content;
                        httpRequest.Headers.Referrer = new Uri($"https://scrap.tf/raffles/{raffle}");
                    var response = await _http.SendAsync(httpRequest);

                    string json = await response.Content.ReadAsStringAsync();

                    var joinRaffleResponse = JsonSerializer.Deserialize<JoinRaffleResponse>(json);
                    if (joinRaffleResponse.Success)
                    {
                        entered++;

                        OnRaffleJoined?.Invoke(this, new RaffleJoinedArgs(raffle, html, entered, total));

                        _log.Info("Joined raffle {Id} ({Entered} of {Total})", raffle, entered, total);

                        _enteredRaffles.Add(raffle);
                    }
                    else
                    {
                        _log.Error("Unable to join raffle {Id}: {Message}", raffle, joinRaffleResponse.Message);
                    }

                    SendStatus("Waiting");

                    await Task.Delay(_joinDelay, _cancelToken);
                }
                else
                {
                    _enteredRaffles.Add(raffle);

                    _log.Error("Could not obtain hash from raffle {Id}", raffle);
                }
            }

            if (entered > 0)
            {
                _log.Info("Finished raffle queue");
            }
        }

        private async Task<string> GetBanReason()
        {
            string reason = "Could not obtain reason";
            string html = await _http.GetStringAsync("https://scrap.tf/banappeal");
            var match = RegexPatterns.BAN_REASON.Match(html);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return reason;
        }

        private void SendStatus(string message) => OnStatus?.Invoke(this, new(message));

        async Task<AccountBannedException> NewAccountBannedException()
        {
            return new AccountBannedException(await GetBanReason());
        }
        #endregion
    }
}
