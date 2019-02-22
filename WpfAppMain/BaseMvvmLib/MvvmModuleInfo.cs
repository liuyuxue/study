using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BaseMvvmLib
{
    public class MvvmModuleInfo
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public BitmapSource Img { get; set; }
        public DelegateCommand  LoadViewCommand { get; set; }
    }

    public class MvvmModuleInfoManager
    {
        public static readonly string MainRegion = "MainRegion";
        static ObservableCollection<MvvmModuleInfo> _moduleInfos;
        public static ObservableCollection<MvvmModuleInfo> ModuleInfos
        {
            get
            {
                if (_moduleInfos == null)
                    _moduleInfos = new ObservableCollection<MvvmModuleInfo>();
                return _moduleInfos;
            }
            set
            {
                _moduleInfos = value;
            }
        } 
    }
}
