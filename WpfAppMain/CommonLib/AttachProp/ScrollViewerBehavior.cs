﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonLib.AttachProp
{
    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty HorizontalOffsetProperty = 
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior), 
                new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));
        public static void SetHorizontalOffset(FrameworkElement target, double value)
            => target.SetValue(HorizontalOffsetProperty, value);
        public static double GetHorizontalOffset(FrameworkElement target) 
            => (double)target.GetValue(HorizontalOffsetProperty);
        private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) => 
            (target as System.Windows.Controls.ScrollViewer)?.ScrollToHorizontalOffset((double)e.NewValue);

        public static readonly DependencyProperty VerticalOffsetProperty = 
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerBehavior), 
                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));
        public static void SetVerticalOffset(FrameworkElement target, double value) 
            => target.SetValue(VerticalOffsetProperty, value);
        public static double GetVerticalOffset(FrameworkElement target)
            => (double)target.GetValue(VerticalOffsetProperty);
        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) => 
            (target as System.Windows.Controls.ScrollViewer)?.ScrollToVerticalOffset((double)e.NewValue);
    }
}
