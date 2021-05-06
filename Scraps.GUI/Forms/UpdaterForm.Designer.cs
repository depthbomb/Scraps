
namespace Scraps.GUI.Forms
{
    partial class UpdaterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterForm));
            this._UpdateStatusLabel = new System.Windows.Forms.Label();
            this._UpdateProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // _UpdateStatusLabel
            // 
            this._UpdateStatusLabel.AutoSize = true;
            this._UpdateStatusLabel.Location = new System.Drawing.Point(13, 13);
            this._UpdateStatusLabel.Name = "_UpdateStatusLabel";
            this._UpdateStatusLabel.Size = new System.Drawing.Size(129, 15);
            this._UpdateStatusLabel.TabIndex = 0;
            this._UpdateStatusLabel.Text = "Checking for updates...";
            // 
            // _UpdateProgressBar
            // 
            this._UpdateProgressBar.Location = new System.Drawing.Point(13, 42);
            this._UpdateProgressBar.MarqueeAnimationSpeed = 33;
            this._UpdateProgressBar.Name = "_UpdateProgressBar";
            this._UpdateProgressBar.Size = new System.Drawing.Size(311, 23);
            this._UpdateProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this._UpdateProgressBar.TabIndex = 1;
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 77);
            this.ControlBox = false;
            this.Controls.Add(this._UpdateProgressBar);
            this.Controls.Add(this._UpdateStatusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdaterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _UpdateStatusLabel;
        private System.Windows.Forms.ProgressBar _UpdateProgressBar;
    }
}