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

using System.Media;

using AngleSharp.Html.Parser;
using Windows.Win32.Foundation;

using Scraps.Events;
using Scraps.Models;
using Scraps.Resources;
using Scraps.Exceptions;
using Scraps.Extensions;
using Scraps.Services.Honeypot;

namespace Scraps.Services;

public class RaffleService : IDisposable
{
    /// <summary>
    /// Whether the runner is running because a cancellation is not requested.
    /// </summary>
    public bool Running;
    
    #region Events

    /// <summary>
    /// Raised when a start is requested
    /// </summary>
    public event EventHandler<RaffleServiceStartingArgs> OnStarting;

    /// <summary>
    /// Raised when the main loop has officially started
    /// </summary>
    public event EventHandler<RaffleServiceStartedArgs> OnStarted;

    /// <summary>
    /// Raised when cancellation is requested
    /// </summary>
    public event EventHandler<RaffleServiceStoppingArgs> OnStopping; 

    /// <summary>
    /// Raised when cancellation has succeeded
    /// </summary>
    public event EventHandler<RaffleServiceStoppedArgs> OnStopped;

    /// <summary>
    /// Raised when the CSRF token has been parsed from the site
    /// </summary>
    public event EventHandler<RaffleServiceCsrfTokenObtainedArgs> OnCsrfTokenObtained;

    /// <summary>
    /// Raised when the runner starts paginating the raffle index
    /// </summary>
    public event EventHandler<RaffleServicePaginatingArgs> OnPaginating;

    /// <summary>
    /// Raised when the runner receives a response from paginating the raffle index
    /// </summary>
    public event EventHandler<RaffleServicePaginatedArgs> OnPaginated;

    /// <summary>
    /// Raised when a raffle has been successfully joined
    /// </summary>
    public event EventHandler<RaffleServiceJoinedRaffleArgs> OnJoinedRaffle;

    /// <summary>
    /// Raised when item withdrawals are available from won raffles
    /// </summary>
    public event EventHandler<RaffleServiceWithdrawalAvailableArgs> OnWithdrawalAvailable;

    /// <summary>
    /// Raised when a status message is broadcast
    /// </summary>
    public event EventHandler<RaffleServiceStatusArgs> OnStatus;

    #endregion
    
    private const string RafflePanelSelector   = ".panel-raffle:not(.raffle-entered)";
    private const string AccountBannedString   = "You have received a site-ban";
    private const string ProfileNotSetUpString = "Scrap.TF requires your Steam profile and inventory set to <b>Public</b> visibility.";
    private const string SiteDownString        = "<div class=\"dialog-title\">We're down!</div>";
    private const string CloudflareString      = "challenge-form";
    private const string UserAgent             = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
    
    private string                  _cookie;
    private string                  _csrfToken;
    private HttpClient              _http;
    private int                     _scanDelay;
    private int                     _joinDelay;
    private int                     _paginateDelay;
    private bool                    _alertedOfWonRaffles;
    private CancellationToken       _cancelToken;
    private CancellationTokenSource _cancelTokenSource;
    private System.Random           _randomifier;

    private readonly SettingsService _settings;
    private readonly SoundPlayer     _alertPlayer;
    private readonly Logger          _log             = LogManager.GetCurrentClassLogger();
    private readonly HtmlParser      _html            = new();
    private readonly List<string>    _raffleQueue     = new();
    private readonly List<string>    _enteredRaffles  = new();
    private readonly Regex           _csrfRegex       = new(@"value=""(?<CsrfToken>[a-f\d]{64})""");
    private readonly Regex           _banReasonRegex  = new(@"<b>Reason:<\/b> (?<Reason>[\w\s]+)");
    private readonly Regex           _wonRafflesRegex = new(@"You've won \d raffles? that must be withdrawn");
    private readonly Regex           _entryRegex      = new(@"ScrapTF\.Raffles\.RedirectToRaffle\('(?<RaffleId>[A-Z0-9]{6,})'\)", RegexOptions.Compiled);
    private readonly Regex           _hashRegex       = new(@"EnterRaffle\('(?<RaffleId>[A-Z0-9]{6,})', '(?<RaffleHash>[a-f0-9]{64})'", RegexOptions.Compiled);
    private readonly Regex           _limitRegex      = new(@"total=""(?<Entered>\d+)"" data-max=""(?<Max>\d+)", RegexOptions.Compiled);
    


    public RaffleService(SettingsService settings)
    {
        _settings    = settings;
        _alertPlayer = new SoundPlayer(Audio.alert);
        _randomifier = new System.Random();
    }
    
    ~RaffleService() => Dispose();
    
    public void Dispose()
    {
        _http?.Dispose();
        _cancelTokenSource?.Cancel();
        _cancelTokenSource?.Dispose();
    }

    /// <summary>
    /// Starts the runner loop.
    /// </summary>
    public async Task StartAsync()
    {
        _log.Trace("Attempting to start");
        
        if (Running)
        {
            _log.Debug("Already running, stopping");
            
            Stop();
        }
        
        OnStarting?.Invoke(this, new RaffleServiceStartingArgs());
        
        _cancelTokenSource = new CancellationTokenSource();
        _cancelToken       = _cancelTokenSource.Token;

        _log.Info("Starting");
        
        Broadcast("Starting");

        try
        {
            _http          = CreateHttpClient();
            _csrfToken     = await GetCsrfTokenAsync();
            _scanDelay     = _settings.GetInt("ScanDelay");
            _joinDelay     = _settings.GetInt("JoinDelay");
            _paginateDelay = _settings.GetInt("PaginateDelay");
            
            bool incrementScanDelay = _settings.GetBool("IncrementScanDelay");

            Running = true;

            OnStarted?.Invoke(this, new RaffleServiceStartedArgs());

            while (Running && !_cancelToken.IsCancellationRequested)
            {
                Broadcast("Scanning raffles");

                await EnqueueRafflesAsync();

                if (_raffleQueue.Count > 0)
                {
                    Broadcast("Joining raffles");

                    _scanDelay = _settings.GetInt("ScanDelay");

                    await JoinRafflesAsync();

                    continue;
                }

                Broadcast("Waiting to scan again");

                int scanDelayJittered = _scanDelay + _randomifier.Next(0, _settings.GetInt("ScanJitter"));

                _log.Info("All raffles have been entered, scanning again after {Delay} seconds", scanDelayJittered / 1000);

                await Task.Delay(scanDelayJittered,  _cancelToken);

                if (incrementScanDelay)
                    _scanDelay += 1000;
            }
        }
        catch (Exception ex)
        {
            if (ex is not TaskCanceledException)
            {
                switch (ex)
                {
                    case AccountBannedException:
                        _log.Fatal("Account banned: {Reason}", ex.Message);
                        break;
                    case CloudflareException:
                        _log.Fatal(ex.Message);
                        break;
                    default:
                        _log.Fatal("{Err}", ex.ToString());
                        break;
                }
            }
        }
        finally
        {
            _log.Info("Stopped");
            Broadcast("Stopped");
        
            OnStopped?.Invoke(this, new RaffleServiceStoppedArgs());
        }
    }

    /// <summary>
    /// Cancels all running tasks and breaks the main loop.
    /// </summary>
    public void Stop()
    {
        Broadcast("Stopping");
        
        OnStopping?.Invoke(this, new RaffleServiceStoppingArgs());
        
        if (!_cancelToken.IsCancellationRequested)
        {
            _cancelTokenSource.Cancel();
            
            Running = false;
        }
    }

    #region Private Methods
    
    private void Broadcast(string message) => OnStatus?.Invoke(this, new RaffleServiceStatusArgs(message));
    
    private HttpClient CreateHttpClient()
    {
        _log.Debug("Creating HTTP client");
        
        _http?.Dispose();
        _cookie = _settings.GetString("Cookie");
        var cookies = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookies,
            UseCookies      = true,
        };

        var client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://scrap.tf");
            client.DefaultRequestHeaders.Add("user-agent", UserAgent);
            client.DefaultRequestHeaders.Add("cookie", "scr_session=" + _cookie);

        _log.Debug("Created HTTP client (cookie={First32}...{Last32},{Length})", _cookie[..32], _cookie[^32..], _cookie.Length);
        
        return client;
    }

    private async Task<string> GetCsrfTokenAsync()
    {
        Broadcast("Grabbing CSRF token");
        
        string html = await GetStringAsync();
        var    csrf = _csrfRegex.Match(html);
        if (csrf.Success)
        {
            string csrfToken = csrf.Groups["CsrfToken"].Value;
            
            OnCsrfTokenObtained?.Invoke(this, new RaffleServiceCsrfTokenObtainedArgs(csrfToken));
            
            return csrfToken;
        }

        throw new Exception("Unable to retrieve CSRF token. Please check your cookie value.");
    }
    
    private async Task<string> GetBanReasonAsync()
    {
        _log.Debug("Retrieving ban reason");
        
        string html  = await GetStringAsync("/banappeal");
        var    match = _banReasonRegex.Match(html);
        return match.Success ? match.Groups["Reason"].Value.Trim() : "Could not obtain reason";
    }

    private async Task EnqueueRafflesAsync()
    {
        _raffleQueue.Clear();
        
        _log.Debug("Scanning raffles");
        
        bool         doneScanning  = false;
        string       html          = await GetStringAsync("/raffles");
        string       lastId        = string.Empty;

        while (!doneScanning && !_cancelToken.IsCancellationRequested)
        {
            var res = await PaginateAsync(lastId);
            if (res == null)
            {
                _log.Error("Pagination returned an invalid response instead of JSON. Waiting 10 seconds");
                
                await Task.Delay(10_000, _cancelToken);
                continue;
            }

            if (!res.Success)
            {
                if (res.Message.Contains("active site ban"))
                {
                    throw await ThrowAccountBannedAsync();
                }

                if (res.Message != null)
                {
                    _log.Error("Encountered an error while paginating: {Message} - Waiting 10 seconds", res.Message);
                    await Task.Delay(10_000, _cancelToken);
                }
                else
                {
                    _log.Error("Paginate response for apex {Apex} was unsuccessful", lastId.IsNullOrEmpty() ? "<empty>" : lastId);
                }
                
                continue;
            }

            html   += res.Html;
            lastId =  res.LastId;

            if (!res.Done)
            {
                await Task.Delay(_paginateDelay, _cancelToken);

                _log.Debug("Scanning next page (apex={Apex})", lastId);
                continue;
            }

            doneScanning = true;
            
            _log.Debug("Done scanning all raffles, enqueueing available raffles");

            using (var document = await _html.ParseDocumentAsync(html, _cancelToken))
            {
                var raffleElements = document.QuerySelectorAll(RafflePanelSelector);
                if (html.Contains("ScrapTF.Raffles.WithdrawRaffle"))
                {
                    var    wonRafflesMatch   = _wonRafflesRegex.Match(html);
                    string wonRafflesMessage = wonRafflesMatch.Groups[0].Value;

                    OnWithdrawalAvailable?.Invoke(this, new RaffleServiceWithdrawalAvailableArgs(wonRafflesMessage));

                    _log.Info(wonRafflesMessage);

                    if (!_alertedOfWonRaffles)
                    {
                        Native.FlashWindow((HWND)Utils.GetMainForm().Handle, true);
                    
                        _alertPlayer.Play();
                        _alertedOfWonRaffles = true;
                    }
                }
                else
                {
                    _alertedOfWonRaffles = false;
                }

                foreach (var raffleElement in raffleElements)
                {
                    string elementHtml   = raffleElement.InnerHtml;
                    var    raffleIdMatch = _entryRegex.Match(elementHtml);
                    if (!raffleIdMatch.Success)
                    {
                        _log.Error("Unable to find raffle ID from {Html}", elementHtml);
                        continue;
                    }
                            
                    string raffleId = raffleIdMatch.Groups["RaffleId"].Value;

                    if (_raffleQueue.Contains(raffleId) || _enteredRaffles.Contains(raffleId)) continue;

                    Broadcast($"Adding raffle {raffleId} to queue");
                    
                    _raffleQueue.Add(raffleId);
                }
            }
        }
    }

    private async Task JoinRafflesAsync()
    {
        int joinedRaffles = 0;
        int queueCount    = _raffleQueue.Count;
        var queue         = _raffleQueue.Where(r => !_enteredRaffles.Contains(r));
        foreach (string raffle in queue)
        {
            Broadcast($"Joining raffle {raffle}");
            
            string html        = await GetStringAsync($"/raffles/{raffle}");
            var    hashMatch   = _hashRegex.Match(html);
            var    limitsMatch = _limitRegex.Match(html);
            bool   hasEnded    = html.Contains(@"data-time=""Raffle Ended""");
            bool   paranoid    = _settings.GetBool("Paranoid");
            
            if (hasEnded)
            {
                _log.Info("Raffle {Id} has ended", raffle);

                queueCount--;
                _enteredRaffles.Add(raffle);
                continue;
            }
            
            if (limitsMatch.Success)
            {
                int num = int.Parse(limitsMatch.Groups["Entered"].Value);
                int max = int.Parse(limitsMatch.Groups["Max"].Value);
                if (paranoid && num < 2)
                {
                    _log.Info("Skipping raffle {Id} because it has too few entries (paranoid mode)", raffle);

                    queueCount--;
                    continue;
                }

                if (num >= max)
                {
                    _log.Info("Raffle {Id} is full ({Num}/{Max})", raffle, num, max);

                    queueCount--;
                    _enteredRaffles.Add(raffle);
                    continue;
                }
            }
            
            if (hashMatch.Success)
            {
                using (var honeypot = new HoneypotService())
                {
                    honeypot.Check(html);
                    if (honeypot.IsHoneypot)
                    {
                        _log.Info("Raffle {Id} is likely a honeypot: {Reason:l}", raffle, honeypot.Reason);

                        queueCount--;
                        _enteredRaffles.Add(raffle);
                        continue;
                    }
                }
                
                var hash = hashMatch.Groups["RaffleHash"].Value;
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("raffle", raffle),
                    new KeyValuePair<string, string>("captcha", ""),
                    new KeyValuePair<string, string>("hash", hash),
                    new KeyValuePair<string, string>("flag", ""),
                    new KeyValuePair<string, string>("csrf", _csrfToken),
                });
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/ajax/viewraffle/EnterRaffle")
                {
                    Content = content,
                    Headers =
                    {
                        Referrer = new Uri($"https://scrap.tf/raffles/{raffle}")
                    }
                };
                var response = await _http.SendAsync(httpRequest, _cancelToken);

                string json               = await response.Content.ReadAsStringAsync(_cancelToken);
                var    joinRaffleResponse = JsonSerializer.Deserialize<JoinRaffleResponse>(json);
                if (joinRaffleResponse is { Success: true })
                {
                    joinedRaffles++;

                    OnJoinedRaffle?.Invoke(this, new RaffleServiceJoinedRaffleArgs(raffle, html, joinedRaffles, queueCount));
                    
                    _log.Info("Joined raffle {Id} ({Entered} of {Total})", raffle, joinedRaffles, queueCount);

                    _enteredRaffles.Add(raffle);
                }
                else
                {
                    _log.Error("Unable to join raffle {Id}: {Message}", raffle, joinRaffleResponse?.Message ?? "Unknown");
                }

                int joinDelayJittered = _joinDelay + _randomifier.Next(0, _settings.GetInt("JoinJitter"));

                _log.Info("Waiting for next raffle, attempting to join in {Delay} seconds", joinDelayJittered / 1000);


                Broadcast("Waiting to join next raffle");


                await Task.Delay(joinDelayJittered, _cancelToken);
            }
            else
            {
                _enteredRaffles.Add(raffle);

                _log.Error("Could not obtain hash from raffle {Id}", raffle);
            }
        }
        
        if (joinedRaffles > 0)
        {
            _log.Info("Finished raffle queue");
        }
    }

    private async Task<PaginateResponse> PaginateAsync(string apex, bool sortByNew = true)
    {
        Broadcast("Paginating");
        
        OnPaginating?.Invoke(this, new RaffleServicePaginatingArgs(apex));
        
        var payload = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("start", apex),
            new KeyValuePair<string, string>("sort", sortByNew ? "1" : "0"),
            new KeyValuePair<string, string>("puzzle", "0"),
            new KeyValuePair<string, string>("csrf", _csrfToken),
        });

        try
        {
            var res = await _http.PostAsync("/ajax/raffles/Paginate", payload, _cancelToken);
            if (res.IsSuccessStatusCode)
            {
                var json             = await res.Content.ReadAsStreamAsync(_cancelToken);
                var paginateResponse = await JsonSerializer.DeserializeAsync<PaginateResponse>(json, cancellationToken: _cancelToken);
                
                OnPaginated?.Invoke(this, new RaffleServicePaginatedArgs(apex));

                return paginateResponse;
            }
        }
        catch (Exception ex)
        {
            _log.Error("Error while paginating");
            _log.Error(ex);
        }

        return null;
    }

    private async Task<string> GetStringAsync(string path = "/")
    {
        _http ??= CreateHttpClient();
        
        _log.Debug("Sending GET request to {Path}", path);

        var res = await _http.GetAsync(path, _cancelToken);
        if (res.StatusCode == HttpStatusCode.OK)
        {
            return await res.Content.ReadAsStringAsync(_cancelToken);
        }
        
        string body = await res.Content.ReadAsStringAsync(_cancelToken);
        
        if (body.Contains(CloudflareString))
            throw new CloudflareException("Scrap.TF is displaying a Cloudflare challenge and Scraps cannot continue. Please try again later. This is not a bug so do not report it.");
        
        if (body.Contains(AccountBannedString))
            throw await ThrowAccountBannedAsync();
        
        if (body.Contains(ProfileNotSetUpString))
            throw new ProfileNotSetUpException("Profile is not set up to use Scrap.TF. See the website for more info.");
        
        if (body.Contains(SiteDownString))
            throw new MaintenanceException("Site appears to be down/under maintenance.");
        
        _log.Error("Unable to get string: {Reason}", res.ReasonPhrase);
        _log.Error(body);

        throw new Exception("Unable to make request");
    }

    private async Task<AccountBannedException> ThrowAccountBannedAsync()
    {
        string banReason = await GetBanReasonAsync();
        return new AccountBannedException(banReason);
    }

    #endregion
}
