using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2.Views
{
    public class ViewAViewModel : Prism.Mvvm.BindableBase
    {

        public ViewAViewModel()
        {

        }


        private string _title = "Lib2TitleB";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
