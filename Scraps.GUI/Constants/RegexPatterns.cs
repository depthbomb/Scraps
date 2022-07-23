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

using System.Text.RegularExpressions;

namespace Scraps.GUI.Constants
{
    public class RegexPatterns
    {
        public static readonly Regex CSRF = new(@"ScrapTF\.User\.Hash = ""([a-f0-9]{64})"";");
        public static readonly Regex RAFFLE_ENTRY = new(@"ScrapTF\.Raffles\.RedirectToRaffle\('([A-Z0-9]{6,})'\)", RegexOptions.Compiled);
        public static readonly Regex RAFFLE_HASH = new(@"EnterRaffle\('[A-Z0-9]{6,}', '([a-f0-9]{64})'", RegexOptions.Compiled);
        public static readonly Regex RAFFLE_LIMIT = new(@"total=""(\d+)"" data-max=""(\d+)", RegexOptions.Compiled);
        public static readonly Regex WON_RAFFLES_ALERT = new(@"You've won \d raffles? that must be withdrawn", RegexOptions.Compiled);
        public static readonly Regex BAN_REASON = new(@"<b>Reason:<\/b> ([\w\s]+)");
        public static readonly Regex HONEYPOT_RAFFLE_STYLE = new(@"<style>\.enter-raffle-btns \.btn{(.*)}<\/style>", RegexOptions.Compiled);
        public static readonly Regex HONEYPOT_RAFFLE_MAX_ENTRIES = new(@"data-max=""(\d{1,})""", RegexOptions.Compiled);
        public static readonly Regex HONEYPOT_RAFFLE_BANNED_USERS = new(@"<img class='tiny-raffle-avatar\s?' style='border-color:\s?#CC1100;?' src='(.*)' loading=""lazy""\s?\/>", RegexOptions.Compiled);
    }
}
