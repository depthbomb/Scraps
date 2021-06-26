
namespace Scraps.GUI.Forms
{
    partial class DebugForm
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
            this._CreateToastButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._ClearToastHistoryButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._CheckWebView2RuntimeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _CreateToastButton
            // 
            this._CreateToastButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._CreateToastButton.Location = new System.Drawing.Point(6, 22);
            this._CreateToastButton.Name = "_CreateToastButton";
            this._CreateToastButton.Size = new System.Drawing.Size(268, 23);
            this._CreateToastButton.TabIndex = 0;
            this._CreateToastButton.Text = "Create toast notification";
            this._CreateToastButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._ClearToastHistoryButton);
            this.groupBox1.Controls.Add(this._CreateToastButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 84);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Toast Notifications";
            // 
            // _ClearToastHistoryButton
            // 
            this._ClearToastHistoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ClearToastHistoryButton.Location = new System.Drawing.Point(7, 52);
            this._ClearToastHistoryButton.Name = "_ClearToastHistoryButton";
            this._ClearToastHistoryButton.Size = new System.Drawing.Size(267, 23);
            this._ClearToastHistoryButton.TabIndex = 1;
            this._ClearToastHistoryButton.Text = "Clear toast history";
            this._ClearToastHistoryButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this._CheckWebView2RuntimeButton);
            this.groupBox2.Location = new System.Drawing.Point(18, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 55);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WebView2";
            // 
            // _CheckWebView2RuntimeButton
            // 
            this._CheckWebView2RuntimeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._CheckWebView2RuntimeButton.Location = new System.Drawing.Point(7, 23);
            this._CheckWebView2RuntimeButton.Name = "_CheckWebView2RuntimeButton";
            this._CheckWebView2RuntimeButton.Size = new System.Drawing.Size(255, 23);
            this._CheckWebView2RuntimeButton.TabIndex = 0;
            this._CheckWebView2RuntimeButton.Text = "Check if runtime is installed";
            this._CheckWebView2RuntimeButton.UseVisualStyleBackColor = true;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 281);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(320, 320);
            this.Name = "DebugForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Debug";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _CreateToastButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button _ClearToastHistoryButton;
        private System.Windows.Forms.Button _CheckWebView2RuntimeButton;
    }
}