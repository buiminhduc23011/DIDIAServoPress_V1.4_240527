using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    static class CONSTANT
    {
        public const int StepAmount = 10;        //步序數量
        public const long LiveDataAmount = 500000;    //批次壓合次數記錄數量
        //public const long ChartPointsAmount = 300000000;    //單次壓合掃描繪圖點數量
        public const long ChartPointsAmount = 3000000;    //單次壓合掃描繪圖點數量
        public const int RecipeAmount = 200;   //配方數量
        public const int TimeScaleOld = 10;
        public const int TimeScaleNew = 100;

    }
}
