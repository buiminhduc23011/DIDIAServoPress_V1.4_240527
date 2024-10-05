using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace DIAServoPress
{
    public class ServoPressInfor
    {
        private PLC plc;

        public string Type = "";
        public string Unit = "";
        public int PositionMax = 0;
        public int ForceMax = 0;
        public int VelocityMax = 0;

        public int PressStatus = 0;       //The flag record whether or not the PC software had been recording 0: not recording 1:recording  軟體記錄狀態
        public int Recorded = 0;
        public int ConnectStatus = 0;


        private string tempPath="";                                             //The string recording the path for file saved  存檔路徑位址字串
        public string TempPath 
        {
            get 
            {
                return tempPath;
            }
        }



        private string startPath= Properties.Settings.Default.Path;
        public string StartPath
        {
            get 
            {
                return startPath;
            }
        }

        private string path;
        public string Path
        {
            get { return path; }
        }
        private string barcode = "E" + DateTime.Today.ToString("yyMMdd");
        public string Barcode
        {
            get 
            {
                if (plc.WorkOrder1 == 0 && plc.WorkOrder2 == 0) barcode = "E" + DateTime.Today.ToString("yyMMdd");
                else barcode = getString_Name(plc.WorkOrder1, plc.WorkOrder2, plc.WorkOrder3, plc.WorkOrder4, plc.WorkOrder5, plc.WorkOrder6, plc.WorkOrder7);
                return barcode; 
            }
        }

        private string startDateTime = DateTime.Now.ToString("hmms");
        public string StartDateTime
        {
            get 
            {
                return startDateTime;
            }
        }


        public int SaveFlag = 0;                                           //The flag recorded the status of the saved requirement  是否已存檔旗標
        public int recNum = 0;

        public ServoPressInfor(PLC plc) 
        {
            this.plc = plc;
            tempPath = Application.StartupPath + "\\Temp_" + plc.IPAddress;
        }

        public ServoPressInfor(PLC plc, string ip)
        {
            this.plc = plc;
            tempPath = Application.StartupPath + "\\Temp_" + ip;
        }

        public void setType_Max(string type,int positionMax,int forceMax) 
        {
            this.Type = type;
            this.PositionMax = positionMax;
            this.ForceMax = forceMax;

            if (forceMax < 2000)
            { 
               VelocityMax=270;
            }
            else if (forceMax >= 2000)
            { 
               VelocityMax =200;
            }
        }

        public string getUnit()
        {
            if (plc.Unit == 0)
            {
                Unit = "kgf";
            }
            else if (plc.Unit == 1)
            {
                Unit = "N";
            }
            else if (plc.Unit == 2)
            {
                Unit = "lbf";
            }
            else
            {
                Unit = "kgf";
            }
            return Unit;
        }

        public void ChangeStartDateTime() 
        {
            startDateTime= DateTime.Now.ToString("hmms");
        }

        public void Build_Path(string selectPath)
        {
            if (selectPath == "")
            {
               path = Application.StartupPath + "\\" + Barcode + "_" + startDateTime + "\\";
            }
            else
            {
               path = selectPath + "\\" + Barcode + "_" + startDateTime + "\\";
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


    }
}
