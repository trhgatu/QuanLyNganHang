using System;
using System.Text;
using QuanLyNganHang.Helpers;

namespace QuanLyNganHang.Tools
{
    public static class EncryptDebugger
    {
        /// <summary>
        /// Kiểm tra chuỗi có thể mã hóa thành công bằng Hybrid (AES + RSA)
        /// </summary>
        public static void TestEncryption(string label, string input)
        {
            Console.WriteLine($"--- Đang kiểm tra: {label} ---");

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("⚠️ Chuỗi rỗng hoặc null.");
                return;
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(input);
            Console.WriteLine($"Độ dài chuỗi: {input.Length} ký tự");
            Console.WriteLine($"Số byte UTF-8: {dataBytes.Length} bytes");

            // Chỉ kiểm tra Hybrid Encrypt
            try
            {
                var hybrid = EncryptionHelper.EncryptHybrid(input);
                Console.WriteLine("✅ Hybrid Encrypt: Thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hybrid Encrypt: LỖI - {ex.Message}");
                System.IO.File.AppendAllText("rsa_error.log", $"[{DateTime.Now}] {label} gây lỗi Hybrid Encrypt: {ex.Message}\nChuỗi: {input}\n\n");
                throw new Exception($"Trường [{label}] gây lỗi mã hóa Hybrid: {ex.Message}");
            }

            Console.WriteLine();
        }

        public static void RunTests()
        {
            TestEncryption("CMND", "090679360000");
            TestEncryption("Phone", "0987123456");
            TestEncryption("Email", "testlongemailaddress@example.com");
            TestEncryption("Địa chỉ", "100 Hoàng Quốc Việt, Cầu Giấy, Hà Nội");

            // Test chuỗi dài
            TestEncryption("Chuỗi rất dài", new string('A', 300));
        }
    }
}
