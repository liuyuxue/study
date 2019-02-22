using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class FileTools
    {
        /// <summary>
        /// 读取指定目录下的指定扩展名的文件列表(filter不带.)
        /// </summary>
        /// <param name="dir"></param>
        public static List<string> GetFiles(string dir, string filter)
        {
            List<string> alist = new List<string>();
            try
            {
                string[] files = Directory.GetFiles(dir);//得到文件
                foreach (string file in files)//循环文件
                {
                    if (filter != null)
                    {
                        string exname = file.Substring(file.LastIndexOf(".") + 1);//得到后缀名
                        if (("." + filter).IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1)//如果后缀名为.txt文件
                        {
                            FileInfo fi = new FileInfo(file);//建立FileInfo对象
                            alist.Add(fi.FullName);//把.txt文件全名加人到FileInfo对象
                        }
                    }
                    else
                    {
                        FileInfo fi = new FileInfo(file);//建立FileInfo对象
                        alist.Add(fi.FullName);//把.txt文件全名加人到FileInfo对象
                    }
                }
            }
            catch
            { }
            return alist;
        }


    }
}
