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

using System.Text.RegularExpressions;

using Scraps.Constants;
using Scraps.Interfaces;

namespace Scraps.Services.Impl
{
    public class ButtonStyleCheck : IHoneypotCheck
    {
        private string _html;

        public bool Detected { get; set; }
        public string DetectReason { get; set; }

        public ButtonStyleCheck(string html)
        {
            _html = html;
            Detected = false;
        }

        public void Check()
        {
            // The latest honeypot raffle included internal styles in the raffle message that hid the enter button so this checks for styles that modify the button.
            // This method appears to no longer work, but keeping it in just in case.
            Match styleMatch = RegexPatterns.HoneypotRaffleStyleRegex.Match(_html);

            Detected = styleMatch.Success;
            DetectReason = string.Format("Enter button style is modified: {0}", styleMatch.Groups[1].Value);
        }
    }
}
