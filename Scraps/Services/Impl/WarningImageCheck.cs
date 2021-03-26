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
    public class WarningImageCheck : IHoneypotCheck
    {
        private string _html;

        public bool Detected { get; set; }
        public string DetectReason { get; set; }

        public WarningImageCheck(string html)
        {
            _html = html;
            Detected = false;
        }

        public void Check()
        {
            string[] warningImages = new string[]
            {
                "<img src=\"https://i.nikkigar.de",
            };

            foreach (string image in warningImages)
            {
                if (_html.Contains(image))
                {
                    Detected = true;
                    DetectReason = string.Format("Found warning image: {0}", image);
                    break;
                }
            }
        }
    }
}
