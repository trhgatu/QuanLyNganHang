using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyNganHang.Core.Helpers
{
    public static class HybridEncryptor
    {
        public static string Encrypt(string plainText)
        {
            try
            {
                // Sinh khóa và IV AES
                byte[] aesKey = AesEncryptor.GenerateKey();
                byte[] aesIV = AesEncryptor.GenerateIV();

                // Mã hóa dữ liệu bằng AES
                byte[] encryptedData = AesEncryptor.Encrypt(plainText, aesKey, aesIV);

                // Mã hóa key + IV bằng RSA
                byte[] encryptedKey = RsaEncryptor.Encrypt(aesKey); // <= 256 bytes nếu key 2048 bit
                byte[] encryptedIV = RsaEncryptor.Encrypt(aesIV);

                // Nối lại thành [lenKey][encryptedKey][lenIV][encryptedIV][encryptedData]
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(encryptedKey.Length);
                    writer.Write(encryptedKey);

                    writer.Write(encryptedIV.Length);
                    writer.Write(encryptedIV);

                    writer.Write(encryptedData);

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi Encrypt (Hybrid): " + ex.Message, ex);
            }
        }

        public static string Decrypt(string base64Encrypted)
        {
            try
            {
                byte[] allBytes = Convert.FromBase64String(base64Encrypted);

                using (MemoryStream ms = new MemoryStream(allBytes))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    // Đọc độ dài và dữ liệu khóa AES
                    int keyLength = reader.ReadInt32();
                    byte[] encryptedKey = reader.ReadBytes(keyLength);

                    // Đọc độ dài và dữ liệu IV
                    int ivLength = reader.ReadInt32();
                    byte[] encryptedIV = reader.ReadBytes(ivLength);

                    // Dữ liệu AES còn lại
                    byte[] encryptedData = reader.ReadBytes((int)(ms.Length - ms.Position));

                    // Giải mã AES key và IV bằng RSA
                    byte[] aesKey = RsaEncryptor.Decrypt(encryptedKey);
                    byte[] aesIV = RsaEncryptor.Decrypt(encryptedIV);

                    // Giải mã dữ liệu AES
                    return AesEncryptor.Decrypt(encryptedData, aesKey, aesIV);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi Decrypt (Hybrid): " + ex.Message, ex);
            }
        }
    }
}
