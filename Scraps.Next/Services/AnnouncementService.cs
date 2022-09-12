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

using Scraps.Next.Events;

namespace Scraps.Next.Services;

public class AnnouncementService : IDisposable
{
    /// <summary>
    /// Raised when an announcement is received as a result of fetching.
    /// </summary>
    public event EventHandler<AnnouncementServiceAnnouncementReceivedArgs> OnAnnouncementReceived; 

    private readonly Logger       _log;
    private readonly HttpClient   _http;
    private readonly List<string> _announcements;

    public AnnouncementService()
    {
        _log          = LogManager.GetCurrentClassLogger();
        _http         = new HttpClient();
        _http.Timeout = TimeSpan.FromSeconds(3);
        _http.DefaultRequestHeaders.Add("user-agent", GlobalShared.UserAgent);

        _announcements = new List<string>();
    }
    
    ~AnnouncementService() => Dispose();
    
    public void Dispose() => _http?.Dispose();

    public List<string> GetAnnouncements() => _announcements;

    public async Task FetchAnnouncementsAsync()
    {
        _log.Debug("Fetching announcements");
        
        try
        {
            var res = await _http.GetAsync(GlobalShared.AnnouncementFileUrl);
            if (res.IsSuccessStatusCode)
            {
                string contents = await res.Content.ReadAsStringAsync();

                var announcementLines = contents.Split("\n").ToList();
                
                // Iterate through the announcements instead of adding them as a range so we can raise an event per announcement.
                foreach (var announcement in announcementLines.Where(announcement => !_announcements.Contains(announcement)))
                {
                    _announcements.Add(announcement);
                        
                    OnAnnouncementReceived?.Invoke(this, new AnnouncementServiceAnnouncementReceivedArgs(announcement));
                }
            }
            else
            {
                _log.Error("Failed to fetch announcements: {Reason}", res.ReasonPhrase);
            }
        }
        catch(Exception ex)
        {
            _log.Error(ex, "Failed to fetch announcements");
        }
    }
}