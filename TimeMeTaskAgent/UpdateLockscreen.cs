using System;
using System.Diagnostics;
using System.Net;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Update the lockscreen information
        void UpdateLockscreen()
        {
            try
            {
                Debug.WriteLine("Updating the lock screen information.");

                //Set enter string if enabled
                if (setLockEnter) { LockscreenEnter = "\n"; }

                //Load lockscreen calendar
                if (setLockCalendar && !String.IsNullOrEmpty(CalendarAppoName))
                {
                    string CalendarInfoLock = "";
                    if (!String.IsNullOrEmpty(CalendarAppoSummary)) { CalendarInfoLock = "\n" + CalendarAppoSummary; }
                    if (!String.IsNullOrEmpty(CalendarAppoEstimated)) { CalendarInfoLock = CalendarInfoLock + "\n" + CalendarAppoEstimated; }
                    LockscreenText = CalendarAppoName + CalendarInfoLock + LockscreenEnter;
                }

                //Load lockscreen countdown
                if (setLockCountdown && !String.IsNullOrEmpty(CountdownEventDate))
                {
                    string CalenderCountInfo = CountdownEventName + " (" + CountdownEventDate + "d)";
                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = CalenderCountInfo + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\n" + CalenderCountInfo + LockscreenEnter; }
                }

                //Load lockscreen week number
                if (setLockWeekNumber)
                {
                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = "Week number " + WeekNumberCurrent + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\nWeek number " + WeekNumberCurrent + LockscreenEnter; }
                }

                //Load lockscreen Weather
                if (setBackgroundDownload && setDownloadWeather && setLockWeather)
                {
                    if (String.IsNullOrEmpty(LockscreenText))
                    {
                        if ((setLockWeatherDetailed || setLockLocation) && setLockEnter) { LockscreenText = BgStatusWeatherCurrent; }
                        else { LockscreenText = BgStatusWeatherCurrent + LockscreenEnter; }
                    }
                    else
                    {
                        if ((setLockWeatherDetailed || setLockLocation) && setLockEnter) { LockscreenText = LockscreenText + "\n" + BgStatusWeatherCurrent; }
                        else { LockscreenText = LockscreenText + "\n" + BgStatusWeatherCurrent + LockscreenEnter; }
                    }
                }

                //Load lockscreen Weather Detailed
                if (setBackgroundDownload && setDownloadWeather && setLockWeatherDetailed)
                {
                    string LockRainChance = BgStatusWeatherCurrentRainChance + " chance of rain";
                    string LockWindSpeed = BgStatusWeatherCurrentWindSpeed + " windspeed";

                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = LockRainChance + "\n" + LockWindSpeed + LockscreenEnter; }
                    else
                    {
                        if (setLockLocation && setLockEnter) { LockscreenText = LockscreenText + "\n" + LockRainChance + "\n" + LockWindSpeed; }
                        else { LockscreenText = LockscreenText + "\n" + LockRainChance + "\n" + LockWindSpeed + LockscreenEnter; }
                    }
                }

                //Load lockscreen location
                if (setBackgroundDownload && setDownloadWeather && setLockLocation)
                {
                    string LockLocation = BgStatusWeatherCurrentLocationShort;
                    if (LockLocation.Length < 7) { LockLocation = "Near town " + LockLocation; }
                    else if (LockLocation.Length < 15) { LockLocation = "Near " + LockLocation; }

                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = LockLocation + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\n" + LockLocation + LockscreenEnter; }
                }

                //Load lockscreen alarm status
                if (setLockAlarm && TimerAlarmActive)
                {
                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = "Timer alarm is on" + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\nTimer alarm is on" + LockscreenEnter; }
                }

                //Load custom lockscreen text
                if (setLockscreenNoteText)
                {
                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = setLockscreenNoteTextString + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\n" + setLockscreenNoteTextString + LockscreenEnter; }
                }

                //Load lockscreen current network name
                if (setLockNetwork)
                {
                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = "Network " + TextNetworkName + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\nNetwork " + TextNetworkName + LockscreenEnter; }
                }

                //Load lockscreen detailed battery
                if (setLockBatteryDetailed && BatteryLevel != "error")
                {
                    string LockBattery = "";
                    if (BatteryCharging) { LockBattery = "Battery level is " + BatteryLevel + "%\nand is now charging"; }
                    else { LockBattery = "Battery level is " + BatteryLevel + "% and\n" + BatteryTime.ToLower() + "time remaining"; }

                    if (String.IsNullOrEmpty(LockscreenText)) { LockscreenText = LockBattery + LockscreenEnter; }
                    else { LockscreenText = LockscreenText + "\n" + LockBattery + LockscreenEnter; }
                }

                //Update Lockscreen Information
                Tile_XmlContent.LoadXml("<tile><visual><binding template=\"TileWide\" hint-lockDetailedStatus1=\"" + WebUtility.HtmlEncode(LockscreenText) + "\"><image src=\"ms-appx:///Assets/WideLogo310150.scale-200.png\" placement=\"background\"/></binding></visual></tile>");
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(Tile_XmlContent));

                //Update Battery Badge Information
                if (setLockBattery)
                {
                    Tile_XmlContent.LoadXml("<badge value=\"" + BatteryLevel + "\"/>");
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(Tile_XmlContent));
                }
                else
                {
                    Tile_XmlContent.LoadXml("<badge value=\"none\"/>");
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(Tile_XmlContent));
                }
            }
            catch
            {
                Debug.WriteLine("Failed to update the lock screen information");

                //Update Badge Information
                Tile_XmlContent.LoadXml("<badge value=\"error\"/>");
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(Tile_XmlContent));

                //Update Lockscreen Information
                Tile_XmlContent.LoadXml("<tile><visual><binding template=\"TileWide\" hint-lockDetailedStatus1=\"Failed to update lock information\"><image src=\"ms-appx:///Assets/WideLogo310150.scale-200.png\" placement=\"background\"/></binding></visual></tile>");
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(Tile_XmlContent));
            }
        }
    }
}