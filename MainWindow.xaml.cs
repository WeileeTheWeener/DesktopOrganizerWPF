using DesktopOrganizerWPF.ViewModel;
using Microsoft.Win32;
using System.Windows;

namespace DesktopOrganizerWPF
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModelInstance;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel("init");
            viewModelInstance = (MainWindowViewModel)DataContext;

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");
            UpdateUI();

            AddContextMenuEntries();
            this.Closing += MainWindow_Closing;
        }

        private void Turkish_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");
            UpdateUI();
        }
        private void English_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            UpdateUI();
        }
        private void UpdateUI()
        {
            viewModelInstance.UiElementNames.SettingsString = DesktopOrganizer.Properties.Resources.Settings;
            viewModelInstance.UiElementNames.BrowseString = DesktopOrganizer.Properties.Resources.Browse;
            viewModelInstance.UiElementNames.ExtensionsString = DesktopOrganizer.Properties.Resources.Extensions;
            viewModelInstance.UiElementNames.OrganizeString = DesktopOrganizer.Properties.Resources.Organize;
            viewModelInstance.UiElementNames.AutoOrganizeString = DesktopOrganizer.Properties.Resources.AutoOrganize;
            viewModelInstance.UiElementNames.FolderNameString = DesktopOrganizer.Properties.Resources.FolderName;
            viewModelInstance.UiElementNames.ChangeLangString = DesktopOrganizer.Properties.Resources.ChangeLanguage;
            viewModelInstance.UiElementNames.TargetFolderString = DesktopOrganizer.Properties.Resources.TargetFolder;


            viewModelInstance.UiElementNames.OrganizeAudio = DesktopOrganizer.Properties.Resources.OrganizeAudio;
            viewModelInstance.UiElementNames.OrganizeProgram = DesktopOrganizer.Properties.Resources.OrganizePrograms;
            viewModelInstance.UiElementNames.OrganizeImage = DesktopOrganizer.Properties.Resources.OrganizeImages;
            viewModelInstance.UiElementNames.OrganizeDocument = DesktopOrganizer.Properties.Resources.OrganizeDocuments;
            viewModelInstance.UiElementNames.OrganizeCompressed = DesktopOrganizer.Properties.Resources.OrganizeCompressed;
            
        }
        private void AddContextMenuEntries()
        {
            try
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"*\shell\AddTag");
                if (key != null)
                {
                    key.SetValue("", "Add Tag");
                    key.SetValue("Icon", exePath);

                    RegistryKey commandKey = key.CreateSubKey("command");
                    if (commandKey != null)
                    {
                        commandKey.SetValue("", "\"" + exePath + "\" \"%1\"");
                        commandKey.Close();
                    }

                    key.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add context menu entries: " + ex.Message);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RemoveContextMenuEntries();
        }

        private void RemoveContextMenuEntries()
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree(@"*\shell\AddTag", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to remove context menu entries: " + ex.Message);
            }
        }
    }
}