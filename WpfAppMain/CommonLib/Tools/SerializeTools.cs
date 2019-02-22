using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CommonLib.Tools
{
    public class SerializeTools
    {
         
        public static T JsonDeserializeObject<T>(string json)
        {
           var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
           return obj;
        }

        public static string JsonSerializeObject(object obj)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json;
        }


        
         

         

       


    }
}
