using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceLib
{
   
    public class SettingHelper
    {
        public const string CN = "cn";
        public const string EN = "en";


        public const string Style1 = "Style1";
        public const string Style2 = "Style2";


        private static string currentLanguage = CN;
        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                currentLanguage = value;
                LoadLanguage(currentLanguage);
            }
        }


        private static string currentStyle = Style1;
        public static string CurrentStyle
        {
            get { return currentStyle; }
            set
            {
                currentStyle = value;
                LoadStyle(currentStyle);
            }
        }


        static void LoadLanguage(string currentLanguage)
        {
            var rd = new System.Windows.ResourceDictionary() { Source = new Uri("pack://application:,,,/ResourceLib;component/Language/" + 
                currentLanguage + ".xaml", UriKind.RelativeOrAbsolute) };
            if (System.Windows.Application.Current.Resources.MergedDictionaries.Count == 0)
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(rd);
            else
                System.Windows.Application.Current.Resources.MergedDictionaries[0] = rd;
        }

        static void LoadStyle(string currentstyle)
        {
            var rd = new System.Windows.ResourceDictionary() { Source = new Uri("pack://application:,,,/ResourceLib;component/Style/" + 
                currentstyle + "/Default.xaml", UriKind.RelativeOrAbsolute) };
            if (System.Windows.Application.Current.Resources.MergedDictionaries.Count <2)
                System.Windows.Application.Current.Resources.MergedDictionaries.Add(rd);
            else
                System.Windows.Application.Current.Resources.MergedDictionaries[1] = rd;
        }


    }
}
