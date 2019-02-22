using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

namespace Winsion.Tools
{
   public class IniTools
    {
    
       [DllImport("kernel32")]
       private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll")]
       public extern static int GetPrivateProfileStringA(string segName, string keyName, string sDefault, byte[] buffer, int iLen, string fileName); // ANSI版本

       //[DllImport("kernel32.dll")]
       //public extern static int GetPrivateProfileSection(string segName, StringBuilder buffer, int nSize, string fileName);

       [DllImport("kernel32.dll")]
       public extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);

       [DllImport("kernel32")]
       private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

       /// <summary>
       /// 写INI
       /// </summary>
       /// <param name="section">INI文件中的段落名称</param>
       /// <param name="key">INI文件中的关键字</param>
       /// <param name="val">INI文件中关键字的数值</param>
       /// <returns>返回执行结果</returns>
       public static int WriteINI(string section, string key, string val, string iniFileFullName)
       {
           int executeResult = 0;
         //  lock (Application.StartupPath + "\\" + iniFileName)
           lock (iniFileFullName)
           {
               executeResult = WritePrivateProfileString(section, key, val, iniFileFullName);               
           }
           return executeResult;
       }

        /// <summary>
        /// 读取INI
        /// </summary>
        /// <param name="section">INI文件中的段落名称</param>
        /// <param name="key">INI文件中的关键字</param>
        /// <returns>返回结果</returns>
        public static string ReadINI(string section, string key,string filepath)
       {
           string str;  // 返回值

           StringBuilder sb = new StringBuilder(260);

           GetPrivateProfileString(section, key,
               "", sb, 255, filepath);

           str = Convert.ToString(sb);
           return str;

       }


       /// <summary>
       /// 读取INI
       /// </summary>
       /// <param name="section">INI文件中的段落名称</param>
       /// <param name="key">INI文件中的关键字</param>
       /// <returns>返回结果</returns>
       //public static string ReadINI(string section, string key, string iniFileFullName)
       //{
       //    if (File.Exists(iniFileFullName))
       //    {
       //        string str;  // 返回值

       //        StringBuilder sb = new StringBuilder(260);

       //        GetPrivateProfileString(section, key,
       //            "", sb, 255, iniFileFullName);

       //        str = Convert.ToString(sb);
       //        return str;
       //    }
       //  else
       //  {
       //      return "";
       //  }
       

       //}

     /// <summary>
     /// 删除section
     /// </summary>
     /// <param name="section"></param>
       public static void DeleteSection(string section, string iniFileFullName)
       {
           WritePrivateProfileString(section, null, null,iniFileFullName);
       }

       /// <summary>
       /// 删除某个section下得键
       /// </summary>
       /// <param name="section"></param>
       public static void DeleteKey(string section,string key,string iniFileFullName)
       {
           WritePrivateProfileString(section, key, null,iniFileFullName);

       }

       /// <summary>
       /// 返回该配置文件中所有Section名称的集合
       /// </summary>
       /// <returns></returns>
       public static ArrayList ReadSections(string iniFileFullName)
       {
           byte[] buffer = new byte[65535];
           int rel = GetPrivateProfileSectionNamesA(buffer, buffer.GetUpperBound(0), iniFileFullName);
           int iCnt, iPos;
           ArrayList arrayList = new ArrayList();
           string tmp;
           if (rel > 0)
           {
               iCnt = 0; iPos = 0;
               for (iCnt = 0; iCnt < rel; iCnt++)
               {
                   if (buffer[iCnt] == 0x00)
                   {
                       tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                       iPos = iCnt + 1;
                       if (tmp != "")
                           arrayList.Add(tmp);
                   }
               }
           }
           return arrayList;
       }

       /// <summary>
       /// 获取节点的所有KEY值
       /// </summary>
       /// <param name="sectionName"></param>
       /// <returns></returns>
       public static ArrayList ReadKeys(string sectionName, string iniFileFullName)
       {

           byte[] buffer = new byte[5120];
           int rel = GetPrivateProfileStringA(sectionName, null, "", buffer, buffer.GetUpperBound(0),iniFileFullName);

           int iCnt, iPos;
           ArrayList arrayList = new ArrayList();
           string tmp;
           if (rel > 0)
           {
               iCnt = 0; iPos = 0;
               for (iCnt = 0; iCnt < rel; iCnt++)
               {
                   if (buffer[iCnt] == 0x00)
                   {
                       tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                       iPos = iCnt + 1;
                       if (tmp != "")
                           arrayList.Add(tmp);
                   }
               }
           }
           return arrayList;
       }

       /// <summary>
       /// 获取节点的所有KEY和Value值
       /// </summary>
       /// <param name="sectionName"></param>
       /// <returns></returns>
       public static Dictionary<string, string> ReadKeysAndValue(string sectionName, string iniFileFullName)
       {
           var dic = new Dictionary<string, string>();
           ArrayList keys = ReadKeys(sectionName,  iniFileFullName);
           foreach (string k in keys)
           {
               var v = ReadINI(sectionName, k, iniFileFullName);
               dic.Add(k, v);
           }
           return dic;
       }


    }
}
