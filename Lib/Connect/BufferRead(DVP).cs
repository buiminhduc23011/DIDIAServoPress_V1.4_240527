/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using EasyModbus;

namespace DIAServoPress
{
    public partial class BufferRead: ModbusClient
    {
        private static class CONSTANT
        {
            public const int regShift = 4096;
        }

        /// <summary>SE PLC WORD Device</summary>
        public enum SE_WordDevice : ushort
        {
            D = 0x9000,
        }

        /// <summary>SE PLC Bit Device</summary>
        public enum SE_BitDevice : ushort
        {
            M = 0x0800,
        }

        public BufferRead()
            : base()
        {
        }

        public BufferRead(string ipAddress, int port)
            : base(ipAddress, port)
        {
        }

        ~BufferRead()
        {
            Disconnect();
        }

        internal new void Disconnect()
        {
            if (base.Connected == true)
                base.Disconnect();
        }

        internal bool ReConnect()
        {
            bool bRt = false;

            try
            {
                Disconnect();
                base.Connect();
                bRt = Connected;
            }
            catch (Exception)
            {

            }
            return bRt;
        }

        private static object _thisLock = new object();

        //Read the single value at specfic register on PLC
        public int[] Read_PLC_Multiple(string sDev, int num)
        {
            lock (_thisLock)
            {
                    string[] tt = sDev.Split('.');
                    string sDevice = "";
                    string sPort = "";

                    for (int iSt = 0; iSt < tt[0].Length; iSt++)
                    {
                        if (char.IsNumber(tt[0][iSt]) == true)
                        {
                            sDevice = tt[0].Substring(0, iSt);
                            sPort = tt[0].Substring(iSt);
                            break;
                        }
                    }
                    int mbAddr = GetModbus_ADDR(sDevice + sPort, typeof(SE_WordDevice));
                    //return ReadHoldingRegisters(mbAddr, num);
                    return ReadRegister(mbAddr, num);
            }
        }

        private int[] ReadRegister(int mbAddr, int num) 
        {
            while (true) 
            {
                try
                {
                    return ReadHoldingRegisters(mbAddr, num);
                }
                catch (Exception)
                {
                    ReConnect();
                }
            }    
        }


        //Read the single value at specfic register on PLC
        public long Read_PLC(string sDevice)
        {
            lock (_thisLock)
            {
                return ReadRegisterData<Int32>(sDevice);
            }
        }


        public T ReadRegisterData<T>(string sDevice) where T : struct
        {
            int mbAddr = -1;
            T rtValue = default(T);

            mbAddr = GetModbus_ADDR(sDevice, typeof(SE_WordDevice));

            int[] regDatas = null;
            int iSize = System.Runtime.InteropServices.Marshal.SizeOf(rtValue) / sizeof(UInt16);    // 計算對應 WORD 數量

            try
            {
                //regDatas = ReadHoldingRegisters(mbAddr, iSize); // Modbus Reading.
                regDatas = ReadRegister(mbAddr, iSize); // Modbus Reading.
                TypeConverter ConvertKey = TypeDescriptor.GetConverter(typeof(T));   // 取得 T 類型的類型轉換子
                if (regDatas.Length == 4)
                    rtValue = (T)ConvertKey.ConvertTo(ModbusClient.ConvertRegistersToLong(regDatas), typeof(T));

                else if (regDatas.Length == 2)
                    rtValue = (T)ConvertKey.ConvertTo(ModbusClient.ConvertRegistersToInt(regDatas), typeof(T));
                else
                    rtValue = (T)ConvertKey.ConvertTo(regDatas[0], typeof(T));
            }
            catch (Exception)
            {

            }
            return rtValue;
        }

        private int GetModbus_ADDR(string sDev, Type tp)
        {
            int iAddr = -1;
            int iChNo = 0;
            string[] tt = sDev.Split('.');
            string sDevice = "";
            string sPort = "";

            for (int iSt = 0; iSt < tt[0].Length; iSt++)
            {
                if (char.IsNumber(tt[0][iSt]) == true)
                {
                    sDevice = tt[0].Substring(0, iSt);
                    sPort = tt[0].Substring(iSt);
                    break;
                }
            }

            if (tp == typeof(SE_BitDevice))
            {
                SE_BitDevice dBit;
                if (Enum.TryParse<SE_BitDevice>(sDevice, out dBit) == true)
                {
                    int iBitNo = 0;
                    if (int.TryParse(sPort, out iChNo) == true)
                    {
                        if (tt.Length == 1)
                            iAddr = (int)dBit + iChNo;
                        else if (int.TryParse(tt[1], out iBitNo) == true)
                            iAddr = (int)dBit + (iChNo << 4) + iBitNo;
                    }
                }
            }
            else if (tp == typeof(SE_WordDevice))
            {
                SE_WordDevice devNo;
                if (Enum.TryParse<SE_WordDevice>(sDevice, out devNo) && int.TryParse(sPort, out iChNo))
                    iAddr = (int)devNo + iChNo;
            }
            return iAddr - CONSTANT.regShift;
        }

        //Write the single value at specfic register on PLC
        public void Write_PLC(string sDevice, string strPLC)
        {
            lock (_thisLock)
            {
                long regData;
                if (long.TryParse(strPLC, out regData) == true)
                {
                    WriteRegisterData<int>(sDevice, (int)(regData & uint.MaxValue));
                }
            }
        }

        public void WriteRegisterData<T>(string sDevice, T tData) where T : struct
        {
            Type typ = tData.GetType();

            int mbAddr = GetModbus_ADDR(sDevice, typeof(SE_WordDevice));

            long dec;
            TypeConverter ConvertKey = TypeDescriptor.GetConverter(typeof(T));   // 取得 T 類型的類型轉換子
            long.TryParse(Convert.ToString(tData), out dec);    //   ConvertKey.ConvertFrom(tData)), out dec);
            byte[] bAr = BitConverter.GetBytes(dec);   // this.GetBytes(dec))//; decimal.Truncate(dec));   // decimal to bytes.
            int iSize = System.Runtime.InteropServices.Marshal.SizeOf(tData) / sizeof(ushort);
            int[] iDatas = new int[iSize];     //bAr.Length / sizeof(int)];

            for (int iLp = 0; iLp < iDatas.Length; iLp += 1)
                iDatas[iLp] = (int)BitConverter.ToInt16(bAr, iLp << 1);

            try
            {
                WriteMultipleRegisters(mbAddr, iDatas);  // 
            }
            catch (Exception)
            {

            }


        }
    }
}
*/