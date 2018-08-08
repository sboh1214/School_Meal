using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Windows.Data.Json;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Xamarin.Essentials;
using System.Linq;

namespace School_Meal
{
    public enum NetworkType {None, NotCellular, Wifi, Cellular, Ethernet, NotSupported, Error, Invaild};
    public enum LoadType {None, Day, Month, Error, Invaild};
    public enum DeviceType {Win10, Win7, XF, XA, XI};

    public class SchoolMealClass
    {
        private string SchoolCode = null;
        private DateTime DateCursor = DateTime.Now;
        private DateTime WeekCursor = DateTime.Now;
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
            try
            {
                var Connection = CheckNetwork(deviceType);
                if (Connection == NetworkType.None)
                {

                }

                var JObject = HttpRequest(Year,Month,0,LoadType.Month);

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

                    switch(deviceType)
                    {
                        case DeviceType.Win10:

                            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                            localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "B"] = Breakfast_String;
                            localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "L"] = Lunch_String;
                            localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "D"] = Dinner_String;
                            break;

                        case DeviceType.Win7:

                            break;

                        case DeviceType.XF:
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

        private JsonObject HttpRequest (int Year,int Month,int Day,LoadType Type)
        {
            try
            {
                //http://schoolmenukr.ml/api/ice/E100002238?year=2018&month=7
                Uri Url = null;
                if (Type == LoadType.Day)
                {
                    Url = new Uri("http://schoolmenukr.ml/api/ice/"+SchoolCode+"?year=" + Year.ToString() + "&month=" + Month.ToString() + Day.ToString());
                }
                else if (Type == LoadType.Month)
                {
                    Url = new Uri("http://schoolmenukr.ml/api/ice/"+SchoolCode+"?year=" + Year.ToString() + "&month=" + Month.ToString());
                }
                else
                {
                    throw new Exception();
                }
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "GET";
                WebResponse myresponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myresponse.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myresponse.Close();
                var JObject = JsonObject.Parse(result).GetObject();
                return JObject;
            }
            catch
            {
                return null;
            }

        }

        public Dictionary<string,string> GetDayMenu(DeviceType deviceType)
        {
            return GetDayMenu(DateCursor.Year, DateCursor.Month, DateCursor.Day, deviceType);
        }

        public Dictionary<string, string> GetDayMenu(DateTime Date,DeviceType deviceType)
        {
            return GetDayMenu(Date.Year, Date.Month, Date.Day, deviceType);
        }

        public Dictionary<string,string> GetDayMenu(int Year, int Month, int Day, DeviceType deviceType)
        {
            var DayMealDictionary = new Dictionary<string, string>();
            DayMealDictionary.Add("Connection",CheckNetwork(deviceType).ToString());

            switch (deviceType)
            {
                case DeviceType.Win10:

                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    string Win10_Breakfast = localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "B"].ToString();
                    string Win10_Lunch =localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "L"].ToString();
                    string Win10_Dinner =localSettings.Values[Year.ToString() + Month.ToString() + Day.ToString() + "D"].ToString();

                    DayMealDictionary.Add("Breakfast", Win10_Breakfast);
                    DayMealDictionary.Add("Lunch", Win10_Lunch);
                    DayMealDictionary.Add("Dinner", Win10_Dinner);

                    break;

                case DeviceType.Win7:


                    break;

                case DeviceType.XF:

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

        public Dictionary<string,string> GetWeekMenu(DeviceType deviceType)
        {
            return GetWeekMenu(WeekCursor, deviceType);
        }

        public Dictionary<string,string> GetWeekMenu(DateTime dtToday, DeviceType deviceType)
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

        private Dictionary<string,string> GetWeekMenu_Win10(DateTime TempWeekCursor)
        {
            var WeekMealDictionary = new Dictionary<string,string>();
            var Settings = ApplicationData.Current.LocalSettings;
            string YearMonth = TempWeekCursor.Year.ToString() + TempWeekCursor.Month.ToString();

            var SunB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var SunL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var SunD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(1);
            var MonB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var MonL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var MonD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(1);
            var TueB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var TueL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var TueD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(1);
            var WedB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var WedL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var WedD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(1);
            var ThuB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var ThuL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var ThuD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(1);
            var FriB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var FriL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var FriD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(1);
            var SatB = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"B"];
            var SatL = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"L"];
            var SatD = Settings.Values[YearMonth+TempWeekCursor.Day.ToString()+"D"];
            TempWeekCursor.AddDays(-7);

            if (SunB == null)
            {
                WeekMealDictionary.Add("MonB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("MonB", MonB.ToString());
            }
            if (SunB == null)
            {
                WeekMealDictionary.Add("MonL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("MonL", MonL.ToString());
            }
            if (SunB == null)
            {
                WeekMealDictionary.Add("MonD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("MonD", MonD.ToString());
            }


            if (TueB == null)
            {
                WeekMealDictionary.Add("TueB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("TueB", TueB.ToString());
            }
            if (TueL == null)
            {
                WeekMealDictionary.Add("TueL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("TueL", TueL.ToString());
            }
            if (TueD == null)
            {
                WeekMealDictionary.Add("TueD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("TueD", TueD.ToString());
            }

            if (WedB == null)
            {
                WeekMealDictionary.Add("WedB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("WedB", WedB.ToString());
            }
            if (WedL == null)
            {
                WeekMealDictionary.Add("WedL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("WedL", WedL.ToString());
            }
            if (WedD == null)
            {
                WeekMealDictionary.Add("WedD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("WedD", WedD.ToString());
            }

            if (ThuB == null)
            {
                WeekMealDictionary.Add("ThuB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("ThuB", ThuB.ToString());
            }
            if (ThuL == null)
            {
                WeekMealDictionary.Add("ThuL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("ThuL", ThuL.ToString());
            }
            if (ThuD == null)
            {
                WeekMealDictionary.Add("ThuD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("ThuD", ThuD.ToString());
            }

            if (FriB == null)
            {
                WeekMealDictionary.Add("FriB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("FriB", FriB.ToString());
            }
            if (FriL == null)
            {
                WeekMealDictionary.Add("FriL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("FriL", FriL.ToString());
            }
            if (FriD == null)
            {
                WeekMealDictionary.Add("FriD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("FriD", FriD.ToString());
            }

            if (SatB == null)
            {
                WeekMealDictionary.Add("SatB", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SatB", SatL.ToString());
            }
            if (SatL == null)
            {
                WeekMealDictionary.Add("SatL", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SatL", SatL.ToString());
            }
            if (SatD == null)
            {
                WeekMealDictionary.Add("SatD", "급식정보없음");
            }
            else
            {
                WeekMealDictionary.Add("SatD", SatD.ToString());
            }

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

            WeekMealDictionary.Add("MonB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("MonL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("MonD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("TueB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("TueL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("TueD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("WedB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("WedL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("WedD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("ThuB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("ThuL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("ThuD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("FriB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("FriL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("FriD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SatB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("SatL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("SatD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
            TempWeekCursor.AddDays(1);

            WeekMealDictionary.Add("SunB",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"B","급식정보없음"));
            WeekMealDictionary.Add("SunL",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"L","급식정보없음"));
            WeekMealDictionary.Add("SunD",Preferences.Get(YearMonth+TempWeekCursor.Day.ToString()+"D","급식정보없음"));
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

        public NetworkType CheckNetwork(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Win10:
                    return CheckNetwork_Win10();
                case DeviceType.Win7:
                    return NetworkType.NotSupported;
                case DeviceType.XF:
                    return CheckNetwork_XF();
                default:
                    return NetworkType.Invaild;
            }
        }

        private NetworkType CheckNetwork_Win10()
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();

            var InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool isWLANConnection = (InternetConnectionProfile == null) ? false : InternetConnectionProfile.IsWlanConnectionProfile;  //Wifi
            bool isWWANConnection = (InternetConnectionProfile == null) ? false : InternetConnectionProfile.IsWwanConnectionProfile;  //Mobile

            if (isInternetConnected == false)
            {
                return NetworkType.None;
            }
            else if (isWLANConnection == true)
            {
                return NetworkType.NotCellular;
            }
            else if (isWWANConnection == true)
            {
                return NetworkType.Cellular;
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
            else if (profiles.Contains(Xamarin.Essentials.ConnectionProfile.WiFi))
            {
                return NetworkType.Wifi;
            }
            else if (profiles.Contains(Xamarin.Essentials.ConnectionProfile.Cellular))
            {
                return NetworkType.Cellular;
            }
            else if (profiles.Contains(Xamarin.Essentials.ConnectionProfile.Ethernet))
            {
                return NetworkType.Ethernet;
            }
            else
            {
                return NetworkType.Error;
            }
        }
    }
}