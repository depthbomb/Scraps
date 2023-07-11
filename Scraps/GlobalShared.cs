#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2021 Caprine Logic
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

namespace Scraps;

public static class GlobalShared
{
    public static readonly uint WM_SCRAPSNEXT_SHOWME = Native.RegisterWindowMessage("WM_SCRAPSNEXT_SHOWME");

    #region Version
    public enum ReleaseTypes
    {
        Development,
        PreRelease,
        Release
    }

    public static Version      VersionAsDotNet() => new(MajorVersion, MinorVersion, PatchVersion, HotfixVersion);
    public const  int          MajorVersion       = 5;
    public const  int          MinorVersion       = 3;
    public const  int          PatchVersion       = 5;
    public const  int          HotfixVersion      = 0;
    public const  ReleaseTypes VersionReleaseType = ReleaseTypes.Release;
    public static string       SemVerVersion => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{HotfixVersion}";
    public static string       FullVersion   => $"{SemVerVersion}-{VersionReleaseType}";
    #endregion
    
    #region Strings
    public const           string WindowTitle          = "Scraps - Scrap.TF Raffle Joiner";
    public static readonly string UserAgent            = $"Scraps {FullVersion}";
    public const           string MutexName            = "ScrapsNext";
    public const           string WebView2InstallerUrl = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
    #endregion

    #region Paths
    public static readonly string StorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Caprine Logic", "Scraps");
    public static readonly string LogsPath  = Path.Combine(StorePath, "Logs");
    public static readonly string DataPath  = Path.Combine(StorePath, "Data");
    #endregion
    
    #region Update Service
    public const int    UpdateAutoCheckInterval     = 60_000;
    public const string UpdateLatestReleaseEndpoint = "https://api.github.com/repos/depthbomb/scraps/releases/latest";
    #endregion

    #region Announcement Service
    public const string AnnouncementFileUrl = "https://raw.githubusercontent.com/depthbomb/Scraps/master/ANNOUNCEMENT";
    #endregion
}
