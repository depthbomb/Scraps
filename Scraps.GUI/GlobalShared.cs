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
public static class GlobalShared
{
    #region Version

    public enum ReleaseTypes
    {
        Development,
        PreRelease,
        Release
    }

    public static Version      VersionAsDotNet()  => new(MajorVersion, MinorVersion, PatchVersion, HotfixVersion);
    public static int          MajorVersion       => 4;
    public static int          MinorVersion       => 11;
    public static int          PatchVersion       => 0;
    public static int          HotfixVersion      => 0;
    public static ReleaseTypes VersionReleaseType => ReleaseTypes.Release;
    public static string       SemVerVersion      => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{HotfixVersion}";
    public static string       FullVersion        => $"{SemVerVersion}-{VersionReleaseType}";
    #endregion
    
    #region Strings

    public static string UserAgent      = $"Scraps {FullVersion}";
    public static string MimicUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";

    #endregion
    
    #region Paths

    public static string StorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Caprine Logic", "Scraps");
    public static string LogsPath  = Path.Combine(StorePath, "Logs");
    public static string DataPath  = Path.Combine(StorePath, "Data");
    public static string[] RequiredPaths = {
        StorePath,
        LogsPath,
        DataPath
    };
    public static string LogoPath = Path.Combine(DataPath, "Logo.png");

    #endregion
}