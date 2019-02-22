using System.Collections.Generic;

namespace CommonLib.Tools
{
    public class TxtHelper
    {
        public static void Write_txt(string FILE_NAME, string str)   //向文本内写入字符串
        {
            System.IO.StreamWriter WriteTXT;  //创建一个流失文件
            if (System.IO.File.Exists(FILE_NAME))  //判断是否有FILE_NAME名称的文件存在 
            {
                WriteTXT = System.IO.File.AppendText(FILE_NAME);  //实例化流矢操作
            }
            else                        //不存在FILE_NAME名称的文件
            {
                WriteTXT = System.IO.File.CreateText(FILE_NAME);  //创建一个FILE_NAME的文件
            }
            WriteTXT.WriteLine(str); //向FILE_NAME文件名称的TXT文件逐行追加字符串
            WriteTXT.Close();  //关闭文件
        }

        public List<string> ReadTxt(string path)
        {
            var lst = new List<string>();
            System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            while (!reader.EndOfStream)
            {
                lst.Add(reader.ReadLine()); //一次读取一行
            }
            reader.Close();
            stream.Close();
            return lst;
        }
    }
}
