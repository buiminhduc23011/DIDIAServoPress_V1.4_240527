using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;

namespace DIAServoPress
{
    public class PLC
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        public NewMbusClient mbc;

        public double LivePos;
        public double LiveForce;
        public double LiveVel;

        public int LiveStep;

        public double LiveStandbyTime;
        public double LivePressTime;
        public double LiveProductTime;

        public int LiveOKNG ;
        public long LiveProductAmount;
        public long LiveOKAmount;
        public long LiveNGAmount;

        public double PressedPos;
        public double PressedForce;

        public double TargetStandbyPos;
        public int Unit;
        public long Type;
        public long MaxStroke;
        
        public int LiveStatus;

        public long RecipeName1;
        public long RecipeName2;
        public long RecipeName3;
        public long RecipeName4;
        public long RecipeName5;
        public long RecipeName6;
        public long RecipeName7;

        public long WorkOrder1;
        public long WorkOrder2;
        public long WorkOrder3;
        public long WorkOrder4;
        public long WorkOrder5;
        public long WorkOrder6;
        public long WorkOrder7;


        public double OriginPos;
        public double OriginVel;
        public double StandbyPos;
        public double StandbyVel;
        public double StandbyTime;

        public int Mode1;
        public double PressPos1;
        public double PressForce1;
        public double PressVel1;
        public double PressTime1;
        public double LimitType1;
        public double BeginMaxForceLimit1;
        public double BeginMinForceLimit1;
        public double EndMaxForceLimit1;
        public double EndMinForceLimit1;
        public double EndMaxPosLimit1;
        public double EndMinPosLimit1;

        public int Mode2;
        public double PressPos2;
        public double PressForce2;
        public double PressVel2;
        public double PressTime2;
        public double LimitType2;
        public double BeginMaxForceLimit2;
        public double BeginMinForceLimit2;
        public double EndMaxForceLimit2;
        public double EndMinForceLimit2;
        public double EndMaxPosLimit2;
        public double EndMinPosLimit2;

        public int Mode3;
        public double PressPos3;
        public double PressForce3;
        public double PressVel3;
        public double PressTime3;
        public double LimitType3;
        public double BeginMaxForceLimit3;
        public double BeginMinForceLimit3;
        public double EndMaxForceLimit3;
        public double EndMinForceLimit3;
        public double EndMaxPosLimit3;
        public double EndMinPosLimit3;

        public int Mode4;
        public double PressPos4;
        public double PressForce4;
        public double PressVel4;
        public double PressTime4;
        public double LimitType4;
        public double BeginMaxForceLimit4;
        public double BeginMinForceLimit4;
        public double EndMaxForceLimit4;
        public double EndMinForceLimit4;
        public double EndMaxPosLimit4;
        public double EndMinPosLimit4;

        public int Mode5;
        public double PressPos5;
        public double PressForce5;
        public double PressVel5;
        public double PressTime5;
        public double LimitType5;
        public double BeginMaxForceLimit5;
        public double BeginMinForceLimit5;
        public double EndMaxForceLimit5;
        public double EndMinForceLimit5;
        public double EndMaxPosLimit5;
        public double EndMinPosLimit5;

        public int Mode6;
        public double PressPos6;
        public double PressForce6;
        public double PressVel6;
        public double PressTime6;
        public double LimitType6;
        public double BeginMaxForceLimit6;
        public double BeginMinForceLimit6;
        public double EndMaxForceLimit6;
        public double EndMinForceLimit6;
        public double EndMaxPosLimit6;
        public double EndMinPosLimit6;

        public int Mode7;
        public double PressPos7;
        public double PressForce7;
        public double PressVel7;
        public double PressTime7;
        public double LimitType7;
        public double BeginMaxForceLimit7;
        public double BeginMinForceLimit7;
        public double EndMaxForceLimit7;
        public double EndMinForceLimit7;
        public double EndMaxPosLimit7;
        public double EndMinPosLimit7;

        public int Mode8;
        public double PressPos8;
        public double PressForce8;
        public double PressVel8;
        public double PressTime8;
        public double LimitType8;
        public double BeginMaxForceLimit8;
        public double BeginMinForceLimit8;
        public double EndMaxForceLimit8;
        public double EndMinForceLimit8;
        public double EndMaxPosLimit8;
        public double EndMinPosLimit8;

        public int Mode9;
        public double PressPos9;
        public double PressForce9;
        public double PressVel9;
        public double PressTime9;
        public double LimitType9;
        public double BeginMaxForceLimit9;
        public double BeginMinForceLimit9;
        public double EndMaxForceLimit9;
        public double EndMinForceLimit9;
        public double EndMaxPosLimit9;
        public double EndMinPosLimit9;

        public int Mode10;
        public double PressPos10;
        public double PressForce10;
        public double PressVel10;
        public double PressTime10;
        public double LimitType10;
        public double BeginMaxForceLimit10;
        public double BeginMinForceLimit10;
        public double EndMaxForceLimit10;
        public double EndMinForceLimit10;
        public double EndMaxPosLimit10;
        public double EndMinPosLimit10;


        public double Manual_ContactPos;
        public double Manual_MaxPos;
        public double Manual_ForceAtMaxPos;
        public double Manual_MaxForce;
        public double Manual_PosAtMaxForce;


        public long OnOff;
        public long Initialized;
        public long Press;
        public long DIAPress;
        public long ConnectStatus;
        public long RecipeNum;
        public long RecipeCtrl;
        public long EnhanceRecipeCtrl;
        public long RecipeChanged;
        public long RecipeChangedSave;

        public string IPAddress;
        public int Port;

        private int clock = 0;
        private int TimeScale = 0;

        public bool Pause = false;
        public PLC() 
        {

        }

        public bool Connect(string ip, string port) 
        {
            IPAddress = ip;
            Port = Convert.ToInt32(port);
            mbc = new NewMbusClient();
            mbc.IPAddress = ip;
            mbc.Port = Int16.Parse(port);

            if (mbc.ReConnect() == true)
            {
                LiveUpdate();
                SlowUpdate();
                StartConnect();
                CheckTimeScale();
                return true;
            }
            else if(mbc.ReConnect()==false)
            {
                return false;
            }
            return false;
        }

        private void CheckTimeScale()
        {
            if (mbc.Read_PLC_SingleCoil(Ether_Addr.TimeScale.ToString()) == 1)
            {
                TimeScale = CONSTANT.TimeScaleNew;
            }
            else
            {
                TimeScale = CONSTANT.TimeScaleOld;
            }
        }

        public bool CheckKey() 
        {
            if (mbc.Read_PLC_SingleCoil(Ether_Addr.DIAServoPressKey.ToString()) == 1)
            {
                return true;
            }
            return false;
        }

        public int getTimeScale()
        {
            return TimeScale;
        }

        public void StartConnect()
        {
            mbc.Write_PLC_16(Ether_Addr.DIAPress.ToString(), "0");
        }

        public void ManualUpdate() 
        { 
           Manual_ContactPos=mbc.Read_PLC(Ether_Addr.Manual_ContactPos.ToString())/(double)1000;
           Manual_MaxPos = mbc.Read_PLC(Ether_Addr.Manual_MaxPos.ToString()) / (double)1000;
           Manual_ForceAtMaxPos = mbc.Read_PLC(Ether_Addr.Manual_ForceAtMaxPos.ToString()) / (double)10;
           Manual_MaxForce = mbc.Read_PLC(Ether_Addr.Manual_MaxForce.ToString()) / (double)10;
           Manual_PosAtMaxForce = mbc.Read_PLC(Ether_Addr.Manual_PosAtMaxForce.ToString()) / (double)1000;
        }


        public int State
        {
            get { return (int)mbc.Read_PLC_16(Ether_Addr.State.ToString()); }
            
        }
        /*
        public void ResetManual() 
        { 
           mbc.Write_PLC(Ether_Addr.Manual_ContactPos.ToString(),"0");
           mbc.Write_PLC(Ether_Addr.Manual_MaxPos.ToString(),"0");
           mbc.Write_PLC(Ether_Addr.Manual_ForceAtMaxPos.ToString(),"0");
           mbc.Write_PLC(Ether_Addr.Manual_MaxForce.ToString(),"0");
           mbc.Write_PLC(Ether_Addr.Manual_PosAtMaxForce.ToString(),"0");
        }*/

        public void DIAPressClosed()
        {
            mbc.Write_PLC_16(Ether_Addr.DIAPress.ToString(), "0");
        }

        public bool ReConnect() 
        {
            return mbc.ReConnect();
        }
        
        public void Disconnect()
        {
            mbc.Disconnect();
        }

        public void Scan() 
        {
            if (clock < 20)
            {
                LiveUpdate();
                if (clock % 5 == 0)
                {
                    NomalUpdate();
                }
            }
            else 
            {
                SlowUpdate();
                clock = 0;
            }
            clock++;
        }

        private void LiveUpdate() 
        {
            if (Pause == false)
            {
                LivePos = mbc.Read_PLC(Ether_Addr.LivePos.ToString()) / (double)1000;
                LiveForce = mbc.Read_PLC(Ether_Addr.LiveForce.ToString()) / (double)10;
                LiveVel = mbc.Read_PLC(Ether_Addr.LiveVel.ToString()) / (double)1000;

                Press = mbc.Read_PLC_16(Ether_Addr.Press.ToString());
                DIAPress = mbc.Read_PLC_16(Ether_Addr.DIAPress.ToString());
                LiveOKNG = (int)mbc.Read_PLC_16(Ether_Addr.LiveOKNG.ToString());
                LiveStatus = (int)mbc.Read_PLC(Ether_Addr.LiveStatus.ToString());
            }
        }

        public void NomalUpdate() 
        {
            if (Pause == false)
            {
                LiveStandbyTime = mbc.Read_PLC(Ether_Addr.LiveStandbyTime.ToString()) / (double)10;
                LivePressTime = mbc.Read_PLC(Ether_Addr.LivePressTime.ToString()) / (double)10; ;
                LiveProductTime = mbc.Read_PLC(Ether_Addr.LiveProductTime.ToString()) / (double)TimeScale;//TimeScale

                LiveStep = (int)mbc.Read_PLC(Ether_Addr.LiveStep.ToString());

                LiveProductAmount = mbc.Read_PLC(Ether_Addr.LiveProductAmount.ToString());
                LiveOKAmount = mbc.Read_PLC(Ether_Addr.LiveOKAmount.ToString());
                LiveNGAmount = mbc.Read_PLC(Ether_Addr.LiveNGAmount.ToString());

                PressedPos = (mbc.Read_PLC(Ether_Addr.PressedPos.ToString()) / (double)1000);
                PressedForce = (mbc.Read_PLC(Ether_Addr.PressedForce.ToString()) / (double)10);
                TargetStandbyPos = (mbc.Read_PLC(Ether_Addr.TargetStandbyPos.ToString()) / (double)1000);
            }
        }

        private void SlowUpdate() 
        {
            if (Pause == false)
            {
                Unit = (int)mbc.Read_PLC_16(Ether_Addr.Unit.ToString());
                Type = mbc.Read_PLC_16(Ether_Addr.Type.ToString());
                MaxStroke = mbc.Read_PLC(Ether_Addr.MaxStroke.ToString()) / 1000;

                RecipeName1 = mbc.Read_PLC_16(Ether_Addr.RecipeName1.ToString());
                RecipeName2 = mbc.Read_PLC_16(Ether_Addr.RecipeName2.ToString());
                RecipeName3 = mbc.Read_PLC_16(Ether_Addr.RecipeName3.ToString());
                RecipeName4 = mbc.Read_PLC_16(Ether_Addr.RecipeName4.ToString());
                RecipeName5 = mbc.Read_PLC_16(Ether_Addr.RecipeName5.ToString());
                RecipeName6 = mbc.Read_PLC_16(Ether_Addr.RecipeName6.ToString());
                RecipeName7 = mbc.Read_PLC_16(Ether_Addr.RecipeName7.ToString());

                WorkOrder1 = mbc.Read_PLC_16(Ether_Addr.WorkOrder1.ToString());
                WorkOrder2 = mbc.Read_PLC_16(Ether_Addr.WorkOrder2.ToString());
                WorkOrder3 = mbc.Read_PLC_16(Ether_Addr.WorkOrder3.ToString());
                WorkOrder4 = mbc.Read_PLC_16(Ether_Addr.WorkOrder4.ToString());
                WorkOrder5 = mbc.Read_PLC_16(Ether_Addr.WorkOrder5.ToString());
                WorkOrder6 = mbc.Read_PLC_16(Ether_Addr.WorkOrder6.ToString());
                WorkOrder7 = mbc.Read_PLC_16(Ether_Addr.WorkOrder7.ToString());

                OnOff = mbc.Read_PLC_16(Ether_Addr.OnOff.ToString());
                Initialized = mbc.Read_PLC_16(Ether_Addr.Initialized.ToString());
                ConnectStatus = mbc.Read_PLC_16(Ether_Addr.Connect.ToString());

                RecipeNum = mbc.Read_PLC_16(Ether_Addr.RecipeNum.ToString());
                RecipeCtrl = mbc.Read_PLC_16(Ether_Addr.RecipeCtrl.ToString());
                EnhanceRecipeCtrl = mbc.Read_PLC_16(Ether_Addr.EnhanceRecipeCtrl.ToString());

                RecipeChanged = mbc.Read_PLC_16(Ether_Addr.RecipeChanged.ToString());
            }
        }


        public void ReturnToZero() 
        { 
            WriteProductAmount(0);
            WriteOKAmount(0);
            WriteNGAmount(0);
        }
        
        public void Write(string context,double number) 
        {
            switch (context)
            {
                case "Recipe":
                    mbc.Write_PLC_SingleCoil(Ether_Addr.WriteRecipe.ToString(), Convert.ToInt32(number));
                    break;
                case "EnhancedRecipeNum":
                    mbc.Write_PLC_16(Ether_Addr.EnhancedRecipeNum.ToString(), Convert.ToString(number));//配方名稱組別寫入
                    break;
                case "EnhanceRecipeCtrl":
                    mbc.Write_PLC_16(Ether_Addr.EnhanceRecipeCtrl.ToString(), Convert.ToString(number));
                    break;
                case "RecipeCtrl":
                    mbc.Write_PLC_16(Ether_Addr.RecipeCtrl.ToString(), Convert.ToString(number));
                    break;
                case "RecipeNum":
                    mbc.Write_PLC_16(Ether_Addr.RecipeNum.ToString(), Convert.ToString(number));
                    break;
                case "ScreenNum":
                    mbc.Write_PLC_16(Ether_Addr.ScreenNum.ToString(), Convert.ToString(number)); //加強型配方控制-將配方參數從PLC寫至HMI
                    break;
                case "Connect":
                    mbc.Write_PLC_16(Ether_Addr.Connect.ToString(), "4");
                    break;
                case "RecipeChanged":
                    mbc.Write_PLC_16(Ether_Addr.RecipeChanged.ToString(), Convert.ToString(number));
                    break;
                default:
                    break;
                    //return "";
            }
        }

        private int ConvertTo32(int num1, int num2)
        {
            string sTemp1 = Convert.ToString(num1, 2);
            string sTemp2 = Convert.ToString(num2, 2);

            if (sTemp1.Length > 16)
            {
                while (sTemp1.Length < 32)
                {
                    sTemp1 = "0" + sTemp1;
                }

                sTemp1 = sTemp1.Substring(16, 16);
            }
            else if (sTemp1.Length < 16)
            {
                while (sTemp1.Length < 16)
                {
                    sTemp1 = "0" + sTemp1;
                }
            }

            if (num2 == 0)
            {
                sTemp2 = "0000000000000000";
            }
            else if (num2 < 0)
            {
                sTemp2 = "1111111111111111";
            }

            return Convert.ToInt32(sTemp2 + sTemp1, 2);
        }

        public void ReadRecipe()
        {
            int[] sRecipe1 = new int[100];
            int[] sRecipe4 = new int[100];
            int[] sRecipe8 = new int[100];

            sRecipe1 = mbc.Read_PLC_Multiple("D" + Convert.ToString(Ether_Addr.RecipeA1), 100);
            sRecipe4 = mbc.Read_PLC_Multiple("D" + Convert.ToString(Ether_Addr.RecipeA2), 100);
            sRecipe8 = mbc.Read_PLC_Multiple("D" + Convert.ToString(Ether_Addr.RecipeA3), 80);


            OriginPos = ConvertTo32(sRecipe1[4], sRecipe1[5]) / (double)1000;
            OriginVel = ConvertTo32(sRecipe1[6], sRecipe1[7]) / (double)1000;
            StandbyPos = ConvertTo32(sRecipe1[8], sRecipe1[9]) / (double)1000;
            StandbyVel = ConvertTo32(sRecipe1[10], sRecipe1[11]) / (double)1000;
            StandbyTime = ConvertTo32(sRecipe1[12], sRecipe1[13]) / (double)10;

            Mode1 = ConvertTo32(sRecipe1[14], sRecipe1[15]);
            PressPos1 = ConvertTo32(sRecipe1[16], sRecipe1[17]) / (double)1000;
            PressForce1 = ConvertTo32(sRecipe1[18], sRecipe1[19]) / (double)10;
            PressVel1 = ConvertTo32(sRecipe1[20], sRecipe1[21]) / (double)1000;
            PressTime1 = ConvertTo32(sRecipe1[22], sRecipe1[23]) / (double)10;
            LimitType1 = ConvertTo32(sRecipe1[24], sRecipe1[25]);
            BeginMaxForceLimit1 = ConvertTo32(sRecipe1[26], sRecipe1[27]) / (double)10;
            BeginMinForceLimit1 = ConvertTo32(sRecipe1[28], sRecipe1[29]) / (double)10;
            EndMaxForceLimit1 = ConvertTo32(sRecipe1[30], sRecipe1[31]) / (double)10;
            EndMinForceLimit1 = ConvertTo32(sRecipe1[32], sRecipe1[33]) / (double)10;
            EndMaxPosLimit1 = ConvertTo32(sRecipe1[34], sRecipe1[35]) / (double)1000;
            EndMinPosLimit1 = ConvertTo32(sRecipe1[36], sRecipe1[37]) / (double)1000;

            Mode2 = ConvertTo32(sRecipe1[38], sRecipe1[39]);
            PressPos2 = ConvertTo32(sRecipe1[40], sRecipe1[41]) / (double)1000;
            PressForce2 = ConvertTo32(sRecipe1[42], sRecipe1[43]) / (double)10;
            PressVel2 = ConvertTo32(sRecipe1[44], sRecipe1[45]) / (double)1000;
            PressTime2 = ConvertTo32(sRecipe1[46], sRecipe1[47]) / (double)10;
            LimitType2 = ConvertTo32(sRecipe1[48], sRecipe1[49]);
            BeginMaxForceLimit2 = ConvertTo32(sRecipe1[50], sRecipe1[51]) / (double)10;
            BeginMinForceLimit2 = ConvertTo32(sRecipe1[52], sRecipe1[53]) / (double)10;
            EndMaxForceLimit2 = ConvertTo32(sRecipe1[54], sRecipe1[55]) / (double)10;
            EndMinForceLimit2 = ConvertTo32(sRecipe1[56], sRecipe1[57]) / (double)10;
            EndMaxPosLimit2 = ConvertTo32(sRecipe1[58], sRecipe1[59]) / (double)1000;
            EndMinPosLimit2 = ConvertTo32(sRecipe1[60], sRecipe1[61]) / (double)1000;

            Mode3 = ConvertTo32(sRecipe1[62], sRecipe1[63]);
            PressPos3 = ConvertTo32(sRecipe1[64], sRecipe1[65]) / (double)1000;
            PressForce3 = ConvertTo32(sRecipe1[66], sRecipe1[67]) / (double)10;
            PressVel3 = ConvertTo32(sRecipe1[68], sRecipe1[69]) / (double)1000;
            PressTime3 = ConvertTo32(sRecipe1[70], sRecipe1[71]) / (double)10;
            LimitType3 = ConvertTo32(sRecipe1[72], sRecipe1[73]);
            BeginMaxForceLimit3 = ConvertTo32(sRecipe1[74], sRecipe1[75]) / (double)10;
            BeginMinForceLimit3 = ConvertTo32(sRecipe1[76], sRecipe1[77]) / (double)10;
            EndMaxForceLimit3 = ConvertTo32(sRecipe1[78], sRecipe1[79]) / (double)10;
            EndMinForceLimit3 = ConvertTo32(sRecipe1[80], sRecipe1[81]) / (double)10;
            EndMaxPosLimit3 = ConvertTo32(sRecipe1[82], sRecipe1[83]) / (double)1000;
            EndMinPosLimit3 = ConvertTo32(sRecipe1[84], sRecipe1[85]) / (double)1000;

            Mode4 = ConvertTo32(sRecipe4[0], sRecipe4[1]);
            PressPos4 = ConvertTo32(sRecipe4[2], sRecipe4[3]) / (double)1000;
            PressForce4 = ConvertTo32(sRecipe4[4], sRecipe4[5]) / (double)10;
            PressVel4 = ConvertTo32(sRecipe4[6], sRecipe4[7]) / (double)1000;
            PressTime4 = ConvertTo32(sRecipe4[8], sRecipe4[9]) / (double)10;
            LimitType4 = ConvertTo32(sRecipe4[10], sRecipe4[11]);
            BeginMaxForceLimit4 = ConvertTo32(sRecipe4[12], sRecipe4[13]) / (double)10;
            BeginMinForceLimit4 = ConvertTo32(sRecipe4[14], sRecipe4[15]) / (double)10;
            EndMaxForceLimit4 = ConvertTo32(sRecipe4[16], sRecipe4[17]) / (double)10;
            EndMinForceLimit4 = ConvertTo32(sRecipe4[18], sRecipe4[19]) / (double)10;
            EndMaxPosLimit4 = ConvertTo32(sRecipe4[20], sRecipe4[21]) / (double)1000;
            EndMinPosLimit4 = ConvertTo32(sRecipe4[22], sRecipe4[23]) / (double)1000;

            Mode5 = ConvertTo32(sRecipe4[24], sRecipe4[25]);
            PressPos5 = ConvertTo32(sRecipe4[26], sRecipe4[27]) / (double)1000;
            PressForce5 = ConvertTo32(sRecipe4[28], sRecipe4[29]) / (double)10;
            PressVel5 = ConvertTo32(sRecipe4[30], sRecipe4[31]) / (double)1000;
            PressTime5 = ConvertTo32(sRecipe4[32], sRecipe4[33]) / (double)10;
            LimitType5 = ConvertTo32(sRecipe4[34], sRecipe4[35]);
            BeginMaxForceLimit5 = ConvertTo32(sRecipe4[36], sRecipe4[37]) / (double)10;
            BeginMinForceLimit5 = ConvertTo32(sRecipe4[38], sRecipe4[39]) / (double)10;
            EndMaxForceLimit5 = ConvertTo32(sRecipe4[40], sRecipe4[41]) / (double)10;
            EndMinForceLimit5 = ConvertTo32(sRecipe4[42], sRecipe4[43]) / (double)10;
            EndMaxPosLimit5 = ConvertTo32(sRecipe4[44], sRecipe4[45]) / (double)1000;
            EndMinPosLimit5 = ConvertTo32(sRecipe4[46], sRecipe4[47]) / (double)1000;

            Mode6 = ConvertTo32(sRecipe4[48], sRecipe4[49]);
            PressPos6 = ConvertTo32(sRecipe4[50], sRecipe4[51]) / (double)1000;
            PressForce6 = ConvertTo32(sRecipe4[52], sRecipe4[53]) / (double)10;
            PressVel6 = ConvertTo32(sRecipe4[54], sRecipe4[55]) / (double)1000;
            PressTime6 = ConvertTo32(sRecipe4[56], sRecipe4[57]) / (double)10;
            LimitType6 = ConvertTo32(sRecipe4[58], sRecipe4[59]);
            BeginMaxForceLimit6 = ConvertTo32(sRecipe4[60], sRecipe4[61]) / (double)10;
            BeginMinForceLimit6 = ConvertTo32(sRecipe4[62], sRecipe4[63]) / (double)10;
            EndMaxForceLimit6 = ConvertTo32(sRecipe4[64], sRecipe4[65]) / (double)10;
            EndMinForceLimit6 = ConvertTo32(sRecipe4[66], sRecipe4[67]) / (double)10;
            EndMaxPosLimit6 = ConvertTo32(sRecipe4[68], sRecipe4[69]) / (double)1000;
            EndMinPosLimit6 = ConvertTo32(sRecipe4[70], sRecipe4[71]) / (double)1000;

            Mode7 = ConvertTo32(sRecipe4[72], sRecipe4[73]);
            PressPos7 = ConvertTo32(sRecipe4[74], sRecipe4[75]) / (double)1000;
            PressForce7 = ConvertTo32(sRecipe4[76], sRecipe4[77]) / (double)10;
            PressVel7 = ConvertTo32(sRecipe4[78], sRecipe4[79]) / (double)1000;
            PressTime7 = ConvertTo32(sRecipe4[80], sRecipe4[81]) / (double)10;
            LimitType7 = ConvertTo32(sRecipe4[82], sRecipe4[83]);
            BeginMaxForceLimit7 = ConvertTo32(sRecipe4[84], sRecipe4[85]) / (double)10;
            BeginMinForceLimit7 = ConvertTo32(sRecipe4[86], sRecipe4[87]) / (double)10;
            EndMaxForceLimit7 = ConvertTo32(sRecipe4[88], sRecipe4[89]) / (double)10;
            EndMinForceLimit7 = ConvertTo32(sRecipe4[90], sRecipe4[91]) / (double)10;
            EndMaxPosLimit7 = ConvertTo32(sRecipe4[92], sRecipe4[93]) / (double)1000;
            EndMinPosLimit7 = ConvertTo32(sRecipe4[94], sRecipe4[95]) / (double)1000;

            Mode8 = ConvertTo32(sRecipe8[0], sRecipe8[1]);
            PressPos8 = ConvertTo32(sRecipe8[2], sRecipe8[3]) / (double)1000;
            PressForce8 = ConvertTo32(sRecipe8[4], sRecipe8[5]) / (double)10;
            PressVel8 = ConvertTo32(sRecipe8[6], sRecipe8[7]) / (double)1000;
            PressTime8 = ConvertTo32(sRecipe8[8], sRecipe8[9]) / (double)10;
            LimitType8 = ConvertTo32(sRecipe8[10], sRecipe8[11]);
            BeginMaxForceLimit8 = ConvertTo32(sRecipe8[12], sRecipe8[13]) / (double)10;
            BeginMinForceLimit8 = ConvertTo32(sRecipe8[14], sRecipe8[15]) / (double)10;
            EndMaxForceLimit8 = ConvertTo32(sRecipe8[16], sRecipe8[17]) / (double)10;
            EndMinForceLimit8 = ConvertTo32(sRecipe8[18], sRecipe8[19]) / (double)10;
            EndMaxPosLimit8 = ConvertTo32(sRecipe8[20], sRecipe8[21]) / (double)1000;
            EndMinPosLimit8 = ConvertTo32(sRecipe8[22], sRecipe8[23]) / (double)1000;

            Mode9 = ConvertTo32(sRecipe8[24], sRecipe8[25]);
            PressPos9 = ConvertTo32(sRecipe8[26], sRecipe8[27]) / (double)1000;
            PressForce9 = ConvertTo32(sRecipe8[28], sRecipe8[29]) / (double)10;
            PressVel9 = ConvertTo32(sRecipe8[30], sRecipe8[31]) / (double)1000;
            PressTime9 = ConvertTo32(sRecipe8[32], sRecipe8[33]) / (double)10;
            LimitType9 = ConvertTo32(sRecipe8[34], sRecipe8[35]);
            BeginMaxForceLimit9 = ConvertTo32(sRecipe8[36], sRecipe8[37]) / (double)10;
            BeginMinForceLimit9 = ConvertTo32(sRecipe8[38], sRecipe8[39]) / (double)10;
            EndMaxForceLimit9 = ConvertTo32(sRecipe8[40], sRecipe8[41]) / (double)10;
            EndMinForceLimit9 = ConvertTo32(sRecipe8[42], sRecipe8[43]) / (double)10;
            EndMaxPosLimit9 = ConvertTo32(sRecipe8[44], sRecipe8[45]) / (double)1000;
            EndMinPosLimit9 = ConvertTo32(sRecipe8[46], sRecipe8[47]) / (double)1000;

            Mode10 = ConvertTo32(sRecipe8[48], sRecipe8[49]);
            PressPos10 = ConvertTo32(sRecipe8[50], sRecipe8[51]) / (double)1000;
            PressForce10 = ConvertTo32(sRecipe8[52], sRecipe8[53]) / (double)10;
            PressVel10 = ConvertTo32(sRecipe8[54], sRecipe8[55]) / (double)1000;
            PressTime10 = ConvertTo32(sRecipe8[56], sRecipe8[57]) / (double)10;
            LimitType10 = ConvertTo32(sRecipe8[58], sRecipe8[59]);
            BeginMaxForceLimit10 = ConvertTo32(sRecipe8[60], sRecipe8[61]) / (double)10;
            BeginMinForceLimit10 = ConvertTo32(sRecipe8[62], sRecipe8[63]) / (double)10;
            EndMaxForceLimit10 = ConvertTo32(sRecipe8[64], sRecipe8[65]) / (double)10;
            EndMinForceLimit10 = ConvertTo32(sRecipe8[66], sRecipe8[67]) / (double)10;
            EndMaxPosLimit10 = ConvertTo32(sRecipe8[68], sRecipe8[69]) / (double)1000;
            EndMinPosLimit10 = ConvertTo32(sRecipe8[70], sRecipe8[71]) / (double)1000;
        }


        private string RecipeAddress(string name)
        { 
            switch(name)
            {
                case "WorkOrigin1": return Ether_Addr.OriginPos.ToString();
                case "OriginVel1": return Ether_Addr.OriginVel.ToString();
                case "StandbyPos1": return Ether_Addr.StandbyPos.ToString();
                case "StandbyVel1": return Ether_Addr.StandbyVel.ToString();
                case "StandbyTime1": return Ether_Addr.StandbyTime.ToString();

                case "Mode1": return Ether_Addr.Mode1.ToString();
                case "PressPos1": return Ether_Addr.PressPos1.ToString();
                case "PressForce1": return Ether_Addr.PressForce1.ToString();
                case "PressVel1":return Ether_Addr.PressVel1.ToString();
                case "PressTime1":return Ether_Addr.PressTime1.ToString();
                case "DynamicLimit1": return Ether_Addr.LimitType1.ToString();
                case "BeginMaxForce1": return Ether_Addr.BeginMaxForceLimit1.ToString();
                case "BeginMinForce1": return Ether_Addr.BeginMinForceLimit1.ToString();
                case "EndMaxForce1": return Ether_Addr.EndMaxForceLimit1.ToString();
                case "EndMinForce1": return Ether_Addr.EndMinForceLimit1.ToString();
                case "EndMaxPos1": return Ether_Addr.EndMaxPosLimit1.ToString();
                case "EndMinPos1": return Ether_Addr.EndMinPosLimit1.ToString();

                case "Mode2": return Ether_Addr.Mode2.ToString();
                case "PressPos2": return Ether_Addr.PressPos2.ToString();
                case "PressForce2": return Ether_Addr.PressForce2.ToString();
                case "PressVel2": return Ether_Addr.PressVel2.ToString();
                case "PressTime2": return Ether_Addr.PressTime2.ToString();
                case "DynamicLimit2": return Ether_Addr.LimitType2.ToString();
                case "BeginMaxForce2": return Ether_Addr.BeginMaxForceLimit2.ToString();
                case "BeginMinForce2": return Ether_Addr.BeginMinForceLimit2.ToString();
                case "EndMaxForce2": return Ether_Addr.EndMaxForceLimit2.ToString();
                case "EndMinForce2": return Ether_Addr.EndMinForceLimit2.ToString();
                case "EndMaxPos2": return Ether_Addr.EndMaxPosLimit2.ToString();
                case "EndMinPos2": return Ether_Addr.EndMinPosLimit2.ToString();

                case "Mode3": return Ether_Addr.Mode3.ToString();
                case "PressPos3": return Ether_Addr.PressPos3.ToString();
                case "PressForce3": return Ether_Addr.PressForce3.ToString();
                case "PressVel3": return Ether_Addr.PressVel3.ToString();
                case "PressTime3": return Ether_Addr.PressTime3.ToString();
                case "DynamicLimit3": return Ether_Addr.LimitType3.ToString();
                case "BeginMaxForce3": return Ether_Addr.BeginMaxForceLimit3.ToString();
                case "BeginMinForce3": return Ether_Addr.BeginMinForceLimit3.ToString();
                case "EndMaxForce3": return Ether_Addr.EndMaxForceLimit3.ToString();
                case "EndMinForce3": return Ether_Addr.EndMinForceLimit3.ToString();
                case "EndMaxPos3": return Ether_Addr.EndMaxPosLimit3.ToString();
                case "EndMinPos3": return Ether_Addr.EndMinPosLimit3.ToString();

                case "Mode4": return Ether_Addr.Mode4.ToString();
                case "PressPos4": return Ether_Addr.PressPos4.ToString();
                case "PressForce4": return Ether_Addr.PressForce4.ToString();
                case "PressVel4": return Ether_Addr.PressVel4.ToString();
                case "PressTime4": return Ether_Addr.PressTime4.ToString();
                case "DynamicLimit4": return Ether_Addr.LimitType4.ToString();
                case "BeginMaxForce4": return Ether_Addr.BeginMaxForceLimit4.ToString();
                case "BeginMinForce4": return Ether_Addr.BeginMinForceLimit4.ToString();
                case "EndMaxForce4": return Ether_Addr.EndMaxForceLimit4.ToString();
                case "EndMinForce4": return Ether_Addr.EndMinForceLimit4.ToString();
                case "EndMaxPos4": return Ether_Addr.EndMaxPosLimit4.ToString();
                case "EndMinPos4": return Ether_Addr.EndMinPosLimit4.ToString();

                case "Mode5": return Ether_Addr.Mode5.ToString();
                case "PressPos5": return Ether_Addr.PressPos5.ToString();
                case "PressForce5": return Ether_Addr.PressForce5.ToString();
                case "PressVel5": return Ether_Addr.PressVel5.ToString();
                case "PressTime5": return Ether_Addr.PressTime5.ToString();
                case "DynamicLimit5": return Ether_Addr.LimitType5.ToString();
                case "BeginMaxForce5": return Ether_Addr.BeginMaxForceLimit5.ToString();
                case "BeginMinForce5": return Ether_Addr.BeginMinForceLimit5.ToString();
                case "EndMaxForce5": return Ether_Addr.EndMaxForceLimit5.ToString();
                case "EndMinForce5": return Ether_Addr.EndMinForceLimit5.ToString();
                case "EndMaxPos5": return Ether_Addr.EndMaxPosLimit5.ToString();
                case "EndMinPos5": return Ether_Addr.EndMinPosLimit5.ToString();

                case "Mode6": return Ether_Addr.Mode6.ToString();
                case "PressPos6": return Ether_Addr.PressPos6.ToString();
                case "PressForce6": return Ether_Addr.PressForce6.ToString();
                case "PressVel6": return Ether_Addr.PressVel6.ToString();
                case "PressTime6": return Ether_Addr.PressTime6.ToString();
                case "DynamicLimit6": return Ether_Addr.LimitType6.ToString();
                case "BeginMaxForce6": return Ether_Addr.BeginMaxForceLimit6.ToString();
                case "BeginMinForce6": return Ether_Addr.BeginMinForceLimit6.ToString();
                case "EndMaxForce6": return Ether_Addr.EndMaxForceLimit6.ToString();
                case "EndMinForce6": return Ether_Addr.EndMinForceLimit6.ToString();
                case "EndMaxPos6": return Ether_Addr.EndMaxPosLimit6.ToString();
                case "EndMinPos6": return Ether_Addr.EndMinPosLimit6.ToString();

                case "Mode7": return Ether_Addr.Mode7.ToString();
                case "PressPos7": return Ether_Addr.PressPos7.ToString();
                case "PressForce7": return Ether_Addr.PressForce7.ToString();
                case "PressVel7": return Ether_Addr.PressVel7.ToString();
                case "PressTime7": return Ether_Addr.PressTime7.ToString();
                case "DynamicLimit7": return Ether_Addr.LimitType7.ToString();
                case "BeginMaxForce7": return Ether_Addr.BeginMaxForceLimit7.ToString();
                case "BeginMinForce7": return Ether_Addr.BeginMinForceLimit7.ToString();
                case "EndMaxForce7": return Ether_Addr.EndMaxForceLimit7.ToString();
                case "EndMinForce7": return Ether_Addr.EndMinForceLimit7.ToString();
                case "EndMaxPos7": return Ether_Addr.EndMaxPosLimit7.ToString();
                case "EndMinPos7": return Ether_Addr.EndMinPosLimit7.ToString();

                case "Mode8": return Ether_Addr.Mode8.ToString();
                case "PressPos8": return Ether_Addr.PressPos8.ToString();
                case "PressForce8": return Ether_Addr.PressForce8.ToString();
                case "PressVel8": return Ether_Addr.PressVel8.ToString();
                case "PressTime8": return Ether_Addr.PressTime8.ToString();
                case "DynamicLimit8": return Ether_Addr.LimitType8.ToString();
                case "BeginMaxForce8": return Ether_Addr.BeginMaxForceLimit8.ToString();
                case "BeginMinForce8": return Ether_Addr.BeginMinForceLimit8.ToString();
                case "EndMaxForce8": return Ether_Addr.EndMaxForceLimit8.ToString();
                case "EndMinForce8": return Ether_Addr.EndMinForceLimit8.ToString();
                case "EndMaxPos8": return Ether_Addr.EndMaxPosLimit8.ToString();
                case "EndMinPos8": return Ether_Addr.EndMinPosLimit8.ToString();

                case "Mode9": return Ether_Addr.Mode9.ToString();
                case "PressPos9": return Ether_Addr.PressPos9.ToString();
                case "PressForce9": return Ether_Addr.PressForce9.ToString();
                case "PressVel9": return Ether_Addr.PressVel9.ToString();
                case "PressTime9": return Ether_Addr.PressTime9.ToString();
                case "DynamicLimit9": return Ether_Addr.LimitType9.ToString();
                case "BeginMaxForce9": return Ether_Addr.BeginMaxForceLimit9.ToString();
                case "BeginMinForce9": return Ether_Addr.BeginMinForceLimit9.ToString();
                case "EndMaxForce9": return Ether_Addr.EndMaxForceLimit9.ToString();
                case "EndMinForce9": return Ether_Addr.EndMinForceLimit9.ToString();
                case "EndMaxPos9": return Ether_Addr.EndMaxPosLimit9.ToString();
                case "EndMinPos9": return Ether_Addr.EndMinPosLimit9.ToString();

                case "Mode10": return Ether_Addr.Mode10.ToString();
                case "PressPos10": return Ether_Addr.PressPos10.ToString();
                case "PressForce10": return Ether_Addr.PressForce10.ToString();
                case "PressVel10": return Ether_Addr.PressVel10.ToString();
                case "PressTime10": return Ether_Addr.PressTime10.ToString();
                case "DynamicLimit10": return Ether_Addr.LimitType10.ToString();
                case "BeginMaxForce10": return Ether_Addr.BeginMaxForceLimit10.ToString();
                case "BeginMinForce10": return Ether_Addr.BeginMinForceLimit10.ToString();
                case "EndMaxForce10": return Ether_Addr.EndMaxForceLimit10.ToString();
                case "EndMinForce10": return Ether_Addr.EndMinForceLimit10.ToString();
                case "EndMaxPos10": return Ether_Addr.EndMaxPosLimit10.ToString();
                case "EndMinPos10": return Ether_Addr.EndMinPosLimit10.ToString();

                default:
                    return "";
            }
        }

        public void WriteRecipe(string name,double value)
        {
            mbc.Write_PLC(RecipeAddress(name), Convert.ToString(value));
        }

        public void WriteProductAmount(long number) 
        {
            mbc.Write_PLC(Ether_Addr.LiveProductAmount.ToString(), Convert.ToString(number));
        }

        public void WriteOKAmount(long number)
        {
            mbc.Write_PLC(Ether_Addr.LiveOKAmount.ToString(), Convert.ToString(number));
        }

        public void WriteNGAmount(long number)
        {
            mbc.Write_PLC(Ether_Addr.LiveNGAmount.ToString(), Convert.ToString(number));
        }


        public void Initialize() 
        {
            mbc.Write_PLC_SingleCoil(Ether_Addr.Initialization.ToString(), 1);
        }

        public void TurnOn()
        {
            mbc.Write_PLC_SingleCoil(Ether_Addr.TurnOnStatus.ToString(), 1);
        }

        public void TurnOff() 
        {
            mbc.Write_PLC_SingleCoil(Ether_Addr.TurnOnStatus.ToString(), 0);
        }

        public int TurnOnStatus() 
        {
            return mbc.Read_PLC_SingleCoil(Ether_Addr.TurnOnStatus.ToString());
        }


        public string RecipeName
        {
            get 
            {
                return getString_Name(RecipeName1, RecipeName2, RecipeName3, RecipeName4, RecipeName5, RecipeName6, RecipeName7);
            }
        }

        private string getString_Name(long s1, long s2, long s3, long s4, long s5, long s6, long s7)
        {
            string strFinalOutput = "";

            string[] sInput = new string[8];
            sInput[1] = Convert.ToString(s1, 2);
            sInput[2] = Convert.ToString(s2, 2);
            sInput[3] = Convert.ToString(s3, 2);
            sInput[4] = Convert.ToString(s4, 2);
            sInput[5] = Convert.ToString(s5, 2);
            sInput[6] = Convert.ToString(s6, 2);
            sInput[7] = Convert.ToString(s7, 2);

            for (int i = 1; i <= 7; i++)
            {
                if (sInput[i] != "0")
                {
                    strFinalOutput += recipe_Char(sInput[i]);   //二進位轉換成字串，並組合成字串
                }
            }

            if (strFinalOutput == "")
            {
                strFinalOutput = "-";
            }

            return strFinalOutput;
        }

        private string recipe_Char(string input)  //Seperate the bin on D register into two word and then export the string of these two words 二進位轉換成字串，並組合成字串
        {
            string output_1 = "";
            string output_2 = "";
            int iStringLength = input.Length;
            if (iStringLength == 14)
            {
                output_1 = Convert.ToString(Convert.ToChar(Convert.ToInt32(input.Substring(7, 7), 2)));
                output_2 = Convert.ToString(Convert.ToChar(Convert.ToInt32(input.Substring(0, 6), 2)));
            }
            else if (iStringLength == 15)
            {
                output_1 = Convert.ToString(Convert.ToChar(Convert.ToInt32(input.Substring(8, 7), 2)));
                output_2 = Convert.ToString(Convert.ToChar(Convert.ToInt32(input.Substring(0, 7), 2)));
            }
            else if (iStringLength == 7)
            {
                output_1 = Convert.ToString(Convert.ToChar(Convert.ToInt32(input.Substring(0, 7), 2)));
            }
            else if (iStringLength == 6)
            {
                output_1 = Convert.ToString(Convert.ToChar(Convert.ToInt32(input.Substring(0, 6), 2)));
            }
            return output_1 + output_2;
        }


        public string LiveMessage
        {
            get 
            {
                if (LiveStatus == 0)
                {
                    return rM.GetString("PutOn");
                }
                else if (LiveStatus == 1)
                {
                    return rM.GetString("Down");
                }
                else if (LiveStatus == 2)
                {
                    return rM.GetString("AtIni");
                }
                else if (LiveStatus == 3)
                {
                    return rM.GetString("Pressing");
                }
                else if (LiveStatus == 4)
                {
                    return rM.GetString("GoHome");
                }
                else if (LiveStatus == 5)
                {
                    return rM.GetString("NG");
                }
                else if (LiveStatus == 6)
                {
                    return rM.GetString("NG");
                }
                else if (LiveStatus == 7)
                {
                    return rM.GetString("NG");
                }
                else if (LiveStatus == 8)
                {
                    return rM.GetString("NG");
                }
                else if (LiveStatus == 9)
                {
                    return rM.GetString("ExceedForceLimit");
                }
                else if (LiveStatus == 10)
                {
                    return rM.GetString("ExceedPosLimit");
                }
                return " ";

            }
        }

    }
}
