using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DIAServoPress
{
    public enum A2_SCOPE_ITEM_INDEX
    {
        /// <summary>回授位置 [PUU]</summary>
        FEEDBACK_POS = 0,

        /// <summary>命令位置 [PUU]</summary>
        COMMAND_POS = 1,

        /// <summary>位置誤差 [PUU]</summary>
        ERROR_POS = 2,

        /// <summary>脈波命令頻率 [kHz]</summary>
        PULSE_COMMAND_FRREQENCY = 6,

        /// <summary>速度命令：電壓 [Volt]</summary>
        SPEED_COMMAND_VOLT = 8,

        /// <summary>速度命令：轉速 [r/min]</summary>
        SPEED_COMMAND_RPM = 0x32,

        /// <summary>馬達速度：即時 [r/min]</summary>
        MOTOR_SPEED_RPM = 0x33,

        /// <summary>馬達速度：濾波 [r/min]</summary>
        MOTOR_SPEED_LOW_PASS = 0x34,

        /// <summary>電流命令：電壓 [volt]</summary>
        TORQUE_COMMAND_VOLT = 10,

        /// <summary>電流命令：電壓 [volt]</summary>
        TORQUE_COMMAND_PERCENTAGE = 0x35,

        /// <summary>馬達電流：百分比 [%]</summary>
        MOTOR_CURRENT_PERCENTAGE = 0x36,

        /// <summary>馬達電流：安培 [Amp]</summary>
        MOTOR_CURRENT_AMP = 0x37,

        /// <summary>DI 狀態</summary>
        DI_STATUS = 0x27,

        /// <summary>DO 狀態</summary>
        DO_STATUS = 0x28,

        /// <summary>V Bus 電壓 [Volt]</summary>
        V_BUS_VOLT = 0x38,

        /// <summary>負載慣量比 [x倍]</summary>
        J_L_RATIO = 0x39,

        /// <summary>Z相偏移位置 [0 ~ 9999]</summary>
        DIFF_Z_PULSE = 0x12,

        /// <summary>平均負載率 [%]</summary>
        AVERAGE_LOAD_RATE = 12,

        /// <summary>映射參數內容 -1</summary>
        MAPPING_1 = 0x13,

        /// <summary>映射參數內容 -2</summary>
        MAPPING_2 = 0x14,

        /// <summary>映射參數內容 -3</summary>
        MAPPING_3 = 0x15,

        /// <summary>映射參數內容 -4</summary>
        MAPPING_4 = 0x16,

        /// <summary>監視變數內容 #1</summary>
        MONITOR_1 = 0x17,

        /// <summary>監視變數內容 #2</summary>
        MONITOR_2 = 0x18,

        /// <summary>監視變數內容 #3</summary>
        MONITOR_3 = 0x19,

        /// <summary>監視變數內容 #4</summary>
        MONITOR_4 = 0x1A,

        /// <summary>PR命令路徑索引</summary>
        PR_CMD_PATH = 0x2A,

        /// <summary>過負載(AL006)保護計數</summary>
        AL006_OVERLOAD = 0x5B7F,

        /// <summary>回生異常(AL005)保護計數</summary>
        AL005_REGENERATION = 0x6F7F,
    }
    public enum A3_SCOPE_ITEM_INDEX
    {
        A3_FEEDBACK_POS = 0,
        A3_COMMAND_POS = 1,
        A3_ERROR_POS = 2,
        A3_PULSE_COMMAND_FRREQENCY = 6,
        A3_SPEED_COMMAND_VOLT = 8,
        A3_SPEED_COMMAND_RPM = 0x32,
        A3_MOTOR_SPEED_RPM = 0x33,
        A3_MOTOR_SPEED_LOW_PASS = 0x34,
        A3_TORQUE_COMMAND_VOLT = 10,
        A3_TORQUE_COMMAND_PERCENTAGE = 0x35,
        A3_MOTOR_CURRENT_PERCENTAGE = 0x36,
        A3_MOTOR_CURRENT_AMP = 0x37,
        A3_DI_STATUS = 0x27,
        A3_DO_STATUS = 0x28,
        A3_V_BUS_VOLT = 0x38,
        A3_J_L_RATIO = 0x39,
        A3_DIFF_Z_PULSE = 0x12,
        A3_AVERAGE_LOAD_RATE = 12,
        A3_MAPPING_1 = 0x13,
        A3_MAPPING_2 = 0x14,
        A3_MAPPING_3 = 0x15,
        A3_MAPPING_4 = 0x16,
        A3_MONITOR_1 = 0x17,
        A3_MONITOR_2 = 0x18,
        A3_MONITOR_3 = 0x19,
        A3_MONITOR_4 = 0x1A,
        A3_PR_CMD_PATH = 0x2A,
        A3_AL006_OVERLOAD = 0x5B7F,
        A3_AL005_REGENERATION = 0x6F7F,

    };
    public enum M_SCOPE_ITEM_INDEX
    {
        M_FEEDBACK_POS = 0,
        M_COMMAND_POS = 1,
        M_ERROR_POS = 2,
        M_PULSE_COMMAND_FRREQENCY = 6,
        M_SPEED_COMMAND_VOLT = 8,
        M_SPEED_COMMAND_RPM = 0x32,
        M_MOTOR_SPEED_RPM = 0x33,
        M_MOTOR_SPEED_LOW_PASS = 0x34,
        M_TORQUE_COMMAND_VOLT = 10,
        M_TORQUE_COMMAND_PERCENTAGE = 0x35,
        M_MOTOR_CURRENT_PERCENTAGE = 0x36,
        M_MOTOR_CURRENT_AMP = 0x37,
        M_DI_STATUS = 0x27,
        M_DO_STATUS = 0x28,
        M_V_BUS_VOLT = 0x38,
        M_J_L_RATIO = 0x39,
        M_DIFF_Z_PULSE = 0x12,
        M_AVERAGE_LOAD_RATE = 12,
        M_MAPPING_1 = 0x13,
        M_MAPPING_2 = 0x14,
        M_MAPPING_3 = 0x15,
        M_MAPPING_4 = 0x16,
        M_MONITOR_1 = 0x17,
        M_MONITOR_2 = 0x18,
        M_MONITOR_3 = 0x19,
        M_MONITOR_4 = 0x1A,
        M_PR_CMD_PATH = 0x2A,
        M_AL006_OVERLOAD = 0x5B7F,
        M_AL005_REGENERATION = 0x6F7F,

    };
    // Scope CH Item type
    public enum CHITEM_TYPE
    {
        CHNOR = 0,  // normal item idx
        CHADR = 1,   // assign address
        CHVAR = 2,  // variable: 0~127
        CHPAR = 3,  // Parameter :
        CHCAN = 4,  // CANopen object
        CHUSERARRAY = 5,
        CHSYSVAR = 6,
        CHEXT = 10, // extra  item idx
        CH_AXI_MS = 21,     //MS-Normal
        CH_GRP_MS = 22,     //MS-Group
        CH_ADR_MS = 23,     //MS-Address
        CH_PLC_MS = 24      //MS-PLCMB3

    };
    /// <summary>
    /// 【ASDA Scope】監控頻率
    /// </summary>
    /// <remarks>
    /// <br /><br /><br />
    /// <h3><b>改版紀錄</b></h3>
    /// Created by YENYUN – 2019.10.21
    /// </remarks>
    public enum ESCOPE_TYPE
    {
        ASDA_NONE = -1,
        // ASDA-A Series
        ASDA_SCOPE = 0,
        A_CONTINUE_SCP = 1,
        A_TRIGGER_SCP = 2,
        AP_CONTINUE_SCP = 3,
        // ASDA-B Series
        B_CONTINUE_SCP = 101,
        // ASDA-A2 Series
        A2_CONTINUE_SCP = 201,  // Scope_Define.h註解: old version scope HighBaud-8K, and LowBaud-1K (DSP Ver<= 0..35)
        // Scope_Define.h註解: Low/high 由pScpCom->nH_L_Baud_old 0/1決定
        A2_CONTINUE_SCP_8K = 202,  //-- new ver(v036(含)  以上) High-rate 8k scp
        A2_CONTINUE_SCP_16K = 203,  //-- new ver(v036(含)  以上) Hig-rate 16k scp
        A2_CONTINUE_SCP_L = 211,   //-- new ver(V036(含)  以上) Low-rate scp
        // ASDA-B2 Series
        B2_CONTINUE_SCP_2K = 302,
        B2_CONTINUE_SCP_4K = 303,
        // ASDA-M Series
        M_CONTINUE_SCP_8K = 402,
        M_CONTINUE_SCP_16K = 403,
        M_CONTINUE_SCP_4K = 404,  //-- For 4CH_4K_32Bit
        M_CONTINUE_SCP_2K = 405,  // For 8CH_2K_32Bit // 20120827 add 2k
        M_CONTINUE_SCP_L = 411,
        A3_CONTINUE_SCP_8K = 501,
        A3_CONTINUE_SCP_16K = 502,
        A3_CONTINUE_SCP_10K = 503,
        A3_CONTINUE_SCP_20K = 504,
        A3_CONTINUE_SCP_L = 505,

        MS_SCP_8K = 908,
        MS_SCP_16K = 910,

        DXMC_SCP_05K = 920,
        DXMC_SCP_1K = 921,
        DXMC_SCP_2K = 922,
        DXMC_SCP_4K = 923,
        DXMC_SCP_8K = 924,
        DXMC_SCP_16K = 925,
    }
    public class A3_Usb_Link
    {

        public const string ASD_SCOPE_DLL_FILE = "ASD_SCOPE.dll"; // DLL引用路徑(與*.exe同路徑)

        [DllImport(ASD_SCOPE_DLL_FILE, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_IP_To_PortID(char[] strIP);

        [DllImport(ASD_SCOPE_DLL_FILE, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_StartDLL();

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_StopDLL", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_StopDLL();

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Allocate_Scope", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Allocate_Scope(int nPort);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Free_Scope", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Free_Scope(int nPort);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Open_COMM_HCT", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Open_COMM_HCT(int nPort);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Open_COMM_HCTL", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Open_COMM_HCTL(int nPort);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Open_COMM_HCTL_E3L", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Open_COMM_HCTL_B3B(int nPort);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Open_COMM_MODBUS", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Open_COMM_MODBUS(int nPort, int nBaud, int nProtocol);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Open_COMM_TCP", CallingConvention = CallingConvention.Cdecl)]
        // public static extern bool ASD_Open_COMM_TCP(int nPort);
        public static extern bool ASD_Open_COMM_TCP(char[] server_ip, int server_port, char[] client_ip, int client_port);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Close_COMM", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Close_COMM(int nPort);


        //[DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_Para_N", CallingConvention = CallingConvention.Cdecl)]
        //public static extern bool ASD_Read_Para_N(int nPort, int nstation, int nAxis, int nGID, int nPID, int nReadSizeOfInt, int[] Val);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_ReadDevInf", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_ReadDevInf(int nPort);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Set_Device_ID", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Set_Device_ID(int nPort, int dev0, int dev1, int dev2);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Set_Station", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Set_Station(int nPort, int station);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Scope_Run", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Scope_Run(int nPort);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Scope_Write_Settings", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Scope_Write_Settings(int nPort);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Scope_Reset_Before_Run", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Scope_Reset_Before_Run(int nPort);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Scope_Write_Run_CMD", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Scope_Write_Run_CMD(int nPort);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Scope_Stop", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Scope_Stop(int nPort);



        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetChDataValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ASD_GetChDataValue(int nComPortID, int ch_idx, int index);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetChDataValue_Back", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ASD_GetChDataValue_Back(int nComPortID, int ch_idx);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Set_ChData_Que_Pointer_Pos", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ASD_Set_ChData_Que_Pointer_Pos(int nComPortID, int ch_idx, int pos);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetChDataValue_Auto_Next", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ASD_GetChDataValue_Auto_Next(int nComPortID, int ch_idx);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_ChDataValue_Unvisited_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_Get_ChDataValue_Unvisited_Size(int nComPortID, int ch_idx_based);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_ChDataValue_N_Auto_Next", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_Get_ChDataValue_N_Auto_Next(int nComPortID, int ch_idx, int get_size, double[] pVal);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "Get_ChDataValue_Unvisited_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Get_ChDataValue_Unvisited_Size(int nComPortID, int ch_idx_based);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_ChDataValue_Fix_N_Auto_Next", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Get_ChDataValue_Fix_N_Auto_Next(int nComPortID, int ch_idx, int get_fix_size, double[] pVal);

        //---SETTING FUNCTION
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Set_Scope_Type", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Set_Scope_Type(int nComPortID, ESCOPE_TYPE nScopeType);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Set_Scope_Type_And_BufferFactor", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Set_Scope_Type_And_BufferFactor(int nComPortID, ESCOPE_TYPE nScopeType, int nDataBufferSizeFactor);
        //[DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Assign_App_HWND_MSG", CallingConvention = CallingConvention.Cdecl)]
        //public static extern  void ASD_Assign_App_HWND_MSG(int nComPortID, HWND h, UINT msg);
        //[DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Assign_Scp_HWND_MSG", CallingConvention = CallingConvention.Cdecl)]
        //public static extern  void ASD_Assign_Scp_HWND_MSG(int nComPortID, HWND h, UINT msg);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetChMonItem_A2", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetChMonItem_A2(int nComPortID, int ch_idx, A2_SCOPE_ITEM_INDEX mon_item);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetChMonItem_A3", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetChMonItem_A3(int nComPortID, int ch_idx, A3_SCOPE_ITEM_INDEX mon_item);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetChMonItem_GA3", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetChMonItem_GA3(int nComPortID, int ch_idx, A3_SCOPE_ITEM_INDEX mon_item);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetChMonItem_M", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetChMonItem_M(int nComPortID, int ch_idx, M_SCOPE_ITEM_INDEX mon_item);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetCH_MonVal", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetCH_MonVal(int nComPortID, int ch_idx, int Axis, CHITEM_TYPE MonType, int monVal);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetCH_MonVal_2Val", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetCH_MonVal_2Val(int nComPortID, int ch_idx, int Axis, CHITEM_TYPE MonType, int nVal1, int nVal2);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetCH_MonVal_2Val", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_GetCH_Data_Size(int nComPortID, int ch_idx);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetCH_MonVal_2Val", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetCHVal(int nComPortID, int ch_idx, int Axis, CHITEM_TYPE MonType, int monVal);  //請改用ASD_SetCH_MonVal()
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_SetCH_MonVal_2Val", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_SetCH2Val(int nComPortID, int ch_idx, int Axis, CHITEM_TYPE MonType, int nVal1, int nVal2);//請改用ASD_SetCH_MonVal_2Val()


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetChMonItemScale", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ASD_GetChMonItemScale(int nComPortID, int ch_idx, A2_SCOPE_ITEM_INDEX mon_item);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Load_Setup_From_MyScopeConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern bool ASD_Load_Setup_From_MyScopeConfig(int nComPortID, char[] pWFileName);



        // public static extern  bool SetScopeBufferFactor(int nComPortID, int nScpFactor);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetScopeBufferFactor", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_GetScopeBufferFactor(int nComPortID, ref int nScpFactor);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Set_ModbusTCP_Send_Timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ASD_Set_ModbusTCP_Send_Timeout(int nComPortID, int new_timeout);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_ModbusTCP_Send_Timeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ASD_Get_ModbusTCP_Send_Timeout(int nComPortID, int new_timeout);


        //---INFORMATION FUNCTION
        //bool IsInitOk();
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetScopeModelVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_GetScopeModelVersion(int nComPortID, char[] strVer, int size);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_IsReady", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_IsReady(int nComPortID);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_IsOnline", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_IsOnline(int nComPortID);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_Scope_Sample_Rate_Category", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Get_Scope_Sample_Rate_Category(int nComPortID, ref int nCategory);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_IsScopeRunning", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_IsScopeRunning(int nComPortID);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_Max_CH_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_Get_Max_CH_Size(int nComPortID, ESCOPE_TYPE nScopeType);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_Current_CH_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_Get_Current_CH_Size(int nComPortID);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_CH_Max_Min_Pure_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Get_CH_Max_Min_Pure_Value(int nComPortID, int ch_idx, ref int nMax, ref int nMin);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_Sample_Rate_HZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASD_Get_Sample_Rate_HZ(int nComPortID);


        // public static extern  const wchar_t * ASD_GetErrText(int nComPortID);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetErrText", CallingConvention = CallingConvention.Cdecl)]
        public static extern char[] ASD_GetErrText(int nComPortID);
        //[DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_GetErrWText", CallingConvention = CallingConvention.Cdecl)]
        //public static extern w_chart[] ASD_GetErrWText(int nComPortID);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Write_Para_N", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Write_Para_N(int nComPortID, int nstation, int nAxis, int nGID, int nPID, int nWriteSizeOfInt, int[] Val);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_Para_N", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_Para_N(int nComPortID, int nstation, int nAxis, int nGID, int nPID, int nReadSizeOfInt, int[] Val);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Write_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Write_Value(int nComPortID, short station, short addr, int count, short[] pVal);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_Value(int nComPortID, short station, short addr, int count, short[] pVal);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_PLC_Memory_Base_Address", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_PLC_Memory_Base_Address(int nComPortID, int[] pData);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Fetch_ALL_PLC_Memory_Base_Address", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Fetch_ALL_PLC_Memory_Base_Address(int nComPortID);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_PLC_Memory_Section_Base_Address", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Get_PLC_Memory_Section_Base_Address(int nComPortID, int nSectionID, int[] pData);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Get_PLC_Memory_Section_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Get_PLC_Memory_Section_Size(int nComPortID, int nSectionID, int[] pData);


        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Write_PLC_Bit", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Write_PLC_Bit(int nComPortID, byte area, ushort offset, int bit_num, short sData);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Write_PLC_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Write_PLC_Value(int nComPortID, byte area, ushort offset, byte size_short, short[] pData);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_PLC_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_PLC_Value(int nComPortID, byte area, ushort offset, byte size_short, short[] pData);

        //--讀取DH Word值
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_PLC_DH_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_PLC_DH_Value(int ComPort, short offset, short size_short, short[] pData);
        //--讀取DV Word值
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_PLC_DV_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_PLC_DV_Value(int ComPort, short offset, short size_short, short[] pData);
        //--讀取MH Word值
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_PLC_MH_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_PLC_MH_Value(int ComPort, short offset, short size_short, short[] pData);
        //--讀取MV Word值
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Read_PLC_MV_Value", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Read_PLC_MV_Value(int ComPort, short offset, short size_short, short[] pData);
        public static short[] pData;
        //public static extern bool ASD_Save_As_Scp_File(int nComPortID, std::wstring FileName);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Save_As_Scp_File_By_char_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Save_As_Scp_File_By_char_name(int nComPortID, char[] pFileName);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Save_As_Scp_File_By_wchar_name", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Save_As_Scp_File_By_wchar_name(int nComPortID, char[] pWFileName);
        //[DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Save_As_Scp_File_By_wchar_name_II", CallingConvention = CallingConvention.Cdecl)]
        //public static extern bool ASD_Save_As_Scp_File_By_wchar_name_II(int nComPortID, wchar_t[] pWFileName);

        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Enable_Hidden", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Enable_Hidden(int nComPortID);
        [DllImport(ASD_SCOPE_DLL_FILE, EntryPoint = "ASD_Disable_Hidden", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ASD_Disable_Hidden(int nComPortID);


        //--
    }
}
