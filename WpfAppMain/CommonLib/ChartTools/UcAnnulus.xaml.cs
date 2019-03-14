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

namespace CommonLib.ChartTools
{
    /// <summary>
    /// UcAnnulus.xaml 的交互逻辑
    /// </summary>
    public partial class UcAnnulus : UserControl
    {
        public UcAnnulus()
        {
            InitializeComponent();
            
        }


        public String Bg
        {
            get { return (String)GetValue(BgProperty); }
            set { SetValue(BgProperty, value); }
        }

        public static readonly DependencyProperty BgProperty =
            DependencyProperty.Register("Bg", typeof(String), typeof(UcAnnulus), new PropertyMetadata(("#393837"), BgChangedCallback));

        public static void BgChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as UcAnnulus;
            uc.g.Background = Tools.ColorTools.GetWpfColorBrushByHexstr(e.NewValue.ToString());
        }

        public String UnUseColor
        {
            get { return (String)GetValue(UnUseColorProperty); }
            set { SetValue(UnUseColorProperty, value); }
        }

        public static readonly DependencyProperty UnUseColorProperty =
            DependencyProperty.Register("UnUseColor", typeof(String), typeof(UcAnnulus), new PropertyMetadata(("#DCDED7"), UnUseColorChangedCallback));
          
        public static void UnUseColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as UcAnnulus;
            uc.ellipse.Stroke = Tools.ColorTools.GetWpfColorBrushByHexstr(e.NewValue.ToString());
        }

        public String InUseColor
        {
            get { return (String)GetValue(InUseColorProperty); }
            set { SetValue(InUseColorProperty, value); }
        }

        public static readonly DependencyProperty InUseColorProperty =
            DependencyProperty.Register("InUseColor", typeof(String), typeof(UcAnnulus), new PropertyMetadata(("#68A8D7"), InUseColorChangedCallback));

        public static void InUseColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as UcAnnulus;
            uc.path1.Fill = Tools.ColorTools.GetWpfColorBrushByHexstr(e.NewValue.ToString());
        }


        public Double Angle
        {
            get { return (Double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(Double), typeof(UcAnnulus), new PropertyMetadata(0d, AngleChangedCallback )    );

        public static void AngleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as UcAnnulus;
            ArcSegment arc1 = uc.arc1;
            double angle =(double) e.NewValue ;
            if (angle == 360d)
                angle = 359.99;
            while (angle > 360)
                angle = angle - 360;

            double r1 = arc1.Size.Width;
            var x1 = r1 + Math.Sin(angle * Math.PI / 180.0) * r1;
            var y1 = r1 - Math.Cos(angle * Math.PI / 180.0) * r1;
            arc1.Point = new Point(x1, y1);       

            ArcSegment arc2= uc.arc2;
            double r2 = arc2.Size.Width; ;
            double x2 = r1 + Math.Sin(angle * Math.PI / 180.0) * r2;
            double y2 = r1 - Math.Cos(angle * Math.PI / 180.0) * r2;
            arc2.Point = new Point(x2, y2);

            if (angle <= 180)
                arc1.IsLargeArc = arc2.IsLargeArc = false;
            else
                arc1.IsLargeArc = arc2.IsLargeArc = true;
        }
    }
}
