
using BaseMvvmLib;
using ClassLibrary2.Views;
using CommonLib;

using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace ClassLibrary2
{
    public class ClassLibrary2Module : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        
        public ClassLibrary2Module(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }


        public void Initialize()
        {
            string url = "lib2Main";
            string title = "lib2MainTitle";
            _container.RegisterTypeForNavigation<Views.Main>(url);
            var cmd = new Prism.Commands.DelegateCommand(() =>
            {
                _regionManager.RequestNavigate(MvvmModuleInfoManager.MainRegion, url);
            });
            MvvmModuleInfo mi = new MvvmModuleInfo() { Title = title, Url = url, LoadViewCommand = cmd };
            MvvmModuleInfoManager.ModuleInfos.Add(mi);
        }
    }
}