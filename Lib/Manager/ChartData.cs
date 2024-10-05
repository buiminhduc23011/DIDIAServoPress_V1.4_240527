using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class ChartData
    {
        public double[] ChartPosition = new double[CONSTANT.ChartPointsAmount];
        public double[] ChartForce = new double[CONSTANT.ChartPointsAmount];
        public double[] ChartVelocity = new double[CONSTANT.ChartPointsAmount];
        public double[] ChartTime = new double[CONSTANT.ChartPointsAmount];

        public ChartData() 
        { 
        
        }
    }
}
