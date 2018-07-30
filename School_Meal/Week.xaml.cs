using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class Week : Page
    {
        public SchoolMealClass WeekClass = new SchoolMealClass();

        public Week()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Week Page");

            WeekClass.MoveWeekCursorToToday();
            WeekClass.GetWeekMenu("Win10");
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.MoveWeekCursor(-1);
            WeekClass.GetWeekMenu("Win10");
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.GetWeekMenu("Win10");
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.MoveWeekCursor(1);
            WeekClass.GetWeekMenu("Win10");
        }
    }
}
