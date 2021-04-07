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

using Scraps.Events;
using Scraps.Models;
using Scraps.Abtract;

using Scraps.Plugins.VoteInPolls.Models;

namespace Scraps.Plugins.VoteInPolls
{
    public class VoteInPolls : PluginBase
    {
        public string Name = "VoteInPolls";
        public Version Version = new Version(1, 0, 0, 0);

        private Logger _log;
        private Random _rng;
        private HttpClient _http;
        private BotManager _manager;

        private readonly Regex _rafflePollPattern = new Regex(@"ScrapTF\.Polls\.SubmitAnswer\('([A-Z0-9]{6,})'\)");
        private readonly Regex _rafflePollOptionPattern = new Regex(@"<input name=""optionsRadios"" type=""(radio|checkbox)"" data-toggle=""(radio|checkbox)"" value=""(.*)"" id=""radio(.*)"" required>");

        public VoteInPolls(Config config, HttpClient http, BotManager manager)
        {
            _log = LogManager.GetCurrentClassLogger();
            _rng = new();
            _http = http;
            _manager = manager;
        }

        protected override void OnInitialized()
        {
            _manager.OnRaffleJoined += OnRaffleJoined;
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e)
        {
            string csrfToken = _manager.CsrfToken;
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
