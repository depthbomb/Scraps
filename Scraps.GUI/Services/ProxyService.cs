#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2022 Caprine Logic

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

using Scraps.GUI.Extensions;

namespace Scraps.GUI.Services
{
    public class ProxyService
    {
        private string _localIP;

        private readonly Logger _log;
        private readonly string _testURL;

        public ProxyService()
        {
            _log = LogManager.GetCurrentClassLogger();
            _testURL = "http://api.ipify.org";
        }

        public bool HasProxies() => Properties.UserConfig.Default.Proxies.Split("\n").Length > 0;

        public (string, int) GetProxy()
        {
            var rng = new Random();
            string[] proxies = Properties.UserConfig.Default.Proxies.Split("\n");
            string randomProxy = rng.RandomArrayItem(proxies);

            return GetProxyParts(randomProxy);
        }

        public async Task<bool> TestProxyAsync(string proxyToTest)
        {
            var (address, port) = GetProxyParts(proxyToTest);

            string testIP;
            string currentIP = await GetCurrentIPAsync();

            _log.Info("Testing proxy {Proxy} against local IP {Ip}", proxyToTest, currentIP);

            var handler = new HttpClientHandler
            {
                Proxy = new WebProxy(address, port),
                UseProxy = true
            };
            using (var client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(10);

                try
                {
                    var res = await client.GetAsync(_testURL);

                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        testIP = await res.Content.ReadAsStringAsync();

                        _log.Info("Got result {Ip}", testIP);
                    }
                    else
                    {
                        _log.Info("Got HTTP status {Status}", res.StatusCode);

                        return false;
                    }
                }
                catch (TaskCanceledException)
                {
                    _log.Error("Timed out after 10 seconds");

                    return false;
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);

                    return false;
                }
            }

            return testIP != currentIP;
        }

        private async Task<string> GetCurrentIPAsync()
        {
            if (_localIP == null)
            {
                _log.Debug("Retrieving local IP");
                using (var client = new HttpClient())
                {
                    _localIP = await client.GetStringAsync(_testURL);
                }
            }

            return _localIP;
        }

        private (string, int) GetProxyParts(string proxy)
        {
            var split = proxy.Trim().Split(":");

            string address = split[0];
            int port = int.Parse(split[1]);

            return (address, port);
        }
    }
}
