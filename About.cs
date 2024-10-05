using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Globalization;
using System.Threading;

namespace DIAServoPress
{
    public partial class About : Form
    {
        public About()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language); 
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

    }
}
