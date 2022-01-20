#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2022 Caprine Logic

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

using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Scraps.GUI.Forms
{
    public partial class AboutForm : Form
    {
        private readonly Random _rng;
        private readonly Bitmap[] _images;

        public AboutForm()
        {
            _rng = new();
            _images = new Bitmap[]
            {
                Images.AuthorIcon,
                Images.AuthorIcon2,
            };

            InitializeComponent();

            this.Shown += AboutForm_OnShown;
        }

        private void AboutForm_OnShown(object sender, EventArgs e)
        {
            _AuthorIcon.Image = _images[_rng.Next(_images.Length)];

            _VersionLabel.Text = string.Format("v{0} by depthbomb", Common.Constants.Version.Full);

            _GithubLink.Click += (object sender, EventArgs e)
                => Process.Start("explorer", "https://github.com/depthbomb/Scraps");

            _PatreonLink.Click += (object sender, EventArgs e)
                => Process.Start("explorer", "https://patreon.com/depthbomb");
        }
    }
}
