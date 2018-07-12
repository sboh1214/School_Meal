using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Windows.Data.Json;
using Windows.Storage;

namespace School_Meal
{
    class SchoolMealClass
    {
        private DateTime DateCursor;

        public bool LoadMenu(int Year, int Month)
        {
            try
            {
                //http://schoolmenukr.ml/api/ice/E100002238?year=2018&month=7

                var Url = new Uri("http://schoolmenukr.ml/api/ice/E100002238?year=" + Year.ToString() + "&month=" + Month.ToString()); //사이트 주소
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "GET";
                WebResponse myresponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myresponse.Close();

                var jObject = JsonObject.Parse(result);
                foreach (var JItem_Temp in jObject)
                {
                    var JKey = JItem_Temp.Key;
                    if (JKey.Length != 4)
                    {
                        break;
                    }
                    
                    s

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

                    int breakfastdotindex = breakfaststr.IndexOf('.');
                    int lunchdotindex = lunchstr.IndexOf('.');
                    int dinnerdotindex = dinnerstr.IndexOf('.');

                    if (breakfastdotindex != 0)
                    {
                        breakfaststr = breakfaststr.Substring(0, breakfastdotindex);
                    }
                    if (lunchdotindex != 0)
                    {
                        lunchstr = lunchstr.Substring(0, lunchdotindex);
                    }
                    if (dinnerdotindex != 0)
                    {
                        dinnerstr = dinnerstr.Substring(0, dinnerdotindex);
                    }

                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

                    localSettings.Values[Year.ToString() + Month.ToString() + date.ToString() + "B"] = breakfaststr;
                    localSettings.Values[Year.ToString() + Month.ToString() + date.ToString() + "L"] = lunchstr;
                    localSettings.Values[Year.ToString() + Month.ToString() + date.ToString() + "D"] = dinnerstr;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public Dictionary<string,string> GetDayMenu()
        {
            var DayMealDictionary = new Dictionary<string, string>();

            //TODO

            return DayMealDictionary;
        }

        public Dictionary<string,string> GetWeekMenu()
        {
            var WeekMealDictionary = new Dictionary<string, string>();

            //TODO

            return WeekMealDictionary;
        }

        public bool MoveDateCursorToToday()
        {
            try
            {
                DateCursor = DateTime.Today;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool MoveDateCursor(double Date)
        {
            try
            {
                DateCursor = DateCursor.AddDays(Date);
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
