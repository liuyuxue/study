using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class WinTools
    {
        //private const int WM_SYSCOMMAND = 274;

        //private const int SC_CLOSE = 61536;

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);


        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hwnd, int msg, uint wParam, uint lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetCursorPos(out WinPOINT pt);

        public static IntPtr FindWindow(string winTitle)
        {
            return  FindWindow(winTitle, null);
        }

        /// 按照标题关闭窗口
        public static bool CloseWindowByTitle(string sValue)
        {
            System.Threading.Thread.Sleep(500);
            IntPtr maindHwnd = FindWindow(null, sValue); //自动关闭弹出的错误提示窗体
            if (maindHwnd != IntPtr.Zero)
            {
                SendMessage(maindHwnd, 0x0010, 0, 0);
                return true;
            }
            return false;
        }

        public static void ShowScreenKeyboard()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "C:\\Program Files\\Common Files\\Microsoft Shared\\ink\\TabTip.exe"
            });
        }

        public static void HideScreenKeyboard()
        {
            IntPtr tabTipWinHandler = FindWindow("IPTip_Main_Window");
            if (tabTipWinHandler != IntPtr.Zero)
            {
                PostMessage(tabTipWinHandler, 274, 61536u, 0u);
            }
        }


        public static System.Windows.Point GetCursorPos()
        {
            System.Windows.Point retPoint = new System.Windows.Point(0.0, 0.0);
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                WinPOINT pt;
                GetCursorPos(out pt);
                retPoint.X = (double)pt.X * 96.0 / (double)graphics.DpiX;
                retPoint.Y = (double)pt.Y * 96.0 / (double)graphics.DpiY;
            }
            return retPoint;
        }

        


    }


    public struct WinPOINT
    {
        public int X;

        public int Y;

        public WinPOINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

}
