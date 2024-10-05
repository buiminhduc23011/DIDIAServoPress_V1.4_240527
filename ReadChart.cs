using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Graph = System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System.Threading;
using System.Resources;

namespace DIAServoPress
{
    public partial class ReadChart : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);

        private double scaleXMax;     //圖表X軸最大值
        private double scaleYMax;     //圖表Y軸最大值
        private double scaleXmin;     //圖表X軸最小值
        private double scaleYmin;     //圖表Y軸最小值
        private int scaleX;        //圖表X軸座標
        private int scaleY;        //圖表Y軸座標

        private string chartPath;   //圖表csv儲存路徑
        private string unit = "";

        public ReadChart(double scaleXMax, double scaleYMax, double scaleXmin, double scaleYmin, int scaleX, int scaleY, string chartPath, string unit)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();

            this.scaleXMax=scaleXMax;     //圖表X軸最大值
            this.scaleYMax=scaleYMax;     //圖表Y軸最大值
            this.scaleXmin=scaleXmin;     //圖表X軸最小值
            this.scaleYmin=scaleYmin;     //圖表Y軸最小值
            this.scaleX=scaleX;        //圖表X軸座標
            this.scaleY=scaleY;        //圖表Y軸座標

            this.chartPath=chartPath;   //圖表csv儲存路徑
            this.unit = unit;
        }


        private void ReadChart_Load(object sender, EventArgs e)
        {
            Initialize_ChResult();   //圖表初始化
            Read_Chart();            //讀圖檔

            txtX_Max.Text = Convert.ToString(scaleXMax);    //圖表資料更新
            txtY_Max.Text = Convert.ToString(scaleYMax);
            txtX_min.Text = Convert.ToString(scaleXmin);
            txtY_min.Text = Convert.ToString(scaleYmin);
            txtX.Text = Convert.ToString(ScaleX(scaleX));
            txtY.Text = Convert.ToString(ScaleY(scaleY));

        }

        private void Initialize_ChResult()     //圖表初始化
        {
            chOutput.ChartAreas[0].AxisY.Title = ScaleY(scaleY);
            chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(scaleYMax));
            chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(scaleYmin));
            chOutput.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chOutput.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
            chOutput.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;

            chOutput.ChartAreas[0].AxisX.Title = ScaleX(scaleX);
            chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(scaleXMax));
            chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(scaleXmin));
            chOutput.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            chOutput.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chOutput.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;

            chOutput.ChartAreas[0].CursorX.IsUserEnabled = true;
            chOutput.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chOutput.ChartAreas[0].CursorX.SelectionColor = Color.LawnGreen;
            chOutput.ChartAreas[0].CursorX.LineColor = Color.LawnGreen;
            chOutput.ChartAreas[0].CursorY.IsUserEnabled = true;
            chOutput.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chOutput.ChartAreas[0].CursorY.SelectionColor = Color.LawnGreen;
            chOutput.ChartAreas[0].CursorY.LineColor = Color.LawnGreen;

        }

        private void Read_Chart()    //讀圖檔
        {
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(chartPath, Encoding.Default);
                String strResult = "";
                decimal i = 0;
                while ((strResult = sr.ReadLine()) != null)
                {
                    string[] sCoordinate = strResult.Split(',');
                    if (decimal.TryParse(sCoordinate[0], out i) == true)
                    {
                        if (scaleY == 0)
                        {
                            if (scaleX == 0)
                            {
                                chOutput.Series["data"].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[1]));
                            }
                            else if (scaleX == 1)
                            {
                                chOutput.Series["data"].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[1]));
                            }
                        }
                        else if (scaleY == 1)
                        {
                            if (scaleX == 0)
                            {
                                chOutput.Series["data"].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[2]));
                            }
                            else if (scaleX == 1)
                            {
                                chOutput.Series["data"].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[2]));
                            }
                        }
                        else if (scaleY == 2)
                        {
                            if (scaleX == 0)
                            {
                                chOutput.Series["data"].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[0]));
                            }
                            else if (scaleX == 1)
                            {
                                chOutput.Series["data"].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[0]));
                            }
                        }
                    }


                }
                sr.Close();

            }
            catch (Exception)
            {
                MessageBox.Show(rM.GetString("ChartCsvErr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ScaleX(int i) 
        {
            if (i == 0)
            { 
                return rM.GetString("PosUC");
            }
            else if(i == 1)
            {
                return rM.GetString("TimeUC");
            }
            return "Err";
        }

        private string ScaleY(int i)
        {
            if (i == 0)
            {
                if (unit == "kgf")
                {
                    return rM.GetString("ForceUC");
                }
                else if (unit == "N")
                {
                    return rM.GetString("ForceNC");
                }
                else if (unit == "lbf")
                {
                    return rM.GetString("ForceLC");
                } 
            }
            else if (i == 1)
            {
                return rM.GetString("VelUC");
            }
            else if (i == 2)
            {
                return rM.GetString("PosUC");
            }
            return "Err";
        }


        //Press the "另存圖片" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog filePath = new SaveFileDialog();
            filePath.Filter = "jpg (*.jpg)|*.jpg|All files (*.*)|*.*";
            filePath.ShowDialog();
            if (filePath.FileName != "")
            {
                chOutput.SaveImage(filePath.FileName, ChartImageFormat.Jpeg);
            }
        }

        //Press the "更新" button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Double i;
            if (Double.TryParse(txtY_Max.Text, out i) == false || Double.TryParse(txtX_Max.Text, out i) == false || Double.TryParse(txtY_min.Text, out i) == false || Double.TryParse(txtX_min.Text, out i) == false)
            {
                MessageBox.Show(this, rM.GetString("ValueErrStr"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((Double.Parse(txtX_Max.Text) < Double.Parse(txtX_min.Text)) || ((Double.Parse(txtY_Max.Text) < Double.Parse(txtY_min.Text))))
            {
                MessageBox.Show(this, rM.GetString("ScaleErr"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(txtY_Max.Text));
                chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(txtX_Max.Text));
                chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(txtY_min.Text));
                chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(txtX_min.Text));
            }
        }

        ToolTip tt = null;
        Point tl = Point.Empty;
        private void chOutput_MouseMove(object sender, MouseEventArgs e)
        {
            if (tt == null) tt = new ToolTip();

            ChartArea ca = chOutput.ChartAreas[0];

            if (InnerPlotPositionClientRectangle(chOutput, ca).Contains(e.Location))
            {

                Axis ax = ca.AxisX;
                Axis ay = ca.AxisY;
                double x = ax.PixelPositionToValue(e.X);
                double y = ay.PixelPositionToValue(e.Y);
                //string s = DateTime.FromOADate(x).ToShortDateString();
                if (e.Location != tl)
                    tt.SetToolTip(chOutput, string.Format("X={0:0.###} ; Y={1:0.###}", x, y));
                //tt.SetToolTip(chOutput, string.Format("X={0:0.####}} ; Y={1:0.####}", x, y));
                tl = e.Location;
            }
            else tt.Hide(chOutput);
        }


        RectangleF InnerPlotPositionClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF IPP = CA.InnerPlotPosition.ToRectangleF();
            RectangleF CArp = ChartAreaClientRectangle(chart, CA);

            float pw = CArp.Width / 100f;
            float ph = CArp.Height / 100f;

            return new RectangleF(CArp.X + pw * IPP.X, CArp.Y + ph * IPP.Y,
                                    pw * IPP.Width, ph * IPP.Height);
        }

        RectangleF ChartAreaClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }
    }
}
