using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Windows.Data.Json;
using Windows.Storage;
using Xamarin.Essentials;

namespace School_Meal
{
    public class SchoolMealClass
    {
        private DateTime DateCursor;
        private DateTime WeekCursor;
        private IJsonValue JItem_Day;
        private IJsonValue JItem_Breakfast;
        private IJsonValue JItem_Lunch;
        private IJsonValue JItem_Dinner;

        public bool LoadMonthMenu(string DeviceType)
        {
            return LoadMonthMenu(DateCursor.Year, DateCursor.Month, DeviceType);
        }

        public bool LoadMonthMenu(int Year, int Month, string DeviceType) //DeviceType : "Win10", "Win7", "XF"
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

                var JObject = JsonObject.Parse(result).GetObject();
                foreach (var JItem_Date in JObject)
                {
                    if (JItem_Date.Key.Length != 4)
                    {
                        break;
                    }

                    foreach (var JItem_Temp in JItem_Date.Value.GetObject())
                    {
                        switch (JItem_Temp.Key)
                        {
                            case "date":
                                JItem_Day = JItem_Temp.Value;
                                break;
                            case "breakfast":
                                JItem_Breakfast = JItem_Temp.Value;
                                break;
                            case "lunch":
                                JItem_Lunch = JItem_Temp.Value;
                                break;
                            case "dinner":
                                JItem_Dinner = JItem_Temp.Value;
                                break;
                        }
                    }

                    var Breakfast_Array = JItem_Breakfast.GetArray();
                    var Lunch_Array = JItem_Lunch.GetArray();
                    var Dinner_Array = JItem_Dinner.GetArray();

                    string Breakfast_String = "";
                    string Lunch_String = "";
                    string Dinner_String = "";

                    int Day = Convert.ToInt32(JItem_Day.GetNumber());

                    for (int i=0; i<Breakfast_Array.Count; i++)
                    {
                        string Breakfast_TempString = RemoveAllergyInfo(Breakfast_Array[i].GetString());
                        Breakfast_String += Breakfast_TempString;
                    }
                    for (int j=0; j<Lunch_Array.Count; j++)
                    {
                        string Lunch_TempString = RemoveAllergyInfo(Lunch_Array[j].GetString());
                        Lunch_String += Lunch_TempString;
                    }
                    for (int k=0; k<Dinner_Array.Count; k++)
                    {
                        string Dinner_TempString = RemoveAllergyInfo(Dinner_Array[k].GetString());
                        Dinner_String += Dinner_TempString;
                    }

                    switch(DeviceType)
                    {
                        case "Win10":

                            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                            localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "B"] = Breakfast_String;
                            localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "L"] = Lunch_String;
                            localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "D"] = Dinner_String;
                            break;

                        case "Win7":
                            
                            break;

                        case "XF":
                            Preferences.Set(Year.ToString() + Month.ToString() + Day.ToString() + "B", Breakfast_String);
                            Preferences.Set(Year.ToString() + Month.ToString() + Day.ToString() + "L", Lunch_String);
                            Preferences.Set(Year.ToString() + Month.ToString() + Day.ToString() + "D", Dinner_String);
                            break;

                        default:
                            break;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public Dictionary<string,string> GetDayMenu(string DeviceType)
        {
            return GetDayMenu(DateCursor.Year, DateCursor.Month, DateCursor.Day, DeviceType);
        }

        public Dictionary<string, string> GetDayMenu(DateTime Date,string DeviceType)
        {
            return GetDayMenu(Date.Year, Date.Month, Date.Day, DeviceType);
        }

        public Dictionary<string,string> GetDayMenu(int Year, int Month, int Day, string DeviceType)
        {
            var DayMealDictionary = new Dictionary<string, string>();

            switch (DeviceType)
            {
                case "Win10":

                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    string Win10_Breakfast = localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "B"].ToString();
                    string Win10_Lunch =localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "L"].ToString();
                    string Win10_Dinner =localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "D"].ToString();

                    DayMealDictionary.Add("Breakfast", Win10_Breakfast);
                    DayMealDictionary.Add("Lunch", Win10_Lunch);
                    DayMealDictionary.Add("Dinner", Win10_Dinner);

                    break;

                case "Win7":
                    

                    break;

                case "XF":

                    string XF_Breakfast = Preferences.Get(Year.ToString() + Month.ToString() + Day.ToString() + "B", null);
                    string XF_Lunch = Preferences.Get(Year.ToString() + Month.ToString() + Day.ToString() + "L", null);
                    string XF_Dinner = Preferences.Get(Year.ToString() + Month.ToString() + Day.ToString() + "D", null);

                    DayMealDictionary.Add("Breakfast", XF_Breakfast);
                    DayMealDictionary.Add("Lunch",XF_Lunch);
                    DayMealDictionary.Add("Dinner",XF_Dinner);

                    break;

                default:
                    break;
            }

            return DayMealDictionary;
        }

        public Dictionary<string,string> GetWeekMenu(int Year, int Month, int Day, string DeviceType)
        {
            var WeekMealDictionary = new Dictionary<string, string>();

            DateTime dtToday = DateTime.Now;
            CultureInfo ciCurrent = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek dwFirst = ciCurrent.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek dwToday = ciCurrent.Calendar.GetDayOfWeek(dtToday);
            int iDiff = dwToday - dwFirst;
            var TempWeekCursor = dtToday.AddDays(-iDiff);

            switch (DeviceType)
            {
                case "Win10":
                    WeekMealDictionary = GetWeekMenu_Win10(TempWeekCursor);
                    break;

                case "Win7":
                    WeekMealDictionary = GetWeekMenu_Win7(TempWeekCursor);
                    break;

                case "XF":
                    WeekMealDictionary = GetWeekMenu_XF(TempWeekCursor);
                    break;
            }
            return WeekMealDictionary;
        }

        private Dictionary<string,string> GetWeekMenu_Win10(DateTime TempWeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string,string>();
            var Settings = ApplicationData.Current.LocalSettings;
            string YearMonth = TempWeekCursor.Year.ToString() + TempWeekCursor.Month.ToString();

            WeekMealDictionary.Add("MonB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("MonL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("MonD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("TueB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("TueL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("TueD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("WedB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("WedL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("WedD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("ThuB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("ThuL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("ThuD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("FriB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("FriL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("FriD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SatB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("SatL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("SatD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SunB", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"].ToString());
            WeekMealDictionary.Add("SunL", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"].ToString());
            WeekMealDictionary.Add("SunD", Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"].ToString());
            TempWeekCursor.AddDays(-7);

            return WeekMealDictionary;
        }

        private Dictionary<string,string> GetWeekMenu_Win7(DateTime WeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string,string>();



            return WeekMealDictionary;
        }

        private Dictionary<string,string> GetWeekMenu_XF(DateTime TempWeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string,string>();
            string YearMonth = TempWeekCursor.Year.ToString()+TempWeekCursor.Month.ToString();

            WeekMealDictionary.Add("MonB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("MonL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("MonD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("TueB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("TueL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("TueD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("WedB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("WedL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("WedD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("ThuB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("ThuL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("ThuD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("FriB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("FriL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("FriD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SatB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("SatL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("SatD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SunB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B",null));
            WeekMealDictionary.Add("SunL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L",null));
            WeekMealDictionary.Add("SunD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D",null));
            TempWeekCursor.AddDays(-7);

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

        public bool MoveWeekCursorToToday()
        {
            try
            {
                DateTime dtToday = DateTime.Now;
                CultureInfo ciCurrent = System.Threading.Thread.CurrentThread.CurrentCulture;
                DayOfWeek dwFirst = ciCurrent.DateTimeFormat.FirstDayOfWeek;
                DayOfWeek dwToday = ciCurrent.Calendar.GetDayOfWeek(dtToday);
                int iDiff = dwToday - dwFirst;
                WeekCursor = dtToday.AddDays(-iDiff);

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

        public bool MoveWeekCursor(double Week)
        {
            try
            {
                WeekCursor = WeekCursor.AddDays(7*Week);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public DateTime GetDateCursor()
        {
            return DateCursor;
        }

        public string GetDateCursor(string Type)
        {
            string Year = DateCursor.Year.ToString();
            string Month = DateCursor.Month.ToString();
            string Day = DateCursor.Day.ToString();

            switch (Type)
            {
                case "YYYYMMDD": case "yyyymmdd":
                    return Year + Month + Day;
                case "YYMMDD": case "yymmdd":
                    return Year.Substring(2,2) + Month + Day;
                case "YYYYMM": case "yyyymm":
                    return Year + Month;
                case "YYMM": case "yymm":
                    return Year.Substring(2, 2) + Month;
                case "MMDD": case "mmdd":
                    return Month + Day;
                case "YYYY": case "yyyy":
                    return Year;
                case "YY": case "yy":
                    return Year.Substring(2, 2);
                case "MM": case "mm":
                    return Month;
                case "DD": case "dd":
                    return Day;
                case "YYYY년 MM월 DD일": case "yyyy년 mm월 dd일":
                    return Year + "년 " + Month+ "월 " + Day+"일";
                case "MM월 DD일": case "mm월 dd일":
                    return Month+"월 " + Day+"일";
                default:
                    return "Error";
            }
        }

        public DateTime GetWeekCursor()
        {
            return WeekCursor;
        }

        private string RemoveAllergyInfo(string Input)
        {
            int cursor = Input.Length;
            while (true)
            {
                if (IsDot(Input,cursor-1)==true && IsNumber(Input,cursor-2)==true)
                {
                    Input = Input.Substring(0, Input.Length - 2);
                }
                else if (IsDot(Input,cursor-1)==true && IsNumber(Input,cursor-2)==true && IsNumber(Input,cursor-3)==true)
                {
                    Input = Input.Substring(0, Input.Length - 3);
                }
                else
                {
                    break;
                }
            }
            return Input;
        }

        private bool IsDot(string Input, int Index)
        {
            if (Index>Input.Length-1)
            {
                return false;
            }
            
            if (Input.Substring(Index, 1) == ".")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNumber(string Input, int Index)
        {
            if (Index > Input.Length - 1)
            {
                return false;
            }
        
            string Word = Input.Substring(Index, 1);
            switch(Word)
            {
                case "1": case "2": case "3": case "4": case "5": case "6": case "7": case "8": case "9": case "0":
                    return true;
                default:
                    return false;
            }
        }
    }
}