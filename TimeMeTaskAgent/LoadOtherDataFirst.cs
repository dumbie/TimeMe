using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Load first other used data
        async Task<bool> LoadOtherDataFirst()
        {
            try
            {
                Debug.WriteLine("Loading first other miscellaneous data.");

                //Load and set time splitter character
                if (setDisplayCurrentTime)
                {
                    switch (setDisplayTimeSplitter)
                    {
                        case 0: { TextTimeSplit = ":"; break; }
                        case 1: { TextTimeSplit = "."; break; }
                        case 2: { TextTimeSplit = ","; break; }
                        case 3: { TextTimeSplit = "_"; break; }
                        case 4: { TextTimeSplit = "-"; break; }
                        case 5: { TextTimeSplit = "+"; break; }
                        case 6: { TextTimeSplit = "x"; break; }
                        case 7: { TextTimeSplit = "|"; break; }
                        case 8: { TextTimeSplit = "!"; break; }
                        case 9: { TextTimeSplit = "*"; break; }
                        case 10: { TextTimeSplit = "@"; break; }
                        case 11: { TextTimeSplit = "#"; break; }
                        case 12: { TextTimeSplit = "&"; break; }
                        case 13: { TextTimeSplit = "̥"; break; }
                        case 14: { TextTimeSplit = "͓"; break; }
                        case 15: { TextTimeSplit = " "; break; }
                    }
                }

                //Check and set the weather icon style
                if (setDisplayWeatherWhiteIcons) { WeatherIconStyle = "White"; }

                //Check if current weather icon is available
                string WeatherIconFormat = "WeatherSquare" + WeatherIconStyle;
                if (await AVFunctions.AppFileExists("Assets/" + WeatherIconFormat + "/" + BgStatusWeatherCurrentIcon + ".png")) { WeatherIconCurrent = BgStatusWeatherCurrentIcon; }
                else { WeatherIconCurrent = "0"; }

                //Check for active alarms and timers
                if ((setDisplayAlarm || setLockAlarm) && Toast_UpdateManager.GetScheduledToastNotifications().Any(x => x.DeliveryTime > DateTimeNow)) { TimerAlarmActive = true; }

                //Load and set current battery text
                if (TextPositionUsed(Setting_TextPositions.Battery) || setLockBattery || setLockBatteryDetailed || setNotiBattery || setNotiLowBattery || TileBattery_Pinned || setDisplayTimeCustomText) { await LoadBatteryLevel(); }

                //Load and set current network name
                if (TextPositionUsed(Setting_TextPositions.Network) || setLockNetwork || setNotiNetworkChange || setDisplayTimeCustomText)
                {
                    //Get and set the current network name
                    TextNetworkName = await AVFunctions.GetNetworkName();

                    //Set the network name to tile text
                    TextPositionSet(Setting_TextPositions.Network, TextNetworkName);

                    //Check if the network has changed
                    if (TextNetworkName != BgStatusNetworkName)
                    {
                        vApplicationSettings["BgStatusNetworkName"] = TextNetworkName;
                        //Notification - Network change
                        ShowNotiNetworkChange();
                    }
                }

                //Load current week number
                if (TextPositionUsed(Setting_TextPositions.WeekNumber) || setLockWeekNumber || setDisplayDateWeekNumber || setNotiWeekNumber || setLiveTileSizeName == "WideWords")
                {
                    DateTime DateTimeWeekNow = DateTimeNow;
                    DayOfWeek DateTimeDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(DateTimeWeekNow);
                    if (DateTimeDay >= DayOfWeek.Monday && DateTimeDay <= DayOfWeek.Wednesday) { DateTimeWeekNow = DateTimeWeekNow.AddDays(3); }
                    WeekNumberCurrent = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTimeWeekNow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                    //Notification - Current week number
                    ShowNotiWeekNumber();
                }

                //Load and set day time progression
                if (TextPositionUsed(Setting_TextPositions.DayProgress) || setNotiDayTime)
                {
                    DayTimeProgress = Convert.ToInt32(100 - (((1440 - DateTimeNow.Subtract(DateTime.Today).TotalMinutes) / 1440) * 100));

                    //Set day time progression to tile text
                    if (DayTimeProgress < 25) { TextPositionSet(Setting_TextPositions.DayProgress, "🕛 " + DayTimeProgress + " %"); }
                    else if (DayTimeProgress < 50) { TextPositionSet(Setting_TextPositions.DayProgress, "🕒 " + DayTimeProgress + " %"); }
                    else if (DayTimeProgress < 75) { TextPositionSet(Setting_TextPositions.DayProgress, "🕕 " + DayTimeProgress + " %"); }
                    else { TextPositionSet(Setting_TextPositions.DayProgress, "🕘 " + DayTimeProgress + " %"); }

                    //Notification - Day time progression
                    ShowNotiDayTime();
                }

                //Load Next calendar time
                if (TextPositionUsed(Setting_TextPositions.CalendarName) || TextPositionUsed(Setting_TextPositions.CalendarDateTime) || setLockCalendar || setNotiCalendarTime)
                {
                    await LoadCalendarEvent();

                    //Notification - Next calendar time
                    ShowNotiCalendarTime();
                }

                //Load countdown event information
                if (TextPositionUsed(Setting_TextPositions.Countdown) || setLockCountdown || setNotiCountdownTime)
                {
                    //Load first countdown event from XML
                    using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeCountdown.xml"))
                    {
                        XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                        OpenStreamForReadAsync.Dispose();

                        IEnumerable<XElement> XmlCountdownEvents = XDocument.Descendants("TimeMeCountdown").Elements("Count");
                        if (XmlCountdownEvents.Any())
                        {
                            XElement FirstEvent = XmlCountdownEvents.Last();
                            DateTime LoadedDate = DateTime.Parse(FirstEvent.Attribute("CountDate").Value);

                            //Datetime to string
                            string ConvertedDate = "";
                            if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedDate = AVFunctions.ToTitleCase(LoadedDate.Date.ToString("d MMMM yyyy", vCultureInfoReg)); }
                            else { ConvertedDate = LoadedDate.Date.ToString("d MMMM yyyy", vCultureInfoEng); }

                            //Calculate the days left
                            CountdownEventDate = (LoadedDate.Date.Subtract(DateTimeNow.Date).Days).ToString();
                            if (CountdownEventDate == "0") { CountdownEventDate = "today"; }

                            //Set the countdown name
                            CountdownEventName = FirstEvent.Attribute("CountName").Value;
                        }
                    }

                    //Notification - Countdown Time
                    ShowNotiCountdownTime();
                }
                return true;
            }
            catch { return false; }
        }
    }
}