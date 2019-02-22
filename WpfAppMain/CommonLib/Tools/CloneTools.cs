using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class CloneTools
    {
        /// <summary>
        /// 深度克隆对象
        /// </summary>
        public static T CloneObject<T>(object o)
        {
            if (o == null)
                return default(T);
            Type newT = typeof(T);
            Type t = o.GetType();
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            System.Reflection.PropertyInfo[] properties2 = newT.GetProperties();
            Object p = newT.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (System.Reflection.PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    foreach (System.Reflection.PropertyInfo pi2 in properties2)
                    {
                        if (pi2.ToString() == pi.ToString())
                        {
                            object value = pi.GetValue(o, null);
                            pi2.SetValue(p, value, null);
                        }
                    }
                }
            }
            return (T)p;
        }
    }
}
