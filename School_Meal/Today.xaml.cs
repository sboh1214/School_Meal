using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Meal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class Today : Page
    {
        public DateTime TodayPageDateTime = DateTime.Now;

        public Today()
        {
            this.InitializeComponent();
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            TodayProgressBar_Row.Height = new GridLength(12);
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        public bool ShowMenu()
        {
            var date = TodayPageDateTime;

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var TodayBreakfast = localSettings.Values[date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "B"];
            var TodayLunch = localSettings.Values[date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "L"];
            var TodayDinner = localSettings.Values[date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "D"];

            if (TodayBreakfast == null && TodayLunch == null && TodayDinner == null)
            {
                GetMenu(date.Year, date.Month);
                TodayBreakfast = localSettings.Values[date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "B"];
                TodayLunch = localSettings.Values[date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "L"];
                TodayDinner = localSettings.Values[date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "D"];
            }

            if (TodayBreakfast == null)
            {
                Today_Breakfast_TextBlock.Text = "급식 정보 없음";
            }
            else
            {
                Today_Breakfast_TextBlock.Text = TodayBreakfast.ToString();
            }

            if (TodayLunch == null)
            {
                Today_Lunch_TextBlock.Text = "급식 정보 없음";
            }
            else
            {
                Today_Lunch_TextBlock.Text = TodayLunch.ToString();
            }

            if (TodayDinner == null)
            {
                Today_Dinner_TextBlock.Text = "급식 정보 없음";
            }
            else
            {
                Today_Dinner_TextBlock.Text = TodayDinner.ToString();
            }

            TodayProgressBar_Row.Height = new GridLength(0);
            return true;
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

        private void Back_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayProgressBar_Row.Height = new GridLength(12);
            TodayPageDateTime = TodayPageDateTime.AddDays(-1);
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        private void Refresh_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            TodayProgressBar_Row.Height = new GridLength(12);
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }

        private void Forward_ABB_Click(object sender, RoutedEventArgs e)
        {
            TodayProgressBar_Row.Height = new GridLength(12);
            TodayPageDateTime = TodayPageDateTime.AddDays(1);
            TodayHeader_TextBlock.Text = TodayPageDateTime.Month.ToString()+"월 "+TodayPageDateTime.Day.ToString()+"일 급식";
            ShowMenu();
            TodayProgressBar_Row.Height = new GridLength(0);
        }
    }
}
