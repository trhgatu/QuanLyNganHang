using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace QuanLyNganHang.Core.Helpers
{
    public static class RsaEncryptor
    {
        private static readonly string PublicKeyPath = "public_key.xml";
        private static readonly string PrivateKeyPath = "private_key.xml";

        /// <summary>
        /// Sinh cặp khóa RSA và lưu ra file
        /// </summary>
        public static void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                File.WriteAllText(PublicKeyPath, rsa.ToXmlString(false)); // Public key
                File.WriteAllText(PrivateKeyPath, rsa.ToXmlString(true)); // Private key
            }
        }

        /// <summary>
        /// Mã hóa mảng byte bằng khóa RSA (OAEP padding, max ~245 byte)
        /// </summary>
        public static byte[] Encrypt(byte[] data)
        {
            if (!File.Exists(PublicKeyPath))
                throw new FileNotFoundException("Không tìm thấy file public_key.xml");

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(PublicKeyPath));

                int maxLength = (2048 / 8) - 42; // = 214 bytes for OAEP
                if (data.Length > maxLength)
                {
                    throw new ArgumentException(
                        $"Dữ liệu quá dài ({data.Length} bytes) để mã hóa RSA trực tiếp (giới hạn {maxLength} bytes).\n" +
                        $"👉 Hãy dùng Hybrid Encryption thay thế.");
                }

                return rsa.Encrypt(data, true); // true = OAEP
            }
        }

        /// <summary>
        /// Giải mã mảng byte đã mã hóa bằng RSA (OAEP)
        /// </summary>
        public static byte[] Decrypt(byte[] encryptedData)
        {
            if (!File.Exists(PrivateKeyPath))
                throw new FileNotFoundException("Không tìm thấy file private_key.xml");

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(PrivateKeyPath));
                return rsa.Decrypt(encryptedData, true);
            }
        }

        /// <summary>
        /// Mã hóa chuỗi (ngắn) sang base64 bằng RSA – KHÔNG KHUYẾN NGHỊ
        /// </summary>
        public static string EncryptString(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText)) return null;
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = Encrypt(data);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Giải mã chuỗi base64 từ RSA sang plaintext
        /// </summary>
        public static string DecryptString(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText)) return null;
            byte[] encrypted = Convert.FromBase64String(encryptedText);
            byte[] decrypted = Decrypt(encrypted);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
