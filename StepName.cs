using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public static class StepName
    {
        private string input;
        private NewMbusClient mbc;

        public StepName
        {

        }

        public void StepNameExport(string input, out int output1, out int output2, out int output3, out int output4, out int output5) 
        {
            output1 = Convert.ToString(Convert.ToInt32(Convert.ToString((int)input[1], 2) + Convert.ToString((int)input[0], 2), 2));
            output2 = Convert.ToString(Convert.ToInt32(Convert.ToString((int)input[3], 2) + Convert.ToString((int)input[2], 2), 2));
            output3 = Convert.ToString(Convert.ToInt32(Convert.ToString((int)input[5], 2) + Convert.ToString((int)input[4], 2), 2));
            output4 = Convert.ToString(Convert.ToInt32(Convert.ToString((int)input[7], 2) + Convert.ToString((int)input[6], 2), 2));
            output5 = Convert.ToString(Convert.ToInt32(Convert.ToString((int)input[9], 2) + Convert.ToString((int)input[8], 2), 2));

            mbc.Write_PLC_16("D840", output1);
            mbc.Write_PLC_16("D841", output2);
            mbc.Write_PLC_16("D842", output3);
            mbc.Write_PLC_16("D843", output4);
            mbc.Write_PLC_16("D844", output5);
        }
    }
}
