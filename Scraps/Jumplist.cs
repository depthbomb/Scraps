#region License
/// Scraps - Scrap.TF Raffle Joiner
/// Copyright(C) 2020  Caprine Logic

/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.

/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.

/// You should have received a copy of the GNU General Public License
/// along with this program.If not, see<https://www.gnu.org/licenses/>.
#endregion License

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
