using System.ComponentModel;

namespace Scraps.Controls
{
    partial class SettingsControl
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
            this._SaveSettingsButton = new System.Windows.Forms.Button();
            this._ResetSettingsButton = new System.Windows.Forms.Button();
            this._CookieInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._ScanDelayInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._PaginateDelayInput = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this._JoinDelayInput = new System.Windows.Forms.NumericUpDown();
            this._AutoIncrementScanDelayToggle = new System.Windows.Forms.CheckBox();
            this._RaffleSortByNewToggle = new System.Windows.Forms.CheckBox();
            this._ParanoidModeToggle = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this._TopmostToggle = new System.Windows.Forms.CheckBox();
            this._CheckUpdatesToggle = new System.Windows.Forms.CheckBox();
            this._FetchAnnouncementsToggle = new System.Windows.Forms.CheckBox();
            this._SettingsParentPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this._ScanJitterInput = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this._JoinJitterInput = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this._ScanDelayInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._PaginateDelayInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._JoinDelayInput)).BeginInit();
            this._SettingsParentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ScanJitterInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._JoinJitterInput)).BeginInit();
            this.SuspendLayout();
            // 
            // _SaveSettingsButton
            // 
            this._SaveSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._SaveSettingsButton.Image = global::Scraps.Resources.Images.disk;
            this._SaveSettingsButton.Location = new System.Drawing.Point(767, 584);
            this._SaveSettingsButton.Name = "_SaveSettingsButton";
            this._SaveSettingsButton.Size = new System.Drawing.Size(90, 24);
            this._SaveSettingsButton.TabIndex = 12;
            this._SaveSettingsButton.Text = "Save";
            this._SaveSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._SaveSettingsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._SaveSettingsButton.UseVisualStyleBackColor = true;
            this._SaveSettingsButton.Click += new System.EventHandler(this._SaveSettingsButton_Click);
            // 
            // _ResetSettingsButton
            // 
            this._ResetSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ResetSettingsButton.Image = global::Scraps.Resources.Images.arrow_circle;
            this._ResetSettingsButton.Location = new System.Drawing.Point(863, 584);
            this._ResetSettingsButton.Name = "_ResetSettingsButton";
            this._ResetSettingsButton.Size = new System.Drawing.Size(90, 24);
            this._ResetSettingsButton.TabIndex = 13;
            this._ResetSettingsButton.Text = "Reset";
            this._ResetSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._ResetSettingsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._ResetSettingsButton.UseVisualStyleBackColor = true;
            this._ResetSettingsButton.Click += new System.EventHandler(this._ResetSettingsButton_Click);
            // 
            // _CookieInput
            // 
            this._CookieInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._CookieInput.Location = new System.Drawing.Point(102, 9);
            this._CookieInput.Margin = new System.Windows.Forms.Padding(3, 3, 3, 18);
            this._CookieInput.Name = "_CookieInput";
            this._CookieInput.Size = new System.Drawing.Size(845, 23);
            this._CookieInput.TabIndex = 0;
            this._CookieInput.UseSystemPasswordChar = true;
            this._CookieInput.MouseEnter += new System.EventHandler(this._CookieInput_MouseEnter);
            this._CookieInput.MouseLeave += new System.EventHandler(this._CookieInput_MouseLeave);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cookie value:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _ScanDelayInput
            // 
            this._ScanDelayInput.Location = new System.Drawing.Point(102, 53);
            this._ScanDelayInput.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this._ScanDelayInput.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this._ScanDelayInput.Name = "_ScanDelayInput";
            this._ScanDelayInput.Size = new System.Drawing.Size(135, 23);
            this._ScanDelayInput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Scan delay:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 321);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Paginate delay:";
            // 
            // _PaginateDelayInput
            // 
            this._PaginateDelayInput.Location = new System.Drawing.Point(102, 317);
            this._PaginateDelayInput.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._PaginateDelayInput.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this._PaginateDelayInput.Name = "_PaginateDelayInput";
            this._PaginateDelayInput.Size = new System.Drawing.Size(135, 23);
            this._PaginateDelayInput.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Join delay:";
            // 
            // _JoinDelayInput
            // 
            this._JoinDelayInput.Location = new System.Drawing.Point(102, 185);
            this._JoinDelayInput.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._JoinDelayInput.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this._JoinDelayInput.Name = "_JoinDelayInput";
            this._JoinDelayInput.Size = new System.Drawing.Size(135, 23);
            this._JoinDelayInput.TabIndex = 4;
            // 
            // _AutoIncrementScanDelayToggle
            // 
            this._AutoIncrementScanDelayToggle.AutoSize = true;
            this._AutoIncrementScanDelayToggle.Location = new System.Drawing.Point(246, 55);
            this._AutoIncrementScanDelayToggle.Name = "_AutoIncrementScanDelayToggle";
            this._AutoIncrementScanDelayToggle.Size = new System.Drawing.Size(111, 19);
            this._AutoIncrementScanDelayToggle.TabIndex = 2;
            this._AutoIncrementScanDelayToggle.Text = "Auto-increment";
            this._AutoIncrementScanDelayToggle.UseVisualStyleBackColor = true;
            // 
            // _RaffleSortByNewToggle
            // 
            this._RaffleSortByNewToggle.AutoSize = true;
            this._RaffleSortByNewToggle.Location = new System.Drawing.Point(102, 399);
            this._RaffleSortByNewToggle.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._RaffleSortByNewToggle.Name = "_RaffleSortByNewToggle";
            this._RaffleSortByNewToggle.Size = new System.Drawing.Size(171, 19);
            this._RaffleSortByNewToggle.TabIndex = 7;
            this._RaffleSortByNewToggle.Text = "Sort raffles by newer entries";
            this._RaffleSortByNewToggle.UseVisualStyleBackColor = true;
            // 
            // _ParanoidModeToggle
            // 
            this._ParanoidModeToggle.AutoSize = true;
            this._ParanoidModeToggle.Location = new System.Drawing.Point(102, 427);
            this._ParanoidModeToggle.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._ParanoidModeToggle.Name = "_ParanoidModeToggle";
            this._ParanoidModeToggle.Size = new System.Drawing.Size(145, 19);
            this._ParanoidModeToggle.TabIndex = 8;
            this._ParanoidModeToggle.Text = "Enable paranoid mode";
            this._ParanoidModeToggle.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(102, 82);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(845, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "The delay between scanning the raffle index when a previous scan returned no resu" +
    "lts.";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(102, 346);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(845, 16);
            this.label6.TabIndex = 14;
            this.label6.Text = "The delay between parsing raffle index page results.";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(102, 214);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(845, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "The delay between joining queued raffles. Settings this too low will result in ca" +
    "ptchas.";
            // 
            // _TopmostToggle
            // 
            this._TopmostToggle.AutoSize = true;
            this._TopmostToggle.Location = new System.Drawing.Point(102, 455);
            this._TopmostToggle.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._TopmostToggle.Name = "_TopmostToggle";
            this._TopmostToggle.Size = new System.Drawing.Size(157, 19);
            this._TopmostToggle.TabIndex = 9;
            this._TopmostToggle.Text = "Window is always on top";
            this._TopmostToggle.UseVisualStyleBackColor = true;
            // 
            // _CheckUpdatesToggle
            // 
            this._CheckUpdatesToggle.AutoSize = true;
            this._CheckUpdatesToggle.Location = new System.Drawing.Point(102, 483);
            this._CheckUpdatesToggle.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._CheckUpdatesToggle.Name = "_CheckUpdatesToggle";
            this._CheckUpdatesToggle.Size = new System.Drawing.Size(197, 19);
            this._CheckUpdatesToggle.TabIndex = 10;
            this._CheckUpdatesToggle.Text = "Check for updates automatically";
            this._CheckUpdatesToggle.UseVisualStyleBackColor = true;
            // 
            // _FetchAnnouncementsToggle
            // 
            this._FetchAnnouncementsToggle.AutoSize = true;
            this._FetchAnnouncementsToggle.Location = new System.Drawing.Point(102, 511);
            this._FetchAnnouncementsToggle.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this._FetchAnnouncementsToggle.Name = "_FetchAnnouncementsToggle";
            this._FetchAnnouncementsToggle.Size = new System.Drawing.Size(201, 19);
            this._FetchAnnouncementsToggle.TabIndex = 11;
            this._FetchAnnouncementsToggle.Text = "Fetch announcements on startup";
            this._FetchAnnouncementsToggle.UseVisualStyleBackColor = true;
            // 
            // _SettingsParentPanel
            // 
            this._SettingsParentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._SettingsParentPanel.AutoScroll = true;
            this._SettingsParentPanel.Controls.Add(this.label8);
            this._SettingsParentPanel.Controls.Add(this._ScanJitterInput);
            this._SettingsParentPanel.Controls.Add(this.label9);
            this._SettingsParentPanel.Controls.Add(this.label10);
            this._SettingsParentPanel.Controls.Add(this.label11);
            this._SettingsParentPanel.Controls.Add(this._JoinJitterInput);
            this._SettingsParentPanel.Controls.Add(this.label1);
            this._SettingsParentPanel.Controls.Add(this._FetchAnnouncementsToggle);
            this._SettingsParentPanel.Controls.Add(this.label3);
            this._SettingsParentPanel.Controls.Add(this._CheckUpdatesToggle);
            this._SettingsParentPanel.Controls.Add(this._RaffleSortByNewToggle);
            this._SettingsParentPanel.Controls.Add(this._ParanoidModeToggle);
            this._SettingsParentPanel.Controls.Add(this._PaginateDelayInput);
            this._SettingsParentPanel.Controls.Add(this._TopmostToggle);
            this._SettingsParentPanel.Controls.Add(this.label2);
            this._SettingsParentPanel.Controls.Add(this._AutoIncrementScanDelayToggle);
            this._SettingsParentPanel.Controls.Add(this.label7);
            this._SettingsParentPanel.Controls.Add(this.label5);
            this._SettingsParentPanel.Controls.Add(this._CookieInput);
            this._SettingsParentPanel.Controls.Add(this.label4);
            this._SettingsParentPanel.Controls.Add(this.label6);
            this._SettingsParentPanel.Controls.Add(this._ScanDelayInput);
            this._SettingsParentPanel.Controls.Add(this._JoinDelayInput);
            this._SettingsParentPanel.Location = new System.Drawing.Point(0, 0);
            this._SettingsParentPanel.Name = "_SettingsParentPanel";
            this._SettingsParentPanel.Padding = new System.Windows.Forms.Padding(6);
            this._SettingsParentPanel.Size = new System.Drawing.Size(956, 575);
            this._SettingsParentPanel.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 15);
            this.label8.TabIndex = 22;
            this.label8.Text = "Scan jitter:";
            // 
            // _ScanJitterInput
            // 
            this._ScanJitterInput.Location = new System.Drawing.Point(102, 119);
            this._ScanJitterInput.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._ScanJitterInput.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this._ScanJitterInput.Name = "_ScanJitterInput";
            this._ScanJitterInput.Size = new System.Drawing.Size(135, 23);
            this._ScanJitterInput.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(102, 280);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(845, 16);
            this.label9.TabIndex = 27;
            this.label9.Text = "Adds \"jitter\" to the time between raffle joins to seem more human. 0 to disable, " +
    "or integer to set maximum variation.";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 255);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 15);
            this.label10.TabIndex = 24;
            this.label10.Text = "Join jitter:";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(102, 148);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(845, 16);
            this.label11.TabIndex = 26;
            this.label11.Text = "Adds \"jitter\" to the scan delay to seem more human. 0 to disable, or integer to s" +
    "et maximum variation.";
            // 
            // _JoinJitterInput
            // 
            this._JoinJitterInput.Location = new System.Drawing.Point(102, 251);
            this._JoinJitterInput.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._JoinJitterInput.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this._JoinJitterInput.Name = "_JoinJitterInput";
            this._JoinJitterInput.Size = new System.Drawing.Size(135, 23);
            this._JoinJitterInput.TabIndex = 5;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._SettingsParentPanel);
            this.Controls.Add(this._ResetSettingsButton);
            this.Controls.Add(this._SaveSettingsButton);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(956, 611);
            ((System.ComponentModel.ISupportInitialize)(this._ScanDelayInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._PaginateDelayInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._JoinDelayInput)).EndInit();
            this._SettingsParentPanel.ResumeLayout(false);
            this._SettingsParentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ScanJitterInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._JoinJitterInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button _SaveSettingsButton;
        private Button _ResetSettingsButton;
        private TextBox _CookieInput;
        private Label label1;
        private NumericUpDown _ScanDelayInput;
        private Label label2;
        private Label label3;
        private NumericUpDown _PaginateDelayInput;
        private Label label4;
        private NumericUpDown _JoinDelayInput;
        private CheckBox _AutoIncrementScanDelayToggle;
        private CheckBox _RaffleSortByNewToggle;
        private CheckBox _ParanoidModeToggle;
        private Label label5;
        private Label label6;
        private Label label7;
        private CheckBox _TopmostToggle;
        private CheckBox _CheckUpdatesToggle;
        private CheckBox _FetchAnnouncementsToggle;
        private Panel _SettingsParentPanel;
        private Label label8;
        private NumericUpDown _ScanJitterInput;
        private Label label9;
        private Label label10;
        private Label label11;
        private NumericUpDown _JoinJitterInput;
    }
}
