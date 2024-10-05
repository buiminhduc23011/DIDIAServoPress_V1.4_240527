namespace DIAServoPress
{
    partial class Language
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Language));
            this.grpDevice = new System.Windows.Forms.GroupBox();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblSubMask = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.cboLan = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpDevice
            // 
            this.grpDevice.Controls.Add(this.lblIP);
            this.grpDevice.Controls.Add(this.lblDescription);
            this.grpDevice.Controls.Add(this.lblSubMask);
            resources.ApplyResources(this.grpDevice, "grpDevice");
            this.grpDevice.Name = "grpDevice";
            this.grpDevice.TabStop = false;
            // 
            // lblIP
            // 
            resources.ApplyResources(this.lblIP, "lblIP");
            this.lblIP.Name = "lblIP";
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Click += new System.EventHandler(this.lblDescription_Click);
            // 
            // lblSubMask
            // 
            resources.ApplyResources(this.lblSubMask, "lblSubMask");
            this.lblSubMask.Name = "lblSubMask";
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
            // cboLan
            // 
            resources.ApplyResources(this.cboLan, "cboLan");
            this.cboLan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.cboLan.FormattingEnabled = true;
            this.cboLan.Name = "cboLan";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Language
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grpDevice);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboLan);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Language";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Language_FormClosing);
            this.Load += new System.EventHandler(this.Language_Load);
            this.grpDevice.ResumeLayout(false);
            this.grpDevice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpDevice;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblSubMask;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboLan;
        private System.Windows.Forms.Label label1;

    }
}