using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CommonLib.Control
{
    public class U3dConfig
    {
        public static string U3dFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "HighSpeedStation.8.exe") ;
    }
    
         

    public class UnityView : System.Windows.Controls.Control, IDisposable
    {

        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);
        private Process process;
        private IntPtr unityHwnd;
        private IntPtr thisHandler;
        private Thread thread;
      
        public delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);
        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lparam);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);



        private int WndEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHwnd = hwnd;
            ActivateUnityWindow();
            return 0;
        }

        private void ActivateUnityWindow()
        {
            SendMessage(unityHwnd, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }
        private void DactivateUnityWindow()
        {
            SendMessage(unityHwnd, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
        }

        static UnityView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnityView), new FrameworkPropertyMetadata(typeof(UnityView)));
        }
        public UnityView()
        {
            this.SizeChanged += UnityView_SizeChanged;
        }


        public Window HostWindow
        {
            get { return (Window)GetValue(HostWindowProperty); }
            set { SetValue(HostWindowProperty, value); }
        }

        public static readonly DependencyProperty HostWindowProperty =
            DependencyProperty.Register("HostWindow", typeof(Window), typeof(UnityView), new PropertyMetadata(OnHostWindowChanged));

        private static void OnHostWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UnityView).HostWindowChanged(e.NewValue as Window);
        }

        private void HostWindowChanged(Window window)
        {
            HostWindow.SourceInitialized += HostWindow_SourceInitialized;
            HostWindow.Loaded += HostWindow_Loaded;
            HostWindow.Activated += HostWindow_Activated;
            HostWindow.Deactivated += HostWindow_Deactivated;
        }

        private void HostWindow_Deactivated(object sender, EventArgs e)
        {
            DactivateUnityWindow();
        }

        private void HostWindow_Activated(object sender, EventArgs e)
        {
            ActivateUnityWindow();
        }

        private void HostWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(200);
            EnumChildWindows(thisHandler, WndEnum, IntPtr.Zero);
            Point p = TranslatePoint(new Point(0, 0), HostWindow);
            MoveWindow(unityHwnd, (int)p.X, (int)p.Y, (int)this.ActualWidth, (int)this.ActualHeight, true);
            ActivateUnityWindow();
        }

        private void HostWindow_Closed(object sender, EventArgs e)
        {
            process.CloseMainWindow();
            Thread.Sleep(1000);
            while (process.HasExited == false)
                process.Kill();
        }

        private void HostWindow_SourceInitialized(object sender, EventArgs e)
        {
            thisHandler = new WindowInteropHelper(HostWindow).Handle;
            process = new Process();
            process.StartInfo.FileName = U3dConfig.U3dFilePath;
            process.StartInfo.Arguments = "-parentHWND " + thisHandler.ToInt32() + " " + Environment.CommandLine;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForInputIdle();
        }

        private void UnityView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Point p = TranslatePoint(new Point(0, 0), HostWindow);
            MoveWindow(unityHwnd, (int)p.X, (int)p.Y, (int)this.ActualWidth, (int)this.ActualHeight, true);
            ActivateUnityWindow();
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposed)
        {

            if (process.HasExited)
                return;

            process.CloseMainWindow();
            Thread.Sleep(1000);
            while (process.HasExited == false)
                process.Kill();
        }

        ~UnityView()
        {
            Dispose(true);
        }
    }
}
