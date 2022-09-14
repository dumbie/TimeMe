using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System.Power;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace TimeMe
{
    partial class MainPage
    {
        //Check - Application Settings
        async Task SettingsCheck()
        {
            try
            {
                //Check if there is battery status available
                BatteryStatus BatteryStatus = PowerManager.BatteryStatus;

                //Check - Display Horizontal Alignment Time
                if (!vApplicationSettings.ContainsKey("DisplayHorizontalAlignmentTime")) { vApplicationSettings["DisplayHorizontalAlignmentTime"] = 0; }

                //Check - Display Text Position 1
                if (!vApplicationSettings.ContainsKey("DisplayPosition1")) { vApplicationSettings["DisplayPosition1"] = (int)Setting_TextPositions.DayOnly; }

                //Check - Display Text Position 2
                if (!vApplicationSettings.ContainsKey("DisplayPosition2")) { vApplicationSettings["DisplayPosition2"] = (int)Setting_TextPositions.DateMonth; }

                //Check - Display Text Position 3
                if (!vApplicationSettings.ContainsKey("DisplayPosition3"))
                {
                    if (BatteryStatus == BatteryStatus.NotPresent) { vApplicationSettings["DisplayPosition3"] = (int)Setting_TextPositions.Location; }
                    else { vApplicationSettings["DisplayPosition3"] = (int)Setting_TextPositions.Battery; }
                }

                //Check - Display Text Position 4
                if (!vApplicationSettings.ContainsKey("DisplayPosition4")) { vApplicationSettings["DisplayPosition4"] = (int)Setting_TextPositions.WeatherFull; }

                //Check Device Status Settings
                if (AVFunctions.DevMobile()) { vApplicationSettings["DevStatusMobile"] = true; } else { vApplicationSettings["DevStatusMobile"] = false; }
                if (!vApplicationSettings.ContainsKey("DevStatusBatteryTime")) { vApplicationSettings["DevStatusBatteryTime"] = new TimeSpan(); }

                //Check - Display Live Tile Size and Style
                if (!vApplicationSettings.ContainsKey("LiveTileSizeName")) { vApplicationSettings["LiveTileSizeName"] = "WideText"; }
                if (!vApplicationSettings.ContainsKey("LiveTileSizeLight")) { vApplicationSettings["LiveTileSizeLight"] = false; }

                //Check - Display Weather Tile Size and Style
                if (!vApplicationSettings.ContainsKey("WeatherTileSizeName")) { vApplicationSettings["WeatherTileSizeName"] = "WeatherCombo"; }

                //Check - Display Battery Tile Size and Style
                if (!vApplicationSettings.ContainsKey("BatteryTileSizeName")) { vApplicationSettings["BatteryTileSizeName"] = "BatteryIcon"; }

                //Check - Display Live Tile Time Font Family
                if (!vApplicationSettings.ContainsKey("LiveTileFont")) { vApplicationSettings["LiveTileFont"] = "Segoe UI"; }

                //Check - Increase the live tile font size
                if (!vApplicationSettings.ContainsKey("LiveTileFontSize")) { vApplicationSettings["LiveTileFontSize"] = 0; }

                //Check - Display Live Tile Time Font Color
                if (!vApplicationSettings.ContainsKey("LiveTileColorFont")) { vApplicationSettings["LiveTileColorFont"] = Colors.White.ToString(); }

                //Check - Is option 'Show More Tiles' enabled?
                if (!vApplicationSettings.ContainsKey("ShowMoreTiles")) { vApplicationSettings["ShowMoreTiles"] = true; }

                //Check - Display keyboard/region language text
                if (!vApplicationSettings.ContainsKey("DisplayRegionLanguage")) { vApplicationSettings["DisplayRegionLanguage"] = true; }

                //Check - Display 24-hour Clock
                if (!vApplicationSettings.ContainsKey("Display24hClock"))
                {
                    if (!String.IsNullOrEmpty(vCultureInfoReg.DateTimeFormat.AMDesignator)) { vApplicationSettings["Display24hClock"] = false; }
                    else { vApplicationSettings["Display24hClock"] = true; }
                }

                //Check - Display AM or PM Clock
                if (!vApplicationSettings.ContainsKey("DisplayAMPMClock"))
                {
                    if (!String.IsNullOrEmpty(vCultureInfoReg.DateTimeFormat.AMDesignator)) { vApplicationSettings["DisplayAMPMClock"] = true; }
                    else { vApplicationSettings["DisplayAMPMClock"] = false; }
                }

                //Check - Display the hour bold
                if (!vApplicationSettings.ContainsKey("DisplayHourBold")) { vApplicationSettings["DisplayHourBold"] = false; }

                //Check - Display AM or PM Font
                if (!vApplicationSettings.ContainsKey("DisplayAMPMFont")) { vApplicationSettings["DisplayAMPMFont"] = true; }

                //Check - Display Current Time
                if (!vApplicationSettings.ContainsKey("DisplayCurrentTime")) { vApplicationSettings["DisplayCurrentTime"] = true; }

                //Check - Display Time Splitter
                if (!vApplicationSettings.ContainsKey("DisplayTimeSplitter")) { vApplicationSettings["DisplayTimeSplitter"] = 0; }

                //Check - Display Date Year
                if (!vApplicationSettings.ContainsKey("DisplayDateYear")) { vApplicationSettings["DisplayDateYear"] = false; }

                //Check - Display Date Week number
                if (!vApplicationSettings.ContainsKey("DisplayDateWeekNumber")) { vApplicationSettings["DisplayDateWeekNumber"] = false; }

                //Check - Display Weather White Icons
                if (!vApplicationSettings.ContainsKey("DisplayWeatherWhiteIcons")) { vApplicationSettings["DisplayWeatherWhiteIcons"] = true; }

                //Check - Sleep Weather
                if (!vApplicationSettings.ContainsKey("SleepWeather")) { vApplicationSettings["SleepWeather"] = true; }

                //Check - Sleep Date
                if (!vApplicationSettings.ContainsKey("SleepDate")) { vApplicationSettings["SleepDate"] = true; }

                //Check - Sleep Analog Clock
                if (!vApplicationSettings.ContainsKey("SleepAnalogClock")) { vApplicationSettings["SleepAnalogClock"] = false; }

                //Check - Sleep Battery Level
                if (!vApplicationSettings.ContainsKey("SleepBattery"))
                {
                    if (BatteryStatus == BatteryStatus.NotPresent) { vApplicationSettings["SleepBattery"] = false; }
                    else { vApplicationSettings["SleepBattery"] = true; }
                }

                //Check - Display Alarm Icon
                if (!vApplicationSettings.ContainsKey("DisplayAlarm")) { vApplicationSettings["DisplayAlarm"] = true; }

                //Check - Display Live Tile Font Duo Color
                if (!vApplicationSettings.ContainsKey("LiveTileFontDuoColor")) { vApplicationSettings["LiveTileFontDuoColor"] = false; }

                //Check - Cutout Live Tile Time Text
                if (!vApplicationSettings.ContainsKey("LiveTileTimeCutOut")) { vApplicationSettings["LiveTileTimeCutOut"] = false; }

                //Check - Display Time Custom Text / String
                if (!vApplicationSettings.ContainsKey("DisplayTimeCustomText")) { vApplicationSettings["DisplayTimeCustomText"] = false; }
                if (!vApplicationSettings.ContainsKey("DisplayTimeCustomTextString")) { vApplicationSettings["DisplayTimeCustomTextString"] = "It is *time*"; }

                //Check - Lockscreen Note Text / String
                if (!vApplicationSettings.ContainsKey("LockscreenNoteText")) { vApplicationSettings["LockscreenNoteText"] = false; }
                if (!vApplicationSettings.ContainsKey("LockscreenNoteTextString")) { vApplicationSettings["LockscreenNoteTextString"] = ""; }

                //Check - Display Weather High and Low Temp
                if (!vApplicationSettings.ContainsKey("DisplayWeatherTempHighLow")) { vApplicationSettings["DisplayWeatherTempHighLow"] = true; }

                //Check - Display Weather Tile Location
                if (!vApplicationSettings.ContainsKey("DisplayWeatherTileLocation")) { vApplicationSettings["DisplayWeatherTileLocation"] = true; }

                //Check - Display Weather Tile Provider
                if (!vApplicationSettings.ContainsKey("DisplayWeatherTileProvider")) { vApplicationSettings["DisplayWeatherTileProvider"] = false; }

                //Check - Display Weather Tile UpdateTime
                if (!vApplicationSettings.ContainsKey("DisplayWeatherTileUpdateTime")) { vApplicationSettings["DisplayWeatherTileUpdateTime"] = true; }

                //Check - Sleep Alarm Icon
                if (!vApplicationSettings.ContainsKey("SleepAlarm")) { vApplicationSettings["SleepAlarm"] = true; }

                //Check - Background Color
                if (!vApplicationSettings.ContainsKey("LiveTileColorBackground")) { vApplicationSettings["LiveTileColorBackground"] = Colors.DimGray.ToString(); }

                //Check - Display Background Color Live
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundColor")) { vApplicationSettings["DisplayBackgroundColor"] = false; }

                //Check - Display Background Color Weather
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundColorWeather")) { vApplicationSettings["DisplayBackgroundColorWeather"] = false; }

                //Check - Display Background Color Battery
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundColorBattery")) { vApplicationSettings["DisplayBackgroundColorBattery"] = false; }

                //Check - Display Background Photo Live
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundPhoto")) { vApplicationSettings["DisplayBackgroundPhoto"] = false; }

                //Check - Display Background Photo Weather
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundPhotoWeather")) { vApplicationSettings["DisplayBackgroundPhotoWeather"] = false; }

                //Check - Display Background Photo Battery
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundPhotoBattery")) { vApplicationSettings["DisplayBackgroundPhotoBattery"] = false; }

                //Check - Display Background Brightness
                if (!vApplicationSettings.ContainsKey("DisplayBackgroundBrightness")) { vApplicationSettings["DisplayBackgroundBrightness"] = "70"; }

                //Check - Download Bing Wallpaper
                if (!vApplicationSettings.ContainsKey("DownloadBingWallpaper")) { vApplicationSettings["DownloadBingWallpaper"] = false; }

                //Check - Weather Download Enabled or Disabled
                if (!vApplicationSettings.ContainsKey("DownloadWeather")) { vApplicationSettings["DownloadWeather"] = true; }

                //Check - Use GPS for Weather Location
                if (!vApplicationSettings.ContainsKey("WeatherGpsLocation")) { vApplicationSettings["WeatherGpsLocation"] = true; }

                //Check - Display Weather Background
                if (!vApplicationSettings.ContainsKey("WeatherDisplayWallpaper")) { vApplicationSettings["WeatherDisplayWallpaper"] = true; }

                //Check - Non GPS Weather Location
                if (!vApplicationSettings.ContainsKey("WeatherNonGpsLocation")) { vApplicationSettings["WeatherNonGpsLocation"] = ""; }

                //Check - Weather Fahrenheit or Celsius
                if (!vApplicationSettings.ContainsKey("FahrenheitCelsius"))
                {
                    if (!vCultureInfoReg.IsNeutralCulture)
                    {
                        if (!new RegionInfo(vCultureInfoReg.Name).IsMetric) { vApplicationSettings["FahrenheitCelsius"] = 0; }
                        else { vApplicationSettings["FahrenheitCelsius"] = 1; }
                    }
                    else
                    { vApplicationSettings["FahrenheitCelsius"] = 1; }
                }

                //Check - Lockscreen Background 
                if (!vApplicationSettings.ContainsKey("LockWallpaper")) { vApplicationSettings["LockWallpaper"] = false; }

                //Check - Device Background 
                if (!vApplicationSettings.ContainsKey("DeviceWallpaper")) { vApplicationSettings["DeviceWallpaper"] = false; }

                //Check - Lockscreen Calendar
                if (!vApplicationSettings.ContainsKey("LockCalendar")) { vApplicationSettings["LockCalendar"] = true; }

                //Check - Lockscreen Weather
                if (!vApplicationSettings.ContainsKey("LockWeather")) { vApplicationSettings["LockWeather"] = true; }

                //Check - Lockscreen Weather Detailed
                if (!vApplicationSettings.ContainsKey("LockWeatherDetailed")) { vApplicationSettings["LockWeatherDetailed"] = false; }

                //Check - Lockscreen Location
                if (!vApplicationSettings.ContainsKey("LockLocation")) { vApplicationSettings["LockLocation"] = false; }

                //Check - Lockscreen Battery Level
                if (!vApplicationSettings.ContainsKey("LockBattery"))
                {
                    if (BatteryStatus == BatteryStatus.NotPresent) { vApplicationSettings["LockBattery"] = false; }
                    else { vApplicationSettings["LockBattery"] = true; }
                }

                //Check - Lockscreen Battery Level Detailed
                if (!vApplicationSettings.ContainsKey("LockBatteryDetailed")) { vApplicationSettings["LockBatteryDetailed"] = false; }

                //Check - Lockscreen Enter
                if (!vApplicationSettings.ContainsKey("LockEnter")) { vApplicationSettings["LockEnter"] = true; }

                //Check - Lockscreen Week Number
                if (!vApplicationSettings.ContainsKey("LockWeekNumber")) { vApplicationSettings["LockWeekNumber"] = false; }

                //Check - Lockscreen Alarm
                if (!vApplicationSettings.ContainsKey("LockAlarm")) { vApplicationSettings["LockAlarm"] = true; }

                //Check - Lockscreen Network
                if (!vApplicationSettings.ContainsKey("LockNetwork")) { vApplicationSettings["LockNetwork"] = false; }

                //Check - Lockscreen Countdown
                if (!vApplicationSettings.ContainsKey("LockCountdown")) { vApplicationSettings["LockCountdown"] = false; }

                //Check - Display current weather as notification
                if (!vApplicationSettings.ContainsKey("NotiWeatherCurrent")) { vApplicationSettings["NotiWeatherCurrent"] = false; }

                //Check - Display bing description as notification
                if (!vApplicationSettings.ContainsKey("NotiBingDescription")) { vApplicationSettings["NotiBingDescription"] = false; }

                //Check - Show battery time as notification
                if (!vApplicationSettings.ContainsKey("NotiBattery")) { vApplicationSettings["NotiBattery"] = false; }

                //Check - Show low battery warning as notification
                if (!vApplicationSettings.ContainsKey("NotiLowBattery"))
                {
                    if (BatteryStatus == BatteryStatus.NotPresent) { vApplicationSettings["NotiLowBattery"] = false; }
                    else { vApplicationSettings["NotiLowBattery"] = true; }
                }

                //Check - Show next calendar time as notification
                if (!vApplicationSettings.ContainsKey("NotiCalendarTime")) { vApplicationSettings["NotiCalendarTime"] = false; }

                //Check - Show countdown event days as notification
                if (!vApplicationSettings.ContainsKey("NotiCountdownTime")) { vApplicationSettings["NotiCountdownTime"] = false; }

                //Check - Show remaining day time as notification
                if (!vApplicationSettings.ContainsKey("NotiDayTime")) { vApplicationSettings["NotiDayTime"] = false; }

                //Check - Show week number as notification
                if (!vApplicationSettings.ContainsKey("NotiWeekNumber")) { vApplicationSettings["NotiWeekNumber"] = false; }

                //Check - Show network change as notification
                if (!vApplicationSettings.ContainsKey("NotiNetworkChange")) { vApplicationSettings["NotiNetworkChange"] = false; }

                //Check - Show battery saver as notification
                if (!vApplicationSettings.ContainsKey("NotiBatterySaver")) { vApplicationSettings["NotiBatterySaver"] = true; }

                //Check - Notifications Style
                if (!vApplicationSettings.ContainsKey("NotiStyle")) { vApplicationSettings["NotiStyle"] = 0; }

                //Check - Sleepscreen Double tap to Start
                if (!vApplicationSettings.ContainsKey("ScreenDoubleTapStart")) { vApplicationSettings["ScreenDoubleTapStart"] = false; }

                //Check - Sleepscreen display background
                if (!vApplicationSettings.ContainsKey("ScreenWallpaper")) { vApplicationSettings["ScreenWallpaper"] = false; }

                //Check - Flashlight camera output
                if (!vApplicationSettings.ContainsKey("FlashCameraOutput")) { vApplicationSettings["FlashCameraOutput"] = true; }

                //Check - Sleepscreen display orientation
                if (!vApplicationSettings.ContainsKey("ScreenOrientation")) { vApplicationSettings["ScreenOrientation"] = 1; }

                //Check - Sleepscreen Analog Clock Style
                if (!vApplicationSettings.ContainsKey("SleepAnalogClockStyle")) { vApplicationSettings["SleepAnalogClockStyle"] = 0; }

                //Check - Background Download Enabled or Disabled
                if (!vApplicationSettings.ContainsKey("BackgroundDownload")) { vApplicationSettings["BackgroundDownload"] = true; }

                //Check - Background Download Wi-Fi only
                if (!vApplicationSettings.ContainsKey("DownloadWifiOnly")) { vApplicationSettings["DownloadWifiOnly"] = false; }

                //Check - Background Download Update Interval
                if (!vApplicationSettings.ContainsKey("BackgroundDownloadInterval") && !vApplicationSettings.ContainsKey("BackgroundDownloadIntervalMin"))
                {
                    vApplicationSettings["BackgroundDownloadInterval"] = 2;
                    vApplicationSettings["BackgroundDownloadIntervalMin"] = 118;
                }

                //Check - Background Download Bing Region
                if (!vApplicationSettings.ContainsKey("DownloadBingRegion")) { vApplicationSettings["DownloadBingRegion"] = (int)Setting_BingRegion.Keyboard; }

                //Check - Background Download Bing Resolution
                if (!vApplicationSettings.ContainsKey("DownloadBingResolution"))
                {
                    if ((bool)vApplicationSettings["DevStatusMobile"]) { vApplicationSettings["DownloadBingResolution"] = (int)Setting_BingResolution._720x1280; }
                    else { vApplicationSettings["DownloadBingResolution"] = (int)Setting_BingResolution._1920x1080; }
                }

                //Check - Font Display Weight
                if (!vApplicationSettings.ContainsKey("LiveTileFontWeight")) { vApplicationSettings["LiveTileFontWeight"] = (int)Setting_FontWeight.Light; }

                //Check - Other Startup Tab
                if (!vApplicationSettings.ContainsKey("StartupTab")) { vApplicationSettings["StartupTab"] = 1; }

                //Check - Application debugging
                if (!vApplicationSettings.ContainsKey("AppDebug")) { vApplicationSettings["AppDebug"] = false; }

                //Check - Application debug message
                if (!vApplicationSettings.ContainsKey("AppDebugMsg")) { vApplicationSettings["AppDebugMsg"] = "Never"; }

                //Check - Screens Disable Back Button
                if (!vApplicationSettings.ContainsKey("ScreenBackButton")) { vApplicationSettings["ScreenBackButton"] = false; }

                //Check Background Status Settings
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrent")) { vApplicationSettings["BgStatusWeatherCurrent"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentText")) { vApplicationSettings["BgStatusWeatherCurrentText"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentTemp")) { vApplicationSettings["BgStatusWeatherCurrentTemp"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentIcon")) { vApplicationSettings["BgStatusWeatherCurrentIcon"] = "1000"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentWindSpeed")) { vApplicationSettings["BgStatusWeatherCurrentWindSpeed"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentRainChance")) { vApplicationSettings["BgStatusWeatherCurrentRainChance"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentTempLow")) { vApplicationSettings["BgStatusWeatherCurrentTempLow"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentTempHigh")) { vApplicationSettings["BgStatusWeatherCurrentTempHigh"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentLocationShort")) { vApplicationSettings["BgStatusWeatherCurrentLocationShort"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherCurrentLocationFull")) { vApplicationSettings["BgStatusWeatherCurrentLocationFull"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusWeatherProvider")) { vApplicationSettings["BgStatusWeatherProvider"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusBingDescription")) { vApplicationSettings["BgStatusBingDescription"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgStatusDownloadWeatherTime")) { vApplicationSettings["BgStatusDownloadWeatherTime"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgStatusDownloadLocation")) { vApplicationSettings["BgStatusDownloadLocation"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgStatusDownloadBing")) { vApplicationSettings["BgStatusDownloadBing"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgStatusPhotoName")) { vApplicationSettings["BgStatusPhotoName"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusNetworkName")) { vApplicationSettings["BgStatusNetworkName"] = "N/A"; }
                if (!vApplicationSettings.ContainsKey("BgStatusLastRunDate")) { vApplicationSettings["BgStatusLastRunDate"] = "Never"; }

                //Check Background Render Settings
                if (!vApplicationSettings.ContainsKey("BgRenderNameLast")) { vApplicationSettings["BgRenderNameLast"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgRenderNameTask")) { vApplicationSettings["BgRenderNameTask"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgRenderDateTask")) { vApplicationSettings["BgRenderDateTask"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgRenderNameRun")) { vApplicationSettings["BgRenderNameRun"] = "Never"; }
                if (!vApplicationSettings.ContainsKey("BgRenderDateRun")) { vApplicationSettings["BgRenderDateRun"] = "Never"; }

                //Check Application Version Update
                PackageVersion AppVersion = Package.Current.Id.Version;
                if (!vApplicationSettings.ContainsKey("AppVersion")) { vApplicationSettings["AppVersion"] = AppVersion.Major + "." + AppVersion.Minor + "." + AppVersion.Build + "." + AppVersion.Revision; }
                else if (vApplicationSettings["AppVersion"].ToString() != AppVersion.Major + "." + AppVersion.Minor + "." + AppVersion.Build + "." + AppVersion.Revision)
                {
                    vApplicationSettings["AppVersion"] = AppVersion.Major + "." + AppVersion.Minor + "." + AppVersion.Build + "." + AppVersion.Revision;
                }

                ////Delete all unused and outdated local files 
                //foreach (IStorageItem LocalFile in await ApplicationData.Current.LocalFolder.GetItemsAsync())
                //{
                //    //Delete all ghost TimeMeTilePhoto files
                //    if (LocalFile.Name.Contains("TimeMeTilePhoto") && LocalFile.Name != "TimeMeTilePhoto.png") { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }
                //    else if (LocalFile.Name.EndsWith("~tmp")) { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }

                //    //Delete unused and outdated xml files
                //    else if (LocalFile.Name == "TimeMeStatus.xml") { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }
                //    else if (LocalFile.Name == "TimeMeRender.xml") { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }
                //    else if (LocalFile.Name == "TimeMeWeatherData.xml") { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }
                //    else if (LocalFile.Name == "TimeMeWeatherCurrent.xml") { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }
                //    else if (LocalFile.Name == "TimeMeWeatherForecast.xml") { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }
                //}

                //Save and check Stopwatch XML File
                try
                {
                    if (!await AVFunctions.LocalFileExists("TimeMeStopwatch.xml")) { await StopwatchResetXml(); }
                    else
                    {
                        using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeStopwatch.xml"))
                        {
                            if (OpenStreamForReadAsync.Length < 50) { await StopwatchResetXml(); }
                            else
                            {
                                using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                                { if (!StreamReader.ReadToEnd().StartsWith("<")) { await StopwatchResetXml(); } }
                            }
                            OpenStreamForReadAsync.Dispose();
                        }
                    }
                }
                catch { Debug.WriteLine("Failed to check file: TimeMeStopwatch"); }

                //Save and check Countdown XML File
                try
                {
                    if (!await AVFunctions.LocalFileExists("TimeMeCountdown.xml")) { await CountdownResetXml(); }
                    else
                    {
                        using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeCountdown.xml"))
                        {
                            if (OpenStreamForReadAsync.Length < 50) { await CountdownResetXml(); }
                            else
                            {
                                using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                                { if (!StreamReader.ReadToEnd().StartsWith("<")) { await CountdownResetXml(); } }
                            }
                            OpenStreamForReadAsync.Dispose();
                        }
                    }
                }
                catch { Debug.WriteLine("Failed to check file: TimeMeCountdown"); }
            }
            catch (Exception Ex)
            {
                await new MessageDialog("The application settings failed to be set for the first time and can't continue, please try to restart the application.\n\nThe following error occured: " + Ex.Message, "TimeMe").ShowAsync();
                Application.Current.Exit();
                return;
            }
        }
    }
}