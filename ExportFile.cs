using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DIAServoPress
{
    public partial class ExportFile : Form
    {
        public int Type = 0;
        
        public ExportFile()
        {
            InitializeComponent();
        }

        private void ExportFile_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbCsv.Checked == true) 
            {
                Type = 1;
            }
            else if (rbXls.Checked == true)
            {
                Type = 2;
            }
        }
    }
}
