using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class BufferSort
    {
        private int processPts = 0;
        bool back = false;

        public BufferSort()
        {

        }

        public double[] Export(double[] inputArray, int digit, int bufferNum)
        {
            //double[] Array = inputArray;
            double[] Array = (double[])inputArray.Clone();
            processPts = 0;

            int startPts = startPoint(Array, bufferNum);

            if (startPts > 0)
            {
                int BackStartPts = startPts;
                int endPts = endPoint(Array, startPts, bufferNum);
                double gap = calGap(Array, startPts, endPts);
                //Back(Array, startPts, endPts, gap, digit);
                for (int i = 0; processPts < bufferNum - 1; i++)
                {
                    Create(Array, digit, bufferNum);
                }
                Back(Array, BackStartPts, digit);
            }
            
            return Array;
        }

        public double[] Create(double[] Array, int digit, int bufferNum)
        {
            int startPts = startPoint(Array, bufferNum);
            int endPts = endPoint(Array, startPts, bufferNum);
            double gap = calGap(Array, startPts, endPts);

            //if (gap == 0) gap = 0.1;

            int k = 1;
            if (gap != 0)
            {
                for (int j = startPts + 1; j < endPts + 1; j++)
                {
                    Array[j] = Math.Round(Array[j] + gap * k, digit);
                    k++;
                }
            }
            processPts = endPts;

            return Array;
        }


        public void Back(double[] Array, int startPts, int digit)
        {
            try
            {
                double a = Array[startPts];
                double b = Array[startPts + 10];


                double gap = (b - a) / 10;
                if (gap < 1) gap = 1;

                if (back == true)
                {
                    int i = startPts - 2;
                    int k = 1;
                    double temp = 0;
                    while (i > 0)
                    {
                        temp = Math.Round(Array[startPts] - gap * k, digit);
                        if (temp > 0)
                        {
                            Array[i] = temp;
                            i--;
                            k++;
                        }
                        else
                        {
                            back = false;
                            break;
                        }
                    }
                    back = false;
                }
            }
            catch 
            { 
            
            }
            
            
        }


        private int startPoint(double[] Array, int bufferNum)
        {
            for (int i = processPts; i < bufferNum - 2; i++)
            {
                if ((Array[i] != 0) && (Array[i] == Array[i + 1]))
                {
                    if ((i > 1) && (Array[i - 1] == 0))
                    {
                        back = true;
                    }
                    return i + 1;
                }
            }
            return -1;
        }

        private int endPoint(double[] Array, int startPts, int bufferNum)
        {
            if (startPts != -1)
            {
                for (int i = startPts; i < bufferNum - 1; i++)
                {
                    if (Array[i] != Array[i + 1] && Array[i + 1] > 0)
                    {
                        return i;
                    }
                }
            }
            return bufferNum-1;
        }

        private double calGap(double[] Array, int startPts, int endPts)
        {
            if (endPts != -1 && startPts >= 0 && endPts >= 0 && Array[endPts + 1] != 0)
            {
                return (Array[endPts + 1] - Array[startPts]) / (endPts + 1 - startPts);
            }
            else
                return 0;
        }
    }
}
