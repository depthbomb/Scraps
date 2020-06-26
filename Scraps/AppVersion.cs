#region License
/// Scraps - Scrap.TF Raffle Joiner
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
/// along with this program. If not, see<https://www.gnu.org/licenses/>.
#endregion License

using System;

namespace Scraps
{
    public class AppVersion
    {
        public enum ReleaseTypes
        {
            Development,
            PreRelease,
            Release
        }

        public static Version AsDotNetVersion() => new Version(Major, Minor, Patch, Hotfix);
        public static int Major => 2;
        public static int Minor => 1;
        public static int Patch => 0;
        public static int Hotfix => 0;
        public static ReleaseTypes ReleaseType => ReleaseTypes.Release;
        public static string SemVer => $"{Major}.{Minor}.{Patch}.{Hotfix}";
        public static string Full => $"{SemVer}-{ReleaseType}";
    }
}
