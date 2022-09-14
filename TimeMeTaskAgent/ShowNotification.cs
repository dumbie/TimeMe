using ArnoldVinkCode;
using System;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Notification - Current Weather
        void ShowNotiWeatherCurrent()
        {
            try
            {
                if (setNotiWeatherCurrent)
                {
                    string NotiRainChance = BgStatusWeatherCurrentRainChance + " chance of rain";
                    string NotiWindSpeed = "\n" + BgStatusWeatherCurrentWindSpeed + " windspeed";

                    string NotiLocation = BgStatusWeatherCurrentLocationShort;
                    if (NotiLocation.Length < 7) { NotiLocation = "\nNear town " + NotiLocation; }
                    else if (NotiLocation.Length < 15) { NotiLocation = "\nNear " + NotiLocation; }

                    if (setNotiStyle == 0)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\"/><text id=\"1\">" + WebUtility.HtmlEncode(BgStatusWeatherCurrent) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(NotiRainChance) + WebUtility.HtmlEncode(NotiWindSpeed) + WebUtility.HtmlEncode(NotiLocation) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T1", Group = "G1" });
                    }
                    else if (setNotiStyle == 1)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\"/><text id=\"1\">" + WebUtility.HtmlEncode(BgStatusWeatherCurrent) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(NotiRainChance) + WebUtility.HtmlEncode(NotiWindSpeed) + WebUtility.HtmlEncode(NotiLocation) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T1", Group = "G1" });
                    }
                    else if (setNotiStyle == 2)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\"/><text id=\"1\">" + WebUtility.HtmlEncode(BgStatusWeatherCurrent) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(NotiRainChance) + WebUtility.HtmlEncode(NotiWindSpeed) + WebUtility.HtmlEncode(NotiLocation) + "</text></binding></visual><audio silent=\"false\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T1", Group = "G1" });
                    }
                }
                //else { Toast_History.Remove("T1", "G1"); }
            }
            catch { }
        }

        //Notification - Bing description
        void ShowNotiBingDescription()
        {
            try
            {
                if (setNotiBingDescription)
                {
                    if (setNotiStyle == 0)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Background.png\"/><text id=\"1\">Daily Bing description</text><text id=\"2\">" + WebUtility.HtmlEncode(BgStatusBingDescription) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T2", Group = "G1" });
                    }
                    else if (setNotiStyle == 1)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Background.png\"/><text id=\"1\">Daily Bing description</text><text id=\"2\">" + WebUtility.HtmlEncode(BgStatusBingDescription) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T2", Group = "G1" });
                    }
                    else if (setNotiStyle == 2)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Background.png\"/><text id=\"1\">Daily Bing description</text><text id=\"2\">" + WebUtility.HtmlEncode(BgStatusBingDescription) + "</text></binding></visual><audio silent=\"false\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T2", Group = "G1" });
                    }
                }
                //else { Toast_History.Remove("T2", "G1"); }
            }
            catch { }
        }

        //Notification - Current Battery Level
        void ShowNotiBattery()
        {
            try
            {
                if (setNotiBattery)
                {
                    if (BatteryLevel != "error")
                    {
                        string NotiBattery = "";
                        if (BatteryCharging) { NotiBattery = BatteryLevel + "% left and is now charging"; }
                        else { NotiBattery = BatteryLevel + "% battery life left and " + BatteryTime.ToLower() + "time remaining"; }

                        if (setNotiStyle == 0)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/><text id=\"1\">Your battery level now has</text><text id=\"2\">" + NotiBattery + "</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T3", Group = "G1" });
                        }
                        else if (setNotiStyle == 1)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/><text id=\"1\">Your battery level now has</text><text id=\"2\">" + NotiBattery + "</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T3", Group = "G1" });
                        }
                        else if (setNotiStyle == 2)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/><text id=\"1\">Your battery level now has</text><text id=\"2\">" + NotiBattery + "</text></binding></visual><audio silent=\"false\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T3", Group = "G1" });
                        }
                    }
                    else { Toast_History.Remove("T3", "G1"); }
                }
            }
            catch { }
        }

        //Notification - Low Battery Level
        void ShowNotiLowBattery()
        {
            try
            {
                if (setNotiLowBattery)
                {
                    if (BatteryLevel != "error" && BatteryLevelInt <= 25)
                    {
                        string NotiBattery = "";
                        if (BatteryCharging) { NotiBattery = BatteryLevel + "% left and is now charging"; }
                        else { NotiBattery = BatteryLevel + "% battery life left and " + BatteryTime.ToLower() + "time remaining"; }

                        if (setNotiStyle == 0)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/><text id=\"1\">Your battery level is low</text><text id=\"2\">" + NotiBattery + "</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T3", Group = "G1" });
                        }
                        else if (setNotiStyle == 1)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/><text id=\"1\">Your battery level is low</text><text id=\"2\">" + NotiBattery + "</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T3", Group = "G1" });
                        }
                        else if (setNotiStyle == 2)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/><text id=\"1\">Your battery level is low</text><text id=\"2\">" + NotiBattery + "</text></binding></visual><audio silent=\"false\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T3", Group = "G1" });
                        }
                    }
                    else { Toast_History.Remove("T3", "G1"); }
                }
            }
            catch { }
        }

        //Notification - Battery Saver Warning
        async Task ShowNotiBatterySaver()
        {
            try
            {
                if (setNotiBatterySaver && AVFunctions.DevOsVersion() >= 14393)
                {
                    BackgroundAccessStatus BackgroundManager = await BackgroundExecutionManager.RequestAccessAsync();
                    if (BatteryLevel != "error" && BackgroundManager != BackgroundAccessStatus.AlwaysAllowed)
                    {
                        if (setNotiStyle == 0)
                        {
                            Tile_XmlContent.LoadXml("<toast launch=\"NotiBatterySaver\"><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/BatteryLevelSquare.png\"/><text id=\"1\">Hello battery device user,</text><text id=\"2\">Please make sure TimeMe is always allowed to run in the background battery saver to keep the tile up-to-date.</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T8", Group = "G1" });
                        }
                        else if (setNotiStyle == 1)
                        {
                            Tile_XmlContent.LoadXml("<toast launch=\"NotiBatterySaver\"><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/BatteryLevelSquare.png\"/><text id=\"1\">Hello battery device user,</text><text id=\"2\">Please make sure TimeMe is always allowed to run in the background battery saver to keep the tile up-to-date.</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T8", Group = "G1" });
                        }
                        else if (setNotiStyle == 2)
                        {
                            Tile_XmlContent.LoadXml("<toast launch=\"NotiBatterySaver\"><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/BatteryLevelSquare.png\"/><text id=\"1\">Hello battery device user,</text><text id=\"2\">Please make sure TimeMe is always allowed to run in the background battery saver to keep the tile up-to-date.</text></binding></visual><audio silent=\"false\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T8", Group = "G1" });
                        }
                    }
                    //else if (!setNotiBattery) { Toast_History.Remove("T8", "G1"); }
                }
            }
            catch { }
        }

        //Notification - Next calendar time
        void ShowNotiCalendarTime()
        {
            try
            {
                if (setNotiCalendarTime && !String.IsNullOrEmpty(CalendarAppoName))
                {
                    if (setNotiStyle == 0)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Calendar.png\"/><text id=\"1\">" + WebUtility.HtmlEncode(CalendarAppoName) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(CalendarAppoSummary + "\n" + CalendarAppoEstimated) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T4", Group = "G1" });
                    }
                    else if (setNotiStyle == 1)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Calendar.png\"/><text id=\"1\">" + WebUtility.HtmlEncode(CalendarAppoName) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(CalendarAppoSummary + "\n" + CalendarAppoEstimated) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T4", Group = "G1" });
                    }
                    else if (setNotiStyle == 2)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Calendar.png\"/><text id=\"1\">" + WebUtility.HtmlEncode(CalendarAppoName) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(CalendarAppoSummary + "\n" + CalendarAppoEstimated) + "</text></binding></visual><audio silent=\"false\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T4", Group = "G1" });
                    }
                }
                //else { Toast_History.Remove("T4", "G1"); }
            }
            catch { }
        }

        //Notification - Day time progression
        void ShowNotiDayTime()
        {
            try
            {
                if (setNotiDayTime)
                {
                    //Load Day Time Remaining
                    TimeSpan TimeTomorrow = DateTime.Today.AddDays(1).Subtract(DateTimeNow);
                    int TimeTomorrowHours = TimeTomorrow.Hours; int TimeTomorrowMinutes = TimeTomorrow.Minutes;

                    string TimeTillTomorrow = "";
                    if (TimeTomorrowHours != 0) { TimeTillTomorrow = TimeTillTomorrow + TimeTomorrowHours + "h "; }
                    if (TimeTomorrowMinutes != 0) { TimeTillTomorrow = TimeTillTomorrow + TimeTomorrowMinutes + "m "; }
                    if (String.IsNullOrEmpty(TimeTillTomorrow)) { TimeTillTomorrow = "a minute "; }

                    //Set Notification Clock Icon
                    string ClockIcon = "ms-appx:///Assets/Analog/Minimal/" + DateTimeNow.ToString("hmm") + ".png";

                    if (setNotiStyle == 0)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"" + ClockIcon + "\"/><text id=\"1\">The day has progressed " + DayTimeProgress + "%</text><text id=\"2\">And has about " + TimeTillTomorrow + "time remaining, enjoy the rest of your day!</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T5", Group = "G1" });
                    }
                    else if (setNotiStyle == 1)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"" + ClockIcon + "\"/><text id=\"1\">The day has progressed " + DayTimeProgress + "%</text><text id=\"2\">And has about " + TimeTillTomorrow + "time remaining, enjoy the rest of your day!</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T5", Group = "G1" });
                    }
                    else if (setNotiStyle == 2)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"" + ClockIcon + "\"/><text id=\"1\">The day has progressed " + DayTimeProgress + "%</text><text id=\"2\">And has about " + TimeTillTomorrow + "time remaining, enjoy the rest of your day!</text></binding></visual><audio silent=\"false\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T5", Group = "G1" });
                    }
                }
                //else { Toast_History.Remove("T5", "G1"); }
            }
            catch { }
        }

        //Notification - Network change
        void ShowNotiNetworkChange()
        {
            try
            {
                if (setNotiNetworkChange)
                {
                    if (setNotiStyle == 0)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/Network.png\"/><text id=\"1\">Network has changed</text><text id=\"2\">You are now on: " + TextNetworkName + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T9", Group = "G1" });
                    }
                    else if (setNotiStyle == 1)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/Network.png\"/><text id=\"1\">Network has changed</text><text id=\"2\">You are now on: " + TextNetworkName + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T9", Group = "G1" });
                    }
                    else if (setNotiStyle == 2)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/Network.png\"/><text id=\"1\">Network has changed</text><text id=\"2\">You are now on: " + TextNetworkName + "</text></binding></visual><audio silent=\"false\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T9", Group = "G1" });
                    }
                }
                //else { Toast_History.Remove("T9", "G1"); }
            }
            catch { }
        }

        //Notification - Current week number
        void ShowNotiWeekNumber()
        {
            try
            {
                if (setNotiWeekNumber)
                {
                    //if (DateTimeNow.DayOfWeek == vCultureInfoReg.DateTimeFormat.FirstDayOfWeek)
                    if (DateTimeNow.DayOfWeek == DayOfWeek.Monday || BgStatusLastRunDate == "Never")
                    {
                        if (BgStatusLastRunDate == "Never" || DateTime.Parse(BgStatusLastRunDate, vCultureInfoEng).Day != DateTimeNow.Day)
                        {
                            if (setNotiStyle == 0)
                            {
                                Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Week.png\"/><text id=\"1\">Welcome to a new week</text><text id=\"2\">We are in week number " + WeekNumberCurrent + "</text></binding></visual><audio silent=\"true\"/></toast>");
                                Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T6", Group = "G1" });
                            }
                            else if (setNotiStyle == 1)
                            {
                                Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Week.png\"/><text id=\"1\">Welcome to a new week</text><text id=\"2\">We are in week number " + WeekNumberCurrent + "</text></binding></visual><audio silent=\"true\"/></toast>");
                                Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T6", Group = "G1" });
                            }
                            else if (setNotiStyle == 2)
                            {
                                Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Week.png\"/><text id=\"1\">Welcome to a new week</text><text id=\"2\">We are in week number " + WeekNumberCurrent + "</text></binding></visual><audio silent=\"false\"/></toast>");
                                Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T6", Group = "G1" });
                            }
                        }
                    }
                }
                //else { Toast_History.Remove("T6", "G1"); }
            }
            catch { }
        }

        //Notification - Countdown Time
        void ShowNotiCountdownTime()
        {
            try
            {
                if (setNotiCountdownTime && !String.IsNullOrEmpty(CountdownEventName) && !String.IsNullOrEmpty(CountdownEventDate))
                {
                    string CountDownDateText = String.Empty;
                    if (CountdownEventDate == "today") { CountDownDateText = "Is today's event"; } else { CountDownDateText = "Arrives in " + CountdownEventDate + " days"; }

                    if (setNotiStyle == 0)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Countdown.png\"/><text id=\"1\">" + WebUtility.HtmlEncode(CountdownEventName) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(CountDownDateText) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T7", Group = "G1" });
                    }
                    else if (setNotiStyle == 1)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Countdown.png\"/><text id=\"1\">" + WebUtility.HtmlEncode(CountdownEventName) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(CountDownDateText) + "</text></binding></visual><audio silent=\"true\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T7", Group = "G1" });
                    }
                    else if (setNotiStyle == 2)
                    {
                        Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Menu/Countdown.png\"/><text id=\"1\">" + WebUtility.HtmlEncode(CountdownEventName) + "</text><text id=\"2\">" + WebUtility.HtmlEncode(CountDownDateText) + "</text></binding></visual><audio silent=\"false\"/></toast>");
                        Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = false, Tag = "T7", Group = "G1" });
                    }
                }
                //else { Toast_History.Remove("T7", "G1"); }
            }
            catch { }
        }
    }
}