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

namespace Scraps.Next.Controls
{
    public partial class DebugControl : UserControl
    {
        public DebugControl()
        {
            InitializeComponent();
        }

        private async void _OpenSettingsFolder_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);

            await Utils.OpenDirectory(path);
        }

        private void _ForceExceptionButton_Click(object sender, EventArgs e)
        {
            throw new DivideByZeroException();
        }
    }
}
