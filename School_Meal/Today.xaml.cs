using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class Today : Page
    {
        public SchoolMealClass TodayClass = new SchoolMealClass("E100002238");

        public Today()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Today Page");

            ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
            if (Settings.Values["Alignment"] == null)
            {
                Settings.Values["Alignment"] = "Left";
                Today_Breakfast_TextBlock.TextAlignment = TextAlignment.Left;
                Today_Lunch_TextBlock.TextAlignment = TextAlignment.Left;
                Today_Dinner_TextBlock.TextAlignment = TextAlignment.Left;
            }
            else
            {
                string Alignment = Settings.Values["Alignment"].ToString();
                if (Alignment == "Left")
                {
                    Today_Breakfast_TextBlock.TextAlignment = TextAlignment.Left;
                    Today_Lunch_TextBlock.TextAlignment = TextAlignment.Left;
                    Today_Dinner_TextBlock.TextAlignment = TextAlignment.Left;
                }
                else if (Alignment == "Middle")
                {
                    Today_Breakfast_TextBlock.TextAlignment = TextAlignment.Center;
                    Today_Lunch_TextBlock.TextAlignment = TextAlignment.Center;
                    Today_Dinner_TextBlock.TextAlignment = TextAlignment.Center;
                }
                else
                {
                    Today_Breakfast_TextBlock.TextAlignment = TextAlignment.Right;
                    Today_Lunch_TextBlock.TextAlignment = TextAlignment.Right;
                    Today_Dinner_TextBlock.TextAlignment = TextAlignment.Right;
                }
            }

            TodayClass.MoveDateCursorToToday();
            ShowMenu();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.MoveDateCursor(-1);
            ShowMenu();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.LoadMonthMenu(DeviceType.Win10);
            ShowMenu();
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayClass.MoveDateCursor(1);
            ShowMenu();
            TodayHeader_TextBlock.Text = TodayClass.GetDateCursor("MM월 DD일");
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width>=1008)
            {
                Today_Breakfast_TextBlock.FontSize = 26;
                Today_Lunch_TextBlock.FontSize = 26;
                Today_Dinner_TextBlock.FontSize = 26;
            }
            else if (e.NewSize.Width>=641)
            {
                Today_Breakfast_TextBlock.FontSize = 18;
                Today_Lunch_TextBlock.FontSize = 18;
                Today_Dinner_TextBlock.FontSize = 18;
            }
            else
            {
                Today_Breakfast_TextBlock.FontSize = 12;
                Today_Lunch_TextBlock.FontSize = 12;
                Today_Dinner_TextBlock.FontSize = 12;
            }
        }

        public bool ShowMenu ()
        {
            try
            {
                var Menu = TodayClass.GetDayMenu(DeviceType.Win10);
                Today_Breakfast_TextBlock.Text = Menu["Breakfast"];
                Today_Lunch_TextBlock.Text = Menu["Lunch"];
                Today_Dinner_TextBlock.Text = Menu["Dinner"];
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
