namespace DIAServoPress
{
    partial class Index
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Index));
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rbOneToMany = new System.Windows.Forms.RadioButton();
            this.rbOneToOne = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.rbOneToMany);
            this.grpMode.Controls.Add(this.rbOneToOne);
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // rbOneToMany
            // 
            resources.ApplyResources(this.rbOneToMany, "rbOneToMany");
            this.rbOneToMany.Name = "rbOneToMany";
            this.rbOneToMany.UseVisualStyleBackColor = true;
            // 
            // rbOneToOne
            // 
            resources.ApplyResources(this.rbOneToOne, "rbOneToOne");
            this.rbOneToOne.Checked = true;
            this.rbOneToOne.Name = "rbOneToOne";
            this.rbOneToOne.TabStop = true;
            this.rbOneToOne.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Index
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.grpMode);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Index";
            this.Load += new System.EventHandler(this.Index_Load);
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rbOneToMany;
        private System.Windows.Forms.RadioButton rbOneToOne;
        private System.Windows.Forms.Button btnOK;
    }
}