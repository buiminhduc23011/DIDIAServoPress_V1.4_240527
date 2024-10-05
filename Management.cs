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
using System.Xml;
using EasyModbus;
using System.Resources;

namespace DIAServoPress
{
    public partial class Management : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private int mechineNum = 0;
        private PLC plc;

        public Management()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language); 
            InitializeComponent();
        }

        private void Management_Load(object sender, EventArgs e)
        {
            
            if (Properties.Settings.Default.Path == "")
            {
                Properties.Settings.Default.Path = Convert.ToString(Application.StartupPath);
                Properties.Settings.Default.Save();
            }
            tstxtPath.Text = Properties.Settings.Default.Path;

            DialogResult scan = MessageBox.Show(this, rM.GetString("ScanString"), rM.GetString("Scan"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (scan == DialogResult.Yes)
            {
                Scan();
            }
        }

        private void Scan()
        {
            Scan ScanForm = new Scan();
            ScanForm.ShowDialog();

            if (ScanForm.DialogResult == DialogResult.OK)
            {
                string sPath = Convert.ToString(Application.StartupPath) + ".\\Property.xml";
                XmlDocument document = new XmlDocument();
                document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", ""));//將宣告節點加入document中

                XmlNode xmlnode_ServoPress = document.CreateNode(XmlNodeType.Element, "ServoPress", "");
                XmlNode xmlnode_info = document.CreateNode(XmlNodeType.Element, "info", "");
                XmlNode[] xmlnode_Data = new XmlNode[ScanForm.IPNum];

                XmlAttribute xmlattribute_num = document.CreateAttribute("num");
                xmlattribute_num.Value = Convert.ToString(ScanForm.IPNum);
                xmlnode_info.Attributes.Append(xmlattribute_num);
                xmlnode_ServoPress.AppendChild(xmlnode_info);

                for (int i = 0; i < ScanForm.IPNum; i++)
                {
                    xmlnode_Data[i] = document.CreateNode(XmlNodeType.Element, "i" + i, "");

                    XmlAttribute xmlattribute_ip = document.CreateAttribute("ip");
                    xmlattribute_ip.Value = ScanForm.IP[i];
                    xmlnode_Data[i].Attributes.Append(xmlattribute_ip);
                    xmlnode_ServoPress.AppendChild(xmlnode_Data[i]);

                    XmlAttribute xmlattribute_Type = document.CreateAttribute("type");
                    xmlattribute_Type.Value = ScanForm.Type[i];
                    xmlnode_Data[i].Attributes.Append(xmlattribute_Type);
                    xmlnode_ServoPress.AppendChild(xmlnode_Data[i]);

                    XmlAttribute xmlattribute_Unit = document.CreateAttribute("unit");
                    xmlattribute_Unit.Value = ScanForm.Unit[i];
                    xmlnode_Data[i].Attributes.Append(xmlattribute_Unit);
                    xmlnode_ServoPress.AppendChild(xmlnode_Data[i]);

                    Basic BasicForm = new Basic(ScanForm.IP[i], "502");
                    BasicForm.MdiParent = this;
                    mechineNum++;
                    BasicForm.Show();
                    LayoutMdi(MdiLayout.ArrangeIcons);
                }

                document.AppendChild(xmlnode_ServoPress);
                document.Save(sPath);
            }
        }

        private void tslbNew_Click(object sender, EventArgs e)
        {
            /*
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath + ".\\Property.xml");
            int iIPNum = Int32.Parse(doc.SelectSingleNode("/ServoPress/info").Attributes["num"].Value);
            string[] sTmpIP = new string[iIPNum + 1];
            string[] sTmpType = new string[iIPNum + 1];
            string[] sTmpUnit = new string[iIPNum + 1];

            for (int i = 0; i < iIPNum; i++)
            {
                sTmpIP[i] = doc.SelectSingleNode("/ServoPress/i" + i).Attributes["ip"].Value;
                sTmpType[i] = doc.SelectSingleNode("/ServoPress/i" + i).Attributes["type"].Value;
                sTmpUnit[i] = doc.SelectSingleNode("/ServoPress/i" + i).Attributes["unit"].Value;
            }

            sTmpIP[iIPNum] = tstxtIP.Text;
            Connect(sTmpIP[iIPNum]);
            sTmpType[iIPNum] = TypeString(plc.Type);

            if (plc.Unit == 0)
            {
                sTmpUnit[iIPNum] = "kgf";
            }
            else if (plc.Unit == 1)
            {
                sTmpUnit[iIPNum] = "N";
            }
            else if (plc.Unit == 2)
            {
                sTmpUnit[iIPNum] = "lbf";
            }

            XmlDocument document = new XmlDocument();
            document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", ""));//將宣告節點加入document中

            XmlNode xmlnode_ServoPress = document.CreateNode(XmlNodeType.Element, "ServoPress", "");
            XmlNode xmlnode_info = document.CreateNode(XmlNodeType.Element, "info", "");
            XmlNode[] xmlnode_Data = new XmlNode[iIPNum + 1];

            XmlAttribute xmlattribute_num = document.CreateAttribute("num");
            xmlattribute_num.Value = Convert.ToString(iIPNum + 1);
            xmlnode_info.Attributes.Append(xmlattribute_num);
            xmlnode_ServoPress.AppendChild(xmlnode_info);

            for (int i = 0; i <= iIPNum; i++)
            {
                xmlnode_Data[i] = document.CreateNode(XmlNodeType.Element, "i" + i, "");

                XmlAttribute xmlattribute_ip = document.CreateAttribute("ip");
                xmlattribute_ip.Value = sTmpIP[i];
                xmlnode_Data[i].Attributes.Append(xmlattribute_ip);
                xmlnode_ServoPress.AppendChild(xmlnode_Data[i]);

                XmlAttribute xmlattribute_Type = document.CreateAttribute("type");
                xmlattribute_Type.Value = sTmpType[i];
                xmlnode_Data[i].Attributes.Append(xmlattribute_Type);
                xmlnode_ServoPress.AppendChild(xmlnode_Data[i]);

                XmlAttribute xmlattribute_Unit = document.CreateAttribute("unit");
                xmlattribute_Unit.Value = sTmpUnit[i];
                xmlnode_Data[i].Attributes.Append(xmlattribute_Unit);
                xmlnode_ServoPress.AppendChild(xmlnode_Data[i]);

            }
            
            document.AppendChild(xmlnode_ServoPress);
            document.Save(Convert.ToString(Application.StartupPath) + ".\\Property.xml");
            */
            //
            Basic BasicForm = new Basic(tstxtIP.Text,"502");
            BasicForm.MdiParent = this;
            mechineNum++;
            BasicForm.Show();
            LayoutMdi(MdiLayout.ArrangeIcons);
            
        }

        private string TypeString(long value)
        {
            if (value == 1)
            {
                return "AM-ESP001";
            }
            else if (value == 2)
            {
                return "AM-ESP002";
            }
            else if (value == 3)
            {
                return "AM-ESP005";
            }
            else if (value == 4)
            {
                return "AM-ESP010";
            }
            else if (value == 5)
            {
                return "AM-ESP030";
            }
            else if (value == 6)
            {
                return "AM-ESP050";
            }
            else
            {
                return "N/A";
            }
        }

        private void Connect(string strIP)
        {
            plc = new PLC();
            plc.Connect(strIP, "502");
        }

        private void Management_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Environment.Exit(Environment.ExitCode); 
        }

        private void tslbChangePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog filePath = new FolderBrowserDialog();
            filePath.ShowDialog();
            if (filePath.SelectedPath == "")
            {

            }
            else
            {
                Properties.Settings.Default.Path = Convert.ToString(filePath.SelectedPath);
                Properties.Settings.Default.Save();
                tstxtPath.Text = Properties.Settings.Default.Path;
            }
        }

        private void tslbLanguage_Click(object sender, EventArgs e)
        {
            Language LanguageForm = new Language();
            LanguageForm.ShowDialog(this);

            if (LanguageForm.OK == 1)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);

                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Management));
                resources.ApplyResources(this.tstxtIP, "tstxtIP");
                resources.ApplyResources(this.tslbNew, "tslbNew");
                resources.ApplyResources(this.tslbChangePath, "tslbChangePath");
                resources.ApplyResources(this.tslbLanguage, "tslbLanguage");
                //
                Properties.Settings.Default.LanguageChange = 1;
                Properties.Settings.Default.Save();
                //
            }
        }

        private void tslbScan_Click(object sender, EventArgs e)
        {
            Scan();
        }
    }
}
