using BaseMvvmLib.Event;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClassLibrary1.Views
{
    public class ViewBViewModel : Prism.Mvvm.BindableBase
    {

        IEventAggregator _ea;

        public ViewBViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<DemoEvent>().Subscribe(MessageReceived);

            WorkDoubleClickCommand  = new DelegateCommand<object>(WorkDoubleClickComm);

            ListA.Add(new AA() { id = "id1", name = "name1", age = 1 });
            ListA.Add(new AA() { id = "id2", name = "name2", age = 2 });
            ListA.Add(new AA() { id = "id3", name = "name3", age = 3 });
            ListA.Add(new AA() { id = "id4", name = "name4", age = 4 });
        }

        private string _content = "ContentB";
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private List<AA> _list = new List<AA>();
        public List<AA> ListA
        {
            get { return _list; }
            set { SetProperty(ref _list, value); }
        }


        public DelegateCommand<object>  WorkDoubleClickCommand {get;set;}

        void WorkDoubleClickComm(object oo)
        {
            var data = oo as AA;
            System.Windows.MessageBox.Show(data.id);
        }


        private void MessageReceived(string message)
        {
            
             
            
        }
    }

   

}
