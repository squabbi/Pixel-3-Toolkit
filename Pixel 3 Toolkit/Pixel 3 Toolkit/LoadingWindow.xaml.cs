using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
