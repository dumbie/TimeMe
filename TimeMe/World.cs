using ArnoldVinkCode;
using System;

namespace TimeMe
{
    partial class MainPage
    {
        void WorldLoad()
        {
            lb_WorldListBox.Items.Clear();
            foreach (string[] City in WorldCities)
            {
                try
                {
                    DateTime CityTimeNow = AVTimeZones.GetTimeZoneTime(City[0]);
                    TimeSpan CityTimeDiff = CityTimeNow.Subtract(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0, 0, DateTimeKind.Utc));

                    string CityOffset = "N/A";
                    if (CityTimeDiff.Hours.ToString().StartsWith("-")) { CityOffset = CityTimeDiff.Hours + ":" + CityTimeDiff.ToString("mm"); }
                    else { CityOffset = "+" + CityTimeDiff.Hours + ":" + CityTimeDiff.ToString("mm"); }

                    string AmPm = "Morning";
                    if (CityTimeNow.Hour >= 6 && CityTimeNow.Hour < 10) { AmPm = "Dawn"; }
                    else if (CityTimeNow.Hour >= 10 && CityTimeNow.Hour < 12) { AmPm = "Morning"; }
                    else if (CityTimeNow.Hour >= 12 && CityTimeNow.Hour < 17) { AmPm = "Afternoon"; }
                    else if (CityTimeNow.Hour >= 17 && CityTimeNow.Hour < 21) { AmPm = "Morning"; }
                    else { AmPm = "Evening"; }

                    string Day = "Today";
                    if (CityTimeNow.Day == DateTime.Now.AddDays(1).Day) { Day = "Tomor"; }
                    else if (CityTimeNow.Day == DateTime.Now.AddDays(-1).Day) { Day = "Yester"; }

                    string ConvertedTime = "N/A";
                    if ((bool)vApplicationSettings["Display24hClock"]) { ConvertedTime = CityTimeNow.ToString("HH:mm"); }
                    else { ConvertedTime = CityTimeNow.ToString("h:mm tt", vCultureInfoEng); }

                    lb_WorldListBox.Items.Add(new WorldList { AmPm = "/Assets/WeatherOther/" + AmPm + ".png", City = "(" + ConvertedTime + ") " + City[1], Day = Day, Zone = ", " + City[0].Replace("Standard Time", "Timezone") + " " + CityOffset });
                }
                catch { }
            }
        }

        string[][] WorldCities = new string[][]
        {
            new string[] {"Arabian Standard Time", "Abu Dhabi, United Arab Emirates"},
            new string[] {"W. Europe Standard Time", "Amsterdam, Netherlands"},
            new string[] {"SE Asia Standard Time", "Bangkok, Thailand"},
            new string[] {"China Standard Time", "Beijing (Peking), China"},
            new string[] {"W. Europe Standard Time", "Berlin, Germany"},
            new string[] {"Central Europe Standard Time", "Budapest, Hungary"},
            new string[] {"Argentina Standard Time", "Buenos Aires, Argentina"},
            new string[] {"Egypt Standard Time", "Cairo, Egypt"},
            new string[] {"Central Standard Time", "Chicago, United States"},
            new string[] {"Bangladesh Standard Time", "Dhaka, Bangladesh"},
            new string[] {"SE Asia Standard Time", "Hanoi, Vietnam"},
            new string[] {"Hawaiian Standard Time", "Honolulu, Hawaii"},
            new string[] {"Turkey Standard Time", "Istanbul, Turkey"},
            new string[] {"SE Asia Standard Time", "Jakarta, Indonesia"},
            new string[] {"Sri Lanka Standard Time", "Jayawardenepura, Sri Lanka"},
            new string[] {"Alaskan Standard Time", "Juneau, United States"},
            new string[] {"Afghanistan Standard Time", "Kabul, Afghanistan"},
            new string[] {"Singapore Standard Time", "Kuala Lumpur, Malaysia"},
            new string[] {"E. Europe Standard Time", "Kyiv, Ukraine"},
            new string[] {"GMT Standard Time", "London, England"},
            new string[] {"Pacific Standard Time", "Los Angeles, United States"},
            new string[] {"Romance Standard Time", "Madrid, Spain"},
            new string[] {"Arabic Standard Time", "Manama, Bahrain"},
            new string[] {"Singapore Standard Time", "Manila, Philippines"},
            new string[] {"AUS Eastern Standard Time", "Melbourne, Australia"},
            new string[] {"Central Standard Time (Mexico)", "Mexico City, Mexico"},
            new string[] {"Russian Standard Time", "Moscow, Russia"},
            new string[] {"India Standard Time", "New Delhi, India"},
            new string[] {"Eastern Standard Time", "New York, United States"},
            new string[] {"Tonga Standard Time", "Nuku'alofa, Tonga"},
            new string[] {"Greenland Standard Time", "Nuuk, Greenland"},
            new string[] {"Eastern Standard Time", "Ottawa, Canada"},
            new string[] {"Romance Standard Time", "Paris, France"},
            new string[] {"US Mountain Standard Time", "Phoenix, United States"},
            new string[] {"South Africa Standard Time", "Pretoria, South Africa"},
            new string[] {"W. Europe Standard Time", "Rome, Italy"},
            new string[] {"E. South America Standard Time", "São Paulo, Brazil"},
            new string[] {"Korea Standard Time", "Seoul, South Korea"},
            new string[] {"Singapore Standard Time", "Singapore, Singapore"},
            new string[] {"Iran Standard Time", "Tehran, Iran"},
            new string[] {"Tokyo Standard Time", "Tokyo, Japan"},
            new string[] {"Pacific Standard Time", "Vancouver, Canada"},
            new string[] {"New Zealand Standard Time", "Wellington, New Zealand"},
        };
    }
}