using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Resources;

namespace DIAServoPress
{
    public class MotionManager
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private PLC plc;
        private ServoPressInfor infor;
        
        public MotionData[] MotionData = new MotionData[CONSTANT.StepAmount];  // Record setting of five step motions from user 單一步序參數紀錄
        //public int MotionSelect;

        public MotionManager(PLC plc,ServoPressInfor infor) 
        {
            this.plc = plc;
            this.infor = infor;
        }

        public void Build_MotionData()    //Build the five motions data by reading the PLC  建立各步序參數物件
        {
            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                MotionData[i] = new MotionData();
                MotionData_Read_From_PLC(i);  //從PLC讀取各步序參數對應暫存器
            }
        }

        public void Build_Motion_List(ListView lstMotion)   //Build motion list on the listview   建立步序參數於ListView
        {
            int motionCreate = 0;
            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Convert.ToString((motionCreate + 1));
                
                lvi.SubItems.Add(getModeName(MotionData[i].Mode)); //"動作模式"
                lvi.SubItems.Add(Convert.ToString(MotionData[i].OriginalVel));

                if (motionCreate == 0)
                {
                    lvi.SubItems.Add(MotionData[motionCreate].OriginalPos);
                }
                else
                {
                    lvi.SubItems.Add("-");
                }
                lvi.SubItems.Add(Convert.ToString(MotionData[i].StandbyVel));
                lvi.SubItems.Add(Convert.ToString(MotionData[i].StandbyPos));
                lvi.SubItems.Add(Convert.ToString(MotionData[i].StandbyTime));

                if (MotionData[motionCreate].Mode == 6)
                {
                    lvi.SubItems.Add("-");
                    lvi.SubItems.Add("-");
                    lvi.SubItems.Add("-");
                    lvi.SubItems.Add("-");
                }
                else
                {
                    lvi.SubItems.Add(Convert.ToString(MotionData[i].PressVel));
                    lvi.SubItems.Add(Convert.ToString(MotionData[i].PressPos));

                    if (infor.Unit == "kgf")
                    {
                        lvi.SubItems.Add(MotionData[i].PressForce);
                    }
                    else if (infor.Unit == "N")
                    {
                        bool j = Double.TryParse(MotionData[i].PressForce, out double result);
                        if (j == true)
                        {
                            lvi.SubItems.Add(Convert.ToString(result * 10));
                        }
                        else
                        {
                            lvi.SubItems.Add(MotionData[i].PressForce);
                        }
                    }
                    else if (infor.Unit == "lbf")
                    {
                        bool j = Double.TryParse(MotionData[i].PressForce, out double result);
                        if (j == true)
                        {
                            lvi.SubItems.Add(Convert.ToString(result /2));
                        }
                        else
                        {
                            lvi.SubItems.Add(MotionData[i].PressForce);
                        }
                    }

                    lvi.SubItems.Add(Convert.ToString(MotionData[i].PressTime));
                }

                lvi.SubItems.Add(Convert.ToString(MotionData[i].EndMaxPos));
                lvi.SubItems.Add(Convert.ToString(MotionData[i].EndMinPos));

                if (infor.Unit == "kgf")
                {
                    lvi.SubItems.Add(MotionData[i].EndMaxForce);
                    lvi.SubItems.Add(MotionData[i].EndMinForce);
                }
                else if (infor.Unit == "N")
                {
                    bool j = Double.TryParse(MotionData[i].EndMaxForce, out double result);
                    if (j == true)
                    {
                        lvi.SubItems.Add(Convert.ToString(Convert.ToDouble(MotionData[i].EndMaxForce) * 10));
                        lvi.SubItems.Add(Convert.ToString(Convert.ToDouble(MotionData[i].EndMinForce) * 10));
                    }
                    else
                    {
                        lvi.SubItems.Add(MotionData[i].EndMaxForce);
                        lvi.SubItems.Add(MotionData[i].EndMinForce);
                    }
                }
                else if (infor.Unit == "lbf")
                {
                    bool j = Double.TryParse(MotionData[i].EndMaxForce, out double result);
                    if (j == true)
                    {
                        lvi.SubItems.Add(Convert.ToString(Convert.ToDouble(MotionData[i].EndMaxForce) /2));
                        lvi.SubItems.Add(Convert.ToString(Convert.ToDouble(MotionData[i].EndMinForce) /2));
                    }
                    else
                    {
                        lvi.SubItems.Add(MotionData[i].EndMaxForce);
                        lvi.SubItems.Add(MotionData[i].EndMinForce);
                    }
                }

                lstMotion.Items.Add(lvi);
                lvi.ImageIndex++;
                if (motionCreate != 0) { MotionData[motionCreate].OriginalPos = "-"; }
                motionCreate++;
            }
        }   
        
        public void MotionData_Read_From_PLC(int motionNum)
        {
            plc.ReadRecipe();
            
            if (motionNum == 0)
            {
                MotionData[motionNum].Mode = plc.Mode1;
                MotionData[motionNum].OriginalPos = Convert.ToString(plc.OriginPos);
                MotionData[motionNum].OriginalVel = Convert.ToString(plc.OriginVel);

                MotionData[motionNum].StandbyPos =  Convert.ToString(plc.StandbyPos); 
                MotionData[motionNum].StandbyVel =  Convert.ToString(plc.StandbyVel);
                MotionData[motionNum].StandbyTime = Convert.ToString(plc.StandbyTime);

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos1);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce1);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel1);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime1);

                
                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit1);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit1);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit1);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit1);
                
                MotionData[motionNum].LimitType =  Convert.ToString(plc.LimitType1);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit1);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit1);

                
            }
            else if (motionNum == 1)
            {
                MotionData[motionNum].Mode = plc.Mode2;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";
                
                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos2);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce2);
                MotionData[motionNum].PressVel = Convert.ToString( plc.PressVel2);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime2);

                
                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit2);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit2);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit2);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit2);

                MotionData[motionNum].LimitType =  Convert.ToString(plc.LimitType2);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit2);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit2);
                
            }
            else if (motionNum == 2)
            {
                MotionData[motionNum].Mode = plc.Mode3;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos3);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce3);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel3);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime3);

                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit3);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit3);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit3);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit3);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType3);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit3);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit3);
            }
            else if (motionNum == 3)
            {
                MotionData[motionNum].Mode = plc.Mode4;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos4);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce4);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel4);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime4);

                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit4);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit4);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit4);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit4);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType4);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit4);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit4);
            }
            else if (motionNum == 4)
            {
                MotionData[motionNum].Mode = plc.Mode5;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos5);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce5);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel5);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime5);

                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit5);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit5);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit5);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit5);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType5);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit5);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit5);
            }
            else if (motionNum == 5)
            {
                MotionData[motionNum].Mode = plc.Mode6;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos6);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce6);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel6);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime6);

                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit6);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit6);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit6);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit6);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType6);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit6);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit6);
            }
            else if (motionNum == 6)
            {
                MotionData[motionNum].Mode = plc.Mode7;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos7);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce7);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel7);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime7);
   
                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit7);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit7);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit7);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit7);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType7);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit7);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit7);
            }
            else if (motionNum == 7)
            {
                MotionData[motionNum].Mode = plc.Mode8;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos8);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce8);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel8);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime8);

                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit8);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit8);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit8);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit8);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType8);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit8);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit8);

            }
            else if (motionNum == 8)
            {
                MotionData[motionNum].Mode = plc.Mode9;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos9);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce9);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel9);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime9);

                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit9);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit9);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit9);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit9);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType9);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit9);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit9);

            }
            else if (motionNum == 9)
            {
                MotionData[motionNum].Mode = plc.Mode10;
                MotionData[motionNum].OriginalPos = "-";
                MotionData[motionNum].OriginalVel = "-";
                MotionData[motionNum].StandbyPos = "-";
                MotionData[motionNum].StandbyVel = "-";
                MotionData[motionNum].StandbyTime = "-";

                MotionData[motionNum].PressPos = Convert.ToString(plc.PressPos10);
                MotionData[motionNum].PressForce = Convert.ToString(plc.PressForce10);
                MotionData[motionNum].PressVel = Convert.ToString(plc.PressVel10);
                MotionData[motionNum].PressTime = Convert.ToString(plc.PressTime10);

                
                MotionData[motionNum].EndMinPos = Convert.ToString(plc.EndMinPosLimit10);
                MotionData[motionNum].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit10);

                MotionData[motionNum].EndMinForce = Convert.ToString(plc.EndMinForceLimit10);
                MotionData[motionNum].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit10);

                MotionData[motionNum].LimitType = Convert.ToString(plc.LimitType10);
                MotionData[motionNum].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit10);
                MotionData[motionNum].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit10);
                
            }


            hide_Item_By_Mode(motionNum);   //根據各模式遮蔽未使用參數

            if (MotionData[motionNum].Mode != 3 || (MotionData[motionNum].Cpk != "Force" && MotionData[motionNum].Cpk != "Position"))
            {
                cpk_Initialize(motionNum);       //根據各模式，Cpk紀錄值初始化設定
            }
        }   //Read the Motion Setting from PLC    從PLC讀取各步序參數對應暫存器

        public void MotionData_Read_From_PLC()
        {
            plc.ReadRecipe();

                    MotionData[0].Mode = plc.Mode1;
                    MotionData[0].OriginalPos = Convert.ToString(plc.OriginPos);
                    MotionData[0].OriginalVel = Convert.ToString(plc.OriginVel);
                    MotionData[0].StandbyPos = Convert.ToString(plc.StandbyPos);
                    MotionData[0].StandbyVel = Convert.ToString(plc.StandbyVel);
                    MotionData[0].StandbyTime = Convert.ToString(plc.StandbyTime);

                    MotionData[0].PressPos = Convert.ToString(plc.PressPos1);
                    MotionData[0].PressForce = Convert.ToString(plc.PressForce1);
                    MotionData[0].PressVel = Convert.ToString(plc.PressVel1);
                    MotionData[0].PressTime = Convert.ToString(plc.PressTime1);

                    MotionData[0].EndMinPos = Convert.ToString(plc.EndMinPosLimit1);
                    MotionData[0].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit1);

                    MotionData[0].EndMinForce = Convert.ToString(plc.EndMinForceLimit1);
                    MotionData[0].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit1);

                    MotionData[0].LimitType = Convert.ToString(plc.LimitType1);
                    MotionData[0].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit1);
                    MotionData[0].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit1);
               
                    MotionData[1].Mode = plc.Mode2;
                    MotionData[1].OriginalPos = "-";
                    MotionData[1].OriginalVel = "-";
                    MotionData[1].StandbyPos = "-";
                    MotionData[1].StandbyVel = "-";
                    MotionData[1].StandbyTime = "-";

                    MotionData[1].PressPos = Convert.ToString(plc.PressPos2);
                    MotionData[1].PressForce = Convert.ToString(plc.PressForce2);
                    MotionData[1].PressVel = Convert.ToString(plc.PressVel2);
                    MotionData[1].PressTime = Convert.ToString(plc.PressTime2);

                    MotionData[1].EndMinPos = Convert.ToString(plc.EndMinPosLimit2);
                    MotionData[1].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit2);

                    MotionData[1].EndMinForce = Convert.ToString(plc.EndMinForceLimit2);
                    MotionData[1].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit2);

                    MotionData[1].LimitType = Convert.ToString(plc.LimitType2);
                    MotionData[1].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit2);
                    MotionData[1].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit2);
                
                    MotionData[2].Mode = plc.Mode3;
                    MotionData[2].OriginalPos = "-";
                    MotionData[2].OriginalVel = "-";
                    MotionData[2].StandbyPos = "-";
                    MotionData[2].StandbyVel = "-";
                    MotionData[2].StandbyTime = "-";

                    MotionData[2].PressPos = Convert.ToString(plc.PressPos3);
                    MotionData[2].PressForce = Convert.ToString(plc.PressForce3);
                    MotionData[2].PressVel = Convert.ToString(plc.PressVel3);
                    MotionData[2].PressTime = Convert.ToString(plc.PressTime3);

                    MotionData[2].EndMinPos = Convert.ToString(plc.EndMinPosLimit3);
                    MotionData[2].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit3);

                    MotionData[2].EndMinForce = Convert.ToString(plc.EndMinForceLimit3);
                    MotionData[2].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit3);

                    MotionData[2].LimitType = Convert.ToString(plc.LimitType3);
                    MotionData[2].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit3);
                    MotionData[2].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit3);
                
                    MotionData[3].Mode = plc.Mode4;
                    MotionData[3].OriginalPos = "-";
                    MotionData[3].OriginalVel = "-";
                    MotionData[3].StandbyPos = "-";
                    MotionData[3].StandbyVel = "-";
                    MotionData[3].StandbyTime = "-";

                    MotionData[3].PressPos = Convert.ToString(plc.PressPos4);
                    MotionData[3].PressForce = Convert.ToString(plc.PressForce4);
                    MotionData[3].PressVel = Convert.ToString(plc.PressVel4);
                    MotionData[3].PressTime = Convert.ToString(plc.PressTime4);

                    MotionData[3].EndMinPos = Convert.ToString(plc.EndMinPosLimit4);
                    MotionData[3].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit4);

                    MotionData[3].EndMinForce = Convert.ToString(plc.EndMinForceLimit4);
                    MotionData[3].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit4);

                    MotionData[3].LimitType = Convert.ToString(plc.LimitType4);
                    MotionData[3].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit4);
                    MotionData[3].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit4);
                
                    MotionData[4].Mode = plc.Mode5;
                    MotionData[4].OriginalPos = "-";
                    MotionData[4].OriginalVel = "-";
                    MotionData[4].StandbyPos = "-";
                    MotionData[4].StandbyVel = "-";
                    MotionData[4].StandbyTime = "-";

                    MotionData[4].PressPos = Convert.ToString(plc.PressPos5);
                    MotionData[4].PressForce = Convert.ToString(plc.PressForce5);
                    MotionData[4].PressVel = Convert.ToString(plc.PressVel5);
                    MotionData[4].PressTime = Convert.ToString(plc.PressTime5);

                    MotionData[4].EndMinPos = Convert.ToString(plc.EndMinPosLimit5);
                    MotionData[4].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit5);

                    MotionData[4].EndMinForce = Convert.ToString(plc.EndMinForceLimit5);
                    MotionData[4].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit5);

                    MotionData[4].LimitType = Convert.ToString(plc.LimitType5);
                    MotionData[4].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit5);
                    MotionData[4].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit5);

                    MotionData[5].Mode = plc.Mode6;
                    MotionData[5].OriginalPos = "-";
                    MotionData[5].OriginalVel = "-";
                    MotionData[5].StandbyPos = "-";
                    MotionData[5].StandbyVel = "-";
                    MotionData[5].StandbyTime = "-";

                    MotionData[5].PressPos = Convert.ToString(plc.PressPos6);
                    MotionData[5].PressForce = Convert.ToString(plc.PressForce6);
                    MotionData[5].PressVel = Convert.ToString(plc.PressVel6);
                    MotionData[5].PressTime = Convert.ToString(plc.PressTime6);

                    MotionData[5].EndMinPos = Convert.ToString(plc.EndMinPosLimit6);
                    MotionData[5].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit6);

                    MotionData[5].EndMinForce = Convert.ToString(plc.EndMinForceLimit6);
                    MotionData[5].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit6);

                    MotionData[5].LimitType = Convert.ToString(plc.LimitType6);
                    MotionData[5].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit6);
                    MotionData[5].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit6);

                    MotionData[6].Mode = plc.Mode7;
                    MotionData[6].OriginalPos = "-";
                    MotionData[6].OriginalVel = "-";
                    MotionData[6].StandbyPos = "-";
                    MotionData[6].StandbyVel = "-";
                    MotionData[6].StandbyTime = "-";

                    MotionData[6].PressPos = Convert.ToString(plc.PressPos7);
                    MotionData[6].PressForce = Convert.ToString(plc.PressForce7);
                    MotionData[6].PressVel = Convert.ToString(plc.PressVel7);
                    MotionData[6].PressTime = Convert.ToString(plc.PressTime7);

                    MotionData[6].EndMinPos = Convert.ToString(plc.EndMinPosLimit7);
                    MotionData[6].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit7);

                    MotionData[6].EndMinForce = Convert.ToString(plc.EndMinForceLimit7);
                    MotionData[6].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit7);

                    MotionData[6].LimitType = Convert.ToString(plc.LimitType7);
                    MotionData[6].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit7);
                    MotionData[6].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit7);

                    MotionData[7].Mode = plc.Mode8;
                    MotionData[7].OriginalPos = "-";
                    MotionData[7].OriginalVel = "-";
                    MotionData[7].StandbyPos = "-";
                    MotionData[7].StandbyVel = "-";
                    MotionData[7].StandbyTime = "-";

                    MotionData[7].PressPos = Convert.ToString(plc.PressPos8);
                    MotionData[7].PressForce = Convert.ToString(plc.PressForce8);
                    MotionData[7].PressVel = Convert.ToString(plc.PressVel8);
                    MotionData[7].PressTime = Convert.ToString(plc.PressTime8);

                    MotionData[7].EndMinPos = Convert.ToString(plc.EndMinPosLimit8);
                    MotionData[7].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit8);

                    MotionData[7].EndMinForce = Convert.ToString(plc.EndMinForceLimit8);
                    MotionData[7].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit8);

                    MotionData[7].LimitType = Convert.ToString(plc.LimitType8);
                    MotionData[7].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit8);
                    MotionData[7].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit8);

                    MotionData[8].Mode = plc.Mode9;
                    MotionData[8].OriginalPos = "-";
                    MotionData[8].OriginalVel = "-";
                    MotionData[8].StandbyPos = "-";
                    MotionData[8].StandbyVel = "-";
                    MotionData[8].StandbyTime = "-";

                    MotionData[8].PressPos = Convert.ToString(plc.PressPos9);
                    MotionData[8].PressForce = Convert.ToString(plc.PressForce9);
                    MotionData[8].PressVel = Convert.ToString(plc.PressVel9);
                    MotionData[8].PressTime = Convert.ToString(plc.PressTime9);

                    MotionData[8].EndMinPos = Convert.ToString(plc.EndMinPosLimit9);
                    MotionData[8].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit9);

                    MotionData[8].EndMinForce = Convert.ToString(plc.EndMinForceLimit9);
                    MotionData[8].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit9);

                    MotionData[8].LimitType = Convert.ToString(plc.LimitType9);
                    MotionData[8].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit9);
                    MotionData[8].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit9);

                    MotionData[9].Mode = plc.Mode10;
                    MotionData[9].OriginalPos = "-";
                    MotionData[9].OriginalVel = "-";
                    MotionData[9].StandbyPos = "-";
                    MotionData[9].StandbyVel = "-";
                    MotionData[9].StandbyTime = "-";

                    MotionData[9].PressPos = Convert.ToString(plc.PressPos10);
                    MotionData[9].PressForce = Convert.ToString(plc.PressForce10);
                    MotionData[9].PressVel = Convert.ToString(plc.PressVel10);
                    MotionData[9].PressTime = Convert.ToString(plc.PressTime10);

                    MotionData[9].EndMinPos = Convert.ToString(plc.EndMinPosLimit10);
                    MotionData[9].EndMaxPos = Convert.ToString(plc.EndMaxPosLimit10);

                    MotionData[9].EndMinForce = Convert.ToString(plc.EndMinForceLimit10);
                    MotionData[9].EndMaxForce = Convert.ToString(plc.EndMaxForceLimit10);

                    MotionData[9].LimitType = Convert.ToString(plc.LimitType10);
                    MotionData[9].BeginMinForce = Convert.ToString(plc.BeginMinForceLimit10);
                    MotionData[9].BeginMaxForce = Convert.ToString(plc.BeginMaxForceLimit10);

            for (int i = 0; i < CONSTANT.StepAmount; i++)
                {
                    hide_Item_By_Mode(i);   //根據各模式遮蔽未使用參數

                    if (MotionData[i].Mode!=3 || (MotionData[i].Cpk != "Force" && MotionData[i].Cpk != "Position"))
                    {
                        cpk_Initialize(i);       //根據各模式，Cpk紀錄值初始化設定
                    }
                }
        }   //Read the Motion Setting from PLC    從PLC讀取各步序參數對應暫存器
        
        public void MotionData_Write_To_PLC()   //After the editing by the user the motion written into PLC  將在Motion物件的步序參數寫入PLC 
        {
                plc.WriteRecipe("Mode1", MotionData[0].Mode);
                plc.WriteRecipe("WorkOrigin1", double.Parse(MotionData[0].OriginalPos) * 1000);
                plc.WriteRecipe("OriginVel1", double.Parse(MotionData[0].OriginalVel) * 1000);

                plc.WriteRecipe("StandbyPos1", double.Parse(MotionData[0].StandbyPos) * 1000);
                plc.WriteRecipe("StandbyVel1", double.Parse(MotionData[0].StandbyVel) * 1000);
                plc.WriteRecipe("StandbyTime1", double.Parse(MotionData[0].StandbyTime) * 10);

                plc.WriteRecipe("PressPos1", double.Parse(MotionData[0].PressPos) * 1000);
                plc.WriteRecipe("PressForce1", double.Parse(MotionData[0].PressForce) * 10);
                plc.WriteRecipe("PressVel1", double.Parse(MotionData[0].PressVel) * 1000);
                plc.WriteRecipe("PressTime1", double.Parse(MotionData[0].PressTime) * 10);

                plc.WriteRecipe("Mode2",MotionData[1].Mode);
                plc.WriteRecipe("PressPos2", double.Parse(MotionData[1].PressPos) * 1000);
                plc.WriteRecipe("PressForce2", double.Parse(MotionData[1].PressForce) * 10);
                plc.WriteRecipe("PressVel2",double.Parse(MotionData[1].PressVel) * 1000);
                plc.WriteRecipe("PressTime2",double.Parse(MotionData[1].PressTime) * 10);

                plc.WriteRecipe("Mode3", MotionData[2].Mode);
                plc.WriteRecipe("PressPos3", double.Parse(MotionData[2].PressPos) * 1000);
                plc.WriteRecipe("PressForce3", double.Parse(MotionData[2].PressForce) * 10);
                plc.WriteRecipe("PressVel3", double.Parse(MotionData[2].PressVel) * 1000);
                plc.WriteRecipe("PressTime3", double.Parse(MotionData[2].PressTime) * 10);

                plc.WriteRecipe("Mode4", MotionData[3].Mode);
                plc.WriteRecipe("PressPos4", double.Parse(MotionData[3].PressPos) * 1000);
                plc.WriteRecipe("PressForce4", double.Parse(MotionData[3].PressForce) * 10);
                plc.WriteRecipe("PressVel4", double.Parse(MotionData[3].PressVel) * 1000);
                plc.WriteRecipe("PressTime4", double.Parse(MotionData[3].PressTime) * 10);

                plc.WriteRecipe("Mode5", MotionData[4].Mode);
                plc.WriteRecipe("PressPos5", double.Parse(MotionData[4].PressPos) * 1000);
                plc.WriteRecipe("PressForce5", double.Parse(MotionData[4].PressForce) * 10);
                plc.WriteRecipe("PressVel5", double.Parse(MotionData[4].PressVel) * 1000);
                plc.WriteRecipe("PressTime5", double.Parse(MotionData[4].PressTime) * 10);

                plc.WriteRecipe("Mode6", MotionData[5].Mode);
                plc.WriteRecipe("PressPos6", double.Parse(MotionData[5].PressPos) * 1000);
                plc.WriteRecipe("PressForce6", double.Parse(MotionData[5].PressForce) * 10);
                plc.WriteRecipe("PressVel6", double.Parse(MotionData[5].PressVel) * 1000);
                plc.WriteRecipe("PressTime6", double.Parse(MotionData[5].PressTime) * 10);

                plc.WriteRecipe("Mode7", MotionData[6].Mode);
                plc.WriteRecipe("PressPos7", double.Parse(MotionData[6].PressPos) * 1000);
                plc.WriteRecipe("PressForce7", double.Parse(MotionData[6].PressForce) * 10);
                plc.WriteRecipe("PressVel7", double.Parse(MotionData[6].PressVel) * 1000);
                plc.WriteRecipe("PressTime7", double.Parse(MotionData[6].PressTime) * 10);

                plc.WriteRecipe("Mode8", MotionData[7].Mode);
                plc.WriteRecipe("PressPos8", double.Parse(MotionData[7].PressPos) * 1000);
                plc.WriteRecipe("PressForce8", double.Parse(MotionData[7].PressForce) * 10);
                plc.WriteRecipe("PressVel8", double.Parse(MotionData[7].PressVel) * 1000);
                plc.WriteRecipe("PressTime8", double.Parse(MotionData[7].PressTime) * 10);

                plc.WriteRecipe("Mode9", MotionData[8].Mode);
                plc.WriteRecipe("PressPos9", double.Parse(MotionData[8].PressPos) * 1000);
                plc.WriteRecipe("PressForce9", double.Parse(MotionData[8].PressForce) * 10);
                plc.WriteRecipe("PressVel9", double.Parse(MotionData[8].PressVel) * 1000);
                plc.WriteRecipe("PressTime9", double.Parse(MotionData[8].PressTime) * 10);

                plc.WriteRecipe("Mode10", MotionData[9].Mode);
                plc.WriteRecipe("PressPos10", double.Parse(MotionData[9].PressPos) * 1000);
                plc.WriteRecipe("PressForce10", double.Parse(MotionData[9].PressForce) * 10);
                plc.WriteRecipe("PressVel10", double.Parse(MotionData[9].PressVel) * 1000);
                plc.WriteRecipe("PressTime10", double.Parse(MotionData[9].PressTime) * 10);

            if (MotionData[0].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit1", double.Parse(MotionData[0].LimitType));
                plc.WriteRecipe("BeginMaxForce1", double.Parse(MotionData[0].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce1", double.Parse(MotionData[0].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce1", double.Parse(MotionData[0].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce1", double.Parse(MotionData[0].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos1", double.Parse(MotionData[0].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos1", double.Parse(MotionData[0].EndMaxPos) * 1000);
            }

            if (MotionData[1].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit2", double.Parse(MotionData[1].LimitType));
                plc.WriteRecipe("BeginMaxForce2", double.Parse(MotionData[1].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce2", double.Parse(MotionData[1].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce2", double.Parse(MotionData[1].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce2", double.Parse(MotionData[1].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos2", double.Parse(MotionData[1].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos2", double.Parse(MotionData[1].EndMaxPos) * 1000);
            }

            if (MotionData[2].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit3", double.Parse(MotionData[2].LimitType));
                plc.WriteRecipe("BeginMaxForce3", double.Parse(MotionData[2].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce3", double.Parse(MotionData[2].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce3", double.Parse(MotionData[2].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce3", double.Parse(MotionData[2].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos3", double.Parse(MotionData[2].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos3", double.Parse(MotionData[2].EndMaxPos) * 1000);
            }

            if (MotionData[3].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit4", double.Parse(MotionData[3].LimitType));
                plc.WriteRecipe("BeginMaxForce4", double.Parse(MotionData[3].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce4", double.Parse(MotionData[3].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce4", double.Parse(MotionData[3].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce4", double.Parse(MotionData[3].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos4", double.Parse(MotionData[3].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos4", double.Parse(MotionData[3].EndMaxPos) * 1000);
            }

            if (MotionData[4].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit5", double.Parse(MotionData[4].LimitType));
                plc.WriteRecipe("BeginMaxForce5", double.Parse(MotionData[4].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce5", double.Parse(MotionData[4].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce5", double.Parse(MotionData[4].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce5", double.Parse(MotionData[4].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos5", double.Parse(MotionData[4].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos5", double.Parse(MotionData[4].EndMaxPos) * 1000);
            }

            if (MotionData[5].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit6", double.Parse(MotionData[5].LimitType));
                plc.WriteRecipe("BeginMaxForce6", double.Parse(MotionData[5].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce6", double.Parse(MotionData[5].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce6", double.Parse(MotionData[5].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce6", double.Parse(MotionData[5].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos6", double.Parse(MotionData[5].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos6", double.Parse(MotionData[5].EndMaxPos) * 1000);
            }

            if (MotionData[6].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit7", double.Parse(MotionData[6].LimitType));
                plc.WriteRecipe("BeginMaxForce7", double.Parse(MotionData[6].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce7", double.Parse(MotionData[6].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce7", double.Parse(MotionData[6].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce7", double.Parse(MotionData[6].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos7", double.Parse(MotionData[6].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos7", double.Parse(MotionData[6].EndMaxPos) * 1000);
            }

            if (MotionData[7].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit8", double.Parse(MotionData[7].LimitType));
                plc.WriteRecipe("BeginMaxForce8", double.Parse(MotionData[7].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce8", double.Parse(MotionData[7].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce8", double.Parse(MotionData[7].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce8", double.Parse(MotionData[7].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos8", double.Parse(MotionData[7].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos8", double.Parse(MotionData[7].EndMaxPos) * 1000);
            }

            if (MotionData[8].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit9", double.Parse(MotionData[8].LimitType));
                plc.WriteRecipe("BeginMaxForce9", double.Parse(MotionData[8].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce9", double.Parse(MotionData[8].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce9", double.Parse(MotionData[8].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce9", double.Parse(MotionData[8].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos9", double.Parse(MotionData[8].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos9", double.Parse(MotionData[8].EndMaxPos) * 1000);
            }

            if (MotionData[9].LimitType != "")
            {
                plc.WriteRecipe("DynamicLimit10", double.Parse(MotionData[9].LimitType));
                plc.WriteRecipe("BeginMaxForce10", double.Parse(MotionData[9].BeginMaxForce) * 10);
                plc.WriteRecipe("BeginMinForce10", double.Parse(MotionData[9].BeginMinForce) * 10);
                plc.WriteRecipe("EndMinForce10", double.Parse(MotionData[9].EndMinForce) * 10);
                plc.WriteRecipe("EndMaxForce10", double.Parse(MotionData[9].EndMaxForce) * 10);
                plc.WriteRecipe("EndMinPos10", double.Parse(MotionData[9].EndMinPos) * 1000);
                plc.WriteRecipe("EndMaxPos10", double.Parse(MotionData[9].EndMaxPos) * 1000);
            }
            plc.Write("EnhanceRecipeCtrl", 2);//加強型配方控制-將配方參數從PLC寫至HMI
            plc.Write("ScreenNum", 42);
        }

        public int getLastMotionNum()   //Return the last step in the motion   最後一個會執行的步序1~5
        {
            int temp = 0;
            for (int i = 0; i < CONSTANT.StepAmount; i++)
            {
                if (MotionData[i].Mode == 0 && (i - 1) > 0)
                {
                    return i - 1;
                }
                temp = i;
            }
            return temp;
        }
        public string getModeName(int mode)
        {
            if (mode == 0)
            {
                return rM.GetString("Motionless");
            }
            else if (mode == 1)
            {
                return rM.GetString("PosMode");
            }
            else if (mode == 2)
            {
                return rM.GetString("ForceMode");
            }
            else if (mode == 3)
            {
                return rM.GetString("DistMode");
            }
            else if (mode == 4)
            {
                return rM.GetString("ForcePosMode");
            }
            else if (mode == 5)
            {
                return rM.GetString("ForceDistMode");
            }
            else if (mode == 6)
            {
                return rM.GetString("IOSignal");
            }
            else if (mode == 7)
            {
                return rM.GetString("OutputSignal");
            }
            else if (mode == 8)
            {
                return rM.GetString("SlopeMode");
            }
            else if (mode == 9)
            {
                return rM.GetString("SignalMode");
            }
            else if (mode == 10)
            {
                return rM.GetString("Jump");
            }
            return "";
        }   //Mapping the mode number with Chinese words 依模式編號輸出中文名稱

        private void hide_Item_By_Mode(int motionNum)   //Hide the useless items according to the different type of mode    根據各模式遮蔽未使用參數
        {
            if (MotionData[motionNum].Mode == 0) //Motionless
            {
                if (motionNum>0)
                {
                    MotionData[motionNum].OriginalPos = "-";
                    MotionData[motionNum].OriginalVel = "-";

                    MotionData[motionNum].StandbyPos = "-";
                    MotionData[motionNum].StandbyVel = "-";
                    MotionData[motionNum].StandbyTime = "-";
                }

                MotionData[motionNum].PressPos = "-";
                MotionData[motionNum].PressForce = "-";
                MotionData[motionNum].PressVel = "-";
                MotionData[motionNum].PressTime = "-";
            }
            else if (MotionData[motionNum].Mode == 1)   //Position Mode
            {
                MotionData[motionNum].PressForce = "-";
            }
            else if (MotionData[motionNum].Mode == 2)  //Force Mode
            {
                MotionData[motionNum].PressPos = "-";
            }
            else if (MotionData[motionNum].Mode == 3) //Distance Mode
            {
                MotionData[motionNum].PressForce = "-";
            }

        }
        private void cpk_Initialize(int motionNum)   //Initialize the Cpk setting based on the specfic mode  根據各模式，Cpk紀錄值初始化設定
        {
            if (MotionData[motionNum].Mode == 0) //Motionless
            {
                MotionData[motionNum].Cpk = "-";
            }
            else if (MotionData[motionNum].Mode == 1)  //Position Mode
            {
                MotionData[motionNum].Cpk = "Force";
            }
            else if (MotionData[motionNum].Mode == 2)  //Force Mode
            {
                MotionData[motionNum].Cpk = "Position";
            }
            else if (MotionData[motionNum].Mode == 3)  //Distance Mode
            {
                MotionData[motionNum].Cpk = "Force";
            }
            else if (MotionData[motionNum].Mode == 4)  //Force Position Mode
            {
                MotionData[motionNum].Cpk = "Force";
            }
            else if (MotionData[motionNum].Mode == 5)  //Force Distance Mode
            {
                MotionData[motionNum].Cpk = "Position";
            }
            else if (MotionData[motionNum].Mode == 6)  //IO Signal
            {
                MotionData[motionNum].Cpk = "Force";
            }
            else  //Other Mode
            {
                MotionData[motionNum].Cpk = "Force";
            }
        }
    }
}
