using Microsoft.AppCenter.Analytics;
using Microsoft.Services.Store.Engagement;
using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();

            Analytics.TrackEvent("Settings Page");
            Version_TextBlock.Text = GetAppVersion();
        }

        public static string GetAppVersion()
        {

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

        }

        private void RemoveCache_Button_Click(object sender, RoutedEventArgs e)
        {
            SettingProgressBar_Row.Height = new GridLength(16);

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Clear();

            SettingProgressBar_Row.Height = new GridLength(0);
        }

        private async void Feedback_Button_Click(object sender, RoutedEventArgs e)
        {
            if (StoreServicesFeedbackLauncher.IsSupported())
            {
                var launcher = StoreServicesFeedbackLauncher.GetDefault();
                await launcher.LaunchAsync();
            }
            else
            {
                var MailUri = new Uri(@"mailto:Windowsapp5.korea@gmail.com");

                // Set the option to show a warning
                var promptOptions = new LauncherOptions
                {
                    TreatAsUntrusted = false
                };

                // Launch the URI
                var success = await Launcher.LaunchUriAsync(MailUri, promptOptions);
            }
        }

        private void WindowsTheme_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Theme_Radio_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
