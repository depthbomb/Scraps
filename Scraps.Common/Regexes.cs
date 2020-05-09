using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Scraps.Common
{
    public class Regexes
    {
        public static readonly Regex CsrfRegex = new Regex(@"ScrapTF\.User\.Hash = ""([a-f0-9]{64})"";");
        public static readonly Regex RaffleEntryRegex = new Regex(@"<div class=""panel-raffle "" id=""raffle-box-([A-Z0-9]{6,})"">");
        public static readonly Regex RaffleHashRegex = new Regex(@"EnterRaffle\('[A-Z0-9]{6,}', '([a-f0-9]{64})'");
        public static readonly Regex RaffleLimitRegex = new Regex(@"total=""(\d+)"" data-max=""(\d+)");
    }
}
