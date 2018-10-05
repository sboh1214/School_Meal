﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Json;
using School_Meal__Win_7_.Properties;

namespace School_Meal__Win_7_
{
    public enum NetworkType { None, Connected, Wifi, Cellular, Ethernet, Error };
    public enum DeviceType { Win10, Win7, XF, XA, XI };

    public partial class SchoolMealClass_WPF
    {
        private readonly string SchoolCode = null;
        private DateTime DateCursor { get; set; }
        private DateTime WeekCursor { get; set; }
        private JsonValue JItem_Day;
        private JsonValue JItem_Breakfast;
        private JsonValue JItem_Lunch;
        private JsonValue JItem_Dinner;

        public SchoolMealClass_WPF(string Code)
        {
            SchoolCode = Code;
        }

        public bool LoadMonthMenu(DeviceType deviceType)
        {
            return LoadMonthMenu(DateCursor.Year, DateCursor.Month, deviceType);
        }

        public bool LoadMonthMenu(int Year, int Month, DeviceType deviceType)
        {
            var JObject = RequestMonthMenu(Year, Month);
            if (JObject == null)
            {
                return false;
            }

            foreach (var JItem_Temp1 in JObject)
            {
                var JItem_Date = (JsonObject)JItem_Temp1;
                if (JItem_Date.Count != 4)
                {
                    continue;
                }

                JItem_Day = JItem_Date["date"];
                JItem_Breakfast = JItem_Date["breakfast"];
                JItem_Lunch = JItem_Date["lunch"];
                JItem_Dinner = JItem_Date["dinner"];

                if (JItem_Day.JsonType != JsonType.String)
                {
                    continue;
                }
                bool IsDay = int.TryParse(JItem_Day, out var Day);
                if (IsDay == false)
                {
                    continue;
                }

                string Breakfast_String = "";
                string Lunch_String = "";
                string Dinner_String = "";

                if (JItem_Breakfast.JsonType != JsonType.Array)
                {
                    Breakfast_String = "급식 정보 없음";
                }
                else
                {
                    var Breakfast_Array = JItem_Breakfast;
                    for (int i = 0; i < Breakfast_Array.Count; i++)
                    {
                        string Breakfast_TempString = Breakfast_Array[i].ToString();
                        Breakfast_String += RemoveAllergyInfo(Breakfast_TempString);
                        Breakfast_String += "\n";
                    }
                }

                if (JItem_Lunch.JsonType != JsonType.Array)
                {
                    Lunch_String = "급식 정보 없음";
                }
                else
                {
                    var Lunch_Array = JItem_Lunch;
                    for (int j = 0; j < Lunch_Array.Count; j++)
                    {
                        string Lunch_TempString = Lunch_Array[j].ToString();
                        Lunch_String += RemoveAllergyInfo(Lunch_TempString);
                        Lunch_String += "\n";
                    }
                }

                if (JItem_Dinner.JsonType != JsonType.Array)
                {
                    Dinner_String = "급식 정보 없음";
                }
                else
                {
                    var Dinner_Array = JItem_Dinner;
                    for (int k = 0; k < Dinner_Array.Count; k++)
                    {
                        string Dinner_TempString = Dinner_Array[k].ToString();
                        Dinner_String += RemoveAllergyInfo(Dinner_TempString);
                        Dinner_String += "\n";
                    }
                }

                Settings.Default[MakeDateString(Year, Month, Day) + "B"] = Breakfast_String;
                Settings.Default[MakeDateString(Year, Month, Day) + "L"] = Lunch_String;
                Settings.Default[MakeDateString(Year, Month, Day) + "D"] = Dinner_String;
            }
            return true;
        }

        //public bool LoadDayMenu(DeviceType deviceType)
        //{
        //    return LoadDayMenu(DateCursor.Year, DateCursor.Month, DateCursor.Day, deviceType);
        //}

        //public bool LoadDayMenu(int Year, int Month, int Day, DeviceType deviceType)
        //{
        //    var JObject = RequestMonthMenu(Year, Month);
        //    if (JObject == null)
        //    {
        //        return false;
        //    }

        //    string Breakfast_String = "";
        //    string Lunch_String = "";
        //    string Dinner_String = "";

        //    if (JItem_Breakfast.ValueType != JsonValueType.Array)
        //    {
        //        Breakfast_String = "급식 정보 없음";
        //    }
        //    else
        //    {
        //        var Breakfast_Array = JItem_Breakfast.GetArray();
        //        for (int i = 0; i < Breakfast_Array.Count; i++)
        //        {
        //            string Breakfast_TempString = Breakfast_Array[i].GetString();
        //            Breakfast_String += RemoveAllergyInfo(Breakfast_TempString);
        //            Breakfast_String += "\n";
        //        }
        //    }

        //    if (JItem_Lunch.ValueType != JsonValueType.Array)
        //    {
        //        Lunch_String = "급식 정보 없음";
        //    }
        //    else
        //    {
        //        var Lunch_Array = JItem_Lunch.GetArray();
        //        for (int j = 0; j < Lunch_Array.Count; j++)
        //        {
        //            string Lunch_TempString = Lunch_Array[j].GetString();
        //            Lunch_String += RemoveAllergyInfo(Lunch_TempString);
        //            Lunch_String += "\n";
        //        }
        //    }

        //    if (JItem_Dinner.ValueType != JsonValueType.Array)
        //    {
        //        Dinner_String = "급식 정보 없음";
        //    }
        //    else
        //    {
        //        var Dinner_Array = JItem_Dinner.GetArray();
        //        for (int k = 0; k < Dinner_Array.Count; k++)
        //        {
        //            string Dinner_TempString = Dinner_Array[k].GetString();
        //            Dinner_String += RemoveAllergyInfo(Dinner_TempString);
        //            Dinner_String += "\n";
        //        }
        //    }

        //    switch (deviceType)
        //    {
        //        case DeviceType.Win7:
        //            break;


        //        default:
        //            break;
        //    }
        //    return true;
        //}

        private JsonArray RequestMonthMenu(int Year, int Month)
        {
            try
            {
                //http://schoolmenukr.ml/api/high/E100002238?year=2018&month=7

                var Url = new Uri("http://schoolmenukr.ml/api/high/" + SchoolCode + "?year=" + Year.ToString() + "&month=" + Month.ToString());
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "GET";
                WebResponse myresponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myresponse.Close();
                return (JsonArray)JsonValue.Parse(result)["menu"];
            }
            catch
            {
                return null;
            }

        }

        //private JsonObject RequestDayMenu(int Year, int Month, int Day)
        //{
        //    try
        //    {
        //        //http://schoolmenukr.ml/api/ice/E100002238?year=2018&month=8&date=17

        //        var Url = new Uri("http://schoolmenukr.ml/api/ice/" + SchoolCode +
        //            "?year=" + Year.ToString() + "&month=" + Month.ToString() + "&date=" + Day.ToString());
        //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
        //        myRequest.Method = "GET";
        //        WebResponse myresponse = myRequest.GetResponse();
        //        StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
        //        string result = sr.ReadToEnd();
        //        sr.Close();
        //        myresponse.Close();
        //        result = result.Substring(1, result.Length - 2);
        //        return JsonObject.Parse(result);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

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
            string DateString = MakeDateString(Year, Month, Day);

            switch (deviceType)
            {
                case DeviceType.Win7:
                    var Breakfast = Settings.Default[DateString + "B"];
                    var Lunch = Settings.Default[DateString + "L"];
                    var Dinner = Settings.Default[DateString + "D"];

                    if (Breakfast == null)
                    {
                        DayMealDictionary.Add("Breakfast", "급식정보없음");
                    }
                    else
                    {
                        DayMealDictionary.Add("Breakfast", Breakfast.ToString());
                    }
                    if (Lunch == null)
                    {
                        DayMealDictionary.Add("Lunch", "급식정보없음");
                    }
                    else
                    {
                        DayMealDictionary.Add("Lunch", Lunch.ToString());
                    }
                    if (Dinner == null)
                    {
                        DayMealDictionary.Add("Dinner", "급식정보없음");
                    }
                    else
                    {
                        DayMealDictionary.Add("Dinner", Dinner.ToString());
                    }
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
                case DeviceType.Win7:
                    WeekMealDictionary = GetWeekMenu_Win7(TempWeekCursor);
                    break;
            }
            return WeekMealDictionary;
        }

        

        private Dictionary<string, string> GetWeekMenu_Win7(DateTime WeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string, string>();



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

        private string MakeDateString(DateTime dateTime)
        {
            return MakeDateString(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        private string MakeDateString(int Year, int Month, int Day)
        {
            string DateString = Year.ToString();
            if (Month < 10)
            {
                DateString = DateString + "0" + Month.ToString();
            }
            else
            {
                DateString = DateString + Month.ToString();
            }
            if (Day < 10)
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