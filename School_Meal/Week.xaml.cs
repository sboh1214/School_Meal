using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Week : Page
    {
        public Week()
        {
            this.InitializeComponent();

            DateTime dtToday = DateTime.Now;

            CultureInfo ciCurrent = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek dwFirst = ciCurrent.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek dwToday = ciCurrent.Calendar.GetDayOfWeek(dtToday);

            int iDiff = dwToday - dwFirst;
            DateTime FirstDay = dtToday.AddDays(-iDiff);

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var breakfaststr = "급식 정보 없음";
            var lunchstr = "급식 정보 없음";
            var dinnerstr = "급식 정보 없음";

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            SunB.Text = breakfaststr;
            SunL.Text = lunchstr;
            SunD.Text = dinnerstr;

            FirstDay = FirstDay.AddDays(1);

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            MonB.Text = breakfaststr;
            MonL.Text = lunchstr;
            MonD.Text = dinnerstr;

            FirstDay = FirstDay.AddDays(1);

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            TueB.Text = breakfaststr;
            TueL.Text = lunchstr;
            TueD.Text = dinnerstr;

            FirstDay = FirstDay.AddDays(1);

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            WedB.Text = breakfaststr;
            WedL.Text = lunchstr;
            WedD.Text = dinnerstr;

            FirstDay = FirstDay.AddDays(1);

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            ThuB.Text = breakfaststr;
            ThuL.Text = lunchstr;
            ThuD.Text = dinnerstr;

            FirstDay = FirstDay.AddDays(1);

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            FriB.Text = breakfaststr;
            FriL.Text = lunchstr;
            FriD.Text = dinnerstr;

            FirstDay = FirstDay.AddDays(1);

            breakfaststr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[FirstDay.Year.ToString() + FirstDay.Month.ToString() + FirstDay.Day.ToString() + "D"].ToString();

            SatB.Text = breakfaststr;
            SatL.Text = lunchstr;
            SatD.Text = dinnerstr;

        }

        private void Previous_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Next_ABB_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
