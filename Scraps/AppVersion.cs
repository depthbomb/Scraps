namespace Scraps
{
    using System;

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
        public static int Minor => 0;
        public static int Patch => 0;
        public static int Hotfix => 0;
        public static ReleaseTypes ReleaseType => ReleaseTypes.Release;
        public static string SemVer => $"{Major}.{Minor}.{Patch}.{Hotfix}";
        public static string Full => $"{SemVer}-{ReleaseType}";
    }
}
