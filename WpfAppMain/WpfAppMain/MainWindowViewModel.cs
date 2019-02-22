using BaseMvvmLib;
using CommonLib;

using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMain
{
    public class MainWindowViewModel: Prism.Mvvm.BindableBase
    {

        IRegionManager _regionManager;
        IUnityContainer _container;
        public MainWindowViewModel(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<MvvmModuleInfo> ModuleInfos
        {
            get { return MvvmModuleInfoManager.ModuleInfos; }
        }

        

       

    }
}
