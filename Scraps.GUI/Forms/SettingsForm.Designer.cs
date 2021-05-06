
namespace Scraps.GUI.Forms
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this._CookieInput = new System.Windows.Forms.TextBox();
            this._SortNewToggle = new System.Windows.Forms.CheckBox();
            this._ParanoidToggle = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._IncrementScanDelayToggle = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._JoinDelayInput = new System.Windows.Forms.NumericUpDown();
            this._PaginateDelayInput = new System.Windows.Forms.NumericUpDown();
            this._ScanDelayInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this._SaveButton = new System.Windows.Forms.Button();
            this._ToastToggle = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._JoinDelayInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._PaginateDelayInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ScanDelayInput)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cookie";
            // 
            // _CookieInput
            // 
            this._CookieInput.Location = new System.Drawing.Point(12, 27);
            this._CookieInput.Multiline = true;
            this._CookieInput.Name = "_CookieInput";
            this._CookieInput.Size = new System.Drawing.Size(382, 89);
            this._CookieInput.TabIndex = 1;
            // 
            // _SortNewToggle
            // 
            this._SortNewToggle.AutoSize = true;
            this._SortNewToggle.Location = new System.Drawing.Point(12, 122);
            this._SortNewToggle.Name = "_SortNewToggle";
            this._SortNewToggle.Size = new System.Drawing.Size(140, 19);
            this._SortNewToggle.TabIndex = 4;
            this._SortNewToggle.Text = "Join newer raffles first";
            this._SortNewToggle.UseVisualStyleBackColor = true;
            // 
            // _ParanoidToggle
            // 
            this._ParanoidToggle.AutoSize = true;
            this._ParanoidToggle.Location = new System.Drawing.Point(12, 147);
            this._ParanoidToggle.Name = "_ParanoidToggle";
            this._ParanoidToggle.Size = new System.Drawing.Size(107, 19);
            this._ParanoidToggle.TabIndex = 3;
            this._ParanoidToggle.Text = "Paranoid Mode";
            this._ParanoidToggle.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._IncrementScanDelayToggle);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this._JoinDelayInput);
            this.groupBox2.Controls.Add(this._PaginateDelayInput);
            this.groupBox2.Controls.Add(this._ScanDelayInput);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 103);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Delays";
            // 
            // _IncrementScanDelayToggle
            // 
            this._IncrementScanDelayToggle.AutoSize = true;
            this._IncrementScanDelayToggle.Location = new System.Drawing.Point(7, 76);
            this._IncrementScanDelayToggle.Name = "_IncrementScanDelayToggle";
            this._IncrementScanDelayToggle.Size = new System.Drawing.Size(140, 19);
            this._IncrementScanDelayToggle.TabIndex = 6;
            this._IncrementScanDelayToggle.Text = "Increment Scan Delay";
            this._IncrementScanDelayToggle.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(256, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Join Delay";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Paginate Delay";
            // 
            // _JoinDelayInput
            // 
            this._JoinDelayInput.Location = new System.Drawing.Point(256, 46);
            this._JoinDelayInput.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._JoinDelayInput.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._JoinDelayInput.Name = "_JoinDelayInput";
            this._JoinDelayInput.Size = new System.Drawing.Size(119, 23);
            this._JoinDelayInput.TabIndex = 3;
            this._JoinDelayInput.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _PaginateDelayInput
            // 
            this._PaginateDelayInput.Location = new System.Drawing.Point(131, 46);
            this._PaginateDelayInput.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._PaginateDelayInput.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._PaginateDelayInput.Name = "_PaginateDelayInput";
            this._PaginateDelayInput.Size = new System.Drawing.Size(119, 23);
            this._PaginateDelayInput.TabIndex = 2;
            this._PaginateDelayInput.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _ScanDelayInput
            // 
            this._ScanDelayInput.Location = new System.Drawing.Point(6, 46);
            this._ScanDelayInput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this._ScanDelayInput.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._ScanDelayInput.Name = "_ScanDelayInput";
            this._ScanDelayInput.Size = new System.Drawing.Size(119, 23);
            this._ScanDelayInput.TabIndex = 1;
            this._ScanDelayInput.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Scan Delay";
            // 
            // _SaveButton
            // 
            this._SaveButton.Image = global::Scraps.GUI.Icons.Save;
            this._SaveButton.Location = new System.Drawing.Point(12, 286);
            this._SaveButton.Name = "_SaveButton";
            this._SaveButton.Size = new System.Drawing.Size(382, 33);
            this._SaveButton.TabIndex = 4;
            this._SaveButton.Text = "Save";
            this._SaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this._SaveButton.UseVisualStyleBackColor = true;
            this._SaveButton.Click += new System.EventHandler(this.SaveButton_OnClick);
            // 
            // _ToastToggle
            // 
            this._ToastToggle.AutoSize = true;
            this._ToastToggle.Location = new System.Drawing.Point(232, 122);
            this._ToastToggle.Name = "_ToastToggle";
            this._ToastToggle.Size = new System.Drawing.Size(162, 19);
            this._ToastToggle.TabIndex = 5;
            this._ToastToggle.Text = "Enable Toast Notifications";
            this._ToastToggle.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 331);
            this.Controls.Add(this._ToastToggle);
            this.Controls.Add(this._ParanoidToggle);
            this.Controls.Add(this._SortNewToggle);
            this.Controls.Add(this._SaveButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._CookieInput);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scraps Settings";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._JoinDelayInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._PaginateDelayInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ScanDelayInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _CookieInput;
        private System.Windows.Forms.CheckBox _ParanoidToggle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _ScanDelayInput;
        private System.Windows.Forms.NumericUpDown _PaginateDelayInput;
        private System.Windows.Forms.NumericUpDown _JoinDelayInput;
        private System.Windows.Forms.CheckBox _IncrementScanDelayToggle;
        private System.Windows.Forms.Button _SaveButton;
        private System.Windows.Forms.CheckBox _SortNewToggle;
        private System.Windows.Forms.CheckBox _ToastToggle;
    }
}