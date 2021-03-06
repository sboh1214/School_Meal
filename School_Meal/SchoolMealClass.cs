﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Windows.Data.Json;
using Windows.Storage;
using Xamarin.Essentials;
using System.Linq;

namespace School_Meal
{
    public enum NetworkType { None,Connected, Wifi, Cellular, Ethernet, Error };
    public enum DeviceType { Win10, Win7, XF, XA, XI };

    public class SchoolMealClass
    {
        private readonly string SchoolCode = null;
        private DateTime DateCursor { get; set; }
        private DateTime WeekCursor { get; set; }
        private IJsonValue JItem_Day;
        private IJsonValue JItem_Breakfast;
        private IJsonValue JItem_Lunch;
        private IJsonValue JItem_Dinner;

        public SchoolMealClass(string Code)
        {
            SchoolCode = Code;
        }

        public bool LoadMonthMenu(DeviceType deviceType)
        {
            return LoadMonthMenu(DateCursor.Year, DateCursor.Month, deviceType);
        }

        public bool LoadMonthMenu(int Year, int Month, DeviceType deviceType)
        {
            var Connection = CheckNetwork(deviceType);
            if (Connection == NetworkType.None || Connection == NetworkType.Error)
            {
                return false;
            }
            var JObject = RequestMonthMenu(Year, Month);
            if (JObject == null)
            {
                return false;
            }

            foreach (var JItem_Temp1 in JObject)
            {
                var JItem_Date = JItem_Temp1.GetObject();
                if (JItem_Date.Keys.Count != 4)
                {
                    continue;
                }
                foreach (var JItem_Temp2 in JItem_Date)
                {
                    switch (JItem_Temp2.Key)
                    {
                        case "date":
                            JItem_Day = JItem_Temp2.Value;
                            break;
                        case "breakfast":
                            JItem_Breakfast = JItem_Temp2.Value;
                            break;
                        case "lunch":
                            JItem_Lunch = JItem_Temp2.Value;
                            break;
                        case "dinner":
                            JItem_Dinner = JItem_Temp2.Value;
                            break;
                    }
                }
                
                if (JItem_Day.ValueType != JsonValueType.String)
                {
                    continue;
                }
                bool IsDay = int.TryParse(JItem_Day.GetString(),out var Day);
                if (IsDay == false)
                {
                    continue;
                }

                string Breakfast_String = "";
                string Lunch_String = "";
                string Dinner_String = "";

                if (JItem_Breakfast.ValueType != JsonValueType.Array)
                {
                    Breakfast_String = "급식 정보 없음";
                }
                else
                {
                    var Breakfast_Array = JItem_Breakfast.GetArray();
                    for (int i = 0; i < Breakfast_Array.Count; i++)
                    {
                        string Breakfast_TempString = Breakfast_Array[i].GetString();
                        Breakfast_String += RemoveAllergyInfo( Breakfast_TempString );
                        Breakfast_String += "\n";
                    }
                }

                if (JItem_Lunch.ValueType != JsonValueType.Array)
                {
                    Lunch_String = "급식 정보 없음";
                }
                else
                {
                    var Lunch_Array = JItem_Lunch.GetArray();
                    for (int j = 0; j < Lunch_Array.Count; j++)
                    {
                        string Lunch_TempString = Lunch_Array[j].GetString();
                        Lunch_String += RemoveAllergyInfo( Lunch_TempString );
                        Lunch_String += "\n";
                    }
                }

                if (JItem_Dinner.ValueType != JsonValueType.Array)
                {
                    Dinner_String = "급식 정보 없음";
                }
                else
                {
                    var Dinner_Array = JItem_Dinner.GetArray();
                    for (int k = 0; k < Dinner_Array.Count; k++)
                    {
                        string Dinner_TempString = Dinner_Array[k].GetString();
                        Dinner_String += RemoveAllergyInfo( Dinner_TempString );
                        Dinner_String += "\n";
                    }
                }

                switch (deviceType)
                {
                    case DeviceType.Win10:
                        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values[MakeDateString(Year, Month, Day) + "B"] = Breakfast_String;
                        localSettings.Values[MakeDateString(Year, Month, Day) + "L"] = Lunch_String;
                        localSettings.Values[MakeDateString(Year, Month, Day) + "D"] = Dinner_String;
                        break;

                    case DeviceType.Win7:

                        break;

                    case DeviceType.XF:
                        Preferences.Set(MakeDateString(Year, Month, Day) + "B", Breakfast_String);
                        Preferences.Set(MakeDateString(Year, Month, Day) + "L", Lunch_String);
                        Preferences.Set(MakeDateString(Year, Month, Day) + "D", Dinner_String);
                        break;

                    default:
                        break;
                }
            }
            return true;
        }

        public bool LoadDayMenu(DeviceType deviceType)
        {
            return LoadDayMenu(DateCursor.Year, DateCursor.Month, DateCursor.Day, deviceType);
        }

        public bool LoadDayMenu(int Year, int Month, int Day, DeviceType deviceType)
        {
            var Connection = CheckNetwork(deviceType);
            if (Connection == NetworkType.None || Connection == NetworkType.Error)
            {
                return false;
            }
            var JObject = RequestMonthMenu(Year, Month);
            if (JObject == null)
            {
                return false;
            }
            
            string Breakfast_String = "";
            string Lunch_String = "";
            string Dinner_String = "";

            if (JItem_Breakfast.ValueType != JsonValueType.Array)
            {
                Breakfast_String = "급식 정보 없음";
            }
            else
            {
                var Breakfast_Array = JItem_Breakfast.GetArray();
                for (int i = 0; i < Breakfast_Array.Count; i++)
                {
                    string Breakfast_TempString = Breakfast_Array[i].GetString();
                    Breakfast_String += RemoveAllergyInfo(Breakfast_TempString);
                    Breakfast_String += "\n";
                }
            }

            if (JItem_Lunch.ValueType != JsonValueType.Array)
            {
                Lunch_String = "급식 정보 없음";
            }
            else
            {
                var Lunch_Array = JItem_Lunch.GetArray();
                for (int j = 0; j < Lunch_Array.Count; j++)
                {
                    string Lunch_TempString = Lunch_Array[j].GetString();
                    Lunch_String += RemoveAllergyInfo(Lunch_TempString);
                    Lunch_String += "\n";
                }
            }

            if (JItem_Dinner.ValueType != JsonValueType.Array)
            {
                Dinner_String = "급식 정보 없음";
            }
            else
            {
                var Dinner_Array = JItem_Dinner.GetArray();
                for (int k = 0; k < Dinner_Array.Count; k++)
                {
                    string Dinner_TempString = Dinner_Array[k].GetString();
                    Dinner_String += RemoveAllergyInfo(Dinner_TempString);
                    Dinner_String += "\n";
                }
            }

            switch (deviceType)
            {
                case DeviceType.Win10:
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values[MakeDateString(Year, Month, Day) + "B"] = Breakfast_String;
                    localSettings.Values[MakeDateString(Year, Month, Day) + "L"] = Lunch_String;
                    localSettings.Values[MakeDateString(Year, Month, Day) + "D"] = Dinner_String;
                    break;

                case DeviceType.Win7:

                    break;

                case DeviceType.XF:
                    Preferences.Set(MakeDateString(Year, Month, Day) + "B", Breakfast_String);
                    Preferences.Set(MakeDateString(Year, Month, Day) + "L", Lunch_String);
                    Preferences.Set(MakeDateString(Year, Month, Day) + "D", Dinner_String);
                    break;

                default:
                    break;
            }
            return true;
        }

        private JsonArray RequestMonthMenu(int Year, int Month)
        {
            try
            {
                //https://schoolmenukr.ml/api/HIGH/E100002238?year=2018&month=7
                
                var Url = new Uri("https://schoolmenukr.ml/api/HIGH/" + SchoolCode + "?year=" + Year.ToString() + "&month=" + Month.ToString());
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "GET";
                WebResponse myresponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myresponse.Close();
                var JObject = JsonObject.Parse(result);
                return JObject["menu"].GetArray(); 
            }
            catch
            {
                return null;
            }

        }

        private JsonObject RequestDayMenu(int Year, int Month, int Day)
        {
            try
            {
                //http://schoolmenukr.ml/api/ice/E100002238?year=2018&month=8&date=17

                var Url = new Uri("http://schoolmenukr.ml/api/ice/" + SchoolCode + 
                    "?year=" + Year.ToString() + "&month=" + Month.ToString() + "&date=" + Day.ToString());
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "GET";
                WebResponse myresponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myresponse.Close();
                result = result.Substring(1, result.Length - 2);
                return JsonObject.Parse(result);
            }
            catch
            {
                return null;
            }
        }

        public Dictionary<string, string> GetDayMenu(DeviceType deviceType)
        {
            return GetDayMenu(DateCursor.Year, DateCursor.Month, DateCursor.Day, deviceType);
        }

        public Dictionary<string, string> GetDayMenu(DateTime Date, DeviceType deviceType)
        {
            return GetDayMenu(Date.Year, Date.Month, Date.Day, deviceType);
        }

        public Dictionary<string, string> GetDayMenu(int Year, int Month, int Day, DeviceType deviceType)
        {
            var DayMealDictionary = new Dictionary<string, string>();
            DayMealDictionary.Add("Connection", CheckNetwork(deviceType).ToString());

            switch (deviceType)
            {
                case DeviceType.Win10:

                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    string Win10_Breakfast = localSettings.Values[MakeDateString(Year, Month, Day) + "B"].ToString();
                    string Win10_Lunch = localSettings.Values[MakeDateString(Year, Month, Day) + "L"].ToString();
                    string Win10_Dinner = localSettings.Values[MakeDateString(Year, Month, Day) + "D"].ToString();

                    DayMealDictionary.Add("Breakfast", Win10_Breakfast);
                    DayMealDictionary.Add("Lunch", Win10_Lunch);
                    DayMealDictionary.Add("Dinner", Win10_Dinner);

                    break;

                case DeviceType.Win7:


                    break;

                case DeviceType.XF:

                    string XF_Breakfast = Preferences.Get(MakeDateString(Year, Month, Day) + "B", "급식정보없음");
                    string XF_Lunch = Preferences.Get(MakeDateString(Year, Month, Day) + "L", "급식정보없음");
                    string XF_Dinner = Preferences.Get(MakeDateString(Year, Month, Day) + "D", "급식정보없음");

                    DayMealDictionary.Add("Breakfast", XF_Breakfast);
                    DayMealDictionary.Add("Lunch", XF_Lunch);
                    DayMealDictionary.Add("Dinner", XF_Dinner);

                    break;

                default:
                    break;
            }

            return DayMealDictionary;
        }

        public Dictionary<string, string> GetWeekMenu(DeviceType deviceType)
        {
            return GetWeekMenu(WeekCursor, deviceType);
        }

        public Dictionary<string, string> GetWeekMenu(DateTime dtToday, DeviceType deviceType)
        {
            var WeekMealDictionary = new Dictionary<string, string>();

            CultureInfo ciCurrent = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek dwFirst = ciCurrent.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek dwToday = ciCurrent.Calendar.GetDayOfWeek(dtToday);
            int iDiff = dwToday - dwFirst;
            var TempWeekCursor = dtToday.AddDays(-iDiff);

            switch (deviceType)
            {
                case DeviceType.Win10:
                    WeekMealDictionary = GetWeekMenu_Win10(TempWeekCursor);
                    break;

                case DeviceType.Win7:
                    WeekMealDictionary = GetWeekMenu_Win7(TempWeekCursor);
                    break;

                case DeviceType.XF:
                    WeekMealDictionary = GetWeekMenu_XF(TempWeekCursor);
                    break;
            }
            return WeekMealDictionary;
        }

        private Dictionary<string, string> GetWeekMenu_Win10(DateTime TempWeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string, string>();
            var Settings = ApplicationData.Current.LocalSettings;
            
            var SunB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var SunL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var SunD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];
            TempWeekCursor = TempWeekCursor.AddDays(1);
            var MonB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var MonL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var MonD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];
            TempWeekCursor = TempWeekCursor.AddDays(1);
            var TueB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var TueL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var TueD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];
            TempWeekCursor = TempWeekCursor.AddDays(1);
            var WedB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var WedL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var WedD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];
            TempWeekCursor = TempWeekCursor.AddDays(1);
            var ThuB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var ThuL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var ThuD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];
            TempWeekCursor = TempWeekCursor.AddDays(1);
            var FriB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var FriL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var FriD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];
            TempWeekCursor = TempWeekCursor.AddDays(1);
            var SatB = Settings.Values[MakeDateString(TempWeekCursor) + "B"];
            var SatL = Settings.Values[MakeDateString(TempWeekCursor) + "L"];
            var SatD = Settings.Values[MakeDateString(TempWeekCursor) + "D"];

            if (SunB == null || SunB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("SunB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SunB", SunB.ToString());
            }

            if (SunL == null || SunL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("SunL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SunL", SunL.ToString());
            }

            if (SunD == null || SunD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("SunD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SunD", SunD.ToString());
            }

            if (MonB == null || MonB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("MonB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("MonB", MonB.ToString());
            }
            if (MonL == null || MonL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("MonL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("MonL", MonL.ToString());
            }
            if (MonD == null || MonD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("MonD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("MonD", MonD.ToString());
            }


            if (TueB == null || TueB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("TueB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("TueB", TueB.ToString());
            }
            if (TueL == null || TueL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("TueL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("TueL", TueL.ToString());
            }
            if (TueD == null || TueD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("TueD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("TueD", TueD.ToString());
            }

            if (WedB == null || WedB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("WedB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("WedB", WedB.ToString());
            }
            if (WedL == null || WedL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("WedL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("WedL", WedL.ToString());
            }
            if (WedD == null || WedD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("WedD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("WedD", WedD.ToString());
            }

            if (ThuB == null || ThuB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("ThuB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("ThuB", ThuB.ToString());
            }
            if (ThuL == null || TueL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("ThuL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("ThuL", ThuL.ToString());
            }
            if (ThuD == null || ThuD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("ThuD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("ThuD", ThuD.ToString());
            }

            if (FriB == null || FriB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("FriB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("FriB", FriB.ToString());
            }
            if (FriL == null || FriL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("FriL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("FriL", FriL.ToString());
            }
            if (FriD == null || FriD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("FriD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("FriD", FriD.ToString());
            }

            if (SatB == null || SatB.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("SatB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SatB", SatB.ToString());
            }
            if (SatL == null || SatL.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("SatL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SatL", SatL.ToString());
            }
            if (SatD == null || SatD.ToString() == "급식이 없습니다.\n")
            {
                WeekMealDictionary.Add("SatD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SatD", SatD.ToString());
            }

            return WeekMealDictionary;
        }

        private Dictionary<string, string> GetWeekMenu_Win7(DateTime WeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string, string>();



            return WeekMealDictionary;
        }

        private Dictionary<string, string> GetWeekMenu_XF(DateTime TempWeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string, string>();

            WeekMealDictionary.Add("MonB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("MonL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("MonD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("TueB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("TueL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("TueD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("WedB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("WedL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("WedD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("ThuB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("ThuL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("ThuD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("FriB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("FriL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("FriD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SatB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("SatL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("SatD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SunB", Preferences.Get(MakeDateString(TempWeekCursor) + "B", "급식정보없음"));
            WeekMealDictionary.Add("SunL", Preferences.Get(MakeDateString(TempWeekCursor) + "L", "급식정보없음"));
            WeekMealDictionary.Add("SunD", Preferences.Get(MakeDateString(TempWeekCursor) + "D", "급식정보없음"));
            TempWeekCursor = TempWeekCursor.AddDays(-7);

            return WeekMealDictionary;
        }

        public bool MoveDateCursorToToday()
        {
            try
            {
                DateCursor = DateTime.Today;
                return true;
            }
            catch
            {
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
            catch
            {
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
            catch
            {
                return false;
            }
        }

        public bool MoveDateCursor(int Year, int Month, int Day)
        {
            try
            {
                DateCursor = new DateTime(Year, Month, Day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool MoveWeekCursor(double Week)
        {
            try
            {
                WeekCursor = WeekCursor.AddDays(7 * Week);
                return true;
            }
            catch
            { 
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
                case "YYYYMMDD":
                case "yyyymmdd":
                    return Year + Month + Day;
                case "YYMMDD":
                case "yymmdd":
                    return Year.Substring(2, 2) + Month + Day;
                case "YYYYMM":
                case "yyyymm":
                    return Year + Month;
                case "YYMM":
                case "yymm":
                    return Year.Substring(2, 2) + Month;
                case "MMDD":
                case "mmdd":
                    return Month + Day;
                case "YYYY":
                case "yyyy":
                    return Year;
                case "YY":
                case "yy":
                    return Year.Substring(2, 2);
                case "MM":
                case "mm":
                    return Month;
                case "DD":
                case "dd":
                    return Day;
                case "YYYY년 MM월 DD일":
                case "yyyy년 mm월 dd일":
                    return Year + "년 " + Month + "월 " + Day + "일";
                case "MM월 DD일":
                case "mm월 dd일":
                    return Month + "월 " + Day + "일";
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
            while (true)
            {
                if (IsDot(Input, -1) == true && IsNumber(Input, -2) == true && IsNumber(Input, -3) == true)
                {
                    Input = Input.Substring(0, Input.Length - 3);
                }
                else if (IsDot(Input, -1) == true && IsNumber(Input, -2) == true)
                {
                    Input = Input.Substring(0, Input.Length - 2);
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
            if (Input.Substring(Input.Length + Index, 1) == ".")
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
            string Word = Input.Substring(Input.Length + Index, 1);
            switch (Word)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    return true;
                default:
                    return false;
            }
        }

        public NetworkType CheckNetwork(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Win10:
                    return CheckNetwork_Win10();
                case DeviceType.Win7:
                    return NetworkType.Error;
                case DeviceType.XF:
                    return CheckNetwork_XF();
                default:
                    return NetworkType.Error;
            }
        }

        private NetworkType CheckNetwork_Win10()
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();

            if (isInternetConnected==true)
            {
                return NetworkType.Connected;
            }
            else if(isInternetConnected==false)
            {
                return NetworkType.None;
            }
            else
            {
                return NetworkType.Error;
            }
        }

        private NetworkType CheckNetwork_XF()
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.Profiles;

            if (current != NetworkAccess.Internet)
            {
                return NetworkType.None;
            }
            else if (profiles.Contains(ConnectionProfile.WiFi))
            {
                return NetworkType.Wifi;
            }
            else if (profiles.Contains(ConnectionProfile.Cellular))
            {
                return NetworkType.Cellular;
            }
            else if (profiles.Contains(ConnectionProfile.Ethernet))
            {
                return NetworkType.Ethernet;
            }
            else
            {
                return NetworkType.Error;
            }
        }

        private string MakeDateString(DateTime dateTime)
        {
            return MakeDateString(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        private string MakeDateString(int Year, int Month, int Day)
        {
            string DateString = Year.ToString();
            if (Month<10)
            {
                DateString = DateString + "0" + Month.ToString();
            }
            else
            {
                DateString = DateString + Month.ToString();
            }
            if (Day<10)
            {
                DateString = DateString + "0" + Day.ToString();
            }
            else
            {
                DateString = DateString + Day.ToString();
            }
            return DateString;
        }
    }
}