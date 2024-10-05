using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public partial class Register : BufferRead
    {
        public Buffer bufferData = new Buffer();
        public int ibufferIndex = 0;


        private static class CONSTANT
        {
            public const int Position_Section1 = 7050;
            public const int Force_Section1 = 7500;
            public const int Velocity_Section1 = 8000;
            public const int Time_Section1 = 8500;

            public const int Position_Section2 = 9000;
            public const int Force_Section2 = 9500;
            public const int Velocity_Section2 = 1000;
            public const int Time_Section2 = 1500;

            public const int OneBufferSize = 400;
            public const int OnceReadNum = 100;

        }


        public class Buffer
        {
            internal int[,] iPositionTmp = new int[(CONSTANT.OneBufferSize / CONSTANT.OnceReadNum), CONSTANT.OnceReadNum];
            internal int[,] iForceTmp = new int[(CONSTANT.OneBufferSize / CONSTANT.OnceReadNum), CONSTANT.OnceReadNum];
            internal int[,] iVelocityTmp = new int[(CONSTANT.OneBufferSize / CONSTANT.OnceReadNum), CONSTANT.OnceReadNum];
            internal int[,] iTimeTmp = new int[(CONSTANT.OneBufferSize / CONSTANT.OnceReadNum), CONSTANT.OnceReadNum];

            public int[] iPosition = new int[CONSTANT.OneBufferSize / 2];
            public int[] iForce = new int[CONSTANT.OneBufferSize / 2];
            public int[] iVelocity = new int[CONSTANT.OneBufferSize / 2];
            public int[] iTime = new int[CONSTANT.OneBufferSize / 2];

        }

        public int getOneBufferSize()
        {
            return CONSTANT.OneBufferSize;
        }

        public int getPosition(int iNum)
        {
            return bufferData.iPosition[iNum];
        }

        public int getForce(int iNum)
        {
            return bufferData.iForce[iNum];
        }

        public int getVelocity(int iNum)
        {
            return bufferData.iVelocity[iNum];
        }

        public int getTime(int iNum)
        {
            return bufferData.iTime[iNum];
        }
        public int[] tempProcess(int[] iTarget, int[,] iTemp, string sType)
        {
            int iTempIndex = 0;
            for (int i = 0; i < (CONSTANT.OneBufferSize / 2); i++)
            {
                //if (sType == "")
                //{
                    iTarget[i] = ConvertTo32(iTemp[(int)Math.Floor((double)i / ((CONSTANT.OneBufferSize / 2) / (CONSTANT.OneBufferSize / CONSTANT.OnceReadNum))), iTempIndex], iTemp[(int)Math.Floor((double)i / ((CONSTANT.OneBufferSize / 2) / (CONSTANT.OneBufferSize / CONSTANT.OnceReadNum))), iTempIndex + 1]);
                    iTempIndex = iTempIndex + 2;
                //}
                //else if (sType == "Time")
                //{
                //    iTarget[i] = iTemp[(int)Math.Floor((double)i / ((CONSTANT.OneBufferSize / 2) / ((CONSTANT.OneBufferSize / 2) / CONSTANT.OnceReadNum))), iTempIndex];

                //    iTempIndex++;
                //}
                if (iTempIndex == 100)
                {
                    iTempIndex = 0;
                }
            }
            return iTarget;
        }


        public Register(string sIP, int iPort)
        {
            IPAddress = sIP;
            Port = iPort;

            try
            {
                Connect();
            }
            catch (Exception)
            {

            }
            Buffer[] buffer = new Buffer[2];
            ibufferIndex = 0;

        }

        public int[,] Read_Register(int[,] iData, int iStartRegister, string sType)
        {
            int[] iReadTemp = new int[CONSTANT.OnceReadNum];
            int iBufferSize = 0;


            //if (sType == "")
            //{
                iBufferSize = CONSTANT.OneBufferSize;
            //}
            //else if (sType == "Time")
            //{
            //   iBufferSize = (int)Math.Floor((double)CONSTANT.OneBufferSize / 2);
            //}

            for (int i = 0; i < (iBufferSize / CONSTANT.OnceReadNum); i++)
            {
                iReadTemp = Read_PLC_Multiple("D" + Convert.ToString(iStartRegister + (CONSTANT.OnceReadNum * i)), CONSTANT.OnceReadNum);
                for (int j = 0; j < CONSTANT.OnceReadNum; j++)
                {
                    iData[i, j] = iReadTemp[j];
                }
            }
            return iData;
        }


        public Buffer Read(int iSection)
        {
            if (iSection == 1)
            {
                bufferData.iPosition = tempProcess(bufferData.iPosition, Read_Register(bufferData.iPositionTmp, CONSTANT.Position_Section1, ""), "");
                bufferData.iForce = tempProcess(bufferData.iForce, Read_Register(bufferData.iForceTmp, CONSTANT.Force_Section1, ""), "");
                bufferData.iVelocity = tempProcess(bufferData.iVelocity, Read_Register(bufferData.iVelocityTmp, CONSTANT.Velocity_Section1, ""), "");
                bufferData.iTime = tempProcess(bufferData.iTime, Read_Register(bufferData.iTimeTmp, CONSTANT.Time_Section1, "Time"), "Time");
            }
            else if (iSection == 2)
            {
                bufferData.iPosition = tempProcess(bufferData.iPosition, Read_Register(bufferData.iPositionTmp, CONSTANT.Position_Section2, ""), "");
                bufferData.iForce = tempProcess(bufferData.iForce, Read_Register(bufferData.iForceTmp, CONSTANT.Force_Section2, ""), "");
                bufferData.iVelocity = tempProcess(bufferData.iVelocity, Read_Register(bufferData.iVelocityTmp, CONSTANT.Velocity_Section2, ""), "");
                bufferData.iTime = tempProcess(bufferData.iTime, Read_Register(bufferData.iTimeTmp, CONSTANT.Time_Section2, "Time"), "Time");
            }
            return bufferData;
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
    }
}
