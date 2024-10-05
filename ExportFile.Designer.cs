namespace DIAServoPress
{
    partial class ExportFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportFile));
            this.grpType = new System.Windows.Forms.GroupBox();
            this.rbCsv = new System.Windows.Forms.RadioButton();
            this.rbXls = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpType.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpType
            // 
            this.grpType.Controls.Add(this.rbCsv);
            this.grpType.Controls.Add(this.rbXls);
            resources.ApplyResources(this.grpType, "grpType");
            this.grpType.Name = "grpType";
            this.grpType.TabStop = false;
            // 
            // rbCsv
            // 
            resources.ApplyResources(this.rbCsv, "rbCsv");
            this.rbCsv.Name = "rbCsv";
            this.rbCsv.UseVisualStyleBackColor = true;
            // 
            // rbXls
            // 
            resources.ApplyResources(this.rbXls, "rbXls");
            this.rbXls.Checked = true;
            this.rbXls.Name = "rbXls";
            this.rbXls.TabStop = true;
            this.rbXls.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ExportFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExportFile";
            this.Load += new System.EventHandler(this.ExportFile_Load);
            this.grpType.ResumeLayout(false);
            this.grpType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpType;
        private System.Windows.Forms.RadioButton rbCsv;
        private System.Windows.Forms.RadioButton rbXls;
        private System.Windows.Forms.Button btnOK;
    }
}