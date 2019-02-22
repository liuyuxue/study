using System;
using System.Text;

namespace CommonLib.Tools
{
    public class ByteCalcHelper
    {
        public static int GetBitFormByte(byte by, int index)
        {
            if (index < 0 || index > 7)
                return -1;
            return (by >> index) & 1;
        }

        /// <summary>
        /// 2进制字符串转十进制数字
        /// </summary>
        public static int BinaryString2Num(string BinaryString)
        {
            int n = Convert.ToInt32(BinaryString, 2);
            return n;
        }

        /// <summary>
        /// 计算异或值
        /// </summary>
        public static byte CalcXOr(Byte[] bs, int startIndex, int endIndex)
        {
            byte VerifyByte = bs[startIndex];
            for (int i = startIndex + 1; i <= endIndex; i++)
            {
                VerifyByte = (byte)(VerifyByte ^ bs[i]);
            }
            return VerifyByte;
        }

      

        /// <summary>
        /// 将字节转换成指定进制的字符串
        /// </summary>
        /// <param name="byteArray">要转换的字符串</param>
        /// <param name="toBase">它必须是 2 或 16</param>
        /// <returns></returns>
        public static string ByteToString(byte b, int toBase  )
        {
            if (toBase == 16)
            {
                string s = b.ToString("X2");
                return s;
            }
            else if (toBase == 2)
            {
                string s = Convert.ToString(b, 2).PadLeft(8, '0');
                return s;
            }
            return null;
        }


        /// <summary>
        /// 将字节数组节转换成指定进制的字符串
        /// </summary>
        /// <param name="byteArray">要转换的字符串</param>
        /// <param name="toBase">它必须是 2 或 16</param>
        /// <returns></returns>
        public static string BytesToString(byte[] byteArray, int toBase = 16)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            switch (toBase)
            {
                case 2:
                    return ByteArrayToBinaryString(byteArray);
                case 16:
                    return ByteArrayToHexStr(byteArray);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 将指定进制的字符串转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromBase"></param>
        /// <returns></returns>
        public static byte[] StrToByteArray(string value, int fromBase = 16)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            switch (fromBase)
            {
                case 2:
                    return BinaryStringToByteArray(value);
                case 16:
                    return HexStrToByteArray(value);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 二进制字符串转换成字节数组
        /// </summary>
        /// <param name="binaryString">要转换的字符，如"00000000 11111111"</param>
        /// <returns></returns>
        private static byte[] BinaryStringToByteArray(string binaryString)
        {
            binaryString = binaryString.Replace(" ", "");

            if ((binaryString.Length % 8) != 0)
                throw new ArgumentException("二进制字符串长度不对");

            byte[] buffer = new byte[binaryString.Length / 8];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(binaryString.Substring(i * 8, 8).Trim(), 2);
            }
            return buffer;

        }

        /// <summary>
        /// 十六进制字符串转换成字节数组 
        /// </summary>
        /// <param name="hexString">要转换的字符串</param>
        /// <returns></returns>
        private static byte[] HexStrToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                throw new ArgumentException("十六进制字符串长度不对");
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 0x10);
            }
            return buffer;
        }


        /// <summary>
        /// 字节数组节转换成二进制字符串
        /// </summary>
        /// <param name="b">要转换的字节数组</param>
        /// <returns></returns>
        private static string ByteArrayToBinaryString(byte[] byteArray)
        {
            int capacity = byteArray.Length * 8;
            StringBuilder sb = new StringBuilder(capacity);
            for (int i = 0; i < byteArray.Length; i++)
            {
                string tmp = Convert.ToString(byteArray[i], 2).PadLeft(8, '0');
                sb.Append(tmp);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字节数组转换成十六进制字符串
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns></returns>
        private static string ByteArrayToHexStr(byte[] byteArray)
        {
            int capacity = byteArray.Length * 2;
            StringBuilder sb = new StringBuilder(capacity);

            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    sb.Append(byteArray[i].ToString("X2")+" ");
                }
            }
            return sb.ToString().Trim();
        }


        /// <summary>
        /// 两个bytes加起来
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] BytesUnion(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }

    }
}
 