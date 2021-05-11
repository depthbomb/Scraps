using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using Scraps.GUI.Forms;
using Scraps.Common.Constants;

namespace Scraps.GUI
{
    static class Bootstrapper
    {
        [STAThread]
        static void Main(string[] args)
        {
            EnsureFileSystem();

            if (!IsAlreadyRunning())
            {
                if (Properties.UserConfig.Default.UpgradeRequired)
                {
                    Properties.UserConfig.Default.Upgrade();
                    Properties.UserConfig.Default.UpgradeRequired = false;
                    Properties.UserConfig.Default.Save();
                }

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        static bool IsAlreadyRunning() => Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1;

        static void EnsureFileSystem()
        {
            foreach (string path in new string[]
            {
                Paths.LogsPath,
                Paths.DataPath,
            })
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }
}
