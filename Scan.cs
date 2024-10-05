using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.NetworkInformation;
using System.Net;
using EasyModbus;
using SharpPcap;
using PacketDotNet;

using System.Net.Sockets;
using System.Threading;
using System.Resources;
using System.Xml;

namespace DIAServoPress
{
    public partial class Scan : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private PLC plc;

        private CaptureDeviceList devices = CaptureDeviceList.Instance;
        private UDPer udp = new UDPer();

        public int IPNum = 0;
        public String[] IP = new String[10000];
        public String[] Type = new String[10000];
        public String[] Unit = new String[10000];

        private int iClock = 0;
        private int loopNum = 0;

        class UDPer
        {
            const int PORT_NUMBER = 20006;
            
            
            //private readonly UdpClient udp = new UdpClient(PORT_NUMBER);

            public void Send(byte[] bytes)
            {
                UdpClient client = new UdpClient();
                IPEndPoint ip = new IPEndPoint(System.Net.IPAddress.Broadcast, PORT_NUMBER);
                client.Send(bytes, bytes.Length, ip);
                client.Close();
            }
        }

        public Scan()
        {
            InitializeComponent();
        }

        private void Scan_Load(object sender, EventArgs e)
        {
            int i = 0;
            string strTemp = "";
            foreach (var dev in devices)
            {
                strTemp = dev.Description;
                strTemp = Convert.ToString(strTemp.Substring(strTemp.IndexOf("'") + 1));
                strTemp = Convert.ToString(strTemp.Substring(0, strTemp.IndexOf("'")));

                cboNetwork.Items.Add(strTemp);
                i++;

            }
            cboNetwork.SelectedIndex = i - 1;

            btnScan.Focus();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            pbar.Value = 1;
            listView1.Items.Clear();
            loopNum = 0;
            iClock = 1;
            IPNum = 0;

            tmrScan.Enabled = true;

            var device = devices[cboNetwork.SelectedIndex];
            device.Open(DeviceMode.Promiscuous, 1);
            device.Filter = "udp";

            SendBroadcast();

            while (true)
            {
                var rawCapture = device.GetNextPacket();
                if (rawCapture != null)
                {
                    int a = 0, b = 0, c = 0, d = 0;

                    Packet p = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
                    a = p.Bytes[42];
                    b = p.Bytes[43];
                    c = p.Bytes[44];
                    d = p.Bytes[45];
                    if ((p.Bytes[42] == 0) && (p.Bytes[43] == 21) && (p.Bytes[44] == 0) && (p.Bytes[45] == 1))
                    {
                        IP[IPNum] = Convert.ToString(p.Bytes[26]) + "." + Convert.ToString(p.Bytes[27]) + "." + Convert.ToString(p.Bytes[28]) + "." + Convert.ToString(p.Bytes[29]);
                        IPNum++;
                        break;
                    }
                }
                if (loopNum > 1000)
                {
                    break;
                }

                loopNum++;
            }
            export(IP, IPNum);
        }

        private void export(string[] ip, int totalNum)
        {
            for (int i = 0; i < totalNum; i++)
            {
                Connect(IP[i]);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = Convert.ToString(IP[i]);
                Type[i] = TypeString(plc.Type);

                if (plc.Unit== 0)
                {
                    Unit[i] = "kgf";
                }
                else if (plc.Unit == 1)
                {
                    Unit[i] = "N";
                }
                else if (plc.Unit == 2)
                {
                    Unit[i] = "lbf";
                }

                lvi.SubItems.Add(Type[i]);
                lvi.SubItems.Add(Unit[i]);
                listView1.Items.Add(lvi);
                lvi.ImageIndex++;
            }
        }

        
        private void Connect(string ip)
        {
            plc = new PLC();
            plc.Connect(ip, "502");

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

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void SendBroadcast()
        {
            byte[] header = new byte[36];

            // Product ID
            header[0] = 0x04;
            header[1] = 0x05;

            // Version
            header[2] = 0x00;
            header[3] = 0x01;

            // Connection Number
            header[4] = 0x04;
            header[5] = 0x05;
            header[6] = 0x06;
            header[7] = 0x07;

            // Operate Code
            header[8] = 0x00;
            header[9] = 0x00;

            // MAC Address
            header[10] = 0x00;
            header[11] = 0x00;
            header[12] = 0x00;
            header[13] = 0x00;
            header[14] = 0x00;
            header[15] = 0x00;

            //Appended Index
            header[16] = 0x00;
            header[17] = 0x00;

            //Authenticate Enable
            header[18] = 0x00;
            header[19] = 0x00;

            //Password
            header[20] = 0x00;
            header[21] = 0x00;
            header[22] = 0x00;
            header[23] = 0x00;

            //Reserved
            header[24] = 0x00;
            header[25] = 0x00;
            header[26] = 0x00;
            header[27] = 0x00;

            //Slave Address
            header[28] = 0x00;
            header[29] = 0x00;

            //Reserved
            header[30] = 0x00;
            header[31] = 0x00;

            //Firmware Version
            header[32] = 0x00;
            header[33] = 0x00;

            //ODM Code
            header[34] = 0x00;
            header[35] = 0x00;

            udp.Send(header);

        }

        private void tmrScan_Tick(object sender, EventArgs e)
        {
            iClock++;
            if (iClock > 3)
            {
                tmrScan.Enabled = false;
                if (IPNum > 0)
                {
                    MessageBox.Show(this, rM.GetString("ScanOKStr"), rM.GetString("ScanOKStr"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnConnect.Enabled = true;
                    btnConnect.BackColor = Color.FromArgb(0, 173, 240);
                    btnConnect.ForeColor = Color.White;
                }
                else
                {
                    MessageBox.Show(this, rM.GetString("ScanErrStr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                iClock = 3;
            }
            pbar.Value = iClock;
        }

        private void btnAbandon_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML設定檔|*.xml";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlDocument document = new XmlDocument();
                document.Load(openFileDialog1.FileName);

                try
                {
                    IPNum = Int32.Parse(document.SelectSingleNode("/ServoPress/info").Attributes["num"].Value);
                    for (int i = 0; i < IPNum; i++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = document.SelectSingleNode("/ServoPress/i" + i).Attributes["ip"].Value;
                        IP[i] = lvi.Text;

                        lvi.SubItems.Add(document.SelectSingleNode("/ServoPress/i" + i).Attributes["type"].Value);
                        Type[i] = document.SelectSingleNode("/ServoPress/i" + i).Attributes["type"].Value;

                        lvi.SubItems.Add(document.SelectSingleNode("/ServoPress/i" + i).Attributes["unit"].Value);
                        Unit[i] = document.SelectSingleNode("/ServoPress/i" + i).Attributes["unit"].Value;

                        listView1.Items.Add(lvi);
                        lvi.ImageIndex++;
                    }
                    btnConnect.Enabled = true;
                    btnConnect.BackColor = Color.FromArgb(0, 173, 240);
                    btnConnect.ForeColor = Color.White;
                }

                catch (Exception)
                {
                    MessageBox.Show(this, rM.GetString("LoadErr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Scan_FormClosing(object sender, FormClosingEventArgs e)
        {
            //udp
        }

    }
}
