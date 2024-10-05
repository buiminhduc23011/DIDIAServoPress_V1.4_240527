using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Windows.Forms;

namespace DIAServoPress
{
    public class USB
    {
        A3_Usb_Link Usb_Link = new A3_Usb_Link();
        private PLC plc = new PLC();

        private int ComPrtBoxID = 0;
        private int NextNumber = 0;
        public int nUnvisitSize = 0;
        public int TotalAmount = 0;

        private int PosAddr = 600;
        private int ForceAddr = 602;
        private int VelAddr = 604;
        private int TimeAddr = 618;

        private int Ch1_Address = 0;
        private int Ch2_Address = 0;
        private int Ch3_Address = 0;
        private int Ch4_Address = 0;

        public double[] ChartPosition = new double[1000000];
        public double[] ChartForce = new double[1000000];
        public double[] ChartVelocity = new double[1000000];
        public double[] ChartTime = new double[1000000];

        private double[][] pCH_Val = new double[16][];
        private short[] pData = new short[10];

        //每當要使用ASD_SCOPE DLL時，APP端只要起動DLL一次

        public USB(PLC _plc)
        {
            this.plc = _plc;
        }
        public string[] getPortNames()
        {
            return SerialPort.GetPortNames();   //直接取得所有port的名字
        }

        public bool Connect(int id)
        {
            ComPrtBoxID = id;
            A3_Usb_Link.ASD_StartDLL();
            try
            {
                if (A3_Usb_Link.ASD_Allocate_Scope(ComPrtBoxID) == true)
                {
                    if (A3_Usb_Link.ASD_Open_COMM_HCTL(ComPrtBoxID) == true)
                    {
                        if (A3_Usb_Link.ASD_ReadDevInf(ComPrtBoxID) == true)
                        {
                            if (A3_Usb_Link.ASD_Enable_Hidden(ComPrtBoxID))
                            {
                                if (A3_Usb_Link.ASD_Fetch_ALL_PLC_Memory_Base_Address(ComPrtBoxID))
                                {   /*查詢設定暫存器類型*/
                                    int[] nSetBaseAddr = new int[1];

                                    /*搜尋暫存器address*/
                                    if (A3_Usb_Link.ASD_Get_PLC_Memory_Section_Base_Address(ComPrtBoxID, 7, nSetBaseAddr))
                                    {
                                        Ch1_Address = nSetBaseAddr[0] + PosAddr;
                                        Ch2_Address = nSetBaseAddr[0] + ForceAddr;
                                        Ch3_Address = nSetBaseAddr[0] + VelAddr;
                                        Ch4_Address = nSetBaseAddr[0] + TimeAddr;
                                    }

                                    /*設定讀取兩筆DW資料暫存器位置*/
                                    if (A3_Usb_Link.ASD_Set_Scope_Type(ComPrtBoxID, ESCOPE_TYPE.A3_CONTINUE_SCP_8K))
                                    {
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 0, 1, CHITEM_TYPE.CHADR, Ch1_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 1, 1, CHITEM_TYPE.CHADR, Ch2_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 2, 1, CHITEM_TYPE.CHADR, Ch1_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 3, 1, CHITEM_TYPE.CHADR, Ch2_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 4, 1, CHITEM_TYPE.CHADR, Ch3_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 5, 1, CHITEM_TYPE.CHADR, Ch4_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 6, 1, CHITEM_TYPE.CHADR, Ch3_Address);
                                        A3_Usb_Link.ASD_SetCH_MonVal(ComPrtBoxID, 7, 1, CHITEM_TYPE.CHADR, Ch4_Address);

                                        A3_Usb_Link.ASD_Scope_Write_Settings(ComPrtBoxID);
                                    }
                                }
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void DisConnect()
        {
            A3_Usb_Link.ASD_Scope_Stop(ComPrtBoxID);
            A3_Usb_Link.ASD_Disable_Hidden(ComPrtBoxID); //關閉nComPortID的通訊，(只是關閉通訊，尚未釋放SCOPE資源配置)                                             
            A3_Usb_Link.ASD_Close_COMM(ComPrtBoxID);  //--應用程式結束時，取消資源配置，有兩種 Free_All_Port() 跟Free_Scope(nComPortID) 擇一使用即可
            A3_Usb_Link.ASD_Free_Scope(ComPrtBoxID);//釋放指定PORT的資源配置
            A3_Usb_Link.ASD_StopDLL();  //--每當要使用ASD_SCOPE DLL時，APP端只要起動DLL一次，並且再APP結束後，呼叫一次結束ASD_StopDLL();
        }

        public void Start()
        {
            //A3_Usb_Link.ASD_Scope_Run(ComPrtBoxID);
            ///A3_Usb_Link.ASD_Scope_Write_Settings(ComPrtBoxID);
            A3_Usb_Link.ASD_Scope_Reset_Before_Run(ComPrtBoxID);
            A3_Usb_Link.ASD_Scope_Write_Run_CMD(ComPrtBoxID);
            TotalAmount = 0;
            NextNumber = 0;
            pCH_Val[0] = new double[1000000];//40960
            pCH_Val[1] = new double[1000000];
            pCH_Val[2] = new double[1000000];
            pCH_Val[3] = new double[1000000];
        }

        public void Stop()
        {
            A3_Usb_Link.ASD_Scope_Stop(ComPrtBoxID);
        }

        public void Trigger()
        {
            A3_Usb_Link.ASD_Scope_Write_Run_CMD(ComPrtBoxID);
            TotalAmount = 0;
            NextNumber = 0;
            pCH_Val[0] = new double[1000000];
            pCH_Val[1] = new double[1000000];
            pCH_Val[2] = new double[1000000];
            pCH_Val[3] = new double[1000000];
        }

        public void End()
        {
            A3_Usb_Link.ASD_Scope_Stop(ComPrtBoxID);
            A3_Usb_Link.ASD_Scope_Reset_Before_Run(ComPrtBoxID);
        }

        public bool Scope_Time_Tick(int b)
        {
            nUnvisitSize = A3_Usb_Link.ASD_Get_ChDataValue_Unvisited_Size(ComPrtBoxID, 1);

            if (nUnvisitSize > 0)
            {
                NextNumber = nUnvisitSize;
                A3_Usb_Link.ASD_Get_ChDataValue_Fix_N_Auto_Next(ComPrtBoxID, 0, nUnvisitSize, pCH_Val[0]);
                A3_Usb_Link.ASD_Get_ChDataValue_Fix_N_Auto_Next(ComPrtBoxID, 1, nUnvisitSize, pCH_Val[1]);
                A3_Usb_Link.ASD_Get_ChDataValue_Fix_N_Auto_Next(ComPrtBoxID, 4, nUnvisitSize, pCH_Val[2]);
                A3_Usb_Link.ASD_Get_ChDataValue_Fix_N_Auto_Next(ComPrtBoxID, 5, nUnvisitSize, pCH_Val[3]);

                //NextNumber = (NextNumber < 5) ? NextNumber++ : 0;
            }
            else
            {
                return false;
            }

            if (b == 1)
            {
                double posTemp = 0;
                double forTemp = 0;

                for (int i = 0; i < NextNumber; i++)
                {
                    double position = Math.Round((pCH_Val[0][i] / 1000), 3, MidpointRounding.AwayFromZero);
                    double force = Math.Round((pCH_Val[1][i] / 10), 1, MidpointRounding.AwayFromZero);
                    double velocity = Math.Round((pCH_Val[2][i] / 1000), 3, MidpointRounding.AwayFromZero);
                    double time = Math.Round((pCH_Val[3][i] / 100), 2, MidpointRounding.AwayFromZero);

                    //if ((TotalAmount == 0) || (TotalAmount > 0 && position != posTemp && force != forTemp))
                    if ((TotalAmount == 0) || (TotalAmount > 0 && position != posTemp))
                    {
                        ChartPosition[TotalAmount] = position;

                        if (force < 5) force = 0;
                        if (plc.Unit == 0)
                        {
                            ChartForce[TotalAmount] = force;
                        }
                        else if (plc.Unit == 1)
                        {
                            ChartForce[TotalAmount] = force * 10;
                        }
                        else if (plc.Unit == 2)
                        {
                            ChartForce[TotalAmount] = force / 2;
                        }
                        posTemp = ChartPosition[TotalAmount];
                        forTemp = ChartForce[TotalAmount];

                        ChartVelocity[TotalAmount] = velocity;
                        ChartTime[TotalAmount] = time;
                        TotalAmount++;
                    }
                }
            }
            return true;
        }
    }
}
