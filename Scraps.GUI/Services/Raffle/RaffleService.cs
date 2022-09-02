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

using System.Text.RegularExpressions;

using AngleSharp.Html.Parser;

using Scraps.GUI.Models;
using Scraps.GUI.Extensions;
using Scraps.GUI.Services.Raffle;
using Scraps.GUI.Services.Raffle.Exceptions;

namespace Scraps.GUI.Services;

public class RaffleService
{
    public bool Running;
    
    private const string RafflePanelSelector   = ".panel-raffle:not(.raffle-entered)";
    private const string AccountBannedString   = "You have received a site-ban";
    private const string ProfileNotSetUpString = "Scrap.TF requires your Steam profile and inventory set to <b>Public</b> visibility.";
    private const string SiteDownString        = "<div class=\"dialog-title\">We're down!</div>";
    private const string CloudflareString      = "cf-wrapper";

    private string                  _cookie;
    private string                  _csrfToken;
    private HttpClient              _http;
    private int                     _scanDelay;
    private int                     _joinDelay;
    private bool                    _stoppedFromError;
    private bool                    _alertedOfWonRaffles;
    private CancellationToken       _cancelToken;
    private CancellationTokenSource _cancelTokenSource;

    private readonly LaunchOptions _options;
    private readonly Logger        _log               = LogManager.GetCurrentClassLogger();
    private readonly HtmlParser    _html              = new();
    private readonly List<string>  _raffleQueue       = new();
    private readonly List<string>  _enteredRaffles    = new();
    private readonly Regex         _csrfPattern       = new(@"value=""(?<CsrfToken>[a-f\d]{64})""");
    private readonly Regex         _banReasonPattern  = new(@"<b>Reason:<\/b> (?<Reason>[\w\s]+)");
    private readonly Regex         _wonRafflesPattern = new(@"You've won \d raffles? that must be withdrawn");
    private readonly Regex         _entryPattern      = new(@"ScrapTF\.Raffles\.RedirectToRaffle\('(?<RaffleId>[A-Z0-9]{6,})'\)", RegexOptions.Compiled);
    private readonly Regex         _hashPattern       = new(@"EnterRaffle\('(?<RaffleId>[A-Z0-9]{6,})', '(?<RaffleHash>[a-f0-9]{64})'", RegexOptions.Compiled);
    private readonly Regex         _limitPattern      = new(@"total=""(?<Entered>\d+)"" data-max=""(?<Max>\d+)", RegexOptions.Compiled);

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
        _options = options;
    }

    #region Public Methods

    public async Task StartAsync()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _cancelToken       = _cancelTokenSource.Token;

        Running = true;

        SendStatus("Starting");
        _log.Info("Starting");

        OnStarting?.Invoke(this, new StartingArgs());

        CreateHttpClient();

        try
        {
            await GetCsrfTokenAsync();

            _scanDelay = Properties.UserConfig.Default.ScanDelay;
            _joinDelay = Properties.UserConfig.Default.JoinDelay;

            OnRunning?.Invoke(this, new RunningArgs());

            while (Running && !_cancelToken.IsCancellationRequested)
            {
                SendStatus("Scanning raffles");

                await ScanRafflesAsync();

                OnPaginateDone?.Invoke(this, new PaginateDoneArgs());

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
                switch (ex)
                {
                    case AccountBannedException:
                        _stoppedFromError = false; // So we don't try to restart for no reason
                        _log.Error("Account banned: {Reason}", ex.Message);
                        break;
                    case CloudflareException:
                        _stoppedFromError = false;
                        _log.Error(ex.Message);
                        break;
                    default:
                        _stoppedFromError = true;
                        _log.Error("{Err}", ex.ToString());
                        break;
                }
            }

            Cancel();
        }
        finally
        {
            OnStopped?.Invoke(this, new StoppedArgs());
            _log.Info("Stopped");
            SendStatus(null);
            Running = false;

            if (_options.AutoReconnect && _stoppedFromError)
            {
                _log.Info("Restarting in 5 seconds.");
                await Task.Delay(5_000, _cancelToken);
                await StartAsync();
            }
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

    private void CreateHttpClient()
    {
        _http?.Dispose();

        _log.Debug("Creating HTTP client");

        _cookie = Properties.UserConfig.Default.Cookie;
        var cookies = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookies,
            UseCookies      = true,
        };

        var client = new HttpClient(handler);
        client.BaseAddress = new Uri("https://scrap.tf");
        client.DefaultRequestHeaders.Add("user-agent", GlobalShared.MimicUserAgent);
        client.DefaultRequestHeaders.Add("cookie", "scr_session=" + _cookie);

        _http = client;

        _log.Debug("Created HTTP client (cookie={First20},{Length})", _cookie[..20], _cookie.Length);
    }

    private async Task GetCsrfTokenAsync()
    {
        SendStatus("Obtaining CSRF token");

        string html = await GetStringAsync();
        if (html.Contains(AccountBannedString))
        {
            throw await NewAccountBannedException();
        }
        
        if (html.Contains(CloudflareString))
        {
            throw new CloudflareException("Scrap.TF is displaying a Cloudflare challenge, Scraps cannot continue.");
        }
        
        if (html.Contains(ProfileNotSetUpString))
        {
            throw new ProfileNotSetUpException("Profile is not set up to use Scrap.TF. See the website for more info.");
        }
        
        var csrf = _csrfPattern.Match(html);
        if (csrf.Success)
        {
            _csrfToken = csrf.Groups["CsrfToken"].Value;
            OnCsrfTokenObtained?.Invoke(this, new CsrfTokenObtainedArgs(_csrfToken));

            using (var document = await _html.ParseDocumentAsync(html, _cancelToken))
            {
                var usernameElement = document.QuerySelector("li.dropdown.nav-userinfo");
                if (usernameElement != null)
                {
                    var username = usernameElement.GetAttribute("title");
                    _log.Info("Logged in as {Username}", username);
                }
            }
        }
        else
        {
            if (html.Contains(SiteDownString))
            {
                throw new DownForMaintenanceException("Site appears to be down/under maintenance.");
            }

            throw new Exception("Unable to retrieve CSRF token. Please check your cookie value.");
        }
    }

    private async Task ScanRafflesAsync()
    {
        _raffleQueue.Clear();

        _log.Debug("Scanning raffles");

        bool   doneScanning = false;
        string html         = await GetStringAsync("/raffles");
        string lastId       = string.Empty;

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
                    switch (paginateResponse)
                    {
                        case { Success: true }:
                        {
                            html   += paginateResponse.Html;
                            lastId =  paginateResponse.LastId;

                            if (!paginateResponse.Done)
                            {
                                _log.Debug("Scanning next page (apex = {Apex})", lastId);

                                await Task.Delay(Properties.UserConfig.Default.PaginateDelay, _cancelToken);
                                continue;
                            }

                            doneScanning = true;

                            _log.Debug("Done scanning all raffles, grabbing IDs of un-entered raffles");

                            using (var document = await _html.ParseDocumentAsync(html, _cancelToken))
                            {
                                var raffleElements = document.QuerySelectorAll(RafflePanelSelector);
                                if (html.Contains("ScrapTF.Raffles.WithdrawRaffle"))
                                {
                                    CheckForWonRaffles(html);
                                }
                                else
                                {
                                    _alertedOfWonRaffles = false;
                                }

                                foreach (var raffleElement in raffleElements)
                                {
                                    string elementHtml   = raffleElement.InnerHtml;
                                    var    raffleIdMatch = _entryPattern.Match(elementHtml);
                                    if (!raffleIdMatch.Success)
                                    {
                                        _log.Error("Unable to find raffle ID from {Html}", elementHtml);
                                        continue;
                                    }
                            
                                    string raffleId = raffleIdMatch.Groups["RaffleId"].Value;

                                    if (_raffleQueue.Contains(raffleId) || _enteredRaffles.Contains(raffleId)) continue;
                            
                                    SendStatus($"Adding raffle {raffleId} to queue");

                                    _raffleQueue.Add(raffleId);
                                }
                            }

                            break;
                        }
                        case { Message: { } } when paginateResponse.Message.Contains("active site ban"):
                            throw await NewAccountBannedException();
                        case { Message: { } }:
                            _log.Error("Encountered an error while paginating: {Message} - Waiting 10 seconds", paginateResponse.Message);

                            await Task.Delay(10_000, _cancelToken);
                            break;
                        default:
                            _log.Error("Paginate response for apex {Apex} was unsuccessful", lastId.IsNullOrEmpty() ? "<empty>" : lastId);
                            break;
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
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("start", apex),
            new KeyValuePair<string, string>("sort", sort),
            new KeyValuePair<string, string>("puzzle", "0"),
            new KeyValuePair<string, string>("csrf", _csrfToken),
        });

        var response = await _http.PostAsync("/ajax/raffles/Paginate", content, _cancelToken);

        if (response.StatusCode != HttpStatusCode.OK) return null;
        
        string html = await response.Content.ReadAsStringAsync(_cancelToken);
        OnPaginate?.Invoke(this, new PaginateArgs(apex, html));

        return html;

    }

    private void CheckForWonRaffles(string html)
    {
        if (_alertedOfWonRaffles) return;
        
        var    match   = _wonRafflesPattern.Match(html);
        string message = match.Groups[0].Value;

        OnRafflesWon?.Invoke(this, new RafflesWonArgs(message));

        _log.Info(message);
        _alertedOfWonRaffles = true;
    }

    private async Task JoinRafflesAsync()
    {
        int entered = 0;
        int total   = _raffleQueue.Count;
        var queue   = _raffleQueue.Where(r => !_enteredRaffles.Contains(r));
        
        foreach (string raffle in queue)
        {
            SendStatus($"Joining raffle {raffle}");

            string html        = await GetStringAsync($"/raffles/{raffle}");
            var    hashMatch   = _hashPattern.Match(html);
            var    limitsMatch = _limitPattern.Match(html);
            bool   hasEnded    = html.Contains(@"data-time=""Raffle Ended""");

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

            if (limitsMatch.Success)
            {
                int num = int.Parse(limitsMatch.Groups["Entered"].Value);
                int max = int.Parse(limitsMatch.Groups["Max"].Value);
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

            if (hashMatch.Success)
            {
                var hash = hashMatch.Groups["RaffleHash"].Value;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("raffle", raffle),
                    new KeyValuePair<string, string>("captcha", ""),
                    new KeyValuePair<string, string>("hash", hash),
                    new KeyValuePair<string, string>("flag", ""),
                    new KeyValuePair<string, string>("csrf", _csrfToken),
                });

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/ajax/viewraffle/EnterRaffle");
                httpRequest.Content          = content;
                httpRequest.Headers.Referrer = new Uri($"https://scrap.tf/raffles/{raffle}");
                var response = await _http.SendAsync(httpRequest, _cancelToken);

                string json               = await response.Content.ReadAsStringAsync(_cancelToken);
                var    joinRaffleResponse = JsonSerializer.Deserialize<JoinRaffleResponse>(json);
                if (joinRaffleResponse is { Success: true })
                {
                    entered++;

                    OnRaffleJoined?.Invoke(this, new RaffleJoinedArgs(raffle, html, entered, total));

                    _log.Info("Joined raffle {Id} ({Entered} of {Total})", raffle, entered, total);

                    _enteredRaffles.Add(raffle);
                }
                else
                {
                    _log.Error("Unable to join raffle {Id}: {Message}", raffle, joinRaffleResponse?.Message ?? "Unknown");
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
        string       html   = await GetStringAsync("/banappeal");
        var          match  = _banReasonPattern.Match(html);
        return match.Success ? match.Groups["Reason"].Value.Trim() : "Could not obtain reason";
    }

    private void SendStatus(string message) => OnStatus?.Invoke(this, new StatusArgs(message));

    private async Task<AccountBannedException> NewAccountBannedException()
    {
        return new AccountBannedException(await GetBanReason());
    }
    
    private async Task<string> GetStringAsync(string path = "/")
    {
        _log.Debug("Sending GET request to {Path}", path);
        
        var res = await _http.GetAsync(path, _cancelToken);
        if (res.StatusCode == HttpStatusCode.OK)
        {
            return await res.Content.ReadAsStringAsync(_cancelToken);
        }
        
        throw new HttpRequestException($"Unable to get string: {res.ReasonPhrase}");
    }

    #endregion
}