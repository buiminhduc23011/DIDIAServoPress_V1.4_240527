using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Resources;
using System.Globalization;

namespace DIAServoPress
{
    public partial class Index : Form
    {
        public Index()
        {
            Language_Changed();
            InitializeComponent();
        }

        private void Language_Changed() 
        {
            if (Properties.Settings.Default.Language == "0")
            {
                Language LanguageForm = new Language();
                LanguageForm.ShowDialog(this);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (rbOneToOne.Checked == true)
            {
                Function FunctionForm = new Function();
                FunctionForm.Show();

            }
            else if (rbOneToMany.Checked == true)
            {
                Management ManagementForm = new Management();
                ManagementForm.Show();
            }
        }

        private void Index_Load(object sender, EventArgs e)
        {

        }
    }
}
