using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonLib.AttachProp
{

    //<DataGrid.RowStyle>
    //            <Style TargetType = "DataGridRow" >
    //                < Setter  Property="Attach:DataGridRowBehaviours.DoubleClickCommand" 
    //                         Value="{Binding Path=DataContext.DataGridRowDoubleClickCommand,
    //                    RelativeSource={RelativeSource FindAncestor, AncestorType = { x:Type UserControl }}}"/>
    //            </Style>
    //        </DataGrid.RowStyle>

    public static class DataGridRowBehaviours
    {
        public static DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(DataGridRowBehaviours),
                                           new PropertyMetadata(DoubleClick_PropertyChanged));

        public static void SetDoubleClickCommand(UIElement element, ICommand value)
        {
            element.SetValue(DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClickCommand(UIElement element)
        {
            return (ICommand)element.GetValue(DoubleClickCommandProperty);
        }

        private static void DoubleClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = d as DataGridRow;
            if (row == null) return;

            if (e.NewValue != null)
            {
                row.AddHandler(DataGridRow.MouseDoubleClickEvent, new RoutedEventHandler(DataGrid_MouseDoubleClick));
            }
            else
            {
                row.RemoveHandler(DataGridRow.MouseDoubleClickEvent, new RoutedEventHandler(DataGrid_MouseDoubleClick));
            }
        }

        private static void DataGrid_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;

            if (row != null)
            {
                var cmd = GetDoubleClickCommand(row);
                if (cmd.CanExecute(row.Item))
                    cmd.Execute(row.Item);
            }
        }





        //public static DependencyProperty SingleClickCommandProperty =
        //    DependencyProperty.RegisterAttached("SingleClickCommand", typeof(ICommand), typeof(DataGridRowBehaviours),
        //                                   new PropertyMetadata(SingleClick_PropertyChanged));

        //public static void SetSingleClickCommand(UIElement element, ICommand value)
        //{
        //    element.SetValue(SingleClickCommandProperty, value);
        //}

        //public static ICommand GetSingleClickCommand(UIElement element)
        //{
        //    return (ICommand)element.GetValue(SingleClickCommandProperty);
        //}

        //private static void SingleClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var row = d as DataGridRow;
        //    if (row == null) return;

        //    if (e.NewValue != null)
        //    {
        //        row.AddHandler(DataGridRow.MouseDownEvent, new RoutedEventHandler(DataGrid_MouseSingleClick));
        //    }
        //    else
        //    {
        //        row.RemoveHandler(DataGridRow.MouseDownEvent, new RoutedEventHandler(DataGrid_MouseSingleClick));
        //    }
        //}

        //private static void DataGrid_MouseSingleClick(object sender, RoutedEventArgs e)
        //{
        //    var row = sender as DataGridRow;

        //    if (row != null)
        //    {
        //        var cmd = GetSingleClickCommand(row);
        //        if (cmd.CanExecute(row.Item))
        //            cmd.Execute(row.Item);
        //    }
        //}
    }
}
