namespace DIAServoPress
{
    partial class Management
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Management));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tslbScan = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tstxtIP = new System.Windows.Forms.ToolStripTextBox();
            this.tslbNew = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tstxtPath = new System.Windows.Forms.ToolStripTextBox();
            this.Function = new System.Windows.Forms.ToolStripDropDownButton();
            this.tslbChangePath = new System.Windows.Forms.ToolStripMenuItem();
            this.tslbLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslbScan,
            this.toolStripSeparator2,
            this.tstxtIP,
            this.tslbNew,
            this.toolStripSeparator1,
            this.tstxtPath,
            this.Function});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // tslbScan
            // 
            this.tslbScan.Name = "tslbScan";
            resources.ApplyResources(this.tslbScan, "tslbScan");
            this.tslbScan.Click += new System.EventHandler(this.tslbScan_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tstxtIP
            // 
            this.tstxtIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.tstxtIP.Name = "tstxtIP";
            resources.ApplyResources(this.tstxtIP, "tstxtIP");
            // 
            // tslbNew
            // 
            this.tslbNew.Name = "tslbNew";
            resources.ApplyResources(this.tslbNew, "tslbNew");
            this.tslbNew.Click += new System.EventHandler(this.tslbNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tstxtPath
            // 
            this.tstxtPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.tstxtPath.Name = "tstxtPath";
            resources.ApplyResources(this.tstxtPath, "tstxtPath");
            // 
            // Function
            // 
            this.Function.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Function.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslbChangePath,
            this.tslbLanguage});
            resources.ApplyResources(this.Function, "Function");
            this.Function.Name = "Function";
            // 
            // tslbChangePath
            // 
            this.tslbChangePath.Name = "tslbChangePath";
            resources.ApplyResources(this.tslbChangePath, "tslbChangePath");
            this.tslbChangePath.Click += new System.EventHandler(this.tslbChangePath_Click);
            // 
            // tslbLanguage
            // 
            this.tslbLanguage.Name = "tslbLanguage";
            resources.ApplyResources(this.tslbLanguage, "tslbLanguage");
            this.tslbLanguage.Click += new System.EventHandler(this.tslbLanguage_Click);
            // 
            // Management
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.Name = "Management";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Management_FormClosed);
            this.Load += new System.EventHandler(this.Management_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox tstxtIP;
        private System.Windows.Forms.ToolStripLabel tslbNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox tstxtPath;
        private System.Windows.Forms.ToolStripDropDownButton Function;
        private System.Windows.Forms.ToolStripMenuItem tslbChangePath;
        private System.Windows.Forms.ToolStripMenuItem tslbLanguage;
        private System.Windows.Forms.ToolStripLabel tslbScan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}