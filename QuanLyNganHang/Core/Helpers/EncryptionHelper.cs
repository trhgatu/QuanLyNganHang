using QuanLyNganHang.Core.Helpers;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNganHang.Helpers
{
    public static class EncryptionHelper
    {
        /// <summary>
        /// Mã hóa văn bản bằng mã hóa lai (AES + RSA)
        /// </summary>
        public static string EncryptHybrid(string plainText)
        {
            try
            {
                return HybridEncryptor.Encrypt(plainText);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi EncryptHybrid: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Giải mã văn bản bằng mã hóa lai (AES + RSA)
        /// </summary>
        public static string DecryptHybrid(string encryptedText)
        {
            try
            {
                return HybridEncryptor.Decrypt(encryptedText);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DecryptHybrid: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mã hóa dữ liệu bằng RSA trực tiếp (chỉ nên dùng cho dữ liệu ngắn)
        /// </summary>
        public static byte[] EncryptWithRSA(byte[] data)
        {
            try
            {
                return RsaEncryptor.Encrypt(data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi EncryptWithRSA: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Giải mã dữ liệu bằng RSA trực tiếp
        /// </summary>
        public static byte[] DecryptWithRSA(byte[] encryptedData)
        {
            try
            {
                return RsaEncryptor.Decrypt(encryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DecryptWithRSA: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sinh cặp khóa RSA mới và lưu ra file
        /// </summary>
        public static void GenerateNewRSAKeys()
        {
            try
            {
                RsaEncryptor.GenerateKeys();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi GenerateNewRSAKeys: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mã hóa văn bản bằng AES thuần (trả về mảng byte)
        /// </summary>
        public static byte[] EncryptWithAES(string plainText, byte[] key, byte[] iv)
        {
            try
            {
                return AesEncryptor.Encrypt(plainText, key, iv);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi EncryptWithAES: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Giải mã văn bản bằng AES thuần
        /// </summary>
        public static string DecryptWithAES(byte[] encryptedData, byte[] key, byte[] iv)
        {
            try
            {
                return AesEncryptor.Decrypt(encryptedData, key, iv);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DecryptWithAES: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Giải mã hybrid, trả về chuỗi gốc hoặc giữ nguyên nếu lỗi
        /// </summary>
        public static string TryDecryptHybrid(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            try
            {
                return DecryptHybrid(encryptedText);
            }
            catch
            {
                // Nếu lỗi giải mã, trả lại nguyên văn để tránh crash
                return encryptedText;
            }
        }
    }
}
