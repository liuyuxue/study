using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonLib.AttachProp
{
    public class GridLineBehavior
    {

        //使用前提:gird必须设Grid.ColumnDefinitions和Grid.RowDefinitions,即使只有1行或1列也要设.

        public static bool GetShowBorder(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowBorderProperty);
        }

        public static void SetShowBorder(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowBorderProperty, value);
        }


        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(GridLineBehavior), new PropertyMetadata(OnShowBorderChanged));


        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var grid = d as System.Windows.Controls.Grid;
            if ((bool)e.OldValue)
            {
                grid.Loaded -= (s, arg) => { };
            }
            if ((bool)e.NewValue)
            {
                grid.Loaded += (s, arg) =>
                {
                    //根据Grid的顶层子控件的个数去添加边框，但不支持单元格合并的情况
                    var controls = grid.Children;
                    var count = controls.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var item = controls[i] as FrameworkElement;
                        var row = Grid.GetRow(item); 
                        var column = Grid.GetColumn(item);
                        var rowspan = Grid.GetRowSpan(item);
                        var columnspan = Grid.GetColumnSpan(item);

                        var border = new Border();
                        border.BorderBrush = myBorderBrush;//new SolidColorBrush(Colors.Red);

                        if (row == grid.RowDefinitions.Count - 1 && column == grid.ColumnDefinitions.Count - 1)
                            border.BorderThickness = new Thickness(myBorderWidth);
                        else if (row == grid.RowDefinitions.Count - 1)
                            border.BorderThickness = new Thickness(myBorderWidth, myBorderWidth, 0, myBorderWidth);


                        else if (column == grid.ColumnDefinitions.Count - 1)
                            border.BorderThickness = new Thickness(myBorderWidth, myBorderWidth, myBorderWidth, 0);
                        else
                            border.BorderThickness = new Thickness(myBorderWidth, myBorderWidth, 0, 0);

                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, column);
                        Grid.SetRowSpan(border, rowspan);
                        Grid.SetColumnSpan(border, columnspan);
                        grid.Children.Add(border);
                    }
                };
            }
        }



        static double myBorderWidth = 0.5;
        public static double GetBorderWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(BorderWidthProperty);
        }
           
        public static void SetBorderWidth(DependencyObject obj, double value)
        {
            obj.SetValue(BorderWidthProperty, value);
        }

        public static readonly DependencyProperty BorderWidthProperty =
            DependencyProperty.RegisterAttached("BorderWidth", typeof(double), typeof(GridLineBehavior), new PropertyMetadata(OnBorderWidthChanged));

        private static void OnBorderWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            myBorderWidth = (double)e.NewValue;
        }



         
        static SolidColorBrush myBorderBrush =  Brushes.Red;
        public static double GetBorderLineBrush(DependencyObject obj)
        {
            return (double)obj.GetValue(BorderLineBrushProperty);
        }

        public static void SetBorderLineBrush(DependencyObject obj, double value)
        {
            obj.SetValue(BorderLineBrushProperty, value);
        }

        public static readonly DependencyProperty BorderLineBrushProperty =
            DependencyProperty.RegisterAttached("BorderLineBrush", typeof(SolidColorBrush), typeof(GridLineBehavior), new PropertyMetadata(OnBorderBrushChanged));

        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            myBorderBrush = (SolidColorBrush)e.NewValue;
        }




    }
 
}
