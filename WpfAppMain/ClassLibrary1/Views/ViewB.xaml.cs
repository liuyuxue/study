
using CommonLib.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClassLibrary1.Views
{
    /// <summary>
    /// ViewB.xaml 的交互逻辑
    /// </summary>
    public partial class ViewB : UserControl
    {
        public ViewB()
        {
         //   DataGridRowBehaviours.
            InitializeComponent();

            
        }

        private void dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label ll = new Label() { Width = 111, Height = 33, Content = "123", Background = Brushes.Green, Foreground = Brushes.White };
            PopWindow pp = new PopWindow();
            pp.Plugin = ll;
            pp.Show();
        }
    }


    public  class AA
    {
        public string id { get; set; }
        public string name { get; set; }
        public int age{ get; set; }
    }
}
