using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public interface IPage
    {
        DelegateCommand FirstPageCmd { get; set; }

        DelegateCommand LastPageCmd { get; set; }

        DelegateCommand DownPageCmd { get; set; }

        DelegateCommand UpPageCmd { get; set; }

        DelegateCommand<object> GoToPageCmd { get; set; }

        int PageCount { get; set; }
        int CurrentPageIndex { get; set; }
    }
}
