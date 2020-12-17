#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2020  Caprine Logic

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

using System.Xml;
using System.Xml.Serialization;

namespace Scraps.Common.Models
{
	public class Settings
    {
        [XmlAnyElement("c0")]
        public XmlComment c0 { get; set; } = new XmlDocument().CreateComment("Do not modify this, it will be used in the future to make it easier for me to add new settings.");
        public int Version { get; set; }

        [XmlAnyElement("c1")]
        public XmlComment c1 { get; set; } = new XmlDocument().CreateComment("This cookie allows the bot to log in as you on Scrap.TF. This cookie will so long as *you* are still logged in. If you log out then you will need to log in again and change this value. Also, do not give this cookie to anyone!");
        public string Cookie { get; set; } = "scr_session cookie here!";

        [XmlAnyElement("c2")]
        public XmlComment c2 { get; set; } = new XmlDocument().CreateComment("This will increment the delay before rescanning by 1 second if the scan returned no available raffles.");
        public bool IncrementScanDelay { get; set; } = true;

        [XmlAnyElement("c3")]
        public XmlComment c3 { get; set; } = new XmlDocument().CreateComment("The delay between joining raffles in queue in milliseconds (1000 = 1 second). Setting this number below 4000 may result in rate limiting.");
        public int JoinDelay { get; set; } = 4000;

        [XmlAnyElement("c4")]
        public XmlComment c4 { get; set; } = new XmlDocument().CreateComment("Whether to vote for a random answer in a poll if the raffle has one.");
        public bool VoteInPolls { get; set; } = false;
    }
}
