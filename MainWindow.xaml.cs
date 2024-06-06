using DesktopOrganizerWPF.ViewModel;
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


            viewModelInstance.UiElementNames.OrganizeAudio = DesktopOrganizer.Properties.Resources.OrganizeAudio;
            viewModelInstance.UiElementNames.OrganizeProgram = DesktopOrganizer.Properties.Resources.OrganizePrograms;
            viewModelInstance.UiElementNames.OrganizeImage = DesktopOrganizer.Properties.Resources.OrganizeImages;
            viewModelInstance.UiElementNames.OrganizeDocument = DesktopOrganizer.Properties.Resources.OrganizeDocuments;
            viewModelInstance.UiElementNames.OrganizeCompressed = DesktopOrganizer.Properties.Resources.OrganizeCompressed;
            
        }
    }
}