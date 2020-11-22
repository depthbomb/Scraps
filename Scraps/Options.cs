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

using CommandLine;

namespace Scraps
{
	public class Options
	{
		[Option('v', "verbose", Required = false, HelpText = "Whether to display debug messages to the console")]
		public bool Verbose { get; set; } = false;

		[Option('c', "config", Required = false, HelpText = "Open the settings file")]
		public bool OpenSettings { get; set; } = false;

		[Option('p', "proxy", Required = false, HelpText = "Whether to use a random proxy from a proxies.txt file")]
		public bool UseProxy { get; set; } = false;
	}
}
