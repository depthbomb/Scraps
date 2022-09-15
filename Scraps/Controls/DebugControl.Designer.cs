namespace Scraps.Controls
{
    partial class DebugControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._ForceExceptionButton = new System.Windows.Forms.Button();
            this._DeleteSettingsSubkeyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _ForceExceptionButton
            // 
            this._ForceExceptionButton.AutoSize = true;
            this._ForceExceptionButton.Location = new System.Drawing.Point(3, 3);
            this._ForceExceptionButton.Name = "_ForceExceptionButton";
            this._ForceExceptionButton.Size = new System.Drawing.Size(101, 25);
            this._ForceExceptionButton.TabIndex = 1;
            this._ForceExceptionButton.Text = "Force exception";
            this._ForceExceptionButton.UseVisualStyleBackColor = true;
            this._ForceExceptionButton.Click += new System.EventHandler(this._ForceExceptionButton_Click);
            // 
            // _DeleteSettingsSubkeyButton
            // 
            this._DeleteSettingsSubkeyButton.AutoSize = true;
            this._DeleteSettingsSubkeyButton.Location = new System.Drawing.Point(3, 34);
            this._DeleteSettingsSubkeyButton.Name = "_DeleteSettingsSubkeyButton";
            this._DeleteSettingsSubkeyButton.Size = new System.Drawing.Size(223, 25);
            this._DeleteSettingsSubkeyButton.TabIndex = 2;
            this._DeleteSettingsSubkeyButton.Text = "Delete settings subkey (requires restart)";
            this._DeleteSettingsSubkeyButton.UseVisualStyleBackColor = true;
            this._DeleteSettingsSubkeyButton.Click += new System.EventHandler(this._DeleteSettingsSubKeyButton_Click);
            // 
            // DebugControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._DeleteSettingsSubkeyButton);
            this.Controls.Add(this._ForceExceptionButton);
            this.Name = "DebugControl";
            this.Size = new System.Drawing.Size(950, 570);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button _ForceExceptionButton;
        private Button _DeleteSettingsSubkeyButton;
    }
}
