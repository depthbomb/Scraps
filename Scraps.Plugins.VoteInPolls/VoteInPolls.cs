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
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NLog;
using NLog.Config;
using NLog.Targets;

using Scraps.Events;
using Scraps.Models;
using Scraps.Abstractions;

using Scraps.Plugins.VoteInPolls.Models;

namespace Scraps.Plugins.VoteInPolls
{
    public class VoteInPolls : PluginBase
    {
        private Logger _log;
        private Random _rng;
        private HttpClient _http;
        private RaffleRunner _runner;

        private readonly Regex _rafflePollPattern = new Regex(@"ScrapTF\.Polls\.SubmitAnswer\('([A-Z0-9]{6,})'\)");
        private readonly Regex _rafflePollOptionPattern = new Regex(@"<input name=""optionsRadios"" type=""(radio|checkbox)"" data-toggle=""(radio|checkbox)"" value=""(.*)"" id=""radio(.*)"" required>");

        private const string _logTarget = "VoteInPolls Plugin";

        public VoteInPolls(Config config, HttpClient http, RaffleRunner runner)
        {
            var target = new ColoredConsoleTarget
            {
                Layout = @"${date:format=HH\:mm\:ss} | ${pad:padding=5:inner=${level:uppercase=true}} | [${logger:shortName=true}] ${message}${exception}"
            };

            LogManager.Configuration.AddTarget(_logTarget, target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", Shared.Debug ? LogLevel.Trace : LogLevel.Info, target));

            _log = LogManager.GetLogger(_logTarget);
            _rng = new();
            _http = http;
            _runner = runner;
        }

        protected override void OnInitialized()
        {
            _runner.OnRaffleJoined += OnRaffleJoined;
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e)
        {
            string csrfToken = _runner.CsrfToken;
            string html = e.PageHtml;

            var poll = _rafflePollPattern.Match(html);
            if (poll.Success)
            {
                string pollId = poll.Groups[1].Value;

                _log.Debug("Voting in poll {Poll}", pollId);

                var optionsMatches = _rafflePollOptionPattern.Matches(html);
                if (optionsMatches.Count > 0)
                {
                    int numOptions = optionsMatches.Count;

                    _log.Debug("Found {OptionCount} option(s) in poll", optionsMatches.Count);

                    int choice = _rng.Next(0, numOptions);
                    string url = "https://scrap.tf/ajax/viewpoll/SubmitAnswer";

                    _log.Debug("Chose poll option index {Option}", choice);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("poll", pollId),
                        new KeyValuePair<string, string>("answers[]", choice.ToString()),
                        new KeyValuePair<string, string>("csrf", csrfToken),
                    });

                    var response = _http.PostAsync(url, content).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string json = response.Content.ReadAsStringAsync().Result;
                        var data = JsonSerializer.Deserialize<SubmitAnswerResponse>(json);
                        if (!data.success || data.message != "Answered!")
                        {
                            _log.Error("Poll answer failed: {Message}", data.message);
                        }

                        _log.Info("Successfully answered poll {Poll}", pollId);
                    }
                    else
                    {
                        _log.Error("Request to answer poll failed: {StatusCode}", response.StatusCode);
                    }
                }
                else
                {
                    _log.Warn("Didn't find any options for poll");
                }
            }
        }
    }
}
