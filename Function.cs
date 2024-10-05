#region Basic Namespace
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#endregion 

#region Additional Namespace
using System.IO;
using EasyModbus;
using System.Globalization;
using Graph = System.Windows.Forms.DataVisualization.Charting;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

#endregion

namespace DIAServoPress
{
    public partial class Function : Basic
    {
        protected ChartManager chartManager;

        protected Others others;
        protected USB usb;

        private double timeTemp = 0;


        #region 啟動與關閉程式
        public Function()
        {
            Language_Initialize();
            InitializeComponent();

            plc = new PLC();
            infor = new ServoPressInfor(plc);

            motionManager = new MotionManager(plc,infor);
            liveDataManager = new LiveDataManager(plc);
            sql = new SQL(liveDataManager);
            statsManager = new StatsManager(motionManager, liveDataManager,plc);
            export = new Export(plc,statsManager, infor, motionManager, liveDataManager);
            //autoSave = new AutoSave(export);

            others = new Others(plc,lstItemMinForceLimit, lstItemPressForce, lstItemMaxForceLimit, infor, motionManager, lstMotion, chkAuto, chOutput, pnOriginal, pnPrepare, pnPress, cboX, cboY);
            
            chartManager = new ChartManager(plc,chOutput, infor, motionManager, cboX, cboY);   //圖表初始化
            usb = new USB(plc);
        }
        private void Function_Load(object sender, EventArgs e)
        {
            ComPortBox.Items.AddRange(usb.getPortNames());
            if (ComPortBox.Items.Count > 0)
            {
                ComPortBox.SelectedIndex = ComPortBox.Items.Count-1;
            }
            //tscbConnect.SelectedIndex = 0;
            tscbConnect.SelectedIndex = Properties.Settings.Default.Connect;
            tstxtIP.Text = Properties.Settings.Default.IP;
            txtTimeGap.Text = Convert.ToString(Properties.Settings.Default.TimeGap);
        }
        #endregion

        #region 連線及離線功能
        private void tsbtnConnect_Click(object sender, EventArgs e)  //連線按鈕
        {
            Properties.Settings.Default.IP = tstxtIP.Text;
            Properties.Settings.Default.Save();

            register = new Register(plc.IPAddress, plc.Port);

            int usbNum = 0;
            if (ComPortBox.Text.Length == 4)
            {
                usbNum = Convert.ToInt16(ComPortBox.Text.Substring(ComPortBox.Text.Length - 1, 1));
            }
            else if (ComPortBox.Text.Length == 5)
            {
                usbNum = Convert.ToInt16(ComPortBox.Text.Substring(ComPortBox.Text.Length - 2, 2));
            }

            if (tscbConnect.SelectedIndex <= 0 && plc.Connect(tstxtIP.Text, tstxtPort.Text) == false)
            {
                tslblConnectStatus.Text = rM.GetString("ConnectFailure");
                tsbtnDisconnect.PerformClick();
            }
            else if ((tscbConnect.SelectedIndex == 1 && ((usb.Connect(usbNum) == false) || plc.Connect(tstxtIP.Text, tstxtPort.Text) == false)))
            {
                tslblConnectStatus.Text = rM.GetString("ConnectFailure");
                tsbtnDisconnect.PerformClick();
            }
            /*
            else if (plc.CheckKey() == false) 
            {
                tslblConnectStatus.Text = rM.GetString("NoKey");
                tsbtnDisconnect.PerformClick();
            }*/
            else
            {
                MachineType fMachineType = new MachineType(plc, infor.getUnit());
                fMachineType.ShowDialog(this);
                infor.setType_Max(fMachineType.Type, fMachineType.PositionMax, fMachineType.ForceMax);
                motionManager.Build_MotionData();
                motionManager.Build_Motion_List(lstMotion);


                chartManager.Initialize_Chart();

                infor.ConnectStatus = 1;

                tmrRun.Enabled = true;           //Run timer 

                Directory.CreateDirectory(infor.TempPath + "\\");
                tscbConnect.Enabled = false;
                ComPortBox.Enabled = false;
                tstxtIP.Enabled = false;
                tstxtPort.Enabled = false;

                infor.Build_Path("");
                chartManager.DrawFrame();
                tmrUI.Enabled = true;
                tmrSlowUI.Enabled = true;
                Start();
                
            }
        }
        private void tsbtnDisconnect_Click(object sender, EventArgs e)   //Execute the disconnection with the mechine   斷線按鈕
        {
            tscbConnect.Enabled = true;
            ComPortBox.Enabled = true;
            tstxtIP.Enabled = true;
            tstxtPort.Enabled = true;

            infor.ConnectStatus = 0;
            if (tscbConnect.SelectedIndex == 1)
            {
                usb.DisConnect();
            }

            others.Clean();
            tmrRun.Enabled = false;

            plc.Disconnect();
        }
        #endregion

        #region 按鈕
        private void btnWrkShet_Click(object sender, EventArgs e)
        {
            if (infor.SaveFlag == -1)  //當壓合紀錄未儲存
            {
                DialogResult result = MessageBox.Show(this, rM.GetString("UnSaveStr"), rM.GetString("UnSave"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    DialogResult changeWorkOrder = MessageBox.Show(this, rM.GetString("ChangeWorkOrder"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (changeWorkOrder == DialogResult.Yes)
                    {
                        MessageBox.Show(this, rM.GetString("chgWOStr") + infor.Barcode, rM.GetString("chgWO"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        infor.ChangeStartDateTime();
                        infor.Build_Path("");
                        liveDataManager.LiveDataNum = 0;
                        changeWrkShet();
                    }
                }
            }
            else if (infor.SaveFlag == 0)    //當壓合紀錄已儲存
            {
                DialogResult changeWorkOrder = MessageBox.Show(this, rM.GetString("ChangeWorkOrder"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (changeWorkOrder == DialogResult.Yes)
                {
                    MessageBox.Show(this, rM.GetString("chgWOStr") + infor.Barcode, rM.GetString("chgWO"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    infor.ChangeStartDateTime();
                    infor.Build_Path("");
                    liveDataManager.LiveDataNum = 0;
                    changeWrkShet();
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)   //export the result data to the excel   "另存新檔"
        {
            ExportFile fExportFile = new ExportFile();
            fExportFile.ShowDialog(this);

            if (fExportFile.Type == 1)
            {
                MessageBox.Show(this, export.ExportCsv(), rM.GetString("SaveOk"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (fExportFile.Type == 2)
            {
                export.ExportXls();
            }
        }

        private void Start() 
        {
            if (tscbConnect.SelectedIndex == 1)
            {
                usb.Start();
                tmrScope.Enabled = true;
            }
            DialogResult zero = MessageBox.Show(this, rM.GetString("Zero"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (zero == DialogResult.Yes)
            {
                tmrWriteAmount.Enabled = true;
            }
            else if ((zero != DialogResult.Yes) && (plc.LiveProductAmount != 0) && (infor.Recorded == 1))
            {
                plc.WriteProductAmount(plc.LiveProductAmount);
                plc.WriteOKAmount(plc.LiveOKAmount);
                plc.WriteNGAmount(plc.LiveNGAmount);


                infor.Recorded = 0;
            }
            //tmrAutoSave.Enabled = true;
        }

        private void tmrWriteAmount_Tick(object sender, EventArgs e)
        {
            if (writeAmountClock == 1)
            {
                plc.WriteProductAmount(0);

            }
            else if (writeAmountClock == 2)
            {
                plc.WriteOKAmount(0);

            }
            else if (writeAmountClock == 3)
            {
                plc.WriteNGAmount(0);
                tmrWriteAmount.Enabled = false;
            }
            writeAmountClock++;
        }

        private void btnResult_Click(object sender, EventArgs e)   // Display the batch data and statistical data on the window 螢幕顯示批量輸出結果
        {
            Report fReport = new Report(infor, liveDataManager);                    //Fundamental report parameter
            fReport.IP = plc.IPAddress;
            fReport.ScaleXMax = chOutput.ChartAreas[0].AxisX.Maximum;
            fReport.ScaleYMax = chOutput.ChartAreas[0].AxisY.Maximum;
            fReport.ScaleXmin = chOutput.ChartAreas[0].AxisX.Minimum;
            fReport.ScaleYmin = chOutput.ChartAreas[0].AxisY.Minimum;
            fReport.OriginalPos = motionManager.MotionData[0].OriginalPos;

            for (int i = 0; i < liveDataManager.LiveDataNum; i++)
            {
                fReport.Press_Data_From_LiveData(i);
            }

            StatsData[] statsData = new StatsData[CONSTANT.StepAmount];                    //Statistical Data  統計資料
            for (int i = 0; i < CONSTANT.StepAmount; i++) { statsData[i] = new StatsData(); }
            statsManager.Stat(statsData);  //統計數據計算 
            for (int iStep = 0; iStep <= motionManager.getLastMotionNum(); iStep++)  
            {
                fReport.statsData[iStep] = statsData[iStep];
            }

            if ((liveDataManager.LiveDataNum - 1) >= 0)
            {
                fReport.ProductAmount = plc.LiveProductAmount;
                fReport.PassAmount = plc.LiveOKAmount;
                fReport.NGAmount = plc.LiveNGAmount;
            }
            fReport.ShowDialog(this);
        }
        private void btnEditMotion_Click(object sender, EventArgs e)
        {
            EditModification(others, chartManager);
            chartManager.DrawFrame();
        }

        private void btnPath_Click(object sender, EventArgs e)   //Change the path for file saving  變更儲存檔案路徑
        {
            filePath.ShowDialog();
            if (filePath.SelectedPath != "") infor.Build_Path(filePath.SelectedPath);
        }
        #endregion

        #region 上排按鈕

        #region 開始製程紀錄
        //Open the new windows for particular functions 開啟製程紀錄檔
        private void 開啟圖檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                OpenFileDialog oExcelFile = new OpenFileDialog();

                oExcelFile.Filter = "xlsx(*.xlsx)|*.xlsx";

                if (oExcelFile.ShowDialog() == DialogResult.OK)
                {
                    Report fReadReport = new Report(infor, liveDataManager);
                    fReadReport.FileName = oExcelFile.FileName;
                    fReadReport.ReadFileFlag = 1;
                    fReadReport.ShowDialog(this);
                }

        }
        #endregion

        #region 載入配方檔
        private void 載入配方檔ToolStripMenuItem_Click(object sender, EventArgs e) //載入外部Excel配方檔
        {
            //if (mbc.Read_PLC_SingleCoil(Ether_Addr.TurnOnStatus.ToString()) == 1)
            if (plc.TurnOnStatus() == 1)
            {
                MessageBox.Show(this, rM.GetString("ImportRecipe"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                OpenFileDialog oExcelFile = new OpenFileDialog();
                oExcelFile.Filter = "xlsx(*.xlsx)|*.xlsx";

                if (oExcelFile.ShowDialog() == DialogResult.OK)
                {
                    //lstMotion.Items.Clear();
                    export.Import_Recipe(oExcelFile.FileName);
                    //motionManager.Build_Motion_List(lstMotion);  //建立步序參數於ListView
                    others.ListView_Subitem_Update(motionManager);
                    txtRecipeNum.Text = "-";   //配方編號顯示
                    txtRecipeName.Text = oExcelFile.FileName;  //配方路徑顯示
                }
            }

            
        }
        #endregion

        #region 載出配方檔
        private void 載出配方檔ToolStripMenuItem_Click(object sender, EventArgs e)   //載出外部Excel配方檔
        {
            ExportRecipe();
        }
        #endregion

        #region 結束
        private void 結束ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 上傳資料庫
        private void mES資料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openSQL();
        }
        #endregion


        #region 參數設定
        private void 參數設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region 語言選擇
        private void 語言選擇ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openLanguage();
        }
        #endregion
        #region 使用說明
        private void 使用說明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openInstruction();
        }
        #endregion

        #region 關於
        private void 關於台達電子壓床ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openAbout();
        }
        #endregion


        #region 忽略光閘
        private void 忽略光閘ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion


        #endregion

        #region 圖表按鈕

        #region X軸變更
        private void cboX_SelectedIndexChanged(object sender, EventArgs e)  //X軸座標變更
        {
            if (cboX.SelectedIndex == 0)
            {
                lblXMax.Text = "mm";
                lblXmin.Text = "mm";
                txtXmin.Text = motionManager.MotionData[0].OriginalPos;
                txtXMax.Text = Convert.ToString((Convert.ToDouble(motionManager.MotionData[0].OriginalPos)+10));
            }
            else if (cboX.SelectedIndex == 1)
            {
                lblXMax.Text = "s";
                lblXmin.Text = "s";
                txtXmin.Text = "0";
                txtXMax.Text = "5";
            }
            ChartChange();
            if (liveDataManager.LiveDataNum == 0)
            {
                chOutput.Series["data0"].Points.AddXY(0, 0);
            }
            chartManager.DrawFrame();
        }
        #endregion

        #region Y軸變更
        private void cboY_SelectedIndexChanged(object sender, EventArgs e)  //Y軸座標變更
        {
            if (cboY.SelectedIndex == 0)
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
                txtYmin.Text = "0";
                txtYMax.Text = "50";
            }
            else if (cboY.SelectedIndex == 1)
            {
                lblYMax.Text = "mm/s";
                lblYmin.Text = "mm/s";
                txtYmin.Text = "0";
                txtYMax.Text = "20";
            }

            else if (cboY.SelectedIndex == 2)
            {
                lblYMax.Text = "mm";
                lblYmin.Text = "mm";
                txtYmin.Text = motionManager.MotionData[0].OriginalPos;
                txtYMax.Text = Convert.ToString(Convert.ToInt32(motionManager.MotionData[0].OriginalPos)+10);
            }
            ChartChange();
            
            if (liveDataManager.LiveDataNum == 0)
            {
                chOutput.Series["data0"].Points.AddXY(0, 0);
            }
            chartManager.DrawFrame();
        }
        #endregion

        #region 變更圖表
        //Change the in-time chart output style 變更即時輸出圖表型式
        //private void btnChartChange_Click(object sender, EventArgs e)
        private void ChartChange()
        {
            //Double i;
            /*
            if (Double.TryParse(txtYMax.Text, out i) == false || Double.TryParse(txtXMax.Text, out i) == false)
            {
                MessageBox.Show(this, rM.GetString("Integer"), rM.GetString("ValueErr"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (chartManager.XMax < chartManager.XMin || ((Double.Parse(txtYMax.Text) < Double.Parse(txtYmin.Text))))
            {
                MessageBox.Show(this, rM.GetString("ScaleErr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            */
            //else
            //{
                chartManager.BufferChart(liveDataManager);
            //}
        }
        #endregion

        #region Lock
        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAuto.Checked == true)
            {
                txtXMax.Enabled = false;
                txtXmin.Enabled = false;
                txtYMax.Enabled = false;
                txtYmin.Enabled = false;
            }
            else if (chkAuto.Checked == false)
            {
                txtXMax.Enabled = true;
                txtXmin.Enabled = true;
                txtYMax.Enabled = true;
                txtYmin.Enabled = true;
                chartManager.Lock();
            }
        }
        #endregion

        #endregion

        private void tmrRun_Tick(object sender, EventArgs e)   //The timer started after running and monitor the action begin "開始記錄"後即時監控狀態
        {
            try
            {
                plc.Scan();
                if (plc.ConnectStatus > 0 && plc.ConnectStatus <= 64)
                {
                    if ((infor.PressStatus == 0) && ((plc.Press == 1))) //當壓合開始
                    {
                        infor.PressStatus = 1;   //壓合狀態 1:向下移動中 0:壓合停止
                        chartManager.Pressing_Start(false);
                        //
                        register = new Register(plc.IPAddress, plc.Port);
                        register.Connect();
                        register.Write_PLC(Ether_Addr.BufferNum, "1");

                        bufferNum = 0;
                        bufferReadStart = 0;
                        bufferReadCount = 0;
                        bufferRead = 0;
                        tmrReadBatch.Enabled = true;
                        //


                        liveDataManager.New_LiveData(infor.Barcode, txtRecipeName.Text);   //建立即時記錄物件

                        if (tscbConnect.SelectedIndex == 1)
                        {
                            if (tmrScope.Enabled == false)
                            {
                                tmrScope.Enabled = true;
                            }
                            usb.Trigger();
                        }
                    }

                    if (infor.PressStatus == 1 && (plc.LiveOKNG != 0) && plc.LiveStep >= 2)
                    {
                        plc.NomalUpdate();
                        liveDataManager.LiveData_Save_StaticValue(plc.LiveStep - 1 - 1, motionManager.MotionData[plc.LiveStep - 1].Cpk);
                    }

                    if ((infor.PressStatus == 1) && (plc.LiveOKNG != 0) && (plc.Press == 0))
                    {
                        if (tscbConnect.SelectedIndex == 1)
                        {
                            usb.Scope_Time_Tick(1);
                        }
                        tmrReadBatch.Enabled = false;

                        if (register.ibufferIndex == 0)
                        {
                            register.ibufferIndex = 1;
                            bufferRead = 1;
                        }
                        else if (register.ibufferIndex == 1)
                        {
                            register.ibufferIndex = 2;
                            bufferRead = 1;
                        }
                        else if (register.ibufferIndex == 2)
                        {
                            register.ibufferIndex = 1;
                            bufferRead = 1;
                        }

                        Read();
                        register = null;

                        infor.PressStatus = 0;
                        plc.NomalUpdate();
                        liveDataManager.LiveData_Save_AfterPress();
                        liveDataManager.LiveData_SaveSingleTime(plc.LiveProductTime);

                        //if (liveStatus.RecordOn == 1)
                        //{
                        liveDataManager.LiveData_SaveLiveDataNum();
                        liveDataManager.LiveData_Save_StaticValue(plc.LiveStep - 1, motionManager.MotionData[plc.LiveStep - 1].Cpk);

                        if (tscbConnect.SelectedIndex == 0)
                        {
                            Temp_Output(Convert.ToDouble(txtTimeGap.Text));   //即時圖表座標暫存txt輸出
                        }
                        else if (tscbConnect.SelectedIndex == 1)
                        {
                            usb.End();
                            Temp_Output(usb, Convert.ToDouble(txtTimeGap.Text));   //即時圖表座標暫存txt輸出
                        }
                        chartManager.Pressing_Start(false);
                        chartManager.Pressing_End();

                        if (sql.SQLOn == "1")
                        {
                            sql.Upload_SQL(plc.IPAddress, infor.Barcode);
                        }
                        liveDataManager.LiveDataNum_Add();

                        chartManager.BufferChart(liveDataManager);

                        export.ExportCsv();
                        //chOutput.SaveImage(infor.Path + "\\Chart\\"+infor.Barcode+"_"+infor.StartDateTime+"_"+Convert.ToString(liveDataManager.LiveDataNum) + ".jpg", ChartImageFormat.Jpeg);
                        infor.SaveFlag = -1;
                        checkMaxData();
                    }
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private void DisconnectSave() 
        {
            if (tscbConnect.SelectedIndex == 0)
            {
                Temp_Output(Convert.ToDouble(txtTimeGap.Text));   //即時圖表座標暫存txt輸出
            }
            else if (tscbConnect.SelectedIndex == 1)
            {
                usb.End();
                Temp_Output(usb, Convert.ToDouble(txtTimeGap.Text));   //即時圖表座標暫存txt輸出
            }
            export.ExportCsv();
        }

        private void Disconnect() 
        {
            tmrRun.Enabled = false;
            MessageBox.Show(this, "Connection Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            tsbtnDisconnect.PerformClick();

            if (infor.PressStatus == 1)
            {
                DisconnectSave();
            }
            
            
        }

        private void checkMaxData()
        {
            if (liveDataManager.LiveDataNum > CONSTANT.LiveDataAmount)
            {
                export.ExportCsv();
                liveDataManager.LiveDataNum = 0;
                plc.ReturnToZero();
            }

        }

        private void tmrReadBatch_Tick(object sender, EventArgs e)
        {

            if ((bufferReadStart == 0) && (int)register.Read_PLC(Ether_Addr.BufferNum) == 2)
            {
                register.ibufferIndex = 1;
                bufferReadStart = 1;
                bufferRead = 1;
            }
            else if ((bufferReadStart == 1) && (register.ibufferIndex == 1) && (int)register.Read_PLC(Ether_Addr.BufferNum) == 1)
            {
                register.ibufferIndex = 2;
                bufferRead = 1;
            }
            else if ((bufferReadStart == 1) && (register.ibufferIndex == 2) && (int)register.Read_PLC(Ether_Addr.BufferNum) == 2)
            {
                register.ibufferIndex = 1;
                bufferRead = 1;
            }

            if (bufferRead == 1)
            {
                Read();
                //DrawChart();
                bufferReadCount++;
            }
        }

        private void tmrAutoSave_Tick(object sender, EventArgs e)
        {
            //autoSave.Tick();
        }

        private void Function_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Environment.Exit(Environment.ExitCode);
        }

        private void Function_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
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
                if (e.Location != tl)
                    tt.SetToolTip(chOutput, string.Format("X={0:0.###} ; Y={1:0.###}", x, y));
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

        private void Scope_Time_Tick(object sender, EventArgs e)
        {
            if (infor.PressStatus > 0) 
            {
                if (usb.Scope_Time_Tick(infor.PressStatus) == false) tmrScope.Enabled = false;
            }
            
        }

        protected void Temp_Output(USB usb,double _timeGap)     //即時圖表座標暫存txt輸出
        {
            curvePath = infor.TempPath + "\\temp_" + liveDataManager.LiveDataNum + ".txt";
            usb.ChartTime = bufferSort.Export(usb.ChartTime, 3, bufferNum);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(infor.TempPath + "\\temp_" + liveDataManager.LiveDataNum + ".txt"))
            {
                    if (plc.Unit == 0) file.WriteLine("Position(mm),Force(kgf),Velocity(mm/s),Time(s)");
                    else if (plc.Unit == 1) file.WriteLine("Position(mm),Force(N),Velocity(mm/s),Time(s)");
                    else if(plc.Unit == 2) file.WriteLine("Position(mm),Force(lbf),Velocity(mm/s),Time(s)");

                    int j = 0;
                    timeTemp = 0;
                    //double timeGap = Convert.ToDouble(txtTimeGap.Text);
                    while (j < usb.TotalAmount)
                    {
                        if ((j > 10 && usb.ChartTime[j] >= liveDataManager.LiveData[liveDataManager.LiveDataNum].SingleTime))
                        {
                            break;
                        }
                        file.WriteLine(usb.ChartPosition[j] + "," + usb.ChartForce[j] + "," + usb.ChartVelocity[j] + "," + usb.ChartTime[j]);
                        
                        if (j != 0)
                        {
                             //file.WriteLine(usb.ChartPosition[j] + "," + usb.ChartForce[j] + "," + usb.ChartVelocity[j] + "," + usb.ChartTime[j]);
                             if ((_timeGap < 10) || (usb.ChartTime[j] - timeTemp > (_timeGap /(double) 1000)))
                             {
                                file.WriteLine(usb.ChartPosition[j] + "," + usb.ChartForce[j] + "," + usb.ChartVelocity[j] + "," + usb.ChartTime[j]);
                                timeTemp = usb.ChartTime[j];
                             }
                        }
                        else
                        {
                            file.WriteLine(usb.ChartPosition[j] + "," + usb.ChartForce[j] + ",0,0");
                            timeTemp = usb.ChartTime[j];
                        }
                        j++;
                    }


                if (plc.LivePressTime == 0)
                {
                    file.WriteLine(plc.PressedPos + "," + plc.PressedForce + "," + 0 + "," + plc.LiveProductTime);
                }
                else if (j > 0 && plc.LivePressTime > 0)
                {
                        file.WriteLine(plc.PressedPos + "," + usb.ChartForce[j - 1] + "," + 0 + "," + plc.LiveProductTime);
                }

            }
        }

        private void tscbConnect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tscbConnect.SelectedIndex == 0)
            {
                ComPortBox.Enabled = false;
                txtTimeGap.Enabled = false;
            }
            else if (tscbConnect.SelectedIndex == 1)
            {
                ComPortBox.Enabled = true;
                txtTimeGap.Enabled = true;
            }
            Properties.Settings.Default.Connect = tscbConnect.SelectedIndex;
            Properties.Settings.Default.Save();

        }

        private void tmrUI_Tick(object sender, EventArgs e)
        {
            txtOriginalPos.Text = motionManager.MotionData[0].OriginalPos;  //顯示工作原點,預備位置,預備時間
            txtPreparePos.Text = motionManager.MotionData[0].StandbyPos;
            txtPrepareTime.Text = motionManager.MotionData[0].StandbyTime;
            lblStandbyVelV.Text = motionManager.MotionData[0].StandbyVel;

            txtStep.Text = Convert.ToString(plc.LiveStep);
            Step_Press_Condition(plc.LiveStep);

            txtCurrentPos.Text = Convert.ToString(plc.LivePos);
            txtCurrentForce.Text = Convert.ToString(plc.LiveForce);

            if (plc.LiveOKNG == 0)
            {
                lblPressResultV.Text = "0";
            }
            else if (plc.LiveOKNG == 1)
            {
                lblPressResultV.Text = "OK";
            }
            else if (plc.LiveOKNG == 2)
            {
                lblPressResultV.Text = "NG";
            }

            txtSingleTime.Text = Convert.ToString(plc.LiveProductTime);
            lblPressTimV.Text = Convert.ToString(plc.LivePressTime);
            lblStandbyTimV.Text = Convert.ToString(plc.LiveStandbyTime);

            lblPressPosV.Text = Convert.ToString(plc.PressedPos);
            lblPressForceV.Text = Convert.ToString(plc.PressedForce);

            if (infor.PressStatus == 1)
            {
                if (tscbConnect.SelectedIndex == 0) chartManager.Live_Chart();
                else if (tscbConnect.SelectedIndex == 1) chartManager.Live_Chart(usb);
            }
        }

        public string Message(int num)   ///HMI介面顯示的狀態訊息
        {
            if (num == 0)
            {
                picMain.Image = Properties.Resources._0;
                return rM.GetString("PutOn");
            }
            else if (num == 1)
            {
                picMain.Image = Properties.Resources._1;
                return rM.GetString("Down");
            }
            else if (num == 2)
            {
                picMain.Image = Properties.Resources._2;
                return rM.GetString("AtIni");
            }
            else if (num == 3)
            {
                picMain.Image = Properties.Resources._3;
                return rM.GetString("Pressing");
            }
            else if (num == 4)
            {
                picMain.Image = Properties.Resources._4;
                return rM.GetString("GoHome");
            }
            else if (num == 5)
            {
                picMain.Image = Properties.Resources._5;
                return rM.GetString("ForceLNG");
            }
            else if (num == 6)
            {
                picMain.Image = Properties.Resources._5;
                return rM.GetString("ForceSNG");
            }
            else if (num == 7)
            {
                picMain.Image = Properties.Resources._5;
                return rM.GetString("PosLNG");
            }
            else if (num == 8)
            {
                picMain.Image = Properties.Resources._5;
                return rM.GetString("PosSNG");
            }
            else if (num == 9)
            {
                picMain.Image = Properties.Resources._5;
                return rM.GetString("ExceedForceLimit");
            }
            else if (num == 10)
            {
                picMain.Image = Properties.Resources._5;
                return rM.GetString("ExceedPosLimit");
            }
            return " ";
        }

        public void Step_Press_Condition(int temp)
        {
            int step = temp -1;
            if (step < CONSTANT.StepAmount && step >= 0)
            {
                if (motionManager.MotionData[step].Mode == 1)   //Position Mode
                {
                    txtMode.Text = rM.GetString("PosMode");
                    lbl1.Text = rM.GetString("PressPos");
                    lbl2.Text = rM.GetString("PressVel");
                    lbl3.Text = rM.GetString("minForceLimit");
                    lbl4.Text = rM.GetString("MaxForceLimit");
                    lbl5.Text = "";
                    lbl6.Text = "";
                    lbl7.Text = "";


                    lbl1V.Text = motionManager.MotionData[step].PressPos;
                    lbl2V.Text = motionManager.MotionData[step].PressVel;
                    
                    lbl5V.Text = "";
                    lbl6V.Text = "";
                    lbl7V.Text = "";

                    lbl1u.Text = "mm";
                    lbl2u.Text = "mm/s";

                    if (infor.Unit == "kgf")
                    {
                        lbl3u.Text = "kgf";
                        lbl4u.Text = "kgf";
                        lbl3V.Text = motionManager.MotionData[step].EndMinForce;
                        lbl4V.Text = motionManager.MotionData[step].EndMaxForce;
                    }
                    else if (infor.Unit == "N")
                    {
                        lbl3u.Text = "N";
                        lbl4u.Text = "N";
                        lbl3V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMinForce)*10);
                        lbl4V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMaxForce)*10);
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lbl3u.Text = "lbf";
                        lbl4u.Text = "lbf";
                        lbl3V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMinForce) / 2);
                        lbl4V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMaxForce) / 2);
                    }
                    lbl5u.Text = "";
                    lbl6u.Text = "";
                    lbl7u.Text = "";
                }
                else if (motionManager.MotionData[step].Mode == 2)  //Force Mode
                {
                    txtMode.Text = rM.GetString("ForceMode");
                    lbl1.Text = rM.GetString("PressForce");
                    lbl2.Text = rM.GetString("PressVel");
                    lbl3.Text = rM.GetString("minPosLimit");
                    lbl4.Text = rM.GetString("MaxPosLimit");
                    lbl5.Text = "";
                    lbl6.Text = "";
                    lbl7.Text = "";

                    
                    lbl2V.Text = motionManager.MotionData[step].PressVel;
                    lbl3V.Text = motionManager.MotionData[step].EndMinPos;
                    lbl4V.Text = motionManager.MotionData[step].EndMaxPos;
                    lbl5V.Text = "";
                    lbl6V.Text = "";
                    lbl7V.Text = "";

                    if (infor.Unit == "kgf")
                    {
                        lbl1u.Text = "kgf";
                        lbl1V.Text = motionManager.MotionData[step].PressForce;
                    }
                    else if (infor.Unit == "N")
                    {
                        lbl1u.Text = "N";
                        lbl1V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].PressForce) * 10);
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lbl1u.Text = "lbf";
                        lbl1V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].PressForce) / 2);
                    }

                    lbl2u.Text = "mm/s";
                    lbl3u.Text = "mm";
                    lbl4u.Text = "mm";
                    lbl5u.Text = "";
                    lbl6u.Text = "";
                    lbl7u.Text = "";
                }
                else if (motionManager.MotionData[step].Mode == 3)  //Distance Mode
                {
                    txtMode.Text = rM.GetString("DistMode");
                    lbl1.Text = rM.GetString("PressDist");
                    lbl2.Text = rM.GetString("PressVel");
                    lbl3.Text = rM.GetString("minPosLimit");
                    lbl4.Text = rM.GetString("MaxPosLimit");
                    lbl5.Text = rM.GetString("minForceLimit");
                    lbl6.Text = rM.GetString("MaxForceLimit");
                    lbl7.Text = "";

                    lbl1V.Text = motionManager.MotionData[step].PressPos;
                    lbl2V.Text = motionManager.MotionData[step].PressVel;
                    lbl3V.Text = motionManager.MotionData[step].EndMinPos;
                    lbl4V.Text = motionManager.MotionData[step].EndMaxPos;
                    
                    lbl7V.Text = "";

                    lbl1u.Text = "mm";
                    lbl2u.Text = "mm/s";
                    lbl3u.Text = "mm";
                    lbl4u.Text = "mm";

                    if (infor.Unit == "kgf")
                    {
                        lbl5u.Text = "kgf";
                        lbl6u.Text = "kgf";
                        lbl5V.Text = motionManager.MotionData[step].EndMinForce;
                        lbl6V.Text = motionManager.MotionData[step].EndMaxForce;
                    }
                    else if (infor.Unit == "N")
                    {
                        lbl5u.Text = "N";
                        lbl6u.Text = "N";
                        lbl5V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMinForce) * 10);
                        lbl6V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMaxForce) * 10);
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lbl5u.Text = "lbf";
                        lbl6u.Text = "lbf";
                        lbl5V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMinForce) / 2);
                        lbl6V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].EndMaxForce) / 2);
                    }
                    lbl7u.Text = "";
                }
                else if (motionManager.MotionData[step].Mode == 4)  //Force Position Mode
                {
                    txtMode.Text = rM.GetString("ForcePosMode");
                    lbl1.Text = rM.GetString("PressForce");
                    lbl2.Text = rM.GetString("PressPos");
                    lbl3.Text = rM.GetString("PressVel");
                    lbl4.Text = "";
                    lbl5.Text = "";
                    lbl6.Text = "";
                    lbl7.Text = "";

                    
                    lbl2V.Text = motionManager.MotionData[step].PressPos;
                    lbl3V.Text = motionManager.MotionData[step].PressVel;
                    lbl4V.Text = "";
                    lbl5V.Text = "";
                    lbl6V.Text = "";
                    lbl7V.Text = "";


                    if (infor.Unit == "kgf")
                    {
                        lbl1u.Text = "kgf";
                        lbl1V.Text = motionManager.MotionData[step].PressForce;
                    }
                    else if (infor.Unit == "N")
                    {
                        lbl1u.Text = "N";
                        lbl1V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].PressForce) * 10);
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lbl1u.Text = "lbf";
                        lbl1V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].PressForce) / 2);
                    }

                    lbl2u.Text = "mm";
                    lbl3u.Text = "mm/s";
                    lbl4u.Text = "";
                    lbl5u.Text = "";
                    lbl6u.Text = "";
                    lbl7u.Text = "";
                }
                else if (motionManager.MotionData[step].Mode == 5)  //Force Disance Mode
                {
                    txtMode.Text = rM.GetString("ForceDistMode");
                    lbl1.Text = rM.GetString("PressForce");
                    lbl2.Text = rM.GetString("PressDist");
                    lbl3.Text = rM.GetString("PressVel");
                    lbl4.Text = rM.GetString("minPosLimit");
                    lbl5.Text = rM.GetString("MaxPosLimit");
                    lbl6.Text = "";
                    lbl7.Text = "";

                    
                    lbl2V.Text = motionManager.MotionData[step].PressPos;
                    lbl3V.Text = motionManager.MotionData[step].PressVel;
                    lbl4V.Text = motionManager.MotionData[step].EndMinPos;
                    lbl5V.Text = motionManager.MotionData[step].EndMaxPos;

                    lbl6V.Text = "";
                    lbl7V.Text = "";


                    if (infor.Unit == "kgf")
                    {
                        lbl1u.Text = "kgf";
                        lbl1V.Text = motionManager.MotionData[step].PressForce;
                    }
                    else if (infor.Unit == "N")
                    {
                        lbl1u.Text = "N";
                        lbl1V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].PressForce) * 10); 
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lbl1u.Text = "lbf";
                        lbl1V.Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[step].PressForce) / 2);
                    }
                    lbl2u.Text = "mm";
                    lbl3u.Text = "mm/s";
                    lbl4u.Text = "mm";
                    lbl5u.Text = "mm";
                    lbl6u.Text = "";
                    lbl7u.Text = "";
                }
                else if (motionManager.MotionData[step].Mode == 6)  //IOSignal
                {
                    txtMode.Text = rM.GetString("IOSignal");
                    lbl1.Text = "";
                    lbl2.Text = "";
                    lbl3.Text = "";
                    lbl4.Text = "";
                    lbl5.Text = "";
                    lbl6.Text = "";
                    lbl7.Text = "";

                    lbl1V.Text = "";
                    lbl2V.Text = "";
                    lbl3V.Text = "";
                    lbl4V.Text = "";
                    lbl5V.Text = "";
                    lbl6V.Text = "";
                    lbl7V.Text = "";

                    if (infor.Unit == "kgf")
                    {
                        lbl1u.Text = "";
                    }
                    else if (infor.Unit == "N")
                    {
                        lbl1u.Text = "";
                    }
                    else if (infor.Unit == "lbf")
                    {
                        lbl1u.Text = "";
                    }
                    lbl2u.Text = "";
                    lbl3u.Text = "";
                    lbl4u.Text = "";
                    lbl5u.Text = "";
                    lbl6u.Text = "";
                    lbl7u.Text = "";
                }
                else
                {
                    lbl1.Text = "";
                    lbl2.Text = "";
                    lbl3.Text = "";
                    lbl4.Text = "";
                    lbl5.Text = "";
                    lbl6.Text = "";
                    lbl7.Text = "";

                    lbl1V.Text = "";
                    lbl2V.Text = "";
                    lbl3V.Text = "";
                    lbl4V.Text = "";
                    lbl5V.Text = "";
                    lbl6V.Text = "";
                    lbl7V.Text = "";

                    lbl1u.Text = "";
                    lbl2u.Text = "";
                    lbl3u.Text = "";
                    lbl4u.Text = "";
                    lbl5u.Text = "";
                    lbl6u.Text = "";
                    lbl7u.Text = "";
                }
            }
        }

        private void CurveUI()
        {
            if (chkAuto.Checked == true)
            {
                chOutput.ChartAreas[0].AxisY.Minimum = chartManager.YMin;
                chOutput.ChartAreas[0].AxisY.Maximum = chartManager.YMax;
                chOutput.ChartAreas[0].AxisX.Minimum = chartManager.XMin;
                chOutput.ChartAreas[0].AxisX.Maximum = chartManager.XMax;

                txtXmin.Text = Convert.ToString(chartManager.XMin);
                txtXMax.Text = Convert.ToString(chartManager.XMax);
                txtYmin.Text = Convert.ToString(chartManager.YMin);
                txtYMax.Text = Convert.ToString(chartManager.YMax);
            }
            else if (chkAuto.Checked == false) 
            {
                chOutput.ChartAreas[0].AxisY.Minimum = chartManager.LockYMin;
                chOutput.ChartAreas[0].AxisY.Maximum = chartManager.LockYMax;
                chOutput.ChartAreas[0].AxisX.Minimum = chartManager.LockXMin;
                chOutput.ChartAreas[0].AxisX.Maximum = chartManager.LockXMax;

                txtXmin.Text = Convert.ToString(chartManager.LockXMin);
                txtXMax.Text = Convert.ToString(chartManager.LockXMax);
                txtYmin.Text = Convert.ToString(chartManager.LockYMin);
                txtYMax.Text = Convert.ToString(chartManager.LockYMax);
            }

            chOutput.ChartAreas[0].AxisX.Interval = Math.Round((chOutput.ChartAreas[0].AxisX.Maximum - chOutput.ChartAreas[0].AxisX.Minimum) / 5, 2);
            chOutput.ChartAreas[0].AxisY.Interval = Math.Round((chOutput.ChartAreas[0].AxisY.Maximum - chOutput.ChartAreas[0].AxisY.Minimum) / 5, 2);

            


            if (plc.RecipeChangedSave == 1)
            {
                chartManager.DrawFrame();
                plc.RecipeChangedSave = 0;
            }

            if (cboX.SelectedIndex == 0) chOutput.ChartAreas[0].AxisX.Title = rM.GetString("PosUC");
            else if (cboX.SelectedIndex == 1) chOutput.ChartAreas[0].AxisX.Title = rM.GetString("TimeUC");

            if (cboY.SelectedIndex == 0)
            {
                if (infor.Unit == "kgf") chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceUC");
                else if (infor.Unit == "N") chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceNC");
                else if (infor.Unit == "lbf") chOutput.ChartAreas[0].AxisY.Title = rM.GetString("ForceLC");
            }
            else if (cboY.SelectedIndex == 1) chOutput.ChartAreas[0].AxisY.Title = rM.GetString("VelUC");
            else if (cboY.SelectedIndex == 2) chOutput.ChartAreas[0].AxisY.Title = rM.GetString("PosUC");
        }



        private void txtXmin_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtXmin.Text, out temp) == true)
            {
                if (temp < chartManager.XMax)
                {
                    if (chkAuto.Checked == true)
                    {
                        chartManager.XMin = temp;
                    }
                    else if (chkAuto.Checked == false)
                    {
                        chartManager.LockXMin = temp;
                        chartManager.XMin = chartManager.LockXMin;
                    }
                }
            }
        }

        private void txtXMax_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtXMax.Text, out temp) == true)
            {
                if (temp > chartManager.XMin)
                {
                    if (chkAuto.Checked == true)
                    {
                        chartManager.XMax = temp;
                    }
                    else if (chkAuto.Checked == false)
                    {
                        chartManager.LockXMax= temp;
                        chartManager.XMax = chartManager.LockXMax;
                    }
                }
            }
        }

        private void txtYmin_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtYmin.Text, out temp) == true)
            {
                if (temp < chartManager.YMax)
                {
                    if (chkAuto.Checked == true)
                    {
                        chartManager.YMin = temp;
                    }
                    else if (chkAuto.Checked == false)
                    {
                        chartManager.LockYMin = temp;
                        chartManager.YMin = chartManager.LockYMin;
                    }
                    
                }
            }
        }

        private void txtYMax_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtYMax.Text, out temp) == true)
            {
                if (temp > chartManager.YMin)
                {
                    if (chkAuto.Checked == true)
                    {
                        chartManager.YMax = temp;
                    }
                    else if (chkAuto.Checked == false)
                    {
                        chartManager.LockYMax = temp;
                        chartManager.YMax = chartManager.LockYMax;
                    }
                }
            }
        }

        private void chOutput_Click(object sender, EventArgs e)
        {

        }

        private void lblOriginalPos_Click(object sender, EventArgs e)
        {

        }

        private void lblPreparePos_Click(object sender, EventArgs e)
        {

        }

        private void lblStandbyVel_Click(object sender, EventArgs e)
        {

        }

        private void lblPrepareTime_Click(object sender, EventArgs e)
        {

        }

        private void lblCurrentStep_Click(object sender, EventArgs e)
        {

        }

        private void lbl1_Click(object sender, EventArgs e)
        {

        }

        private void lbl2_Click(object sender, EventArgs e)
        {

        }

        private void lbl3_Click(object sender, EventArgs e)
        {

        }

        private void lbl4_Click(object sender, EventArgs e)
        {

        }

        private void lbl5_Click(object sender, EventArgs e)
        {

        }

        private void txtMode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStep_TextChanged(object sender, EventArgs e)
        {

        }

        private void tmrSlowUI_Tick(object sender, EventArgs e)
        {
            txtProductAmount.Text = Convert.ToString(plc.LiveProductAmount);
            txtPassAmount.Text = Convert.ToString(plc.LiveOKAmount);
            txtNGAmount.Text = Convert.ToString(plc.LiveNGAmount);
            if (Int32.Parse(txtProductAmount.Text) == 0)
            {
                txtPassRatio.Text = "0%";
            }
            else
            {
                txtPassRatio.Text = Convert.ToString(Math.Round(((double.Parse(txtPassAmount.Text) / double.Parse(txtProductAmount.Text)) * 100), 1)) + "%";
            }

            if (infor.Unit == "kgf")
            {
                lblCurrentPress.Text = rM.GetString("LiveForceU");
                lblPressForceU.Text = rM.GetString("kgf");

                lstItemPressForce.Text = rM.GetString("PressForceU");
                lstItemMaxForceLimit.Text = rM.GetString("MaxForceLimitU");
                lstItemMinForceLimit.Text = rM.GetString("minForceLimitU");
                //lblYmin.Text = rM.GetString("kgf");
                //lblYMax.Text = rM.GetString("kgf");
            }
            else if (infor.Unit == "N")
            {
                lblCurrentPress.Text = rM.GetString("LiveForceN");
                lblPressForceU.Text = rM.GetString("N");

                lstItemPressForce.Text = rM.GetString("PressForceN");
                lstItemMaxForceLimit.Text = rM.GetString("MaxForceLimitN");
                lstItemMinForceLimit.Text = rM.GetString("minForceLimitN");
                //lblYmin.Text = rM.GetString("N");
                //lblYMax.Text = rM.GetString("N");
            }
            else if (infor.Unit == "lbf")
            {
                lblCurrentPress.Text = rM.GetString("LiveForceL");
                lblPressForceU.Text = rM.GetString("lbf");

                lstItemPressForce.Text = rM.GetString("PressForceL");
                lstItemMaxForceLimit.Text = rM.GetString("MaxForceLimitL");
                lstItemMinForceLimit.Text = rM.GetString("minForceLimitL");
                //lblYmin.Text = rM.GetString("lbf");
                //lblYMax.Text = rM.GetString("lbf");
            }
            txtMessage.Text = Message(plc.LiveStatus);　　　//HMI介面顯示的狀態訊息

            txtRecipeNum.Text = Convert.ToString(plc.RecipeNum);
            txtRecipeName.Text = plc.RecipeName;


            if (plc.TurnOnStatus() == 1)
            {
                btnEditMotion.Enabled = false;
                txtStart.Text = rM.GetString("ON");
            }
            else
            {
                btnEditMotion.Enabled = true;
                txtStart.Text = rM.GetString("UnON");
            }

            if (infor.ConnectStatus == 1)
            {
                載入配方檔ToolStripMenuItem.Enabled = true;
                載出配方檔ToolStripMenuItem.Enabled = true;
                //功能ToolStripMenuItem.Enabled = true;

                tslblConnectStatus.Text = rM.GetString("Connecting");               //Connecting Status display and connecting button
                tslblConnectStatus.ForeColor = Color.FromArgb(1, 0, 173, 240);
                tsbtnConnect.Enabled = false;
                tsbtnDisconnect.Enabled = true;

                others.SlowRun();

            }
            else
            {
                載入配方檔ToolStripMenuItem.Enabled = false;
                載出配方檔ToolStripMenuItem.Enabled = false;
                //功能ToolStripMenuItem.Enabled = false;

                tslblConnectStatus.Text = rM.GetString("Offline");           //連線按鈕   
                tslblConnectStatus.ForeColor = Color.Gray;
                tsbtnConnect.Enabled = true;
                tsbtnDisconnect.Enabled = false;
            }

            if (plc.Initialized == 1)
            {
                lblINI.Text = rM.GetString("Initialized");
                lblINI.ForeColor = Color.FromArgb(0, 135, 220);
            }
            else
            {
                lblINI.Text = rM.GetString("Uninitialized");
                lblINI.ForeColor = Color.Red;
            }

            if (plc.Press == 1)
            {
                cboX.Enabled = false;
                cboY.Enabled = false;
            }
            else
            {
                cboX.Enabled = true;
                cboY.Enabled = true;
            }


            txtBarCode.Text = infor.Barcode;
            txtPath.Text = infor.Path;

            CurveUI();
        }

        private void tscbConnect_Click(object sender, EventArgs e)
        {
            
        }

        private void txtTimeGap_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TimeGap = Convert.ToInt32(txtTimeGap.Text);
            Properties.Settings.Default.Save();
        }
    }
}
