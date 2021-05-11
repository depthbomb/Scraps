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

namespace Scraps.Common.Constants
{
    public class RegexPatterns
    {
        public static readonly Regex CsrfRegex = new(@"ScrapTF\.User\.Hash = ""([a-f0-9]{64})"";");
        public static readonly Regex RaffleEntryRegex = new(@"ScrapTF\.Raffles\.RedirectToRaffle\('([A-Z0-9]{6,})'\)");
        public static readonly Regex RaffleHashRegex = new(@"EnterRaffle\('[A-Z0-9]{6,}', '([a-f0-9]{64})'");
        public static readonly Regex RaffleLimitRegex = new(@"total=""(\d+)"" data-max=""(\d+)");
        public static readonly Regex RaffleQualityRegex = new(@"class=""item hoverable (?:quality(\d{1,2}))?\s*(?:rarity([a-z]{3,}))?\s*app\d{3,} "" style");
        public static readonly Regex RaffleWithdrawRegex = new(@"ScrapTF\.Raffles\.WithdrawRaffle\('([A-Z0-9]{6,})'\)");
        public static readonly Regex HoneypotRaffleStyleRegex = new(@"<style>\.enter-raffle-btns \.btn{(.*)}<\/style>");
        public static readonly Regex HoneypotRaffleMaxEntriesRegex = new(@"data-max=""(\d{1,})""");
        public static readonly Regex HoneypotRaffleBannedUsersRegex = new(@"<img class='tiny-raffle-avatar\s?' style='border-color:\s?#CC1100;?' src='(.*)' loading=""lazy""\s?\/>");
    }
}
