using System.ComponentModel;

namespace Scraps.Controls
{
    partial class AboutControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this._LicenseTextLabel = new System.Windows.Forms.Label();
            this._SubmitIssueLink = new System.Windows.Forms.LinkLabel();
            this._CheckForUpdatesButton = new System.Windows.Forms.Button();
            this._RepositoryLink = new System.Windows.Forms.LinkLabel();
            this._OpenLogsFolderButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this._AuthorGithubLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this._AssemblyBuildDateLabel = new System.Windows.Forms.Label();
            this._AssemblyNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Scraps.Resources.Images.scraps;
            this.pictureBox1.Location = new System.Drawing.Point(9, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this._SubmitIssueLink);
            this.panel1.Controls.Add(this._CheckForUpdatesButton);
            this.panel1.Controls.Add(this._RepositoryLink);
            this.panel1.Controls.Add(this._OpenLogsFolderButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this._AuthorGithubLink);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this._AssemblyBuildDateLabel);
            this.panel1.Controls.Add(this._AssemblyNameLabel);
            this.panel1.Location = new System.Drawing.Point(82, 9);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6, 0, 6, 6);
            this.panel1.Size = new System.Drawing.Size(848, 477);
            this.panel1.TabIndex = 1;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Scraps.Resources.Images.heart;
            this.pictureBox4.Location = new System.Drawing.Point(78, 152);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(16, 16);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 14;
            this.pictureBox4.TabStop = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(96, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(372, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "If you like my work then consider supporting me! Info on my GitHub.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this._LicenseTextLabel);
            this.panel2.Location = new System.Drawing.Point(9, 193);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(6);
            this.panel2.Size = new System.Drawing.Size(830, 253);
            this.panel2.TabIndex = 1;
            // 
            // _LicenseTextLabel
            // 
            this._LicenseTextLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._LicenseTextLabel.Location = new System.Drawing.Point(6, 6);
            this._LicenseTextLabel.Name = "_LicenseTextLabel";
            this._LicenseTextLabel.Size = new System.Drawing.Size(818, 239);
            this._LicenseTextLabel.TabIndex = 0;
            this._LicenseTextLabel.Text = "label4";
            // 
            // _SubmitIssueLink
            // 
            this._SubmitIssueLink.AutoSize = true;
            this._SubmitIssueLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._SubmitIssueLink.Location = new System.Drawing.Point(92, 72);
            this._SubmitIssueLink.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this._SubmitIssueLink.Name = "_SubmitIssueLink";
            this._SubmitIssueLink.Size = new System.Drawing.Size(87, 15);
            this._SubmitIssueLink.TabIndex = 12;
            this._SubmitIssueLink.TabStop = true;
            this._SubmitIssueLink.Text = "Report an Issue";
            this._SubmitIssueLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._SubmitIssueLink_LinkClicked);
            // 
            // _CheckForUpdatesButton
            // 
            this._CheckForUpdatesButton.Image = global::Scraps.Resources.Images.navigation_090;
            this._CheckForUpdatesButton.Location = new System.Drawing.Point(188, 67);
            this._CheckForUpdatesButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this._CheckForUpdatesButton.Name = "_CheckForUpdatesButton";
            this._CheckForUpdatesButton.Size = new System.Drawing.Size(156, 24);
            this._CheckForUpdatesButton.TabIndex = 11;
            this._CheckForUpdatesButton.Text = "Check for Updates";
            this._CheckForUpdatesButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._CheckForUpdatesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._CheckForUpdatesButton.UseVisualStyleBackColor = true;
            this._CheckForUpdatesButton.Click += new System.EventHandler(this._CheckForUpdatesButton_Click);
            // 
            // _RepositoryLink
            // 
            this._RepositoryLink.AutoSize = true;
            this._RepositoryLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._RepositoryLink.Location = new System.Drawing.Point(9, 72);
            this._RepositoryLink.Margin = new System.Windows.Forms.Padding(3, 0, 6, 18);
            this._RepositoryLink.Name = "_RepositoryLink";
            this._RepositoryLink.Size = new System.Drawing.Size(74, 15);
            this._RepositoryLink.TabIndex = 10;
            this._RepositoryLink.TabStop = true;
            this._RepositoryLink.Text = "Source Code";
            this._RepositoryLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._RepositoryLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._RepositoryLink_LinkClicked);
            // 
            // _OpenLogsFolderButton
            // 
            this._OpenLogsFolderButton.Image = global::Scraps.Resources.Images.folder_open_document_text;
            this._OpenLogsFolderButton.Location = new System.Drawing.Point(353, 67);
            this._OpenLogsFolderButton.Name = "_OpenLogsFolderButton";
            this._OpenLogsFolderButton.Size = new System.Drawing.Size(122, 24);
            this._OpenLogsFolderButton.TabIndex = 8;
            this._OpenLogsFolderButton.Text = "Logs Folder";
            this._OpenLogsFolderButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._OpenLogsFolderButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._OpenLogsFolderButton.UseVisualStyleBackColor = true;
            this._OpenLogsFolderButton.Click += new System.EventHandler(this._OpenLogsFolderButton_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 453);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(248, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "This tool is in no way associated with Scrap.TF";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox3.Image = global::Scraps.Resources.Images.information;
            this.pictureBox3.Location = new System.Drawing.Point(9, 452);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 6;
            this.pictureBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "depthbomb#0163";
            // 
            // _AuthorGithubLink
            // 
            this._AuthorGithubLink.AutoSize = true;
            this._AuthorGithubLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._AuthorGithubLink.Location = new System.Drawing.Point(78, 131);
            this._AuthorGithubLink.Margin = new System.Windows.Forms.Padding(3, 0, 6, 6);
            this._AuthorGithubLink.Name = "_AuthorGithubLink";
            this._AuthorGithubLink.Size = new System.Drawing.Size(45, 15);
            this._AuthorGithubLink.TabIndex = 4;
            this._AuthorGithubLink.TabStop = true;
            this._AuthorGithubLink.Text = "GitHub";
            this._AuthorGithubLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._AuthorGithubLink_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(78, 108);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Made by depthbomb";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Scraps.Resources.Images.AuthorIcon;
            this.pictureBox2.Location = new System.Drawing.Point(9, 108);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 3, 6, 18);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 60);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // _AssemblyBuildDateLabel
            // 
            this._AssemblyBuildDateLabel.AutoSize = true;
            this._AssemblyBuildDateLabel.Location = new System.Drawing.Point(9, 39);
            this._AssemblyBuildDateLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this._AssemblyBuildDateLabel.Name = "_AssemblyBuildDateLabel";
            this._AssemblyBuildDateLabel.Size = new System.Drawing.Size(128, 15);
            this._AssemblyBuildDateLabel.TabIndex = 1;
            this._AssemblyBuildDateLabel.Text = "ASSEMBLY BUILD DATE";
            // 
            // _AssemblyNameLabel
            // 
            this._AssemblyNameLabel.AutoSize = true;
            this._AssemblyNameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._AssemblyNameLabel.Location = new System.Drawing.Point(9, 0);
            this._AssemblyNameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this._AssemblyNameLabel.Name = "_AssemblyNameLabel";
            this._AssemblyNameLabel.Size = new System.Drawing.Size(255, 21);
            this._AssemblyNameLabel.TabIndex = 0;
            this._AssemblyNameLabel.Text = "ASSEMBLY NAME AND VERSION";
            // 
            // AboutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "AboutControl";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(939, 495);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private Panel panel1;
        private Label _AssemblyNameLabel;
        private Label _AssemblyBuildDateLabel;
        private PictureBox pictureBox2;
        private Label label1;
        private LinkLabel _AuthorGithubLink;
        private Label label2;
        private Button _OpenLogsFolderButton;
        private LinkLabel _RepositoryLink;
        private Button _CheckForUpdatesButton;
        private Label label3;
        private PictureBox pictureBox3;
        private LinkLabel _SubmitIssueLink;
        private Panel panel2;
        private Label _LicenseTextLabel;
        private Label label4;
        private PictureBox pictureBox4;
    }
}
