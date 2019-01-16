using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Pixel_3_Toolkit.Properties;

using Ookii.Dialogs.Wpf;


namespace Pixel_3_Toolkit
{
    /// <summary>
    /// Interaction logic for Configurator.xaml
    /// </summary>
    public partial class Configurator : Window
    {
        // ResourceDictionary for Strings
        

        public Configurator()
        {
            InitializeComponent();

            // Check if FirstRun is true, block close and cancel buttons
            if (Properties.Settings.Default.FirstRun)
            {
                cancelBtn.IsEnabled = false;
                this.Closing += ConfiguratorFirstRun_Closing;
            }
            else
            {
                // Load settings and apply to UI elements
                LoadSettings();
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
                platformToolsTxtBx.Text = path;
                return;
            }

            if (textBox == factoryImageLocTxtBx)
            {
                factoryImageLocTxtBx.Text = path;
                return;
            }

            if (textBox == twrpLocTxtBx)
            {
                twrpLocTxtBx.Text = path;
                return;
            }

            if (textBox == magiskLocTxtBx)
            {
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

            if ((bool)dialog.ShowDialog(this))
                return dialog.SelectedPath;
            else
                return null;
        }

        private void LoadSettings()
        {
            platformToolsTxtBx.Text = Settings.Default.ACtrl_Location;
            factoryImageLocTxtBx.Text = Settings.Default.ToolkitData_FactoryImages;
            twrpLocTxtBx.Text = Settings.Default.ToolkitData_TWRP;
            magiskLocTxtBx.Text = Settings.Default.ToolkitData_Magisk;
            // magiskModulesLocTxtBx.Text = Settings.Default.ToolkitData_MagiskModules;

            magiskCustomUrlTxtBx.Text = Settings.Default.MagiskCustomUrl;

            killAdbOnCloseCbk.IsChecked = Settings.Default.KillAdbServerOnExit;
            cleanUpAfterFacImgSetupCbk.IsChecked = Settings.Default.CleanUpAfterFactoryImageFlash;
        }

        private void SaveSettings()
        {
            // Save all details
            Settings.Default.ACtrl_Location = platformToolsTxtBx.Text;
            Settings.Default.ToolkitData_FactoryImages = factoryImageLocTxtBx.Text;
            Settings.Default.ToolkitData_TWRP = twrpLocTxtBx.Text;
            Settings.Default.ToolkitData_Magisk = magiskLocTxtBx.Text;
            //TODO: Magisk modules
            // Properties.Settings.Default.ToolkitData_MagiskModules = magiskModulesLocTxtBx.Text;

            Settings.Default.MagiskCustomUrl = magiskCustomUrlTxtBx.Text;

            Settings.Default.KillAdbServerOnExit = killAdbOnCloseCbk.IsChecked.Value;
            Settings.Default.CleanUpAfterFactoryImageFlash = cleanUpAfterFacImgSetupCbk.IsChecked.Value;

            // Save the settings
            Settings.Default.Save();
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
        }
    }
}
