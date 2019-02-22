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

namespace CommonLib.Control
{
    /// <summary>
    /// PopWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PopWindow : Window
    {
        /// <summary>
        /// 是否拖拽
        /// </summary>
        private bool isDrag;
        public bool IsDrag
        {
            get { return isDrag; }
            set { isDrag = value; }
        }

        public string PopTitle
        {
            get { return (string)GetValue(PopTitleProperty); }
            set { SetValue(PopTitleProperty, value); }
        }

        public static readonly DependencyProperty PopTitleProperty =
            DependencyProperty.Register("PopTitle", typeof(string), typeof(PopWindow), new PropertyMetadata(null));

        public object Plugin
        {
            get { return (object)GetValue(PluginProperty); }
            set { SetValue(PluginProperty, value); }
        }

        public static readonly DependencyProperty PluginProperty =
            DependencyProperty.Register("Plugin", typeof(object), typeof(PopWindow), new PropertyMetadata(null));

        public PopWindow()
        {
            InitializeComponent();
            this.Loaded += PopWindow_Loaded;
        }

        private void PopWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseMove += PopWindow_MouseMove;
        }

        private void PopWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsDrag)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Plugin != null)
            {
                this.Width = (this.Plugin as FrameworkElement).Width + 60;
                this.Height = (this.Plugin as FrameworkElement).Height + 105;
            }
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (this.WindowStartupLocation != WindowStartupLocation.Manual)
            {
                this.Left = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
                this.Top = SystemParameters.PrimaryScreenHeight / 2 - this.Height / 2;
            }
            return base.ArrangeOverride(arrangeBounds);
        }

         
    }
}
