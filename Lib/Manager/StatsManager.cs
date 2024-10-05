using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class StatsManager
    {
        private MotionManager motionManager;
        private LiveDataManager liveDataManager;
        private PLC plc;

        public StatsManager(MotionManager motionManager, LiveDataManager liveDataManager,PLC plc) 
        {
            this.motionManager = motionManager;
            this.liveDataManager = liveDataManager;
            this.plc = plc;
        }

        public void Stat(StatsData[] statsData)
        {
            for (int step = 0; step <= motionManager.getLastMotionNum(); step++)  //最後一個會執行的步序1~5
            {
                statsData[step].Mode = motionManager.MotionData[step].Mode;

                if (statsData[step].Mode == 2)
                {
                    statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxPos);
                    statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinPos);
                    statsData[step].StatsValue = "Position";
                }
                else if (statsData[step].Mode == 1)
                {
                    if (plc.Unit == 0)
                    {
                        statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxForce);
                        statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinForce);
                    }
                    else if (plc.Unit == 1)
                    {
                        statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxForce)*10;
                        statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinForce)*10;
                    }
                    else if (plc.Unit == 2)
                    {
                        statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxForce) /2;
                        statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinForce) /2;
                    }
                    
                    
                    statsData[step].StatsValue = "Force";
                }
                else
                {
                    if (motionManager.MotionData[step].Cpk == "Force")    //Cpk以壓力計算
                    {
                        if (plc.Unit == 0)
                        {
                            statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxForce);
                            statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinForce);
                        }
                        else if (plc.Unit == 1)
                        {
                            statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxForce) * 10;
                            statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinForce) * 10;
                        }
                        else if (plc.Unit == 2)
                        {
                            statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxForce) / 2;
                            statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinForce) / 2;
                        }
                        statsData[step].StatsValue = "Force";
                    }
                    else if (motionManager.MotionData[step].Cpk == "Position")   //Cpk以位置計算
                    {
                        statsData[step].Upper = double.Parse(motionManager.MotionData[step].EndMaxPos);
                        statsData[step].Lower = double.Parse(motionManager.MotionData[step].EndMinPos);
                        statsData[step].StatsValue = "Position";
                    }
                    else
                    {

                    }
                }

                statsData[step].MidValue = (statsData[step].Upper + statsData[step].Lower) / 2;   //中間值
                statsData[step].Average = average(step, liveDataManager.LiveDataNum);   //平均值
                statsData[step].SD = standard_Deviation(step, liveDataManager.LiveDataNum, statsData[step].Average);   //標準差
                statsData[step].Ca = ca(statsData[step].Average, statsData[step].MidValue, statsData[step].Upper, statsData[step].Lower);
                statsData[step].Cp = cp(statsData[step].SD, statsData[step].Upper, statsData[step].Lower);
                statsData[step].Cpk = cpk(statsData[step].Cp, statsData[step].Ca);
            }
        }

        private double average(int step, int liveDataNum)    //平均數
        {
            double sum = 0;
            for (int i = 0; i < liveDataNum; i++)
            {
                double temp = liveDataManager.LiveData[i].StatsValue[step];
                sum = sum + temp;
            }
            return sum / (double)(liveDataNum);
        }
        private double standard_Deviation(int step, int liveDataNum, double average)  //標準差
        {
            double SDSum = 0;
            for (int i = 0; i < liveDataNum; i++)
            {
                SDSum = SDSum + Math.Pow(Math.Abs(liveDataManager.LiveData[i].StatsValue[step] - average), 2);
            }

            if (liveDataNum <= 1)   //當統計資訊過少時
            {
                return Math.Sqrt(SDSum / (double)(liveDataNum));
            }
            return Math.Sqrt(SDSum / (double)(liveDataNum - 1));
        }
        private double ca(double average, double median, double upper, double lower)
        {
            return Math.Abs(average - median) / ((double)(upper - lower) / 2);
        }
        private double cp(double sd, double upper, double lower)
        {
            return ((double)(upper - lower)) / (6 * sd);
        }
        private double cpk(double cp, double ca)
        {
            return cp * (1 - ca);
        }
    }
}
