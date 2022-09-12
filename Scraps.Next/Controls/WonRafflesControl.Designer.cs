using System.ComponentModel;
using Microsoft.Web.WebView2.WinForms;

namespace Scraps.Next.Controls;

partial class WonRafflesControl
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
            this._WebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this._WebView)).BeginInit();
            this.SuspendLayout();
            // 
            // _WebView
            // 
            this._WebView.AllowExternalDrop = true;
            this._WebView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._WebView.CreationProperties = null;
            this._WebView.DefaultBackgroundColor = System.Drawing.Color.White;
            this._WebView.Location = new System.Drawing.Point(6, 6);
            this._WebView.Name = "_WebView";
            this._WebView.Size = new System.Drawing.Size(1100, 673);
            this._WebView.TabIndex = 0;
            this._WebView.ZoomFactor = 1D;
            // 
            // WonRafflesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._WebView);
            this.Name = "WonRafflesControl";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(1112, 685);
            ((System.ComponentModel.ISupportInitialize)(this._WebView)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private WebView2 _WebView;
}