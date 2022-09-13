using System.ComponentModel;

namespace Scraps.Next.Forms
{
    sealed partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.MainWindowStatusStrip = new System.Windows.Forms.StatusStrip();
            this._MainWindowStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this._MainWindowSecondaryStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this._MainWindowTabs = new System.Windows.Forms.TabControl();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.MainWindowStatusStrip.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainWindowStatusStrip
            // 
            this.MainWindowStatusStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainWindowStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MainWindowStatusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.MainWindowStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._MainWindowStatus,
            this._MainWindowSecondaryStatus});
            this.MainWindowStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.MainWindowStatusStrip.Name = "MainWindowStatusStrip";
            this.MainWindowStatusStrip.Size = new System.Drawing.Size(800, 22);
            this.MainWindowStatusStrip.TabIndex = 0;
            this.MainWindowStatusStrip.Text = "statusStrip1";
            // 
            // _MainWindowStatus
            // 
            this._MainWindowStatus.AutoSize = false;
            this._MainWindowStatus.BackColor = System.Drawing.Color.Transparent;
            this._MainWindowStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._MainWindowStatus.Margin = new System.Windows.Forms.Padding(0);
            this._MainWindowStatus.Name = "_MainWindowStatus";
            this._MainWindowStatus.Size = new System.Drawing.Size(785, 22);
            this._MainWindowStatus.Spring = true;
            this._MainWindowStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _MainWindowSecondaryStatus
            // 
            this._MainWindowSecondaryStatus.BackColor = System.Drawing.Color.Transparent;
            this._MainWindowSecondaryStatus.Margin = new System.Windows.Forms.Padding(0);
            this._MainWindowSecondaryStatus.Name = "_MainWindowSecondaryStatus";
            this._MainWindowSecondaryStatus.Size = new System.Drawing.Size(0, 0);
            this._MainWindowSecondaryStatus.Spring = true;
            this._MainWindowSecondaryStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _MainWindowTabs
            // 
            this._MainWindowTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._MainWindowTabs.Location = new System.Drawing.Point(0, 0);
            this._MainWindowTabs.Margin = new System.Windows.Forms.Padding(0);
            this._MainWindowTabs.Name = "_MainWindowTabs";
            this._MainWindowTabs.Padding = new System.Drawing.Point(3, 3);
            this._MainWindowTabs.SelectedIndex = 0;
            this._MainWindowTabs.Size = new System.Drawing.Size(800, 428);
            this._MainWindowTabs.TabIndex = 1;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.MainWindowStatusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._MainWindowTabs);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 428);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.MainWindowStatusStrip.ResumeLayout(false);
            this.MainWindowStatusStrip.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StatusStrip MainWindowStatusStrip;
        private TabControl _MainWindowTabs;
        private ToolStripStatusLabel _MainWindowStatus;
        private ToolStripStatusLabel _MainWindowSecondaryStatus;
        private ToolStripContainer toolStripContainer1;
    }
}