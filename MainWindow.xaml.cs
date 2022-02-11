using Physics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms; // NotifyIcon control
using System.Drawing; // Icon
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OCREye
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Physics physics;
        private const int WM_CLIPBOARDUPDATE = 0x031D;

        private IntPtr windowHandle;

        public event EventHandler ClipboardUpdate;
        public MainWindow()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height-1;
            physics = new Physics(ImageSurface, (int)mainWindow.Width, (int)mainWindow.Height);
            CompositionTarget.Rendering += this.physics.Update;

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            windowHandle = new WindowInteropHelper(this).EnsureHandle();
            HwndSource.FromHwnd(windowHandle)?.AddHook(HwndHandler);
            Start();
        }

        public static readonly DependencyProperty ClipboardUpdateCommandProperty =
            DependencyProperty.Register("ClipboardUpdateCommand", typeof(ICommand), typeof(MainWindow), new FrameworkPropertyMetadata(null));

        public ICommand ClipboardUpdateCommand
        {
            get { return (ICommand)GetValue(ClipboardUpdateCommandProperty); }
            set { SetValue(ClipboardUpdateCommandProperty, value); }
        }

        protected virtual void OnClipboardUpdate()
        {
            OCR.GetOCRImage();
            String Result = OCR.doOCR();
            Stop();
            System.Windows.Forms.Clipboard.SetText(Result);
        }

        public void Start()
        {
            NativeMethods.AddClipboardFormatListener(windowHandle);
        }

        public void Stop()
        {
            NativeMethods.RemoveClipboardFormatListener(windowHandle);
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == WM_CLIPBOARDUPDATE)
            {
                // fire event
                this.ClipboardUpdate?.Invoke(this, new EventArgs());
                // execute command
                if (this.ClipboardUpdateCommand?.CanExecute(null) ?? false)
                {
                    this.ClipboardUpdateCommand?.Execute(null);
                }
                // call virtual method
                OnClipboardUpdate();
            }
            handled = false;
            return IntPtr.Zero;
        }


        private static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AddClipboardFormatListener(IntPtr hwnd);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);
        }

    //private void ImageCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    //{
    //    this.physics.Down(e.GetPosition(this.ImageCanvas).X.ToSimUnits(), e.GetPosition(this.ImageCanvas).Y.ToSimUnits());
    //}

    private void Image_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(01, 00, 00, 00));
            if (e.RightButton.Equals(MouseButtonState.Pressed))
            {
                this.physics.imageDown(e.GetPosition(this.ImageSurface).X.ToSimUnits(), e.GetPosition(this.ImageSurface).Y.ToSimUnits());
            }
            else
            {
                OCR.GetOCRImage();
                if(OCR.image != null)
                {
                    String Result = OCR.doOCR();
                    System.Windows.Forms.Clipboard.SetText(Result);
                }
                else
                {
                    Start();

                    Process p = new Process();
                    p.StartInfo.FileName = Readjson("snipPath");
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Arguments = "snip";
                    p.Start();
                }
                
            }
        }

        private void ImageCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            mainWindow.Background = System.Windows.Media.Brushes.Transparent;
            this.physics.Up();
        }

        private void ImageCanvas_OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.physics.Move(e.GetPosition(this.ImageCanvas).X.ToSimUnits(), e.GetPosition(this.ImageCanvas).Y.ToSimUnits());
        }
        private void Image_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ImageSurface.Height + e.Delta / 20 <= 840 && ImageSurface.Height + e.Delta / 20 >= 30)
            {
                ImageSurface.Height += e.Delta / 20;
                ImageSurface.Width += e.Delta / 20;
                physics.newShape(ImageSurface.Height/2);
                //CompositionTarget.Rendering -= this.physics.Update;
                //CompositionTarget.Rendering += this.physics.Update;
            }
        }

        public static string Readjson(string key)
        {
            string jsonfile = System.Environment.CurrentDirectory + "\\config.json";

            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    var value = o[key].ToString();
                    return value;
                }
            }
        }
    }
}
