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
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

using Scraps.Events;
using Scraps.Abstractions;

using NLog;

using Scraps.Plugins.CommentOnRaffles.Models;

using Config = Scraps.Models.Config;
using PluginConfig = Scraps.Plugins.CommentOnRaffles.Models.Config;

namespace Scraps.Plugins.CommentOnRaffles
{
    public class CommentOnRaffles : PluginBase
    {
        public string Name = "CommentOnRaffles";
        public Version Version = new Version(1, 0, 0, 0);

        private Logger _log;
        private Random _rng;
        private HttpClient _http;
        private RaffleRunner _runner;
        private List<string> _comments = new();

        private PluginConfig _config;
        private List<string> _previousComments = new();
        private DateTime _nextComment = DateTime.UtcNow;
        private string _pluginConfigFile = Path.Combine(Constants.Paths.StorePath, "CommentOnRaffles.json");
        private string _acceptRulesRule = Path.Combine(Constants.Paths.DataPath, "RulesAccepted");

        public CommentOnRaffles(Config config, HttpClient http, RaffleRunner runner)
        {
            _log = LogManager.GetCurrentClassLogger();
            _rng = new();
            _http = http;
            _runner = runner;
        }

        protected override void OnInitialized()
        {
            // This is a simple file we check for to see if we have accepted the site rules in order to post comments.
            if (!File.Exists(_acceptRulesRule))
            {
                _log.Info("Accepting site rules in order to comment");

                string url = "https://scrap.tf/ajax/rules/Accept";
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("csrf", _runner.CsrfToken),
                });
                var response = _http.PostAsync(url, content).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var data = JsonSerializer.Deserialize<AcceptRulesResponse>(json);
                    if (data.success == true)
                    {
                        _log.Debug("Accepted site rules");

                        File.Create(_acceptRulesRule).Dispose();
                    }
                }
                else
                {
                    _log.Error("Request to accept rules failed: {StatusCode}", response.ReasonPhrase);
                }
            }

            if (!File.Exists(_pluginConfigFile))
            {
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                };
                string json = JsonSerializer.Serialize(new PluginConfig(), options);
                File.WriteAllText(_pluginConfigFile, json);

                _log.Warn("A default config for this plugin has been created at {Path}", _pluginConfigFile);
                _log.Warn("Please modify it to your needs and restart Scraps.");

                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                using (var fs = new StreamReader(_pluginConfigFile))
                {
                    string json = fs.ReadToEnd();
                    var pluginConfig = JsonSerializer.Deserialize<PluginConfig>(json);

                    _config = pluginConfig;
                }

                if (_config.PreviousCommentsToExclude > _config.Comments.Length)
                {
                    _log.Fatal("PreviousCommentsToExclude must be less than the number of Comments");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }

            _comments.AddRange(_config.Comments);

            _runner.OnRaffleJoined += OnRaffleJoined;
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e)
        {
            string csrfToken = _runner.CsrfToken;
            string raffleId = e.RaffleId;
            var now = DateTime.UtcNow;
            if (now >= _nextComment)
            {
                int maxPreviousComments = _config.PreviousCommentsToExclude;
                bool search = true;
                string comment = null;
                // Loop until we get a random comment we haven't previously posted 
                do
                {
                    // Choose a comment at random
                    string randomComment = _config.Comments[_rng.Next(_config.Comments.Length)];

                    // Check if the previous comments contains the one we chose at random
                    if (!_previousComments.Contains(randomComment))
                    {
                        // Break the loop
                        search = false;

                        comment = randomComment;
                    }

                    // Check if the number of previous comments is equal to our max allowed
                    if (_previousComments.Count == maxPreviousComments)
                    {
                        // Remove the first item
                        _previousComments.RemoveAt(0);

                        // Add our new random comment
                        _previousComments.Add(randomComment);
                    }
                } while (search);

                string url = "https://scrap.tf/ajax/comments/Post";
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("id", raffleId),
                    new KeyValuePair<string, string>("objType", "Raffle"),
                    new KeyValuePair<string, string>("comment", comment),
                    new KeyValuePair<string, string>("csrf", csrfToken),
                });
                var response = _http.PostAsync(url, content).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var data = JsonSerializer.Deserialize<CommentsPostResponse>(json);
                    if (data.success)
                    {
                        _log.Debug("Post comment {Comment} on raffle {RaffleId}", comment, raffleId);

                        _nextComment = DateTime.UtcNow.AddMinutes(_config.PostDelayMinutes);

                        _log.Debug("Commenting again at {Date}", _nextComment);
                    }
                }
                else
                {
                    _log.Error("Request to post comment failed: {StatusCode}", response.ReasonPhrase);
                }
            }
        }
    }
}
