namespace DIAServoPress
{
    partial class ReadChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadChart));
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chOutput = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnSave = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtScaleMax = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtY_min = new System.Windows.Forms.TextBox();
            this.lblY_min = new System.Windows.Forms.Label();
            this.txtY_Max = new System.Windows.Forms.TextBox();
            this.lblY_Max = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.lblX_Max = new System.Windows.Forms.Label();
            this.txtX_Max = new System.Windows.Forms.TextBox();
            this.lblX_Min = new System.Windows.Forms.Label();
            this.txtX_min = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chOutput)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chOutput
            // 
            this.chOutput.BorderlineWidth = 0;
            chartArea1.AxisX.LabelStyle.Format = "N3";
            chartArea1.AxisY.LabelStyle.Format = "N3";
            chartArea1.Name = "ChartArea1";
            this.chOutput.ChartAreas.Add(chartArea1);
            resources.ApplyResources(this.chOutput, "chOutput");
            this.chOutput.Name = "chOutput";
            this.chOutput.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(173)))), ((int)(((byte)(240)))));
            series1.MarkerBorderColor = System.Drawing.Color.Green;
            series1.MarkerBorderWidth = 2;
            series1.MarkerColor = System.Drawing.Color.Green;
            series1.Name = "data";
            series2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Silver;
            series2.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series2.MarkerBorderColor = System.Drawing.Color.Red;
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.Name = "VerticalLine";
            series3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Silver;
            series3.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series3.MarkerBorderColor = System.Drawing.Color.Red;
            series3.MarkerColor = System.Drawing.Color.Red;
            series3.Name = "VerticalLine2";
            series4.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Silver;
            series4.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            series4.MarkerBorderColor = System.Drawing.Color.Yellow;
            series4.MarkerColor = System.Drawing.Color.Yellow;
            series4.Name = "HorizontalLine";
            series5.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.Silver;
            series5.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            series5.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            series5.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            series5.Name = "HorizontalLine2";
            this.chOutput.Series.Add(series1);
            this.chOutput.Series.Add(series2);
            this.chOutput.Series.Add(series3);
            this.chOutput.Series.Add(series4);
            this.chOutput.Series.Add(series5);
            this.chOutput.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chOutput_MouseMove);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Name = "textBox1";
            // 
            // txtScaleMax
            // 
            this.txtScaleMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtScaleMax.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txtScaleMax, "txtScaleMax");
            this.txtScaleMax.ForeColor = System.Drawing.SystemColors.Window;
            this.txtScaleMax.Name = "txtScaleMax";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chOutput, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.txtScaleMax, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtY_min, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.lblY_min, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.txtY_Max, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.lblY_Max, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.txtY, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.txtX, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblX_Max, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtX_Max, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblX_Min, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtX_min, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.btnSave, 0, 15);
            this.tableLayoutPanel3.Controls.Add(this.btnUpdate, 0, 13);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // txtY_min
            // 
            resources.ApplyResources(this.txtY_min, "txtY_min");
            this.txtY_min.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtY_min.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtY_min.Name = "txtY_min";
            // 
            // lblY_min
            // 
            resources.ApplyResources(this.lblY_min, "lblY_min");
            this.lblY_min.Name = "lblY_min";
            // 
            // txtY_Max
            // 
            resources.ApplyResources(this.txtY_Max, "txtY_Max");
            this.txtY_Max.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtY_Max.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtY_Max.Name = "txtY_Max";
            // 
            // lblY_Max
            // 
            resources.ApplyResources(this.lblY_Max, "lblY_Max");
            this.lblY_Max.Name = "lblY_Max";
            // 
            // txtY
            // 
            resources.ApplyResources(this.txtY, "txtY");
            this.txtY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtY.Name = "txtY";
            // 
            // txtX
            // 
            resources.ApplyResources(this.txtX, "txtX");
            this.txtX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtX.Name = "txtX";
            // 
            // lblX_Max
            // 
            resources.ApplyResources(this.lblX_Max, "lblX_Max");
            this.lblX_Max.Name = "lblX_Max";
            // 
            // txtX_Max
            // 
            resources.ApplyResources(this.txtX_Max, "txtX_Max");
            this.txtX_Max.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtX_Max.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtX_Max.Name = "txtX_Max";
            // 
            // lblX_Min
            // 
            resources.ApplyResources(this.lblX_Min, "lblX_Min");
            this.lblX_Min.Name = "lblX_Min";
            // 
            // txtX_min
            // 
            resources.ApplyResources(this.txtX_min, "txtX_min");
            this.txtX_min.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtX_min.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            this.txtX_min.Name = "txtX_min";
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(220)))));
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // ReadChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ReadChart";
            this.Load += new System.EventHandler(this.ReadChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chOutput)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chOutput;
        private System.Windows.Forms.Button btnSave;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtScaleMax;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY_min;
        private System.Windows.Forms.Label lblX_Max;
        private System.Windows.Forms.Label lblY_min;
        private System.Windows.Forms.TextBox txtY_Max;
        private System.Windows.Forms.Label lblY_Max;
        private System.Windows.Forms.TextBox txtX_min;
        private System.Windows.Forms.Label lblX_Min;
        private System.Windows.Forms.TextBox txtX_Max;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Button btnUpdate;
    }
}