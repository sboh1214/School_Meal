using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace School_Meal_Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
        public DateTime TodayPageDateTime = DateTime.Now;

        public TodayPage ()
		{
			InitializeComponent ();
            ShowMenu();
        }

        public bool ShowMenu()
        {
            var date = TodayPageDateTime;

            var TodayBreakfast = Preferences.Get(date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "B",null);
            var TodayLunch = Preferences.Get(date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "L", null);
            var TodayDinner = Preferences.Get(date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "D", null);

            if (TodayBreakfast == null && TodayLunch == null && TodayDinner == null)
            {
                GetMenu(date.Year, date.Month);
                TodayBreakfast = Preferences.Get(date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "B", null);
                TodayLunch = Preferences.Get(date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "L", null);
                TodayDinner = Preferences.Get(date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + "D", null);

            }

            if (TodayBreakfast == null)
            {
                Today_Breakfast_Label.Text = "급식 정보 없음";
            }
            else
            {
                Today_Breakfast_Label.Text = TodayBreakfast.ToString();
            }

            if (TodayLunch == null)
            {
                Today_Lunch_Label.Text = "급식 정보 없음";
            }
            else
            {
                Today_Lunch_Label.Text = TodayLunch.ToString();
            }

            if (TodayDinner == null)
            {
                Today_Dinner_Label.Text = "급식 정보 없음";
            }
            else
            {
                Today_Dinner_Label.Text = TodayDinner.ToString();
            }
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

                Preferences.Set(Year.ToString() + Month.ToString() + date.ToString() + "B", breakfaststr);
                Preferences.Set(Year.ToString() + Month.ToString() + date.ToString() + "L", lunchstr);
                Preferences.Set(Year.ToString() + Month.ToString() + date.ToString() + "D", dinnerstr);

            }
            return true;
        }
    }
}