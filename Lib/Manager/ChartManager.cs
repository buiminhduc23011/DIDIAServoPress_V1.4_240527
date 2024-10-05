using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using Graph = System.Windows.Forms.DataVisualization.Charting;
using System.Resources;
using System.Windows.Forms;

namespace DIAServoPress
{
    public class ChartManager
    {
        public double XMax = 10;
        public double XMin = 0;
        public double YMax = 10;
        public double YMin = 0;

        public double LockXMax = 10;
        public double LockXMin = 0;
        public double LockYMax = 10;
        public double LockYMin = 0;

        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        protected PLC plc;
        private ServoPressInfor infor;
        private MotionManager motionManager;

        private Chart chart;
        private ComboBox cboX;
        private ComboBox cboY;

        private double drawX=0;
        private double drawY = 0;

        private string leftStep = "";
        private string rightStep = "";
        private string topStep = "";
        private string bottomStep = "";

        private bool frameLimit = false;
        //public int Keep = 0;
        //private int keepNum = 0;
        //private int firstPoint = 0;

        public ChartManager(PLC plc,Chart chart, ServoPressInfor infor, MotionManager motionManager, ComboBox cboX, ComboBox cboY) 
        {
            this.plc = plc;
            this.chart = chart;
            this.infor = infor;
            this.motionManager = motionManager;

            this.cboX = cboX;
            this.cboY = cboY;
        }

        public void Initialize_Chart()   //Build the initial information of chart denpend on the type of unit chosen  圖表初始化
        {
            chart.Series["Temp"].Points.Clear();
            /*
            string data = "";
            for (int i = 0; i <= 31; i++)
            {
                data = "data" + Convert.ToString(i);
                chart.Series[data].Points.Clear();
            }*/
            chart.Series["data0"].Points.Clear();
            chart.Series["UpperLimit"].Points.Clear();
            chart.Series["LowerLimit"].Points.Clear();

            chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
            chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;

            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
            chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;

            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].CursorX.SelectionColor = Color.LawnGreen;
            chart.ChartAreas[0].CursorX.LineColor = Color.LawnGreen;

            chart.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].CursorY.SelectionColor = Color.LawnGreen;
            chart.ChartAreas[0].CursorY.LineColor = Color.LawnGreen;

            XMax = Math.Floor(Convert.ToDouble(infor.PositionMax));
            XMin = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos));
            

            YMax = Math.Floor(Convert.ToDouble(infor.ForceMax / 10));
            YMin = 0;

            cboY.Items.Clear();
            cboY.Items.Add(rM.GetString("Force"));
            cboY.Items.Add(rM.GetString("Vel"));
            cboY.Items.Add(rM.GetString("Pos"));
            cboY.SelectedIndex = 0;

            cboX.Items.Clear();
            cboX.Items.Add(rM.GetString("Pos"));
            cboX.Items.Add(rM.GetString("Time"));
            cboX.SelectedIndex = 0;

            
        }



        public void Live_Chart() 
        {
                if (cboY.SelectedIndex == 0)
                {
                    if (cboX.SelectedIndex == 0)
                    {
                        drawX = plc.LivePos;
                        drawY = plc.LiveForce;

                        /*
                        if ((firstPoint == 0) && (drawY!=0))
                        {
                            chart.Series["Temp"].Points.AddXY(drawX, 0);
                            firstPoint = 1;
                        }*/
                    }

                    else if (cboX.SelectedIndex == 1)
                    {
                        drawX = plc.LiveProductTime;
                        drawY = plc.LiveForce;
                    }
                }
                else if (cboY.SelectedIndex == 1)
                {
                    if (cboX.SelectedIndex == 0)
                    {
                        drawX = plc.LivePos;
                        drawY = plc.LiveVel;
                    }
                    else if (cboX.SelectedIndex == 1)
                    {
                        drawX = plc.LiveProductTime;
                        drawY = plc.LiveVel;
                    }
                }

                else if (cboY.SelectedIndex == 2)
                {
                    if (cboX.SelectedIndex == 0)
                    {
                        drawX = plc.LivePos;
                        drawY = plc.LivePos;
                    }
                    else if (cboX.SelectedIndex == 1)
                    {
                        drawX = plc.LiveProductTime;
                        drawY = plc.LivePos;
                    }
                }

                //if ((drawX != 0) && (drawY != 0) && (firstPoint == 1))
                if (drawX>0 && plc.LiveVel>0)
                {
                    chart.Series["Temp"].Points.AddXY(drawX, drawY);
                    adjScaleX(drawX);
                    adjScaleY(drawY);
                }
        }

        
        private int lastPoint ;
        private int lastTotalAmount;

        public void Live_Chart(USB usb)
        {
            if (usb.TotalAmount != lastTotalAmount)
            {
                lastTotalAmount = usb.TotalAmount;
                int i = lastPoint;
                while (i < usb.TotalAmount)
                {
                    if (cboY.SelectedIndex == 0)
                    {
                        if (cboX.SelectedIndex == 0)
                        {
                            drawX = usb.ChartPosition[i];
                            drawY = usb.ChartForce[i];
                        }

                        else if (cboX.SelectedIndex == 1)
                        {
                            drawX = usb.ChartTime[i];
                            drawY = usb.ChartForce[i];
                        }
                    }
                    else if (cboY.SelectedIndex == 1)
                    {
                        if (cboX.SelectedIndex == 0)
                        {
                            drawX = usb.ChartPosition[i];
                            drawY = usb.ChartVelocity[i];
                        }
                        else if (cboX.SelectedIndex == 1)
                        {
                            drawX = usb.ChartTime[i];
                            drawY = usb.ChartVelocity[i];
                        }
                    }

                    else if (cboY.SelectedIndex == 2)
                    {
                        if (cboX.SelectedIndex == 0)
                        {
                            drawX = usb.ChartPosition[i];
                            drawY = usb.ChartPosition[i];
                        }
                        else if (cboX.SelectedIndex == 1)
                        {
                            drawX = usb.ChartTime[i];
                            drawY = usb.ChartPosition[i];
                        }
                    }

                    //if (drawY != 0)
                    if (drawX>0 && usb.ChartVelocity[i]>0)
                    {
                        chart.Series["Temp"].Points.AddXY(drawX, drawY);
                        adjScaleX(drawX);
                        adjScaleY(drawY);
                    }
                    lastPoint = usb.TotalAmount;
                    i++;
                }

            }
        }

        public void Pressing_Start(bool keep)
        {
            double _XMax = 10;
            double _XMin = 0;
            double _YMax = 10;
            double _YMin = 0;

            cboX.Enabled = false;
            cboY.Enabled = false;

            chart.Series["Temp"].Points.Clear();
            //Keep = 1;
            //firstPoint = 0;
            /*
            if (keep == true)
            {
                Keep = 1;
               
            }
            else 
            {
                Keep = 0;
                for (int i = 0; i <= 31; i++)
                {
                    chart.Series["data" + Convert.ToString(i)].Points.Clear();
                }
            }*/
            chart.Series["data0"].Points.Clear();
            chart.Series["UpperLimit"].Points.Clear();
            chart.Series["LowerLimit"].Points.Clear();

            if (cboY.SelectedIndex == 0)//壓力
            {
                if (cboX.SelectedIndex == 0)//位置
                {
                    chartAddXY(Double.Parse(motionManager.MotionData[0].OriginalPos), 0);
                    
                    _XMin = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos));
                    _YMin = 0;
                    _XMax = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos) + 10);
                    _YMax = Math.Floor(Convert.ToDouble(infor.ForceMax / 10));
                }
                else if (cboX.SelectedIndex == 1) //時間
                {
                    chartAddXY(0, 0);

                    _XMin = 0;
                    _YMin = 0;
                    _XMax = Math.Floor(Convert.ToDouble(Properties.Settings.Default.TimeMax / 10));
                    _YMax = Math.Floor(Convert.ToDouble(infor.ForceMax / 10));
                }
            }
            else if (cboY.SelectedIndex == 1)//速度
            {
                if (cboX.SelectedIndex == 0)//位置
                {
                    chartAddXY(Double.Parse(motionManager.MotionData[0].OriginalPos), 0);
                    _XMin = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos));
                    _YMin = 0;
                    _XMax = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos) + 10);
                    _YMax = Math.Floor(Convert.ToDouble(Properties.Settings.Default.VelocityMax / 10));
                }
                else if (cboX.SelectedIndex == 1) //時間
                {
                    chartAddXY(0, 0);
                    _XMin = 0;
                    _YMin = 0;
                    _XMax = Math.Floor(Convert.ToDouble(Properties.Settings.Default.TimeMax / 10));
                    _YMax = Math.Floor(Convert.ToDouble(Properties.Settings.Default.VelocityMax / 10));
                }
            }
            else if (cboY.SelectedIndex == 2)//位置
            {
                if (cboX.SelectedIndex == 0)//位置
                {
                    chartAddXY(Double.Parse(motionManager.MotionData[0].OriginalPos), Double.Parse(motionManager.MotionData[0].OriginalPos));
                    _XMin = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos));
                    _YMin = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos));
                    _XMax = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos) + 10);
                    _YMax = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos) + 10);
                }
                else if (cboX.SelectedIndex == 1) //時間
                {
                    //chart.Series["data"].Points.AddXY(0, Double.Parse(motionManager.MotionData[0].OriginalPos));
                    chartAddXY(0, Double.Parse(motionManager.MotionData[0].OriginalPos));

                    _XMin = 0;
                    _YMin = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos));
                    _XMax = Math.Floor(Convert.ToDouble(Properties.Settings.Default.TimeMax / 10));
                    _YMax = Math.Floor(Convert.ToDouble(motionManager.MotionData[0].OriginalPos) + 10);
                }
            }
            if (frameLimit == false)
            {
                XMin = _XMin;
                YMin = _YMin;
                XMax = _XMax;
                YMax = _YMax;
            }

            lastPoint = 0;
            lastTotalAmount = 0;
        }

        public void Pressing_End()
        {
            cboX.Enabled = true;
            cboY.Enabled = true;
        }

        public void BufferChart(LiveDataManager liveDataManager) 
        {
            chart.Series["Temp"].Points.Clear();
            /*
            if (Keep == 0 )
            {
                for (int i = 0; i <= 31; i++)
                {
                    chart.Series["data" + Convert.ToString(i)].Points.Clear();
                }
            }*/
            chart.Series["data0"].Points.Clear();
            chart.Series["UpperLimit"].Points.Clear();
            chart.Series["LowerLimit"].Points.Clear();

            if (liveDataManager.LiveDataNum >= 1)
            {
                read_Temp(liveDataManager);  //讀temp檔及繪圖
            }

            if (chart.Series["data0"].Points.Count == 0)
            {
                chart.Series["data0"].Points.AddXY(XMin, YMin);
            }
            /*
            if (chart.Series["data" + Convert.ToString(keepNum)].Points.Count == 0)
            {
                chart.Series["data" + Convert.ToString(keepNum)].Points.AddXY(XMin, YMin);
            }*/
        
        }

        private void read_Temp(LiveDataManager liveDataManager)    //Read .csv for chart drawing   讀temp檔及繪圖
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(infor.TempPath + "\\temp_" + (liveDataManager.LiveDataNum - 1) + ".txt", Encoding.Default);
            String strResult = "";
            decimal i = 0;
            
            while ((strResult = sr.ReadLine()) != null) 
            {
                string[] sCoordinate = strResult.Split(',');
                if ((decimal.TryParse(sCoordinate[0], out i) == true))
                {
                    if (cboY.SelectedIndex == 0)  //壓力
                    {
                        if (cboX.SelectedIndex == 0)  //位置
                        {
                            //chart.Series["data"].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[1]));
                            chartAddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[1]));
                            //adjScaleXY(Double.Parse(sCoordinate[0]),Double.Parse(sCoordinate[1]));
                            adjScaleX(Double.Parse(sCoordinate[0]));
                            adjScaleY(Double.Parse(sCoordinate[1]));
                        }
                        else if (cboX.SelectedIndex == 1)  //速度
                        {
                            //chart.Series["data"].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[1]));
                            chartAddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[1]));
                            //adjScaleXY(Double.Parse(sCoordinate[3]),Double.Parse(sCoordinate[1]));
                            adjScaleX(Double.Parse(sCoordinate[3]));
                            adjScaleY(Double.Parse(sCoordinate[1]));
                        }
                    }
                    else if (cboY.SelectedIndex == 1)   //速度
                    {
                        if (cboX.SelectedIndex == 0)   //位置
                        {
                            //chart.Series["data"].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[2]));
                            chartAddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[2]));
                            //adjScaleXY(Double.Parse(sCoordinate[0]),Double.Parse(sCoordinate[2]));
                            adjScaleX(Double.Parse(sCoordinate[0]));
                            adjScaleY(Double.Parse(sCoordinate[2]));
                        }
                        else if (cboX.SelectedIndex == 1)    //速度
                        {
                            //chart.Series["data"].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[2]));
                            chartAddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[2]));
                            //adjScaleXY(Double.Parse(sCoordinate[3]),Double.Parse(sCoordinate[2]));
                            adjScaleX(Double.Parse(sCoordinate[3]));
                            adjScaleY(Double.Parse(sCoordinate[2]));
                        }
                    }
                    else if (cboY.SelectedIndex == 2)   //位置
                    {
                        if (cboX.SelectedIndex == 0)   //位置
                        {
                            //chart.Series["data"].Points.AddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[0]));
                            chartAddXY(Double.Parse(sCoordinate[0]), Double.Parse(sCoordinate[0]));
                            //adjScaleXY(Double.Parse(sCoordinate[0]),Double.Parse(sCoordinate[0]));
                            adjScaleX(Double.Parse(sCoordinate[0]));
                            adjScaleY(Double.Parse(sCoordinate[0]));
                        }
                        else if (cboX.SelectedIndex == 1)    //速度
                        {
                            //chart.Series["data"].Points.AddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[0]));
                            chartAddXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[0]));
                            //adjScaleXY(Double.Parse(sCoordinate[3]), Double.Parse(sCoordinate[0]));
                            adjScaleX(Double.Parse(sCoordinate[3]));
                            adjScaleY(Double.Parse(sCoordinate[0]));
                        }
                    }
                }
            }
            
            /*
            if (keepNum == 3)
            {
                keepNum = 0;
            }
            else
            {
                keepNum++;
                
            }
            chart.Series["data" + Convert.ToString(keepNum)].Points.Clear();*/
            sr.Close();
        }

        
        private void chartAddXY(double x,double y) 
        {
            /*
            if (Keep == 0)
            {
                keepNum = 0;
            }
            chart.Series["data" + Convert.ToString(keepNum)].Points.AddXY(x, y);
            */
            chart.Series["data0"].Points.AddXY(x, y);
        }

        private void adjScaleX(double x)
        {
            if ((XMax < Math.Ceiling(x + 0.5 * (x - XMin))) && x > 0)
            {
                XMax = Math.Ceiling(x + 0.5 * (x - XMin));
            }
        }

        private void adjScaleY(double y)
        {
            if ((YMax < Math.Ceiling(y + 0.3 * (y - YMin))) && y > 0)
            {
                YMax = Math.Ceiling(y + 0.3 * (y - YMin));
            }
        }

        
        public void Lock()
        {
            LockXMin = XMin;
            LockXMax = XMax;
            LockYMin = YMin;
            LockYMax = YMax;
        }

        public void DrawFrame()
        {
            if (cboX.SelectedIndex != 0 && cboY.SelectedIndex != 0)
            {
                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    frameStep(i + 1);
                    cleanLimit();
                }
            }
            else if (cboX.SelectedIndex == 0 && cboY.SelectedIndex == 0)
            {
                frameLimit = false;
                int limitType = 0;
                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    frameStep(i + 1);
                    FrameColor(motionManager.MotionData[i].LimitType);
                    limitType = Convert.ToInt16(motionManager.MotionData[i].LimitType);
                    if (limitType == 0)
                    {
                        cleanLimit();
                    }
                    else if (limitType > 0)
                    {
                        double BeginMinForce = Convert.ToDouble(motionManager.MotionData[i].BeginMinForce);
                        double BeginMaxForce = Convert.ToDouble(motionManager.MotionData[i].BeginMaxForce);
                        double EndMinForce = Convert.ToDouble(motionManager.MotionData[i].EndMinForce);
                        double EndMaxForce = Convert.ToDouble(motionManager.MotionData[i].EndMaxForce);
                        
                        if (plc.Unit == 1)
                        {
                            BeginMinForce = BeginMinForce* 10;
                            BeginMaxForce = BeginMaxForce * 10;
                            EndMinForce = EndMinForce * 10;
                            EndMaxForce = EndMaxForce * 10;
                        }
                        else if (plc.Unit == 2)
                        {
                            BeginMinForce = BeginMinForce/2;
                            BeginMaxForce = BeginMaxForce/2;
                            EndMinForce = EndMinForce/2;
                            EndMaxForce = EndMaxForce/2;
                        }


                        if (limitType == 1)
                        {
                            drawSimpleDynamicLimit(Convert.ToDouble(motionManager.MotionData[i].EndMinPos), Convert.ToDouble(motionManager.MotionData[i].EndMaxPos), BeginMinForce, BeginMaxForce, EndMinForce, EndMaxForce);
                        }
                        else if (limitType > 1)
                        {
                            drawSimpleFrameLimit(Convert.ToDouble(motionManager.MotionData[i].EndMinPos), EndMinForce, Convert.ToDouble(motionManager.MotionData[i].EndMaxPos), EndMaxForce);
                        }
                        frameLimit = true;
                        adjScaleX(Convert.ToDouble(motionManager.MotionData[i].EndMaxPos));
                        adjScaleY(EndMaxForce);
                    }
                }
            }
        }

            private void frameStep(int step)
            {
                leftStep = "left" + Convert.ToString(step);
                rightStep = "right" + Convert.ToString(step);
                topStep = "top" + Convert.ToString(step);
                bottomStep = "bottom" + Convert.ToString(step);
            }

            public void cleanLimit()
            {
                chart.Series[topStep].Points.Clear();
                chart.Series[rightStep].Points.Clear();
                chart.Series[bottomStep].Points.Clear();
                chart.Series[leftStep].Points.Clear();
            }


        private void drawSimpleDynamicLimit(double beginValue, double endValue, double beginMinForce, double beginMaxForce, double endMinForce, double endMaxForce)
            {
                //
                chart.Series[topStep].Points.Clear();
                chart.Series[topStep].Points.AddXY(beginValue, beginMaxForce);
                chart.Series[topStep].Points.AddXY(endValue, endMaxForce);

                chart.Series[rightStep].Points.Clear();
                chart.Series[rightStep].Points.AddXY(endValue, endMaxForce);
                chart.Series[rightStep].Points.AddXY(endValue, endMinForce);

                chart.Series[bottomStep].Points.Clear();
                chart.Series[bottomStep].Points.AddXY(endValue, endMinForce);
                chart.Series[bottomStep].Points.AddXY(beginValue, beginMinForce);

                chart.Series[leftStep].Points.Clear();
                chart.Series[leftStep].Points.AddXY(beginValue, beginMinForce);
                chart.Series[leftStep].Points.AddXY(beginValue, beginMaxForce);
                //
            }

            private void drawSimpleFrameLimit(double X1, double Y1, double X2, double Y2)
            {
                chart.Series[leftStep].Points.Clear();
                chart.Series[leftStep].Points.AddXY(X1, Y1);
                chart.Series[leftStep].Points.AddXY(X1, Y2);

                chart.Series[rightStep].Points.Clear();
                chart.Series[rightStep].Points.AddXY(X2, Y1);
                chart.Series[rightStep].Points.AddXY(X2, Y2);

                chart.Series[topStep].Points.Clear();
                chart.Series[topStep].Points.AddXY(X1, Y2);
                chart.Series[topStep].Points.AddXY(X2, Y2);

                chart.Series[bottomStep].Points.Clear();
                chart.Series[bottomStep].Points.AddXY(X1, Y1);
                chart.Series[bottomStep].Points.AddXY(X2, Y1);

            }
        private void FrameColor(string num)
        {
            if (num == "0")
            {
                chart.Series[topStep].Color = Color.Gray;
                chart.Series[bottomStep].Color = Color.Gray;
                chart.Series[leftStep].Color = Color.Gray;
                chart.Series[rightStep].Color = Color.Gray;

                chart.Series[topStep].BorderWidth = 1;
                chart.Series[bottomStep].BorderWidth = 1;
                chart.Series[leftStep].BorderWidth = 1;
                chart.Series[rightStep].BorderWidth = 1;
            }

            if (num == "1")
            {
                chart.Series[topStep].Color = Color.Red;
                chart.Series[bottomStep].Color = Color.Red;
                chart.Series[leftStep].Color = Color.Gray;
                chart.Series[rightStep].Color = Color.Gray;

                chart.Series[topStep].BorderWidth = 2;
                chart.Series[bottomStep].BorderWidth = 2;
                chart.Series[leftStep].BorderWidth = 1;
                chart.Series[rightStep].BorderWidth = 1;
            }
            if (num == "2")
            {
                chart.Series[topStep].Color = Color.Gray;
                chart.Series[bottomStep].Color = Color.Gray;
                chart.Series[leftStep].Color = Color.Red;
                chart.Series[rightStep].Color = Color.Red;

                chart.Series[topStep].BorderWidth = 1;
                chart.Series[bottomStep].BorderWidth = 1;
                chart.Series[leftStep].BorderWidth = 2;
                chart.Series[rightStep].BorderWidth = 2;
            }
            if (num == "3")
            {
                chart.Series[topStep].Color = Color.Red;
                chart.Series[bottomStep].Color = Color.Red;
                chart.Series[leftStep].Color = Color.Gray;
                chart.Series[rightStep].Color = Color.Gray;

                chart.Series[topStep].BorderWidth = 2;
                chart.Series[bottomStep].BorderWidth = 2;
                chart.Series[leftStep].BorderWidth = 1;
                chart.Series[rightStep].BorderWidth = 1;
            }
            if (num == "4")
            {
                chart.Series[topStep].Color = Color.Gray;
                chart.Series[bottomStep].Color = Color.Red;
                chart.Series[leftStep].Color = Color.Gray;
                chart.Series[rightStep].Color = Color.Red;

                chart.Series[topStep].BorderWidth = 1;
                chart.Series[bottomStep].BorderWidth = 2;
                chart.Series[leftStep].BorderWidth = 1;
                chart.Series[rightStep].BorderWidth = 2;
            }
            if (num == "5")
            {
                chart.Series[topStep].Color = Color.Red;
                chart.Series[bottomStep].Color = Color.Gray;
                chart.Series[leftStep].Color = Color.Gray;
                chart.Series[rightStep].Color = Color.Red;

                chart.Series[topStep].BorderWidth = 2;
                chart.Series[bottomStep].BorderWidth = 1;
                chart.Series[leftStep].BorderWidth = 1;
                chart.Series[rightStep].BorderWidth = 2;
            }
            if (num == "6")
            {
                chart.Series[topStep].Color = Color.Red;
                chart.Series[bottomStep].Color = Color.Gray;
                chart.Series[leftStep].Color = Color.Red;
                chart.Series[rightStep].Color = Color.Gray;

                chart.Series[topStep].BorderWidth = 2;
                chart.Series[bottomStep].BorderWidth = 1;
                chart.Series[leftStep].BorderWidth = 2;
                chart.Series[rightStep].BorderWidth = 1;
            }
            if (num == "7")
            {
                chart.Series[topStep].Color = Color.Red;
                chart.Series[bottomStep].Color = Color.Gray;
                chart.Series[leftStep].Color = Color.Red;
                chart.Series[rightStep].Color = Color.Red;

                chart.Series[topStep].BorderWidth = 2;
                chart.Series[bottomStep].BorderWidth = 1;
                chart.Series[leftStep].BorderWidth = 2;
                chart.Series[rightStep].BorderWidth = 2;
            }
            if (num == "8")
            {
                chart.Series[topStep].Color = Color.Red;
                chart.Series[bottomStep].Color = Color.Red;
                chart.Series[leftStep].Color = Color.Gray;
                chart.Series[rightStep].Color = Color.Red;

                chart.Series[topStep].BorderWidth = 2;
                chart.Series[bottomStep].BorderWidth = 2;
                chart.Series[leftStep].BorderWidth = 1;
                chart.Series[rightStep].BorderWidth = 2;
            }
        }

    }
}
