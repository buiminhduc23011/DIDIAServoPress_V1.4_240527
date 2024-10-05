namespace DIAServoPress
{
    partial class Scan
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Scan));
            this.tmrScan = new System.Windows.Forms.Timer(this.components);
            this.btnRead = new System.Windows.Forms.Button();
            this.btnAbandon = new System.Windows.Forms.Button();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUnit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboNetwork = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tmrScan
            // 
            this.tmrScan.Interval = 1000;
            this.tmrScan.Tick += new System.EventHandler(this.tmrScan_Tick);
            // 
            // btnRead
            // 
            this.btnRead.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnRead, "btnRead");
            this.btnRead.ForeColor = System.Drawing.Color.Black;
            this.btnRead.Name = "btnRead";
            this.btnRead.UseVisualStyleBackColor = false;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnAbandon
            // 
            this.btnAbandon.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnAbandon, "btnAbandon");
            this.btnAbandon.ForeColor = System.Drawing.Color.Black;
            this.btnAbandon.Name = "btnAbandon";
            this.btnAbandon.UseVisualStyleBackColor = false;
            this.btnAbandon.Click += new System.EventHandler(this.btnAbandon_Click);
            // 
            // pbar
            // 
            resources.ApplyResources(this.pbar, "pbar");
            this.pbar.Maximum = 3;
            this.pbar.Name = "pbar";
            this.pbar.Step = 255;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.White;
            this.btnConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.ForeColor = System.Drawing.Color.Gray;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.btnScan, "btnScan");
            this.btnScan.ForeColor = System.Drawing.Color.White;
            this.btnScan.Name = "btnScan";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chIP,
            this.chType,
            this.chUnit});
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.listView1.HideSelection = false;
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // chIP
            // 
            resources.ApplyResources(this.chIP, "chIP");
            // 
            // chType
            // 
            resources.ApplyResources(this.chType, "chType");
            // 
            // chUnit
            // 
            resources.ApplyResources(this.chUnit, "chUnit");
            // 
            // cboNetwork
            // 
            this.cboNetwork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.cboNetwork.FormattingEnabled = true;
            resources.ApplyResources(this.cboNetwork, "cboNetwork");
            this.cboNetwork.Name = "cboNetwork";
            // 
            // Scan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnAbandon);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.cboNetwork);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Scan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Scan_FormClosing);
            this.Load += new System.EventHandler(this.Scan_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrScan;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnAbandon;
        private System.Windows.Forms.ProgressBar pbar;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chIP;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chUnit;
        private System.Windows.Forms.ComboBox cboNetwork;
    }
}