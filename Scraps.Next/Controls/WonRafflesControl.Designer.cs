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
            this._StatusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._WebView)).BeginInit();
            this.SuspendLayout();
            // 
            // _WebView
            // 
            this._WebView.AllowExternalDrop = true;
            this._WebView.CreationProperties = null;
            this._WebView.DefaultBackgroundColor = System.Drawing.Color.White;
            this._WebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._WebView.Location = new System.Drawing.Point(0, 0);
            this._WebView.Margin = new System.Windows.Forms.Padding(0);
            this._WebView.Name = "_WebView";
            this._WebView.Size = new System.Drawing.Size(1112, 685);
            this._WebView.TabIndex = 0;
            this._WebView.ZoomFactor = 1D;
            // 
            // _StatusLabel
            // 
            this._StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._StatusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._StatusLabel.Location = new System.Drawing.Point(0, 0);
            this._StatusLabel.Name = "_StatusLabel";
            this._StatusLabel.Size = new System.Drawing.Size(1112, 685);
            this._StatusLabel.TabIndex = 1;
            this._StatusLabel.Text = "Add a valid cookie to display your won raffles.";
            this._StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WonRafflesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._StatusLabel);
            this.Controls.Add(this._WebView);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "WonRafflesControl";
            this.Size = new System.Drawing.Size(1112, 685);
            this.Load += new System.EventHandler(this.WonRafflesControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this._WebView)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private WebView2 _WebView;
    private Label _StatusLabel;
}