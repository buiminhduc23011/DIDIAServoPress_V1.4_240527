using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
  
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;

using Graph = System.Windows.Forms.DataVisualization.Charting;

using System.IO;
using System.Windows.Forms.DataVisualization.Charting;


namespace DIAServoPress
{
    public partial class Modification : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private MotionManager motionManager;
        private ServoPressInfor infor;
        private PLC plc;

        private int counter = 0;
        private string curvePath = "";

        private class TempRecipe
        {
            public Limit limit = new Limit();

            private int step = 0;
            private int stepIndex = 0;
            
            public int Step { get { return step; } }

            public void setStep(int value) 
            { 
                step = value;
                stepIndex = value - 1;
                //limit.Step = step;

                basicLimit0Step = "BasicLimit0_" + Convert.ToString(step);
                basicLimit1Step = "BasicLimit1_" + Convert.ToString(step);
                basicLimit2Step = "BasicLimit2_" + Convert.ToString(step);
            }


            //基礎限制圖形步序
            private string basicLimit0Step = "";
            public string BasicLimit0Step
            {
                get { return basicLimit0Step; }
            }
            private string basicLimit1Step = "";
            public string BasicLimit1Step
            {
                get { return basicLimit1Step; }
            }
            private string basicLimit2Step = "";
            public string BasicLimit2Step
            {
                get { return basicLimit2Step; }
            }



            //暫存壓合條件
           
            private string originPos = "0";
            public string OriginPos { get { return originPos; } }
            public void setOriginPos(string value) 
            { 
                if(value == "-") originPos = "0";
                else originPos = value; 
            }

            private string originVel = "0";
            public string OriginVel { get { return originVel; } }
            public void setOriginVel(string value) 
            {
                if (value == "-") originVel = "0";
                else originVel = value;
            }

            private string standbyPos = "0";
            public string StandbyPos { get { return standbyPos; } }
            public void setStandbyPos(string value) 
            {
                if (value == "-") standbyPos = "0";
                else standbyPos = value;
            }

            private string standbyVel = "0";
            public string StandbyVel { get { return standbyVel; } }
            public void setStandbyVel(string value) 
            {
                if (value == "-") standbyVel = "0";
                else standbyVel = value;
            }

            private string standbyTime = "0";
            public string StandbyTime { get { return standbyTime; } }
            public void setStandbyTime(string value) 
            {
                if (value == "-") standbyTime = "0";
                else standbyTime = value;
            }

            private int[] mode = new int[CONSTANT.StepAmount];
            private string[] pressingPos = new string[CONSTANT.StepAmount];
            private string[] pressingForce = new string[CONSTANT.StepAmount];
            private string[] pressingVel = new string[CONSTANT.StepAmount];
            private string[] pressingTime = new string[CONSTANT.StepAmount];

            public int Mode { get { return mode[stepIndex]; } }
            public void setMode(int value) 
            { 
                mode[stepIndex] = value; 
            }

            public int getMode(int value)
            {
                return mode[value];
            }

            public string PressingPos { get { return  pressingPos[stepIndex]; } }
            public void setPressingPos(string value) 
            {
                if (value == "-") pressingPos[stepIndex] = "0";
                else pressingPos[stepIndex] = value;
            }

            public string PressingForce { get { return pressingForce[stepIndex]; } }
            public void setPressingForce(string value) 
            {
                if (value == "-") pressingForce[stepIndex] = "0";
                else pressingForce[stepIndex] = value;
            }

            public string PressingVel { get { return pressingVel[stepIndex]; } }
            public void setPressingVel(string value)
            {
                if (value == "-") pressingVel[stepIndex] = "0";
                else pressingVel[stepIndex] = value;
            }

            public string PressingTime { get { return pressingTime[stepIndex]; } }
            public void setPressingTime(string value) 
            {
                if (value == "-") pressingTime[stepIndex] = "0";
                else pressingTime[stepIndex] = value;
            }
            
            public string BasicEndMaxForce
            {
                get
                {
                    return limit.EndMaxForce[stepIndex];
                }
                set
                {
                    limit.EndMaxForce[stepIndex] = value;
                }
            }
            public string BasicEndMinForce
            {
                get
                {
                    return limit.EndMinForce[stepIndex];
                }
                set
                {
                    limit.EndMinForce[stepIndex] = value;
                }
            }
            public string BasicEndMaxPos
            {
                get
                {
                    return limit.EndMaxPos[stepIndex];
                }
                set
                {
                    limit.EndMaxPos[stepIndex] = value;
                }
            }
            public string BasicEndMinPos
            {
                get
                {
                    return limit.EndMinPos[stepIndex];
                }
                set
                {
                    limit.EndMinPos[stepIndex] = value;
                }
            }

            //訊號模式相關專用參數

            string backupPosition = "";
            string backupForce = "";
            string backupVelocity = "";
            string backupTime = "";

            public void BackupIn() 
            {
                setPressingPos(backupPosition);
                setPressingForce(backupForce);
                setPressingVel(backupVelocity);
                setPressingTime(backupTime);
            }

            public void BackupOut() 
            {
                backupPosition = PressingPos;
                backupForce = PressingForce;
                backupVelocity = PressingVel;
                backupTime = PressingTime;
            }

            public void IOInitialize()
            {
                signalPara_1[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingPos[stepIndex]) * 1000), 2).PadLeft(32, '0').Substring(0, 16), 2);
                signal_1[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingPos[stepIndex]) * 1000), 2).PadLeft(32, '0').Substring(16, 16), 2);
                signalPara_2[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingForce[stepIndex]) * 10), 2).PadLeft(32, '0').Substring(0, 16), 2);
                signal_2[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingForce[stepIndex]) * 10), 2).PadLeft(32, '0').Substring(16, 16), 2);
                signalPara_3[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingVel[stepIndex]) * 1000), 2).PadLeft(32, '0').Substring(0, 16), 2);
                signal_3[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingVel[stepIndex]) * 1000), 2).PadLeft(32, '0').Substring(16, 16), 2);
                signalPara_4[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingTime[stepIndex]) * 10), 2).PadLeft(32, '0').Substring(0, 16), 2);
                signal_4[stepIndex] = Convert.ToInt16(Convert.ToString(Convert.ToInt32(Convert.ToDouble(pressingTime[stepIndex]) * 10), 2).PadLeft(32, '0').Substring(16, 16), 2);
            }
            public void IOSave() 
            {
                pressingPos[stepIndex] = Convert.ToString(Convert.ToDouble(Convert.ToInt32(Convert.ToString(signalPara_1[stepIndex], 2).PadLeft(16, '0') + Convert.ToString(signal_1[stepIndex], 2).PadLeft(16, '0'), 2)) / 1000);
                pressingForce[stepIndex] = Convert.ToString(Convert.ToDouble(Convert.ToInt32(Convert.ToString(signalPara_2[stepIndex], 2).PadLeft(16, '0') + Convert.ToString(signal_2[stepIndex], 2).PadLeft(16, '0'), 2)) / 10);
                pressingVel[stepIndex] = Convert.ToString(Convert.ToDouble(Convert.ToInt32(Convert.ToString(signalPara_3[stepIndex], 2).PadLeft(16, '0') + Convert.ToString(signal_3[stepIndex], 2).PadLeft(16, '0'), 2)) / 1000);
                pressingTime[stepIndex] = Convert.ToString(Convert.ToDouble(Convert.ToInt32(Convert.ToString(signalPara_4[stepIndex], 2).PadLeft(16, '0') + Convert.ToString(signal_4[stepIndex], 2).PadLeft(16, '0'), 2)) / 10);
            }

            private int[] signal_1 = new int[CONSTANT.StepAmount];
            private int[] signal_2 = new int[CONSTANT.StepAmount];
            private int[] signal_3 = new int[CONSTANT.StepAmount];
            private int[] signal_4 = new int[CONSTANT.StepAmount];
            private int[] signalPara_1 = new int[CONSTANT.StepAmount];
            private int[] signalPara_2 = new int[CONSTANT.StepAmount];
            private int[] signalPara_3 = new int[CONSTANT.StepAmount];
            private int[] signalPara_4 = new int[CONSTANT.StepAmount];

            //private int signal_1 = 0;
            public int Signal_1 { get { return signal_1[stepIndex]; } }
            public void setSignal_1(int value) { signal_1[stepIndex] = value; }
            
            //private int signal_2 = 0;
            public int Signal_2 { get { return signal_2[stepIndex]; } }
            public void setSignal_2(int value) { signal_2[stepIndex] = value; }
            
            //private int signal_3 = 0;
            public int Signal_3 { get { return signal_3[stepIndex]; } }
            public void setSignal_3(int value) { signal_3[stepIndex] = value; }
            
            //private int signal_4 = 0;
            public int Signal_4 { get { return signal_4[stepIndex]; } }
            public void setSignal_4(int value) { signal_4[stepIndex] = value; }
            
            
            //private int signalPara_1 = 0;
            public int SignalPara_1 { get { return signalPara_1[stepIndex]; } }
            public void setSignalPara_1(int value) { signalPara_1[stepIndex] = value; }

            //private int signalPara_2 = 0;
            public int SignalPara_2 { get { return signalPara_2[stepIndex]; } }
            public void setSignalPara_2(int value) { signalPara_2[stepIndex] = value; }

            //private int signalPara_3 = 0;
            public int SignalPara_3 { get { return signalPara_3[stepIndex]; } }
            public void setSignalPara_3(int value) { signalPara_3[stepIndex] = value; }

            //private int signalPara_4 = 0;
            public int SignalPara_4 { get { return signalPara_4[stepIndex]; } }
            public void setSignalPara_4(int value) { signalPara_4[stepIndex] = value; }

            private string cpk = "";
            public string Cpk { get { return cpk; } }
            public void setCpk(string value) { cpk = value; }

            //目前選取的限制步序條件

            public void CleanLimitValue() 
            {
                EndMaxForce = "0";
                EndMinForce = "0";
                EndMaxPos = "0";
                EndMinPos = "0";
                BeginMinForce = "0";
                BeginMaxForce = "0";
            }
            
            public string EndMaxForce
            {
                get
                {
                    return limit.EndMaxForce[limit.limitStep - 1];
                }
                set
                {
                    limit.EndMaxForce[limit.limitStep - 1] = value;
                }
            }
            public string EndMinForce
            {
                get
                {
                    return limit.EndMinForce[limit.limitStep - 1];
                }
                set
                {
                    limit.EndMinForce[limit.limitStep - 1] = value;
                }
            }
            public string EndMaxPos
            {
                get
                {
                    return limit.EndMaxPos[limit.limitStep - 1];
                }
                set
                {
                    limit.EndMaxPos[limit.limitStep - 1] = value;
                }
            }
            public string EndMinPos
            {
                get
                {
                    return limit.EndMinPos[limit.limitStep - 1];
                }
                set
                {
                    limit.EndMinPos[limit.limitStep - 1] = value;
                }
            }
            public string BeginMaxForce
            {
                get
                {
                    return limit.BeginMaxForce[limit.limitStep - 1];
                }
                set
                {
                    limit.BeginMaxForce[limit.limitStep - 1] = value;
                }
            }
            public string BeginMinForce
            {
                get
                {
                    return limit.BeginMinForce[limit.limitStep - 1];
                }
                set
                {
                    limit.BeginMinForce[limit.limitStep - 1] =value;
                }
            }
            
        }

        private TempRecipe tempRecipe = new TempRecipe();
        public class Limit
        {
            protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);

            public Limit() 
            {
                /*
                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    beginMinForce[i] = "0";
                    beginMaxForce[i] = "0";
                    endMinForce[i] = "0";
                    endMaxForce[i] = "0";
                    endMinPos[i] = "0";
                    endMaxPos[i] = "0";
                }*/
            }

            //限制條件步序
            private int limit = 0;
            private int limitStepIndex = 0;
            public int limitStep 
            {
                get 
                {
                    if (limit == 0) return 1;
                    return limit; 
                }
            }

            public void SetLimitStep(int value) 
            {
                limit = value;
                limitStepIndex = limit - 1;

                leftStep = "left" + Convert.ToString(value);
                rightStep = "right" + Convert.ToString(value);
                topStep = "top" + Convert.ToString(value);
                bottomStep = "bottom" + Convert.ToString(value);

            }

            private int[] mode = new int[CONSTANT.StepAmount];
            private string[] limitType = new string[CONSTANT.StepAmount];
            private string[] beginMinForce = new string[CONSTANT.StepAmount];
            private string[] beginMaxForce = new string[CONSTANT.StepAmount];
            private string[] endMinForce = new string[CONSTANT.StepAmount];
            private string[] endMaxForce = new string[CONSTANT.StepAmount];
            private string[] endMinPos = new string[CONSTANT.StepAmount];
            private string[] endMaxPos = new string[CONSTANT.StepAmount];

            public int[] Mode
            {
                get
                {
                    return mode;
                }
            }

            public int LimitType
            {
                get
                {
                    return Convert.ToInt32(getLimitType(limitStepIndex));
                }

                set
                {
                    setLimitType(limitStepIndex, Convert.ToString(value));
                }
            }




            public string getLimitType(int num) 
            {
                return limitType[num];
            }

            public void setLimitType(int num, string value)
            {
                limitType[num] = value;
            }

            public string getLimitName(int step)
            {
                if (getLimitType(step) == "0" || getLimitType(step) == "10")
                {
                    if (Mode[step] == 0) return rM.GetString("NoLimit");
                    else return rM.GetString("BasicLimit");
                }
                if (getLimitType(step) == "1") return rM.GetString("DynamicForceLimit");
                if (getLimitType(step) == "2") return rM.GetString("FrameLimit1");
                if (getLimitType(step) == "3") return rM.GetString("FrameLimit2");
                if (getLimitType(step) == "4") return rM.GetString("FrameLimit3");
                if (getLimitType(step) == "5") return rM.GetString("FrameLimit4");
                if (getLimitType(step) == "6") return rM.GetString("FrameLimit5");
                if (getLimitType(step) == "7") return rM.GetString("FrameLimit6");
                if (getLimitType(step) == "8") return rM.GetString("FrameLimit7");

                return "Error";
            }  //取得選取步序條件的名稱


            public string[] BeginMinForce
            {
                set { beginMinForce = value; }
                get
                {
                    return beginMinForce;
                }
            }
            public string[] BeginMaxForce
            {
                set { beginMaxForce = value; }
                get
                {
                    return beginMaxForce;
                }
            }
            public string[] EndMinForce
            {
                set { endMinForce = value; }
                get
                {
                    return endMinForce;
                }
            }
            public string[] EndMaxForce
            {
                set { endMaxForce = value; }
                get
                {
                    return endMaxForce;
                }
            }
            public string[] EndMinPos
            {
                set { endMinPos = value; }
                get
                {
                    return endMinPos;
                }
            }
            public string[] EndMaxPos
            {
                set { endMaxPos = value; }
                get
                {
                    return endMaxPos;
                }
            }
            
           
            public bool LimitStepEnabled(int num)
            {
                if (Mode[num] != 0) if (limitType[num] == "0") return false;  //該步序有動作，但已設定基礎限制
                return true;
            }


            //方向限制圖形步序
            private string leftStep = "";
            public string LeftStep
            {
                get { return leftStep; }
            }

            private string rightStep = "";
            public string RightStep
            {
                get { return rightStep; }
            }

            private string topStep = "";
            public string TopStep
            {
                get { return topStep; }
            }

            private string bottomStep = "";
            public string BottomStep
            {
                get { return bottomStep; }
            }
        }

        private class Chart
        {
            protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
            Graph.Chart chart;
            PLC plc;
            TempRecipe tempRecipe;

            private double xMaxTemp, yMaxTemp;
            private double jogXMax = 0;
            private double jogYMax = 0;
            public bool ExternalCurve;


            private double xMin=0;
            public double XMin
            {
                get { return xMin; }
                set 
                {
                        xMin = value;
                }
            }
            private double xMax=10;
            public double XMax
            {
                get { return xMax; }
                set
                {
                        xMax = value;
                }
            }

            private double yMin=0;
            public double YMin
            {
                get { return yMin; }
                set
                {
                        yMin = value;
                }
            }
            private double yMax=10;
            public double YMax
            {
                get { return yMax; }
                set
                {
                        yMax = value;
                }
            }
            private string XTitle, YTitle;

            private double x1 = 0;
            public double X1 
            { 
                set { x1 = value; }
                get { return x1; }
            }
            private double x2 = 0;
            public double X2
            {
                get { return x2; }
                set 
                {
                    if (value >= x1) x2 = value;
                    else 
                    {
                        x2 = x1;
                        x1 = value;
                    }
                }
            }

            private double y1 = 0;
            public double Y1 
            {
                get { return y1; }
                set { y1 = value; } 
            }
            private double y2 = 0;
            public double Y2
            {
                get { return y2; }
                set
                {
                    if (value >= y1) y2 = value;
                    else
                    {
                        y2 = y1;
                        y1 = value;
                    }
                }
            }


            private bool lockScaleStatus = false;
            public bool LockSacleStatus
            {
                get { return lockScaleStatus; }
            }
            /*
            private double defaultXMin = 0;
            private double defaultXMax = 0;
            private double defaultYMin = 0;
            private double defaultYMax = 0;
            */
            public Chart(Graph.Chart _chart, TempRecipe _tempRecipe,PLC _plc)
            {
                chart = _chart;
                tempRecipe = _tempRecipe;
                plc = _plc;
            }

            public void Lock() 
            {
                lockScaleStatus = true;
            }

            public void Unlock() 
            {
                lockScaleStatus = false;
            }

            public void Update()
            {
                chart.ChartAreas[0].AxisY.Maximum = YMax;
                chart.ChartAreas[0].AxisY.Minimum = YMin;
                chart.ChartAreas[0].AxisY.Interval = Math.Round(YMax - YMin) / 5;
                chart.ChartAreas[0].AxisY.Title = YTitle;

                chart.ChartAreas[0].AxisX.Maximum = XMax;
                chart.ChartAreas[0].AxisX.Minimum = XMin;
                chart.ChartAreas[0].AxisX.Interval = Math.Round(XMax - XMin) / 5;
                chart.ChartAreas[0].AxisX.Title = XTitle;
            }

            private void adjScaleX(double x)
            {
                if ((xMaxTemp < Math.Ceiling(x + 0.5 * (x - XMin))) && x > 0) 
                {
                    xMaxTemp = Math.Ceiling(x + 0.5 * (x - XMin));
                } 
            }
            private void adjScaleY(double y)
            {
                if ((yMaxTemp < Math.Ceiling(y + 0.3 * (y - YMin))) && y > 0)
                {
                    yMaxTemp = Math.Ceiling(y + 0.3 * (y - YMin));
                }
            }

            private void autoScale() 
            {
                if (lockScaleStatus == false)
                {
                    if (xMaxTemp != 0 && xMaxTemp > XMax) XMax = xMaxTemp;
                    if (yMaxTemp != 0 && yMaxTemp > yMax) yMax = yMaxTemp;
                }        
            }

            public void Initialize(string unit, int positionMax, int forceMax, string curvePath)  //ok
            {
                chart.Series["data"].Points.Clear();

                XTitle = rM.GetString("PosUC");
                if (unit == "kgf") YTitle = rM.GetString("ForceUC");
                else if (unit == "N") YTitle = rM.GetString("ForceNC");
                else if (unit == "lbf") YTitle = rM.GetString("ForceLC");

                /*
                if (lockScaleStatus == 1)
                {
                    XMin = Math.Round(defaultXMin);
                    XMax = Math.Round(defaultXMax);
                    YMin = Math.Round(defaultYMin);
                    YMax = Math.Round(defaultYMax);

                    xMaxTemp = XMax;
                    yMaxTemp = YMax;
                }*/

                chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
                chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dot;
                chart.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
                chart.ChartAreas[0].CursorX.SelectionColor = Color.Transparent;
                chart.ChartAreas[0].CursorX.LineColor = Color.Transparent;
                chart.ChartAreas[0].CursorX.Interval = 0.1;

                chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
                chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
                chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dot;
                chart.ChartAreas[0].CursorY.IsUserEnabled = true;
                chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
                chart.ChartAreas[0].CursorY.SelectionColor = Color.Transparent;
                chart.ChartAreas[0].CursorY.LineColor = Color.Transparent;
                chart.ChartAreas[0].CursorY.Interval = 0.1;

                DrawLine();
                defaultImportCurve(curvePath);
            }

            public void Reset() 
            {
                XMin = 0;
                XMax = jogXMax+10;
                YMin = 0;
                YMax = jogYMax+10;

                xMaxTemp = XMax;
                yMaxTemp = YMax;
            }

            private void drawOriginPos()
            {
                chart.Series["OriginPos"].Points.Clear();
                chart.Series["OriginPos"].Points.AddXY(Convert.ToDouble(tempRecipe.OriginPos), YMin);
                chart.Series["OriginPos"].Points.AddXY(Convert.ToDouble(tempRecipe.OriginPos), YMax);

                adjScaleX(Convert.ToDouble(tempRecipe.OriginPos));
            }
            private void drawStandbyPos()
            {
                chart.Series["StandbyPos"].Points.Clear();
                chart.Series["StandbyPos"].Points.AddXY(Convert.ToDouble(tempRecipe.StandbyPos), YMin);
                chart.Series["StandbyPos"].Points.AddXY(Convert.ToDouble(tempRecipe.StandbyPos), YMax);
                adjScaleX(Convert.ToDouble(tempRecipe.StandbyPos));
            }
            private void drawPressPos()
            {
                chart.Series["PressingPos"].Points.Clear();
                chart.Series["PressingForce"].Points.Clear();
                chart.Series["PressingPos"].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos), YMin);
                chart.Series["PressingPos"].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos), YMax);
                adjScaleX(Convert.ToDouble(tempRecipe.PressingPos));
            }
            private void drawPressForce()
            {
                double force = Convert.ToDouble(tempRecipe.PressingForce);
                if (plc.Unit == 1) force = force * 10;
                else if (plc.Unit == 2) force = force / 2;

                chart.Series["PressingPos"].Points.Clear();
                chart.Series["PressingForce"].Points.Clear();
                chart.Series["PressingForce"].Points.AddXY(Convert.ToDouble(XMin), force);
                chart.Series["PressingForce"].Points.AddXY(Convert.ToDouble(XMax), force);
                adjScaleY(force);
            }

            private void drawForceLimit()
            {
                if (tempRecipe.limit.LimitType == 0 && tempRecipe.Step == tempRecipe.limit.limitStep) 
                {
                    double BasicEndMaxForce = Convert.ToDouble(tempRecipe.BasicEndMaxForce);
                    double BasicEndMinForce = Convert.ToDouble(tempRecipe.BasicEndMinForce);
                    if (plc.Unit == 1)
                    {
                        BasicEndMaxForce = BasicEndMaxForce * 10;
                        BasicEndMinForce = BasicEndMinForce * 10;
                    }
                    else if (plc.Unit == 2)
                    {
                        BasicEndMaxForce = BasicEndMaxForce /2;
                        BasicEndMinForce = BasicEndMinForce /2;
                    }

                    chart.Series[tempRecipe.BasicLimit0Step].Points.Clear();
                    chart.Series[tempRecipe.BasicLimit0Step].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos),BasicEndMaxForce);
                    chart.Series[tempRecipe.BasicLimit0Step].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos), BasicEndMinForce);

                    chart.Series[tempRecipe.BasicLimit1Step].Points.Clear();
                    chart.Series[tempRecipe.BasicLimit1Step].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos) - (xMax - xMin) / 50, BasicEndMaxForce);
                    chart.Series[tempRecipe.BasicLimit1Step].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos) + (xMax - xMin) / 50, BasicEndMaxForce);
                    
                    chart.Series[tempRecipe.BasicLimit2Step].Points.Clear();
                    chart.Series[tempRecipe.BasicLimit2Step].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos) - (xMax - xMin) / 50, BasicEndMinForce);
                    chart.Series[tempRecipe.BasicLimit2Step].Points.AddXY(Convert.ToDouble(tempRecipe.PressingPos) + (xMax - xMin) / 50, BasicEndMinForce);
                    adjScaleY(Convert.ToDouble(tempRecipe.BasicEndMaxForce));
                }
            }
            private void drawPosLimit()
            {
                if (tempRecipe.limit.LimitType == 0 && tempRecipe.Step == tempRecipe.limit.limitStep)
                {
                    double force = Convert.ToDouble(tempRecipe.PressingForce);
                    if (plc.Unit == 1) force = force * 10;
                    else if (plc.Unit == 2) force = force / 2;


                    chart.Series[tempRecipe.BasicLimit0Step].Points.Clear();
                    chart.Series[tempRecipe.BasicLimit0Step].Points.AddXY(Convert.ToDouble(tempRecipe.BasicEndMaxPos), force);
                    chart.Series[tempRecipe.BasicLimit0Step].Points.AddXY(Convert.ToDouble(tempRecipe.BasicEndMinPos), force);

                    chart.Series[tempRecipe.BasicLimit1Step].Points.Clear();
                    chart.Series[tempRecipe.BasicLimit1Step].Points.AddXY(Convert.ToDouble(tempRecipe.BasicEndMaxPos), force - (yMax - yMin) / 50);
                    chart.Series[tempRecipe.BasicLimit1Step].Points.AddXY(Convert.ToDouble(tempRecipe.BasicEndMaxPos), force + (yMax - yMin) / 50);

                    chart.Series[tempRecipe.BasicLimit2Step].Points.Clear();
                    chart.Series[tempRecipe.BasicLimit2Step].Points.AddXY(Convert.ToDouble(tempRecipe.BasicEndMinPos), force - (yMax - yMin) / 50);
                    chart.Series[tempRecipe.BasicLimit2Step].Points.AddXY(Convert.ToDouble(tempRecipe.BasicEndMinPos), force + (yMax - yMin) / 50);
                    adjScaleX(Convert.ToDouble(tempRecipe.BasicEndMaxPos));
                }
            }

            
            public void drawDynamicLeft()
            {
                if (tempRecipe.limit.LimitType == 1)
                {
                    double BeginMinForce = Convert.ToDouble(tempRecipe.BeginMinForce);
                    double BeginMaxForce = Convert.ToDouble(tempRecipe.BeginMaxForce);
                    if (plc.Unit == 1)
                    {
                        BeginMinForce = BeginMinForce * 10;
                        BeginMaxForce = BeginMaxForce * 10;
                    }
                    else if (plc.Unit == 2)
                    {
                        BeginMinForce = BeginMinForce / 2;
                        BeginMaxForce = BeginMaxForce / 2;
                    }

                    chart.Series[tempRecipe.limit.LeftStep].Points.Clear();
                    chart.Series[tempRecipe.limit.LeftStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), BeginMinForce);
                    chart.Series[tempRecipe.limit.LeftStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), BeginMaxForce);

                }
            }
            public void drawDynamicRight()
            {
                if (tempRecipe.limit.LimitType == 1)
                {
                    double EndMaxForce = Convert.ToDouble(tempRecipe.EndMaxForce);
                    double EndMinForce = Convert.ToDouble(tempRecipe.EndMinForce);
                    if (plc.Unit == 1)
                    {
                        EndMaxForce = EndMaxForce * 10;
                        EndMinForce = EndMinForce * 10;
                    }
                    else if (plc.Unit == 2)
                    {
                        EndMaxForce = EndMaxForce / 2;
                        EndMinForce = EndMinForce / 2;
                    }

                    chart.Series[tempRecipe.limit.RightStep].Points.Clear();
                    chart.Series[tempRecipe.limit.RightStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMaxForce);
                    chart.Series[tempRecipe.limit.RightStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMinForce);
                }
            }
            public void drawDynamicTop()
            {
                if (tempRecipe.limit.LimitType == 1)
                {
                    double BeginMaxForce = Convert.ToDouble(tempRecipe.BeginMaxForce);
                    double EndMaxForce = Convert.ToDouble(tempRecipe.EndMaxForce);
                    if (plc.Unit == 1)
                    {
                        BeginMaxForce = BeginMaxForce * 10;
                        EndMaxForce = EndMaxForce * 10;
                    }
                    else if (plc.Unit == 2)
                    {
                        BeginMaxForce = BeginMaxForce / 2;
                        EndMaxForce = EndMaxForce / 2;
                    }

                    chart.Series[tempRecipe.limit.TopStep].Points.Clear();
                    chart.Series[tempRecipe.limit.TopStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), BeginMaxForce);
                    chart.Series[tempRecipe.limit.TopStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMaxForce);
                }
            }
            public void drawDynamicBottom()
            {
                if (tempRecipe.limit.LimitType == 1)
                {
                    double EndMinForce = Convert.ToDouble(tempRecipe.EndMinForce);
                    double EndMaxForce = Convert.ToDouble(tempRecipe.EndMinForce);
                    double BeginMinForce = Convert.ToDouble(tempRecipe.BeginMinForce);
                    if (plc.Unit == 1)
                    {
                        EndMinForce = EndMinForce * 10;
                        EndMaxForce = EndMaxForce * 10;
                        BeginMinForce = BeginMinForce * 10;
                    }
                    else if (plc.Unit == 2)
                    {
                        EndMinForce = EndMinForce / 2;
                        EndMaxForce = EndMaxForce / 2;
                        BeginMinForce = BeginMinForce / 2;
                    }

                    chart.Series[tempRecipe.limit.BottomStep].Points.Clear();
                    chart.Series[tempRecipe.limit.BottomStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMinForce);
                    chart.Series[tempRecipe.limit.BottomStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), BeginMinForce);
                    adjScaleX(Convert.ToDouble(tempRecipe.EndMaxPos));
                    adjScaleY(EndMaxForce);
                }
            }
            public void drawFrameLimit()
            {
                if (tempRecipe.limit.LimitType > 1)
                {
                    double EndMinForce = Convert.ToDouble(tempRecipe.EndMinForce);
                    double EndMaxForce = Convert.ToDouble(tempRecipe.EndMaxForce);

                    if (plc.Unit == 1)
                    {
                        EndMinForce = EndMinForce * 10;
                        EndMaxForce = EndMaxForce * 10;

                    }
                    else if (plc.Unit == 2)
                    {
                        EndMinForce = EndMinForce / 2;
                        EndMaxForce = EndMaxForce / 2;

                    }

                    chart.Series[tempRecipe.limit.LeftStep].Points.Clear();
                    chart.Series[tempRecipe.limit.LeftStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), EndMinForce);
                    chart.Series[tempRecipe.limit.LeftStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), EndMaxForce);

                    chart.Series[tempRecipe.limit.RightStep].Points.Clear();
                    chart.Series[tempRecipe.limit.RightStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMinForce);
                    chart.Series[tempRecipe.limit.RightStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMaxForce);

                    chart.Series[tempRecipe.limit.TopStep].Points.Clear();
                    chart.Series[tempRecipe.limit.TopStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), EndMaxForce);
                    chart.Series[tempRecipe.limit.TopStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMaxForce);

                    chart.Series[tempRecipe.limit.BottomStep].Points.Clear();
                    chart.Series[tempRecipe.limit.BottomStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMinPos), EndMinForce);
                    chart.Series[tempRecipe.limit.BottomStep].Points.AddXY(Convert.ToDouble(tempRecipe.EndMaxPos), EndMinForce);

                    adjScaleX(Convert.ToDouble(tempRecipe.EndMaxPos));
                    adjScaleY(EndMaxForce);
                }
            }

            public void drawGeometryLimit() 
            {
                if (tempRecipe.limit.LimitType == 1)
                {
                    drawDynamicLeft();
                    drawDynamicTop();
                    drawDynamicRight();
                    drawDynamicBottom();
                }
                else if (tempRecipe.limit.LimitType > 1)
                {
                    drawFrameLimit();
                }
            }

            public void cleanStartCondition()
            {
                chart.Series["OriginPos"].Points.Clear();
                chart.Series["StandbyPos"].Points.Clear();
            }
            public void cleanPressSetting()
            {
                chart.Series["PressingPos"].Points.Clear();
                chart.Series["PressingForce"].Points.Clear();
            }
            public void cleanAllBasicLimit()
            {
                string temp1, temp2, temp3;
                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    temp1 = "BasicLimit0_" + Convert.ToString(i+1);
                    temp2 = "BasicLimit1_" + Convert.ToString(i + 1);
                    temp3 = "BasicLimit2_" + Convert.ToString(i + 1);

                    chart.Series[temp1].Points.Clear();
                    chart.Series[temp2].Points.Clear();
                    chart.Series[temp3].Points.Clear();
                }
            }

            public void cleanBasicLimit(int step)
            {
                 chart.Series["BasicLimit0_" + Convert.ToString(step)].Points.Clear();
                 chart.Series["BasicLimit1_" + Convert.ToString(step)].Points.Clear();
                 chart.Series["BasicLimit2_" + Convert.ToString(step)].Points.Clear();
            }


            public void cleanFrameLimit() 
            {
                string temp1, temp2, temp3,temp4;
                for (int i = 0; i < 10; i++)
                {
                    temp1 = "top" + Convert.ToString(i + 1);
                    temp2 = "bottom" + Convert.ToString(i + 1);
                    temp3 = "left" + Convert.ToString(i + 1);
                    temp4 = "right" + Convert.ToString(i + 1);

                    chart.Series[temp1].Points.Clear();
                    chart.Series[temp2].Points.Clear();
                    chart.Series[temp3].Points.Clear();
                    chart.Series[temp4].Points.Clear();
                }
            }
            public void cleanCurve() 
            {
                chart.Series["data"].Points.Clear();
            }

            public void DrawLine()
            {
                cleanStartCondition();
                cleanPressSetting();
                cleanAllBasicLimit();
                cleanFrameLimit();

                drawOriginPos();
                drawStandbyPos();

                if (tempRecipe.Mode == 1)
                {
                    drawPressPos();
                    drawForceLimit();
                }
                else if (tempRecipe.Mode == 2)
                {
                    drawPressForce();
                    drawPosLimit();

                }
                else if (tempRecipe.Mode == 3)
                {
                    drawForceLimit();
                    drawPosLimit();
                }
                else if (tempRecipe.Mode == 4)
                {
                    drawPressPos();
                }
                else if (tempRecipe.Mode == 5)
                {
                    drawPosLimit();
                }

                int tempStep = tempRecipe.limit.limitStep;
                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    tempRecipe.limit.SetLimitStep(i+1);
                    if (Convert.ToInt32(tempRecipe.limit.getLimitType(i)) == 1)
                    {
                        drawDynamicLeft();
                        drawDynamicTop();
                        drawDynamicRight();
                        drawDynamicBottom();
                    }
                    else if (Convert.ToInt32(tempRecipe.limit.getLimitType(i)) > 1)
                    {
                        drawFrameLimit();
                    }
                }
                autoScale();
                //Unlock();
                tempRecipe.limit.SetLimitStep(tempStep);
            }

            public void DrawCurve(double x,double y) 
            {
                if ((ExternalCurve==false) && (x>0 || y>0))
                {
                    chart.Series["data"].Points.AddXY(x, y);
                    adjScaleX(x);
                    adjScaleY(y);
                    autoScale();
                    if (x > jogXMax) jogXMax = x;
                    if (y > jogYMax) jogYMax = y;
                }
            }

            public  void defaultImportCurve(string curvePath)
            {
                if (curvePath != "")
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(curvePath, Encoding.Default);
                    DrawExternalCurve(sr);
                    sr.Close();
                    DrawLine();
                    ExternalCurve = true;
                }
            }

            public void DrawExternalCurve(StreamReader sr)
            {
                cleanCurve();
                String strResult = "";
                double x, y;
                xMaxTemp = 0;
                yMaxTemp = 0;
                while ((strResult = sr.ReadLine()) != null)
                {
                    string[] sCoordinate = strResult.Split(',');

                    if (Double.TryParse(sCoordinate[0], out x) == true && (Double.TryParse(sCoordinate[1], out y) == true) )
                    {
                        if (x != 0 || y != 0)
                        {
                            chart.Series["data"].Points.AddXY(x, y);
                            adjScaleX(x);
                            adjScaleY(y);
                            if (x > jogXMax) jogXMax = x;
                            if (y > jogYMax) jogYMax = y;
                       }
                    }
                }
                autoScale();
            }
        }
        private Chart chart;

        public class JogChart
        {
            private PLC plc;
            private double liveForce = 0;
            private double livePos = 0;
            private double lastLiveForce = 0;
            private double lastLivePos = 0;
            private long chartAmount = 0;
            private double[] chartPosition = new double[CONSTANT.ChartPointsAmount];
            private double[] chartForce = new double[CONSTANT.ChartPointsAmount];

            public JogChart(PLC _plc)
            {
                plc = _plc;
            }

            public void Drawing(out double x, out double y)
            {
                livePos = plc.LivePos;
                liveForce = plc.LiveForce;

                if (livePos > lastLivePos)
                {
                    lastLivePos = livePos;
                    lastLiveForce = liveForce;

                    chartPosition[chartAmount] = livePos;
                    chartForce[chartAmount] = liveForce;

                    chartAmount++;

                }
                x = livePos;
                y = liveForce;
            }


            public void Reset()
            {
                chartAmount = 0;
            }

            public void Export()     //即時圖表座標暫存txt輸出
            {
                SaveFileDialog filePath = new SaveFileDialog();
                filePath.Filter = "csv (*.csv)|*.csv|All files (*.*)|*.*";
                filePath.ShowDialog();
                if (filePath.FileName != "")
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath.FileName))
                    {
                        if (plc.Unit == 1) file.WriteLine("Position(mm),Force(lbf)");
                        else if (plc.Unit == 2) file.WriteLine("Position(mm),Force(N)");
                        else file.WriteLine("Position(mm),Force(kgf)");

                        chartPosition[chartAmount] = livePos;
                        chartForce[chartAmount] = liveForce;

                        int j = 0;
                        while (j < chartAmount)
                        {
                            file.WriteLine(chartPosition[j] + "," + chartForce[j]);
                            j++;
                        }
                    }
                    MessageBox.Show("OK", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private JogChart jogChart;

        public Modification(PLC plc, ServoPressInfor infor, MotionManager motionManager, ChartManager chartManager, string curvePath)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            InitializeComponent();

            this.motionManager = motionManager;
            this.infor = infor;
            this.plc = plc;
            this.curvePath = curvePath;

            jogChart = new JogChart(this.plc);
            chart = new Chart(this.chart1,tempRecipe,plc);
        }

        //Loading the  motion setp setting from the current selected recipe 載入步序參數
        private void Modification_Load(object sender, EventArgs e)
        {
            tempRecipe.setOriginPos(motionManager.MotionData[0].OriginalPos);
            tempRecipe.setOriginVel(motionManager.MotionData[0].OriginalVel);
            tempRecipe.setStandbyPos(motionManager.MotionData[0].StandbyPos);
            tempRecipe.setStandbyVel(motionManager.MotionData[0].StandbyVel);
            tempRecipe.setStandbyTime(motionManager.MotionData[0].StandbyTime);

            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                tempRecipe.setStep(i+1);
                
                tempRecipe.setMode(motionManager.MotionData[i].Mode);
                tempRecipe.setPressingPos(motionManager.MotionData[i].PressPos);
                tempRecipe.setPressingForce(motionManager.MotionData[i].PressForce);
                tempRecipe.setPressingVel(motionManager.MotionData[i].PressVel);
                tempRecipe.setPressingTime(motionManager.MotionData[i].PressTime);
                tempRecipe.setCpk(motionManager.MotionData[i].Cpk);
                
                if (tempRecipe.Mode == 6) tempRecipe.IOInitialize();
                
                tempRecipe.limit.Mode[i] = Convert.ToInt16(motionManager.MotionData[i].Mode);
                tempRecipe.limit.setLimitType(i,motionManager.MotionData[i].LimitType);
                tempRecipe.limit.BeginMaxForce[i] = motionManager.MotionData[i].BeginMaxForce;
                tempRecipe.limit.BeginMinForce[i] = motionManager.MotionData[i].BeginMinForce;
                tempRecipe.limit.EndMaxForce[i] = motionManager.MotionData[i].EndMaxForce;
                tempRecipe.limit.EndMaxPos[i] = motionManager.MotionData[i].EndMaxPos;
                tempRecipe.limit.EndMinForce[i] = motionManager.MotionData[i].EndMinForce;
                tempRecipe.limit.EndMinPos[i] = motionManager.MotionData[i].EndMinPos;
            }
            tempRecipe.setStep(1);
            chart.Initialize(infor.Unit, infor.PositionMax, infor.ForceMax,curvePath);

            tmrJogCurve.Enabled = true;   //即時監控啟動
            tmrUI.Enabled = true;
            tmrSlowUI.Enabled = true;
            tmrIO.Enabled = true;
        }

        //Adjustment of textbox status when mode changed  切換模式是使用者輸入介面變更
        private void rbNoMode_CheckedChanged(object sender, EventArgs e)//不作動設定
        {
            if (rbNoMode.Checked == true)
            {
                tempRecipe.setMode(0);
                chart.cleanStartCondition();
                chart.cleanPressSetting();
                chart.cleanAllBasicLimit();
                chart.cleanFrameLimit();
            }
        }
        private void rbPosMode_CheckedChanged(object sender, EventArgs e) //位置模式設定
        {
            if (rbPosMode.Checked == true)
            {
                tempRecipe.setMode(1);
                chart.DrawLine();
            }
        }
        private void rbForceMode_CheckedChanged(object sender, EventArgs e) //壓力模式設定
        {
            if (rbForceMode.Checked == true)
            {
                tempRecipe.setMode(2);
                chart.DrawLine();
            }
        }
        private void rbDistMode_CheckedChanged(object sender, EventArgs e)  //距離模式設定
        {
            if (rbDistMode.Checked == true)
            {
                tempRecipe.setMode(3);
                chart.DrawLine();
            }
        }
        private void rbForcePosMode_CheckedChanged(object sender, EventArgs e) //壓力位置模式設定
        {
            if (rbForcePosMode.Checked == true)
            {
                tempRecipe.setMode(4);
                chart.DrawLine();
            }
        }
        private void rbForceDistMode_CheckedChanged(object sender, EventArgs e) //壓力距離模式設定
        {
            if (rbForceDistMode.Checked == true)
            {
                tempRecipe.setMode(5);
                chart.DrawLine();
            }
        }
        private void rbWaitSignal_CheckedChanged(object sender, EventArgs e) //訊號模式設定
        {
            if (rbIOSignal.Checked == true)
            {
                //tempRecipe.BackupOut();
                tempRecipe.setMode(6);
                chart.DrawLine();
            }
            else if (rbIOSignal.Checked == false) 
            {
                //tempRecipe.BackupIn();
            }
        }
           

        //壓合條件設定
        private void txtOriginPos_TextChanged(object sender, EventArgs e) //工作原點
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtOriginPos.Text,out temp))
            {
                tempRecipe.setOriginPos(txtOriginPos.Text);
                chart.DrawLine();
            }
        }
        private void txtOriginVel_TextChanged(object sender, EventArgs e) //原點速度
        {
            double temp;
            if (Double.TryParse(txtOriginVel.Text, out temp))
            {
                tempRecipe.setOriginVel(txtOriginVel.Text);
            }
        }
        private void txtStandbyPos_TextChanged(object sender, EventArgs e) //預備位置
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtStandbyPos.Text, out temp))
            {
                tempRecipe.setStandbyPos(txtStandbyPos.Text);
                chart.DrawLine();
            }
            //chart.Unlock();
        }
        private void txtStandbyVel_TextChanged(object sender, EventArgs e) //預備速度
        {
            double temp;
            if (Double.TryParse(txtStandbyVel.Text, out temp))
            {
                tempRecipe.setStandbyVel(txtStandbyVel.Text);
            }
        }
        private void txtStandbyTime_TextChanged(object sender, EventArgs e) //預備時間
        {
            double temp;
            if (Double.TryParse(txtStandbyTime.Text, out temp))
            {
                tempRecipe.setStandbyTime(txtStandbyTime.Text);
            }
        }
        private void txtPressPos_TextChanged(object sender, EventArgs e) //壓合位置
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtPressPos.Text, out temp))
            {
                tempRecipe.setPressingPos(txtPressPos.Text);
                chart.DrawLine();
            }
        }
        private void txtPressForce_TextChanged(object sender, EventArgs e) //壓合力量
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtPressForce.Text, out temp))
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.setPressingForce(txtPressForce.Text);
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.setPressingForce(Convert.ToString(Convert.ToDouble(txtPressForce.Text)/10));
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.setPressingForce(Convert.ToString(Convert.ToDouble(txtPressForce.Text)*2 ));
                }
                chart.DrawLine();
            }
        }
        private void txtPressTime_TextChanged(object sender, EventArgs e) //壓合時間
        {
            double temp;
            if (Double.TryParse(txtPressTime.Text, out temp))
            {
                tempRecipe.setPressingTime(txtPressTime.Text);
            }
        }
        private void txtPressVel_TextChanged(object sender, EventArgs e) //壓合速度
        {
            double temp;
            if (Double.TryParse(txtOriginPos.Text, out temp))
            {
                tempRecipe.setPressingVel(txtPressVel.Text);
            }
        }
        private void rbCpkPosition_CheckedChanged(object sender, EventArgs e) //Cpk
        {
            tempRecipe.setCpk("Position");
        }
        private void rbCpkForce_CheckedChanged(object sender, EventArgs e) //Cpk
        {
            tempRecipe.setCpk("Force");
        }


        private void cboIO_1_SelectedIndexChanged(object sender, EventArgs e) //IO 模式項目1
        {
            tempRecipe.setSignal_1(cboIO_1.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboIO_2_SelectedIndexChanged(object sender, EventArgs e) //IO 模式項目2
        {
            tempRecipe.setSignal_2(cboIO_2.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboIO_3_SelectedIndexChanged(object sender, EventArgs e) //IO 模式項目3
        {
            tempRecipe.setSignal_3(cboIO_3.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboIO_4_SelectedIndexChanged(object sender, EventArgs e) //IO 模式項目4
        {
            tempRecipe.setSignal_4(cboIO_4.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }

        private void cboOuputSignal_1_SelectedIndexChanged(object sender, EventArgs e) //輸出訊號1
        {
            tempRecipe.setSignalPara_1(cboOuputSignal_1.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboOuputSignal_2_SelectedIndexChanged(object sender, EventArgs e) //輸出訊號2
        {
            tempRecipe.setSignalPara_2(cboOuputSignal_2.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboOuputSignal_3_SelectedIndexChanged(object sender, EventArgs e) //輸出訊號3
        {
            tempRecipe.setSignalPara_3(cboOuputSignal_3.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboOuputSignal_4_SelectedIndexChanged(object sender, EventArgs e) //輸出訊號4
        {
            tempRecipe.setSignalPara_4(cboOuputSignal_4.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }

        private void cboWaitingSignal_1_SelectedIndexChanged(object sender, EventArgs e) //等待訊號1
        {
            tempRecipe.setSignalPara_1(cboWaitingSignal_1.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboWaitingSignal_2_SelectedIndexChanged(object sender, EventArgs e) //等待訊號2
        {
            tempRecipe.setSignalPara_2(cboWaitingSignal_2.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboWaitingSignal_3_SelectedIndexChanged(object sender, EventArgs e) //等待訊號3
        {
            tempRecipe.setSignalPara_3(cboWaitingSignal_3.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }
        private void cboWaitingSignal_4_SelectedIndexChanged(object sender, EventArgs e) //等待訊號4
        {
            tempRecipe.setSignalPara_4(cboWaitingSignal_4.SelectedIndex);
            tempRecipe.IOSave();
            tmrIO.Enabled = true;
        }

        private void txtIODelay_1_TextChanged(object sender, EventArgs e) //延遲1
        {
            double temp;
            if (Double.TryParse(txtIODelay_1.Text, out temp))
            {
                tempRecipe.setSignalPara_1(Convert.ToInt32(txtIODelay_1.Text) * 10);
                tempRecipe.IOSave();
            }
        }
        private void txtIODelay_2_TextChanged(object sender, EventArgs e) //延遲2
        {
            double temp;
            if (Double.TryParse(txtIODelay_2.Text, out temp))
            {
                tempRecipe.setSignalPara_2(Convert.ToInt32(txtIODelay_2.Text) * 10);
                tempRecipe.IOSave();
            }
        }
        private void txtIODelay_3_TextChanged(object sender, EventArgs e) //延遲3
        {
            double temp;
            if (Double.TryParse(txtIODelay_3.Text, out temp))
            {
                tempRecipe.setSignalPara_3(Convert.ToInt32(txtIODelay_3.Text) * 10);
                tempRecipe.IOSave();
            }
        }
        private void txtIODelay_4_TextChanged(object sender, EventArgs e) //延遲4
        {
            double temp;
            if (Double.TryParse(txtIODelay_4.Text, out temp))
            {
                tempRecipe.setSignalPara_4(Convert.ToInt32(txtIODelay_4.Text) * 10);
                tempRecipe.IOSave();
            }
        }

        //基本限制條件設定
        private void rbForceLimit_CheckedChanged(object sender, EventArgs e) //壓力基本限制條件
        {
            if (rbForceLimit.Checked == true)
            {
                counter = 0;
                rbOriginPos.Checked = false;
                rbStandbyPos.Checked = false;
                rbPressingPos.Checked = false;
                rbPosLimit.Checked = false;
                //rbGeometry.Checked = false;

                tempRecipe.limit.LimitType = 0;
            }
        }
        private void txtBasicMaxForce_TextChanged(object sender, EventArgs e) //基本壓力限制最大值
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtBasicMaxForce.Text, out temp) && (tempRecipe.limit.LimitType == 0))
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.BasicEndMaxForce = txtBasicMaxForce.Text;
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.BasicEndMaxForce = Convert.ToString(Convert.ToDouble(txtBasicMaxForce.Text) / 10);
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.BasicEndMaxForce = Convert.ToString(Convert.ToDouble(txtBasicMaxForce.Text) * 2);
                }

                chart.DrawLine();
            }
        }
        private void txtBasicMinForce_TextChanged(object sender, EventArgs e) //基本壓力限制最小值
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtBasicMinForce.Text, out temp) && (tempRecipe.limit.LimitType == 0))
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.BasicEndMinForce = txtBasicMinForce.Text;
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.BasicEndMinForce = Convert.ToString(Convert.ToDouble(txtBasicMinForce.Text) / 10);
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.BasicEndMinForce = Convert.ToString(Convert.ToDouble(txtBasicMinForce.Text) * 2);
                }

                chart.DrawLine();
            }
        }
        private void rbPosLimit_CheckedChanged(object sender, EventArgs e) //位置基本限制條件
        {
            if (rbPosLimit.Checked == true)
            {
                counter = 0;
                rbOriginPos.Checked = false;
                rbStandbyPos.Checked = false;
                rbPressingPos.Checked = false;
                rbForceLimit.Checked = false;
                tempRecipe.limit.LimitType = 0;
            }
        }
        private void txtBasicEndMaxPos_TextChanged(object sender, EventArgs e) //基本位置限制最大值
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtBasicEndMaxPos.Text, out temp) && (tempRecipe.limit.LimitType == 0))
            {
                tempRecipe.BasicEndMaxPos = txtBasicEndMaxPos.Text;
                chart.DrawLine();
            }
        }
        private void txtBasicEndMinPos_TextChanged(object sender, EventArgs e) //基本位置限制最小值
        {
            //chart.Lock();
            double temp;
            if (Double.TryParse(txtBasicEndMinPos.Text, out temp) && (tempRecipe.limit.LimitType == 0))
            {
                tempRecipe.BasicEndMinPos = txtBasicEndMinPos.Text;
                chart.DrawLine();
            }
        }


        //幾何限制步序
        private void rbStep1_CheckedChanged(object sender, EventArgs e) //幾何限制步序1
        {
            if (rbStep1.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(1);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep2_CheckedChanged(object sender, EventArgs e) //幾何限制步序2
        {
            if (rbStep2.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(2);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep3_CheckedChanged(object sender, EventArgs e) //幾何限制步序3
        {
            if (rbStep3.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(3);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep4_CheckedChanged(object sender, EventArgs e) //幾何限制步序4
        {
            if (rbStep4.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(4);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep5_CheckedChanged(object sender, EventArgs e) //幾何限制步序5
        {
            if (rbStep5.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(5);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep6_CheckedChanged(object sender, EventArgs e) //幾何限制步序6
        {
            if (rbStep6.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(6);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep7_CheckedChanged(object sender, EventArgs e) //幾何限制步序7
        {
            if (rbStep7.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(7);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep8_CheckedChanged(object sender, EventArgs e) //幾何限制步序8
        {
            if (rbStep8.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(8);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep9_CheckedChanged(object sender, EventArgs e) //幾何限制步序9
        {
            if (rbStep9.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(9);
                //tempRecipe.limit.LimitType = 0;
            }
        }
        private void rbStep10_CheckedChanged(object sender, EventArgs e) //幾何限制步序10
        {
            if (rbStep10.Checked == true)
            {
                tempRecipe.limit.SetLimitStep(10);
                //tempRecipe.limit.LimitType = 0;
            }
        }



        private void rbNone_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbNone.Checked == true)
            {
                tempRecipe.limit.LimitType = 0;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();

            }
        }
        private void rbDynamic_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbDynamic.Checked == true)
            {
                if (tempRecipe.BeginMaxForce == "0" && tempRecipe.BeginMinForce == "0")
                {
                    //tempRecipe.CleanLimitValue();
                }
                tempRecipe.limit.LimitType = 1;
                chart.DrawLine();

            }
        }
        private void rbLR_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbLR.Checked == true)
            {
                tempRecipe.limit.LimitType = 2;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }
        private void rbTB_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbTB.Checked == true)
            {
                tempRecipe.limit.LimitType = 3;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }
        private void rbBR_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbBR.Checked == true)
            {
                tempRecipe.limit.LimitType = 4;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }

        private void rbTR_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbTR.Checked == true)
            {
                tempRecipe.limit.LimitType = 5;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }
        private void rbLT_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbLT.Checked == true)
            {
                tempRecipe.limit.LimitType = 6;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }
        
        private void rbLTR_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbLTR.Checked == true)
            {
                tempRecipe.limit.LimitType = 7;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }
        private void rbTBR_CheckedChanged(object sender, EventArgs e) //設定幾何限制條件
        {
            if (rbTBR.Checked == true)
            {
                tempRecipe.limit.LimitType = 8;
                //tempRecipe.CleanLimitValue();
                chart.DrawLine();
            }
        }



        private void EndMinForceLimit_TextChanged(object sender, EventArgs e) //終點最小壓力值
        {
            double temp;
            if (tempRecipe.limit.LimitType >= 1 && Double.TryParse(txtEndMinForceLimit.Text, out temp) == true && counter==0) 
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.EndMinForce = txtEndMinForceLimit.Text;
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.EndMinForce = Convert.ToString(Convert.ToDouble(txtEndMinForceLimit.Text) / 10);
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.EndMinForce = Convert.ToString(Convert.ToDouble(txtEndMinForceLimit.Text) * 2);
                }
                chart.drawGeometryLimit();
            }
        }
        private void EndMaxForceLimit_TextChanged(object sender, EventArgs e) //終點最大壓力值
        {
            double temp;
            if (tempRecipe.limit.LimitType >= 1 && Double.TryParse(txtEndMaxForceLimit.Text, out temp) == true && counter == 0)
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.EndMaxForce = txtEndMaxForceLimit.Text;
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.EndMaxForce = Convert.ToString(Convert.ToDouble(txtEndMaxForceLimit.Text) / 10);
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.EndMaxForce = Convert.ToString(Convert.ToDouble(txtEndMaxForceLimit.Text) * 2);
                }
                chart.drawGeometryLimit();
            }
        }
        private void EndMinPosLimit_TextChanged(object sender, EventArgs e) //終點最小位置值
        {
            double temp;
            if (tempRecipe.limit.LimitType >= 1 && Double.TryParse(txtEndMinPosLimit.Text, out temp) == true && counter == 0)
            {
                tempRecipe.EndMinPos = txtEndMinPosLimit.Text;
                chart.drawGeometryLimit();
            }
        }
        private void EndMaxPosLimit_TextChanged(object sender, EventArgs e) //終點最大位置值
        {
            double temp;
            if (tempRecipe.limit.LimitType >= 1 && Double.TryParse(txtEndMaxPosLimit.Text, out temp) == true && counter == 0)
            {
                tempRecipe.EndMaxPos = txtEndMaxPosLimit.Text;
                chart.drawGeometryLimit();
            }
        }
        private void BeginMinForceLimit_TextChanged(object sender, EventArgs e) //起點最小壓力值
        {
            double temp;
            if (tempRecipe.limit.LimitType >= 1 && Double.TryParse(txtBeginMinForceLimit.Text, out temp) == true && counter == 0)
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.BeginMinForce = txtBeginMinForceLimit.Text;
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.BeginMinForce = Convert.ToString(Convert.ToDouble(txtBeginMinForceLimit.Text) / 10);
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.BeginMinForce = Convert.ToString(Convert.ToDouble(txtBeginMinForceLimit.Text) * 2);
                }

                chart.drawGeometryLimit();
            } 
        }
        private void BeginMaxForceLimit_TextChanged(object sender, EventArgs e) //起點最大壓力值
        {
            double temp;
            if (tempRecipe.limit.LimitType >= 1 && Double.TryParse(txtBeginMaxForceLimit.Text, out temp) == true && counter == 0) 
            {
                if (plc.Unit == 0)
                {
                    tempRecipe.BeginMaxForce = txtBeginMaxForceLimit.Text;
                }
                else if (plc.Unit == 1)
                {
                    tempRecipe.BeginMaxForce = Convert.ToString(Convert.ToDouble(txtBeginMaxForceLimit.Text) / 10);
                }
                else if (plc.Unit == 2)
                {
                    tempRecipe.BeginMaxForce = Convert.ToString(Convert.ToDouble(txtBeginMaxForceLimit.Text) * 2);
                }

                chart.drawGeometryLimit();
            }  
        }


        private void btnOk_Click(object sender, EventArgs e)    //確定儲存參數
        {
            string item;
            int max;

            DialogResult = DialogResult.None;
            if (Check_Empty() == -1)   //確認輸入欄位是否有空格
            {
                MessageBox.Show(rM.GetString("FinishData"), rM.GetString("IncompleteData"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            else if (CheckStandbyPos() == -1) //確認預備位置與壓合位置的關係
            {
                MessageBox.Show("[" + lblPressPos.Text + rM.GetString("RequireLarger") + lblStandbyPos.Text + "]", rM.GetString("SettingErr"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            else if (CheckPressPos() == -1) //確認壓合位置是否有超出最大值
            {
                MessageBox.Show(rM.GetString("PosExceed") + Convert.ToString(infor.PositionMax), rM.GetString("SettingErr"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            else if (CheckPressForce() == -1) //確認壓合力量是否有超出最大值
            {
                MessageBox.Show(rM.GetString("ForceExceed") + Convert.ToString(infor.ForceMax), rM.GetString("SettingErr"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            /*
            else if (CheckValueLimit(out item, out max) == -1) //確認其他參數是否超出最大值
            {
                MessageBox.Show("[" + item + "] " + rM.GetString("NotLarger") + max, rM.GetString("Error"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            else if (CheckBasicPos() == -1) //確認位置限制大小關係
            {
                MessageBox.Show(rM.GetString("PosLimitErr"), rM.GetString("SettingErr"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            else if (CheckBasicForce() == -1) //確認壓力限制大小關係
            {
                MessageBox.Show(rM.GetString("ForceLimitErr"), rM.GetString("SettingErr"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }*/
            else
            {
                DialogResult = DialogResult.OK;

                motionManager.MotionData[0].OriginalPos = tempRecipe.OriginPos;
                motionManager.MotionData[0].OriginalVel = tempRecipe.OriginVel;
                motionManager.MotionData[0].StandbyPos = tempRecipe.StandbyPos;
                motionManager.MotionData[0].StandbyVel = tempRecipe.StandbyVel;
                motionManager.MotionData[0].StandbyTime = tempRecipe.StandbyTime;

                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    tempRecipe.setStep(i+1);
                    motionManager.MotionData[i].Mode = tempRecipe.Mode;
                    motionManager.MotionData[i].PressPos = tempRecipe.PressingPos;
                    motionManager.MotionData[i].PressForce = tempRecipe.PressingForce;
                    motionManager.MotionData[i].PressVel = tempRecipe.PressingVel;
                    motionManager.MotionData[i].PressTime = tempRecipe.PressingTime;
                    motionManager.MotionData[i].Cpk = tempRecipe.Cpk;
                    
                    
                    if (tempRecipe.getMode(i) > 0 && tempRecipe.limit.getLimitType(i) == "0")
                    {
                        /*
                        if (motionManager.MotionData[i].Mode == 0)
                        {
                            tempRecipe.BasicEndMaxForce = "0";
                            tempRecipe.BasicEndMinForce = "0";
                            tempRecipe.BasicEndMaxPos = "0";
                            tempRecipe.BasicEndMinPos = "0";
                        }
                        if (motionManager.MotionData[i].Mode == 1)
                        {
                            tempRecipe.BasicEndMaxPos = "0";
                            tempRecipe.BasicEndMinPos = "0";
                        }
                        if (motionManager.MotionData[i].Mode == 2)
                        {
                            tempRecipe.BasicEndMaxForce = "0";
                            tempRecipe.BasicEndMinForce = "0";
                        }
                        if (motionManager.MotionData[i].Mode == 3)
                        {

                        }
                        if (motionManager.MotionData[i].Mode == 4)
                        {
                            tempRecipe.BasicEndMaxForce = "0";
                            tempRecipe.BasicEndMinForce = "0";
                            tempRecipe.BasicEndMaxPos = "0";
                            tempRecipe.BasicEndMinPos = "0";
                        }
                        if (motionManager.MotionData[i].Mode == 5)
                        {
                            tempRecipe.BasicEndMaxForce = "0";
                            tempRecipe.BasicEndMinForce = "0";
                        }
                        if (motionManager.MotionData[i].Mode == 6)
                        {
                            tempRecipe.BasicEndMaxForce = "0";
                            tempRecipe.BasicEndMinForce = "0";
                            tempRecipe.BasicEndMaxPos = "0";
                            tempRecipe.BasicEndMinPos = "0";
                        }
                        */
                        tempRecipe.limit.EndMinForce[i] = tempRecipe.BasicEndMinForce;
                        tempRecipe.limit.EndMaxForce[i] = tempRecipe.BasicEndMaxForce;
                        tempRecipe.limit.EndMinPos[i] = tempRecipe.BasicEndMinPos;
                        tempRecipe.limit.EndMaxPos[i] = tempRecipe.BasicEndMaxPos;
                    }
                    /*
                    if (motionManager.MotionData[i].LimitType != "1")
                    {
                        tempRecipe.limit.BeginMinForce[i] = "0";
                        tempRecipe.limit.BeginMaxForce[i] = "0";
                    }
                    */

                    motionManager.MotionData[i].LimitType = tempRecipe.limit.getLimitType(i);
                    motionManager.MotionData[i].BeginMinForce = tempRecipe.limit.BeginMinForce[i];
                    motionManager.MotionData[i].BeginMaxForce = tempRecipe.limit.BeginMaxForce[i];
                    motionManager.MotionData[i].EndMinForce = tempRecipe.limit.EndMinForce[i];
                    motionManager.MotionData[i].EndMaxForce = tempRecipe.limit.EndMaxForce[i];
                    motionManager.MotionData[i].EndMinPos = tempRecipe.limit.EndMinPos[i];
                    motionManager.MotionData[i].EndMaxPos = tempRecipe.limit.EndMaxPos[i];
                }
            }
        }
        private int Check_Empty() //確認輸入欄位是否有空格
        {
            double dTemp = 0;
            if (txtOriginPos.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.OriginPos, out dTemp)) == "False")) return -1;
            if (txtOriginVel.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.OriginVel, out dTemp)) == "False")) return -1;

            if (txtStandbyPos.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.StandbyPos, out dTemp)) == "False")) return -1;
            if (txtStandbyVel.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.StandbyVel, out dTemp)) == "False")) return -1;
            if (txtStandbyTime.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.StandbyTime, out dTemp)) == "False")) return -1;

            if (rbIOSignal.Checked == false)
            {
                if (txtPressPos.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.PressingPos, out dTemp)) == "False")) return -1;
                if (txtPressForce.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.PressingForce, out dTemp)) == "False")) return -1;
                if (txtPressVel.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.PressingVel, out dTemp)) == "False")) return -1;
                if (txtPressTime.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.PressingTime, out dTemp)) == "False")) return -1;
            }

            if (txtBasicMinForce.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.BasicEndMinForce, out dTemp)) == "False")) return -1;
            if (txtBasicMaxForce.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.BasicEndMaxForce, out dTemp)) == "False")) return -1;
            if (txtBasicEndMinPos.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.BasicEndMinPos, out dTemp)) == "False")) return -1;
            if (txtBasicEndMaxPos.Enabled == true && (Convert.ToString(Double.TryParse(tempRecipe.BasicEndMaxPos, out dTemp)) == "False")) return -1;

            if (txtEndMinPosLimit.Visible == true && txtEndMinPosLimit.Enabled == true && (Convert.ToString(Double.TryParse(txtEndMinPosLimit.Text, out dTemp)) == "False")) return -1;
            if (txtEndMaxPosLimit.Visible == true && txtEndMaxPosLimit.Enabled == true && (Convert.ToString(Double.TryParse(txtEndMaxPosLimit.Text, out dTemp)) == "False")) return -1;
            if (txtEndMinForceLimit.Visible == true && txtEndMinForceLimit.Enabled == true && (Convert.ToString(Double.TryParse(txtEndMinForceLimit.Text, out dTemp)) == "False")) return -1;
            if (txtEndMaxForceLimit.Visible == true && txtEndMaxForceLimit.Enabled == true && (Convert.ToString(Double.TryParse(txtEndMaxForceLimit.Text, out dTemp)) == "False")) return -1;

            if (txtBeginMaxForceLimit.Visible == true && txtBeginMaxForceLimit.Enabled == true && (Convert.ToString(Double.TryParse(txtBeginMaxForceLimit.Text, out dTemp)) == "False")) return -1;
            if (txtBeginMinForceLimit.Visible == true && txtBeginMinForceLimit.Enabled == true && (Convert.ToString(Double.TryParse(txtBeginMinForceLimit.Text, out dTemp)) == "False")) return -1;
            
            return 0;
        }
        private int CheckStandbyPos() //確認預備位置與壓合位置的關係
        {
            if (tempRecipe.Mode == 1 || tempRecipe.Mode == 4) //位置模式 || 壓力位置模式
            {
                if ((tempRecipe.Step == 1) && (Double.Parse(tempRecipe.PressingPos) < Double.Parse(tempRecipe.StandbyPos))) return -1;
            }
            return 0;
        }

        private int CheckPressPos()   //確認壓合位置是否有超出最大值
        {
            if (tempRecipe.Mode == 1 || tempRecipe.Mode == 4) //位置模式 || 壓力位置模式
            {
                //if (Double.Parse(tempRecipe.PressingPos) > infor.PositionMax) return -1;
            }
            return 0;
        }
        private int CheckPressForce() //確認壓合力量是否有超出最大值
        {
            if (tempRecipe.Mode == 2 || tempRecipe.Mode == 4 || tempRecipe.Mode == 5) //位置模式 || 壓力位置模式
            {
                if (Double.Parse(tempRecipe.PressingForce) > infor.ForceMax) return -1;
            }
            return 0;
        }
        private int CheckValueLimit(out string item, out int max) //確認其他參數是否超出最大值
        {
            item = "";
            max = 0;
            if (CheckMax(tempRecipe.OriginPos, infor.PositionMax) == -1)
            {
                item = lblOriginPos.Text;
                max = infor.PositionMax;
            }
            if (CheckMax(tempRecipe.OriginVel, infor.VelocityMax) == -1)
            {
                item = lblOriginVel.Text;
                max = infor.VelocityMax;
            }


            if (CheckMax(tempRecipe.StandbyPos, infor.PositionMax) == -1)
            {
                item = lblStandbyPos.Text;
                max = infor.PositionMax;
            }

            if (CheckMax(tempRecipe.StandbyVel, infor.VelocityMax) == -1)
            {
                item = lblStandbyVel.Text;
                max = infor.VelocityMax;
            }

            if (CheckMax(tempRecipe.StandbyTime, 6500) == -1)
            {
                item = lblStandbyTime.Text;
                max = 6500;
            }


            if (tempRecipe.Mode != 6)
            {
                if (CheckMax(tempRecipe.PressingPos, infor.PositionMax) == -1)
                {
                    item = lblPressPos.Text;
                    max = infor.PositionMax;
                }
                if (CheckMax(tempRecipe.PressingForce, infor.ForceMax) == -1)
                {
                    item = lblPressForce.Text;
                    max = infor.ForceMax;
                }
                if (CheckMax(tempRecipe.PressingVel, 40) == -1)
                {
                    item = lblPressVel.Text;
                    max = 40;
                }
                if (CheckMax(tempRecipe.PressingTime, 6500) == -1)
                {
                    item = lblPressTime.Text;
                    max = 6500;
                }
            }

            if (CheckMax(tempRecipe.BasicEndMinPos, infor.PositionMax) == -1)
            {
                item = txtBasicEndMinPos.Text;
                max = infor.PositionMax;
            }
            if (CheckMax(tempRecipe.BasicEndMaxPos, infor.PositionMax) == -1)
            {
                item = txtBasicEndMaxPos.Text;
                max = infor.PositionMax;
            }

            if (CheckMax(tempRecipe.BeginMinForce, infor.ForceMax) == -1)
            {
                item = lblBeginMinForce.Text;
                max = infor.ForceMax;
            }

            if (CheckMax(tempRecipe.BeginMaxForce, infor.ForceMax) == -1)
            {
                item = lblBeginMaxForce.Text;
                max = infor.ForceMax;
            }

            if (item == "") return 0;
            else return -1;


        }
        private int CheckMax(string value, double max)
        {
            if ((Convert.ToDouble(value) > max)) return -1;
            return 0;
        }  //最大值關係比較
        private int CheckBasicPos()  //確認位置限制大小關係
        {
            if (tempRecipe.BasicEndMaxPos != null && tempRecipe.BasicEndMinPos != null)
            {
                if (Double.Parse(tempRecipe.BasicEndMaxPos) < Double.Parse(tempRecipe.BasicEndMinPos)) return -1;
            }
            return 0;
        }
        private int CheckBasicForce() //確認壓力限制大小關係
        {
            if (tempRecipe.BasicEndMaxForce != null && tempRecipe.BasicEndMinForce != null)
            {
                if (Double.Parse(tempRecipe.BasicEndMaxForce) < Double.Parse(tempRecipe.BasicEndMinForce)) return -1;
            }
            return 0;
        }



        private void btnImport_Click(object sender, EventArgs e) //外部手動控制曲線載入
        {
            OpenFileDialog oCSVfile = new OpenFileDialog();
            oCSVfile.Filter = "csv(*.csv)|*.csv";

            if (oCSVfile.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(oCSVfile.FileName, Encoding.Default);
                chart.DrawExternalCurve(sr);
                sr.Close();
                chart.DrawLine();
                chart.ExternalCurve = true;
            }
        }
        private void btnExport_Click(object sender, EventArgs e) //外部手動控制曲線輸出
        {
            jogChart.Export();
        }
        private void btnClear_Click(object sender, EventArgs e)  //清除曲線
        {
            jogChart.Reset();
            chart.cleanCurve();
            chart.ExternalCurve = false;
        }

        private void chkLock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAuto.Checked == true) chart.Unlock();
            else if (chkAuto.Checked == false) chart.Lock(); 
        }



        private void chart1_Click(object sender, EventArgs e)
        {
            if (rbOriginPos.Checked == true && tempRecipe.Mode != 0)
            {
                tempRecipe.setOriginPos(Convert.ToString(chart1.ChartAreas[0].CursorX.Position));
                chart.DrawLine();
            }
            else if (rbStandbyPos.Checked == true && tempRecipe.Mode != 0)
            {
                tempRecipe.setStandbyPos(Convert.ToString(chart1.ChartAreas[0].CursorX.Position));
                chart.DrawLine();
            }
            else if (rbPressingPos.Checked == true && tempRecipe.Mode == 1)
            {
                tempRecipe.setPressingPos(Convert.ToString(chart1.ChartAreas[0].CursorX.Position));
                chart.DrawLine();
            }
            else if (rbPressingPos.Checked == true && tempRecipe.Mode == 2)
            {
                tempRecipe.setPressingForce(Convert.ToString(chart1.ChartAreas[0].CursorY.Position));
                chart.DrawLine();
            }
            else if (rbPressingPos.Checked == true && tempRecipe.Mode == 3)
            {
                tempRecipe.setPressingPos(Convert.ToString(chart1.ChartAreas[0].CursorX.Position));
                chart.DrawLine();
            }
            else if (rbPosLimit.Checked == true)
            {
                chart.cleanFrameLimit();
                if (counter == 0)
                {
                    chart.X1 = chart1.ChartAreas[0].CursorX.Position;
                    counter++;
                }
                else if (counter == 1)
                {
                    chart.X2 = chart1.ChartAreas[0].CursorX.Position;
                    tempRecipe.BasicEndMinPos = Convert.ToString(chart.X1);
                    tempRecipe.BasicEndMaxPos = Convert.ToString(chart.X2);
                    chart.DrawLine();
                    counter = 0;
                }
            }
            else if (rbForceLimit.Checked == true)
            {
                chart.cleanFrameLimit();
                if (counter == 0)
                {
                    chart.Y1 = chart1.ChartAreas[0].CursorY.Position;
                    counter++;
                }
                else if (counter == 1)
                {
                    chart.Y2 = chart1.ChartAreas[0].CursorY.Position;
                    tempRecipe.BasicEndMinForce = Convert.ToString(chart.Y1);
                    tempRecipe.BasicEndMaxForce = Convert.ToString(chart.Y2);
                    chart.DrawLine();
                    counter = 0;
                }
            }
            else if (rbDynamic.Checked == true)
            {
                chart.cleanAllBasicLimit();
                if (counter == 0)
                {
                    tempRecipe.BeginMaxForce = Convert.ToString(chart1.ChartAreas[0].CursorY.Position);
                    tempRecipe.EndMinPos = Convert.ToString(chart1.ChartAreas[0].CursorX.Position);
                    counter = 1;
                }
                else if (counter == 1)
                {
                    tempRecipe.EndMaxForce = Convert.ToString(chart1.ChartAreas[0].CursorY.Position);
                    tempRecipe.EndMaxPos = Convert.ToString(chart1.ChartAreas[0].CursorX.Position);
                    chart.drawDynamicTop();
                    counter = 2;
                }
                else if (counter == 2)
                {
                    tempRecipe.EndMinForce = Convert.ToString(chart1.ChartAreas[0].CursorY.Position);
                    chart.drawDynamicRight();
                    counter = 3;
                }
                else if (counter == 3)
                {
                    tempRecipe.BeginMinForce = Convert.ToString(chart1.ChartAreas[0].CursorY.Position);
                    chart.drawDynamicBottom();
                    chart.drawDynamicLeft();
                    counter = 0;
                }
            }
            else if (tempRecipe.limit.LimitType >= 2)
            {
                chart.cleanAllBasicLimit();
                if (counter == 0)
                {
                    chart.X1 = chart1.ChartAreas[0].CursorX.Position;
                    chart.Y1 = chart1.ChartAreas[0].CursorY.Position;
                    counter++;
                }
                else if (counter == 1)
                {
                    chart.X2 = chart1.ChartAreas[0].CursorX.Position;
                    chart.Y2 = chart1.ChartAreas[0].CursorY.Position;

                    tempRecipe.EndMinPos = Convert.ToString(chart.X1);
                    tempRecipe.EndMaxPos = Convert.ToString(chart.X2);
                    tempRecipe.EndMinForce = Convert.ToString(chart.Y1);
                    tempRecipe.EndMaxForce = Convert.ToString(chart.Y2);

                    chart.drawFrameLimit();
                    counter = 0;
                }
            }

        }

        private void tmrLive_Tick(object sender, EventArgs e) //即時繪圖Timer
        {
            chart.Update();
            if (plc.State == Convert.ToInt16(Ether_Addr.ST_Unit))
            {
                double x, y;
                jogChart.Drawing(out x, out y);
                chart.DrawCurve(x, y);
            }
        }

        private void tmrUI_Tick(object sender, EventArgs e) //UI Timer
        {

            

            PressModeUI();
            StartConditionUI();
            PressConditionUI();
            CpkUI();

            
        }

        private void ManualUI()
        {
            plc.ManualUpdate();

            txtLivePos.Text = Convert.ToString(plc.LivePos);
            txtLiveForce.Text = Convert.ToString(plc.LiveForce);

            txtContactPos.Text = Convert.ToString(plc.Manual_ContactPos);
            txtMaxPos.Text = Convert.ToString(plc.Manual_MaxPos);
            txtMaxForce.Text = Convert.ToString(plc.Manual_MaxForce);
            txtPosAtMaxForce.Text = Convert.ToString(plc.Manual_PosAtMaxForce);
        }

        private void ChartScaleUI()
        {
            txtXmin.Text = Convert.ToString(chart.XMin);
            txtXMax.Text = Convert.ToString(chart.XMax);
            txtYmin.Text = Convert.ToString(chart.YMin);
            txtYMax.Text = Convert.ToString(chart.YMax);

            if (chart.LockSacleStatus == true)
            {
                txtXMax.Enabled = true;
                txtXmin.Enabled = true;
                txtYMax.Enabled = true;
                txtYmin.Enabled = true;
            }
            else if (chart.LockSacleStatus == false)
            {
                txtXMax.Enabled = false;
                txtXmin.Enabled = false;
                txtYMax.Enabled = false;
                txtYmin.Enabled = false;
            }
        }

        private void PressModeUI()
        {
            
            if (tempRecipe.Step == 1) rb1.Checked = true;
            
            if (tempRecipe.Mode == 0) rbNoMode.Checked = true;//不作動
            else if (tempRecipe.Mode == 1) rbPosMode.Checked = true;//位置模式
            else if (tempRecipe.Mode == 2) rbForceMode.Checked = true; //壓力模式
            else if (tempRecipe.Mode == 3) rbDistMode.Checked = true;//距離模式
            else if (tempRecipe.Mode == 4) rbForcePosMode.Checked = true;//壓力位置模式
            else if (tempRecipe.Mode == 5) rbForceDistMode.Checked = true;//壓力距離模式
            else if (tempRecipe.Mode == 6) rbIOSignal.Checked = true;//等待訊號
        }
        private void StartConditionUI()
        {
            txtOriginPos.Text = Convert.ToString(tempRecipe.OriginPos);
            txtOriginVel.Text = Convert.ToString(tempRecipe.OriginVel);

            txtStandbyPos.Text = Convert.ToString(tempRecipe.StandbyPos);
            txtStandbyVel.Text = Convert.ToString(tempRecipe.StandbyVel);
            txtStandbyTime.Text = Convert.ToString(tempRecipe.StandbyTime);
        }
        private void PressConditionUI()
        {
            if (tempRecipe.Mode == 0) //不作動
            {
                pnPressing.Enabled = false;

                txtPressPos.Enabled = false;
                txtPressForce.Enabled = false;
                txtPressVel.Enabled = false;
                txtPressTime.Enabled = false;

                txtPressPos.Visible = true;
                txtPressForce.Visible = true;
                txtPressVel.Visible = true;
                txtPressTime.Visible = true;

                lblPressPosUnit.Visible = true;
                lblPressForceUnit.Visible = true;
                lblPressVelUnit.Visible = true;
                lblPressTimeUnit.Visible = true;
            }
            else if (tempRecipe.Mode >= 1 && tempRecipe.Mode <= 5)
            {
                pnPressing.Enabled = true;

                txtPressVel.Enabled = true;
                txtPressTime.Enabled = true;

                txtPressVel.Visible = true;
                txtPressTime.Visible = true;

                lblPressPosUnit.Visible = true;
                lblPressForceUnit.Visible = true;
                lblPressVelUnit.Visible = true;
                lblPressTimeUnit.Visible = true;

                if (tempRecipe.Mode == 1) //位置模式
                {
                    lblPressPos.Text = rM.GetString("PressPos");
                    txtPressPos.Enabled = true;
                    txtPressForce.Enabled = false;

                    txtPressPos.Visible = true;
                    txtPressForce.Visible = true;
                }
                if (tempRecipe.Mode == 2) //壓力模式
                {
                    lblPressPos.Text = rM.GetString("PressPos");
                    txtPressPos.Enabled = false;
                    txtPressForce.Enabled = true;

                    txtPressPos.Visible = true;
                    txtPressForce.Visible = true;
                }
                if (tempRecipe.Mode == 3) //距離模式
                {
                    lblPressPos.Text = rM.GetString("PressDist");
                    txtPressPos.Enabled = true;
                    txtPressForce.Enabled = false;

                    txtPressPos.Visible = true;
                    txtPressForce.Visible = true;
                }
                if (tempRecipe.Mode == 4) //壓力位置模式
                {
                    lblPressPos.Text = rM.GetString("PressPos");
                    txtPressPos.Enabled = true;
                    txtPressForce.Enabled = true;

                    txtPressPos.Visible = true;
                    txtPressForce.Visible = true;
                }
                if (tempRecipe.Mode == 5) //壓力距離模式
                {
                    lblPressPos.Text = rM.GetString("PressDist");
                    txtPressPos.Enabled = true;
                    txtPressForce.Enabled = true;

                    txtPressPos.Visible = true;
                    txtPressForce.Visible = true;
                }
            }
            else if (tempRecipe.Mode == 6)
            {
                pnPressing.Enabled = true;

                txtPressPos.Enabled = false;
                txtPressForce.Enabled = false;
                txtPressVel.Enabled = false;
                txtPressTime.Enabled = false;

                txtPressPos.Visible = false;
                txtPressForce.Visible = false;
                txtPressVel.Visible = false;
                txtPressTime.Visible = false;

                lblPressPosUnit.Visible = false;
                lblPressForceUnit.Visible = false;
                lblPressVelUnit.Visible = false;
                lblPressTimeUnit.Visible = false;
            }

            txtPressPos.Text = Convert.ToString(tempRecipe.PressingPos);

            if (plc.Unit == 0)
            {
                txtPressForce.Text = Convert.ToString(tempRecipe.PressingForce);
            }
            if (plc.Unit == 1)
            {
                txtPressForce.Text = Convert.ToString(Convert.ToDouble(tempRecipe.PressingForce)*10);
            }
            if (plc.Unit == 2)
            {
                txtPressForce.Text = Convert.ToString(Convert.ToDouble(tempRecipe.PressingForce) /2);
            }

            txtPressVel.Text = Convert.ToString(tempRecipe.PressingVel);
            txtPressTime.Text = Convert.ToString(tempRecipe.PressingTime);
        }
        private void SignalModeUI()
        {
            if (tempRecipe.Mode != 6)
            {
                cboIO_1.Visible = false;
                cboIO_2.Visible = false;
                cboIO_3.Visible = false;
                cboIO_4.Visible = false;

                txtIODelay_1.Visible = false;
                txtIODelay_2.Visible = false;
                txtIODelay_3.Visible = false;
                txtIODelay_4.Visible = false;
                lblIODelayUnit_1.Visible = false;
                lblIODelayUnit_2.Visible = false;
                lblIODelayUnit_3.Visible = false;
                lblIODelayUnit_4.Visible = false;
                cboOuputSignal_1.Visible = false;
                cboOuputSignal_2.Visible = false;
                cboOuputSignal_3.Visible = false;
                cboOuputSignal_4.Visible = false;
                cboWaitingSignal_1.Visible = false;
                cboWaitingSignal_2.Visible = false;
                cboWaitingSignal_3.Visible = false;
                cboWaitingSignal_4.Visible = false;
            }
            else if (tempRecipe.Mode == 6)
            {
                cboIO_1.Visible = true;
                cboIO_2.Visible = true;
                cboIO_3.Visible = true;
                cboIO_4.Visible = true;

                tempRecipe.IOSave();

                //IO 模式第一步
                if (tempRecipe.Signal_1 > 0)
                {
                    cboIO_1.SelectedIndex = tempRecipe.Signal_1;
                    txtIODelay_1.Visible = false;
                    lblIODelayUnit_1.Visible = false;
                    cboOuputSignal_1.Visible = false;
                    cboWaitingSignal_1.Visible = false;
                }

                if (tempRecipe.Signal_1 == 0)
                {
                    cboOuputSignal_1.Visible = false;//輸出訊號
                    txtIODelay_1.Visible = false;
                    lblIODelayUnit_1.Visible = false;
                    cboWaitingSignal_1.Visible = false;
                }
                else if (tempRecipe.Signal_1 == 1)
                {
                    cboOuputSignal_1.Visible = true;//輸出訊號
                    cboOuputSignal_1.SelectedIndex = tempRecipe.SignalPara_1;
                    txtIODelay_1.Visible = false;
                    lblIODelayUnit_1.Visible = false;
                    cboWaitingSignal_1.Visible = false;
                }
                else if (tempRecipe.Signal_1 == 2)
                {
                    cboWaitingSignal_1.Visible = true;//等待訊號
                    cboWaitingSignal_1.SelectedIndex = tempRecipe.SignalPara_1;
                    txtIODelay_1.Visible = false;
                    lblIODelayUnit_1.Visible = false;
                    cboOuputSignal_1.Visible = false;
                }
                else if (tempRecipe.Signal_1 == 3 || tempRecipe.Signal_1 == 4)
                {
                    txtIODelay_1.Visible = true;
                    lblIODelayUnit_1.Visible = true;
                    txtIODelay_1.Text = Convert.ToString(tempRecipe.SignalPara_1 / 10);
                    cboOuputSignal_1.Visible = false;
                    cboWaitingSignal_1.Visible = false;
                }

                //IO 模式第二步
                if (tempRecipe.Signal_2 > 0)
                {
                    cboIO_2.SelectedIndex = tempRecipe.Signal_2;
                    txtIODelay_2.Visible = false;
                    lblIODelayUnit_2.Visible = false;
                    cboOuputSignal_2.Visible = false;
                    cboWaitingSignal_2.Visible = false;
                }

                if (tempRecipe.Signal_2 == 0)
                {
                    cboOuputSignal_2.Visible = false;
                    txtIODelay_2.Visible = false;
                    lblIODelayUnit_2.Visible = false;
                    cboWaitingSignal_2.Visible = false;
                }
                else if (tempRecipe.Signal_2 == 1)
                {
                    cboOuputSignal_2.Visible = true;
                    cboOuputSignal_2.SelectedIndex = tempRecipe.SignalPara_2;
                    txtIODelay_2.Visible = false;
                    lblIODelayUnit_2.Visible = false;
                    cboWaitingSignal_2.Visible = false;
                }
                else if (tempRecipe.Signal_2 == 2)
                {
                    cboWaitingSignal_2.Visible = true;//等待訊號
                    cboWaitingSignal_2.SelectedIndex = tempRecipe.SignalPara_2;
                    txtIODelay_2.Visible = false;
                    lblIODelayUnit_2.Visible = false;
                    cboOuputSignal_2.Visible = false;
                }
                else if (tempRecipe.Signal_2 == 3 || tempRecipe.Signal_2 == 4)
                {
                    txtIODelay_2.Visible = true;
                    lblIODelayUnit_2.Visible = true;
                    txtIODelay_2.Text = Convert.ToString(tempRecipe.SignalPara_2 / 10);
                    cboOuputSignal_2.Visible = false;
                    cboWaitingSignal_2.Visible = false;
                }

                //IO 模式第三步
                if (tempRecipe.Signal_3 > 0)
                {
                    cboIO_3.SelectedIndex = tempRecipe.Signal_3;
                    txtIODelay_3.Visible = false;
                    lblIODelayUnit_3.Visible = false;
                    cboOuputSignal_3.Visible = false;
                    cboWaitingSignal_3.Visible = false;
                }

                if (tempRecipe.Signal_3 == 0)
                {
                    cboOuputSignal_3.Visible = false;
                    txtIODelay_3.Visible = false;
                    lblIODelayUnit_3.Visible = false;
                    cboWaitingSignal_3.Visible = false;
                }
                else if (tempRecipe.Signal_3 == 1)
                {
                    cboOuputSignal_3.Visible = true;
                    cboOuputSignal_3.SelectedIndex = tempRecipe.SignalPara_3;
                    txtIODelay_3.Visible = false;
                    lblIODelayUnit_3.Visible = false;
                    cboWaitingSignal_3.Visible = false;
                }
                else if (tempRecipe.Signal_3 == 2)
                {
                    cboWaitingSignal_3.Visible = true;//等待訊號
                    cboWaitingSignal_3.SelectedIndex = tempRecipe.SignalPara_3;
                    txtIODelay_3.Visible = false;
                    lblIODelayUnit_3.Visible = false;
                    cboOuputSignal_3.Visible = false;
                }
                else if (tempRecipe.Signal_3 == 3 || tempRecipe.Signal_3 == 4)
                {
                    txtIODelay_3.Visible = true;
                    lblIODelayUnit_3.Visible = true;
                    txtIODelay_3.Text = Convert.ToString(tempRecipe.SignalPara_3 / 10);
                    cboOuputSignal_3.Visible = false;
                    cboWaitingSignal_3.Visible = false;
                }

                //IO 模式第四步
                if (tempRecipe.Signal_4 > 0)
                {
                    cboIO_4.SelectedIndex = tempRecipe.Signal_4;
                    txtIODelay_4.Visible = false;
                    lblIODelayUnit_4.Visible = false;
                    cboOuputSignal_4.Visible = false;
                    cboWaitingSignal_4.Visible = false;
                }

                if (tempRecipe.Signal_4 == 0)
                {
                    cboOuputSignal_4.Visible = false;
                    txtIODelay_4.Visible = false;
                    lblIODelayUnit_4.Visible = false;
                    cboWaitingSignal_4.Visible = false;
                }
                else if (tempRecipe.Signal_4 == 1)
                {
                    cboOuputSignal_4.Visible = true;
                    cboOuputSignal_4.SelectedIndex = tempRecipe.SignalPara_4;
                    txtIODelay_4.Visible = false;
                    lblIODelayUnit_4.Visible = false;
                    cboWaitingSignal_4.Visible = false;
                }
                else if (tempRecipe.Signal_4 == 2)
                {
                    cboWaitingSignal_4.Visible = true;//等待訊號
                    cboWaitingSignal_4.SelectedIndex = tempRecipe.SignalPara_4;
                    txtIODelay_4.Visible = false;
                    lblIODelayUnit_4.Visible = false;
                    cboOuputSignal_4.Visible = false;
                }
                else if (tempRecipe.Signal_4 == 3 || tempRecipe.Signal_4 == 4)
                {
                    txtIODelay_4.Visible = true;
                    lblIODelayUnit_4.Visible = true;
                    txtIODelay_4.Text = Convert.ToString(tempRecipe.SignalPara_4 / 10);
                    cboOuputSignal_4.Visible = false;
                    cboWaitingSignal_4.Visible = false;
                }
            }
        }
        private void CpkUI()
        {
            if (tempRecipe.Cpk == "Force")     //Cpk設定讀取
            {
                rbCpkForce.Checked = true;
                rbCpkPosition.Checked = false;
            }
            else if (tempRecipe.Cpk == "Position")
            {
                rbCpkPosition.Checked = true;
                rbCpkForce.Checked = false;
            }
            else if (tempRecipe.Cpk == "-")
            {
                rbCpkForce.Checked = false;
                rbCpkPosition.Checked = false;
            }


            if (tempRecipe.Mode == 0)
            {
                //pnCpk.Enabled = false;
                rbCpkForce.Enabled = false;
                rbCpkPosition.Enabled = false;
            }
            else
            {
                //pnCpk.Enabled = true;
                rbCpkForce.Enabled = true;
                rbCpkPosition.Enabled = true;

                if (tempRecipe.Mode == 1) //Position Mode
                {
                    rbCpkPosition.Enabled = false;
                    rbCpkForce.Enabled = true;
                    rbCpkForce.Checked = true;
                }
                else if (tempRecipe.Mode == 2) //Force Mode
                {
                    rbCpkPosition.Enabled = true;
                    rbCpkPosition.Checked= true;
                    rbCpkForce.Enabled = false;
                }
                else if (tempRecipe.Mode == 3)  //Distance Mode
                {
                    rbCpkPosition.Enabled = true;
                    rbCpkForce.Enabled = true;
                }

                else if (tempRecipe.Mode == 4)  //Force Position Mode
                {
                    rbCpkPosition.Enabled = false;
                    rbCpkForce.Enabled = true;
                    rbCpkForce.Checked = true;
                }
                else if (tempRecipe.Mode == 5)  //Force Distance Mode
                {
                    rbCpkPosition.Enabled = true;
                    rbCpkPosition.Checked = true;
                    rbCpkForce.Enabled = false;
                }
                else if (tempRecipe.Mode == 6) //IO Signal
                {
                    rbCpkPosition.Enabled = false;
                    rbCpkForce.Enabled = false;
                }
            }
        }


        private void BasicLimitUI()
        {
            if ((tempRecipe.Step == tempRecipe.limit.limitStep) && (tempRecipe.limit.LimitType == 0)) //基本限制
            {
                if (tempRecipe.limit.LimitType == 0)
                {
                    if (tempRecipe.Mode == 0)  //不作動
                    {
                        pnForceLimit.Enabled = false;
                        pnForceLimit.BackColor = SystemColors.Menu;
                        //pnPosLimit.Enabled = false;
                        //pnPosLimit.BackColor = SystemColors.Menu;
                        txtBasicEndMinPos.Enabled = false;
                        txtBasicEndMaxPos.Enabled = false;



                    }
                    else if (tempRecipe.Mode == 1) //位置模式
                    {
                        pnForceLimit.Enabled = true;
                        pnForceLimit.BackColor = Color.White;
                        //pnPosLimit.Enabled = false;
                        //pnPosLimit.BackColor = SystemColors.Menu;
                        txtBasicEndMinPos.Enabled = false;
                        txtBasicEndMaxPos.Enabled = false;
                    }
                    else if (tempRecipe.Mode == 2) //壓力模式
                    {
                        pnForceLimit.Enabled = false;
                        pnForceLimit.BackColor = SystemColors.Menu;
                        //pnPosLimit.Enabled = true;
                        //pnPosLimit.BackColor = Color.White;
                        txtBasicEndMinPos.Enabled = true;
                        txtBasicEndMaxPos.Enabled = true;
                    }
                    else if (tempRecipe.Mode == 3) //距離模式
                    {
                        pnForceLimit.Enabled = true;
                        pnForceLimit.BackColor = Color.White;
                        //pnPosLimit.Enabled = true;
                        //pnPosLimit.BackColor = Color.White;
                        txtBasicEndMinPos.Enabled = true;
                        txtBasicEndMaxPos.Enabled = true;
                    }
                    else if (tempRecipe.Mode == 4) //壓力位置模式
                    {
                        pnForceLimit.Enabled = false;
                        pnForceLimit.BackColor = SystemColors.Menu;
                        //pnPosLimit.Enabled = false;
                        //pnPosLimit.BackColor = SystemColors.Menu;
                        txtBasicEndMinPos.Enabled = false;
                        txtBasicEndMaxPos.Enabled = false;
                    }
                    else if (tempRecipe.Mode == 5) //壓力距離模式
                    {
                        pnForceLimit.Enabled = false;
                        pnForceLimit.BackColor = SystemColors.Menu;
                        //pnPosLimit.Enabled = true;
                        //pnPosLimit.BackColor = Color.White
                        txtBasicEndMinPos.Enabled = false;
                        txtBasicEndMaxPos.Enabled = false;

                    }
                    else if (tempRecipe.Mode == 6) //訊號模式
                    {
                        pnForceLimit.Enabled = false;
                        pnForceLimit.BackColor = SystemColors.Menu;
                        //pnPosLimit.Enabled = false;
                        //pnPosLimit.BackColor = SystemColors.Menu;
                        txtBasicEndMinPos.Enabled = false;
                        txtBasicEndMaxPos.Enabled = false;
                    }
                    
                    if (tempRecipe.BasicEndMinPos == null) tempRecipe.BasicEndMinPos = "0";
                    if (tempRecipe.BasicEndMaxPos == null) tempRecipe.BasicEndMaxPos = "0";
                    if (tempRecipe.BasicEndMinForce == null) tempRecipe.BasicEndMinForce = "0";
                    if (tempRecipe.BasicEndMaxForce == null) tempRecipe.BasicEndMaxForce = "0";

                    txtBasicEndMinPos.Text = tempRecipe.BasicEndMinPos;
                    txtBasicEndMaxPos.Text = tempRecipe.BasicEndMaxPos;


                    if (plc.Unit == 0)
                    {
                        txtBasicMinForce.Text = Convert.ToString(tempRecipe.BasicEndMinForce);
                        txtBasicMaxForce.Text = Convert.ToString(tempRecipe.BasicEndMaxForce);
                    }
                    if (plc.Unit == 1)
                    {
                        txtBasicMinForce.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BasicEndMinForce) * 10);
                        txtBasicMaxForce.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BasicEndMaxForce) * 10);
                    }
                    if (plc.Unit == 2)
                    {
                        txtBasicMinForce.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BasicEndMinForce) / 2);
                        txtBasicMaxForce.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BasicEndMaxForce) / 2);
                    }
                }
            }
            else 
            {
                pnForceLimit.Enabled = false;
                pnForceLimit.BackColor = SystemColors.Menu;
                //pnPosLimit.Enabled = false;
                //pnPosLimit.BackColor = SystemColors.Menu;
                txtBasicEndMinPos.Enabled = false;
                txtBasicEndMaxPos.Enabled = false;
            }
        }
        private void GeometryLimitStepUI()
        {
            
            //幾何限制步序名稱
            lblStepLimit1.Text = tempRecipe.limit.getLimitName(0);
            lblStepLimit2.Text = tempRecipe.limit.getLimitName(1);
            lblStepLimit3.Text = tempRecipe.limit.getLimitName(2);
            lblStepLimit4.Text = tempRecipe.limit.getLimitName(3);
            lblStepLimit5.Text = tempRecipe.limit.getLimitName(4);
            lblStepLimit6.Text = tempRecipe.limit.getLimitName(5);
            lblStepLimit7.Text = tempRecipe.limit.getLimitName(6);
            lblStepLimit8.Text = tempRecipe.limit.getLimitName(7);
            lblStepLimit9.Text = tempRecipe.limit.getLimitName(8);
            lblStepLimit10.Text = tempRecipe.limit.getLimitName(9);

        }
        private void GeometryLimitTypeUI()
        {
           
            if (tempRecipe.limit.LimitType == 0)
            {
                rbNone.Checked = true;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblEndMinPosLimit.Text = rM.GetString("minPosLimit");
                lblEndMaxPosLimit.Text = rM.GetString("MaxPosLimit");
                lblEndMaxForceLimit.Text = rM.GetString("MaxForceLimit");
                lblEndMinForceLimit.Text = rM.GetString("minForceLimit");
            }
            if (tempRecipe.limit.LimitType == 1)
            {
                rbDynamic.Checked = true;
                rbNone.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblBeginMaxForce.Text = rM.GetString("BeginMaxForce");
                lblBeginMinForce.Text = rM.GetString("BeginMinForce");
                lblEndMinPosLimit.Text = rM.GetString("BeginPos");
                lblEndMaxPosLimit.Text = rM.GetString("EndPos");
                lblEndMinForceLimit.Text = rM.GetString("EndMinForce");
                lblEndMaxForceLimit.Text = rM.GetString("EndMaxForce");
            }
            if (tempRecipe.limit.LimitType == 2)
            {
                rbLR.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblEndMinPosLimit.Text = rM.GetString("minPosLimit");
                lblEndMaxPosLimit.Text = rM.GetString("MaxPosLimit");
                lblEndMinForceLimit.Text = rM.GetString("BeginForce");
                lblEndMaxForceLimit.Text = rM.GetString("EndForce");
            }
            if (tempRecipe.limit.LimitType == 3)
            {
                rbTB.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblEndMaxForceLimit.Text = rM.GetString("MaxForceLimit");
                lblEndMinForceLimit.Text = rM.GetString("minForceLimit");
                lblEndMinPosLimit.Text = rM.GetString("BeginPos");
                lblEndMaxPosLimit.Text = rM.GetString("EndPos");

            }
            if (tempRecipe.limit.LimitType == 4)
            {
                rbBR.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblEndMaxPosLimit.Text = rM.GetString("MaxPosLimit");
                lblEndMinForceLimit.Text = rM.GetString("minForceLimit");
                lblEndMaxForceLimit.Text = rM.GetString("EndForce");
                lblEndMinPosLimit.Text = rM.GetString("BeginPos");
            }
            if (tempRecipe.limit.LimitType == 5)
            {
                rbTR.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;

                lblEndMaxPosLimit.Text = rM.GetString("MaxPosLimit");
                lblEndMaxForceLimit.Text = rM.GetString("MaxForceLimit");
                lblEndMinForceLimit.Text = rM.GetString("BeginForce");
                lblEndMinPosLimit.Text = rM.GetString("BeginPos");
            }
            if (tempRecipe.limit.LimitType == 6)
            {
                rbLT.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLTR.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblEndMinPosLimit.Text = rM.GetString("minPosLimit");
                lblEndMaxForceLimit.Text = rM.GetString("MaxForceLimit");
                lblEndMinForceLimit.Text = rM.GetString("BeginForce");
                lblEndMaxPosLimit.Text = rM.GetString("EndPos");

            }
            if (tempRecipe.limit.LimitType == 7)
            {
                rbLTR.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbTBR.Checked = false;
                rbTR.Checked = false;
                lblEndMinPosLimit.Text = rM.GetString("minPosLimit");
                lblEndMaxPosLimit.Text = rM.GetString("MaxPosLimit");
                lblEndMaxForceLimit.Text = rM.GetString("MaxForceLimit");
                lblEndMinForceLimit.Text = rM.GetString("BeginForce");
            }
            if (tempRecipe.limit.LimitType == 8)
            {
                rbTBR.Checked = true;
                rbNone.Checked = false;
                rbDynamic.Checked = false;
                rbLR.Checked = false;
                rbTB.Checked = false;
                rbBR.Checked = false;
                rbLT.Checked = false;
                rbLTR.Checked = false;
                rbTR.Checked = false;
                lblEndMaxPosLimit.Text = rM.GetString("MaxPosLimit");
                lblEndMaxForceLimit.Text = rM.GetString("MaxForceLimit");
                lblEndMinForceLimit.Text = rM.GetString("minForceLimit");
                lblEndMinPosLimit.Text = rM.GetString("BeginPos");
            }
        }
        private void GeometryLimitSettingUI()
        {
            if (tempRecipe.limit.LimitType == 0)  //基本限制
            {
                lblEndMinForceLimit.Visible = false;
                lblEndMinForceLimitUnit.Visible = false;
                txtEndMinForceLimit.Visible = false;
                lblEndMaxForceLimit.Visible = false;
                lblEndMaxForceLimitUnit.Visible = false;
                txtEndMaxForceLimit.Visible = false;

                lblEndMinPosLimit.Visible = false;
                lblEndMinPosLimitUnit.Visible = false;
                txtEndMinPosLimit.Visible = false;
                lblEndMaxPosLimit.Visible = false;
                lblEndMaxPosLimitUnit.Visible = false;
                txtEndMaxPosLimit.Visible = false;

                lblBeginMaxForce.Visible = false;
                lblBeginMaxForceUnit.Visible = false;
                txtBeginMaxForceLimit.Visible = false;
                lblBeginMinForce.Visible = false;
                lblBeginMinForceUnit.Visible = false;
                txtBeginMinForceLimit.Visible = false;

                txtEndMinForceLimit.Enabled = false;
                txtEndMaxForceLimit.Enabled = false;
                txtEndMinPosLimit.Enabled = false;
                txtEndMaxPosLimit.Enabled = false;
                txtBeginMaxForceLimit.Enabled = false;
                txtBeginMinForceLimit.Enabled = false;
            }
            else if (tempRecipe.limit.LimitType != 0)
            {
                if (tempRecipe.limit.LimitType == 1)
                {
                    lblEndMinForceLimit.Visible = true;
                    lblEndMinForceLimitUnit.Visible = true;
                    txtEndMinForceLimit.Visible = true;
                    lblEndMaxForceLimit.Visible = true;
                    lblEndMaxForceLimitUnit.Visible = true;
                    txtEndMaxForceLimit.Visible = true;

                    lblEndMinPosLimit.Visible = true;
                    lblEndMinPosLimitUnit.Visible = true;
                    txtEndMinPosLimit.Visible = true;
                    lblEndMaxPosLimit.Visible = true;
                    lblEndMaxPosLimitUnit.Visible = true;
                    txtEndMaxPosLimit.Visible = true;

                    lblBeginMaxForce.Visible = true;
                    lblBeginMaxForceUnit.Visible = true;
                    txtBeginMaxForceLimit.Visible = true;
                    lblBeginMinForce.Visible = true;
                    lblBeginMinForceUnit.Visible = true;
                    txtBeginMinForceLimit.Visible = true;

                    txtEndMinForceLimit.Enabled = true;
                    txtEndMaxForceLimit.Enabled = true;
                    txtEndMinPosLimit.Enabled = true;
                    txtEndMaxPosLimit.Enabled = true;
                    txtBeginMaxForceLimit.Enabled = true;
                    txtBeginMinForceLimit.Enabled = true;
                }
                else if (tempRecipe.limit.LimitType > 1)
                {
                    lblEndMinForceLimit.Visible = true;
                    lblEndMinForceLimitUnit.Visible = true;
                    txtEndMinForceLimit.Visible = true;
                    lblEndMaxForceLimit.Visible = true;
                    lblEndMaxForceLimitUnit.Visible = true;
                    txtEndMaxForceLimit.Visible = true;

                    lblEndMinPosLimit.Visible = true;
                    lblEndMinPosLimitUnit.Visible = true;
                    txtEndMinPosLimit.Visible = true;
                    lblEndMaxPosLimit.Visible = true;
                    lblEndMaxPosLimitUnit.Visible = true;
                    txtEndMaxPosLimit.Visible = true;

                    lblBeginMaxForce.Visible = false;
                    lblBeginMaxForceUnit.Visible = false;
                    txtBeginMaxForceLimit.Visible = false;
                    lblBeginMinForce.Visible = false;
                    lblBeginMinForceUnit.Visible = false;
                    txtBeginMinForceLimit.Visible = false;

                    txtEndMinForceLimit.Enabled = true;
                    txtEndMaxForceLimit.Enabled = true;
                    txtEndMinPosLimit.Enabled = true;
                    txtEndMaxPosLimit.Enabled = true;
                    txtBeginMaxForceLimit.Enabled = false;
                    txtBeginMinForceLimit.Enabled = false;
                }

                
                txtEndMinPosLimit.Text = tempRecipe.EndMinPos;
                txtEndMaxPosLimit.Text = tempRecipe.EndMaxPos;
                if (plc.Unit == 0)
                {
                    txtEndMinForceLimit.Text = tempRecipe.EndMinForce;
                    txtEndMaxForceLimit.Text = tempRecipe.EndMaxForce;
                    txtBeginMaxForceLimit.Text = tempRecipe.BeginMaxForce;
                    txtBeginMinForceLimit.Text = tempRecipe.BeginMinForce;
                }
                else if(plc.Unit == 1)
                {
                    txtEndMinForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.EndMinForce) * 10);
                    txtEndMaxForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.EndMaxForce) * 10);
                    txtBeginMaxForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BeginMaxForce) * 10);
                    txtBeginMinForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BeginMinForce) * 10);
                }
                else if (plc.Unit == 2)
                {
                    txtEndMinForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.EndMinForce) / 2);
                    txtEndMaxForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.EndMaxForce) / 2);
                    txtBeginMaxForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BeginMaxForce) / 2);
                    txtBeginMinForceLimit.Text = Convert.ToString(Convert.ToDouble(tempRecipe.BeginMinForce) / 2);
                }
            }
        }
        
        private void ChartLimitUI()
        {
            
            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                string leftStep = "left" + Convert.ToString(i+1);
                string rightStep= "right" + Convert.ToString(i + 1);
                string topStep = "top" + Convert.ToString(i + 1);
                string bottomStep = "bottom" + Convert.ToString(i + 1);


                if (tempRecipe.limit.getLimitType(i) == "0")
                {

                }
                else if (tempRecipe.limit.getLimitType(i) == "1")
                {
                    chart1.Series[topStep].Color = Color.Red;
                    chart1.Series[bottomStep].Color = Color.Red;
                    chart1.Series[leftStep].Color = Color.Gray;
                    chart1.Series[rightStep].Color = Color.Gray;

                    chart1.Series[topStep].BorderWidth = 2;
                    chart1.Series[bottomStep].BorderWidth = 2;
                    chart1.Series[leftStep].BorderWidth = 1;
                    chart1.Series[rightStep].BorderWidth = 1;
                }
                else if (tempRecipe.limit.getLimitType(i) == "2")
                {
                    chart1.Series[topStep].Color = Color.Gray;
                    chart1.Series[bottomStep].Color = Color.Gray;
                    chart1.Series[leftStep].Color = Color.Red;
                    chart1.Series[rightStep].Color = Color.Red;

                    chart1.Series[topStep].BorderWidth = 1;
                    chart1.Series[bottomStep].BorderWidth = 1;
                    chart1.Series[leftStep].BorderWidth = 2;
                    chart1.Series[rightStep].BorderWidth = 2;
                }
                else if (tempRecipe.limit.getLimitType(i) == "3")
                {
                    chart1.Series[topStep].Color = Color.Red;
                    chart1.Series[bottomStep].Color = Color.Red;
                    chart1.Series[leftStep].Color = Color.Gray;
                    chart1.Series[rightStep].Color = Color.Gray;

                    chart1.Series[topStep].BorderWidth = 2;
                    chart1.Series[bottomStep].BorderWidth = 2;
                    chart1.Series[leftStep].BorderWidth = 1;
                    chart1.Series[rightStep].BorderWidth = 1;
                }
                else if (tempRecipe.limit.getLimitType(i) == "4")
                {
                    chart1.Series[topStep].Color = Color.Gray;
                    chart1.Series[bottomStep].Color = Color.Red;
                    chart1.Series[leftStep].Color = Color.Gray;
                    chart1.Series[rightStep].Color = Color.Red;

                    chart1.Series[topStep].BorderWidth = 1;
                    chart1.Series[bottomStep].BorderWidth = 2;
                    chart1.Series[leftStep].BorderWidth = 1;
                    chart1.Series[rightStep].BorderWidth = 2;
                }
                else if (tempRecipe.limit.getLimitType(i) == "5")
                {
                    chart1.Series[topStep].Color = Color.Red;
                    chart1.Series[bottomStep].Color = Color.Gray;
                    chart1.Series[leftStep].Color = Color.Gray;
                    chart1.Series[rightStep].Color = Color.Red;

                    chart1.Series[topStep].BorderWidth = 2;
                    chart1.Series[bottomStep].BorderWidth = 1;
                    chart1.Series[leftStep].BorderWidth = 1;
                    chart1.Series[rightStep].BorderWidth = 2;
                }
                else if (tempRecipe.limit.getLimitType(i) == "6")
                {
                    chart1.Series[topStep].Color = Color.Red;
                    chart1.Series[bottomStep].Color = Color.Gray;
                    chart1.Series[leftStep].Color = Color.Red;
                    chart1.Series[rightStep].Color = Color.Gray;

                    chart1.Series[topStep].BorderWidth = 2;
                    chart1.Series[bottomStep].BorderWidth = 1;
                    chart1.Series[leftStep].BorderWidth = 2;
                    chart1.Series[rightStep].BorderWidth = 1;
                }
                else if (tempRecipe.limit.getLimitType(i) == "7")
                {
                    chart1.Series[topStep].Color = Color.Red;
                    chart1.Series[bottomStep].Color = Color.Gray;
                    chart1.Series[leftStep].Color = Color.Red;
                    chart1.Series[rightStep].Color = Color.Red;

                    chart1.Series[topStep].BorderWidth = 2;
                    chart1.Series[bottomStep].BorderWidth = 1;
                    chart1.Series[leftStep].BorderWidth = 2;
                    chart1.Series[rightStep].BorderWidth = 2;
                }
                else if (tempRecipe.limit.getLimitType(i) == "8")
                {
                    chart1.Series[topStep].Color = Color.Red;
                    chart1.Series[bottomStep].Color = Color.Red;
                    chart1.Series[leftStep].Color = Color.Gray;
                    chart1.Series[rightStep].Color = Color.Red;

                    chart1.Series[topStep].BorderWidth = 2;
                    chart1.Series[bottomStep].BorderWidth = 2;
                    chart1.Series[leftStep].BorderWidth = 1;
                    chart1.Series[rightStep].BorderWidth = 2;
                }
            }
        }
        private void UnitUI()
        {
            if (infor.Unit == "kgf")
            {
                lblLiveForce.Text = rM.GetString("LiveForceU");
                lblMaxForceUnit.Text = rM.GetString("kgf");
                lblYUnit.Text = rM.GetString("kgf");
                lblPressForceUnit.Text = rM.GetString("kgf");
                lblBasicMaxForceUnit.Text = rM.GetString("kgf");
                lblBasicMinForceUnit.Text = rM.GetString("kgf");
                lblEndMinForceLimitUnit.Text = rM.GetString("kgf");
                lblEndMaxForceLimitUnit.Text = rM.GetString("kgf");
                lblBeginMinForceUnit.Text = rM.GetString("kgf");
                lblBeginMaxForceUnit.Text = rM.GetString("kgf");
            }
            else if (infor.Unit == "N")
            {
                lblLiveForce.Text = rM.GetString("LiveForceN");
                lblMaxForceUnit.Text = rM.GetString("N");
                lblYUnit.Text = rM.GetString("N");
                lblPressForceUnit.Text = rM.GetString("N");
                lblBasicMaxForceUnit.Text = rM.GetString("N");
                lblBasicMinForceUnit.Text = rM.GetString("N");
                lblEndMinForceLimitUnit.Text = rM.GetString("N");
                lblEndMaxForceLimitUnit.Text = rM.GetString("N");
                lblBeginMinForceUnit.Text = rM.GetString("N");
                lblBeginMaxForceUnit.Text = rM.GetString("N");
            }
            else if (infor.Unit == "lbf")
            {
                lblLiveForce.Text = rM.GetString("LiveForceL");
                lblMaxForceUnit.Text = rM.GetString("lbf");
                lblYUnit.Text = rM.GetString("lbf");
                lblPressForceUnit.Text = rM.GetString("lbf");
                lblBasicMaxForceUnit.Text = rM.GetString("lbf");
                lblBasicMinForceUnit.Text = rM.GetString("lbf");
                lblEndMinForceLimitUnit.Text = rM.GetString("lbf");
                lblEndMaxForceLimitUnit.Text = rM.GetString("lbf");
                lblBeginMinForceUnit.Text = rM.GetString("lbf");
                lblBeginMaxForceUnit.Text = rM.GetString("lbf");
            }
        }

        private void txtXMax_TextChanged(object sender, EventArgs e)
        {
            //chart.Lock();
            
            double temp;
            if (Double.TryParse(txtXMax.Text,out temp) == true && temp> chart.XMin)
            {
                chart.XMax = temp;
                chart.DrawLine();
            }
        }

        private void txtYMax_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtYMax.Text, out temp) == true && temp > chart.YMin)
            {
                chart.YMax = temp;
                chart.DrawLine();
            }
        }

        private void txtXmin_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtXmin.Text, out temp) == true && temp < chart.XMax)
            {
                chart.XMin = temp;
                chart.DrawLine();
            }
        }

        private void txtYmin_TextChanged(object sender, EventArgs e)
        {
            double temp;
            if (Double.TryParse(txtYmin.Text, out temp) == true && temp < chart.YMax)
            {
                chart.YMin = temp;
                chart.DrawLine();
            }
        }

        private void btnChartChange_Click(object sender, EventArgs e)
        {
            chart.Reset();
        }

        private void tmrIO_Tick(object sender, EventArgs e)
        {
            SignalModeUI();
            txtChartTitle.Text = Convert.ToString(tempRecipe.PressingForce);
        }

        private void cboWaitingSignal_1_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboOuputSignal_2_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboOuputSignal_3_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboOuputSignal_4_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboOuputSignal_1_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboWaitingSignal_2_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboWaitingSignal_3_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboWaitingSignal_4_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void cboIO_1_MouseDown(object sender, MouseEventArgs e)
        {
            if (cboIO_1.SelectedIndex > 3)
            {
                tmrIO.Enabled = false;
            }
            else tmrIO.Enabled = true;
        }

        private void cboIO_2_MouseDown(object sender, MouseEventArgs e)
        {
            if (cboIO_1.SelectedIndex > 3)
            {
                tmrIO.Enabled = false;
            }
            else tmrIO.Enabled = true;
        }

        private void cboIO_3_MouseDown(object sender, MouseEventArgs e)
        {
            if (cboIO_1.SelectedIndex > 3)
            {
                tmrIO.Enabled = false;
            }
            else tmrIO.Enabled = true;
        }

        private void cboIO_4_MouseDown(object sender, MouseEventArgs e)
        {
            if (cboIO_1.SelectedIndex > 3)
            {
                tmrIO.Enabled = false;
            }
            else tmrIO.Enabled = true;
        }

        private void txtIODelay_1_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void txtIODelay_2_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void txtIODelay_3_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void txtIODelay_4_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIO.Enabled = false;
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(1);
            rbStep1.Checked = true;
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(2);
            rbStep2.Checked = true;
        }

        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(3);
            rbStep3.Checked = true;
        }

        private void rb4_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(4);
            rbStep4.Checked = true;
        }

        private void rb5_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(5);
            rbStep5.Checked = true;
        }

        private void rb6_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(6);
            rbStep6.Checked = true;
        }

        private void rb7_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(7);
            rbStep7.Checked = true;
        }

        private void rb8_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(8);
            rbStep8.Checked = true;
        }

        private void rb9_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(9);
            rbStep9.Checked = true;
        }

        private void rb10_CheckedChanged(object sender, EventArgs e)
        {
            tempRecipe.setStep(10);
            rbStep10.Checked = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tmrSlowUI_Tick(object sender, EventArgs e)
        {
            ManualUI();
            ChartScaleUI();

            BasicLimitUI();
            GeometryLimitStepUI();
            GeometryLimitTypeUI();
            GeometryLimitSettingUI();

            ChartLimitUI();
            UnitUI();
        }

        private void rbNone_Click(object sender, EventArgs e)
        {
            
            if (rbNone.Checked == true)
            {
                tempRecipe.CleanLimitValue();
            }
        }

        ToolTip tt = null;
        Point tl = Point.Empty;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (tt == null) tt = new ToolTip();

            ChartArea ca = chart1.ChartAreas[0];

            if (InnerPlotPositionClientRectangle(chart1, ca).Contains(e.Location))
            {

                Axis ax = ca.AxisX;
                Axis ay = ca.AxisY;
                double x = ax.PixelPositionToValue(e.X);
                double y = ay.PixelPositionToValue(e.Y);
                if (e.Location != tl)
                    tt.SetToolTip(chart1, string.Format("X={0:0.###} ; Y={1:0.###}", x, y));
                tl = e.Location;
            }
            else tt.Hide(chart1);
        }

        RectangleF InnerPlotPositionClientRectangle(System.Windows.Forms.DataVisualization.Charting.Chart chart, ChartArea CA)
        {
            RectangleF IPP = CA.InnerPlotPosition.ToRectangleF();
            RectangleF CArp = ChartAreaClientRectangle(chart, CA);

            float pw = CArp.Width / 100f;
            float ph = CArp.Height / 100f;

            return new RectangleF(CArp.X + pw * IPP.X, CArp.Y + ph * IPP.Y,
                                    pw * IPP.Width, ph * IPP.Height);
        }

        RectangleF ChartAreaClientRectangle(System.Windows.Forms.DataVisualization.Charting.Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }

        private void txtPressMode_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel36_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel29_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rbOriginPos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOriginPos.Checked == true)
            {
                rbStandbyPos.Checked = false;
                rbPressingPos.Checked = false;
                rbPosLimit.Checked = false;
                rbForceLimit.Checked = false;
            }
        }

        

        private void rbStandbyPos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStandbyPos.Checked == true)
            {
                rbOriginPos.Checked = false;
                rbPressingPos.Checked = false;
                rbPosLimit.Checked = false;
                rbForceLimit.Checked = false;
            }
        }

        private void rbPressingPos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPressingPos.Checked == true)
            {
                rbOriginPos.Checked = false;
                rbStandbyPos.Checked = false;
                rbPosLimit.Checked = false;
                rbForceLimit.Checked = false;
            }
        }

        private void tableLayoutPanel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rbDynamic_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbLR_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbTB_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbBR_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbTR_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbLT_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbLTR_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        private void rbTBR_Click(object sender, EventArgs e)
        {
            rbOriginPos.Checked = false;
            rbStandbyPos.Checked = false;
            rbPressingPos.Checked = false;
            rbPosLimit.Checked = false;
            rbForceLimit.Checked = false;

        }

        /*
        private void rbGeometry_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGeometry.Checked == true)
            {
                counter = 0;
                rbOriginPos.Checked = false;
                rbStandbyPos.Checked = false;
                rbPressingPos.Checked = false;
                rbPosLimit.Checked = false;
                rbForceLimit.Checked = false;

                tempRecipe.limit.LimitType = 1;
            }
        }*/
    }
}
