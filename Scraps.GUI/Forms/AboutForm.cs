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

namespace Scraps.GUI.Forms;

public partial class AboutForm : Form
{
    private readonly Random   _rng;
    private readonly Bitmap[] _images;

    public AboutForm()
    {
        _rng = new Random();
        _images = new[]
        {
            Images.AuthorIcon,
            Images.AuthorIcon2,
        };

        InitializeComponent();

        Shown += AboutForm_OnShown;
    }

    private void AboutForm_OnShown(object sender, EventArgs e)
    {
        _AuthorIcon.Image  =  _images[_rng.Next(_images.Length)];
        _VersionLabel.Text =  $"v{GlobalShared.FullVersion} by depthbomb";
        _GithubLink.Click  += (_, _) => Process.Start("explorer", "https://github.com/depthbomb/Scraps");
        _PatreonLink.Click += (_, _) => Process.Start("explorer", "https://patreon.com/depthbomb");
    }
}