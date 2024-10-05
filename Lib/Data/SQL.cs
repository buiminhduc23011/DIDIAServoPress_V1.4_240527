using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using System.Resources;
using System.Windows.Forms;

namespace DIAServoPress
{
    public class SQL
    {
        protected ResourceManager rM = new ResourceManager("DIAServoPress.RecourceManager", typeof(Function).Assembly);
        private LiveDataManager liveDataManager;

        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        private SqlConnection connection;
        private SqlCommand command;
        private string SQLVerision = "中文";    //The language of the connectiong SQL database

        private string sqlIP = "";
        private string sqlName = "";
        private string sqlID = "";
        private string sqlPW = "";

        public string SQLOn = "";

        public SQL(LiveDataManager liveDataManager)
        {
            this.liveDataManager = liveDataManager;
        } 

        public void Upload_SQL(string ip,string barcode)  //上傳至SQL資料庫
        {
            command = new SqlCommand();
            SqlCommand cmd = new SqlCommand();

            command.Connection = connection;
            cmd.Connection = connection;
            //
            command.CommandText = "INSERT INTO MainTable (No,IP,Barcode,Date,Time,RecipeName,MeasureResult,StandbyPosition,StandbyTime,PressingPosition,PressingForce,PressingTime) VALUES (@No,@IP,@Sheet,@Date,@Time,@Recipe,@Measure,@StandbyPos,@PreT,@PressPos,@PressP,@PressT)";
            cmd.CommandText = "SELECT count(No) FROM MainTable";
            //
            command.Parameters.Add(new SqlParameter("No", Convert.ToString(Int32.Parse(Convert.ToString(cmd.ExecuteScalar())) + 1)));
            command.Parameters.Add(new SqlParameter("IP", ip));
            command.Parameters.Add(new SqlParameter("Sheet", barcode));
            command.Parameters.Add(new SqlParameter("Date", liveDataManager.LiveData[liveDataManager.LiveDataNum].StartDate));
            command.Parameters.Add(new SqlParameter("Time", liveDataManager.LiveData[liveDataManager.LiveDataNum].StartTime));
            command.Parameters.Add(new SqlParameter("Recipe", liveDataManager.LiveData[liveDataManager.LiveDataNum].RecipeName));
            command.Parameters.Add(new SqlParameter("Measure", result(liveDataManager.LiveData[liveDataManager.LiveDataNum].Result)));
            command.Parameters.Add(new SqlParameter("StandbyPos", Convert.ToString(liveDataManager.LiveData[liveDataManager.LiveDataNum].StandbyPos)));
            command.Parameters.Add(new SqlParameter("PreT", Convert.ToString(liveDataManager.LiveData[liveDataManager.LiveDataNum].StandbyTime)));
            command.Parameters.Add(new SqlParameter("PressPos", Convert.ToString(liveDataManager.LiveData[liveDataManager.LiveDataNum].PressPos)));
            command.Parameters.Add(new SqlParameter("PressP", Convert.ToString(liveDataManager.LiveData[liveDataManager.LiveDataNum].PressForce)));
            command.Parameters.Add(new SqlParameter("PressT", Convert.ToString(liveDataManager.LiveData[liveDataManager.LiveDataNum].PressTime)));

            command.ExecuteNonQuery();
        }

        public bool Set_SQL(string sqlOn, string sqlIP, string sqlName, string sqlID, string sqlPW, string sqlVerision) 
        {
            this.SQLOn = sqlOn;
            this.sqlIP = sqlIP;
            this.sqlName = sqlName;
            this.sqlID = sqlID;
            this.sqlPW = sqlPW;

            if (SQLOn == "1")
            {
                try
                {
                    builder = new SqlConnectionStringBuilder();
                    builder.DataSource = sqlIP;
                    builder.InitialCatalog =sqlName;
                    builder.UserID = sqlID;
                    builder.Password = sqlPW;
                    builder.IntegratedSecurity = true;

                    connection = new SqlConnection(builder.ConnectionString);
                    connection.Open();

                    SQLVerision = sqlVerision;
                    return true;
                }
                catch
                {
                    MessageBox.Show(rM.GetString("SQLErrorStr"), rM.GetString("SQLError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (SQLOn == "0")
            {
                
                try
                {
                    connection.Close();
                }
                catch
                {

                }
                return false;
            }
            return false;
        }

        private string result(double result)
        {
            if (result == 0)
            {
                return "OK";
            }
            else if (result == 2)
            {
                return "NG";
            }
            else if (result == 5)
            {
                return rM.GetString("ForceLNG");
            }
            else if (result == 6)
            {
                return rM.GetString("ForceSNG");
            }
            else if (result == 7)
            {
                return rM.GetString("PosLNG");
            }
            else if (result == 8)
            {
                return rM.GetString("PosSNG");
            }
            else if (result == 9)
            {
                return rM.GetString("ExceedForceLimit");
            }
            else if (result == 10)
            {
                return rM.GetString("ExceedPosLimit");
            }
            else
            {
                return "";
            }

        }
    }
}
