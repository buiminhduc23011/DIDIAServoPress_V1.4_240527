using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using EasyModbus;

namespace DIAServoPress
{
    class EthernetClient_AS : ModbusClient
    {
        /// <summary>SE PLC WORD Device</summary>
        public enum SE_WordDevice : ushort
        {
            D = 0x0000,
        }

        /// <summary>SE PLC Bit Device</summary>
        public enum SE_BitDevice : ushort
        {
            M = 0x0000,
        }

        public EthernetClient_AS(string ipAddress, int port)
            : base(ipAddress, port)
        {
        }

        ~EthernetClient_AS()
        {
            Disconnect();
        }

        internal new bool Disconnect()
        {
            base.Disconnect();
            return true;
        }

        internal new bool Connect()
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
        public int[] Read_PLC_Multiple(string sDevice, int num)
        {
                int mbAddr = GetModbus_ADDR(sDevice, typeof(SE_WordDevice));
                return ReadHoldingRegisters(mbAddr, num);
        }


        //Read the single value at specfic register on PLC
        public long Read_PLC(string sDevice)
        {
            lock (_thisLock)
            {
                return ReadRegisterData<Int32>(sDevice);
            }

        }


        //Read the 16-bits D register from PLC   讀取PLC D暫存器
        public long Read_PLC_16(string sDevice)
        {
            lock (_thisLock)
            {
                return ReadRegisterData<Int16>(sDevice);
            }

        }

        //Write the 16-bits D register into PLC  寫入 PLC 16-bits D暫存器
        public void Write_PLC_16(string sDevice, string strPLC)
        {
            lock (_thisLock)
            {
                long regData;
                if (long.TryParse(strPLC, out regData) == true)
                {
                    WriteRegisterData<Int16>(sDevice, (Int16)(regData & UInt16.MaxValue));
                }
            }

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


        //Read the M or X Register from PLC    讀取PLC M,X暫存器
        public int Read_PLC_SingleCoil(string sDevice)
        {
            lock (_thisLock)
            {
                bool bState = Get_SingleCoil(sDevice);

                if (bState == true)
                {
                    return 1;
                }
                else if (bState == false)
                {
                    return 0;
                }
                return -1;
            }

        }


        //Write the M or X Register into  PLC    寫入PLC M,X暫存器
        public void Write_PLC_SingleCoil(string sDevice, int iStatus)
        {
            lock (_thisLock)
            {
                if (iStatus == 1)
                {
                    Set_SingleCoil(sDevice, true);
                }
                else if (iStatus == 0)
                {
                    Set_SingleCoil(sDevice, false);
                }
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
                regDatas = ReadHoldingRegisters(mbAddr, iSize); // Modbus Reading.
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
            return iAddr;
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

        public bool Get_SingleCoil(string sDevice)
        {
            int mbAddr = GetModbus_ADDR(sDevice, typeof(SE_BitDevice));

            try
            {
                bool[] bRead = ReadCoils(mbAddr, 1);
                return bRead[0];
            }
            catch (Exception)
            {

            }
            return false;
        }

        public void Set_SingleCoil(string sDevice, bool bOnOff)
        {
            int mbAddr = GetModbus_ADDR(sDevice, typeof(SE_BitDevice));
            try
            {
                WriteSingleCoil(mbAddr, bOnOff);
            }
            catch (Exception)
            {

            }
        }
    }
}
