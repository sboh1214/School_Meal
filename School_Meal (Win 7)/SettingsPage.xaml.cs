using System.Windows;
using System.Windows.Controls;

namespace School_Meal__Win_7_
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void RemoveCache_Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
        }

        private void System_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SystemCal_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Theme_Radio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Align_Radio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Week_Radio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Email_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
