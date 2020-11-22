#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2020  Caprine Logic

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
using System.Xml;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using CommandLine;
using HtmlAgilityPack;

using Scraps.Models;
using Scraps.Common;
using Scraps.Common.Models;

using CaprineNet.Common.Utils;

namespace Scraps
{
    class Program
    {
        static string Csrf;
        static Logger Logger;
        static HttpClient Client;
        static Settings Settings;
        static HtmlDocument Html = new HtmlDocument();

        static int RafflesJoined = 0;
        static List<string> RaffleQueue = new List<string>();
        static List<string> EnteredRaffles = new List<string>();

        static bool Verbose = false;
        static bool UseProxy = false;
        static bool OnLatestRelease = true;

        static readonly string Title = $"Scraps - {AppVersion.Full}";

        static async Task Main(string[] args)
        {
            if(IsAlreadyRunning())
            {
                Environment.Exit(0);
            }

            SetStatus("Booting...");

            if (!Directory.Exists(Paths.StorePath))
            {
                Directory.CreateDirectory(Paths.StorePath);
            }

            if(File.Exists(Paths.SettingsFile))
            {
                Settings = LoadSettings();
            }
            else
            {
                Settings = new Settings();
                SaveSettings(Settings);
            }

            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                Verbose = o.Verbose;
                UseProxy = o.UseProxy;
                if (o.OpenSettings)
                {
                    Console.WriteLine("Opening settings file...");
                    Process.Start("explorer.exe", Paths.SettingsFile);
                    Environment.Exit(0);
                }
            });

            Client = InitializeHttpClient();

            await CheckForNewReleases();

            Logger = InitializeLogger();

            Console.WriteLine("Scraps - Scrap.TF Raffle Bot");
            Console.WriteLine("By depthbomb - https://s.team/p/fwc-crhc");
            Console.WriteLine("Changelog available at https://github.com/depthbomb/Scraps/blob/master/CHANGELOG.md");
            Console.WriteLine();

            Logger.Debug("Session Started ({Version})", AppVersion.Full);

            if (Settings.Cookie.IsNullOrEmpty())
            {
                Logger.Debug("User is configuring settings");

                var settings = new Settings();

                AskForCookie:

                Console.Write("Please enter your scr_session cookie: ");

                settings.Cookie = Console.ReadLine().Trim();

                if (settings.Cookie.IsNullOrEmpty())
				{
                    goto AskForCookie;
                }

                Settings = settings;

                SaveSettings();
            }

            await Start();
        }

        #region Setup
        static HttpClient InitializeHttpClient()
        {
        InitializeClient:

            int proxyIndex = 0;
            string userIP = GetUserIP();
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
            };

            if (UseProxy)
			{
                string proxyFile = @".\proxies.txt";
                if (!File.Exists(proxyFile))
				{
                    Console.WriteLine("Proxy list file not found. Please read https://github.com/depthbomb/Scraps/blob/master/INSTRUCTIONS.md for more info.");
                    Helpers.ExitState();
                }

                IEnumerable<string> proxies = File.ReadLines(@".\proxies.txt");

                if (proxyIndex + 1 >= proxies.Count())
				{
                    Console.WriteLine("Ran out of proxies to test. Please add new addresses to the proxies.txt file.");
                    Helpers.ExitState();
				}

                string proxy = proxies.ElementAt(proxyIndex);
                Console.WriteLine("Testing proxy {0}", proxy);
                handler.Proxy = new WebProxy(proxy);
                handler.UseProxy = true;
			}

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);
            client.DefaultRequestHeaders.Add("cookie", "scr_session=" + Settings.Cookie);

            if (UseProxy)
			{
                string testUrl = "https://ifconfig.co/ip";
                string ip = client.GetStringAsync($"http://{testUrl}").Result;

                if (ip == userIP)
				{
                    proxyIndex++;
                    goto InitializeClient;
				}
            }

            return client;
        }

        static async Task CheckForNewReleases()
        {
            Console.WriteLine("Checking for new releases...");

            string uri = "https://api.github.com/repos/depthbomb/scraps/releases/latest";
            string json = await Client.GetStringAsync(uri);
            var currentTag = new Version(AppVersion.SemVer);

            var release = JsonSerializer.Deserialize<LatestRelease>(json);
            var latestTag = new Version(release.tag_name.Replace("v", ""));   //  In case I'm inconsistent in tagging

            var compare = currentTag.CompareTo(latestTag);
            if (compare < 0)
            {
                OnLatestRelease = false;
                Console.Clear();
                ConsoleUtils.Restore();
                ConsoleUtils.FlashWindow(5, false);
                Console.WriteLine("A new version of Scraps ({0}) is available to download at {1}", latestTag.ToString(), release.html_url);
                Console.WriteLine("Continuing in 5 seconds");
                await Task.Delay(6000); //  Account for reading
            }
            else
            {
                Console.WriteLine("You are using the latest version of Scraps!");
                Console.Clear();
            }
        }

        static Logger InitializeLogger()
        {
            bool debug;
#if DEBUG
            debug = true;
#else
            debug = Verbose;
#endif
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    restrictedToMinimumLevel: debug ? LogEventLevel.Debug : LogEventLevel.Information,
                    outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] {Message:l}{NewLine}{Exception}"
                 )
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine(Paths.LogsPath, "Fatal-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(Path.Combine(Paths.LogsPath, "Errors-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine(Paths.LogsPath, "Warning-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(Path.Combine(Paths.LogsPath, "Info-.log"), rollingInterval: RollingInterval.Month))
                .WriteTo.File(Path.Combine(Paths.LogsPath, "Verbose-.log"), rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        static string GetUserIP()
		{
            string hostName = Dns.GetHostName();
            string ip = Dns.GetHostByName(hostName).AddressList[0].ToString();

            return ip;
        }
        #endregion

        #region Operations
        static async Task Start()
        {
            Console.CursorVisible = false;

            int scanDelay = 5000;

            try
            {
                SetStatus("Logging in...");
                await GetCsrf();
                Logger.Information("Successfully logged in");
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                Helpers.ExitState();
            }

            ScanRaffles:

            await ScanRaffles();

            if (RaffleQueue.Count > 0)
            {
                SetStatus("Joining raffles...");
                Logger.Information("{Count} " + "raffle".Pluralize(RaffleQueue.Count) + " " + "is".Pluralize(RaffleQueue.Count, "are") + " available to enter", RaffleQueue.Count);

                scanDelay = 5000;

                await JoinRaffles();
            }
            else
            {
                SetStatus("Waiting to scan...");
                Logger.Debug("All raffles have been entered, scanning again after {Delay} seconds", (scanDelay / 1000));

                await Task.Delay(scanDelay);

                if(Settings.IncrementScanDelay)
                {
                    scanDelay = scanDelay + 1000;
                }
            }

            goto ScanRaffles;
        }

        static async Task JoinRaffles()
        {
            int entered = 0;
            int total = RaffleQueue.Count;
            foreach (string raffle in RaffleQueue)
            {
                SetStatus($"Attempting to join raffle {raffle}...");
                string html = await Client.GetStringAsync($"https://scrap.tf/raffles/{raffle}");
                var hash = Regexes.RaffleHashRegex.Match(html);
                var limits = Regexes.RaffleLimitRegex.Match(html);
                bool hasEnded = html.Contains("data-time=\"Raffle Ended\"");

                if (IsHoneypotRaffle(html, out string honeypotInfo))
                {
                    Logger.Information("Raffle {Id} is very likely a honeypot and will be skipped: {Reason}", raffle, honeypotInfo);
                    total--;
                    EnteredRaffles.Add(raffle);
                    continue;
                }
                else
                {
                    if(hasEnded)
                    {
                        total--;
                        Logger.Information("Raffle {Id} has ended, skipping", raffle);
                        EnteredRaffles.Add(raffle);
                        continue;
                    }

                    if(limits.Success)
                    {
                        int num = int.Parse(limits.Groups[1].Value);
                        int max = int.Parse(limits.Groups[2].Value);
                        if(num >= max)
                        {
                            total--;
                            Logger.Information("Raffle {Id} is full ({Num}/{Max}), skipping", raffle, num, max);
                            EnteredRaffles.Add(raffle);
                            continue;
                        }
                    }

                    if(hash.Success)
                    {
                        string url = "https://scrap.tf/ajax/viewraffle/EnterRaffle";
                        var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("raffle", raffle),
                            new KeyValuePair<string, string>("captcha", ""),
                            new KeyValuePair<string, string>("hash", hash.Groups[1].Value),
                            new KeyValuePair<string, string>("flag", ""),
                            new KeyValuePair<string, string>("csrf", Csrf),
                        });

                        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                        httpRequest.Content = content;
                        httpRequest.Headers.Referrer = new Uri("https://scrap.tf/raffles/" + raffle);

                        var response = await Client.SendAsync(httpRequest);

                        string json = await response.Content.ReadAsStringAsync();

                        var resp = JsonSerializer.Deserialize<JoinRaffleResponse>(json);

                        if(resp.success)
                        {
                            entered++;
                            Logger.Information("[{Entered}/{Total}] Joined raffle {Id}", entered, total, raffle);
                            EnteredRaffles.Add(raffle);
                            RafflesJoined++;
                        }
                        else
                        {
                            total--;
                            Logger.Information("Unable to join raffle {Id} with message {Message}", raffle, resp.message);
                        }

                        SetStatus("Waiting to join next raffle...");
                        await Task.Delay(4000);
                    }
                    else
                    {
                        total--;
                        Logger.Warning("Could not obtain hash from raffle {Id}", raffle);
                    }
                }
            }
        }

        static async Task ScanRaffles()
        {
            SetStatus("Starting scan...");

            RaffleQueue.Clear();

            Logger.Debug("Scanning raffles");

            string html = await Client.GetStringAsync("https://scrap.tf/raffles");
            string lastId = string.Empty;

            ScanNext:

            SetStatus("Scanning...");

            string json = await Paginate(lastId);

            if (json == null)
			{
                await Task.Delay(10000);
			}
            else
			{
                try
                {
                    var resp = JsonSerializer.Deserialize<PaginateResponse>(json);
                    if(resp.success)
                    {
                        html += resp.html;
                        lastId = resp.lastid;

                        if(!resp.done)
                        {
                            Logger.Debug("Scanning next page (apex = {Apex})", lastId);
                            await Task.Delay(500);
                            goto ScanNext;
                        }

                        Logger.Debug("Done scanning all raffles, grabbing IDs of un-entered raffles...");

                        SetStatus("Parsing scanned data...");

                        Html.LoadHtml(html);

                        var document = Html.DocumentNode;
                        var raffleElements = document.SelectNodes(Xpaths.UnenteredRaffles);

                        foreach(var el in raffleElements)
                        {
                            string elementHtml = el.InnerHtml.Trim();
                            string raffleId = Regexes.RaffleEntryRegex.Match(elementHtml).Groups[1].Value.Trim();
                            if(
                                !raffleId.IsNullOrEmpty() &&    // For some reason `raffleId` will sometimes give us emptiness
                                !RaffleQueue.Contains(raffleId) &&
                                !EnteredRaffles.Contains(raffleId)
                            )
                            {
                                SetStatus($"Adding raffle {raffleId} to queue...");
                                RaffleQueue.Add(raffleId);
                            }
                        }
                    }
                    else
                    {
                        if(resp.message != null)
                        {
                            if(resp.message.Contains("active site ban"))
                            {
                                DisplayTombstone();
                            }
                            else
                            {
                                Logger.Error("Encountered an error while paginating: {Message}", resp.message);
                            }
                        }
                        else
                        {
                            Logger.Error("Paginate response for apex {Apex} was unsuccessful", lastId.IsNullOrEmpty() ? "<empty>" : lastId);
                        }
                    }
                }
                catch(JsonException ex)
                {
                    Logger.Error("Failed to read pagination data: {Error}", ex.Message);
                }
            }
        }

        static async Task<string> Paginate(string apex = null)
        {
            SetStatus("Paginating...");
            string url = "https://scrap.tf/ajax/raffles/Paginate";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("start", apex),
                new KeyValuePair<string, string>("sort", "1"),
                new KeyValuePair<string, string>("puzzle", "0"),
                new KeyValuePair<string, string>("csrf", Csrf),
            });

            var response = await Client.PostAsync(url, content);

            if (response.StatusCode.ToString() == "OK")
			{
                return await response.Content.ReadAsStringAsync();
            }
            else
			{
                Logger.Warning("Pagination returned a {Status} response instead of JSON. Waiting...", response.StatusCode);
                return null;
			}
        }

        static async Task GetCsrf()
        {
            string html = await Client.GetStringAsync("https://scrap.tf");

            if (html.Contains("You have recieved a site-ban"))
            {
                DisplayTombstone();
            }
            else
            {
                Match csrf = Regexes.CsrfRegex.Match(html);
                if(csrf.Success)
                {
                    Csrf = csrf.Groups[1].Value;
                    Logger.Debug("Obtained CSRF token ({Csrf})", Csrf);
                }
                else
                {
                    throw new Exception("Unable to retreive CSRF token. Please check your cookie value.");
                }
            }
        }
        #endregion

        #region Helpers
        static bool IsHoneypotRaffle(string html, out string info)
        {
            info = null;

            //  The latest honeypot raffle included internal styles in the raffle message that hid the enter button so this checks for styles that modify the button.
            //  This method appears to no longer work, but keeping it in just in case.
            Match styleMatch = Regexes.HoneypotRaffleStyleRegex.Match(html);

            //  Users can only set the max entries to 100,000 while staff can go above this number.
            //  Latest honeypot raffle didn't utilize absurdly high max entries, but keeping this in because it is possible.
            Match maxEntriesMatch = Regexes.HoneypotRaffleMaxEntriesRegex.Match(html);

            //  Banned users appearing in the entries list is rare, this captures all banned user avatars
            MatchCollection bannedEntries = Regexes.HoneypotRaffleBannedUsersRegex.Matches(html);

            //  Checks for warning images used in the raffle message
            bool hasWarningImage = html.Contains("<img src=\"https://feen.us/9o0qduam.png\">") ||
                                   html.Contains("<img src=\"https://i.nikkigar.de/qk4p0ubwef.png\">");

            if (styleMatch.Success)
            {
                info = "Enter button style is modified: " + styleMatch.Groups[1].Value;
                return true;
            }
            else if (hasWarningImage)
            {
                info = "Warning image found";
                return true;
            }
            else if (bannedEntries.Count > 1)
            {
                info = $"{bannedEntries.Count} users who entered raffle are now banned";
                return true;
            }
            else if (maxEntriesMatch.Success)
            {
                if (int.TryParse(maxEntriesMatch.Groups[1].Value, out int maxEntries) && maxEntries > 100_000)
                {
                    info = $"Impossible max entries value ({maxEntries} > 100,000)";
                    return true;
                }
            }

            return false;
        }

        static void SaveSettings(Settings settings = null)
        {
            if (settings == null)
            {
                settings = Settings;
            }

            XmlWriterSettings xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = true
            };

            using (var sw = new StreamWriter(Paths.SettingsFile))
            using (var writer = XmlWriter.Create(sw, xmlSettings))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, settings);
            }
        }

        static Settings LoadSettings()
        {
            if (!File.Exists(Paths.SettingsFile))
            {
                Settings = new Settings();
                SaveSettings();

                return Settings;
            }
            else
            {
                using (var sw = new StreamReader(Paths.SettingsFile))
                {
                    var serializer = new XmlSerializer(typeof(Settings));
                    return serializer.Deserialize(sw) as Settings;
                }
            }
        }

        static void SetStatus(string status) => Console.Title = Title + (OnLatestRelease ? "" : " :: OUTDATED") + $" :: Raffles Joined this session: {RafflesJoined}" + $" :: {status}";

        static bool IsAlreadyRunning() => Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1;

        static void DisplayTombstone()
        {
            ConsoleUtils.FlashWindow(int.MaxValue, false);
            Console.Title = "R.I.P.";
            Logger.Fatal("ACCOUNT HAS BEEN BANNED");
            Logger.Fatal("You can use a different account with Scraps by changing your cookie value located in the settings file at {SettingsFolder}", Paths.SettingsFile);
            Helpers.ExitState();
        }
        #endregion
    }
}
