using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Analytics;
using System;
using System.Diagnostics;

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
            ShowMenu();
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.MoveWeekCursor(-1);
            ShowMenu();
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.LoadMonthMenu("Win10");
            ShowMenu();
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekClass.MoveWeekCursor(1);
            ShowMenu();
        }

        private bool ShowMenu()
        {
            try
            {
                var Menu = WeekClass.GetWeekMenu("Win10");

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
                FriB.Text = Menu["FriB"];
                FriL.Text = Menu["FriL"];
                FriD.Text = Menu["FriD"];
                SatB.Text = Menu["SatB"];
                SatL.Text = Menu["SatL"];
                SatD.Text = Menu["SatD"];
                
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
