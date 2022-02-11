using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms; // NotifyIcon control
using System.Drawing; // Icon

namespace OCREye
{
    public partial class MainWindow : Window
    {
        NotifyIcon notifyIcon;

        void click(object sender, RoutedEventArgs e)
        {
            // Configure and show a notification icon in the system tray
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Text = "OCREye";
            this.notifyIcon.Icon = OCREye.Properties.Resources.Icon;
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(1000);
        }
    }
}
