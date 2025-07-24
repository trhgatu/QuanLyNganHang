using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNganHang.Forms.Dashboard
{
    public static class DashboardConstants
    {
        public static class Colors
        {
            public static readonly Color Primary = Color.FromArgb(31, 81, 139);
            public static readonly Color Secondary = Color.FromArgb(52, 73, 94);
            public static readonly Color Success = Color.FromArgb(46, 204, 113);
            public static readonly Color Warning = Color.FromArgb(241, 196, 15);
            public static readonly Color Danger = Color.FromArgb(231, 76, 60);
            public static readonly Color Info = Color.FromArgb(52, 152, 219);
            public static readonly Color Dark = Color.FromArgb(44, 62, 80);
            public static readonly Color Light = Color.FromArgb(248, 249, 250);
            public static readonly Color Background = Color.FromArgb(240, 244, 247);
            public static readonly Color MenuHover = Color.FromArgb(41, 128, 185);
            public static readonly Color LogoutButton = Color.FromArgb(192, 57, 43);
        }

        public static class Sizes
        {
            public const int MenuPanelWidth = 250;
            public const int HeaderHeight = 80;
            public const int FooterHeight = 40;
            public const int MenuButtonHeight = 60;
            public const int StatCardHeight = 100;
            public const int ActionPanelHeight = 80;
        }

        public static class Texts
        {
            public const string AppTitle = "NGÂN HÀNG QUỐC GIA";
            public const string AppVersion = "© 2025 Hệ thống Quản lý Ngân hàng - Phiên bản 1.0.0";
            public const string SystemStatus = "Hệ thống hoạt động bình thường";
        }
    }
}
