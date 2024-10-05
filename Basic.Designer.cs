namespace DIAServoPress
{
    partial class Basic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Basic));
            this.lstItemUpperLimitPress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstItemLowerLimitPress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstItemTargetPress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstItemPressTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstItemStartTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tmrRun = new System.Windows.Forms.Timer(this.components);
            this.tmrReadRecipeDelay = new System.Windows.Forms.Timer(this.components);
            this.tmrReadBatch = new System.Windows.Forms.Timer(this.components);
            this.tmrWriteAmount = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkSQL = new System.Windows.Forms.CheckBox();
            this.chkRecord = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtNG = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnWrkShet = new System.Windows.Forms.Button();
            this.txtWkShetTitle = new System.Windows.Forms.TextBox();
            this.txtPressTitle = new System.Windows.Forms.TextBox();
            this.txtPosTitle = new System.Windows.Forms.TextBox();
            this.txtForce = new System.Windows.Forms.TextBox();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtBarCode = new System.Windows.Forms.TextBox();
            this.tmrAutoSave = new System.Windows.Forms.Timer(this.components);
            this.tmrUI = new System.Windows.Forms.Timer(this.components);
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstItemUpperLimitPress
            // 
            resources.ApplyResources(this.lstItemUpperLimitPress, "lstItemUpperLimitPress");
            // 
            // lstItemLowerLimitPress
            // 
            resources.ApplyResources(this.lstItemLowerLimitPress, "lstItemLowerLimitPress");
            // 
            // lstItemTargetPress
            // 
            resources.ApplyResources(this.lstItemTargetPress, "lstItemTargetPress");
            // 
            // lstItemPressTime
            // 
            resources.ApplyResources(this.lstItemPressTime, "lstItemPressTime");
            // 
            // lstItemStartTime
            // 
            resources.ApplyResources(this.lstItemStartTime, "lstItemStartTime");
            // 
            // tmrRun
            // 
            this.tmrRun.Interval = 10;
            this.tmrRun.Tick += new System.EventHandler(this.tmrRun_Tick);
            // 
            // tmrReadRecipeDelay
            // 
            this.tmrReadRecipeDelay.Interval = 500;
            this.tmrReadRecipeDelay.Tick += new System.EventHandler(this.tmrReadRecipeDelay_Tick);
            // 
            // tmrReadBatch
            // 
            this.tmrReadBatch.Interval = 1;
            this.tmrReadBatch.Tick += new System.EventHandler(this.tmrReadBatch_Tick);
            // 
            // tmrWriteAmount
            // 
            this.tmrWriteAmount.Tick += new System.EventHandler(this.tmrWriteAmount_Tick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.chkSQL);
            this.panel3.Controls.Add(this.chkRecord);
            this.panel3.Controls.Add(this.btnExport);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // chkSQL
            // 
            resources.ApplyResources(this.chkSQL, "chkSQL");
            this.chkSQL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkSQL.Name = "chkSQL";
            this.chkSQL.UseVisualStyleBackColor = true;
            this.chkSQL.CheckedChanged += new System.EventHandler(this.chkSQL_CheckedChanged);
            // 
            // chkRecord
            // 
            resources.ApplyResources(this.chkRecord, "chkRecord");
            this.chkRecord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkRecord.Name = "chkRecord";
            this.chkRecord.UseVisualStyleBackColor = true;
            this.chkRecord.CheckedChanged += new System.EventHandler(this.chkRecord_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.ForeColor = System.Drawing.Color.Black;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtNG
            // 
            this.txtNG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtNG, "txtNG");
            this.txtNG.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtNG.Name = "txtNG";
            // 
            // txtPass
            // 
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtPass, "txtPass");
            this.txtPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtPass.Name = "txtPass";
            // 
            // txtTotal
            // 
            this.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtTotal, "txtTotal");
            this.txtTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtTotal.Name = "txtTotal";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Name = "label3";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtNG);
            this.panel2.Controls.Add(this.txtPass);
            this.panel2.Controls.Add(this.txtTotal);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnWrkShet
            // 
            this.btnWrkShet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            resources.ApplyResources(this.btnWrkShet, "btnWrkShet");
            this.btnWrkShet.ForeColor = System.Drawing.Color.Black;
            this.btnWrkShet.Name = "btnWrkShet";
            this.btnWrkShet.UseVisualStyleBackColor = false;
            this.btnWrkShet.Click += new System.EventHandler(this.btnWrkShet_Click);
            // 
            // txtWkShetTitle
            // 
            this.txtWkShetTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtWkShetTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtWkShetTitle, "txtWkShetTitle");
            this.txtWkShetTitle.ForeColor = System.Drawing.Color.White;
            this.txtWkShetTitle.Name = "txtWkShetTitle";
            // 
            // txtPressTitle
            // 
            this.txtPressTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtPressTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtPressTitle, "txtPressTitle");
            this.txtPressTitle.ForeColor = System.Drawing.Color.White;
            this.txtPressTitle.Name = "txtPressTitle";
            // 
            // txtPosTitle
            // 
            this.txtPosTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtPosTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtPosTitle, "txtPosTitle");
            this.txtPosTitle.ForeColor = System.Drawing.Color.White;
            this.txtPosTitle.Name = "txtPosTitle";
            // 
            // txtForce
            // 
            this.txtForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtForce, "txtForce");
            this.txtForce.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtForce.Name = "txtForce";
            // 
            // txtPosition
            // 
            this.txtPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtPosition, "txtPosition");
            this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtPosition.Name = "txtPosition";
            // 
            // txtStatus
            // 
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtStatus, "txtStatus");
            this.txtStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtStatus.Name = "txtStatus";
            // 
            // txtAddress
            // 
            this.txtAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtAddress, "txtAddress");
            this.txtAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtAddress.Name = "txtAddress";
            // 
            // lblAddress
            // 
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblAddress.Name = "lblAddress";
            // 
            // txtBarCode
            // 
            this.txtBarCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtBarCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtBarCode, "txtBarCode");
            this.txtBarCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtBarCode.Name = "txtBarCode";
            // 
            // tmrAutoSave
            // 
            this.tmrAutoSave.Interval = 1000;
            this.tmrAutoSave.Tick += new System.EventHandler(this.tmrAutoSave_Tick);
            // 
            // tmrUI
            // 
            this.tmrUI.Enabled = true;
            this.tmrUI.Tick += new System.EventHandler(this.tmrUI_Tick);
            // 
            // Basic
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnWrkShet);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.txtWkShetTitle);
            this.Controls.Add(this.txtPressTitle);
            this.Controls.Add(this.txtPosTitle);
            this.Controls.Add(this.txtForce);
            this.Controls.Add(this.txtPosition);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Basic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Basic_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Basic_FormClosed);
            this.Load += new System.EventHandler(this.Basic_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader lstItemTargetPress;
        private System.Windows.Forms.ColumnHeader lstItemPressTime;
        private System.Windows.Forms.ColumnHeader lstItemUpperLimitPress;
        private System.Windows.Forms.ColumnHeader lstItemLowerLimitPress;
        private System.Windows.Forms.ColumnHeader lstItemStartTime;
        private System.Windows.Forms.Timer tmrRun;
        //private System.Windows.Forms.ColumnHeader lstItemPressVel;
        private System.Windows.Forms.Timer tmrReadRecipeDelay;
        private System.Windows.Forms.Timer tmrReadBatch;
        private System.Windows.Forms.Timer tmrWriteAmount;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkSQL;
        private System.Windows.Forms.CheckBox chkRecord;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtNG;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnWrkShet;
        private System.Windows.Forms.TextBox txtWkShetTitle;
        private System.Windows.Forms.TextBox txtPressTitle;
        private System.Windows.Forms.TextBox txtPosTitle;
        private System.Windows.Forms.TextBox txtForce;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtBarCode;
        private System.Windows.Forms.Timer tmrAutoSave;
        private System.Windows.Forms.Timer tmrUI;
    }
}