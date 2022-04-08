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


namespace Scraps.GUI.Forms
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._AuthorIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this._VersionLabel = new System.Windows.Forms.Label();
            this._GithubLink = new System.Windows.Forms.LinkLabel();
            this._PatreonLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this._AuthorIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // _AuthorIcon
            // 
            this._AuthorIcon.Image = global::Scraps.GUI.Images.AuthorIcon;
            this._AuthorIcon.Location = new System.Drawing.Point(12, 12);
            this._AuthorIcon.Name = "_AuthorIcon";
            this._AuthorIcon.Size = new System.Drawing.Size(64, 64);
            this._AuthorIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._AuthorIcon.TabIndex = 0;
            this._AuthorIcon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(82, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scraps - Scrap.TF Raffle Bot";
            // 
            // _VersionLabel
            // 
            this._VersionLabel.AutoSize = true;
            this._VersionLabel.Location = new System.Drawing.Point(82, 37);
            this._VersionLabel.Name = "_VersionLabel";
            this._VersionLabel.Size = new System.Drawing.Size(45, 15);
            this._VersionLabel.TabIndex = 2;
            this._VersionLabel.Text = "Version";
            this._VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _GithubLink
            // 
            this._GithubLink.AutoSize = true;
            this._GithubLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._GithubLink.Location = new System.Drawing.Point(82, 64);
            this._GithubLink.Name = "_GithubLink";
            this._GithubLink.Size = new System.Drawing.Size(45, 15);
            this._GithubLink.TabIndex = 4;
            this._GithubLink.TabStop = true;
            this._GithubLink.Text = "GitHub";
            // 
            // _PatreonLink
            // 
            this._PatreonLink.AutoSize = true;
            this._PatreonLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._PatreonLink.Location = new System.Drawing.Point(133, 64);
            this._PatreonLink.Name = "_PatreonLink";
            this._PatreonLink.Size = new System.Drawing.Size(109, 15);
            this._PatreonLink.TabIndex = 5;
            this._PatreonLink.TabStop = true;
            this._PatreonLink.Text = "Support the project";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 88);
            this.Controls.Add(this._PatreonLink);
            this.Controls.Add(this._GithubLink);
            this.Controls.Add(this._VersionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._AuthorIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Scraps";
            ((System.ComponentModel.ISupportInitialize)(this._AuthorIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _AuthorIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _VersionLabel;
        private System.Windows.Forms.LinkLabel _GithubLink;
        private System.Windows.Forms.LinkLabel _PatreonLink;
    }
}