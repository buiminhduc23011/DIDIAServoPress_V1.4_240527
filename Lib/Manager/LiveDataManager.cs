using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class LiveDataManager
    {
       
        public LiveData[] LiveData = new LiveData[CONSTANT.LiveDataAmount];   // Record actual value of live press steps 單次壓合資訊紀錄
        public int LiveDataNum = 0;      //The current live data number         目前單次壓合編號
        private PLC plc;


        public LiveDataManager(PLC plc) 
        {
            this.plc = plc;
        }
        
        //Build the first live data of the current motion step by reading from the PLC after connection 建立即時記錄物件
        public void New_LiveData(string barcode,string recipeName)
        {
            LiveData[LiveDataNum] = new LiveData();
            LiveData[LiveDataNum].BarCode = barcode;
            LiveData[LiveDataNum].RecipeName = recipeName;
            LiveData[LiveDataNum].StartTime = DateTime.Now.ToString("T");
            LiveData[LiveDataNum].StartDate = DateTime.Now.ToShortDateString();

            LiveData[LiveDataNum].StandbyPos = plc.TargetStandbyPos;
            LiveData[LiveDataNum].StandbyTime = plc.LiveStandbyTime;
            LiveData[LiveDataNum].PressTime = plc.LivePressTime;

        }
        
        public void LiveData_SaveSingleTime(double singleTime) 
        {
            LiveData[LiveDataNum].SingleTime = singleTime;
        } 

        public void LiveData_Save_AfterPress()
        {
            LiveData[LiveDataNum].PressPos = plc.PressedPos;
            LiveData[LiveDataNum].PressForce = plc.PressedForce;
            LiveData[LiveDataNum].StandbyTime = plc.LiveStandbyTime;
            LiveData[LiveDataNum].PressTime = plc.LivePressTime;
            
            if (plc.LiveStatus >= 5)
            {
                LiveData[LiveDataNum].Result = plc.LiveStatus;
            }
        }

        public void LiveData_SaveLiveDataNum() 
        {
            LiveData[LiveDataNum].DetailNum = LiveDataNum;
        }

        public void LiveDataNum_Add() 
        {
            LiveDataNum++;
        }

        public void LiveData_Save_StaticValue(int step, string cpk) 
        {
            if (step>=0 && LiveData[LiveDataNum].StatsValue[step] == 0 )
            {
                plc.NomalUpdate();

                if ((step >= 0) && (cpk == "Force"))
                {
                    LiveData[LiveDataNum].StatsValue[step] = plc.PressedForce;
                }
                else if ((step >= 0) && (cpk == "Position"))
                {
                    LiveData[LiveDataNum].StatsValue[step] = plc.PressedPos;
                }
            }
        }

    }
}
