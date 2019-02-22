using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class StringTools
    {

        public static bool IsNumeric(string str)  //判断输入的是否是数字
        {
            System.Text.RegularExpressions.Regex reg1
                = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str);
        }


        //   <summary>  
        ///   判断是否为汉字  
        ///   </summary>  
        ///   <param   name="chrStr">待检测字符串</param>  
        ///   <returns>是汉字返回true</returns>  
        public static bool IsChineseCharacters(string chrStr)
        {
            Regex CheckStr = new Regex("[\u4e00-\u9fa5]");
            return CheckStr.IsMatch(chrStr);
        }

        /// <summary>
        /// 得到每个汉字的字首拼音码字母(大写)
        /// </summary>
        /// <param name="chrStr">输入字符串</param>
        /// <returns>返回结果</returns>
        public static string GetHeadCharacter(string chrStr)
        {
            string strHeadString = string.Empty;

            Encoding gb = System.Text.Encoding.GetEncoding("gb2312");
            for (int i = 0; i < chrStr.Length; i++)
            {
                //检测该字符是否为汉字
                if (!IsChineseCharacters(chrStr.Substring(i, 1)))
                {
                    strHeadString += chrStr.Substring(i, 1);
                    continue;
                }

                byte[] bytes = gb.GetBytes(chrStr.Substring(i, 1));
                string lowCode = System.Convert.ToString(bytes[0] - 0xA0, 16);
                string hightCode = System.Convert.ToString(bytes[1] - 0xA0, 16);
                int nCode = Convert.ToUInt16(lowCode, 16) * 100 + Convert.ToUInt16(hightCode, 16);      //得到区位码
                strHeadString += FirstLetter(nCode);
            }
            return strHeadString;
        }

        //通过汉字区位码得到其首字母(大写)
        static string FirstLetter(int nCode)
        {
            if (nCode >= 1601 && nCode < 1637) return "A";
            if (nCode >= 1637 && nCode < 1833) return "B";
            if (nCode >= 1833 && nCode < 2078) return "C";
            if (nCode >= 2078 && nCode < 2274) return "D";
            if (nCode >= 2274 && nCode < 2302) return "E";
            if (nCode >= 2302 && nCode < 2433) return "F";
            if (nCode >= 2433 && nCode < 2594) return "G";
            if (nCode >= 2594 && nCode < 2787) return "H";
            if (nCode >= 2787 && nCode < 3106) return "J";
            if (nCode >= 3106 && nCode < 3212) return "K";
            if (nCode >= 3212 && nCode < 3472) return "L";
            if (nCode >= 3472 && nCode < 3635) return "M";
            if (nCode >= 3635 && nCode < 3722) return "N";
            if (nCode >= 3722 && nCode < 3730) return "O";
            if (nCode >= 3730 && nCode < 3858) return "P";
            if (nCode >= 3858 && nCode < 4027) return "Q";
            if (nCode >= 4027 && nCode < 4086) return "R";
            if (nCode >= 4086 && nCode < 4390) return "S";
            if (nCode >= 4390 && nCode < 4558) return "T";
            if (nCode >= 4558 && nCode < 4684) return "W";
            if (nCode >= 4684 && nCode < 4925) return "X";
            if (nCode >= 4925 && nCode < 5249) return "Y";
            if (nCode >= 5249 && nCode < 5590) return "Z";
            return "";
        }



        /// <summary>
        /// 验证是否为电话号码
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelePhone(string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 验证是否为手机号码
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public bool IsMobilePhone(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,5]+\d{9}");
        }
    }
}
