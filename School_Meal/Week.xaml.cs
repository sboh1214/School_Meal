using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;
using Windows.Storage;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    public sealed partial class Week : Page
    {
        public SchoolMealClass_UWP WeekClass = new SchoolMealClass_UWP("E100002238");

        public Week()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Week Page");

            ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;
            if (Settings.Values["Alignment"] == null)
            {
                Settings.Values["Alignment"] = "Left";
                ChangeAlignment(TextAlignment.Left);
            }
            else
            {
                string Alignment = Settings.Values["Alignment"].ToString();
                if (Alignment == "Left")
                {
                    ChangeAlignment(TextAlignment.Left);
                }
                else if (Alignment == "Middle")
                {
                    ChangeAlignment(TextAlignment.Center);
                }
                else
                {
                    ChangeAlignment(TextAlignment.Right);
                }
            }

            WeekClass.MoveWeekCursorToToday();
            ShowMenu();
        }

        private void ChangeAlignment(TextAlignment Alignment)
        {
            MonB.TextAlignment = Alignment;
            MonL.TextAlignment = Alignment;
            MonD.TextAlignment = Alignment;
            TueB.TextAlignment = Alignment;
            TueL.TextAlignment = Alignment;
            TueD.TextAlignment = Alignment;
            WedB.TextAlignment = Alignment;
            WedL.TextAlignment = Alignment;
            WedD.TextAlignment = Alignment;
            ThuB.TextAlignment = Alignment;
            ThuL.TextAlignment = Alignment;
            ThuD.TextAlignment = Alignment;
            FriB.TextAlignment = Alignment;
            FriL.TextAlignment = Alignment;
            FriD.TextAlignment = Alignment;
            SatB.TextAlignment = Alignment;
            SatL.TextAlignment = Alignment;
            SatD.TextAlignment = Alignment;
            SunB.TextAlignment = Alignment;
            SunL.TextAlignment = Alignment;
            SunD.TextAlignment = Alignment;
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.MoveWeekCursor(-1);
            ShowMenu();
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.LoadMonthMenu(DeviceType.Win10);
            ShowMenu();
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.MoveWeekCursor(1);
            ShowMenu();
        }

        private bool ShowMenu()
        {
            var Menu = WeekClass.GetWeekMenu(DeviceType.Win10);
            SunB.Text = Menu["SunB"];
            SunL.Text = Menu["SunL"];
            SunD.Text = Menu["SunD"];

            MonB.Text = Menu["MonB"];
            MonL.Text = Menu["MonL"];
            MonD.Text = Menu["MonD"];

            TueB.Text = Menu["TueB"];
            TueL.Text = Menu["TueL"];
            TueD.Text = Menu["TueD"];

            WedB.Text = Menu["WedB"];
            WedL.Text = Menu["WedL"];
            WedD.Text = Menu["WedD"];

            ThuB.Text = Menu["ThuB"];
            ThuL.Text = Menu["ThuL"];
            ThuD.Text = Menu["ThuD"];

            FriB.Text = Menu["FriB"];
            FriL.Text = Menu["FriL"];
            FriD.Text = Menu["FriD"];

            SatB.Text = Menu["SatB"];
            SatL.Text = Menu["SatL"];
            SatD.Text = Menu["SatD"];

            var Date = WeekClass.GetWeekCursor();
            Sun.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (일)";
            Date = Date.AddDays(1);
            Mon.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (월)";
            Date = Date.AddDays(1);
            Tue.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (화)";
            Date = Date.AddDays(1);
            Wed.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (수)";
            Date = Date.AddDays(1);
            Thu.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (목)";
            Date = Date.AddDays(1);
            Fri.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (금)";
            Date = Date.AddDays(1);
            Sat.Text = Date.Month.ToString() + "/" + Date.Day.ToString() + " (토)";

            return true;
        }
    }
}
