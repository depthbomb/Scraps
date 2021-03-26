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

using Scraps.Interfaces;
using Scraps.Services.Impl;

namespace Scraps.Services
{
    public class HoneypotChecker
    {
        private string _html;

        public bool IsHoneypot { get; set; }
        public string Reason { get; set; }

        public HoneypotChecker(string html)
        {
            _html = html;
        }

        public void Check()
        {
            var checks = new IHoneypotCheck[]
            {
                new ButtonStyleCheck(_html),
                new MaxEntriesCheck(_html),
                new BannedEntriesCheck(_html),
                new WarningImageCheck(_html),
            };

            foreach (var check in checks)
            {
                // Run each implementation's check method and break on the first detection
                check.Check();
                if (check.Detected)
                {
                    IsHoneypot = true;
                    Reason = check.DetectReason;
                    break;
                }
            }
        }
    }
}
