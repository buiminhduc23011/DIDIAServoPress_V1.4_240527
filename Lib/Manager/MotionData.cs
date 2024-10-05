using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class MotionData
    {
        public int Mode;  //  0-Motionless 1-Position Mode  2-Force 3-Distance  4-Force Position 5-Force Distance
        public string OriginalPos;
        public string OriginalVel;
        public string StandbyPos;
        public string StandbyVel;
        public string StandbyTime;
        public string PressPos;
        public string PressForce;
        public string PressTime;
        public string PressVel;
        public string EndMaxPos;
        public string EndMinPos;
        public string EndMaxForce;
        public string EndMinForce;
        public string Cpk;
        public string LimitType;
        public string BeginMaxForce;
        public string BeginMinForce;

    }
}
