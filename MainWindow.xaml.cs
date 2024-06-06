using DesktopOrganizer;
using DesktopOrganizerWPF.ViewModel;
using System.Windows;
using System.Windows.Media.Animation;


namespace DesktopOrganizerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsPanel.Visibility == Visibility.Collapsed)
            {
                SettingsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SettingsPanel.Visibility = Visibility.Collapsed;
            }
        }*/
    }
}