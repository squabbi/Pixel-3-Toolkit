using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Pixel_3_Toolkit.Properties;

using Ookii.Dialogs.Wpf;
using log4net;
using System.Reflection;

namespace Pixel_3_Toolkit
{
    /// <summary>
    /// Interaction logic for Configurator.xaml
    /// </summary>
    public partial class Configurator : Window
    {
        // Log4NET
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Configurator()
        {
            InitializeComponent();
            _log.Debug("Configurator window initalised");

            // Check if FirstRun is true, block close and cancel buttons
            if (Properties.Settings.Default.FirstRun)
            {
                _log.Info("First run detected");
                cancelBtn.IsEnabled = false;
                this.Closing += ConfiguratorFirstRun_Closing;
            }
            else
            {
                // Load settings and apply to UI elements
                _log.Debug("Loading settings to UI");
                LoadSettings();
                _log.Debug("Loaded settings to UI");
            }

            // Subscribe all text boxes to double click events
            platformToolsTxtBx.MouseDoubleClick += GenericBrowseDoubleClick;
            factoryImageLocTxtBx.MouseDoubleClick += GenericBrowseDoubleClick;
            twrpLocTxtBx.MouseDoubleClick += GenericBrowseDoubleClick;
            magiskLocTxtBx.MouseDoubleClick += GenericBrowseDoubleClick;
        }

        private void GenericBrowseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Show Open folder dialog
            string path = ShowOpenFolderDialog();

            // Cast sender to button
            TextBox textBox = (TextBox)sender;

            // End the method if no folder is selected.
            if (path == null)
                return;

            if (textBox == platformToolsTxtBx)
            {
                _log.Info($"Set {path} >> platformToolsTxtbx");
                platformToolsTxtBx.Text = path;
                return;
            }

            if (textBox == factoryImageLocTxtBx)
            {
                _log.Info($"Set {path} >> factoryImageLocTxtBx");
                factoryImageLocTxtBx.Text = path;
                return;
            }

            if (textBox == twrpLocTxtBx)
            {
                _log.Info($"Set {path} >> twrpLocTxtBx");
                twrpLocTxtBx.Text = path;
                return;
            }

            if (textBox == magiskLocTxtBx)
            {
                _log.Info($"Set {path} >> magiskLocTxtBx");
                magiskLocTxtBx.Text = path;
            }
        }

        /// <summary>
        /// Shows Ookii Browse for Folder window and returns the path.
        /// </summary>
        /// <returns>Path of selected folder.</returns>
        public string ShowOpenFolderDialog()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Please select a folder.";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            _log.Debug("Showing folder dialog");
            if ((bool)dialog.ShowDialog(this))
                return dialog.SelectedPath;
            else
            {
                _log.Debug("Folder dialog cancelled");
                return null;
            }
        }

        private void LoadSettings()
        {
            platformToolsTxtBx.Text = Settings.Default.ACtrl_Location;
            _log.Debug($"Loaded {Settings.Default.ACtrl_Location} >> platformToolsLocation");

            factoryImageLocTxtBx.Text = Settings.Default.ToolkitData_FactoryImages;
            _log.Debug($"Loaded {Settings.Default.ToolkitData_FactoryImages} >> factoryImageLocation");

            twrpLocTxtBx.Text = Settings.Default.ToolkitData_TWRP;
            _log.Debug($"Loaded {Settings.Default.ToolkitData_TWRP} >> twrpLocation");

            magiskLocTxtBx.Text = Settings.Default.ToolkitData_Magisk;
            _log.Debug($"Loaded {Settings.Default.ToolkitData_Magisk} >> magiskLocation");
            // magiskModulesLocTxtBx.Text = Settings.Default.ToolkitData_MagiskModules;

            magiskCustomUrlTxtBx.Text = Settings.Default.MagiskCustomUrl;
            _log.Debug($"Loaded {Settings.Default.MagiskCustomUrl} >> magiskCustomUrl");

            killAdbOnCloseCbk.IsChecked = Settings.Default.KillAdbServerOnExit;
            _log.Debug($"Loaded Kill ADB on exit >> {Settings.Default.KillAdbServerOnExit}");

            cleanUpAfterFacImgSetupCbk.IsChecked = Settings.Default.CleanUpAfterFactoryImageFlash;
            _log.Debug($"Loaded Clean up after flashing Factory Images >> {Settings.Default.CleanUpAfterFactoryImageFlash}");
        }

        private void SaveSettings()
        {
            // Save all details
            Settings.Default.ACtrl_Location = platformToolsTxtBx.Text;
            _log.Debug($"Saving AndroidCtrl Location >> {platformToolsTxtBx.Text}");

            Settings.Default.ToolkitData_FactoryImages = factoryImageLocTxtBx.Text;
            _log.Debug($"Saving Factory Image Location >> {factoryImageLocTxtBx.Text}");

            Settings.Default.ToolkitData_TWRP = twrpLocTxtBx.Text;
            _log.Debug($"Saving TWRP Location >> {twrpLocTxtBx.Text}");

            Settings.Default.ToolkitData_Magisk = magiskLocTxtBx.Text;
            _log.Debug($"Saving Magisk Location >> {magiskLocTxtBx.Text}");
            //TODO: Magisk modules
            // Properties.Settings.Default.ToolkitData_MagiskModules = magiskModulesLocTxtBx.Text;

            Settings.Default.MagiskCustomUrl = magiskCustomUrlTxtBx.Text;
            _log.Debug($"Saving Magisk Custom Update URL >> {magiskCustomUrlTxtBx.Text}");

            Settings.Default.KillAdbServerOnExit = killAdbOnCloseCbk.IsChecked.Value;
            _log.Debug($"Saving kill ADB server on exit >> {killAdbOnCloseCbk.IsChecked.Value}");

            Settings.Default.CleanUpAfterFactoryImageFlash = cleanUpAfterFacImgSetupCbk.IsChecked.Value;
            _log.Debug($"Saving clean up after flashing Factory Image >> {cleanUpAfterFacImgSetupCbk.IsChecked.Value}");

            // Save the settings
            Settings.Default.Save();
            _log.Debug($"Saving settings");
        }

        private void ConfiguratorFirstRun_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Check if settings have been saved, then allow the window to close, and set FirstRun to false.
            if (Settings.Default.FirstRun)
            {
                MessageBox.Show(Properties.Resources.CannotCloseOnFirstRun, Properties.Resources.Notice, MessageBoxButton.OK, MessageBoxImage.Information);
                e.Cancel = true;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Set FirstRun to false
            Settings.Default.FirstRun = false;
            SaveSettings();

            this.Close();
            _log.Debug("Configurator closing");
        }
    }
}
