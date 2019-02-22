
using BaseMvvmLib;
using ClassLibrary1.Views;
using CommonLib;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace ClassLibrary1
{
    public class ClassLibrary1Module : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;


        public ClassLibrary1Module(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
             
            string url = "lib1Main";
            string title = "lib1MainTitle";
            _container.RegisterTypeForNavigation<Views.Main>(url);
            var cmd = new Prism.Commands.DelegateCommand(() =>
            {
                _regionManager.RequestNavigate(MvvmModuleInfoManager.MainRegion, url);
            });
            MvvmModuleInfo mi = new MvvmModuleInfo() { Title = title, Url = url, LoadViewCommand = cmd };
            MvvmModuleInfoManager.ModuleInfos.Add(mi);


            _regionManager.RegisterViewWithRegion("ARegion", typeof(ViewA));
            _regionManager.RegisterViewWithRegion("BRegion", typeof(ViewB));
        }


       



      

    }
}