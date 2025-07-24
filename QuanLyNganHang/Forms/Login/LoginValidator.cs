using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNganHang.Forms.Login
{
    public class LoginValidator
    {
        public bool ValidateInputs(string host, string port, string sid, string user, string pass,
            TextBox txt_Host, TextBox txt_Port, TextBox txt_Sid, TextBox txt_User, TextBox txt_Password)
        {
            var validations = new[]
            {
                (value: host, message: "Chưa điền thông tin Host", control: txt_Host),
                (value: port, message: "Chưa điền thông tin Port", control: txt_Port),
                (value: sid, message: "Chưa điền thông tin SID", control: txt_Sid),
                (value: user, message: "Chưa điền thông tin User", control: txt_User),
                (value: pass, message: "Chưa điền mật khẩu", control: txt_Password)
            };

            foreach (var validation in validations)
            {
                if (string.IsNullOrEmpty(validation.value))
                {
                    MessageBox.Show(validation.message, "Lỗi nhập liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    validation.control.Focus();
                    return false;
                }
            }
            return true;
        }

        public string GetUserStatus(string user)
        {
            string status = Database.Get_Status(user);

            if (status.Contains("ORA-28000") || status.ToUpper().Contains("LOCKED"))
                return "🔒 Tài khoản bị khóa";
            else if (status.Equals("LOCKED(TIMED)"))
                return "🔒 Tài khoản bị khóa tạm thời";
            else if (status.Equals("EXPIRED(GRACE)"))
                return "⚠️ Tài khoản sắp hết hạn";
            else if (status.Equals("EXPIRED & LOCKED(TIMED)"))
                return "🔒 Tài khoản bị khóa do hết hạn";
            else if (status.Equals("EXPIRED"))
                return "⏰ Tài khoản hết hạn";
            else if (status == " ")
                return "❌ Tài khoản không tồn tại";
            else
                return "❌ Đăng nhập thất bại!";
        }

        public bool AttemptLogin(string host, string port, string sid, string user, string pass)
        {
            try
            {
                Database.Set_Database(host, port, sid, user, pass);
                return Database.Connect();
            }
            catch
            {
                return false;
            }
        }
    }
}
