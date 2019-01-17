using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;

using Pixel_3_Toolkit.Properties;
using Pixel_3_Toolkit.Models;

using AndroidCtrl.ADB;
using AndroidCtrl.Fastboot;
using AndroidCtrl.Tools;

using Newtonsoft.Json;

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

            // Reset settings if debugging
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

        private void SetupWorkingDir()
        {
            // Create directories if they do not exist, check user preferences.
            Directory.CreateDirectory("./ToolkitData");
            // If user settings aren't legit folders, create them as subdirs within ToolkitData
            if (!Directory.Exists(Settings.Default.ToolkitData_FactoryImages))
                Directory.CreateDirectory(String.Format("./ToolkitData/{0}", Settings.Default.ToolkitData_FactoryImages));

            if (!Directory.Exists(Settings.Default.ToolkitData_TWRP))
                Directory.CreateDirectory(String.Format("./ToolkitData/{0}", Settings.Default.ToolkitData_TWRP));

            if (!Directory.Exists(Settings.Default.ToolkitData_Magisk))
                Directory.CreateDirectory(String.Format("./ToolkitData/{0}", Settings.Default.ToolkitData_Magisk));

            Directory.CreateDirectory(String.Format("./ToolkitData/{0}/Modules", Settings.Default.ToolkitData_Magisk));
        }

        private async void DownloadData()
        {
            
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check for first run or upgrade
                SetStatus(Properties.Resources.ConfiguringSettings);
                FirstRunCheck();

                SetStatus(Properties.Resources.SettingUpWorkingDirectory);
                SetupWorkingDir();

                SetStatus(Properties.Resources.SettingUpAndroidCtrl);
                await Task.Run(() => SetupAndroidCtrl());

                SetStatus(Properties.Resources.DownloadingData);
                await Task.Run(() => DownloadData());
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(ioEx.Message);
                return;
            }
        }
    }
}
