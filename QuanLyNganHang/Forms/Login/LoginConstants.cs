using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNganHang.Forms.Login
{
    public static class LoginConstants
    {
        public static class Colors
        {
            public static readonly Color Primary = Color.FromArgb(31, 81, 139);
            public static readonly Color Secondary = Color.FromArgb(41, 98, 159);
            public static readonly Color Success = Color.FromArgb(46, 204, 113);
            public static readonly Color Error = Color.FromArgb(231, 76, 60);
            public static readonly Color Warning = Color.FromArgb(241, 196, 15);
            public static readonly Color Info = Color.FromArgb(52, 152, 219);
            public static readonly Color Purple = Color.FromArgb(155, 89, 182);
            public static readonly Color Background = Color.FromArgb(240, 244, 247);
            public static readonly Color Light = Color.FromArgb(248, 249, 250);
            public static readonly Color TextPrimary = Color.FromArgb(52, 73, 94);
            public static readonly Color FocusBackground = Color.FromArgb(235, 245, 255);
        }

        public static class Sizes
        {
            public const int FormWidth = 1000;
            public const int FormHeight = 650;
            public const int LeftPanelWidth = 400;
            public const int RightPanelWidth = 600;
            public const int LogoSize = 100;
            public const int ButtonHeight = 50;
            public const int TextBoxHeight = 30;
            public const int ConnectionPanelWidth = 450;
            public const int ConnectionPanelHeight = 280;
        }

        public static class Texts
        {
            public const string BankName = "NGÂN HÀNG 3T";
            public const string SystemTitle = "HỆ THỐNG QUẢN LÝ CORE BANKING";
            public const string LoginTitle = "ĐĂNG NHẬP HỆ THỐNG";
            public const string WelcomeMessage = "Vui lòng nhập thông tin đăng nhập của bạn";
            public const string SecurityTitle = "🔒 TÍNH NĂNG BẢO MẬT";
            public const string ConnectionTitle = "⚙️ CÀI ĐẶT KẾT NỐI ORACLE";
        }

        public static readonly string[] SecurityFeatures = {
            "🔐 Mã hóa AES 256-bit",
            "🛡️ Xác thực đa lớp",
            "📋 Audit Trail",
            "🚫 Khóa tài khoản sau 3 lần sai"
        };

        public const int MAX_LOGIN_ATTEMPTS = 3;
        public const string ConnectionSettingsFile = "connection_settings.cfg";
        public const string UserSettingsFile = "user_settings.cfg";
    }
}
