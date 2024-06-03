﻿using DesktopOrganizerWPF.ViewModel;
using System.Windows;
using System.Windows.Controls;


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
    }
}