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

using System;

namespace Scraps.Models
{
    [Serializable]
    public class Config
    {
        public int Version { get; set; }
        public string Cookie { get; set; } = "scr_session cookie here!";
        public bool EnableToastNotifications { get; set; } = true;
        public bool Paranoid { get; set; } = true;
        public Delays Delays { get; set; } = new Delays();
        public RaffleActions RaffleActions { get; set; } = new RaffleActions();
        public string[] Proxies { get; set; } = new string[] { };
    }

    [Serializable]
    public class Delays
    {
        public bool IncrementScanDelay { get; set; } = true;
        public int ScanDelay { get; set; } = 5000;
        public int PaginateDelay { get; set; } = 500;
        public int JoinDelay { get; set; } = 4000;
    }

    [Serializable]
    public class RaffleActions
    {
        public bool VoteInPolls { get; set; } = false;
    }
}
