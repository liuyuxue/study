using Prism.Events;
using ResourceLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppMain
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        IEventAggregator eventaggregator;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigApp();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            eventaggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        private void ConfigApp()
        {
            try
            { 
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                ConfigStyle();
                ConfigEnvironmentParamters();
                ConfigMQ();
            }
            catch (Exception ex)
            {
       //         logger.Error(ex, "App配置错误");
            }
        }

        private void ConfigStyle()
        {
            SettingHelper.CurrentLanguage = SettingHelper.CN;
            SettingHelper.CurrentStyle = SettingHelper.Style1;
        }

        private void ConfigMQ()
        {
            
        }

        private void ConfigEnvironmentParamters()
        { }


        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

        }
    }
}
