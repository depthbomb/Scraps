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

namespace Scraps.GUI.Services;

public class AnnouncementService : IDisposable
{
    private readonly string _fileUrl;
    private readonly HttpClient _http;

    public AnnouncementService()
    {
        _fileUrl = "https://raw.githubusercontent.com/depthbomb/Scraps/master/ANNOUNCEMENT";
        _http = new HttpClient();
        _http.Timeout = TimeSpan.FromSeconds(3);
        _http.DefaultRequestHeaders.Add("user-agent", "RaffleRunner - depthbomb/RaffleRunner");
    }

    public async Task<string> GetAnnouncementAsync()
    {
        string announcements = string.Empty;

        var request = await _http.GetAsync(_fileUrl);
        if (request.IsSuccessStatusCode)
        {
            string contents = await request.Content.ReadAsStringAsync();

            announcements = contents.Trim();
        }

        return announcements;
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}