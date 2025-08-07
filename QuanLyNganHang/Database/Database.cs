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
        public static string GetConnectionString()
        {
            string connsys = User.ToUpper().Equals("SYS") ? ";DBA Privilege=SYSDBA;" : "";
            return "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                + Host + ")(PORT = " + Port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                + Sid + ")));User ID=" + User + " ; Password = " + Password + connsys;
        }
        public static OracleConnection Get_Connect()
        {
            if (Conn == null || Conn.State == ConnectionState.Broken || Conn.State == ConnectionState.Closed)
            {
                Connect();
                Console.WriteLine(">>> Current DB user: " + Database.User);
                Console.WriteLine(">>> Conn state: " + (Conn?.State.ToString() ?? "NULL"));
            }


            if (Conn != null && Conn is IDisposable disposable && Conn.State == ConnectionState.Closed)
            {
                Conn.Dispose();
                Conn = null;
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
            try
            {
                Conn = new OracleConnection(GetConnectionString());
                Conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string Get_Status(string user)
        {
            try
            {
                string Function = "ADMIN_NGANHANG.fun_account_status";

                OracleConnection cnn = Get_Connect();

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
                    ParameterName = "return_value", 
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 100,
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(resultParam);

                OracleParameter UserName = new OracleParameter
                {
                    ParameterName = "username",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = user.ToUpper(),
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(UserName);

                cmd.ExecuteNonQuery();

                if (resultParam.Value != DBNull.Value)
                {
                    string status = ((OracleString)resultParam.Value).ToString().Trim();
                    System.Diagnostics.Debug.WriteLine($"User: {user}, Status: '{status}'");

                    return status;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting status for user {user}: {ex.Message}");
                return "Error: " + ex.Message;
            }

            return " "; 
        }



    }
}
