using System;
using System.IO;
using System.Reflection;

using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Scraps.Jumplist
{
    public class Jumplist
    {
        private static string ExePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Scraps.exe");
        private static JumpList List = JumpList.CreateJumpList();

        public static void Register()
        {
            JumpListLink verboseTask = new JumpListLink(ExePath, "Start Scraps (Verbose)")
            {
                Arguments = "--verbose",
                IconReference = new IconReference(ExePath, 0)
            };

            JumpListLink configureTask = new JumpListLink(ExePath, "Configure Scraps")
            {
                Arguments = "--configure",
                IconReference = new IconReference(ExePath, 0)
            };

            List.ClearAllUserTasks();
            List.AddUserTasks(configureTask, verboseTask);
            List.Refresh();
        }
    }
}
