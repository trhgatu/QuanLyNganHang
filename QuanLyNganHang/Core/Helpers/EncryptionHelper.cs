using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNganHang.Helpers
{
    public static class EncryptionHelper
    {
        // --- Khóa RSA có thể lưu trữ ngoài file trong thực tế ---
        private static RSAParameters publicKey;
        private static RSAParameters privateKey;

        static EncryptionHelper()
        {
            using (var rsa = RSA.Create(2048))
            {
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);
            }
        }

        // 🔐 Mã hóa RSA (sử dụng public key)
        public static string EncryptRSA(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedData);
            }
        }

        // 🔓 Giải mã RSA (sử dụng private key)
        public static string DecryptRSA(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText)) return encryptedText;

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedText), RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        // (Các hàm AES cũ giữ nguyên nếu bạn vẫn dùng song song)
    }
}
