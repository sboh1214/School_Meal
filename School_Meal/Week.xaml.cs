using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;

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

            DateTime dtToday = DateTime.Now;
            
            CultureInfo ciCurrent = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek dwFirst = ciCurrent.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek dwToday = ciCurrent.Calendar.GetDayOfWeek(dtToday);

            int iDiff = dwToday - dwFirst;
            WeekPageDateTime = dtToday.AddDays(-iDiff);

            ShowWeekMenu();
        }

        private void Previous_ABB_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekPageDateTime.AddDays(-7);
            ShowWeekMenu();
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            GetMenu(WeekPageDateTime.Year, WeekPageDateTime.Month);
            ShowWeekMenu();
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            WeekPageDateTime.AddDays(7);
            ShowWeekMenu();
        }

        private void Next_ABB_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool GetMenu(int Year, int Month)
        {
            var Url = new Uri("http://schoolmenukr.ml/api/ice/E100002238?year=" + Year.ToString() + "&month=" + Month.ToString()); //사이트 주소
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myresponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myresponse.Close();

            dynamic jObject = JsonConvert.DeserializeObject(result);
            foreach (var Jitem in jObject)
            {
                dynamic jdate = Jitem.GetValue("date");
                int date = Convert.ToInt32(jdate.Value);

                dynamic jbreakfast = Jitem.GetValue("breakfast");
                var breakfastlist = jbreakfast.Children();
                string breakfaststr = "";
                foreach (var BreakfastItem in breakfastlist)
                {
                    breakfaststr += BreakfastItem.Value;
                    breakfaststr += "\n";
                }

                dynamic jlunch = Jitem.GetValue("lunch");
                var lunchlist = jlunch.Children();
                string lunchstr = "";
                foreach (var LunchItem in lunchlist)
                {
                    lunchstr += LunchItem.Value;
                    lunchstr += "\n";
                }

                dynamic jdinner = Jitem.GetValue("dinner");
                var dinnerlist = jdinner.Children();
                string dinnerstr = "";
                foreach (var DinnerItem in dinnerlist)
                {
                    dinnerstr += DinnerItem.Value;
                    dinnerstr += "\n";
                }

                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

                localSettings.Values[Year.ToString() + Month.ToString() + date.ToString() + "B"] = breakfaststr;
                localSettings.Values[Year.ToString() + Month.ToString() + date.ToString() + "L"] = lunchstr;
                localSettings.Values[Year.ToString() + Month.ToString() + date.ToString() + "D"] = dinnerstr;
            }
            return true;
        }

        public void ShowWeekMenu()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var breakfaststr = "급식 정보 없음";
            var lunchstr = "급식 정보 없음";
            var dinnerstr = "급식 정보 없음";

            breakfaststr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "B"].ToString();
            lunchstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "L"].ToString();
            dinnerstr = localSettings.Values[WeekPageDateTime.Year.ToString() + WeekPageDateTime.Month.ToString() + WeekPageDateTime.Day.ToString() + "D"].ToString();

            Sun.Text = WeekPageDateTime.Month.ToString()+ "/" + WeekPageDateTime.Day.ToString() + " (일)";
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

            WeekPageDateTime = WeekPageDateTime.AddDays(-7);
        }
    }
}
