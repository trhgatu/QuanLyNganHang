using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace QuanLyNganHang.DataAccess
{
    public class AuditLogDataAccess
    {
        public AuditLogStatistics GetAuditStatistics()
        {
            AuditLogStatistics stats = new AuditLogStatistics();

            try
            {
                using (var connection = Database.Get_Connect())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    string query = @"
                    SELECT
                        COUNT(*) AS TotalLogs,
                        SUM(CASE WHEN TRUNC(TIME_ACTION) = TRUNC(SYSDATE) THEN 1 ELSE 0 END) AS TodayLogs,
                        SUM(CASE WHEN TRUNC(TIME_ACTION) BETWEEN TRUNC(SYSDATE) - 7 AND TRUNC(SYSDATE) THEN 1 ELSE 0 END) AS Last7DaysLogs,
                        SUM(CASE WHEN RETURNCODE = 0 THEN 1 ELSE 0 END) AS SuccessLogs,
                        SUM(CASE WHEN RETURNCODE <> 0 THEN 1 ELSE 0 END) AS FailedLogs,
                        SUM(CASE WHEN ACTION_NAME IN ('DELETE', 'DROP', 'TRUNCATE') THEN 1 ELSE 0 END) AS SecurityAlerts
                    FROM AUDIT_LOGS";

                    using (var command = new OracleCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.TotalLogs = GetIntFromReader(reader, "TotalLogs");
                            stats.TodayLogs = GetIntFromReader(reader, "TodayLogs");
                            stats.Last7DaysLogs = GetIntFromReader(reader, "Last7DaysLogs");
                            stats.SuccessLogs = GetIntFromReader(reader, "SuccessLogs");
                            stats.FailedLogs = GetIntFromReader(reader, "FailedLogs");
                            stats.SecurityAlerts = GetIntFromReader(reader, "SecurityAlerts");

                            Console.WriteLine($"[DEBUG] Total: {stats.TotalLogs}, Today: {stats.TodayLogs}, 7days: {stats.Last7DaysLogs}, Success: {stats.SuccessLogs}, Failed: {stats.FailedLogs}, Alerts: {stats.SecurityAlerts}");
                        }
                        else
                        {
                            Console.WriteLine("No data returned from AUDIT_LOGS.");
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Error: {ex.Message} (Code: {ex.Number})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error retrieving audit stats: {ex.Message}");
            }

            return stats;
        }

        private int GetIntFromReader(OracleDataReader reader, string columnName)
        {
            int colIndex = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(colIndex))
            {
                object value = reader.GetValue(colIndex);
                if (value is decimal d) return Convert.ToInt32(d);
                if (value is int i) return i;
                if (value is long l) return (int)l;
            }
            return 0;
        }
        public void ExecuteAuditConfig(string sql)
        {
            using (var conn = Database.Get_Connect())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private DataTable LoadAuditLogs(string procName, string userName = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (OracleCommand cmd = new OracleCommand(procName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var userParam = cmd.Parameters.Add("p_user", OracleDbType.Varchar2);
                        if (string.IsNullOrEmpty(userName) || userName == "Tất cả")
                            userParam.Value = DBNull.Value;
                        else
                            userParam.Value = userName;
                        cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                            Console.WriteLine($"⚠️ No data from {procName} for user '{userName ?? "null"}'");
                    }
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle Error in LoadAuditLogs: {ex.Message} (Code: {ex.Number})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error in LoadAuditLogs: {ex.Message}");
            }

            return dt;
        }

        public DataTable GetStandardAuditLogs(string userName = null)
        {
            return LoadAuditLogs("pro_select_audit_trail_user", userName);
        }

        public DataTable GetFineGrainedAuditLogs(string userName = null)
        {
            return LoadAuditLogs("pro_select_fga_audit_user", userName);
        }

        public DataTable GetTriggerAuditLogs(string userName = null)
        {
            return LoadAuditLogs("pro_select_trigger_audit_logs", userName);
        }

        public DataTable GetUserList()
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = Database.Get_Connect())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (OracleCommand cmd = new OracleCommand("pro_sys_select_user_dml", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                            Console.WriteLine("⚠️ Không có user nào được trả về từ stored procedure.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách user: {ex.Message}");
            }

            return dt;
        }
    }
}

public class AuditLogStatistics
{
    public int TotalLogs { get; set; }
    public int TodayLogs { get; set; }
    public int Last7DaysLogs { get; set; }
    public int SuccessLogs { get; set; }
    public int FailedLogs { get; set; }
    public int SecurityAlerts { get; set; }
}
