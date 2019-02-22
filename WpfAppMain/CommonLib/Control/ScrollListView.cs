using CommonLib;
using CommonLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Control
{
    /// <summary>
    /// 当ListView嵌套ListView时,鼠标事件被内部ListView截断,外部ListView获取不到鼠标滚动事件,不支持滚动.
    /// 此控件让外部ListView支持滚动.
    /// </summary>
    public class ScrollListView : System.Windows.Controls.ListView
    {
        public ScrollListView() : base()
        {
            this.AddHandler(System.Windows.Controls.ListView.MouseWheelEvent, new System.Windows.Input.MouseWheelEventHandler(listview_MouseWheel), true);
        }
        void listview_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var listview = sender as System.Windows.Controls.ListView;
            try
            {
                var scroll = WpfTree.GetChildObject<System.Windows.Controls.ScrollViewer>(listview, null);
                if (scroll != null)
                    scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
            }
            catch{}
        }
    }
}
