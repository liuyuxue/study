using Prism.Commands;
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

namespace CommonLib
{

    public partial class PageControl : UserControl
    {
        public PageControl()
        {
            InitializeComponent();
        }


        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(PageControl), new PropertyMetadata(0, OnIndexChanged));

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pager = d as PageControl;
            int current = Convert.ToInt32(e.NewValue);
            bool needUpdate = true;
            foreach (var child in pager.sp_indexs.Children)
            {
                if (child is Button)
                {
                    if ((child as Button).Content.ToString() == e.NewValue.ToString())
                        needUpdate = false;
                }
            }

            if (needUpdate)
            {
                pager.sp_indexs.Children.Clear();
                int total = 5;

                int tmp = pager.TotalPages - current + 1;
                if (total > tmp)
                    total = tmp;
                for (int i = current; i < current + total; i++)
                {
                    Button b = new Button() { Content = i.ToString(), CommandParameter = i };
                    pager.sp_indexs.Children.Add(b);
                }
                if (total < tmp)
                {
                    pager.sp_indexs.Children.Add(pager.GetLabel());
                }
            }

            foreach (var child in pager.sp_indexs.Children)
            {
                if (child is Button)
                {
                    var btn = child as Button;
                    if (btn.Content.ToString() == e.NewValue.ToString())
                        btn.Uid = "1";
                    else
                        btn.Uid = "0";
                }
            }
        }

        public int TotalPages
        {
            get { return (int)GetValue(TotalPagesProperty); }
            set { SetValue(TotalPagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalPagesProperty =
            DependencyProperty.Register("TotalPages", typeof(int), typeof(PageControl), new PropertyMetadata(0, OnTotalPagesChanged));

        private static void OnTotalPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pager = d as PageControl;
            pager.sp_indexs.Children.Clear();
            int total = 5;
            if (total > pager.TotalPages)
                total = pager.TotalPages;
            for (int i = 1; i <= total; i++)
            {
                Button b = new Button() { Content = i.ToString(), CommandParameter = i };
                pager.sp_indexs.Children.Add(b);
                if (i == 1)
                    b.Uid = "1";
            }
            if (total < pager.TotalPages)
            {
                pager.sp_indexs.Children.Add(pager.GetLabel());
            }
        }

        Label GetLabel()
        {
            var lbl = new Label() { Content = ". . .", Background = Brushes.Transparent, Foreground = Brushes.White, Width = 25 };
            lbl.MouseDown += Lbl_MouseDown;
            return lbl;
        }

        private void Lbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //string last = (this.sp_indexs.Children[4] as Button).Content.ToString();
        }

        public static bool DownPageIsEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(DownPageIsEnableProperty);
        }

        public static void DownPageIsEnable(DependencyObject obj, int value)
        {
            obj.SetValue(DownPageIsEnableProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownPageIsEnableProperty =
            DependencyProperty.RegisterAttached("DownPageIsEnable", typeof(bool), typeof(PageControl), new PropertyMetadata(true, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as PageControl;
            element.downbtn.IsEnabled = (bool)e.NewValue;
        }





        public static bool UpPageIsEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpPageIsEnableProperty);
        }

        public static void UpPageIsEnable(DependencyObject obj, int value)
        {
            obj.SetValue(UpPageIsEnableProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpPageIsEnableProperty =
            DependencyProperty.RegisterAttached("UpPageIsEnable", typeof(bool), typeof(PageControl), new PropertyMetadata(true, OnValueChanged2));

        private static void OnValueChanged2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as PageControl;
            element.upbtn.IsEnabled = (bool)e.NewValue;
        }


    }

}
