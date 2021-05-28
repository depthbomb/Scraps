
namespace Scraps.GUI.Forms
{
    partial class WebViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebViewForm));
            this._WebBrowser = new Microsoft.Web.WebView2.WinForms.WebView2();
            this._StatusStrip = new System.Windows.Forms.StatusStrip();
            this._StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._StatusStripButton = new System.Windows.Forms.ToolStripSplitButton();
            ((System.ComponentModel.ISupportInitialize)(this._WebBrowser)).BeginInit();
            this._StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _WebBrowser
            // 
            this._WebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._WebBrowser.CreationProperties = null;
            this._WebBrowser.DefaultBackgroundColor = System.Drawing.Color.White;
            this._WebBrowser.Location = new System.Drawing.Point(0, 0);
            this._WebBrowser.Name = "_WebBrowser";
            this._WebBrowser.Size = new System.Drawing.Size(1264, 659);
            this._WebBrowser.TabIndex = 0;
            this._WebBrowser.ZoomFactor = 1D;
            // 
            // _StatusStrip
            // 
            this._StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._StatusStripLabel,
            this._StatusStripButton});
            this._StatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this._StatusStrip.Location = new System.Drawing.Point(0, 659);
            this._StatusStrip.Name = "_StatusStrip";
            this._StatusStrip.Size = new System.Drawing.Size(1264, 22);
            this._StatusStrip.TabIndex = 1;
            this._StatusStrip.Text = "statusStrip1";
            // 
            // _StatusStripLabel
            // 
            this._StatusStripLabel.Name = "_StatusStripLabel";
            this._StatusStripLabel.Size = new System.Drawing.Size(59, 17);
            this._StatusStripLabel.Text = "Loading...";
            // 
            // _StatusStripButton
            // 
            this._StatusStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._StatusStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._StatusStripButton.DropDownButtonWidth = 0;
            this._StatusStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_StatusStripButton.Image")));
            this._StatusStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._StatusStripButton.Name = "_StatusStripButton";
            this._StatusStripButton.Size = new System.Drawing.Size(99, 20);
            this._StatusStripButton.Text = "Open in browser";
            this._StatusStripButton.ToolTipText = "Open the current URL in your default browser";
            // 
            // WebViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this._StatusStrip);
            this.Controls.Add(this._WebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "WebViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading...";
            ((System.ComponentModel.ISupportInitialize)(this._WebBrowser)).EndInit();
            this._StatusStrip.ResumeLayout(false);
            this._StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 _WebBrowser;
        private System.Windows.Forms.StatusStrip _StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _StatusStripLabel;
        private System.Windows.Forms.ToolStripSplitButton _StatusStripButton;
    }
}