using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Threading;

namespace DIAServoPress
{
    static class Program
    {
        private static Mutex mut;
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isOnlyOne = false;
            mut = new Mutex(true, "MyMutex", out isOnlyOne);
            if (isOnlyOne)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Index());
               
                //Application.Run(new Function());
                //Application.Run(new Management());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
