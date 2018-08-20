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

            Feedback_TextBlock.Visibility = Visibility.Collapsed;
            Email_Textblock.Visibility = Visibility.Collapsed;
            Theme_TextBlock.Visibility = Visibility.Collapsed;

            Analytics.TrackEvent("Settings Page");
            Version_TextBlock.Text = GetAppVersion();
            GetSettings();
        }

        private void GetSettings()
        {
            ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
            var IsBreakfast = Settings.Values["IsBreakfast"];
            var WhenBreakfast = Settings.Values["WhenBreakfast"];
            var IsLunch = Settings.Values["IsLunch"];
            var WhenLunch = Settings.Values["WhenLunch"];
            var IsDinner = Settings.Values["IsDinner"];
            var WhenDinner = Settings.Values["WhenDinner"];
            var Theme = Settings.Values["Theme"];
            var Alignment = Settings.Values["Alignment"];

            if (IsBreakfast == null)
            {
                Settings.Values["IsBreakfast"] = true;
            }
            else if ((bool)IsBreakfast == false)
            {
                Breakfast_CheckBox.IsChecked = true;
            }
            if (IsLunch == null)
            {
                Settings.Values["IsLunch"] = true;
            }
            else if ((bool)IsBreakfast == false)
            {
                Lunch_CheckBox.IsChecked = false;
            }
            if (IsDinner == null)
            {
                Settings.Values["IsDinner"] = true;
            }
            else if ((bool)IsDinner == false)
            {
                Dinner_CheckBox.IsChecked = false;
            }

            if (Theme == null)
            {
                Settings.Values["Theme"] = "System";
            }
            else if ((string)Theme == "Light")
            {
                Light_Radio.IsChecked = true;
            }
            else if ((string)Theme == "Dark")
            {
                Dark_Radio.IsChecked = true;
            }

            if (Alignment == null)
            {
                Settings.Values["Alignment"] = "Left";
            }
            else if ((string)Alignment == "Middle")
            {
                Left_Radio.IsChecked = true;
            }
            else if ((string)Alignment == "Right")
            {
                Right_Radio.IsChecked = true;
            }
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
            Feedback_TextBlock.Visibility = Visibility.Collapsed;
            if (StoreServicesFeedbackLauncher.IsSupported())
            {
                var launcher = StoreServicesFeedbackLauncher.GetDefault();
                bool success = await launcher.LaunchAsync();
                if (success == false)
                {
                    Feedback_TextBlock.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Feedback_TextBlock.Visibility = Visibility.Visible;
            }
        }

        private async void SystemTheme_Button_Click(object sender, RoutedEventArgs e)
        {
            bool result = await Launcher.LaunchUriAsync(new Uri(@"ms-settings:personalization-colors"));
        }

        private async void Email_Button_Click(object sender, RoutedEventArgs e)
        {
            Email_Textblock.Visibility = Visibility.Collapsed;
            var MailUri = new Uri(@"mailto:Windowsapp5.korea@gmail.com");
            // Set the option to show a warning
            var promptOptions = new LauncherOptions
            {
                TreatAsUntrusted = false
            };
            // Launch the URI
            var success = await Launcher.LaunchUriAsync(MailUri, promptOptions);

            if (success == false)
            {
                Email_Textblock.Visibility = Visibility.Visible;
            }
        }

        private void Align_Radio_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
            var Selected = (RadioButton)sender;
            if (Selected.Name == "Left_Radio")
            {
                Settings.Values["Alignment"] = "Left";
            }
            else if (Selected.Name == "Right_Radio")
            {
                Settings.Values["Alignment"] = "Right";
            }
            else
            {
                Settings.Values["Alignment"] = "Middle";
            }
        }

        private void Theme_Radio_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
            var Selected = (RadioButton)sender;
            if (Selected.Name == "Light_Radio")
            {
                Settings.Values["Theme"] = "Light";
            }
            else if (Selected.Name == "Dark_Radio")
            {
                Settings.Values["Theme"] = "Dark";
            }
            else
            {
                Settings.Values["Theme"] = "System";
            }
            Theme_TextBlock.Visibility = Visibility.Visible;
        }
    }
}
