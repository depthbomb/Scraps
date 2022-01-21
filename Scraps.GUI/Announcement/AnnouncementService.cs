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

namespace Scraps.GUI.Announcement
{
    public class AnnouncementService
    {
        private const string FILE_URL = "https://raw.githubusercontent.com/depthbomb/Scraps/master/ANNOUNCEMENT";

        public async Task<string> GetAnnouncementAsync()
        {
            string announcement = "";
            using (var http = new HttpClient())
            {
                http.Timeout = TimeSpan.FromSeconds(3);
                http.DefaultRequestHeaders.Add("user-agent", "Scraps - depthbomb/Scraps");

                var request = await http.GetAsync(FILE_URL);
                if (request.IsSuccessStatusCode)
                {
                    string contents = await request.Content.ReadAsStringAsync();

                    announcement = contents.Trim();
                }
            }

            return announcement;
        }
    }
}
