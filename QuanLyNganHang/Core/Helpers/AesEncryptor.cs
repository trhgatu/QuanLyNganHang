using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNganHang.Core.Helpers
{
    public static class AesEncryptor
    {
        public static byte[] GenerateKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256; // Rất khuyến nghị dùng AES-256
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public static byte[] GenerateIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateIV();
                return aes.IV;
            }
        }

        /// <summary>
        /// Mã hóa chuỗi văn bản bằng AES (trả về mảng byte)
        /// </summary>
        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            if (string.IsNullOrWhiteSpace(plainText)) return null;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = iv;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs, Encoding.UTF8))
                {
                    sw.Write(plainText);
                    sw.Flush();
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Giải mã dữ liệu AES đã mã hóa (trả về chuỗi gốc)
        /// </summary>
        public static string Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
        {
            if (encryptedData == null || encryptedData.Length == 0) return null;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = key;
                aes.IV = iv;

                using (var ms = new MemoryStream(encryptedData))
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
