using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Graph = System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Windows.Forms.DataVisualization.Charting;


namespace DIAServoPress
{
    public partial class Report : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private Excel.Application xlsReport;          //讀檔用Excel物件
        private Excel._Workbook wBook;
        private Excel._Worksheet wSheet;
        private Excel.Range wRange;

        private ServoPressInfor infor;
        private LiveDataManager liveDataManager;
        public StatsData[] statsData = new StatsData[CONSTANT.StepAmount];                    //Statistical Data  統計資料
            
        public string IP = "";

        public double ScaleXMax = 100;     //圖表X,Y軸最大值,最小值
        public double ScaleYMax = 100;
        public double ScaleXmin = 0;
        public double ScaleYmin = 0;

        public string OriginalPos = "0";

        public double ProductAmount;
        public double PassAmount;
        public double NGAmount;

        private int InitialRead = 0;
        public int ReadFileFlag;       //讀檔旗標 1:讀取Excel檔  0:即時結果輸出  
        public String FileName;       //讀取檔案路徑        

        private int iSeriesNum = 0;

        private double dblExcelPos = 0;
        private double dblExcelTime = 0;
        private double dblExcelForce = 0;

        private string strExcelUnit = "kgf";

        public Report(ServoPressInfor infor,LiveDataManager liveDataManager)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();

            this.infor = infor;
            this.liveDataManager = liveDataManager;

            for (int i = 0; i < CONSTANT.StepAmount; i++) 
            { 
                statsData[i] = new StatsData(); 
            }
        }

        private void Report_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ReadFileFlag == 1)
            {
                try
                {
                    wBook.Close();
                    xlsReport.Quit();
                }
                catch { }

            }
        }

        private void Report_Load(object sender, EventArgs e)
        {
            Static_Initialize();        //統計表格初始化
                if (ReadFileFlag == 1)
                {
                    try
                    {
                         Read_Excel();          //讀外部Excel檔案
                    }
                    catch (Exception)
                    {

                        MessageBox.Show(this, rM.GetString("ReadResultFileError"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                }
                else
                {
                    Read_Stats(liveDataManager.LiveDataNum);    //讀取即時壓合統計資料
                }

            chOutput.Series["0"].Points.Clear();
            Initialize_ChResult();      //圖表初始化

            InitialRead = 1;

        }

        private void Read_Excel()   //讀取外部Excel 檔案
        {
            xlsReport = new Excel.Application();
            xlsReport.Visible = false;
            xlsReport.DisplayAlerts = false;

            object missing = System.Reflection.Missing.Value;
            wBook = xlsReport.Workbooks.Open(FileName, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            wSheet = (Excel._Worksheet)wBook.Worksheets[1];
            wSheet.Activate();
            wRange = wSheet.UsedRange;

            strExcelUnit = Convert.ToString(wRange.Cells[2, 1].Value);
            dblExcelPos = Math.Ceiling(Double.Parse(Convert.ToString(wRange.Cells[14, 7].Value)));
            dblExcelTime = Math.Ceiling(Double.Parse(Convert.ToString(wRange.Cells[14, 10].Value)));
            dblExcelForce = Math.Ceiling(Double.Parse(Convert.ToString(wRange.Cells[14, 8].Value)));

            Chart_Scale();    //圖表座標定義

            //Read batch data 讀取批量統計數據
            txtTitle.Text = rM.GetString("ReadExcel") + "-" + Convert.ToString(wRange.Cells[2, 3].Value);
            txtPassAmount.Text = Convert.ToString(wRange.Cells[3, 3].Value);
            txtNGAmount.Text = Convert.ToString(wRange.Cells[4, 3].Value);
            txtProductAmount.Text = Convert.ToString(wRange.Cells[5, 3].Value);
            if (Int32.Parse(txtProductAmount.Text) == 0)
            {
                txtPassRatio.Text = "0%";
            }
            else
            {
                txtPassRatio.Text = Convert.ToString(Math.Round(((double.Parse(txtPassAmount.Text) / double.Parse(txtProductAmount.Text)) * 100), 2)) + "%";
            }

            //Read the statistical data for each step   讀取各步序統計數據
            if (wRange.Cells[2, 4].Value > 0)
            {
                txtMode1.Text = Convert.ToString(wRange.Cells[2, 5].Value);
                txtValue1.Text = Convert.ToString(wRange.Cells[2, 6].Value);
                txtMid1.Text = Convert.ToString(wRange.Cells[2, 7].Value);
                txtUpper1.Text = Convert.ToString(wRange.Cells[2, 8].Value);
                txtLower1.Text = Convert.ToString(wRange.Cells[2, 9].Value);
                txtAve1.Text = Convert.ToString(wRange.Cells[2, 10].Value);
                txtSD1.Text = Convert.ToString(wRange.Cells[2, 11].Value);
                txtCpk1.Text = Convert.ToString(wRange.Cells[2, 12].Value);
            }
            if (wRange.Cells[3, 4].Value > 0)
            {
                txtMode2.Text = Convert.ToString(wRange.Cells[3, 5].Value);
                txtValue2.Text = Convert.ToString(wRange.Cells[3, 6].Value);
                txtMid2.Text = Convert.ToString(wRange.Cells[3, 7].Value);
                txtUpper2.Text = Convert.ToString(wRange.Cells[3, 8].Value);
                txtLower2.Text = Convert.ToString(wRange.Cells[3, 9].Value);
                txtAve2.Text = Convert.ToString(wRange.Cells[3, 10].Value);
                txtSD2.Text = Convert.ToString(wRange.Cells[3, 11].Value);
                txtCpk2.Text = Convert.ToString(wRange.Cells[3, 12].Value);
            }
            if (wRange.Cells[4, 4].Value > 0)
            {
                txtMode3.Text = Convert.ToString(wRange.Cells[4, 5].Value);
                txtValue3.Text = Convert.ToString(wRange.Cells[4, 6].Value);
                txtMid3.Text = Convert.ToString(wRange.Cells[4, 7].Value);
                txtUpper3.Text = Convert.ToString(wRange.Cells[4, 8].Value);
                txtLower3.Text = Convert.ToString(wRange.Cells[4, 9].Value);
                txtAve3.Text = Convert.ToString(wRange.Cells[4, 10].Value);
                txtSD3.Text = Convert.ToString(wRange.Cells[4, 11].Value);
                txtCpk3.Text = Convert.ToString(wRange.Cells[4, 12].Value);
            }
            if (wRange.Cells[5, 4].Value > 0)
            {
                txtMode4.Text = Convert.ToString(wRange.Cells[5, 5].Value);
                txtValue4.Text = Convert.ToString(wRange.Cells[5, 6].Value);
                txtMid4.Text = Convert.ToString(wRange.Cells[5, 7].Value);
                txtUpper4.Text = Convert.ToString(wRange.Cells[5, 8].Value);
                txtLower4.Text = Convert.ToString(wRange.Cells[5, 9].Value);
                txtAve4.Text = Convert.ToString(wRange.Cells[5, 10].Value);
                txtSD4.Text = Convert.ToString(wRange.Cells[5, 11].Value);
                txtCpk4.Text = Convert.ToString(wRange.Cells[5, 12].Value);
            }
            if (wRange.Cells[6, 4].Value > 0)
            {
                txtMode5.Text = Convert.ToString(wRange.Cells[6, 5].Value);
                txtValue5.Text = Convert.ToString(wRange.Cells[6, 6].Value);
                txtMid5.Text = Convert.ToString(wRange.Cells[6, 7].Value);
                txtUpper5.Text = Convert.ToString(wRange.Cells[6, 8].Value);
                txtLower5.Text = Convert.ToString(wRange.Cells[6, 9].Value);
                txtAve5.Text = Convert.ToString(wRange.Cells[6, 10].Value);
                txtSD5.Text = Convert.ToString(wRange.Cells[6, 11].Value);
                txtCpk5.Text = Convert.ToString(wRange.Cells[6, 12].Value);
            }
            if (wRange.Cells[7, 4].Value > 0)
            {
                txtMode6.Text = Convert.ToString(wRange.Cells[7, 5].Value);
                txtValue6.Text = Convert.ToString(wRange.Cells[7, 6].Value);
                txtMid6.Text = Convert.ToString(wRange.Cells[7, 7].Value);
                txtUpper6.Text = Convert.ToString(wRange.Cells[7, 8].Value);
                txtLower6.Text = Convert.ToString(wRange.Cells[7, 9].Value);
                txtAve6.Text = Convert.ToString(wRange.Cells[7, 10].Value);
                txtSD6.Text = Convert.ToString(wRange.Cells[7, 11].Value);
                txtCpk6.Text = Convert.ToString(wRange.Cells[7, 12].Value);
            }
            if (wRange.Cells[8, 4].Value > 0)
            {
                txtMode7.Text = Convert.ToString(wRange.Cells[8, 5].Value);
                txtValue7.Text = Convert.ToString(wRange.Cells[8, 6].Value);
                txtMid7.Text = Convert.ToString(wRange.Cells[8, 7].Value);
                txtUpper7.Text = Convert.ToString(wRange.Cells[8, 8].Value);
                txtLower7.Text = Convert.ToString(wRange.Cells[8, 9].Value);
                txtAve7.Text = Convert.ToString(wRange.Cells[8, 10].Value);
                txtSD7.Text = Convert.ToString(wRange.Cells[8, 11].Value);
                txtCpk7.Text = Convert.ToString(wRange.Cells[8, 12].Value);
            }
            if (wRange.Cells[9, 4].Value > 0)
            {
                txtMode8.Text = Convert.ToString(wRange.Cells[9, 5].Value);
                txtValue8.Text = Convert.ToString(wRange.Cells[9, 6].Value);
                txtMid8.Text = Convert.ToString(wRange.Cells[9, 7].Value);
                txtUpper8.Text = Convert.ToString(wRange.Cells[9, 8].Value);
                txtLower8.Text = Convert.ToString(wRange.Cells[9, 9].Value);
                txtAve8.Text = Convert.ToString(wRange.Cells[9, 10].Value);
                txtSD8.Text = Convert.ToString(wRange.Cells[9, 11].Value);
                txtCpk8.Text = Convert.ToString(wRange.Cells[9, 12].Value);
            }
            if (wRange.Cells[10, 4].Value > 0)
            {
                txtMode9.Text = Convert.ToString(wRange.Cells[10, 5].Value);
                txtValue9.Text = Convert.ToString(wRange.Cells[10, 6].Value);
                txtMid9.Text = Convert.ToString(wRange.Cells[10, 7].Value);
                txtUpper9.Text = Convert.ToString(wRange.Cells[10, 8].Value);
                txtLower9.Text = Convert.ToString(wRange.Cells[10, 9].Value);
                txtAve9.Text = Convert.ToString(wRange.Cells[10, 10].Value);
                txtSD9.Text = Convert.ToString(wRange.Cells[10, 11].Value);
                txtCpk9.Text = Convert.ToString(wRange.Cells[10, 12].Value);
            }
            if (wRange.Cells[11, 4].Value > 0)
            {
                txtMode10.Text = Convert.ToString(wRange.Cells[11, 5].Value);
                txtValue10.Text = Convert.ToString(wRange.Cells[11, 6].Value);
                txtMid10.Text = Convert.ToString(wRange.Cells[11, 7].Value);
                txtUpper10.Text = Convert.ToString(wRange.Cells[11, 8].Value);
                txtLower10.Text = Convert.ToString(wRange.Cells[11, 9].Value);
                txtAve10.Text = Convert.ToString(wRange.Cells[11, 10].Value);
                txtSD10.Text = Convert.ToString(wRange.Cells[11, 11].Value);
                txtCpk10.Text = Convert.ToString(wRange.Cells[11, 12].Value);
            }
            Press_Data_From_Excel();  //從Excel讀取各壓合紀錄

        }

        //Adjustment of chart scale according to the different type 圖表座標定義
        private void Chart_Scale()
        {
            if (ReadFileFlag == 1)
            {
                ScaleXMax = dblExcelPos + 0.1 * dblExcelPos;
                ScaleYMax = dblExcelForce + 0.1 * dblExcelForce;              

                ScaleXmin = 0;
                ScaleYmin = 0;

            }
            txtXMax.Text = Convert.ToString(Math.Ceiling(ScaleXMax));
            txtYMax.Text = Convert.ToString(Math.Ceiling(ScaleYMax));
            txtXmin.Text = Convert.ToString(ScaleXmin);
            txtYmin.Text = Convert.ToString(ScaleYmin);

            chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(ScaleXMax));
            chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(ScaleXmin));
            chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(ScaleYMax));
            chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(ScaleYmin));
        }

        //Read in-time statistical data for current batch  讀取即時壓合統計資料
        private void Read_Stats(int intLiveDataNum)
        {
            Chart_Scale(); //圖表座標定義
            txtTitle.Text = rM.GetString("LiveRes") + "-" + infor.Barcode;

            txtProductAmount.Text = Convert.ToString((int)ProductAmount);
            txtPassAmount.Text = Convert.ToString((int)PassAmount);
            txtNGAmount.Text = Convert.ToString((int)NGAmount);
            if (Int32.Parse(txtProductAmount.Text) == 0)
            {
                txtPassRatio.Text = "0%";
            }
            else
            {
                txtPassRatio.Text = Convert.ToString(Math.Round((double.Parse(txtPassAmount.Text) / double.Parse(txtProductAmount.Text)) * 100,1)) + "%";
            }
            //txtTotalTime.Text = Convert.ToString(liveDataManager.LiveData[0].TotalTime);

            if (statsData[0].Mode != 0)
            {
                txtMode1.Text = strModeWordOutput(statsData[0].Mode);
                txtValue1.Text = strStatsString(statsData[0].StatsValue);
                txtMid1.Text = Convert.ToString(statsData[0].MidValue);
                txtUpper1.Text = Convert.ToString(statsData[0].Upper);
                txtLower1.Text = Convert.ToString(statsData[0].Lower);
                txtAve1.Text = Convert.ToString(Math.Round(statsData[0].Average, 2));
                if (statsData[0].SD > 0)
                {
                    txtSD1.Text = Convert.ToString(Math.Round(statsData[0].SD, 2));
                }
                else
                {
                    txtSD1.Text = "Err";
                }

                if (statsData[0].Cpk > 0)
                {
                    txtCpk1.Text = Convert.ToString(Math.Round(statsData[0].Cpk, 2));
                }
                else
                {
                    txtCpk1.Text = "Err";
                }
            }
            if (statsData[1].Mode != 0)
            {
                txtMode2.Text = strModeWordOutput(statsData[1].Mode);
                txtValue2.Text = strStatsString(statsData[1].StatsValue);
                txtMid2.Text = Convert.ToString(statsData[1].MidValue);
                txtUpper2.Text = Convert.ToString(statsData[1].Upper);
                txtLower2.Text = Convert.ToString(statsData[1].Lower);
                txtAve2.Text = Convert.ToString(Math.Round(statsData[1].Average, 2));

                if (statsData[1].SD > 0)
                {
                    txtSD2.Text = Convert.ToString(Math.Round(statsData[1].SD, 2));
                }
                else
                {
                    txtSD2.Text = "Err";
                }
                if (statsData[1].Cpk > 0)
                {
                    txtCpk2.Text = Convert.ToString(Math.Round(statsData[1].Cpk, 2));
                }
                else
                {
                    txtCpk2.Text = "Err";
                }
            }
            if (statsData[2].Mode != 0)
            {
                txtMode3.Text = strModeWordOutput(statsData[2].Mode);
                txtValue3.Text = strStatsString(statsData[2].StatsValue);
                txtMid3.Text = Convert.ToString(statsData[2].MidValue);
                txtUpper3.Text = Convert.ToString(statsData[2].Upper);
                txtLower3.Text = Convert.ToString(statsData[2].Lower);
                txtAve3.Text = Convert.ToString(Math.Round(statsData[2].Average, 2));

                if (statsData[2].SD > 0)
                {
                    txtSD3.Text = Convert.ToString(Math.Round(statsData[2].SD, 2));
                }
                else
                {
                    txtSD3.Text = "Err";
                }
                if (statsData[2].Cpk > 0)
                {
                    txtCpk3.Text = Convert.ToString(Math.Round(statsData[2].Cpk, 2));
                }
                else
                {
                    txtCpk3.Text = "Err";
                }
            }
            if (statsData[3].Mode != 0)
            {
                txtMode4.Text = strModeWordOutput(statsData[3].Mode);
                txtValue4.Text = strStatsString(statsData[3].StatsValue);
                txtMid4.Text = Convert.ToString(statsData[3].MidValue);
                txtUpper4.Text = Convert.ToString(statsData[3].Upper);
                txtLower4.Text = Convert.ToString(statsData[3].Lower);
                txtAve4.Text = Convert.ToString(Math.Round(statsData[3].Average, 2));

                if (statsData[3].SD > 0)
                {
                    txtSD4.Text = Convert.ToString(Math.Round(statsData[3].SD, 2));
                }
                else
                {
                    txtSD4.Text = "Err";
                }
                if (statsData[3].Cpk > 0)
                {
                    txtCpk4.Text = Convert.ToString(Math.Round(statsData[3].Cpk, 2));
                }
                else
                {
                    txtCpk4.Text = "Err";
                }
            }
            if (statsData[4].Mode != 0)
            {
                txtMode5.Text = strModeWordOutput(statsData[4].Mode);
                txtValue5.Text = strStatsString(statsData[4].StatsValue);
                txtMid5.Text = Convert.ToString(statsData[4].MidValue);
                txtUpper5.Text = Convert.ToString(statsData[4].Upper);
                txtLower5.Text = Convert.ToString(statsData[4].Lower);
                txtAve5.Text = Convert.ToString(Math.Round(statsData[4].Average, 2));

                if (statsData[4].SD > 0)
                {
                    txtSD5.Text = Convert.ToString(Math.Round(statsData[4].SD, 2));
                }
                else
                {
                    txtSD5.Text = "Err";
                }
                if (statsData[4].Cpk > 0)
                {
                    txtCpk5.Text = Convert.ToString(Math.Round(statsData[4].Cpk, 2));
                }
                else
                {
                    txtCpk5.Text = "Err";
                }
            }
            //
            if (statsData[5].Mode != 0)
            {
                txtMode6.Text = strModeWordOutput(statsData[5].Mode);
                txtValue6.Text = strStatsString(statsData[5].StatsValue);
                txtMid6.Text = Convert.ToString(statsData[5].MidValue);
                txtUpper6.Text = Convert.ToString(statsData[5].Upper);
                txtLower6.Text = Convert.ToString(statsData[5].Lower);
                txtAve6.Text = Convert.ToString(Math.Round(statsData[5].Average, 2));

                if (statsData[5].SD > 0)
                {
                    txtSD6.Text = Convert.ToString(Math.Round(statsData[5].SD, 2));
                }
                else
                {
                    txtSD6.Text = "Err";
                }
                if (statsData[5].Cpk > 0)
                {
                    txtCpk6.Text = Convert.ToString(Math.Round(statsData[5].Cpk, 2));
                }
                else
                {
                    txtCpk6.Text = "Err";
                }
            }
            if (statsData[6].Mode != 0)
            {
                txtMode7.Text = strModeWordOutput(statsData[6].Mode);
                txtValue7.Text = strStatsString(statsData[6].StatsValue);
                txtMid7.Text = Convert.ToString(statsData[6].MidValue);
                txtUpper7.Text = Convert.ToString(statsData[6].Upper);
                txtLower7.Text = Convert.ToString(statsData[6].Lower);
                txtAve7.Text = Convert.ToString(Math.Round(statsData[6].Average, 2));

                if (statsData[6].SD > 0)
                {
                    txtSD7.Text = Convert.ToString(Math.Round(statsData[6].SD, 2));
                }
                else
                {
                    txtSD7.Text = "Err";
                }
                if (statsData[6].Cpk > 0)
                {
                    txtCpk7.Text = Convert.ToString(Math.Round(statsData[6].Cpk, 2));
                }
                else
                {
                    txtCpk7.Text = "Err";
                }
            }
            if (statsData[7].Mode != 0)
            {
                txtMode8.Text = strModeWordOutput(statsData[7].Mode);
                txtValue8.Text = strStatsString(statsData[7].StatsValue);
                txtMid8.Text = Convert.ToString(statsData[7].MidValue);
                txtUpper8.Text = Convert.ToString(statsData[7].Upper);
                txtLower8.Text = Convert.ToString(statsData[7].Lower);
                txtAve8.Text = Convert.ToString(Math.Round(statsData[7].Average, 2));

                if (statsData[7].SD > 0)
                {
                    txtSD8.Text = Convert.ToString(Math.Round(statsData[7].SD, 2));
                }
                else
                {
                    txtSD8.Text = "Err";
                }
                if (statsData[7].Cpk > 0)
                {
                    txtCpk8.Text = Convert.ToString(Math.Round(statsData[7].Cpk, 2));
                }
                else
                {
                    txtCpk8.Text = "Err";
                }
            }
            if (statsData[8].Mode != 0)
            {
                txtMode9.Text = strModeWordOutput(statsData[8].Mode);
                txtValue9.Text = strStatsString(statsData[8].StatsValue);
                txtMid9.Text = Convert.ToString(statsData[8].MidValue);
                txtUpper9.Text = Convert.ToString(statsData[8].Upper);
                txtLower9.Text = Convert.ToString(statsData[8].Lower);
                txtAve9.Text = Convert.ToString(Math.Round(statsData[8].Average, 2));

                if (statsData[8].SD > 0)
                {
                    txtSD9.Text = Convert.ToString(Math.Round(statsData[8].SD, 2));
                }
                else
                {
                    txtSD9.Text = "Err";
                }
                if (statsData[8].Cpk > 0)
                {
                    txtCpk9.Text = Convert.ToString(Math.Round(statsData[8].Cpk, 2));
                }
                else
                {
                    txtCpk9.Text = "Err";
                }
            }
            if (statsData[9].Mode != 0)
            {
                txtMode10.Text = strModeWordOutput(statsData[9].Mode);
                txtValue10.Text = strStatsString(statsData[9].StatsValue);
                txtMid10.Text = Convert.ToString(statsData[9].MidValue);
                txtUpper10.Text = Convert.ToString(statsData[9].Upper);
                txtLower10.Text = Convert.ToString(statsData[9].Lower);
                txtAve10.Text = Convert.ToString(Math.Round(statsData[9].Average, 2));

                if (statsData[9].SD > 0)
                {
                    txtSD10.Text = Convert.ToString(Math.Round(statsData[9].SD, 2));
                }
                else
                {
                    txtSD10.Text = "Err";
                }
                if (statsData[9].Cpk > 0)
                {
                    txtCpk10.Text = Convert.ToString(Math.Round(statsData[9].Cpk, 2));
                }
                else
                {
                    txtCpk10.Text = "Err";
                }
            }
        }

        private void Static_Initialize()     //統計表格初始化
        {
            txtMode1.Text = "-";
            txtValue1.Text = "-";
            txtMid1.Text = "-";
            txtUpper1.Text = "-";
            txtLower1.Text = "-";
            txtAve1.Text = "-";
            txtSD1.Text = "-";
            txtCpk1.Text = "-";

            txtMode2.Text = "-";
            txtValue2.Text = "-";
            txtMid2.Text = "-";
            txtUpper2.Text = "-";
            txtLower2.Text = "-";
            txtAve2.Text = "-";
            txtSD2.Text = "-";
            txtCpk2.Text = "-";

            txtMode3.Text = "-";
            txtValue3.Text = "-";
            txtMid3.Text = "-";
            txtUpper3.Text = "-";
            txtLower3.Text = "-";
            txtAve3.Text = "-";
            txtSD3.Text = "-";
            txtCpk3.Text = "-";

            txtMode4.Text = "-";
            txtValue4.Text = "-";
            txtMid4.Text = "-";
            txtUpper4.Text = "-";
            txtLower4.Text = "-";
            txtAve4.Text = "-";
            txtSD4.Text = "-";
            txtCpk4.Text = "-";

            txtMode5.Text = "-";
            txtValue5.Text = "-";
            txtMid5.Text = "-";
            txtUpper5.Text = "-";
            txtLower5.Text = "-";
            txtAve5.Text = "-";
            txtSD5.Text = "-";
            txtCpk5.Text = "-";

            txtMode6.Text = "-";
            txtValue6.Text = "-";
            txtMid6.Text = "-";
            txtUpper6.Text = "-";
            txtLower6.Text = "-";
            txtAve6.Text = "-";
            txtSD6.Text = "-";
            txtCpk6.Text = "-";

            txtMode7.Text = "-";
            txtValue7.Text = "-";
            txtMid7.Text = "-";
            txtUpper7.Text = "-";
            txtLower7.Text = "-";
            txtAve7.Text = "-";
            txtSD7.Text = "-";
            txtCpk7.Text = "-";

            txtMode8.Text = "-";
            txtValue8.Text = "-";
            txtMid8.Text = "-";
            txtUpper8.Text = "-";
            txtLower8.Text = "-";
            txtAve8.Text = "-";
            txtSD8.Text = "-";
            txtCpk8.Text = "-";

            txtMode9.Text = "-";
            txtValue9.Text = "-";
            txtMid9.Text = "-";
            txtUpper9.Text = "-";
            txtLower9.Text = "-";
            txtAve9.Text = "-";
            txtSD9.Text = "-";
            txtCpk9.Text = "-";

            txtMode10.Text = "-";
            txtValue10.Text = "-";
            txtMid10.Text = "-";
            txtUpper10.Text = "-";
            txtLower10.Text = "-";
            txtAve10.Text = "-";
            txtSD10.Text = "-";
            txtCpk10.Text = "-";
        }


        //Return the judgment value in Chinese based on Mode 各模式輸出中文判定值
        private string strStatsString(string strCpk)
        {
            if (strCpk == "Force")
            {
                if (infor.Unit == "kgf")
                {
                    return rM.GetString("ForceU");
                }
                else if (infor.Unit == "N")
                {
                    return rM.GetString("ForceN");
                }
                else if (infor.Unit == "lbf")
                {
                    return rM.GetString("ForceL");
                }

            }
            else if (strCpk == "Position")
            {
                return rM.GetString("PressPosU");
            }
            return "???";

        }

        //Return the Name of mode in Chinese  based on the mode number 輸出中文輸出模式
        private string strModeWordOutput(int iMode)
        {
            if (iMode == 0)
            {
                return rM.GetString("Motionless");
            }
            else if (iMode == 1)
            {
                return rM.GetString("PosMode");
            }
            else if (iMode == 2)
            {
                return rM.GetString("ForceMode");
            }
            else if (iMode == 3)
            {
                return rM.GetString("DistMode");
            }
            else if (iMode == 4)
            {
                return rM.GetString("ForcePosMode");
            }
            else if (iMode == 5)
            {
                return rM.GetString("ForceDistMode");
            }
            else if (iMode == 6)
            {
                return rM.GetString("IOSignal");
            }
            return "";
        }

        //Read the data of press information from Excel file 從Excel讀取各壓合紀錄
        public void Press_Data_From_Excel()
        {
            int i = 14;
            while (Convert.ToString(wRange.Cells[i, 1].Value) != null)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Convert.ToString(wRange.Cells[i, 1].Value);
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 2].Text));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 3].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 4].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 5].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 6].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 7].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 8].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 9].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 10].Value));
                lvi.SubItems.Add(Convert.ToString(wRange.Cells[i, 11].Value));
                this.lstResult.Items.Add(lvi);
                lvi.ImageIndex++;
                i++;
            }
        }

        //Read the data of press information from temp array  從暫存陣列從讀取即時各壓合紀錄
        public void Press_Data_From_LiveData(int i)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = Convert.ToString((i + 1));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].StartTime));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].RecipeName));
            if (liveDataManager.LiveData[i].Result == 0)
            {
                lvi.SubItems.Add("OK");
            }
            else if (liveDataManager.LiveData[i].Result == 2)
            {
                lvi.SubItems.Add("NG");
            }
            else if (liveDataManager.LiveData[i].Result == 5)
            {
                lvi.SubItems.Add(rM.GetString("ForceLNG"));
            }
            else if (liveDataManager.LiveData[i].Result == 6)
            {
                lvi.SubItems.Add(rM.GetString("ForceSNG"));
            }
            else if (liveDataManager.LiveData[i].Result == 7)
            {
                lvi.SubItems.Add(rM.GetString("PosLNG"));
            }
            else if (liveDataManager.LiveData[i].Result == 8)
            {
                lvi.SubItems.Add(rM.GetString("PosSNG"));
            }
            else if (liveDataManager.LiveData[i].Result == 9)
            {
                lvi.SubItems.Add(rM.GetString("ExceedForceLimit"));
            }
            else if (liveDataManager.LiveData[i].Result == 10)
            {
                lvi.SubItems.Add(rM.GetString("ExceedPosLimit"));
            }
            else
            {
                lvi.SubItems.Add("NG");
            }

            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].StandbyPos));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].StandbyTime));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].PressPos));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].PressForce));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].PressTime));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].SingleTime));
            lvi.SubItems.Add(Convert.ToString(liveDataManager.LiveData[i].DetailNum));

            this.lstResult.Items.Add(lvi);
            lvi.ImageIndex++;
        }

        //Read the chart when listview selected item changed  選取各次壓合紀錄變更圖檔顯示
        private void lstResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            Double i;
            if (Double.TryParse(txtYMax.Text, out i) == false || Double.TryParse(txtXMax.Text, out i) == false || Double.TryParse(txtYmin.Text, out i) == false || Double.TryParse(txtXmin.Text, out i) == false)
            {
                MessageBox.Show(this, rM.GetString("Integer"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((Double.Parse(txtXMax.Text) < Double.Parse(txtXmin.Text)) || ((Double.Parse(txtYMax.Text) < Double.Parse(txtYmin.Text))))
            {
                MessageBox.Show(this, rM.GetString("ScaleErr"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else     //變更圖表座標
            {
                try
                {
                    ReadChart();
                }
                catch (Exception)
                {
                    MessageBox.Show(rM.GetString("ChartCsvErr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        
        //Read the data and draw the chart 讀取圖檔
        private void ReadChart()
        {
            txtChartNum.Text = Convert.ToString(lstResult.FocusedItem.Index+1);
            if(chkLock.Checked==false)
            {
                for (int i = 0; i < 10; i++)
                {
                    chOutput.Series[Convert.ToString(i)].Points.Clear();
                }

                    
               iSeriesNum = 0;
            }
            else if (chkLock.Checked == true)
            {
                
                if (iSeriesNum < 9) 
                {
                    iSeriesNum++;
                    
                }
                else if (iSeriesNum == 9)
                {
                    iSeriesNum = 0;
                }
                chOutput.Series[Convert.ToString(iSeriesNum)].Points.Clear();              
            }
            
            
            System.IO.StreamReader sr;    
            if (ReadFileFlag == 1)   //讀取外部Excel
            {
                if (Convert.ToString(wRange.Cells[lstResult.FocusedItem.Index + 14, 11].Value) != "-")
                {
                    sr = new System.IO.StreamReader(Path.GetDirectoryName(FileName) + "\\Chart\\" + wRange.Cells[lstResult.FocusedItem.Index+14,11].Value, Encoding.Default);
                    DrawExternalChart(sr);
                    sr.Close();
                }
                else
                {
                    chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(0, 0);
                    //MessageBox.Show(this, RM.GetString("DetailErr"), RM.GetString("Err"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else      //讀取即時暫存陣列
            {
                
                if (Int32.Parse(lstResult.FocusedItem.SubItems[10].Text) != -1)
                {
                    //sr = new System.IO.StreamReader(@".\Temp_"+strIP+"\\temp_" + Int16.Parse(lstResult.FocusedItem.SubItems[10].Text) + ".txt", Encoding.Default);
                    sr = new System.IO.StreamReader(infor.TempPath + "\\temp_" + Int32.Parse(lstResult.FocusedItem.SubItems[10].Text) + ".txt", Encoding.Default);
                    DrawExternalChart(sr);
                    sr.Close();
                }
                else
                {
                    chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(0, 0);
                    //MessageBox.Show(this, RM.GetString("DetailErr"), RM.GetString("Err"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DrawExternalChart(StreamReader sr)
        {
            String strResult = "";
            decimal i = 0;
            while ((strResult = sr.ReadLine()) != null)
            {
                string[] sCoordinate = strResult.Split(',');

                if (decimal.TryParse(sCoordinate[0], out i)==true)
                {
                    if (cboY.SelectedIndex == 0)      //Y-axis:Force
                    {
                        if (cboX.SelectedIndex == 0)    //X-axis:Position
                        {
                            chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[1]));
                            adjScaleXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[1]));
                        }
                        else if (cboX.SelectedIndex == 1)   //X-axis:Time   
                        {
                            chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[1]));
                            adjScaleXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[1]));
                        }
                    }
                    else if (cboY.SelectedIndex == 1)   //Y-axis:Velocity
                    {
                        if (cboX.SelectedIndex == 0)   //X-axis:Position
                        {
                            chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[2]));
                            adjScaleXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[2]));
                        }
                        else if (cboX.SelectedIndex == 1)   //X-axis:Time   
                        {
                            chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[2]));
                            adjScaleXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[2]));
                        }
                    }
                    else if (cboY.SelectedIndex == 2)   //Y-axis:Position
                    {
                        if (cboX.SelectedIndex == 0)   //X-axis:Position
                        {
                            chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[0]));
                            adjScaleXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[0]));
                        }
                        else if (cboX.SelectedIndex == 1)   //X-axis:Time   
                        {
                            chOutput.Series[Convert.ToString(iSeriesNum)].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[0]));
                            adjScaleXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[0]));
                        }
                    }
                }
                
            }
            chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(txtXMax.Text));
            chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(txtXmin.Text));
            chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(txtYMax.Text));
            chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(txtYmin.Text));
        }

        private void adjScaleXY(double x, double y)
        {
            if (Convert.ToDouble(txtXMax.Text) < Math.Ceiling(x + 0.5 * (x - Convert.ToDouble(txtXmin.Text))))
            {
                txtXMax.Text = Convert.ToString(Math.Ceiling(x + 0.5 * (x - Convert.ToDouble(txtXmin.Text))));
            }
            if (Convert.ToDouble(txtYMax.Text) < Math.Ceiling(y + 0.3 * (y - Convert.ToDouble(txtYmin.Text))))
            {
                txtYMax.Text = Convert.ToString(Math.Ceiling(y + 0.3 * (y - Convert.ToDouble(txtYmin.Text))));
            }
        }

        private void Initialize_ChResult()       //圖表初始化
        {
            chOutput.ChartAreas[0].AxisX.Title = rM.GetString("PosUC");
            chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(ScaleXMax));
            chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(ScaleXmin));
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

            if (infor.Unit == "kgf")
            {
                chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceUC");
            }
            else if (infor.Unit == "N")
            {
                chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceNC");
            }
            else if (infor.Unit == "lbf")
            {
                chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceLC");
            }

            
            chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(ScaleYMax));
            chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(ScaleYmin));
            chOutput.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chOutput.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
            chOutput.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;

            for (int i = 0; i < 10; i++)
            {
                chOutput.Series[Convert.ToString(i)].Points.Clear();
            }

            txtXMax.Text = Convert.ToString(Math.Ceiling(ScaleXMax));
            txtXmin.Text = Convert.ToString(Math.Ceiling(ScaleXmin));
            txtYMax.Text = Convert.ToString(Math.Ceiling(ScaleYMax));
            txtYmin.Text = Convert.ToString(Math.Ceiling(ScaleYmin));

            cboY.Items.Clear();
            
           if (ReadFileFlag == 1)
           {
               if (strExcelUnit == "kgf")
               {
                   colhPressForce.Text = rM.GetString("PressForceU");
                   chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceUC");
                   cboY.Items.Add(rM.GetString("Force"));

               }
               else if (strExcelUnit == "N")
               {
                   colhPressForce.Text = rM.GetString("PressForceN");
                   chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceNC");
                   cboY.Items.Add(rM.GetString("Force"));
               }
               else if (strExcelUnit == "lbf")
               {
                   colhPressForce.Text = rM.GetString("PressForceL");
                   chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceLC");
                   cboY.Items.Add(rM.GetString("Force"));
               }
           }
           else 
           {
               if (infor.Unit == "kgf")
               {
                   colhPressForce.Text = rM.GetString("PressForceU");
                   chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceUC");
                   cboY.Items.Add(rM.GetString("Force"));

               }
               else if (infor.Unit == "N")
               {
                   colhPressForce.Text = rM.GetString("PressForceN");
                   chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceNC");
                   cboY.Items.Add(rM.GetString("Force"));
               }
               else if (infor.Unit == "lbf")
               {
                   colhPressForce.Text = rM.GetString("PressForceL");
                   chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceLC");
                   cboY.Items.Add(rM.GetString("Force"));
               }
           }
           
               cboY.Items.Add(rM.GetString("Vel"));
               cboY.Items.Add(rM.GetString("Pos"));

              cboY.SelectedIndex = 0;
          
              cboX.Items.Clear();
              cboX.Items.Add(rM.GetString("Pos"));
              cboX.Items.Add(rM.GetString("Time"));

               cboX.SelectedIndex = 0;

               chOutput.Series["0"].Points.AddXY(0, 0);
               
        }

        private void btnEnlarge_Click(object sender, EventArgs e)   //"圖表放大"按鈕
        {
            try
            {
                string chartPath = "";
                ReadChart fReadChart;
                if (ReadFileFlag == 1)   //讀取外部csv檔案
                {
                    chartPath = Path.GetDirectoryName(FileName) + "\\Chart\\" + wRange.Cells[2, 3].Value + "_" + wRange.Cells[1, 1].Value + "_" + (lstResult.FocusedItem.Index + 1) + ".csv";
                    fReadChart = new ReadChart(double.Parse(txtXMax.Text), double.Parse(txtYMax.Text), double.Parse(txtXmin.Text), double.Parse(txtYmin.Text), cboX.SelectedIndex, cboY.SelectedIndex, chartPath, strExcelUnit);

                }
                else      //讀取即時暫存檔temp
                {
                    chartPath = infor.TempPath + "\\temp_" + lstResult.FocusedItem.Index + ".txt";
                    fReadChart = new ReadChart(double.Parse(txtXMax.Text), double.Parse(txtYMax.Text), double.Parse(txtXmin.Text), double.Parse(txtYmin.Text), cboX.SelectedIndex, cboY.SelectedIndex, chartPath, infor.Unit);
                }
                fReadChart.ShowDialog(this);
            }
            catch
            {
                MessageBox.Show(rM.GetString("ChartCsvErr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void update() 
        {
            chkLock.Checked = false;
            Double i;
            if (Double.TryParse(txtYMax.Text, out i) == false || Double.TryParse(txtXMax.Text, out i) == false)
            {
                MessageBox.Show(this, rM.GetString("Integer"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((Double.Parse(txtXMax.Text) < Double.Parse(txtXmin.Text)) || ((Double.Parse(txtYMax.Text) < Double.Parse(txtYmin.Text))))
            {
                MessageBox.Show(this, rM.GetString("ScaleErr"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else     //變更圖表座標
            {
                if (cboX.SelectedIndex == 0)
                {
                    chOutput.ChartAreas[0].AxisX.Title = rM.GetString("PosUC");
                }
                else if (cboX.SelectedIndex == 1)
                {
                    chOutput.ChartAreas[0].AxisX.Title = rM.GetString("TimeUC");
                }

                if (cboY.SelectedIndex == 0)
                {
                    if (ReadFileFlag == 1)
                    {
                        if (strExcelUnit == "kgf")
                        {
                            chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceUC");
                        }
                        else if (strExcelUnit == "N")
                        {
                            chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceNC");
                        }
                        else if (strExcelUnit == "lbf")
                        {
                            chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceLC");
                        }
                    }
                    else
                    {
                        if (infor.Unit == "kgf")
                        {
                            chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceUC");
                        }
                        else if (infor.Unit == "N")
                        {
                            chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceNC");
                        }
                        else if (infor.Unit == "lbf")
                        {
                            chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceLC");
                        }
                    }
                }
                else if (cboY.SelectedIndex == 1)
                {
                    chOutput.ChartAreas[0].AxisY.Title = rM.GetString("VelUC");
                }

                else if (cboY.SelectedIndex == 2)
                {
                    chOutput.ChartAreas[0].AxisY.Title = rM.GetString("PosUC");
                }

                if (InitialRead == 1)
                {
                    ReadChart();
                }

                chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(txtYmin.Text));
                chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(txtYMax.Text));
                chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(txtXmin.Text));
                chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(txtXMax.Text));


            }
        
        }


        //變更圖表型式時自動變更單位顯示
        private void cboX_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkLock.Checked = false;
            if (cboX.SelectedIndex == 0)
            {
                lblXMax.Text = "mm";
                lblXmin.Text = "mm";

                /*
                if (ReadFileFlag == 1)
                {
                    txtXMax.Text = Convert.ToString(Math.Ceiling(dblExcelPos + 0.1 * dblExcelPos));
                }
                
                else
                {
                    txtXMax.Text = Convert.ToString(Math.Ceiling(liveDataManager.LiveData[0].PressPos + 0.1 * liveDataManager.LiveData[0].PressPos));   
                }*/
                txtXmin.Text = OriginalPos;
                txtXMax.Text = Convert.ToString(Convert.ToDouble(OriginalPos) + 10);
            }
            else if (cboX.SelectedIndex == 1)
            {
                lblXMax.Text = "s";
                lblXmin.Text = "s";

                /*
                if (ReadFileFlag == 1)
                {
                    txtXMax.Text = Convert.ToString(Math.Ceiling(dblExcelTime + 0.1 * dblExcelTime));
                }
                else
                {
                    txtXMax.Text = Convert.ToString(Math.Ceiling(liveDataManager.LiveData[0].TotalTime + 0.1 * liveDataManager.LiveData[0].TotalTime));
                }
                */
                txtXmin.Text = "0";
                txtXMax.Text = "1";
            }
            update();
        }

        private void cboY_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkLock.Checked = false;
            if (cboY.SelectedIndex == 0)
            {
                if (ReadFileFlag == 1)
                {
                    /*
                    if (dblExcelForce > 0)
                    {
                        txtYMax.Text = Convert.ToString(Math.Ceiling(dblExcelForce + 0.1 * dblExcelForce));
                    }
                    else
                    {
                        txtYMax.Text = "270";
                    }
                    */
                    if (strExcelUnit == "kgf")
                    {
                        lblYMax.Text = "kgf";
                        lblYmin.Text = "kgf";
                    }
                    else if (strExcelUnit == "N")
                    {
                        lblYMax.Text = "N";
                        lblYmin.Text = "N";
                    }
                    else if (strExcelUnit == "lbf")
                    {
                        lblYMax.Text = "lbf";
                        lblYmin.Text = "lbf";
                    }

                }
                else
                {
                    if (infor.Unit == "kgf")
                    {
                        lblYMax.Text = "kgf";
                        lblYmin.Text = "kgf";
                    }
                    else if (infor.Unit == "N")
                    {
                        lblYMax.Text = "N";
                        lblYmin.Text = "N";
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lblYMax.Text = "lbf";
                        lblYmin.Text = "lbf";
                    }
                   /*
                    if (liveDataManager.LiveData[0].PressForce > 0)
                    {
                        txtYMax.Text = Convert.ToString(Math.Ceiling(liveDataManager.LiveData[0].PressForce + 0.1 * liveDataManager.LiveData[0].PressForce));
                    }
                    else 
                    {
                        txtYMax.Text = "270";
                    }
                    */
                }
                txtYmin.Text = "0";
                txtYMax.Text = "1";
            }
            else if (cboY.SelectedIndex == 1)
            {
                lblYMax.Text = "mm/s";
                lblYmin.Text = "mm/s";
                txtYMax.Text = "1";
                txtYmin.Text = "0";
            }

            else if (cboY.SelectedIndex == 2)
            {
                lblYMax.Text = "mm";
                lblYmin.Text = "mm";
                
                /*
                if (ReadFileFlag == 1)
                {
                    txtYMax.Text = Convert.ToString(Math.Ceiling(dblExcelPos + 0.1 * dblExcelPos));
                }
                else
                {
                    txtYMax.Text = Convert.ToString(Math.Ceiling(liveDataManager.LiveData[0].PressPos + 0.1 * liveDataManager.LiveData[0].PressPos));
                }*/
                txtYmin.Text = OriginalPos;
                txtYMax.Text = Convert.ToString(Convert.ToInt32(OriginalPos) + 10);
            }
            update();
        }

        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLock.Checked == false)
            {
                for (int i = 0; i < 10; i++)
                {
                    chOutput.Series[Convert.ToString(i)].Points.Clear();
                }
                chOutput.Series["0"].Points.AddXY(0, 0);

                for (int j = 0; j < lstResult.Items.Count; j++)
                {
                    lstResult.Items[j].ForeColor = Color.FromArgb(0, 135, 220);
                }
            }
            
        }

        private void chOutput_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chkLock.Checked = false;
            Double i;
            if (Double.TryParse(txtYMax.Text, out i) == false || Double.TryParse(txtXMax.Text, out i) == false)
            {
                MessageBox.Show(this, rM.GetString("Integer"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((Double.Parse(txtXMax.Text) < Double.Parse(txtXmin.Text)) || ((Double.Parse(txtYMax.Text) < Double.Parse(txtYmin.Text))))
            {
                MessageBox.Show(this, rM.GetString("ScaleErr"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else     //變更圖表座標
            {
                chOutput.ChartAreas[0].AxisY.Minimum = Math.Floor(Convert.ToDouble(txtYmin.Text));
                chOutput.ChartAreas[0].AxisY.Maximum = Math.Floor(Convert.ToDouble(txtYMax.Text));
                chOutput.ChartAreas[0].AxisX.Minimum = Math.Floor(Convert.ToDouble(txtXmin.Text));
                chOutput.ChartAreas[0].AxisX.Maximum = Math.Floor(Convert.ToDouble(txtXMax.Text));

            }
        
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        ToolTip tt = null;
        Point tl = Point.Empty;
        private void Report_MouseMove(object sender, MouseEventArgs e)
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

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
