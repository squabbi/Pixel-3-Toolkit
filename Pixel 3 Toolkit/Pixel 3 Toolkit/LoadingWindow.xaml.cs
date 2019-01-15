using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using Pixel_3_Toolkit.Properties;

using AndroidCtrl.ADB;
using AndroidCtrl.Fastboot;
using AndroidCtrl.Tools;
using System.Diagnostics;

namespace Pixel_3_Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();

            if (Debugger.IsAttached)
                Settings.Default.Reset();
        }

        /// <summary>
        /// Method for setting the Status message
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        public void SetStatus(string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                statusTxtBlk.Text = message;
            });
        }

        private void FirstRunCheck()
        {
            // Check if program was first run or upgraded
            if (Settings.Default.UpgradeRequired)
            {
                // Upgrade settings and set flag to false
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();

                // Check for First Run after transferring previous settings
                if (Settings.Default.FirstRun)
                {
                    // Open Configurator as dialog
                    Configurator conf = new Configurator();
                    conf.Owner = Window.GetWindow(this);
                    conf.ShowDialog();
                }
            }
        }

        private void SetupAndroidCtrl()
        {
            // Extract AndroidCtrl files if none are found
            if (!ADB.IntegrityCheck())
            {
                Deploy.ADB();
            }

            if (!Fastboot.IntegrityCheck())
            {
                Deploy.Fastboot();
            }

            // Start Monitoring services
            // Check if ADB server is already running, and check if it is mismatched

            if (!ADB.IsStarted || !ADB.IntegrityVersionCheck())
            {
                ADB.Stop();
                ADB.Start();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Check for first run or upgrade
            SetStatus(Strings.ConfiguringSettings);
            FirstRunCheck();
        }
    }
}
