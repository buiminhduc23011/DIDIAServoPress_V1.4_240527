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
using System.IO;
using System.Diagnostics;

namespace DIAServoPress
{
    public partial class Language : Form
    {
        public int OK=0;
        
        public Language()
        {
            InitializeComponent();
        }

        private void Language_Load(object sender, EventArgs e)
        {
            cboLan.Items.Add("English");
            cboLan.Items.Add("中文 (繁體)");
            cboLan.Items.Add("中文 (簡體)");


            if (Properties.Settings.Default.Language == "en") 
            {
                cboLan.SelectedIndex = 0;
            }
            else if (Properties.Settings.Default.Language == "zh-TW")
            {
                cboLan.SelectedIndex = 1;
            }
            else if (Properties.Settings.Default.Language == "zh-Hans")
            {
                cboLan.SelectedIndex = 2;
            }
            else
            {
                cboLan.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OK = 1;
            if (cboLan.SelectedIndex == 1)
            {
                Properties.Settings.Default.Language = "zh-TW";
            }
            else if (cboLan.SelectedIndex == 0)
            {
                Properties.Settings.Default.Language = "en";
            }
            else if (cboLan.SelectedIndex == 2)
            {
                Properties.Settings.Default.Language = "zh-Hans";
            }
            
            Properties.Settings.Default.Save();

        }

        private void Language_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OK != 1)
            {
                Properties.Settings.Default.Language = "zh-TW";
            }
        }

        private void lblDescription_Click(object sender, EventArgs e)
        {
            //Convert The resource Data into Byte[] 
            byte[] PDF = Properties.Resources.NetworkSetting;
            MemoryStream ms = new MemoryStream(PDF);

            //Create PDF File From Binary of resources folders helpFile.pdf
            FileStream f = new FileStream("NetworkSetting.pdf", FileMode.OpenOrCreate);

            //Write Bytes into Our Created helpFile.pdf
            ms.WriteTo(f);
            f.Close();
            ms.Close();

            // Finally Show the Created PDF from resources 
            Process.Start("NetworkSetting.pdf");
        }

    }
}
