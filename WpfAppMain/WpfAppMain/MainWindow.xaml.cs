using CommonLib.Tools;
using ResourceLib;
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

namespace WpfAppMain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           
        }
        String key = "7A020000";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string k = tb1.Text.Trim();
            if (k == "")
                return;
            uc.Angle = Convert.ToDouble(k);

            
        }
    }
}
