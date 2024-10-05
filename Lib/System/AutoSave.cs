using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIAServoPress
{
    public class AutoSave
    {
        Export export;
        
        private int counter;
        private int time;

        public AutoSave(Export export) 
        {
            this.export = export;
        }

        public void AddCounter() 
        {
            counter++;
        }

        public void Tick() 
        {
            if (time > 600 || counter>10)
            {
                export.ExportCsv();
                time = 0;
                counter = 0;
            }
            time++;
        }

    }
}
