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
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using NLog;
using NLog.Config;
using NLog.Targets;

using Scraps.Events;
using Scraps.Models;
using Scraps.Abstractions;

using Scraps.Plugins.ApiServer.Models;

namespace Scraps.Plugins.ApiServer
{
    public class ApiServer : PluginBase
    {
        private Logger _log;        
        private Thread _thread;
        private RaffleRunner _runner;
        private HttpListener _listener;

        private int _rafflesJoined = 0;
        private int _rafflesWon = 0;
        private bool _listening = false;

        private readonly int _port;

        private const string _logTarget = "ApiServer Plugin";

        public ApiServer(Config config, HttpClient http, RaffleRunner runner)
        {
            var target = new ColoredConsoleTarget
            {
                Layout = @"${date:format=HH\:mm\:ss} | ${pad:padding=5:inner=${level:uppercase=true}} | [${logger:shortName=true}] ${message}${exception}"
            };

            LogManager.Configuration.AddTarget(_logTarget, target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", Shared.Debug ? LogLevel.Trace : LogLevel.Info, target));

            _log = LogManager.GetLogger(_logTarget);
            _runner = runner;
            _port = 19318;
        }

        protected override void OnInitialized()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{_port}/");

            _thread = new Thread(ListenAsync);
            _thread.Start();

            _runner.OnRaffleJoined += OnRaffleJoined;
            _runner.OnRafflesWon += OnRafflesWon;
        }

        private void OnRaffleJoined(object sender, RaffleJoinedArgs e) => _rafflesJoined = e.Entered;

        private void OnRafflesWon(object sender, RafflesWonArgs e) => _rafflesWon = e.RaffleIds.Count;

        private async void ListenAsync()
        {
            _listener.Start();
            _listening = true;

            _log.Info("Listening to localhost on port {Port}", _port);

            while (_listening)
            {
                try
                {
                    var ctx = await _listener.GetContextAsync();

                    await ProcessRequestAsync(ctx);
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext ctx)
        {
            _log.Debug("Processing {Verb} request {Url}", ctx.Request.HttpMethod, ctx.Request.RawUrl);

            string endpoint = ctx.Request.Url.LocalPath;
            Response res;
            switch (endpoint)
            {
                case "/Stats":
                    res = CreateResponse(new StatsResponse
                    {
                        RafflesJoined = _rafflesJoined,
                        OutstandingRafflesWon = _rafflesWon,
                        StartTime = Bot.StartTime,
                    });
                    break;
                default:
                    ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    res = CreateResponse(null, false, "Invalid endpoint");
                    break;
            }

            ctx.Response.ContentType = "application/json; charset=utf-8";

            await JsonResponse(res, ctx.Response);

            ctx.Response.Close();
        }

        private Task JsonResponse(object value, HttpListenerResponse response)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(value, options);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using (var ms = new MemoryStream(bytes))
            {
                return ms.CopyToAsync(response.OutputStream);
            }
        }

        private Response CreateResponse(object child, bool success = true, string message = null)
        {
            return new Response
            {
                Success = success,
                Message = message,
                Results = child
            };
        }
    }
}
