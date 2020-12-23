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
using System.Timers;
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
using DiscordRPC;

using Scraps.Models;
using Scraps.Common;
using Scraps.Common.Models;

using CaprineNet.Common.Utils;
using CaprineNet.Common.Extensions;

namespace Scraps
{
    class Program
    {
        static string Csrf;
        static Logger Logger;
        static Timer RpcTimer;
        static HttpClient Client;
        static Settings Settings;
        static DiscordRpcClient RpcClient;
        static Random Rng = new Random();
        static HtmlDocument Html = new HtmlDocument();

        static int SettingsVersion = 4;
        static int RafflesJoined = 0;
        static bool AcceptedRules = false;
        static List<string> RaffleQueue = new List<string>();
        static List<string> EnteredRaffles = new List<string>();

        static bool Verbose = false;
        static bool OnLatestRelease = true;
        static bool AlertedOfWonRaffles = false;

        static readonly string Title = $"Scraps - {AppVersion.Full}";

        static async Task Main(string[] args)
        {
            if(IsAlreadyRunning())
            {
                Environment.Exit(0);
            }

            ParseArguments(args);

            await CheckForNewReleases();

            Logger = InitializeLogger();
            Logger.Debug("Session Started ({Version})", AppVersion.Full);

            LoadSettings();

            Console.WriteLine();
            Console.WriteLine("Scraps - Scrap.TF Raffle Bot");
            Console.WriteLine("By depthbomb - https://s.team/p/fwc-crhc");
            Console.WriteLine("Changelog available at https://github.com/depthbomb/Scraps/blob/master/CHANGELOG.md");
            Console.WriteLine();

            if(Settings.Version != SettingsVersion)
            {
                Console.WriteLine("Your settings file is outdated, press [Enter] to back up the old version so you may fill out the new one.");
                Console.ReadLine();
                SaveSettings(null, "Settings_OLD");
                SaveSettings(new Settings { Version = SettingsVersion });
                IOUtils.OpenFolderAndSelect(Paths.SettingsFile);
                Console.WriteLine("Press [Enter] to restart Scraps when you're done.");
                Console.ReadLine();

                LoadSettings();
            }

            if (Settings.Cookie.IsNullOrEmpty() || Settings.Cookie.Contains("cookie"))
            {
                Logger.Debug("User prompted to edit settings");

                Console.WriteLine("Your cookie value is missing from your settings file.");
                Console.WriteLine("Press [Enter] to open your settings file.");
                Console.ReadLine();
                IOUtils.OpenFolderAndSelect(Paths.SettingsFile);
                Console.WriteLine("Press [Enter] when you are done making modifications to your settings file.");
                Console.ReadLine();

                LoadSettings();
            }

            Client = InitializeHttpClient();

            LoadStats();
            
            if (Settings.EnableDiscordRichPresensce)
            {
                InitializeRichPresence();
            }

            await Start();
        }

        #region Setup
        static void LoadSettings()
        {
            if(!Directory.Exists(Paths.StorePath))
            {
                Directory.CreateDirectory(Paths.StorePath);
            }

            if(!File.Exists(Paths.SettingsFile))
            {
                Settings = new Settings();
                SaveSettings();
            }
            else
            {
                using(var sw = new StreamReader(Paths.SettingsFile))
                {
                    var serializer = new XmlSerializer(typeof(Settings));
                    Settings = serializer.Deserialize(sw) as Settings;
                }
            }
        }

        static void ParseArguments(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                Verbose = o.Verbose;
                if(o.OpenSettings)
                {
                    Console.WriteLine("Opening settings file...");
                    Process.Start("explorer.exe", Paths.SettingsFile);
                    Environment.Exit(0);
                }
            });
        }

        static HttpClient InitializeHttpClient()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
            };

            var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);
                client.DefaultRequestHeaders.Add("cookie", "scr_session=" + Settings.Cookie);

            return client;
        }

        static async Task CheckForNewReleases()
        {
            Console.WriteLine("Checking for new releases...");

            var client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);

            string uri = "https://api.github.com/repos/depthbomb/scraps/releases/latest";
            string json = await client.GetStringAsync(uri);
            var currentTag = new Version(AppVersion.SemVer);

            var release = JsonSerializer.Deserialize<LatestRelease>(json);
            var latestTag = new Version(release.tag_name.Replace("v", ""));   //  In case I'm inconsistent in tagging

            var compare = currentTag.CompareTo(latestTag);
            if (compare < 0)
            {
                OnLatestRelease = false;
                ConsoleUtils.Restore();
                ConsoleUtils.FlashWindow(5, true);
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("A new version of Scraps ({0}) is available to download at {1}", latestTag.ToString(), release.html_url);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("You are running the latest version of Scraps!");
            }

            client.Dispose();
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

        static void InitializeRichPresence()
        {
            Logger.Debug("Initializing Rich Presence");
            var start = DateTime.UtcNow;
            string getJoined()
            {
                return $"{RafflesJoined} "+"raffle".Pluralize(RafflesJoined)+" joined!";
            };
            string details = "Scrap.TF Raffle Bot";
            var timestamps = new Timestamps
            {
                Start = start
            };
            var assets = new Assets
            {
                LargeImageKey = "scraps",
                LargeImageText = "github.com/depthbomb/scraps"
            };
            RpcTimer = new Timer
            {
                Interval = TimeSpan.FromSeconds(7.5).TotalMilliseconds, // Presence updates are only updated every 15 seconds, but send more frequently so we ensure the latest data is available
                Enabled = false
            };

            RpcClient = new DiscordRpcClient("789333681998397460");
            RpcClient.Initialize();
            RpcClient.SetPresence(new RichPresence
            {
                Details = details,
                State = getJoined(),
                Timestamps = timestamps,
                Assets = assets
            });

            RpcTimer.Elapsed += (sender, e) =>
            {
                RpcClient.SetPresence(new RichPresence
                {
                    Details = details,
                    State = getJoined(),
                    Timestamps = timestamps,
                    Assets = assets
                });
            };
            RpcTimer.Start();
            Logger.Debug("Rich Presence update timer started");
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
            catch(Exception e)
            {
                Logger.Fatal(e.Message);
                Helpers.ExitState();
            }

            ScanRaffles:

            await ScanRaffles();

            if (RaffleQueue.Count > 0)
            {
                SetStatus("Joining raffles...");
                Logger.Information("{Count} "+"raffle".Pluralize(RaffleQueue.Count)+" "+"is".Pluralize(RaffleQueue.Count, "are")+" available to enter", RaffleQueue.Count);

                scanDelay = Settings.Delays.ScanDelay;

                await JoinRaffles();
            }
            else
            {
                SetStatus("Waiting to scan...");
                Logger.Debug("All raffles have been entered, scanning again after {Delay} seconds", (scanDelay / 1000));

                await Task.Delay(scanDelay);

                if(Settings.Delays.IncrementScanDelay)
                {
                    scanDelay = scanDelay + 1000;
                }
            }

            SaveStats();

            goto ScanRaffles;
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

            if(json == null)
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
                            await Task.Delay(Settings.Delays.PaginateDelay);
                            goto ScanNext;
                        }

                        Logger.Debug("Done scanning all raffles, grabbing IDs of un-entered raffles...");

                        SetStatus("Parsing scanned data...");

                        Html.LoadHtml(html);

                        var document = Html.DocumentNode;
                        var raffleElements = document.SelectNodes(Xpaths.UnenteredRaffles);

                        if(html.Contains("ScrapTF.Raffles.WithdrawRaffle"))
                        {
                            if(!AlertedOfWonRaffles)
                            {
                                Logger.Information("{Message}", "There won raffles that you need to withdraw!");
                                ConsoleUtils.FlashWindow(10, false);
                                ConsoleUtils.Restore();
                                AlertedOfWonRaffles = true;
                            }
                        }
                        else
                        {
                            AlertedOfWonRaffles = false;
                        }

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

            if(response.StatusCode.ToString() == "OK")
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Logger.Warning("Pagination returned a {Status} response instead of JSON. Waiting...", response.StatusCode);
                return null;
            }
        }

        static async Task JoinRaffles()
        {
            int entered = 0;
            int total = RaffleQueue.Count;
            int joinDelay = Settings.Delays.JoinDelay;
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

                            if (Settings.RaffleActions.VoteInPolls)
                            {
                                var poll = Regexes.RafflePollRegex.Match(html);
                                if(poll.Success)
                                {
                                    string pollId = poll.Groups[1].Value;

                                    Logger.Debug("Voting in poll {Poll}", pollId);

                                    var optionsMatches = Regexes.RafflePollOptionRegex.Matches(html);
                                    if (optionsMatches.Count > 0)
                                    {
                                        Logger.Debug("Found {OptionCount} "+"option".Pluralize(optionsMatches.Count) +" in poll", optionsMatches.Count);
                                        await AnswerPoll(pollId, optionsMatches.Count);
                                    }
                                    else
                                    {
                                        Logger.Warning("Didn't find any options in poll");
                                    }
                                }
                            }
                        }
                        else
                        {
                            total--;
                            Logger.Information("Unable to join raffle {Id} with message {Message}", raffle, resp.message);
                        }

                        SaveStats();

                        if (RaffleQueue.Count > 1)
                        {
                            SetStatus("Waiting to join next raffle...");
                            await Task.Delay(joinDelay);
                        }
                    }
                    else
                    {
                        total--;
                        Logger.Warning("Could not obtain hash from raffle {Id}", raffle);
                    }
                }
            }
        }

        static async Task AnswerPoll(string poll, int numChoices)
        {
            SetStatus($"Voting in poll {poll}");

            string url = "https://scrap.tf/ajax/viewpoll/SubmitAnswer";
            int choice = Rng.Next(0, numChoices);

            Logger.Debug("Randomly chose poll option {Option}", choice + 1);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("poll", poll),
                new KeyValuePair<string, string>("answers[]", choice.ToString()),
                new KeyValuePair<string, string>("csrf", Csrf),
            });

            var response = await Client.PostAsync(url, content);

            if(response.StatusCode.ToString() == "OK")
            {
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<SubmitAnswerResponse>(json);
                if (!data.success || data.message != "Answered!")
                {
                    Logger.Error("Poll answer failed: {Message}", data.message);
                }
                else
                {
                    Logger.Debug("Successfully answered poll {Poll}", poll);
                }
            }
            else
            {
                Logger.Error("Request to answer poll failed: {StatusCode}", response.StatusCode);
            }
        }

        static async Task AcceptRules()
        {
            Logger.Debug("Attempting to accept site rules...");

            string url = "https://scrap.tf/ajax/rules/Accept";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("csrf", Csrf)
            });

            var response = await Client.PostAsync(url, content);

            if(response.StatusCode.ToString() == "OK")
            {
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<AcceptRulesResponse>(json);
                if(data.success == "true")
                {
                    AcceptedRules = true;
                    Logger.Information("Accepted site rules");
                }
                else
                {
                    Logger.Error("Failed to accept site rules: {Message}", data.message);
                }
            }
            else
            {
                Logger.Error("Request to accept site rules failed: {StatusCode}", response.StatusCode);
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
                    if(!AcceptedRules)
                    {
                        await AcceptRules();
                    }
                    SaveStats();
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

        static void SaveSettings(Settings settings = null, string fileName = "Settings")
        {
            if (settings == null)
            {
                settings = Settings;
            }

            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = true
            };

            string settingsFile = Paths.SettingsFile.Replace("Settings.xml", $"{fileName}.xml");

            using (var sw = new StreamWriter(settingsFile))
            using (var writer = XmlWriter.Create(sw, xmlSettings))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, settings);
            }
        }

        static void SetStatus(string status) => Console.Title = Title + (OnLatestRelease ? "" : " — OUTDATED") + $" — {RafflesJoined} {"Raffle".Pluralize(RafflesJoined)} Joined" + $" — {status}";

        static bool IsAlreadyRunning() => Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1;

        static void DisplayTombstone()
        {
            if(Settings.EnableDiscordRichPresensce)
            {
                RpcTimer.Stop();
                RpcClient.SetPresence(new RichPresence
                {
                    Details = "Account Banned",
                    Assets = new Assets
                    {
                        LargeImageKey = "banned",
                        LargeImageText = "R.I.P."
                    }
                });
            }
            ConsoleUtils.FlashWindow(int.MaxValue, false);
            Console.Title = "R.I.P.";
            Logger.Fatal("ACCOUNT HAS BEEN BANNED");
            Logger.Fatal("You can use a different account with Scraps by changing your cookie value located in the settings file at {SettingsFolder}", Paths.SettingsFile);
            Helpers.ExitState();
        }

        static void LoadStats()
        {
            var stats = Properties.Stats.Default;

            if(stats.UpgradeRequired)
            {
                stats.Upgrade();
                stats.UpgradeRequired = false;
                stats.Save();
            }

            if(!stats.Activated)
            {
                Logger.Debug("New user stats file is inactive, activating...");
                string statsFile = Paths.StatsFile;
                if(File.Exists(statsFile))
                {
                    Logger.Debug("User has legacy stats file, importing stats...");
                    using(var sw = new StreamReader(statsFile))
                    {
                        var serializer = new XmlSerializer(typeof(Stats));
                        var data = serializer.Deserialize(sw) as Stats;

                        stats.AcceptedRules = data.AcceptedRules;
                        stats.TotalRafflesJoined = data.TotalRafflesJoined;
                    }
                }
                else
                {
                    Logger.Debug("Setting default stats...");
                    stats.AcceptedRules = false;
                    stats.TotalRafflesJoined = 0;
                }

                stats.Activated = true;
                stats.Save();
            }

            AcceptedRules = stats.AcceptedRules;
            RafflesJoined = stats.TotalRafflesJoined;
        }

        static void SaveStats()
        {
            SetStatus("Saving stats...");

            var settings = Properties.Stats.Default;
                settings.AcceptedRules = AcceptedRules;
                settings.TotalRafflesJoined = RafflesJoined;
                settings.Save();
        }
        #endregion
    }
}
