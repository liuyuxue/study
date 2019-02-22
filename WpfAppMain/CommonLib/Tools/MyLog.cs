using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4netHelp.config", Watch = true)]
namespace CommonLib.Tools
{
    public class MyLog
    {
        static object flag = new object();
        static log4net.ILog _log;
        public static log4net.ILog Log4
        {
            get
            {
                if (_log == null)
                {
                    lock (flag)
                    {
                        if (_log == null)
                            _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    }
                }
                return _log;
            }
        }

         
      
        
    }
}
