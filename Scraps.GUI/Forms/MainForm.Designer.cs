
namespace Scraps.GUI.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._StartStopButton = new System.Windows.Forms.Button();
            this._StatusStrip = new System.Windows.Forms.StatusStrip();
            this._Status = new System.Windows.Forms.ToolStripStatusLabel();
            this._LogWindow = new System.Windows.Forms.RichTextBox();
            this._SettingsButton = new System.Windows.Forms.Button();
            this._InfoButton = new System.Windows.Forms.Button();
            this._WonRafflesButton = new System.Windows.Forms.Button();
            this._StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _StartStopButton
            // 
            this._StartStopButton.Image = global::Scraps.GUI.Icons.Start;
            this._StartStopButton.Location = new System.Drawing.Point(12, 300);
            this._StartStopButton.Name = "_StartStopButton";
            this._StartStopButton.Size = new System.Drawing.Size(299, 31);
            this._StartStopButton.TabIndex = 0;
            this._StartStopButton.Text = "Start";
            this._StartStopButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._StartStopButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._StartStopButton.UseVisualStyleBackColor = true;
            this._StartStopButton.Click += new System.EventHandler(this.StartStopButton_OnClick);
            // 
            // _StatusStrip
            // 
            this._StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._Status});
            this._StatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this._StatusStrip.Location = new System.Drawing.Point(0, 337);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new System.Drawing.Size(627, 20);
            this._StatusStrip.SizingGrip = false;
            this._StatusStrip.TabIndex = 5;
            this._StatusStrip.Text = "statusStrip1";
            // 
            // _Status
            // 
            this._Status.Name = "_Status";
            this._Status.Size = new System.Drawing.Size(10, 15);
            this._Status.Text = " ";
            // 
            // _LogWindow
            // 
            this._LogWindow.BackColor = System.Drawing.Color.Black;
            this._LogWindow.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._LogWindow.ForeColor = System.Drawing.Color.White;
            this._LogWindow.Location = new System.Drawing.Point(13, 13);
            this._LogWindow.Name = "_LogWindow";
            this._LogWindow.ReadOnly = true;
            this._LogWindow.Size = new System.Drawing.Size(602, 281);
            this._LogWindow.TabIndex = 6;
            this._LogWindow.Text = "";
            // 
            // _SettingsButton
            // 
            this._SettingsButton.Image = global::Scraps.GUI.Icons.Settings;
            this._SettingsButton.Location = new System.Drawing.Point(443, 300);
            this._SettingsButton.Name = "_SettingsButton";
            this._SettingsButton.Size = new System.Drawing.Size(83, 31);
            this._SettingsButton.TabIndex = 7;
            this._SettingsButton.Text = "Settings";
            this._SettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._SettingsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._SettingsButton.UseVisualStyleBackColor = true;
            this._SettingsButton.Click += new System.EventHandler(this.SettingsButton_OnClick);
            // 
            // _InfoButton
            // 
            this._InfoButton.Image = global::Scraps.GUI.Icons.Info;
            this._InfoButton.Location = new System.Drawing.Point(532, 300);
            this._InfoButton.Name = "_InfoButton";
            this._InfoButton.Size = new System.Drawing.Size(83, 31);
            this._InfoButton.TabIndex = 8;
            this._InfoButton.Text = "About";
            this._InfoButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._InfoButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._InfoButton.UseVisualStyleBackColor = true;
            this._InfoButton.Click += new System.EventHandler(this.InfoButton_OnClick);
            // 
            // _WonRafflesButton
            // 
            this._WonRafflesButton.Image = global::Scraps.GUI.Icons.OpenLink;
            this._WonRafflesButton.Location = new System.Drawing.Point(317, 300);
            this._WonRafflesButton.Name = "_WonRafflesButton";
            this._WonRafflesButton.Size = new System.Drawing.Size(120, 31);
            this._WonRafflesButton.TabIndex = 9;
            this._WonRafflesButton.Text = "Won Raffles";
            this._WonRafflesButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._WonRafflesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._WonRafflesButton.UseVisualStyleBackColor = true;
            this._WonRafflesButton.Click += new System.EventHandler(this.WonRafflesButton_OnClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 357);
            this.Controls.Add(this._WonRafflesButton);
            this.Controls.Add(this._InfoButton);
            this.Controls.Add(this._SettingsButton);
            this.Controls.Add(this._LogWindow);
            this.Controls.Add(this._StatusStrip);
            this.Controls.Add(this._StartStopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Initializing...";
            this.Shown += new System.EventHandler(this.MainForm_OnShown);
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _StartStopButton;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _Status;
        private System.Windows.Forms.RichTextBox _LogWindow;
        private System.Windows.Forms.Button _SettingsButton;
        private System.Windows.Forms.Button _InfoButton;
        private System.Windows.Forms.Button _WonRafflesButton;
    }
}

