using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace CommonLib.Tools
{
    public class AnimalTools
    {


         //调用demo
        //Fun.RunDoubleAnimation(0, itemWidth, 2, new RepeatBehavior(1),grid , new PropertyPath("Width"), false, Play1);
        //void Play1(object sender, EventArgs e)
        //{
        //    RunDoubleAnimation(0, 1, 1.2, new RepeatBehavior(1),
        //        sp_img, new PropertyPath("Opacity"), false, Play3);
        //}

        public static Storyboard RunDoubleAnimation(double from, double to, double seconds, RepeatBehavior repeatBehavior,
         DependencyObject obj, PropertyPath propertyPath, bool autoReverse = false, System.EventHandler completedHandle = null)
        {

            var duration = new Duration(TimeSpan.FromSeconds(seconds));
            Storyboard storyboard = new Storyboard();
            DoubleAnimation ani = new DoubleAnimation();
            ani.AutoReverse = autoReverse;
            ani.From = from;
            ani.To = to;
            ani.Duration = duration;
            ani.RepeatBehavior = repeatBehavior;
            storyboard.Children.Add(ani);
            Storyboard.SetTarget(ani, obj);
            Storyboard.SetTargetProperty(ani, propertyPath);
            if (completedHandle != null)
                storyboard.Completed += completedHandle;
            storyboard.Begin();
            return storyboard;
        }

        //执行动画
        public static Storyboard RunDoubleAnimation(double from, double to, double seconds, RepeatBehavior repeatBehavior,
            DependencyObject obj, DependencyProperty dependencyProperty,
            bool autoReverse = false, System.EventHandler completedHandle = null)
        {
            var path = new PropertyPath(dependencyProperty);
            return RunDoubleAnimation(from, to, seconds, repeatBehavior, obj, path, autoReverse, completedHandle);
        }
    }
}
