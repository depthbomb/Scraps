using System.ComponentModel;

namespace Scraps.Controls
{
    partial class MainControl
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
            this._RunnerButton = new System.Windows.Forms.Button();
            this._MainViewLog = new System.Windows.Forms.RichTextBox();
            this._AlertLabel = new System.Windows.Forms.Label();
            this._ClearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _RunnerButton
            // 
            this._RunnerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._RunnerButton.Image = global::Scraps.Resources.Images.control;
            this._RunnerButton.Location = new System.Drawing.Point(6, 371);
            this._RunnerButton.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this._RunnerButton.Name = "_RunnerButton";
            this._RunnerButton.Size = new System.Drawing.Size(90, 24);
            this._RunnerButton.TabIndex = 0;
            this._RunnerButton.Text = "Start";
            this._RunnerButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._RunnerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._RunnerButton.UseVisualStyleBackColor = true;
            // 
            // _MainViewLog
            // 
            this._MainViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._MainViewLog.BackColor = System.Drawing.Color.Black;
            this._MainViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._MainViewLog.Font = new System.Drawing.Font("Cascadia Code", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._MainViewLog.ForeColor = System.Drawing.Color.White;
            this._MainViewLog.Location = new System.Drawing.Point(6, 9);
            this._MainViewLog.Name = "_MainViewLog";
            this._MainViewLog.ReadOnly = true;
            this._MainViewLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this._MainViewLog.Size = new System.Drawing.Size(812, 359);
            this._MainViewLog.TabIndex = 2;
            this._MainViewLog.Text = "";
            this._MainViewLog.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._MainViewLog_LinkClicked);
            this._MainViewLog.TextChanged += new System.EventHandler(this._MainViewLog_TextChanged);
            // 
            // _AlertLabel
            // 
            this._AlertLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._AlertLabel.Location = new System.Drawing.Point(207, 371);
            this._AlertLabel.Name = "_AlertLabel";
            this._AlertLabel.Size = new System.Drawing.Size(611, 24);
            this._AlertLabel.TabIndex = 3;
            this._AlertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _ClearButton
            // 
            this._ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ClearButton.Image = global::Scraps.Resources.Images.broom;
            this._ClearButton.Location = new System.Drawing.Point(105, 371);
            this._ClearButton.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this._ClearButton.Name = "_ClearButton";
            this._ClearButton.Size = new System.Drawing.Size(90, 24);
            this._ClearButton.TabIndex = 4;
            this._ClearButton.Text = "Clear";
            this._ClearButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._ClearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._ClearButton.UseVisualStyleBackColor = true;
            this._ClearButton.Click += new System.EventHandler(this._ClearButton_Click);
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._ClearButton);
            this.Controls.Add(this._AlertLabel);
            this.Controls.Add(this._MainViewLog);
            this.Controls.Add(this._RunnerButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainControl";
            this.Padding = new System.Windows.Forms.Padding(6, 6, 6, 3);
            this.Size = new System.Drawing.Size(827, 398);
            this.Load += new System.EventHandler(this.MainControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button _RunnerButton;
        private RichTextBox _MainViewLog;
        private Label _AlertLabel;
        private Button _ClearButton;
    }
}
