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
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;

using NLog;
using NLog.Config;
using NLog.Targets;

using Scraps.Models;
using Scraps.Services;
using Scraps.Constants;
using Scraps.Validators;

namespace Scraps
{
    class Bootstrapper
    {
        static Logger _log;
        static Config _config;
        static HttpClient _http;

        static async Task Main(string[] args)
        {
            if (IsAlreadyRunning()) Environment.Exit(0);

            Console.Title = string.Format("Scraps - {0}", Constants.Version.Full);

            Console.WriteLine();
            Console.WriteLine("Scraps - Scrap.TF Raffle Bot");
            Console.WriteLine("By depthbomb - https://s.team/p/fwc-crhc");
            Console.WriteLine("Changelog available at https://github.com/depthbomb/Scraps/blob/master/CHANGELOG.md");
            Console.WriteLine();

            EnsureFileSystem();

            ParseArguments(args);

            InitializeLogger();
            InitializeSettings();
            InitializeHttpClient();

            using (var us = new UpdateService())
            {
                await us.CheckForUpdates();
            }

            Console.CursorVisible = false;

            var bot = new Bot(_config, _http);
            await bot.LoadPluginsAsync();
            await bot.RunAsync();
        }

        static bool IsAlreadyRunning() => Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1;

        static void EnsureFileSystem()
        {
            foreach (string path in new string[]
            {
                Paths.LogsPath,
                Paths.DataPath,
                Paths.PluginsPath,
            })
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        static void ParseArguments(string[] args)
        {
            bool hasVerboseArgs = args.Contains("-v") || args.Contains("--verbose");

            Shared.Debug = hasVerboseArgs;
        }

        #region Initializers
        static void InitializeLogger()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = @"${date:format=HH\:mm\:ss} | ${pad:padding=5:inner=${level:uppercase=true}} | [${logger:shortName=true}] ${message}${exception}"
            };
            var fileTarget = new FileTarget
            {
                Layout = @"${longdate} | ${pad:padding=5:inner=${level:uppercase=true}} | [${logger:shortName=true}] ${message}${exception}",
                ArchiveEvery = FileArchivePeriod.Month,
                ArchiveFileName = "backup.{#}.zip",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveDateFormat = "yyyyMMddHHmm",
                EnableArchiveFileCompression = true,
                FileName = Path.Combine(Paths.LogsPath, "Scraps.${date:format=yyyy-MM}.log"),
                CreateDirs = true,
                MaxArchiveFiles = 5,
            };

            config.AddTarget("Console", consoleTarget);
            config.AddTarget("File", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", Shared.Debug ? LogLevel.Trace : LogLevel.Info, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;

            _log = LogManager.GetCurrentClassLogger();
        }

        static void InitializeSettings()
        {
            var cm = new ConfigManager();
            if (cm.FileExists())
            {
                _config = cm.Read();

                if (_config.Version != App.SettingsVersion)
                {
                    Console.WriteLine("Your settings file is outdated, press [Enter] to back up the old version so you may fill out the new one.");
                    Console.ReadLine();
                    cm.Save(_config, "Config_OLD");
                    cm.Save(new Config { Version = App.SettingsVersion });
                    if (!Platform.IsUnix)
                    {
                        SelectFile(Files.ConfigFile);
                    }
                    Console.WriteLine("Press [Enter] to continue.");
                    Console.ReadLine();
                    Console.WriteLine();

                    _config = cm.Read();
                }

                var validator = new ConfigValidator();
                var results = validator.Validate(_config);
                if (!results.IsValid)
                {
                    Console.WriteLine("Your config file is not valid:");
                    foreach (var error in results.Errors)
                    {
                        string message = error.ErrorMessage;
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" - {0}", message);
                        Console.ResetColor();
                    }
                    Console.WriteLine("Please fix the above errors.");
                    Console.WriteLine("Press [Enter] to exit.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            else
            {
                cm.Save(new Config { Version = App.SettingsVersion });

                Console.WriteLine("A default config file has been created at {0}", Files.ConfigFile);
                Console.WriteLine("Documentation for the settings file can be found at {0}", "https://github.com/depthbomb/Scraps/blob/master/SETTINGS.md");
                Console.WriteLine("Please fill it out and press [Enter] to continue.");
                Console.ReadLine();
                Console.WriteLine();

                _config = cm.Read();
            }
        }

        static void InitializeHttpClient()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true,
            };

            if (_config.Proxies.Length > 0)
            {
                string proxy = GetWorkingProxy();
                if (proxy == null)
                {
                    _log.Warn("Unable to find a working proxy from provided list of addresses.");
                    _log.Warn("Press [Enter] to continue without a proxy.");
                    Console.ReadLine();
                }
                else
                {
                    handler.Proxy = new WebProxy(proxy, false);
                }
            }

            var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("user-agent", Strings.UserAgent);
                client.DefaultRequestHeaders.Add("cookie", "scr_session=" + _config.Cookie);

            _http = client;
        }
        #endregion

        static void SelectFile(string path)
        {
            string args = string.Format("/e, /select, \"{0}\"", path);

            var pInfo = new ProcessStartInfo()
            {
                FileName = "explorer",
                Arguments = args
            };

            Process.Start(pInfo);
        }

        static string GetWorkingProxy()
        {
            string testUrl = "http://api.ipify.org/";
            string userIp = new HttpClient().GetStringAsync(testUrl).Result;
            string[] proxies = _config.Proxies;
            foreach (string proxy in proxies)
            {
                using (var handler = new HttpClientHandler { Proxy = new WebProxy(proxy, false) })
                using (var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(10) })
                {
                    _log.Info("Testing proxy {Proxy}...", proxy);

                    try
                    {
                        using (var testResponse = http.GetAsync(testUrl).GetAwaiter().GetResult())
                        {
                            if (testResponse.IsSuccessStatusCode)
                            {
                                string testedIp = testResponse.Content.ReadAsStringAsync().Result;
                                if (testedIp != userIp)
                                {
                                    _log.Info("Proxy {Proxy} seems to work ({Ip1} != {Ip2})", proxy, testedIp, userIp);

                                    return proxy;
                                }
                            }
                        }
                    }
                    catch {}
                }
            }

            return null;
        }
    }
}
