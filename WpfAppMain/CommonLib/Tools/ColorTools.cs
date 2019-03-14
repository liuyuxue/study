using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class ColorTools
    {
        public static System.Windows.Media.SolidColorBrush GetWpfColorBrushByHexstr(String hexstr)
        {
            var brushConverter = new System.Windows.Media.BrushConverter();
            System.Windows.Media.SolidColorBrush brush = (System.Windows.Media.SolidColorBrush)brushConverter.ConvertFromString(hexstr);
            return brush ;
        }

        public static System.Windows.Media.Color GetWpfColorByHexstr(String hexstr)
        {
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexstr);
            return color;
        }

        public static System.Drawing.Color GetFormColorByHexstr(String hexstr)
        {
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(hexstr);
            return color;
        }


        public static System.Windows.Media.Color GetWpfColorByFormColor(System.Drawing.Color drawColor)
        {
            System.Windows.Media.Color mediaColor = System.Windows.Media.Color.FromArgb(drawColor.A, drawColor.R, drawColor.G, drawColor.B);
            return mediaColor;
        }

        public static System.Drawing.Color GetFormColorByWpfColor(System.Windows.Media.Color mediaColor)
        {
            System.Drawing.Color drawColor = System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
            return drawColor;
        }
    }


}
