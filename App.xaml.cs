using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace OCREye
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static NotifyIcon trayIcon;

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            RemoveTrayIcon();
            AddTrayIcon();
        }

        private void AddTrayIcon()
        {
            if (trayIcon != null)
            {
                return;
            }
            trayIcon = new NotifyIcon
            {
                Icon = global::OCREye.Properties.Resources.Icon,
                Text = "OCREye"
            };
            trayIcon.Visible = true;

            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();

            System.Windows.Forms.MenuItem closeItem = new System.Windows.Forms.MenuItem();
            closeItem.Text = "Exit";
            closeItem.Click += new EventHandler(delegate { this.Shutdown(); });

            System.Windows.Forms.MenuItem settingsItem = new System.Windows.Forms.MenuItem();
            settingsItem.Text = "Settings";

            menu.MenuItems.Add(settingsItem);
            menu.MenuItems.Add(closeItem);

            trayIcon.ContextMenu = menu;    //设置NotifyIcon的右键弹出菜单
        }

        private void RemoveTrayIcon()
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
                trayIcon = null;
            }
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            RemoveTrayIcon();
        }
    }
    
}
