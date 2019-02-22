using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class EncryptTools
    {
        public static string DesEncrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(pToEncrypt);
            dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(sKey);
            dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(sKey);
            MemoryStream memoryStream = new System.IO.MemoryStream();
            CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), 
                System.Security.Cryptography.CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = memoryStream.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            stringBuilder.ToString();
            return stringBuilder.ToString();
        }

        public static string DesDecrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] array = new byte[pToDecrypt.Length / 2];
            checked
            {
                for (int i = 0; i < pToDecrypt.Length / 2; i++)
                {
                    int num = Convert.ToInt32(pToDecrypt.Substring(i * 2, 2), 16);
                    array[i] = (byte)num;
                }
                dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(sKey);
                dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(sKey);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(),
                    System.Security.Cryptography.CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                StringBuilder stringBuilder = new StringBuilder();
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }



        //public static string DesEncrypt2(string encryptString,string key)
        //{
        //    byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
        //    byte[] keyIV = keyBytes;
        //    byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        //    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        //    MemoryStream mStream = new MemoryStream();
        //    CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
        //    cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //    cStream.FlushFinalBlock();
        //    return Convert.ToBase64String(mStream.ToArray());
        //}

        ///// <summary>
        ///// DES解密
        ///// </summary>
        ///// <param name="decryptString"></param>
        ///// <returns></returns>
        //public static string DesDecrypt2(string decryptString,string key)
        //{
        //    byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
        //    byte[] keyIV = keyBytes;
        //    byte[] inputByteArray = Convert.FromBase64String(decryptString);
        //    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        //    MemoryStream mStream = new MemoryStream();
        //    CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
        //    cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //    cStream.FlushFinalBlock();
        //    return Encoding.UTF8.GetString(mStream.ToArray());
        //}
    }
}
