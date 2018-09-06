using System;
using System.Windows;

namespace School_Meal__Win_7_
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Main_Frame.Navigate(new Uri("TodayPage.xaml",UriKind.Relative));
        }

        private void DayMeal_DockButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Navigate(new Uri("TodayPage.xaml", UriKind.Relative));
        }

        private void WeekMeal_DockButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Navigate(new Uri("WeekPage.xaml", UriKind.Relative));
        }

        private void Settings_DockButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Navigate(new Uri("SettingsPage.xaml", UriKind.Relative));
        }

        private void Web_DockButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Frame.Navigate(new Uri("SchoolWebPage.xaml", UriKind.Relative));
        }
    }
}
