using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Globalization;

namespace DIAServoPress
{
    public partial class MachineType : Form
    {
        private PLC plc;
        private string scale="";

        private string unit = "";
        
        public string Type = "";
        public int PositionMax = 0;
        public int ForceMax = 0;

        public MachineType(PLC plc,string unit)
        {
            this.plc = plc;
            this.unit = unit;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language); 
            InitializeComponent();
        }

        private void MachineType_Load(object sender, EventArgs e)
        {
            cboType.Items.Add("AM-ESP001");
            cboType.Items.Add("AM-ESP002");
            cboType.Items.Add("AM-ESP005");
            cboType.Items.Add("AM-ESP010");
            cboType.Items.Add("AM-ESP030");
            cboType.Items.Add("AM-ESP050");
            
            if(plc.Type==1)
            {
                cboType.SelectedIndex = 0;
            }
            else if (plc.Type == 2)
            {
                cboType.SelectedIndex = 1;
            }
            else if (plc.Type == 3)
            {
                cboType.SelectedIndex = 2;
            }
            else if (plc.Type == 4)
            {
                cboType.SelectedIndex = 3;
            }
            else if (plc.Type == 5)
            {
                cboType.SelectedIndex = 4;
            }
            else if (plc.Type == 6)
            {
                cboType.SelectedIndex = 5;
            }

            txtIP.Text = plc.IPAddress;
            txtDist.Text = Convert.ToString(plc.MaxStroke);

            scale = "1000";

            if (unit == "kgf")
            {
                rbkgf.Select();
                groupBox1.Enabled = false;
            }
            else if (unit == "N")
            {
                rbN.Select();
                groupBox1.Enabled = false;
            }
            else if (unit == "lbf")
            {
                rblbf.Select();
                groupBox1.Enabled = false;
            }
            else
            {
                rbkgf.Select();
                groupBox1.Enabled = true;
            }
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            rbChanged(); 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Type = cboType.Text;
            PositionMax = Int32.Parse(txtDist.Text);
            ForceMax = Int32.Parse(scale);

            Properties.Settings.Default.TimeMax = Int32.Parse(txtTimeMax.Text);

            /*
            Properties.Settings.Default.PositionMax = Int32.Parse(txtDist.Text);
            Properties.Settings.Default.ForceMax = Int32.Parse(scale);
            Properties.Settings.Default.Type = cboType.Text;
            */

            if (rbkgf.Checked == true)
            {
                //Properties.Settings.Default.Unit = "kgf";
                unit = "kgf";

            }
            else if (rbN.Checked == true)
            {
                //Properties.Settings.Default.Unit = "N";
                unit = "N";
            }
            else if (rblbf.Checked == true)
            {
                //Properties.Settings.Default.Unit = "lbf";
                unit = "lbf";
            }
            
            Properties.Settings.Default.Save();
        }

        private void rbkgf_CheckedChanged(object sender, EventArgs e)
        {
            rbChanged(); 
        }

        private void rbN_CheckedChanged(object sender, EventArgs e)
        {
            rbChanged(); 
        }

        private void rblbf_CheckedChanged(object sender, EventArgs e)
        {
            rbChanged(); 
        }

        private void rbChanged() 
        {
            if (rbkgf.Checked == true)
            {
                lblPressU.Text = "kgf";
                if (cboType.SelectedIndex == 0)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "100";
                    scale = "100";
                }
                else if (cboType.SelectedIndex == 1)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "200";
                    scale = "200";
                }
                else if (cboType.SelectedIndex == 2)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "500";
                    scale = "500";
                }
                else if (cboType.SelectedIndex == 3)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "1000";
                    scale = "1000";
                }
                else if (cboType.SelectedIndex == 4)
                {
                    //txtDist.Text = "200";
                    txtPress.Text = "3000";
                    scale = "3000";
                }
                else if (cboType.SelectedIndex == 5)
                {
                    //txtDist.Text = "200";
                    txtPress.Text = "5000";
                    scale = "5000";
                }
            }
            else if (rbN.Checked == true)
            {
                lblPressU.Text = "N";
                if (cboType.SelectedIndex == 0)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "981";
                    scale = "1000";
                }
                else if (cboType.SelectedIndex == 1)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "1962";
                    scale = "2000";
                }
                else if (cboType.SelectedIndex == 2)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "4905";
                    scale = "5000";
                }
                else if (cboType.SelectedIndex == 3)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "9810";
                    scale = "10000";
                }
                else if (cboType.SelectedIndex == 4)
                {
                    //txtDist.Text = "200";
                    txtPress.Text = "29430";
                    scale = "30000";
                }
                else if (cboType.SelectedIndex == 5)
                {
                    //txtDist.Text = "200";
                    txtPress.Text = "49050";
                    scale = "50000";
                }
            }
            else if (rblbf.Checked == true)
            {
                lblPressU.Text = "lbf";
                if (cboType.SelectedIndex == 0)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "220.5";
                    scale = "300";
                }
                else if (cboType.SelectedIndex == 1)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "441";
                    scale = "500";
                }
                else if (cboType.SelectedIndex == 2)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "1102.5";
                    scale = "1200";
                }
                else if (cboType.SelectedIndex == 3)
                {
                    //txtDist.Text = "100";
                    txtPress.Text = "2205";
                    scale = "2300";
                }
                else if (cboType.SelectedIndex == 4)
                {
                    //txtDist.Text = "200";
                    txtPress.Text = "6615";
                    scale = "7000";
                }
                else if (cboType.SelectedIndex == 5)
                {
                    //txtDist.Text = "200";
                    txtPress.Text = "11025";
                    scale = "12000";
                }
            }
        }

        private void MachineType_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnOK.PerformClick();
        }
    }
}
