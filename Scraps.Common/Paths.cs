using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Scraps.Common
{
    public class Paths
    {
        public static string InstallPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly string StorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Caprine Logic", "Scraps");
        public static readonly string LogsPath = Path.Combine(StorePath, "Logs");
        public static readonly string SettingsFile = Path.Combine(StorePath, "Settings.xml");
    }
}
