using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using EasyModbus;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace DIAServoPress
{
    public partial class MES : Form
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        //private NewMbusClient mbc;

        private SqlConnectionStringBuilder builder;
        private SqlConnection connection;
        private SqlDataReader reader;
        //private SqlCommand command;
        //private SqlParameter parameter;
        private SqlDataAdapter myAdapter;
        private DataSet sqlDataSet;
        
        public string SQLLanguage = "中文";
        public string SQLOn ="";
        public string SQLIP = "";
        public string SQLName = "";
        public string SQLID = "";
        public string SQLPW = "";

        public MES(string ip, int port)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language); 
            InitializeComponent();
        }

        private void MES_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void MES_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
           
            Load_Information();
        }

        private void Load_Information()
        {
            if (SQLOn == "1") 
            {
                //txtInitialCatalog.Text = Properties.Settings.Default.SqlName;
                txtInitialCatalog.Text = SQLName;
                chkSQL.Checked = true;
                btnConnect.PerformClick();
                
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog filePath = new FolderBrowserDialog();
            filePath.ShowDialog();

            if (filePath.SelectedPath != "")
            {
                txtPath.Text = filePath.SelectedPath;
            }
        }

        private void chkSQL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSQL.Checked == true)
            {
                btnWrite.Enabled = false;
                btnWrite.BackColor = Color.DarkGray;
                btnConnect.BackColor = Color.Red;
            }
            else if (chkSQL.Checked == false)
            {
                btnWrite.Enabled = true;
                btnWrite.BackColor = Color.FromArgb(0, 135, 220);
                btnConnect.BackColor = Color.FromArgb(0, 135, 220);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //try
            //{
                builder = new SqlConnectionStringBuilder();
                builder.DataSource = txtDataSource.Text;
                builder.InitialCatalog = txtInitialCatalog.Text;
                builder.UserID = txtUserID.Text;
                builder.Password = txtPassword.Text;
                builder.IntegratedSecurity = true;

                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                myAdapter = new SqlDataAdapter("SELECT * FROM MainTable", connection);
                sqlDataSet = new DataSet();
                myAdapter.Fill(sqlDataSet, "PressTable");
                dataGridView1.DataSource = sqlDataSet.Tables["PressTable"];
                

                string str = "SELECT * FROM MainTable";
                SqlCommand command = new SqlCommand(str, connection);
                reader = command.ExecuteReader();


                if (reader.FieldCount == 12)
                {
                    if (Convert.ToString(reader.GetName(0)) == "編號")
                    {
                        SQLLanguage = "中文";
                        txtCheck.Text = "OK";
                        btnWrite.Enabled = true;
                        btnWrite.BackColor = Color.FromArgb(0, 135, 220);
                        btnConnect.BackColor = Color.FromArgb(0, 135, 220);
                    }
                    else if (Convert.ToString(reader.GetName(0)) == "No")
                    {
                        SQLLanguage = "English";
                        txtCheck.Text = "OK";
                        btnWrite.Enabled = true;
                        btnWrite.BackColor = Color.FromArgb(0, 135, 220);
                        btnConnect.BackColor = Color.FromArgb(0, 135, 220);
                    }
                    else
                    {
                        MessageBox.Show(rM.GetString("DBErrorStr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(rM.GetString("DBErrorStr"), rM.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            //}
            //catch
            //{
             //   MessageBox.Show(rm.GetString("ErrorStr"), rm.GetString("Error"),MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }



        private void btnWrite_Click(object sender, EventArgs e)
        {
            //Function fFunction = new Function();
            //Properties.Settings.Default.SqlIP = txtDataSource.Text;
            //Properties.Settings.Default.SqlName = txtInitialCatalog.Text;
            //Properties.Settings.Default.SqlID = txtUserID.Text;
            //Properties.Settings.Default.SqlPW = txtPassword.Text;

            SQLIP = txtDataSource.Text;
            SQLName = txtInitialCatalog.Text;
            SQLID = txtUserID.Text;
            SQLPW = txtPassword.Text;

            if (chkSQL.Checked == true)
            {
                SQLOn = "1";
            }
            else if (chkSQL.Checked == false)
            {
                SQLOn = "0";
            }
        }

        private void btnCreateDB_Click(object sender, EventArgs e)
        {
            String str;
            String strName = txtName.Text;
            String strPath=txtPath.Text;
            String strFileSize = txtSize.Text;
            String strMaxSize = txtMaxSize.Text;

            builder = new SqlConnectionStringBuilder();
            builder.DataSource = txtDataSource.Text;

            builder.UserID = txtUserID.Text;
            builder.Password = txtPassword.Text;
            builder.IntegratedSecurity = true;

            connection = new SqlConnection(builder.ConnectionString);

            str = "CREATE DATABASE " + strName + " ON PRIMARY " +
                "(NAME = " + strName + "_Data, " +
                "FILENAME = '"+strPath+ strName +"Data.mdf', " +
                "SIZE = " + strFileSize + " MB, MAXSIZE = " + strMaxSize + "MB, FILEGROWTH = 10%) " +
                "LOG ON (NAME = ServoPress_Log, " +
                "FILENAME =  '" +strPath + strName + "Log.ldf', " +
                "SIZE = 1MB, " +
                "MAXSIZE = 5MB, " +
                "FILEGROWTH = 10%);";

            SqlCommand myCommand = new SqlCommand(str, connection);
            try
            {
               connection.Open();
               myCommand.ExecuteNonQuery();

               str = "USE " + strName + " CREATE TABLE MainTable(" +
               "No" + " nvarchar(10) PRIMARY KEY," +
               "IP" + " nvarchar(20), " +
               "Barcode" + " nvarchar(10), " +
               "Date" + " nvarchar(20), " +
               "Time" + " nvarchar(20), " +
               "RecipeName" + " nvarchar(10), " +
               "MeasureResult" + " nvarchar(10), " +
               "StandbyPosition" + " nvarchar(10), " +
               "StandbyTime" + " nvarchar(10), " +
               "PressingPosition" + " nvarchar(10), " +
               "PressingForce" + " nvarchar(10), " +
               "PressingTime" + " nvarchar(10), " +
               ")";

               myCommand = new SqlCommand(str, connection);
               myCommand.ExecuteNonQuery();
               MessageBox.Show(rM.GetString("CreateDBOK"), rM.GetString("CreateDB"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString(), rM.GetString("CreateDBErr"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
