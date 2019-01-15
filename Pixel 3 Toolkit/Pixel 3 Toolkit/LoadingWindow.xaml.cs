using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Pixel_3_Toolkit.Properties;

using AndroidCtrl.ADB;
using AndroidCtrl.Fastboot;
using AndroidCtrl.Tools;


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

            // Check for first run or upgrade
        }

        public void SetStatus(string message)
        {
            Task.Run(() => statusTxtBlk.Text = message);
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
    }
}
