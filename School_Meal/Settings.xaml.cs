using Microsoft.AppCenter.Analytics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Settings Page");
        }

        private void RemoveCache_Button_Click(object sender, RoutedEventArgs e)
        {
            SettingProgressBar_Row.Height = new GridLength(16);

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Clear();

            SettingProgressBar_Row.Height = new GridLength(0);
        }
    }
}
