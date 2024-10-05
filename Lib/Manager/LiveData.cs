using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class LiveData
    {
        public double StandbyPos = 0;
        public double StandbyTime = 0;

        public double PressPos = 0;
        public double PressForce = 0;
        public double PressTime = 0;

        public double Result = 0;

        public double SingleTime = 0;

        public string BarCode = "";
        public string RecipeName = "";
        public string StartTime = "";
        public string StartDate = "";

        public int DetailNum = -1;

        
        
        public double[] StatsValue = new double[CONSTANT.StepAmount+1];
    }
}
