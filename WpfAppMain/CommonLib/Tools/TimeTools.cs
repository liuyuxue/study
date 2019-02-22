using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class TimeTools
    {
        public static DateTime GetTime(string timeStamp)
        {
            if (timeStamp.Length == 10)
                timeStamp += "000";
            long unixTimeStamp = long.Parse(timeStamp);

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddMilliseconds(unixTimeStamp);
            return dt;
        }


        // DateTime时间格式转换为Unix时间戳格式
        public static string DateTimeToStamp(System.DateTime time,int len=10)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var n = (int)(time - startTime).TotalSeconds;
            string timeStamp = n.ToString();
            if (len == 13)
                return timeStamp + "000";
            else
                return timeStamp;
        }

        public static string GetCurrentTimeStamp( int len = 10)
        {
            System.DateTime time = DateTime.Now;
            return DateTimeToStamp(time, len);
            
        }

    }
}
