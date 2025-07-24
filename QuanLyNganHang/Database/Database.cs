using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace QuanLyNganHang
{
    public class Database
    {
        public static OracleConnection Conn;

        public static string Host;
        public static string Port;
        public static string Sid;
        public static string User;
        public static string Password;

        public IEnumerable<DataRow> Rows { get; internal set; }

        public static void Set_Database(string host, string port, string sid, string user, string pass)
        {
            Database.Host = host;
            Database.Port = port;
            Database.Sid = sid;
            Database.User = user;
            Database.Password = pass;
        }

        public static OracleConnection Get_Connect()
        {
            if (Conn == null)
            {
                Connect();
            }
            return Conn;
        }

        public static void Close_Connect()
        {
            if (Conn.State.ToString().Equals("Open"))
            {
                Conn.Close();
            }
        }

        public static bool Connect()
        {
            string connsys = "";
            try
            {
                if (User.ToUpper().Equals("SYS"))
                {
                    connsys = ";DBA Privilege=SYSDBA;";
                }
                string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                    + Host + ")(PORT = "
                    + Port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                    + Sid + ")));User ID="
                    + User + " ; Password = "
                    + Password + connsys;

                Conn = new OracleConnection();
                Conn.ConnectionString = connString;
                Conn.Open();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string Get_Status(string user)
        {
            try
            {
                string Function = "fun_account_status";

                OracleConnection cnn = Get_Connect();

                // ✅ ĐẢM BẢO KẾT NỐI MỞ
                if (cnn.State != ConnectionState.Open)
                {
                    cnn.Open();
                }

                OracleCommand cmd = new OracleCommand
                {
                    Connection = cnn,
                    CommandText = Function,
                    CommandType = CommandType.StoredProcedure
                };

                OracleParameter resultParam = new OracleParameter
                {
                    ParameterName = "@Result",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 100,
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(resultParam);

                OracleParameter UserName = new OracleParameter
                {
                    ParameterName = "@User",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = user.ToUpper(),
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(UserName);

                cmd.ExecuteNonQuery();

                if (resultParam.Value != DBNull.Value)
                {
                    return ((OracleString)resultParam.Value).ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

            return "";
        }


    }
}
