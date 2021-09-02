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

namespace Scraps.Common.Constants
{
    public class Version
    {
        public enum ReleaseTypes
        {
            Development,
            PreRelease,
            Release
        }

        public static System.Version AsDotNetVersion() => new(Major, Minor, Patch, Hotfix);
        public static int Major => 4;
        public static int Minor => 2;
        public static int Patch => 5;
        public static int Hotfix => 0;
        public static ReleaseTypes ReleaseType => ReleaseTypes.Release;
        public static string SemVer => $"{Major}.{Minor}.{Patch}.{Hotfix}";
        public static string Full => $"{SemVer}-{ReleaseType}";
    }
}
