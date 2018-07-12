using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Microsoft.AppCenter.Analytics;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Week : Page
    {
        public DateTime WeekPageDateTime;

        public Week()
        {
            this.InitializeComponent();
            Analytics.TrackEvent("Week Page");

            DateTime dtToday = DateTime.Now;
            
            CultureInfo ciCurrent = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek dwFirst = ciCurrent.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek dwToday = ciCurrent.Calendar.GetDayOfWeek(dtToday);

            int iDiff = dwToday - dwFirst;
            WeekPageDateTime = dtToday.AddDays(-iDiff);

            ShowWeekMenu();
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekPageDateTime=WeekPageDateTime.AddDays(-7);
            ShowWeekMenu();
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            GetMenu(WeekPageDateTime.Year, WeekPageDateTime.Month);
            ShowWeekMenu();
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekPageDateTime=WeekPageDateTime.AddDays(7);
            ShowWeekMenu();
        }

        public void ShowWeekMenu()
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

                var breakfaststr = "급식 정보 없음";
                var lunchstr = "급식 정보 없음";
                var dinnerstr = "급식 정보 없음";

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Sun.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (일)";
                SunB.Text = breakfaststr;
                SunL.Text = lunchstr;
                SunD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(1);

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Mon.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (월)";
                MonB.Text = breakfaststr;
                MonL.Text = lunchstr;
                MonD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(1);

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Tue.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (화)";
                TueB.Text = breakfaststr;
                TueL.Text = lunchstr;
                TueD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(1);

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Wed.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (수)";
                WedB.Text = breakfaststr;
                WedL.Text = lunchstr;
                WedD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(1);

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Thu.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (목)";
                ThuB.Text = breakfaststr;
                ThuL.Text = lunchstr;
                ThuD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(1);

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Fri.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (금)";
                FriB.Text = breakfaststr;
                FriL.Text = lunchstr;
                FriD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(1);

                breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
                lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
                dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

                Sat.Text = WeekPageDateTime.Month.ToString() + "/" + WeekPageDateTime.Day.ToString() + " (토)";
                SatB.Text = breakfaststr;
                SatL.Text = lunchstr;
                SatD.Text = dinnerstr;

                WeekPageDateTime = WeekPageDateTime.AddDays(-6);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e);

            }
            catch (Exception e)
            {

            }
        }
    }
}
