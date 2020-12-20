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
	public class Delays
	{
        [XmlAnyElement("c1")]
        public XmlComment c1 { get; set; } = new XmlDocument().CreateComment("This will increment the delay before rescanning by 1 second if the scan returned no available raffles.");
        public bool IncrementScanDelay { get; set; } = true;

        [XmlAnyElement("c2")]
        public XmlComment c2 { get; set; } = new XmlDocument().CreateComment("Delay between starting a new scan in milliseconds (1000 = 1 second). If IncrementScanDelay is enabled then this value gets incremented by 1000 each time a scan returns no available raffles. The value is reset value upon finding available raffles.");
        public int ScanDelay { get; set; } = 5000;

        [XmlAnyElement("c3")]
        public XmlComment c3 { get; set; } = new XmlDocument().CreateComment("Delay between paginating the raffles listing in milliseconds. Lowering this below the default (500) may result in you getting the \"You are refreshing this page too much\" error. However this DOES NOT affect operation, but it may make you more suspicious");
        public int PaginateDelay { get; set; } = 500;

        [XmlAnyElement("c4")]
        public XmlComment c4 { get; set; } = new XmlDocument().CreateComment("The delay between joining raffles in queue in milliseconds. Setting this number below 4000 may result in rate limiting.");
        public int JoinDelay { get; set; } = 4000;
    }
}
