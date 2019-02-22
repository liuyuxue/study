using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Views
{
    public class ViewAViewModel : Prism.Mvvm.BindableBase
    {
        IEventAggregator _ea;
        IUnityContainer _container;
        IRegionManager _regionManager;
        public ViewAViewModel(IEventAggregator ea, IUnityContainer container, IRegionManager regionManager)
        {
            _ea = ea;
            _container = container;
            _regionManager = regionManager;
            
        }

        private string _content = "ContentA";
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }


        private void SendMessage()
        {
            _ea.GetEvent<BaseMvvmLib.Event.DemoEvent>().Publish("Message");
        }

         
    }
}
