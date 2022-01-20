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

using Scraps.Common.Constants;

namespace Scraps.GUI.Honeypot.Checks
{
    public class BannedEntriesCheck : IHoneypotCheck
    {
        private readonly string _html;

        public bool Detected { get; set; } = false;
        public string DetectReason { get; set; }

        /// <summary>
        /// Checks the raffle page HTML for signs of banned user entries.
        /// </summary>
        /// <param name="html">Raffle page HTML</param>
        public BannedEntriesCheck(string html)
        {
            _html = html;
        }

        public void Check()
        {
            var entries = RegexPatterns.HONEYPOT_RAFFLE_BANNED_USERS.Matches(_html);

            if (entries.Count > 1)
            {
                Detected = true;
                DetectReason = string.Format("{0} users who entered this raffle are now banned", entries.Count);
            }
        }
    }
}
