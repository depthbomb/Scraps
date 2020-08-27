#region License
/// Scraps - Scrap.TF Raffle Joiner
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

        static readonly string Title = $"Scraps Raffle Joiner - {AppVersion.Full}";

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
                if (o.OpenSettings)
                {
                    Console.WriteLine("Opening settings file...");
                    Process.Start("explorer.exe", Paths.SettingsFile);
                    Environment.Exit(0);
                }
            });

            Logger = InitializeLogger();
            
            Console.WriteLine("Scraps Raffle Joiner");
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

                if (settings.Cookie.IsNullOrEmpty()) goto AskForCookie;

                Settings = settings;

                SaveSettings();
            }

            SetStatus("Initializing HTTP client...");
            Client = InitializeHttpClient();

            SetStatus("Starting operation...");
            await Start();
        }

        #region Setup
        static HttpClient InitializeHttpClient()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler()
            {
                CookieContainer = cookies,
                UseCookies = true
            };
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);
            client.DefaultRequestHeaders.Add("cookie", "scr_session=" + Settings.Cookie);

            return client;
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

                if (limits.Success)
                {
                    int num = int.Parse(limits.Groups[1].Value);
                    int max = int.Parse(limits.Groups[2].Value);
                    if (num >= max)
                    {
                        total--;
                        Logger.Information("Raffle {Id} is full ({Num}/{Max}), skipping", raffle, num, max);
                        EnteredRaffles.Add(raffle);
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
                        new KeyValuePair<string, string>("csrf", Csrf),
                    });

                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                        httpRequest.Content = content;
                        httpRequest.Headers.Referrer = new Uri("https://scrap.tf/raffles/" + raffle);

                    var response = await Client.SendAsync(httpRequest);

                    string json = await response.Content.ReadAsStringAsync();

                    var resp = JsonSerializer.Deserialize<JoinRaffleResponse>(json);

                    if (resp.success)
                    {
                        entered++;
                        Logger.Information("[{Entered}/{Total}] Joined raffle {Id}", entered, total, raffle);
                        EnteredRaffles.Add(raffle);
                        RafflesJoined++;
                    }
                    else
                    {
                        total--;
                        Logger.Warning("Failed to join raffle {Id} with message {Message}", raffle, resp.message);
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
                    Logger.Error("Paginate response for apex {Apex} returned unsuccessful", lastId ?? "<null>");
                }
            }
            catch(JsonException)
			{
                Logger.Error("Failed to read pagination data, got {String} instead of valid JSON", json);
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

            return await response.Content.ReadAsStringAsync();
        }

        static async Task GetCsrf()
        {
            string html = await Client.GetStringAsync("https://scrap.tf");
            Match csrf = Regexes.CsrfRegex.Match(html);
            if (csrf.Success)
            {
                Csrf = csrf.Groups[1].Value;
                Logger.Debug("Obtained CSRF token ({Csrf})", Csrf);
            }
            else
            {
                Console.WriteLine(html);
                throw new Exception("Unable to retreive CSRF token. Please check your cookie value.");
            }
        }
        #endregion

        #region Helpers
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

        static void SetStatus(string status) => Console.Title = Title + $" - [Raffles Joined this session: {RafflesJoined}]" + $" - [{status}]";

        static bool IsAlreadyRunning()
            => Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1;
        #endregion
    }
}
