#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2022 Caprine Logic
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

namespace Scraps.GUI.Services;

public class WebhookService : IDisposable
{
    private readonly bool       _enabled;
    private readonly Logger     _log;
    private readonly HttpClient _http;

    public WebhookService()
    {
        _enabled = Properties.UserConfig.Default.SendWebhooks;
        _log     = LogManager.GetCurrentClassLogger();
        _http    = new HttpClient();
        _http.DefaultRequestHeaders.Add("user-agent", GlobalShared.UserAgent);
    }

    public async Task SendAsync(string content, CancellationToken cancelToken)
    {
        if (_enabled)
        {
            var payload = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("content", content)
            });

            string webhookUrl = Properties.UserConfig.Default.WebhookUrl;
            var    response   = await _http.PostAsync(webhookUrl, payload, cancelToken);
            if (response.IsSuccessStatusCode)
            {
                _log.Debug("Sent {Message} to {WebhookUrl}", content, webhookUrl);
            }
            else
            {
                _log.Error("Failed to send payload to webhook URL {WebhookUrl}: {Reason}", webhookUrl, response.ReasonPhrase);
            }
        }
    }

    public void Dispose() => _http.Dispose();

    ~WebhookService() => Dispose();
}