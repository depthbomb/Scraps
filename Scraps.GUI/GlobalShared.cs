#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2022 Caprine Logic
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion

namespace Scraps.GUI;

[PublicAPI]
internal static class GlobalShared
{
    #region Version

    internal enum ReleaseTypes
    {
        Development,
        PreRelease,
        Release
    }

    internal static Version      VersionAsDotNet()  => new(MajorVersion, MinorVersion, PatchVersion, HotfixVersion);
    internal static int          MajorVersion       => 4;
    internal static int          MinorVersion       => 10;
    internal static int          PatchVersion       => 0;
    internal static int          HotfixVersion      => 0;
    internal static ReleaseTypes VersionReleaseType => ReleaseTypes.Release;
    internal static string       SemVerVersion      => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{HotfixVersion}";
    internal static string       FullVersion        => $"{SemVerVersion}-{VersionReleaseType}";
    #endregion
    
    #region Strings

    internal static string MimicUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";

    #endregion
    
    #region Paths

    internal static string StorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Caprine Logic", "Scraps");
    internal static string LogsPath  = Path.Combine(StorePath, "Logs");
    internal static string DataPath  = Path.Combine(StorePath, "Data");
    internal static string[] RequiredPaths = {
        StorePath,
        LogsPath,
        DataPath
    };
    internal static string LogoPath = Path.Combine(DataPath, "Logo.png");

    #endregion
}