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
            this._OpenSettingsFolder = new System.Windows.Forms.Button();
            this._ForceExceptionButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _OpenSettingsFolder
            // 
            this._OpenSettingsFolder.AutoSize = true;
            this._OpenSettingsFolder.Location = new System.Drawing.Point(3, 3);
            this._OpenSettingsFolder.Name = "_OpenSettingsFolder";
            this._OpenSettingsFolder.Size = new System.Drawing.Size(124, 25);
            this._OpenSettingsFolder.TabIndex = 0;
            this._OpenSettingsFolder.Text = "Open settings folder";
            this._OpenSettingsFolder.UseVisualStyleBackColor = true;
            this._OpenSettingsFolder.Click += new System.EventHandler(this._OpenSettingsFolder_Click);
            // 
            // _ForceExceptionButton
            // 
            this._ForceExceptionButton.AutoSize = true;
            this._ForceExceptionButton.Location = new System.Drawing.Point(3, 34);
            this._ForceExceptionButton.Name = "_ForceExceptionButton";
            this._ForceExceptionButton.Size = new System.Drawing.Size(101, 25);
            this._ForceExceptionButton.TabIndex = 1;
            this._ForceExceptionButton.Text = "Force exception";
            this._ForceExceptionButton.UseVisualStyleBackColor = true;
            this._ForceExceptionButton.Click += new System.EventHandler(this._ForceExceptionButton_Click);
            // 
            // DebugControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._ForceExceptionButton);
            this.Controls.Add(this._OpenSettingsFolder);
            this.Name = "DebugControl";
            this.Size = new System.Drawing.Size(950, 570);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button _OpenSettingsFolder;
        private Button _ForceExceptionButton;
    }
}
