using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;
using System.Deployment;

using Pixel_3_Toolkit.Util;
using Pixel_3_Toolkit.Properties;
using Pixel_3_Toolkit.Models;

using AndroidCtrl.ADB;
using AndroidCtrl.Fastboot;
using AndroidCtrl.Tools;

using Newtonsoft.Json;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Pixel_3_Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public LoadingWindow()
        {
            InitializeComponent();

            // Initalise Log4Net logging
            XmlConfigurator.Configure();

            // Reset settings if debugging
            if (Debugger.IsAttached)
            {
                _log.Debug("Clearing settings");
                Settings.Default.Reset();
            }
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
            _log.Debug("Checking to see if the program settings require upgrading");
            if (Settings.Default.UpgradeRequired)
            {
                // Upgrade settings and set flag to false
                _log.Info("Upgrade required... Upgrading previous settings");
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();

                // Check for First Run after transferring previous settings
                _log.Debug("Checking if this is the first run");
                if (Settings.Default.FirstRun)
                {
                    // Open Configurator as dialog
                    _log.Info("Determined first run.");
                    Configurator conf = new Configurator();
                    conf.Owner = Window.GetWindow(this);
                    _log.Debug("Showing Configurator window");
                    conf.ShowDialog();
                }
            }
        }

        private void SetupAndroidCtrl()
        {
            // Extract AndroidCtrl files if none are found
            if (!ADB.IntegrityCheck())
            {
                _log.Info("ADB files missing, deploying");
                // If AndroidCtrl path is empty, deply to ./ToolkitData/adb
                if (String.IsNullOrEmpty(Settings.Default.ACtrl_Location))
                {
                    Deploy.ADB("./ToolkitData/platform-tools");
                    _log.Info("ADB deployed to ./ToolkitData/platform-tools");
                }
                else
                {
                    Deploy.ADB(Settings.Default.ACtrl_Location);
                    _log.Info($"ADB deployed to {Settings.Default.ACtrl_Location}");
                }
            }

            if (!Fastboot.IntegrityCheck())
            {
                _log.Info("fastboot files missing, deploying");
                // If AndroidCtrl path is empty, deply to ./ToolkitData/adb
                if (String.IsNullOrEmpty(Settings.Default.ACtrl_Location))
                {
                    Deploy.Fastboot("./ToolkitData/platform-tools");
                    _log.Info("ADB deployed to ./ToolkitData/platform-tools");
                }
                else
                {
                    Deploy.Fastboot(Settings.Default.ACtrl_Location);
                    _log.Info($"ADB deployed to {Settings.Default.ACtrl_Location}");
                }
            }

            // Start Monitoring services
            // Check if ADB server is already running, and check if it is mismatched

            if (!ADB.IsStarted || !ADB.IntegrityVersionCheck())
            {
                _log.Warn("ADB server outdated or not started");
                ADB.Stop();
                _log.Info("Starting ADB server...");
                ADB.Start();
            }
        }

        private void SetupWorkingDir()
        {
            // Create directories if they do not exist, check user preferences.
            Directory.CreateDirectory("./ToolkitData/Client");

            // If user settings aren't legit folders, create them as subdirs within ToolkitData
            if (!Directory.Exists(Settings.Default.ToolkitData_FactoryImages))
                Directory.CreateDirectory(String.Format("./ToolkitData/{0}", Settings.Default.ToolkitData_FactoryImages));

            if (!Directory.Exists(Settings.Default.ToolkitData_TWRP))
                Directory.CreateDirectory(String.Format("./ToolkitData/{0}", Settings.Default.ToolkitData_TWRP));

            if (!Directory.Exists(Settings.Default.ToolkitData_Magisk))
                Directory.CreateDirectory(String.Format("./ToolkitData/{0}", Settings.Default.ToolkitData_Magisk));

            Directory.CreateDirectory(String.Format("./ToolkitData/{0}/Modules", Settings.Default.ToolkitData_Magisk));
        }

        private async Task DownloadFileAsync(string[] downloadsProperties)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string saveLocation = "./ToolkitData/Client/" + downloadsProperties[0];
                    webClient.Credentials = CredentialCache.DefaultNetworkCredentials;

                    _log.Info($"Downloading {downloadsProperties[0]}");
                    await webClient.DownloadFileTaskAsync(new Uri(downloadsProperties[1]), saveLocation);
                }
            }
            catch (Exception)
            {
                _log.Error($"Failed to download {downloadsProperties[0]}");
            }
        }

        private async Task DownloadClientFiles()
        {
            await Task.WhenAll(Constants.clientDownloadsList.Select(download => DownloadFileAsync(download)));
        }

        private async Task CheckForUpdates(Client client)
        {
            // Compare client version
            try
            {
                Tools.Instance.CheckVersion(client.Toolkit);
            }
            catch (Exceptions.RemoteVersionCodeMalformedException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check for first run or upgrade
                SetStatus(Properties.Resources.ConfiguringSettings);
                _log.Info("Configuring settings");
                _log.Debug("Checking FirstRun");
                FirstRunCheck();

                SetStatus(Properties.Resources.SettingUpWorkingDirectory);
                SetupWorkingDir();

                SetStatus(Properties.Resources.SettingUpAndroidCtrl);
                await Task.Run(() => SetupAndroidCtrl());

                SetStatus(Properties.Resources.DownloadingData);
                await DownloadClientFiles();

                SetStatus(Properties.Resources.CheckingForUpdates);
                await CheckForUpdates();
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(ioEx.Message);
                return;
            }
        }
    }
}
