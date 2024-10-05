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
using mSPC;

#endregion


namespace DIAServoPress
{
    public partial class Basic : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        protected FolderBrowserDialog filePath = new FolderBrowserDialog();    //The object of path for file saved  存檔路徑物件

        protected PLC plc;
        protected Register register;
        protected AutoSave autoSave;

        protected Export export;
        protected SQL sql;

        protected ChartData chartData = new ChartData();
        protected MotionManager motionManager;
        protected LiveDataManager liveDataManager;
        protected StatsManager statsManager;

        protected ServoPressInfor infor;

        protected int writeAmountClock = 0;
        protected int showAlarm = 0;      //The flag of alarm 報警旗標
        protected int delayClock = 0;            //The clock for the several events of delay requirement 

        protected int bufferNum = 0;
        protected int bufferReadStart = 0;
        protected int bufferRead = 0;
        protected int bufferReadCount = 0;
        protected BufferSort bufferSort = new BufferSort();

        private double timeTemp = 0;

        #region 啟動與關閉程式
        public Basic()
        {

        }
        public Basic(string ip,string port)
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
            autoSave = new AutoSave(export);
            
            Connect(ip, port);
            infor = new ServoPressInfor(plc);
            infor.Build_Path("");
        }

        protected void Language_Initialize() 
        {
            if (Properties.Settings.Default.Language == "0")
            {
                Language fLan = new Language();
                fLan.ShowDialog(this);
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        }

        private void Basic_Load(object sender, EventArgs e)
        {

        }

        private void Basic_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult close = MessageBox.Show(this, rM.GetString("OffSysStr"), rM.GetString("OffSys"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (close == DialogResult.No)
            {
                e.Cancel = true;
            }
            else if (close == DialogResult.Yes)     //確定關閉後離線
            {
                try
                {
                    tmrRun.Enabled = false;
                    
                    Directory.Delete(infor.TempPath, true);   //刪除暫存檔

                }
                catch (Exception)
                {
                }
            }
        }    
        private void Basic_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        #endregion

        #region 連線及離線功能
        private void Connect(string ip, string port)
        {
            Properties.Settings.Default.IP = ip;
            Properties.Settings.Default.Save();

            if (plc.Connect(ip, port) == false)
            {
                plc.Disconnect();
            }
            else
            {
                register = new Register(plc.IPAddress, plc.Port);
                MachineType fMachineType = new MachineType(plc, infor.getUnit());
                fMachineType.ShowDialog(this);

                infor.setType_Max(fMachineType.Type, fMachineType.PositionMax, fMachineType.ForceMax);

                motionManager.Build_MotionData();
                //infor.StartPath = Properties.Settings.Default.Path;

                tmrRun.Enabled = true;           //Run timer 

                //infor.TempPath = Application.StartupPath + "\\Temp_" + plc.IPAddress;
                //infor.Build_Barcode();
                Directory.CreateDirectory(infor.TempPath + "\\");

                //liveStatus.RecordOn = 1;

                txtAddress.Text = Convert.ToString(plc.IPAddress);
                chkRecord.Checked = true;
                
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            tmrRun.Enabled = false;

            plc.Disconnect();
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
        #endregion

        #region 按鈕

        private void btnWrkShet_Click(object sender, EventArgs e)
        {
            if (infor.SaveFlag == -1)  //當壓合紀錄未儲存
            {
                DialogResult result = MessageBox.Show(this, rM.GetString("UnSaveStr"), rM.GetString("UnSave"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    MessageBox.Show(this, rM.GetString("chgWOStr") + txtBarCode.Text, rM.GetString("chgWO"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //infor.Build_Barcode();
                    infor.ChangeStartDateTime();
                    infor.Build_Path("");
                    liveDataManager.LiveDataNum = 0;
                    changeWrkShet();
                }
            }
            else if (infor.SaveFlag == 0)    //當壓合紀錄已儲存
            {
                MessageBox.Show(this, rM.GetString("chgWOStr") + txtBarCode.Text, rM.GetString("chgWO"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                //infor.Build_Barcode();
                infor.ChangeStartDateTime();
                infor.Build_Path("");
                liveDataManager.LiveDataNum = 0;
                changeWrkShet();
            }
        }

        protected void changeWrkShet()
        {
            DialogResult zero = MessageBox.Show(this, rM.GetString("Zero"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (zero == DialogResult.Yes)
            {
                plc.ReturnToZero();
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
        #endregion

        protected string curvePath = "";
        protected void Temp_Output(double _timeGap)     //即時圖表座標暫存txt輸出
        {
            curvePath = infor.TempPath + "\\temp_" + liveDataManager.LiveDataNum + ".txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(infor.TempPath + "\\temp_" + liveDataManager.LiveDataNum + ".txt"))
            {
                for (int i = 10; i < bufferNum; i++)
                {
                    if ((chartData.ChartTime[i] >= liveDataManager.LiveData[liveDataManager.LiveDataNum].SingleTime - 0.01) || (chartData.ChartTime[i]> chartData.ChartTime[i+1]))
                    {
                        bufferNum = i-1;
                    }
                }
                chartData.ChartPosition[bufferNum] = plc.PressedPos;
                chartData.ChartForce[bufferNum] = plc.PressedForce;
                chartData.ChartVelocity[bufferNum] = 0;
                chartData.ChartTime[bufferNum] = plc.LiveProductTime;

                
                this.chartData.ChartPosition = bufferSort.Export(this.chartData.ChartPosition, 3, bufferNum);
                this.chartData.ChartForce = bufferSort.Export(this.chartData.ChartForce, 1, bufferNum); 
                this.chartData.ChartVelocity=bufferSort.Export(this.chartData.ChartVelocity,3, bufferNum);
                this.chartData.ChartTime = bufferSort.Export(this.chartData.ChartTime, 3, bufferNum);


                if (plc.Unit == 0)
                {
                    file.WriteLine("Position(mm),Force(kgf),Velocity(mm/s),Time(s)");
                }
                else if (plc.Unit == 1)
                {
                    file.WriteLine("Position(mm),Force(N),Velocity(mm/s),Time(s)");
                }
                else if (plc.Unit == 2)
                {
                    file.WriteLine("Position(mm),Force(lbf),Velocity(mm/s),Time(s)");
                }
                
                    file.WriteLine(chartData.ChartPosition[0] + "," + 0 + "," + chartData.ChartVelocity[0] + "," + chartData.ChartTime[0]);
                    timeTemp = 0;
                    int j = 0;
                    while (j <= bufferNum)
                    {
                        if (j > 1 && chartData.ChartTime[j] != chartData.ChartTime[j - 1])
                        {
                            if ((_timeGap < 10) || (chartData.ChartTime[j] - timeTemp > (_timeGap / (double)1000)))
                            {
                                file.WriteLine(chartData.ChartPosition[j] + "," + chartData.ChartForce[j] + "," + chartData.ChartVelocity[j] + "," + chartData.ChartTime[j]);
                                timeTemp = chartData.ChartTime[j];
                            }
                        }
                        j++;
                    }
            }
        }

        protected void Read()
        {
            plc.Pause = true;
            register.ReConnect();
            bufferRead = 0;
            register.bufferData = register.Read(register.ibufferIndex);
            for (int i = 0; i < (register.getOneBufferSize() / 2); i++)
            {
                chartData.ChartPosition[bufferNum] = (double)register.getPosition(i) / 1000;
                chartData.ChartForce[bufferNum] = (double)register.getForce(i) / 10;
                chartData.ChartVelocity[bufferNum] = (double)register.getVelocity(i) / 1000;
                chartData.ChartTime[bufferNum] = (double)register.getTime(i) / plc.getTimeScale();  //TimeScale
                //chartData.ChartTime[bufferNum] = (double)register.getTime(i) / 10;  //TimeScale
                bufferNum++;
            }
            plc.Pause = false;
        }

        private void tmrSlowRun_Tick(object sender, EventArgs e)
        {
            
        }
        
        //The timer started after running and monitor the action begin "開始記錄"後即時監控狀態
        private void tmrRun_Tick(object sender, EventArgs e)
        {
            plc.Scan();
            if (liveDataManager.LiveDataNum > 0)
            {
                btnExport.Enabled = true;
            }
            else
            {
                btnExport.Enabled = false;
            }


            if (plc.ConnectStatus > 0)     
            {
                if ((infor.PressStatus == 0) && (plc.DIAPress == 1)) //當壓合開始
                {
                    //
                    register = new Register(plc.IPAddress,plc.Port);
                    register.Connect();
                    register.Write_PLC(Ether_Addr.BufferNum, "1");

                    bufferNum = 0;
                    bufferReadStart = 0;
                    bufferReadCount = 0;
                    bufferRead = 0;
                    tmrReadBatch.Enabled = true;   
                    //

                    infor.PressStatus = 1;   //壓合狀態 1:向下移動中 0:壓合停止
                    liveDataManager.New_LiveData(infor.Barcode, plc.RecipeName);   //建立即時記錄物件
                }

                //if ((infor.PressStatus == 1) && (plc.Press == 0) && (plc.LiveVel < 0))
                if ((infor.PressStatus == 1) && (plc.Press == 0))
                {
                    //
                    plc.DIAPressClosed();
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
                    liveDataManager.LiveData_Save_AfterPress();
                    liveDataManager.LiveData_SaveSingleTime(plc.LiveProductTime);


                    //if (liveStatus.RecordOn == 1)
                    //{
                        liveDataManager.LiveData_SaveLiveDataNum();
                        liveDataManager.LiveData_Save_StaticValue(plc.LiveStep - 1, motionManager.MotionData[plc.LiveStep - 1].Cpk);

                        Temp_Output(1);   //即時圖表座標暫存txt輸出
                        
                        if (sql.SQLOn == "1")
                        {
                            sql.Upload_SQL(plc.IPAddress,infor.Barcode);
                        }
                        liveDataManager.LiveDataNum_Add();

                        infor.SaveFlag = -1;
                        autoSave.AddCounter();
                    //}
                }
            }
            else
            {
                tmrRun.Enabled = false;

                MessageBox.Show(this, "Connection Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                plc.Disconnect();
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
                bufferReadCount++;
            }           
        }

        private void tmrReadRecipeDelay_Tick(object sender, EventArgs e)
        {
            delayClock++;
            if (delayClock == 3)
            {
                //txtRecipeName.Text = liveStatus.getRecipe_Name();  //將HMI二進位配方名稱轉換為字串
                //txtRecipeNum.Text = Convert.ToString(plc.RecipeNum);
                plc.Write("RecipeChanged", 1);
                tmrReadRecipeDelay.Enabled = false;    //延遲配方讀取
            }
        }

        private void chkRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecord.Checked == true)
            {
               // liveStatus.RecordOn = 1;

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
                tmrAutoSave.Enabled = true;
            }
            else if (chkRecord.Checked == false)
            {
                //liveStatus.RecordOn = 0;
                infor.Recorded = 1;

                if (liveDataManager.LiveDataNum > 0)
                {
                    infor.SaveFlag = -1;        //已儲存旗標 -1:未儲存
                }
                tmrAutoSave.Enabled = false;
            }
        }

        

        private void chkSQL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSQL.Checked == true)
            {
                MES fMES = new MES(plc.IPAddress, plc.Port);
                fMES.ShowDialog(this);

                if (fMES.DialogResult == DialogResult.OK)
                {
                    chkSQL.Checked = sql.Set_SQL(fMES.SQLOn, fMES.SQLIP, fMES.SQLName, fMES.SQLID, fMES.SQLPW, fMES.SQLLanguage);
                }
            }
            else if (chkSQL.Checked == false)
            {
                sql.SQLOn = "0";
            }

        }

        private void tmrSingleProcess_Tick(object sender, EventArgs e)
        {

        }

        protected void EditModification(Others others,ChartManager chartManager)
        {
            //從Motion步序物件載入操作介面
            Modification fModification = new Modification(plc, infor, motionManager, chartManager, curvePath);
            fModification.ShowDialog(this);

            //從操作介面存入Motion物件紀錄
            if (fModification.DialogResult == DialogResult.OK)
            {
                motionManager.MotionData_Write_To_PLC();   //將在Motion物件的步序參數寫入PLC 
                motionManager.MotionData_Read_From_PLC();   //從PLC讀取各步序參數對應暫存器
                others.ListView_Subitem_Update(motionManager); //從Motion物件，更新Listview資料
            }
        }

        protected void ExportRecipe() 
        {
            SaveFileDialog filePath = new SaveFileDialog();
            filePath.Filter = "xlsx (*.xlsx )|*.xlsx |All files (*.*)|*.*";
            filePath.ShowDialog();

            if (filePath.FileName != "")
            {
                export.Export_Recipe(filePath);
            }
        }

        protected void openSQL() 
        {
            MES fMES = new MES(plc.IPAddress, plc.Port);
            fMES.ShowDialog(this);

            if (fMES.DialogResult == DialogResult.OK)
            {
                chkSQL.Checked = sql.Set_SQL(fMES.SQLOn, fMES.SQLIP, fMES.SQLName, fMES.SQLID, fMES.SQLPW, fMES.SQLLanguage);
            }
        }

        protected void openLanguage() 
        {
            Language fLan = new Language();
            fLan.ShowDialog(this);

            if (fLan.OK == 1)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Function));

                MessageBox.Show(this, rM.GetString("Restart"), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected void openInstruction() 
        {
            try
            {
                ProcessStartInfo startInfo;
                if (Properties.Settings.Default.Language == "zh-TW")
                {
                    startInfo = new ProcessStartInfo("DIAServoPress_TC.pdf");
                }
                else if (Properties.Settings.Default.Language == "zh-Hans")
                {
                    startInfo = new ProcessStartInfo("DIAServoPress_SC.pdf");
                }
                else 
                {
                    startInfo = new ProcessStartInfo("DIAServoPress_EN.pdf");
                }
                Process.Start(startInfo);
            }
            catch
            {
                MessageBox.Show(rM.GetString("PDFErr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void openAbout() 
        {
            About fAbout = new About();
            fAbout.ShowDialog(this);
        }

        private void tmrAutoSave_Tick(object sender, EventArgs e)
        {
            //autoSave.Tick();
        }

        private void tmrUI_Tick(object sender, EventArgs e)
        {
            txtPosition.Text = Convert.ToString(plc.LivePos);
            txtForce.Text = Convert.ToString(plc.LiveForce);

            txtTotal.Text = Convert.ToString(plc.LiveProductAmount);
            txtPass.Text = Convert.ToString(plc.LiveOKAmount);
            txtNG.Text = Convert.ToString(plc.LiveNGAmount);

            txtStatus.Text = plc.LiveMessage;
        }
    }
}
