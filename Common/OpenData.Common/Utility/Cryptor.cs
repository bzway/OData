using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OpenData.Utility
{
    public class Cryptor
    {
        public static string DecryptAES(string encryptedDataString, string passwordKey)
        {

            RijndaelManaged managed = new RijndaelManaged();
            byte[] buffer = Convert.FromBase64String(encryptedDataString);
            ICryptoTransform transform = managed.CreateDecryptor(GetKey(passwordKey), GetIV(passwordKey));
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer2 = new byte[buffer.Length];
            int length = stream2.Read(buffer2, 0, buffer2.Length);
            byte[] destinationArray = new byte[length];
            Array.Copy(buffer2, destinationArray, length);
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(destinationArray);
        }
        public static string EncryptAES(string dataToEncrypt, string passwordKey)
        {
            ICryptoTransform transform = new RijndaelManaged().CreateEncryptor(GetKey(passwordKey), GetIV(passwordKey));
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            byte[] bytes = new ASCIIEncoding().GetBytes(dataToEncrypt);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            string str = Convert.ToBase64String(stream.ToArray());
            return str;
        }
        private static byte[] GetIV(string passwordKey)
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(passwordKey, Salt);
            return bytes.GetBytes(0x10);
        }
        private static byte[] GetKey(string passwordKey)
        {
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(passwordKey, Salt);
            return bytes.GetBytes(0x20);
        }
        //private static readonly byte[] Salt = new byte[] { 0x48, 0x76, 0x61, 110, 0x23, 0x4d, 0x65, 100, 0x71, 0x65, 100, 0x65, 0x72 };
        private static readonly byte[] Salt = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        public static string EncryptMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
