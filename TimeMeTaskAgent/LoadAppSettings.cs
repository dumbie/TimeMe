using ArnoldVinkCode;
using System;
using System.Diagnostics;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Application Settings
        int setDisplayHorizontalAlignmentTime;
        int setDisplayPosition1;
        int setDisplayPosition2;
        int setDisplayPosition3;
        int setDisplayPosition4;
        bool setDownloadWeather;
        bool setDisplayWeatherWhiteIcons;
        bool setLockCalendar;
        bool setLockWeather;
        bool setLockWeatherDetailed;
        bool setLockLocation;
        bool setLockBattery;
        bool setLockBatteryDetailed;
        bool setLockEnter;
        bool setLockWeekNumber;
        bool setLockAlarm;
        bool setLockNetwork;
        bool setLockCountdown;
        bool setDisplay24hClock;
        bool setDisplayAMPMClock;
        bool setDisplayHourBold;
        bool setDisplayAMPMFont;
        bool setDisplayBackgroundColor;
        bool setDisplayBackgroundColorWeather;
        bool setDisplayBackgroundColorBattery;
        bool setDisplayBackgroundPhoto;
        bool setDisplayBackgroundPhotoWeather;
        bool setDisplayBackgroundPhotoBattery;
        float setDisplayBackgroundBrightnessFloat;
        int setDisplayBackgroundBrightnessInt;
        bool setShowMoreTiles;
        bool setDisplayRegionLanguage;
        bool setDisplayDateYear;
        bool setDisplayDateWeekNumber;
        bool setDisplayCurrentTime;
        int setDisplayTimeSplitter;
        bool setDisplayAlarm;
        bool setLiveTileFontDuoColor;
        bool setLiveTileTimeCutOut;
        bool setDisplayTimeCustomText;
        string setDisplayTimeCustomTextString;
        bool setLockscreenNoteText;
        string setLockscreenNoteTextString;
        bool setDisplayWeatherTempHighLow;
        bool setDisplayWeatherTileLocation;
        bool setDisplayWeatherTileProvider;
        bool setDisplayWeatherTileUpdateTime;
        bool setBackgroundDownload;
        bool setDownloadWifiOnly;
        bool setDownloadBingWallpaper;
        bool setWeatherGpsLocation;
        bool setLiveTileSizeLight;
        bool setAppDebug;
        string setLiveTileSizeName;
        string setWeatherTileSizeName;
        string setBatteryTileSizeName;
        string setLiveTileColorBackground;
        string setLiveTileColorFont;
        int setLiveTileFontWeight;
        int setLiveTileFontSize;
        string setLiveTileFont;
        string setWeatherNonGpsLocation;
        int setBackgroundDownloadIntervalMin;
        int setFahrenheitCelsius;
        int setDownloadBingRegion;
        int setDownloadBingResolution;
        bool setNotiWeatherCurrent;
        bool setNotiBingDescription;
        bool setNotiBattery;
        bool setNotiLowBattery;
        bool setNotiCalendarTime;
        bool setNotiCountdownTime;
        bool setNotiDayTime;
        bool setNotiWeekNumber;
        bool setNotiNetworkChange;
        bool setNotiBatterySaver;
        int setNotiStyle;
        bool setLockWallpaper;
        bool setDeviceWallpaper;
        bool setDevStatusMobile;
        TimeSpan setDevStatusBatteryTime;
        string BgStatusWeatherCurrent = "N/A";
        string BgStatusWeatherCurrentText = "N/A";
        string BgStatusWeatherCurrentTemp = "N/A";
        string BgStatusWeatherCurrentIcon = "1000";
        string BgStatusWeatherCurrentWindSpeed = "N/A";
        string BgStatusWeatherCurrentRainChance = "N/A";
        string BgStatusWeatherCurrentTempLow = "N/A";
        string BgStatusWeatherCurrentTempHigh = "N/A";
        string BgStatusWeatherCurrentLocationShort = "N/A";
        string BgStatusWeatherCurrentLocationFull = "N/A";
        string BgStatusWeatherProvider = "N/A";
        string BgStatusBingDescription = "Never";
        string BgStatusDownloadWeatherTime = "Never";
        string BgStatusDownloadLocation = "Never";
        string BgStatusDownloadBing = "Never";
        string BgStatusPhotoName = "N/A";
        string BgStatusNetworkName = "N/A";
        string BgStatusLastRunDate = "Never";

        bool LoadAppSettings()
        {
            try
            {
                //Load tile and application settings
                Debug.WriteLine("Loading the tile and application settings.");
                setDisplayHorizontalAlignmentTime = (int)vApplicationSettings["DisplayHorizontalAlignmentTime"];
                setDisplayPosition1 = (int)vApplicationSettings["DisplayPosition1"];
                setDisplayPosition2 = (int)vApplicationSettings["DisplayPosition2"];
                setDisplayPosition3 = (int)vApplicationSettings["DisplayPosition3"];
                setDisplayPosition4 = (int)vApplicationSettings["DisplayPosition4"];
                setDownloadWeather = (bool)vApplicationSettings["DownloadWeather"];
                setDisplayWeatherWhiteIcons = (bool)vApplicationSettings["DisplayWeatherWhiteIcons"];
                setLockCalendar = (bool)vApplicationSettings["LockCalendar"];
                setLockWeather = (bool)vApplicationSettings["LockWeather"];
                setLockWeatherDetailed = (bool)vApplicationSettings["LockWeatherDetailed"];
                setLockLocation = (bool)vApplicationSettings["LockLocation"];
                setLockBattery = (bool)vApplicationSettings["LockBattery"];
                setLockBatteryDetailed = (bool)vApplicationSettings["LockBatteryDetailed"];
                setLockEnter = (bool)vApplicationSettings["LockEnter"];
                setLockWeekNumber = (bool)vApplicationSettings["LockWeekNumber"];
                setLockAlarm = (bool)vApplicationSettings["LockAlarm"];
                setLockNetwork = (bool)vApplicationSettings["LockNetwork"];
                setLockCountdown = (bool)vApplicationSettings["LockCountdown"];
                setDisplay24hClock = (bool)vApplicationSettings["Display24hClock"];
                setDisplayAMPMClock = (bool)vApplicationSettings["DisplayAMPMClock"];
                setDisplayHourBold = (bool)vApplicationSettings["DisplayHourBold"];
                setDisplayAMPMFont = (bool)vApplicationSettings["DisplayAMPMFont"];
                setDisplayBackgroundColor = (bool)vApplicationSettings["DisplayBackgroundColor"];
                setDisplayBackgroundColorWeather = (bool)vApplicationSettings["DisplayBackgroundColorWeather"];
                setDisplayBackgroundColorBattery = (bool)vApplicationSettings["DisplayBackgroundColorBattery"];
                setDisplayBackgroundPhoto = (bool)vApplicationSettings["DisplayBackgroundPhoto"];
                setDisplayBackgroundPhotoWeather = (bool)vApplicationSettings["DisplayBackgroundPhotoWeather"];
                setDisplayBackgroundPhotoBattery = (bool)vApplicationSettings["DisplayBackgroundPhotoBattery"];
                setDisplayBackgroundBrightnessFloat = ((float)Convert.ToInt32(vApplicationSettings["DisplayBackgroundBrightness"]) / 100);
                setDisplayBackgroundBrightnessInt = (100 - Convert.ToInt32(vApplicationSettings["DisplayBackgroundBrightness"]));
                setShowMoreTiles = (bool)vApplicationSettings["ShowMoreTiles"];
                setDisplayRegionLanguage = (bool)vApplicationSettings["DisplayRegionLanguage"];
                setDisplayDateYear = (bool)vApplicationSettings["DisplayDateYear"];
                setDisplayDateWeekNumber = (bool)vApplicationSettings["DisplayDateWeekNumber"];
                setDisplayCurrentTime = (bool)vApplicationSettings["DisplayCurrentTime"];
                setDisplayTimeSplitter = (int)vApplicationSettings["DisplayTimeSplitter"];
                setDisplayAlarm = (bool)vApplicationSettings["DisplayAlarm"];
                setLiveTileFontDuoColor = (bool)vApplicationSettings["LiveTileFontDuoColor"];
                setLiveTileTimeCutOut = (bool)vApplicationSettings["LiveTileTimeCutOut"];
                setDisplayTimeCustomText = (bool)vApplicationSettings["DisplayTimeCustomText"];
                setDisplayTimeCustomTextString = vApplicationSettings["DisplayTimeCustomTextString"].ToString();
                setLockscreenNoteText = (bool)vApplicationSettings["LockscreenNoteText"];
                setLockscreenNoteTextString = vApplicationSettings["LockscreenNoteTextString"].ToString();
                setDisplayWeatherTempHighLow = (bool)vApplicationSettings["DisplayWeatherTempHighLow"];
                setDisplayWeatherTileLocation = (bool)vApplicationSettings["DisplayWeatherTileLocation"];
                setDisplayWeatherTileProvider = (bool)vApplicationSettings["DisplayWeatherTileProvider"];
                setDisplayWeatherTileUpdateTime = (bool)vApplicationSettings["DisplayWeatherTileUpdateTime"];
                setBackgroundDownload = (bool)vApplicationSettings["BackgroundDownload"];
                setDownloadWifiOnly = (bool)vApplicationSettings["DownloadWifiOnly"];
                setDownloadBingWallpaper = (bool)vApplicationSettings["DownloadBingWallpaper"];
                setWeatherGpsLocation = (bool)vApplicationSettings["WeatherGpsLocation"];
                setLiveTileSizeLight = (bool)vApplicationSettings["LiveTileSizeLight"];
                setAppDebug = (bool)vApplicationSettings["AppDebug"];
                setLiveTileSizeName = vApplicationSettings["LiveTileSizeName"].ToString();
                setWeatherTileSizeName = vApplicationSettings["WeatherTileSizeName"].ToString();
                setBatteryTileSizeName = vApplicationSettings["BatteryTileSizeName"].ToString();
                setLiveTileColorBackground = vApplicationSettings["LiveTileColorBackground"].ToString();
                setLiveTileColorFont = vApplicationSettings["LiveTileColorFont"].ToString();
                setLiveTileFontWeight = (int)vApplicationSettings["LiveTileFontWeight"];
                setLiveTileFontSize = (int)vApplicationSettings["LiveTileFontSize"];
                setLiveTileFont = vApplicationSettings["LiveTileFont"].ToString();
                setWeatherNonGpsLocation = vApplicationSettings["WeatherNonGpsLocation"].ToString();
                setBackgroundDownloadIntervalMin = (int)vApplicationSettings["BackgroundDownloadIntervalMin"];
                setFahrenheitCelsius = (int)vApplicationSettings["FahrenheitCelsius"];
                setDownloadBingRegion = (int)vApplicationSettings["DownloadBingRegion"];
                setDownloadBingResolution = (int)vApplicationSettings["DownloadBingResolution"];
                setNotiWeatherCurrent = (bool)vApplicationSettings["NotiWeatherCurrent"];
                setNotiBingDescription = (bool)vApplicationSettings["NotiBingDescription"];
                setNotiBattery = (bool)vApplicationSettings["NotiBattery"];
                setNotiLowBattery = (bool)vApplicationSettings["NotiLowBattery"];
                setNotiCalendarTime = (bool)vApplicationSettings["NotiCalendarTime"];
                setNotiCountdownTime = (bool)vApplicationSettings["NotiCountdownTime"];
                setNotiDayTime = (bool)vApplicationSettings["NotiDayTime"];
                setNotiWeekNumber = (bool)vApplicationSettings["NotiWeekNumber"];
                setNotiNetworkChange = (bool)vApplicationSettings["NotiNetworkChange"];
                setNotiBatterySaver = (bool)vApplicationSettings["NotiBatterySaver"];
                setNotiStyle = (int)vApplicationSettings["NotiStyle"];
                setLockWallpaper = (bool)vApplicationSettings["LockWallpaper"];
                setDeviceWallpaper = (bool)vApplicationSettings["DeviceWallpaper"];

                //Load Device Status Settings
                setDevStatusMobile = (bool)vApplicationSettings["DevStatusMobile"];
                setDevStatusBatteryTime = (TimeSpan)vApplicationSettings["DevStatusBatteryTime"];

                //Load Background Status Settings
                BgStatusWeatherCurrent = vApplicationSettings["BgStatusWeatherCurrent"].ToString();
                BgStatusWeatherCurrentText = vApplicationSettings["BgStatusWeatherCurrentText"].ToString();
                BgStatusWeatherCurrentTemp = vApplicationSettings["BgStatusWeatherCurrentTemp"].ToString();
                BgStatusWeatherCurrentIcon = vApplicationSettings["BgStatusWeatherCurrentIcon"].ToString();
                BgStatusWeatherCurrentWindSpeed = vApplicationSettings["BgStatusWeatherCurrentWindSpeed"].ToString();
                BgStatusWeatherCurrentRainChance = vApplicationSettings["BgStatusWeatherCurrentRainChance"].ToString();
                BgStatusWeatherCurrentTempLow = vApplicationSettings["BgStatusWeatherCurrentTempLow"].ToString();
                BgStatusWeatherCurrentTempHigh = vApplicationSettings["BgStatusWeatherCurrentTempHigh"].ToString();
                BgStatusWeatherCurrentLocationShort = vApplicationSettings["BgStatusWeatherCurrentLocationShort"].ToString();
                BgStatusWeatherCurrentLocationFull = vApplicationSettings["BgStatusWeatherCurrentLocationFull"].ToString();
                BgStatusWeatherProvider = vApplicationSettings["BgStatusWeatherProvider"].ToString();
                BgStatusBingDescription = vApplicationSettings["BgStatusBingDescription"].ToString();
                BgStatusDownloadWeatherTime = vApplicationSettings["BgStatusDownloadWeatherTime"].ToString();
                BgStatusDownloadLocation = vApplicationSettings["BgStatusDownloadLocation"].ToString();
                BgStatusDownloadBing = vApplicationSettings["BgStatusDownloadBing"].ToString();
                BgStatusPhotoName = vApplicationSettings["BgStatusPhotoName"].ToString();
                BgStatusNetworkName = vApplicationSettings["BgStatusNetworkName"].ToString();
                BgStatusLastRunDate = vApplicationSettings["BgStatusLastRunDate"].ToString();

                //Check if the device has recently booted
                FreshDeviceBoot = TimeSpan.FromMilliseconds(AVImports.GetTickCount64()).TotalMinutes < 6;
                return true;
            }
            catch { return false; }
        }

        //Update Background Status Settings
        void BackgroundStatusUpdateSettings(string DownloadWeather, string DownloadLocation, string DownloadBing, string BingDescription, string AppDebugMsg)
        {
            try
            {
                //Always set to Failed, Never or DateTime
                if (!String.IsNullOrEmpty(DownloadWeather))
                {
                    BgStatusWeatherProvider = vApplicationSettings["BgStatusWeatherProvider"].ToString();
                    if (!BgStatusWeatherProvider.EndsWith("!")) { BgStatusWeatherProvider = BgStatusWeatherProvider + "!"; vApplicationSettings["BgStatusWeatherProvider"] = BgStatusWeatherProvider; }

                    BgStatusWeatherCurrent = vApplicationSettings["BgStatusWeatherCurrent"].ToString();
                    if (!BgStatusWeatherCurrent.EndsWith("!")) { BgStatusWeatherCurrent = BgStatusWeatherCurrent + "!"; vApplicationSettings["BgStatusWeatherCurrent"] = BgStatusWeatherCurrent; }

                    BgStatusWeatherCurrentTemp = vApplicationSettings["BgStatusWeatherCurrentTemp"].ToString();
                    if (!BgStatusWeatherCurrentTemp.EndsWith("!")) { BgStatusWeatherCurrentTemp = BgStatusWeatherCurrentTemp + "!"; vApplicationSettings["BgStatusWeatherCurrentTemp"] = BgStatusWeatherCurrentTemp; }

                    BgStatusDownloadWeatherTime = DownloadWeather;
                    vApplicationSettings["BgStatusDownloadWeatherTime"] = DownloadWeather;
                }

                //Always set to Never or DateTime
                if (!String.IsNullOrEmpty(DownloadLocation))
                {
                    BgStatusWeatherCurrentLocationShort = vApplicationSettings["BgStatusWeatherCurrentLocationShort"].ToString();
                    if (!BgStatusWeatherCurrentLocationShort.EndsWith("!")) { BgStatusWeatherCurrentLocationShort = BgStatusWeatherCurrentLocationShort + "!"; vApplicationSettings["BgStatusWeatherCurrentLocationShort"] = BgStatusWeatherCurrentLocationShort; }

                    BgStatusWeatherCurrentLocationFull = vApplicationSettings["BgStatusWeatherCurrentLocationFull"].ToString();
                    if (!BgStatusWeatherCurrentLocationFull.EndsWith("!")) { BgStatusWeatherCurrentLocationFull = BgStatusWeatherCurrentLocationFull + "!"; vApplicationSettings["BgStatusWeatherCurrentLocationFull"] = BgStatusWeatherCurrentLocationFull; }

                    BgStatusDownloadLocation = DownloadLocation;
                    vApplicationSettings["BgStatusDownloadLocation"] = DownloadLocation;
                }

                //Always set to Never or DateTime
                if (!String.IsNullOrEmpty(DownloadBing)) { BgStatusDownloadBing = DownloadBing; vApplicationSettings["BgStatusDownloadBing"] = DownloadBing; }

                //Always set to current Bing Description
                if (!String.IsNullOrEmpty(BingDescription)) { BgStatusBingDescription = BingDescription; vApplicationSettings["BgStatusBingDescription"] = BingDescription; }

                //Set an application debug message
                if (!String.IsNullOrEmpty(AppDebugMsg)) { vApplicationSettings["AppDebugMsg"] = AppDebugMsg; }
            }
            catch { }
        }
    }
}