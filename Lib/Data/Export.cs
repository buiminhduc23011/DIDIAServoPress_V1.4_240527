using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Resources;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Drawing;

namespace DIAServoPress
{
    public class Export
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private PLC plc;
        private ServoPressInfor infor;
        
        private MotionManager motionManager;
        private LiveDataManager liveDataManager;  
        private StatsManager statsManager;
        
        public Export(PLC plc,StatsManager statsManager, ServoPressInfor infor, MotionManager motionManager, LiveDataManager liveDataManager) 
        {
            this.plc = plc;
            this.infor = infor;
            this.motionManager = motionManager;
            this.liveDataManager = liveDataManager;
            this.statsManager = statsManager;
        }
        
        public string ExportCsv()
        {
            Directory.CreateDirectory(infor.Path + "\\Chart");     //建立儲存位址目錄
            if (infor.Path != "")  //儲存路徑非空值
            {
                infor.SaveFlag = 0;    //已儲存旗標 -1:未儲存 0:已儲存

                export_Chart_cvs(infor.Path, infor.Barcode, infor.StartDateTime);   //圖表cvs檔輸出
                
                //csv_begin
                Directory.CreateDirectory(infor.Path);     //建立儲存位址目錄
                if (infor.Path != "")  //儲存路徑非空值
                {
                    infor.SaveFlag = 0;    //已儲存旗標 -1:未儲存 0:已儲存

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(infor.Path + "\\DIAServoPress_" + infor.Barcode + "_" + infor.StartDateTime + ".csv", false, Encoding.UTF8))
                    {
                        file.WriteLine(infor.StartDateTime);
                        file.WriteLine(DateTime.Today.ToString("yyyy/MM/dd"));
                        file.WriteLine(infor.Type + "(" + infor.Unit + ")");

                        file.WriteLine(rM.GetString("WorkOrder") + "," + infor.Barcode);

                        file.WriteLine(rM.GetString("Pass") + "," + Convert.ToString(plc.LiveOKAmount));
                        file.WriteLine(rM.GetString("NG") + "," + Convert.ToString(plc.LiveNGAmount));
                        file.WriteLine(rM.GetString("Total") + "," + Convert.ToString(plc.LiveProductAmount));

                        file.WriteLine();
                        StatsData[] statsData = new StatsData[CONSTANT.StepAmount];   //統計資料
                        for (int i = 0; i < CONSTANT.StepAmount; i++) { statsData[i] = new StatsData(); }
                        statsManager.Stat(statsData);  //統計數據計算 

                        file.WriteLine(rM.GetString("Step") + "," + rM.GetString("PressMode") + "," + rM.GetString("Value") + "," + rM.GetString("Mid") + "," + rM.GetString("UpperLimit") + "," + rM.GetString("LowerLimit") + "," + rM.GetString("Ave") + "," + rM.GetString("Std") + "," + "Cpk");

                        if (motionManager.MotionData[0].Mode != 0)        //各步序統計數據輸出
                        {
                            file.WriteLine("1" + "," + motionManager.getModeName(motionManager.MotionData[0].Mode) + "," + statsValue(motionManager.MotionData[0].Mode) + "," + statsData[0].MidValue + "," + statsData[0].Upper + "," + statsData[0].Lower + "," + Math.Round(statsData[0].Average, 2) + "," + Math.Round(statsData[0].SD, 2) + "," + Math.Round(statsData[0].Cpk, 2));
                        }
                        if (motionManager.MotionData[1].Mode != 0)
                        {
                            file.WriteLine("2" + "," + motionManager.getModeName(motionManager.MotionData[1].Mode) + "," + statsValue(motionManager.MotionData[1].Mode) + "," + statsData[1].MidValue + "," + statsData[1].Upper + "," + statsData[1].Lower + "," + Math.Round(statsData[1].Average, 2) + "," + Math.Round(statsData[1].SD, 2) + "," + Math.Round(statsData[1].Cpk, 2));
                        }
                        if (motionManager.MotionData[2].Mode != 0)
                        {
                            file.WriteLine("3" + "," + motionManager.getModeName(motionManager.MotionData[2].Mode) + "," + statsValue(motionManager.MotionData[2].Mode) + "," + statsData[2].MidValue + "," + statsData[2].Upper + "," + statsData[2].Lower + "," + Math.Round(statsData[2].Average, 2) + "," + Math.Round(statsData[2].SD, 2) + "," + Math.Round(statsData[2].Cpk, 2));
                        }
                        if (motionManager.MotionData[3].Mode != 0)
                        {
                            file.WriteLine("4" + "," + motionManager.getModeName(motionManager.MotionData[3].Mode) + "," + statsValue(motionManager.MotionData[3].Mode) + "," + statsData[3].MidValue + "," + statsData[3].Upper + "," + statsData[3].Lower + "," + Math.Round(statsData[3].Average, 2) + "," + Math.Round(statsData[3].SD, 2) + "," + Math.Round(statsData[3].Cpk, 2));
                        }
                        if (motionManager.MotionData[4].Mode != 0)
                        {
                            file.WriteLine("5" + "," + motionManager.getModeName(motionManager.MotionData[4].Mode) + "," + statsValue(motionManager.MotionData[4].Mode) + "," + statsData[4].MidValue + "," + statsData[4].Upper + "," + statsData[4].Lower + "," + Math.Round(statsData[4].Average, 2) + "," + Math.Round(statsData[4].SD, 2) + "," + Math.Round(statsData[4].Cpk, 2));
                        }
                        //
                        if (motionManager.MotionData[5].Mode != 0)
                        {
                            file.WriteLine("6" + "," + motionManager.getModeName(motionManager.MotionData[5].Mode) + "," + statsValue(motionManager.MotionData[5].Mode) + "," + statsData[5].MidValue + "," + statsData[5].Upper + "," + statsData[5].Lower + "," + Math.Round(statsData[5].Average, 2) + "," + Math.Round(statsData[5].SD, 2) + "," + Math.Round(statsData[5].Cpk, 2));
                        }
                        if (motionManager.MotionData[6].Mode != 0)
                        {
                            file.WriteLine("7" + "," + motionManager.getModeName(motionManager.MotionData[6].Mode) + "," + statsValue(motionManager.MotionData[6].Mode) + "," + statsData[6].MidValue + "," + statsData[6].Upper + "," + statsData[6].Lower + "," + Math.Round(statsData[6].Average, 2) + "," + Math.Round(statsData[6].SD, 2) + "," + Math.Round(statsData[6].Cpk, 2));
                        }
                        if (motionManager.MotionData[7].Mode != 0)
                        {
                            file.WriteLine("8" + "," + motionManager.getModeName(motionManager.MotionData[7].Mode) + "," + statsValue(motionManager.MotionData[7].Mode) + "," + statsData[7].MidValue + "," + statsData[7].Upper + "," + statsData[7].Lower + "," + Math.Round(statsData[7].Average, 2) + "," + Math.Round(statsData[7].SD, 2) + "," + Math.Round(statsData[7].Cpk, 2));
                        }
                        if (motionManager.MotionData[8].Mode != 0)
                        {
                            file.WriteLine("9" + "," + motionManager.getModeName(motionManager.MotionData[8].Mode) + "," + statsValue(motionManager.MotionData[8].Mode) + "," + statsData[8].MidValue + "," + statsData[8].Upper + "," + statsData[8].Lower + "," + Math.Round(statsData[8].Average, 2) + "," + Math.Round(statsData[8].SD, 2) + "," + Math.Round(statsData[8].Cpk, 2));
                        }
                        if (motionManager.MotionData[9].Mode != 0)
                        {
                            file.WriteLine("10" + "," + motionManager.getModeName(motionManager.MotionData[9].Mode) + "," + statsValue(motionManager.MotionData[9].Mode) + "," + statsData[9].MidValue + "," + statsData[9].Upper + "," + statsData[9].Lower + "," + Math.Round(statsData[9].Average, 2) + "," + Math.Round(statsData[9].SD, 2) + "," + Math.Round(statsData[9].Cpk, 2));
                        }

                        file.WriteLine();

                        if (infor.Unit == "kgf")
                        {
                            file.WriteLine(rM.GetString("Num") + "," + rM.GetString("Time") + "," + rM.GetString("RecipeName") + "," + rM.GetString("MeasureResult") + "," + rM.GetString("StandbyPosU") + "," + rM.GetString("StandbyTimeU") + "," + rM.GetString("PressPosU") + "," + rM.GetString("ForceU") + "," + rM.GetString("PressTimeU") + "," + rM.GetString("ProductTimeU"));
                        }
                        else if (infor.Unit == "N")
                        {
                            file.WriteLine(rM.GetString("Num") + "," + rM.GetString("Time") + "," + rM.GetString("RecipeName") + "," + rM.GetString("MeasureResult") + "," + rM.GetString("StandbyPosU") + "," + rM.GetString("StandbyTimeU") + "," + rM.GetString("PressPosU") + "," + rM.GetString("ForceN") + "," + rM.GetString("PressTimeU") + "," + rM.GetString("ProductTimeU"));
                        }
                        else if (infor.Unit == "lbf")
                        {
                            file.WriteLine(rM.GetString("Num") + "," + rM.GetString("Time") + "," + rM.GetString("RecipeName") + "," + rM.GetString("MeasureResult") + "," + rM.GetString("StandbyPosU") + "," + rM.GetString("StandbyTimeU") + "," + rM.GetString("PressPosU") + "," + rM.GetString("ForceL") + "," + rM.GetString("PressTimeU") + "," + rM.GetString("ProductTimeU"));
                        }

                        for (int i = 0; i < liveDataManager.LiveDataNum; i++)
                        {
                            file.WriteLine(Convert.ToString(i + 1) + "," + liveDataManager.LiveData[i].StartTime + "," + liveDataManager.LiveData[i].RecipeName + "," + result(liveDataManager.LiveData[i].Result) + "," + Convert.ToString(liveDataManager.LiveData[i].StandbyPos) + "," + Convert.ToString(liveDataManager.LiveData[i].StandbyTime) + "," + Convert.ToString(liveDataManager.LiveData[i].PressPos) + "," + Convert.ToString(liveDataManager.LiveData[i].PressForce) + "," + Convert.ToString(liveDataManager.LiveData[i].PressTime) + "," + Convert.ToString(liveDataManager.LiveData[i].SingleTime) + "," + infor.Barcode + "_" + infor.StartDateTime + "_" + (i + 1) + ".csv");
                        }
                    }
                }
                return rM.GetString("SaveOk") + " " + infor.Path + "DIAServoPress_" + infor.Barcode + "_" + infor.StartDateTime + ".csv";
            }
            return "";
        }



        public void ExportXls()
        {
            Directory.CreateDirectory(infor.Path + "\\Chart");     //建立儲存位址目錄
            if (infor.Path != "")  //儲存路徑非空值
            {
                infor.SaveFlag = 0;    //已儲存旗標 -1:未儲存 0:已儲存

                export_Chart_cvs(infor.Path, infor.Barcode, infor.StartDateTime);   //圖表cvs檔輸出
                Excel.Application xlsReport;
                Excel._Workbook wBook;
                Excel._Worksheet wSheet;
                Excel.Range wRange;

                xlsReport = new Excel.Application();
                xlsReport.Visible = true;
                xlsReport.DisplayAlerts = false;

                xlsReport.Workbooks.Add(Type.Missing);
                wBook = xlsReport.Workbooks[1];
                wBook.Activate();

                wSheet = (Excel._Worksheet)wBook.Worksheets[1];
                wSheet.Name = rM.GetString("ResultReport");

                // 設定工作表焦點
                wSheet.Activate();

                // 設定第1列資料
                int iDataPos = 13;   //Excel中開始記錄單筆記錄行號
                xlsReport.Cells[iDataPos, 1] = rM.GetString("Num");
                xlsReport.Cells[iDataPos, 2] = rM.GetString("Time");
                xlsReport.Cells[iDataPos, 3] = rM.GetString("RecipeName");
                xlsReport.Cells[iDataPos, 4] = rM.GetString("MeasureResult");
                xlsReport.Cells[iDataPos, 5] = rM.GetString("StandbyPosU");
                xlsReport.Cells[iDataPos, 6] = rM.GetString("StandbyTimeU");
                xlsReport.Cells[iDataPos, 7] = rM.GetString("PressPosU");

                if (infor.Unit == "kgf")
                {
                    xlsReport.Cells[iDataPos, 8] = rM.GetString("ForceU");
                }
                else if (infor.Unit == "N")
                {
                    xlsReport.Cells[iDataPos, 8] = rM.GetString("ForceN");
                }
                else if (infor.Unit == "lbf")
                {
                    xlsReport.Cells[iDataPos, 8] = rM.GetString("ForceL");
                }

                xlsReport.Cells[iDataPos, 9] = rM.GetString("PressTimeU");
                xlsReport.Cells[iDataPos, 10] = rM.GetString("ProductTimeU");
                xlsReport.Cells[iDataPos, 11] = rM.GetString("Chart");

                //單次壓合結果輸出
                for (int i = 0; i < liveDataManager.LiveDataNum; i++)
                {
                    xlsReport.Cells[iDataPos + i + 1, 1] = Convert.ToString(i + 1);
                    xlsReport.Cells[iDataPos + i + 1, 2] = liveDataManager.LiveData[i].StartTime;
                    xlsReport.Cells[iDataPos + i + 1, 3] = liveDataManager.LiveData[i].RecipeName;
                    xlsReport.Cells[iDataPos + i + 1, 4] = result(liveDataManager.LiveData[i].Result);
                    xlsReport.Cells[iDataPos + i + 1, 5] = Convert.ToString(liveDataManager.LiveData[i].StandbyPos);
                    xlsReport.Cells[iDataPos + i + 1, 6] = Convert.ToString(liveDataManager.LiveData[i].StandbyTime);
                    xlsReport.Cells[iDataPos + i + 1, 7] = Convert.ToString(liveDataManager.LiveData[i].PressPos);
                    xlsReport.Cells[iDataPos + i + 1, 8] = Convert.ToString(liveDataManager.LiveData[i].PressForce);
                    xlsReport.Cells[iDataPos + i + 1, 9] = Convert.ToString(liveDataManager.LiveData[i].PressTime);
                    xlsReport.Cells[iDataPos + i + 1, 10] = Convert.ToString(liveDataManager.LiveData[i].SingleTime);
                    //xlsReport.Cells[iDataPos + i + 1, 11] = infor.Barcode + "_" + infor.StartDateTime + "_" + (i + 1) + ".csv";
                    wSheet.Hyperlinks.Add(wSheet.Cells[iDataPos + i + 1, 11], "Chart\\"+infor.Barcode + "_" + infor.StartDateTime + "_" + (i + 1) + ".csv", Type.Missing, "Result", infor.Barcode + "_" + infor.StartDateTime + "_" + (i + 1) + ".csv");
                }

                //批量統計資料輸出
                if ((liveDataManager.LiveDataNum - 1) >= 0)
                {
                    xlsReport.Cells[1, 2] = infor.Type;
                    xlsReport.Cells[1, 3] = DateTime.Today;
                    xlsReport.Cells[2, 2] = rM.GetString("WorkOrder");
                    xlsReport.Cells[2, 3] = infor.Barcode;
                    xlsReport.Cells[1, 1] = infor.StartDateTime;
                    xlsReport.Cells[1, 1].Font.Color = Color.White;
                    xlsReport.Cells[2, 1] = infor.Unit;
                    xlsReport.Cells[2, 1].Font.Color = Color.White;
                    xlsReport.Cells[3, 2] = rM.GetString("Pass");
                    xlsReport.Cells[3, 3] = Convert.ToString(plc.LiveOKAmount);
                    xlsReport.Cells[4, 2] = rM.GetString("NG");
                    xlsReport.Cells[4, 3] = Convert.ToString(plc.LiveNGAmount);
                    xlsReport.Cells[5, 2] = rM.GetString("Total");
                    xlsReport.Cells[5, 3] = Convert.ToString(plc.LiveProductAmount);

                    StatsData[] statsData = new StatsData[CONSTANT.StepAmount];   //統計資料
                    for (int i = 0; i < CONSTANT.StepAmount; i++) { statsData[i] = new StatsData(); }
                    statsManager.Stat(statsData);  //統計數據計算 

                    xlsReport.Cells[1, 4] = rM.GetString("Step");
                    xlsReport.Cells[1, 5] = rM.GetString("PressMode");
                    xlsReport.Cells[1, 6] = rM.GetString("Value");
                    xlsReport.Cells[1, 7] = rM.GetString("Mid");
                    xlsReport.Cells[1, 8] = rM.GetString("UpperLimit");
                    xlsReport.Cells[1, 9] = rM.GetString("LowerLimit");
                    xlsReport.Cells[1, 10] = rM.GetString("Ave");
                    xlsReport.Cells[1, 11] = rM.GetString("Std");
                    xlsReport.Cells[1, 12] = "Cpk";

                    if (motionManager.MotionData[0].Mode != 0)        //各步序統計數據輸出
                    {
                        xlsReport.Cells[2, 4] = "1";
                        xlsReport.Cells[2, 5] = motionManager.getModeName(motionManager.MotionData[0].Mode); //"動作模式"
                        xlsReport.Cells[2, 6] = statsValue(motionManager.MotionData[0].Mode);  //依模式編號輸出模式中文名稱
                        xlsReport.Cells[2, 7] = statsData[0].MidValue;
                        xlsReport.Cells[2, 8] = statsData[0].Upper;
                        xlsReport.Cells[2, 9] = statsData[0].Lower;
                        xlsReport.Cells[2, 10] = Math.Round(statsData[0].Average, 2);
                        xlsReport.Cells[2, 11] = Math.Round(statsData[0].SD, 2);
                        xlsReport.Cells[2, 12] = Math.Round(statsData[0].Cpk, 2);
                    }
                    if (motionManager.MotionData[1].Mode != 0)
                    {
                        xlsReport.Cells[3, 4] = "2";
                        xlsReport.Cells[3, 5] = motionManager.getModeName(motionManager.MotionData[1].Mode);
                        xlsReport.Cells[3, 6] = statsValue(motionManager.MotionData[1].Mode);
                        xlsReport.Cells[3, 7] = statsData[1].MidValue;
                        xlsReport.Cells[3, 8] = statsData[1].Upper;
                        xlsReport.Cells[3, 9] = statsData[1].Lower;
                        xlsReport.Cells[3, 10] = Math.Round(statsData[1].Average, 2);
                        xlsReport.Cells[3, 11] = Math.Round(statsData[1].SD, 2);
                        xlsReport.Cells[3, 12] = Math.Round(statsData[1].Cpk, 2);
                    }
                    if (motionManager.MotionData[2].Mode != 0)
                    {
                        xlsReport.Cells[4, 4] = "3";
                        xlsReport.Cells[4, 5] = motionManager.getModeName(motionManager.MotionData[2].Mode);
                        xlsReport.Cells[4, 6] = statsValue(motionManager.MotionData[2].Mode);
                        xlsReport.Cells[4, 7] = statsData[2].MidValue;
                        xlsReport.Cells[4, 8] = statsData[2].Upper;
                        xlsReport.Cells[4, 9] = statsData[2].Lower;
                        xlsReport.Cells[4, 10] = Math.Round(statsData[2].Average, 2);
                        xlsReport.Cells[4, 11] = Math.Round(statsData[2].SD, 2);
                        xlsReport.Cells[4, 12] = Math.Round(statsData[2].Cpk, 2);
                    }
                    if (motionManager.MotionData[3].Mode != 0)
                    {
                        xlsReport.Cells[5, 4] = "4";
                        xlsReport.Cells[5, 5] = motionManager.getModeName(motionManager.MotionData[3].Mode);
                        xlsReport.Cells[5, 6] = statsValue(motionManager.MotionData[3].Mode);
                        xlsReport.Cells[5, 7] = statsData[3].MidValue;
                        xlsReport.Cells[5, 8] = statsData[3].Upper;
                        xlsReport.Cells[5, 9] = statsData[3].Lower;
                        xlsReport.Cells[5, 10] = Math.Round(statsData[3].Average, 2);
                        xlsReport.Cells[5, 11] = Math.Round(statsData[3].SD, 2);
                        xlsReport.Cells[5, 12] = Math.Round(statsData[3].Cpk, 2);
                    }
                    if (motionManager.MotionData[4].Mode != 0)
                    {
                        xlsReport.Cells[6, 4] = "5";
                        xlsReport.Cells[6, 5] = motionManager.getModeName(motionManager.MotionData[4].Mode);
                        xlsReport.Cells[6, 6] = statsValue(motionManager.MotionData[4].Mode);
                        xlsReport.Cells[6, 7] = statsData[4].MidValue;
                        xlsReport.Cells[6, 8] = statsData[4].Upper;
                        xlsReport.Cells[6, 9] = statsData[4].Lower;
                        xlsReport.Cells[6, 10] = Math.Round(statsData[4].Average, 2);
                        xlsReport.Cells[6, 11] = Math.Round(statsData[4].SD, 2);
                        xlsReport.Cells[6, 12] = Math.Round(statsData[4].Cpk, 2);
                    }
                    //
                    if (motionManager.MotionData[5].Mode != 0)
                    {
                        xlsReport.Cells[7, 4] = "6";
                        xlsReport.Cells[7, 5] = motionManager.getModeName(motionManager.MotionData[5].Mode);
                        xlsReport.Cells[7, 6] = statsValue(motionManager.MotionData[5].Mode);
                        xlsReport.Cells[7, 7] = statsData[5].MidValue;
                        xlsReport.Cells[7, 8] = statsData[5].Upper;
                        xlsReport.Cells[7, 9] = statsData[5].Lower;
                        xlsReport.Cells[7, 10] = Math.Round(statsData[5].Average, 2);
                        xlsReport.Cells[7, 11] = Math.Round(statsData[5].SD, 2);
                        xlsReport.Cells[7, 12] = Math.Round(statsData[5].Cpk, 2);
                    }
                    if (motionManager.MotionData[6].Mode != 0)
                    {
                        xlsReport.Cells[8, 4] = "7";
                        xlsReport.Cells[8, 5] = motionManager.getModeName(motionManager.MotionData[6].Mode);
                        xlsReport.Cells[8, 6] = statsValue(motionManager.MotionData[6].Mode);
                        xlsReport.Cells[8, 7] = statsData[6].MidValue;
                        xlsReport.Cells[8, 8] = statsData[6].Upper;
                        xlsReport.Cells[8, 9] = statsData[6].Lower;
                        xlsReport.Cells[8, 10] = Math.Round(statsData[6].Average, 2);
                        xlsReport.Cells[8, 11] = Math.Round(statsData[6].SD, 2);
                        xlsReport.Cells[8, 12] = Math.Round(statsData[6].Cpk, 2);
                    }
                    if (motionManager.MotionData[7].Mode != 0)
                    {
                        xlsReport.Cells[9, 4] = "8";
                        xlsReport.Cells[9, 5] = motionManager.getModeName(motionManager.MotionData[7].Mode);
                        xlsReport.Cells[9, 6] = statsValue(motionManager.MotionData[7].Mode);
                        xlsReport.Cells[9, 7] = statsData[7].MidValue;
                        xlsReport.Cells[9, 8] = statsData[7].Upper;
                        xlsReport.Cells[9, 9] = statsData[7].Lower;
                        xlsReport.Cells[9, 10] = Math.Round(statsData[7].Average, 2);
                        xlsReport.Cells[9, 11] = Math.Round(statsData[7].SD, 2);
                        xlsReport.Cells[9, 12] = Math.Round(statsData[7].Cpk, 2);
                    }
                    if (motionManager.MotionData[8].Mode != 0)
                    {
                        xlsReport.Cells[10, 4] = "9";
                        xlsReport.Cells[10, 5] = motionManager.getModeName(motionManager.MotionData[8].Mode);
                        xlsReport.Cells[10, 6] = statsValue(motionManager.MotionData[8].Mode);
                        xlsReport.Cells[10, 7] = statsData[8].MidValue;
                        xlsReport.Cells[10, 8] = statsData[8].Upper;
                        xlsReport.Cells[10, 9] = statsData[8].Lower;
                        xlsReport.Cells[10, 10] = Math.Round(statsData[8].Average, 2);
                        xlsReport.Cells[10, 11] = Math.Round(statsData[8].SD, 2);
                        xlsReport.Cells[10, 12] = Math.Round(statsData[8].Cpk, 2);
                    }
                    if (motionManager.MotionData[9].Mode != 0)
                    {
                        xlsReport.Cells[11, 4] = "10";
                        xlsReport.Cells[11, 5] = motionManager.getModeName(motionManager.MotionData[9].Mode);
                        xlsReport.Cells[11, 6] = statsValue(motionManager.MotionData[9].Mode);
                        xlsReport.Cells[11, 7] = statsData[9].MidValue;
                        xlsReport.Cells[11, 8] = statsData[9].Upper;
                        xlsReport.Cells[11, 9] = statsData[9].Lower;
                        xlsReport.Cells[11, 10] = Math.Round(statsData[9].Average, 2);
                        xlsReport.Cells[11, 11] = Math.Round(statsData[9].SD, 2);
                        xlsReport.Cells[11, 12] = Math.Round(statsData[9].Cpk, 2);
                    }


                    //Excel 表格調整
                    wRange = wSheet.Range[wSheet.Cells[1, 2], wSheet.Cells[1, 10]];
                    wRange.Select();
                    wRange.ColumnWidth = 13;

                    wRange = wSheet.Range["A1"];
                    wRange.ColumnWidth = 5;

                    wRange = wSheet.Range["D1"];
                    wRange.ColumnWidth = 10;

                    wRange = wSheet.Range["C1"];
                    wRange.ColumnWidth = 16;

                    wRange = wSheet.Range["K1"];
                    wRange.ColumnWidth = 20;

                    wRange = wSheet.Range["L1"];
                    wRange.ColumnWidth = 11;

                    wRange = wSheet.Range["A:L"];
                    wRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    wRange = wSheet.Range["A1:J6"];
                    wRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    wRange = wSheet.Range["D1:L6"];
                    wRange.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;

                    wRange = wSheet.Range["A1"];
                }
                wBook.SaveAs(infor.Path + "DIAServoPress_" + infor.Barcode + "_" + infor.StartDateTime, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
        }

        public void Import_Recipe(string fileName)
        {
            //讀取Excel檔案
            Excel.Application xlsReport;
            Excel._Workbook wBook;
            Excel._Worksheet wSheet;
            Excel.Range wRange;

            xlsReport = new Excel.Application();
            xlsReport.Visible = false;
            xlsReport.DisplayAlerts = false;

            object missing = System.Reflection.Missing.Value;
            wBook = xlsReport.Workbooks.Open(fileName, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            wSheet = (Excel._Worksheet)wBook.Worksheets[1];
            wSheet.Activate();
            wRange = wSheet.UsedRange;

            string mode;

            motionManager.MotionData[0].OriginalPos = Convert.ToString(wRange.Cells[2, 4].Value);
            motionManager.MotionData[0].OriginalVel = Convert.ToString(wRange.Cells[2, 3].Value);
            motionManager.MotionData[0].StandbyPos = Convert.ToString(wRange.Cells[2, 6].Value);
            motionManager.MotionData[0].StandbyVel = Convert.ToString(wRange.Cells[2, 5].Value);
            motionManager.MotionData[0].StandbyTime = Convert.ToString(wRange.Cells[2, 7].Value);

            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                mode = Convert.ToString(wRange.Cells[i + 2, 2].Value);
                if (mode == rM.GetString("Motionless")) motionManager.MotionData[i].Mode = 0;
                else if (mode == rM.GetString("PosMode")) motionManager.MotionData[i].Mode = 1;
                else if (mode == rM.GetString("ForceMode")) motionManager.MotionData[i].Mode = 2;
                else if (mode == rM.GetString("DistMode")) motionManager.MotionData[i].Mode = 3;
                else if (mode == rM.GetString("ForcePosMode")) motionManager.MotionData[i].Mode = 4;
                else if (mode == rM.GetString("ForceDistMode")) motionManager.MotionData[i].Mode = 5;
                else if (mode == rM.GetString("IOSignal")) motionManager.MotionData[i].Mode = 6;

                if (i != 0)
                {
                    motionManager.MotionData[i].OriginalPos = "0";
                    motionManager.MotionData[i].OriginalVel = "0";
                    motionManager.MotionData[i].StandbyPos = "0";
                    motionManager.MotionData[i].StandbyVel = "0";
                    motionManager.MotionData[i].StandbyTime = "0";
                }

                if (Convert.ToString(wRange.Cells[i + 2, 8].Value) == "-")
                {
                    motionManager.MotionData[i].PressVel = "0";
                }
                else
                {
                    motionManager.MotionData[i].PressVel = Convert.ToString(wRange.Cells[i + 2, 8].Value);
                }
                if (Convert.ToString(wRange.Cells[i + 2, 9].Value) == "-")
                {
                    motionManager.MotionData[i].PressPos = "0";
                }
                else
                {
                    motionManager.MotionData[i].PressPos = Convert.ToString(wRange.Cells[i + 2, 9].Value);
                }
                if (Convert.ToString(wRange.Cells[i + 2, 10].Value) == "-")
                {
                    motionManager.MotionData[i].PressForce = "0";
                }
                else
                {
                    motionManager.MotionData[i].PressForce = Convert.ToString(wRange.Cells[i + 2, 10].Value);
                }
                if (Convert.ToString(wRange.Cells[i + 2, 11].Value) == "-")
                {
                    motionManager.MotionData[i].PressTime = "0";
                }
                else
                {
                    motionManager.MotionData[i].PressTime = Convert.ToString(wRange.Cells[i + 2, 11].Value);
                }
                if (Convert.ToString(wRange.Cells[i + 2, 12].Value) == "-")
                {
                    motionManager.MotionData[i].EndMaxPos = "0";
                }
                else
                {
                    motionManager.MotionData[i].EndMaxPos = Convert.ToString(wRange.Cells[i + 2, 12].Value);
                }
                if (Convert.ToString(wRange.Cells[i + 2, 13].Value) == "-")
                {
                    motionManager.MotionData[i].EndMinPos = "0";
                }
                else
                {
                    motionManager.MotionData[i].EndMinPos = Convert.ToString(wRange.Cells[i + 2, 13].Value);
                }

                if (Convert.ToString(wRange.Cells[i + 2, 14].Value) == "-")
                {
                    motionManager.MotionData[i].EndMaxForce = "0";
                }
                else
                {
                    motionManager.MotionData[i].EndMaxForce = Convert.ToString(wRange.Cells[i + 2, 14].Value);
                }

                if (Convert.ToString(wRange.Cells[i + 2, 15].Value) == "-")
                {
                    motionManager.MotionData[i].EndMinForce = "0";
                }
                else
                {
                    motionManager.MotionData[i].EndMinForce = Convert.ToString(wRange.Cells[i + 2, 15].Value);
                }

                if (Convert.ToString(wRange.Cells[i + 2, 16].Value) == "-")
                {
                    motionManager.MotionData[i].LimitType = "0";
                }
                else
                {
                    motionManager.MotionData[i].LimitType = Convert.ToString(wRange.Cells[i + 2, 16].Value);
                }

                if (Convert.ToString(wRange.Cells[i + 2, 17].Value) == "-")
                {
                    motionManager.MotionData[i].BeginMaxForce = "0";
                }
                else
                {
                    motionManager.MotionData[i].BeginMaxForce = Convert.ToString(wRange.Cells[i + 2, 17].Value);
                }

                if (Convert.ToString(wRange.Cells[i + 2, 18].Value) == "-")
                {
                    motionManager.MotionData[i].BeginMinForce = "0";
                }
                else
                {
                    motionManager.MotionData[i].BeginMinForce = Convert.ToString(wRange.Cells[i + 2, 18].Value);
                }
            }

            motionManager.MotionData_Write_To_PLC();   //將在Motion物件的步序參數寫入PLC 
            for (int i = 0; i < CONSTANT.StepAmount; i++) motionManager.MotionData_Read_From_PLC(i);  //從PLC讀取各步序參數對應暫存器

            xlsReport =null;
            wBook.Close();
            wBook = null;
            wSheet = null;
            wRange = null;

        }
        public void Export_Recipe(SaveFileDialog filePath)
        {
            Excel.Application xlsReport;
            Excel._Workbook wBook;
            Excel._Worksheet wSheet;

            xlsReport = new Excel.Application();
            xlsReport.Visible = true;
            xlsReport.DisplayAlerts = false;

            xlsReport.Workbooks.Add(Type.Missing);
            wBook = xlsReport.Workbooks[1];
            wBook.Activate();

            wSheet = (Excel._Worksheet)wBook.Worksheets[1];
            //wSheet.Name = "結果報告";
            wSheet.Name = rM.GetString("ResultReport");

            // 設定工作表焦點
            wSheet.Activate();

            // 設定第1列資料
            xlsReport.Cells[1, 1] = rM.GetString("Num");
            xlsReport.Cells[1, 2] = rM.GetString("PressMode");
            xlsReport.Cells[1, 3] = rM.GetString("OriginVelU");
            xlsReport.Cells[1, 4] = rM.GetString("OriginPosU");
            xlsReport.Cells[1, 5] = rM.GetString("StandbyVelU");
            xlsReport.Cells[1, 6] = rM.GetString("StandbyPosU");
            xlsReport.Cells[1, 7] = rM.GetString("StandbyTimeU");
            xlsReport.Cells[1, 8] = rM.GetString("PressVelU");
            xlsReport.Cells[1, 9] = rM.GetString("PressPosU");

            xlsReport.Cells[1, 10] = rM.GetString("ForceU");
            /*
            if (infor.Unit == "kgf")
            {
                xlsReport.Cells[1, 10] = rM.GetString("ForceU");
            }
            else if (infor.Unit == "N")
            {
                xlsReport.Cells[1, 10] = rM.GetString("ForceN");
            }
            else if (infor.Unit == "lbf")
            {
                xlsReport.Cells[1, 10] = rM.GetString("ForceL");
            }*/

            xlsReport.Cells[1, 11] = rM.GetString("PressTimeU");
            xlsReport.Cells[1, 12] = rM.GetString("MaxPosLimitU");
            xlsReport.Cells[1, 13] = rM.GetString("minPosLimitU");

            xlsReport.Cells[1, 16] = rM.GetString("DynamicForceLimit");

            if (infor.Unit == "kgf")
            {
                xlsReport.Cells[1, 14] = rM.GetString("MaxForceLimitU");
                xlsReport.Cells[1, 15] = rM.GetString("minForceLimitU");
                xlsReport.Cells[1, 17] = rM.GetString("BeginMaxForceLimitU");
                xlsReport.Cells[1, 18] = rM.GetString("BeginMinForceLimitU");
            }
            else if (infor.Unit == "N")
            {
                xlsReport.Cells[1, 14] = rM.GetString("MaxForceLimitN");
                xlsReport.Cells[1, 15] = rM.GetString("minForceLimitN");
                xlsReport.Cells[1, 17] = rM.GetString("BeginMaxForceLimitN");
                xlsReport.Cells[1, 18] = rM.GetString("BeginMinForceLimitN");
            }
            else if (infor.Unit == "lbf")
            {
                xlsReport.Cells[1, 14] = rM.GetString("MaxForceLimitL");
                xlsReport.Cells[1, 15] = rM.GetString("minForceLimitL");
                xlsReport.Cells[1, 17] = rM.GetString("BeginMaxForceLimitL");
                xlsReport.Cells[1, 18] = rM.GetString("BeginMinForceLimitL");
            }

            xlsReport.Cells[1, 19] = "註解";

            int motionCreate = 0;
            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                //第一列文字
                xlsReport.Cells[i + 2, 1] = Convert.ToString(i + 1); //"編號";
                xlsReport.Cells[i + 2, 2] = motionManager.getModeName(motionManager.MotionData[i].Mode);//"動作模式";
                if (i == 0)
                {
                    xlsReport.Cells[i + 2, 3] = motionManager.MotionData[i].OriginalVel;//"原點速度(mm/s)";
                    xlsReport.Cells[i + 2, 4] = motionManager.MotionData[motionCreate].OriginalPos;//"原點位置(mm)";
                    xlsReport.Cells[i + 2, 5] = motionManager.MotionData[i].StandbyVel;//"預備速度(mm/s)";
                    xlsReport.Cells[i + 2, 6] = motionManager.MotionData[i].StandbyPos;//"預備位置(mm)";
                    xlsReport.Cells[i + 2, 7] = motionManager.MotionData[i].StandbyTime;//"預備時間(s)";
                }
                else
                {
                    xlsReport.Cells[i + 2, 3] = "-";
                    xlsReport.Cells[i + 2, 4] = "-";
                    xlsReport.Cells[i + 2, 5] = "-";
                    xlsReport.Cells[i + 2, 6] = "-";
                    xlsReport.Cells[i + 2, 7] = "-";
                }

                //輸出各步序設定
                xlsReport.Cells[i + 2, 8] = motionManager.MotionData[i].PressVel;
                xlsReport.Cells[i + 2, 9] = motionManager.MotionData[i].PressPos;
                xlsReport.Cells[i + 2, 10] = motionManager.MotionData[i].PressForce;
                xlsReport.Cells[i + 2, 11] = motionManager.MotionData[i].PressTime;
                xlsReport.Cells[i + 2, 12] = motionManager.MotionData[i].EndMaxPos;
                xlsReport.Cells[i + 2, 13] = motionManager.MotionData[i].EndMinPos;
                xlsReport.Cells[i + 2, 14] = motionManager.MotionData[i].EndMaxForce;
                xlsReport.Cells[i + 2, 15] = motionManager.MotionData[i].EndMinForce;
                xlsReport.Cells[i + 2, 16] = motionManager.MotionData[i].LimitType;
                xlsReport.Cells[i + 2, 17] = motionManager.MotionData[i].BeginMaxForce;
                xlsReport.Cells[i + 2, 18] = motionManager.MotionData[i].BeginMinForce;
            }
            wBook.SaveAs(filePath.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlsReport = null;
            wBook = null;
            wSheet = null;
        }

        private string statsValue(int mode)
        {
            if (mode == 0)
            {
                return "-";
            }
            else if (mode == 1 || mode == 3)
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
            else if (mode == 2 || mode == 4 || mode == 5)
            {
                return rM.GetString("PressPosU");
            }
            return "";
        }           //Convert the mode number into the chinese string 依模式編號輸出模式中文名稱
        private string result(double result)
        {
            if (result == 0)
            {
                return "OK";
            }
            else if (result == 2)
            {
                return "NG";
            }
            else if (result == 5)
            {
                return rM.GetString("ForceLNG");
            }
            else if (result == 6)
            {
                return rM.GetString("ForceSNG");
            }
            else if (result == 7)
            {
                return rM.GetString("PosLNG");
            }
            else if (result == 8)
            {
                return rM.GetString("PosSNG");
            }
            else if (result == 9)
            {
                return rM.GetString("ExceedForceLimit");
            }
            else if (result == 10)
            {
                return rM.GetString("ExceedPosLimit");
            }
            else
            {
                return "";
            }

        }
        private void export_Chart_cvs(string path, string barCode, string dateTime)  //圖表cvs檔輸出
        {
            try
            {
                for (int i = 0; i < liveDataManager.LiveDataNum; i++)
                {
                    File.WriteAllLines(path + "\\Chart\\" + barCode + "_" + dateTime + "_" + (i + 1) + ".csv", File.ReadAllLines(infor.TempPath + "\\temp_" + i + ".txt"));
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
