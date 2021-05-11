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
using System.IO;

namespace Scraps.Common.Constants
{
    public class Paths
    {
        public static readonly string StorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Caprine Logic", "Scraps");
        public static readonly string LogsPath = Path.Combine(StorePath, "Logs");
        public static readonly string DataPath = Path.Combine(StorePath, "Data");
        public static readonly string PluginsPath = Path.Combine(StorePath, "Plugins");
    }
}
