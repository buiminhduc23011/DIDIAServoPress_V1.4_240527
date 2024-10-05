using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using Graph = System.Windows.Forms.DataVisualization.Charting;
using System.Resources;

namespace DIAServoPress
{
    public class Others
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private PLC plc;
        private ServoPressInfor infor;
        private MotionManager motionManager;

        private Graph.Chart chart;

        private ListView lstMotion;

        public Others(PLC plc,ColumnHeader lstItemLowerPressLimit, ColumnHeader lstItemPressForce, ColumnHeader lstItemUpperPressLimit, ServoPressInfor infor, MotionManager motionManager, ListView lstMotion, CheckBox chkLock, Graph.Chart chart, Panel pnOriginal, Panel pnPrepare, Panel pnPress, ComboBox cboX, ComboBox cboY) 
        {
            this.plc = plc;

            this.chart= chart;

            this.lstMotion = lstMotion;
            this.motionManager = motionManager;
            this.infor = infor;

        }
       


        public void Clean() 
        {
            lstMotion.Items.Clear();
            /*
            string data = "";
            
            for (int i = 0; i <= 31; i++)
            {
                data = "data" + Convert.ToString(i);
                chart.Series[data].Points.Clear();
            }
            chart.Series["data"].Points.Clear();
            chart.Series["UpperLimit"].Points.Clear();
            chart.Series["LowerLimit"].Points.Clear();
            */
        }

        public void SlowRun() 
        {
            if (plc.RecipeChanged==1)
            {
                lstMotion.Items.Clear();
                for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    motionManager.MotionData_Read_From_PLC(i);  //從PLC讀取各步序參數對應暫存器
                }
                motionManager.Build_Motion_List(lstMotion);     //建立步序參數於ListView
                plc.Write("RecipeChanged", 0);
                plc.RecipeChanged = 0;
                plc.RecipeChangedSave = 1;
            }
        }

        public void ListView_Subitem_Update(MotionManager motionManager)   //Update the motion data on the listview   從Motion物件，更新Listview資料
        {

            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                lstMotion.Items[i].SubItems[0].Text = Convert.ToString(i + 1);
                lstMotion.Items[i].SubItems[1].Text = motionManager.getModeName(motionManager.MotionData[i].Mode);
                lstMotion.Items[i].SubItems[2].Text = motionManager.MotionData[i].OriginalVel;
                lstMotion.Items[i].SubItems[3].Text = motionManager.MotionData[i].OriginalPos;
                lstMotion.Items[i].SubItems[4].Text = motionManager.MotionData[i].StandbyVel;
                lstMotion.Items[i].SubItems[5].Text = motionManager.MotionData[i].StandbyPos;
                lstMotion.Items[i].SubItems[6].Text = motionManager.MotionData[i].StandbyTime;

                if (motionManager.MotionData[i].Mode == 6)
                {
                    lstMotion.Items[i].SubItems[7].Text = "-";
                    lstMotion.Items[i].SubItems[8].Text = "-";
                    lstMotion.Items[i].SubItems[9].Text = "-";
                    lstMotion.Items[i].SubItems[10].Text = "-";
                }
                else
                {
                    lstMotion.Items[i].SubItems[7].Text = motionManager.MotionData[i].PressVel;
                    lstMotion.Items[i].SubItems[8].Text = motionManager.MotionData[i].PressPos;

                    if (plc.Unit == 0) 
                    {
                        lstMotion.Items[i].SubItems[9].Text = motionManager.MotionData[i].PressForce;
                    }
                    else if (plc.Unit == 1)
                    {
                        bool j = Double.TryParse(motionManager.MotionData[i].PressForce, out double result);
                        if (j == true)
                        {
                            lstMotion.Items[i].SubItems[9].Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[i].PressForce) * 10);
                        }
                        else
                        {
                            lstMotion.Items[i].SubItems[9].Text =motionManager.MotionData[i].PressForce;
                        }
                    }
                    else if (plc.Unit == 2)
                    {
                        bool j = Double.TryParse(motionManager.MotionData[i].PressForce, out double result);
                        if (j == true)
                        {
                            lstMotion.Items[i].SubItems[9].Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[i].PressForce) /2);
                        }
                        else
                        {
                            lstMotion.Items[i].SubItems[9].Text = motionManager.MotionData[i].PressForce;
                        }
                    }

                    
                    lstMotion.Items[i].SubItems[10].Text = motionManager.MotionData[i].PressTime;
                }

                lstMotion.Items[i].SubItems[11].Text = motionManager.MotionData[i].EndMaxPos;
                lstMotion.Items[i].SubItems[12].Text = motionManager.MotionData[i].EndMinPos;

                if (plc.Unit == 0)
                {
                    lstMotion.Items[i].SubItems[13].Text = motionManager.MotionData[i].EndMaxForce;
                    lstMotion.Items[i].SubItems[14].Text = motionManager.MotionData[i].EndMinForce;
                }
                else if (plc.Unit == 1)
                {

                    bool j = Double.TryParse(motionManager.MotionData[i].EndMaxForce, out double result);
                    if (j == true)
                    {
                        lstMotion.Items[i].SubItems[13].Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[i].EndMaxForce) * 10);
                        lstMotion.Items[i].SubItems[14].Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[i].EndMinForce) * 10);
                    }
                    else
                    {
                        lstMotion.Items[i].SubItems[13].Text = motionManager.MotionData[i].EndMaxForce;
                        lstMotion.Items[i].SubItems[14].Text = motionManager.MotionData[i].EndMinForce;
                    }
                }
                else if (plc.Unit == 2)
                {
                    bool j = Double.TryParse(motionManager.MotionData[i].EndMaxForce, out double result);
                    if (j == true)
                    {
                        lstMotion.Items[i].SubItems[13].Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[i].EndMaxForce) / 2);
                        lstMotion.Items[i].SubItems[14].Text = Convert.ToString(Convert.ToDouble(motionManager.MotionData[i].EndMinForce) / 2);
                    }
                    else
                    {
                        lstMotion.Items[i].SubItems[13].Text = motionManager.MotionData[i].EndMaxForce;
                        lstMotion.Items[i].SubItems[14].Text = motionManager.MotionData[i].EndMinForce;
                    }
                }
            }

            
        }
        
    }
}
