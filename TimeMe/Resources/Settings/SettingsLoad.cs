using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TimeMe
{
    partial class MainPage
    {
        //Load - Application Settings
        async Task SettingsLoad()
        {
            try
            {
                //Load - Display Horizontal Alignment Time
                cb_SettingsDisplayHorizontalAlignmentTime.SelectedIndex = (int)vApplicationSettings["DisplayHorizontalAlignmentTime"];
                if ((int)vApplicationSettings["DisplayHorizontalAlignmentTime"] != 0)
                {
                    cb_SettingsDisplayAMPMClock.IsEnabled = false;
                }

                //Load - Display Text Position 1
                cb_SettingsDisplayPosition1.SelectedIndex = (int)vApplicationSettings["DisplayPosition1"];
                //Load - Display Text Position 2
                cb_SettingsDisplayPosition2.SelectedIndex = (int)vApplicationSettings["DisplayPosition2"];
                //Load - Display Text Position 3
                cb_SettingsDisplayPosition3.SelectedIndex = (int)vApplicationSettings["DisplayPosition3"];
                //Load - Display Text Position 4
                cb_SettingsDisplayPosition4.SelectedIndex = (int)vApplicationSettings["DisplayPosition4"];

                //Load - Display Live Tile Size and Style
                if (vApplicationSettings["LiveTileSizeName"].ToString() == "Medium")
                { Medium.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumOne")
                { MediumOne.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumThree")
                { MediumThree.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumTimeOnly")
                { MediumTimeOnly.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumRoundImage")
                { MediumRoundImage.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumAnalogMinimal")
                { MediumAnalogMinimal.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumAnalogRound")
                { MediumAnalogRound.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumAnalogCortana")
                { MediumAnalogCortana.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumIcon")
                { MediumIcon.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "MediumText")
                { MediumText.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideText")
                { WideText.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideIcon")
                { WideIcon.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideLite")
                { WideLite.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideLefty")
                { WideLefty.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WidePhoto")
                { WidePhoto.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideDate")
                { WideDate.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideBig")
                { WideBig.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideMulti")
                { WideMulti.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideTwo")
                { WideTwo.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideTimeOnly")
                { WideTimeOnly.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideNumm")
                { WideNumm.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["LiveTileSizeName"].ToString() == "WideWords")
                { WideWords.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }

                //Load - Display Weather Tile Size and Style
                if (vApplicationSettings["WeatherTileSizeName"].ToString() == "WeatherLiteFlip")
                { WeatherLiteFlip.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["WeatherTileSizeName"].ToString() == "WeatherGrey")
                { WeatherGrey.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["WeatherTileSizeName"].ToString() == "WeatherIcon")
                { WeatherIcon.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["WeatherTileSizeName"].ToString() == "WeatherSumm")
                { WeatherSumm.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["WeatherTileSizeName"].ToString() == "WeatherForecast")
                { WeatherForecast.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["WeatherTileSizeName"].ToString() == "WeatherCombo")
                { WeatherCombo.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }

                //Load - Display Battery Tile Size and Style
                if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatteryIcon")
                { BatteryIcon.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatteryText")
                { BatteryText.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatteryTextLeft")
                { BatteryTextLeft.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatteryTextVert")
                { BatteryTextVert.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatteryTextVertTop")
                { BatteryTextVertTop.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatteryPerc")
                { BatteryPerc.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }
                else if (vApplicationSettings["BatteryTileSizeName"].ToString() == "BatterySumm")
                { BatterySumm.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80)); }

                //Load - Display Live Tile Time Font Family
                if (vApplicationSettings["LiveTileFont"].ToString() == "Segoe UI")
                {
                    Segoe.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Segoe.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT")
                {
                    Gothic720.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Gothic720.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue")
                {
                    HelveticaNeue.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_HelveticaNeue.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Existence-Light.ttf#Existence")
                {
                    Existence.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Existence.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Bellota-Light.ttf#Bellota")
                {
                    Bellota.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Bellota.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk")
                {
                    Rawengulk.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Rawengulk.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Dense-Regular.ttf#Dense")
                {
                    Dense.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Dense.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb")
                {
                    DigitalDisplay.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_DigitalDisplay.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/OneDay-Light.ttf#ONE DAY")
                {
                    OneDay.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_OneDay.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Panama-Light.ttf#Panama")
                {
                    Panama.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Panama.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Modeka-Light.ttf#Modeka")
                {
                    Modeka.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Modeka.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif")
                {
                    Nooa.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Nooa.Foreground = new SolidColorBrush(Colors.White);
                }
                else if (vApplicationSettings["LiveTileFont"].ToString() == "/Assets/Fonts/Pier-Regular.ttf#Pier Sans")
                {
                    Pier.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                    txt_Pier.Foreground = new SolidColorBrush(Colors.White);
                }

                //Load - Increase the live tile font size
                lp_SettingLiveTileFontSize.SelectedIndex = ((int)vApplicationSettings["LiveTileFontSize"] + 5);

                //Load - Display Live Tile Time Font Color Preview
                if (vApplicationSettings["LiveTileColorFont"].ToString() != ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString()) { MenuSet_FontColorPreview.Foreground = new SolidColorBrush(Color.FromArgb(Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(1, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(3, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(5, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(7, 2), 16))); }
                else { MenuSet_FontColorPreview.Foreground = new SolidColorBrush(Colors.White); }

                //Load - Is option 'Show More Tiles' enabled?
                cb_SettingShowMoreTiles.IsChecked = (bool)vApplicationSettings["ShowMoreTiles"];

                //Load - Display keyboard/region language text
                txtbox_SettingsDisplayRegionLanguage.Text = vCultureInfoReg.TwoLetterISOLanguageName.ToUpper();
                cb_SettingsDisplayRegionLanguage.IsChecked = (bool)vApplicationSettings["DisplayRegionLanguage"];

                //Load - Display 24-hour Clock
                cb_SettingsDisplay24hClock.IsChecked = (bool)vApplicationSettings["Display24hClock"];

                //Load - Display AM or PM Clock
                cb_SettingsDisplayAMPMClock.IsChecked = (bool)vApplicationSettings["DisplayAMPMClock"];

                //Load - Display the hour bold
                cb_SettingsDisplayHourBold.IsChecked = (bool)vApplicationSettings["DisplayHourBold"];

                //Load - Display AM or PM Font
                cb_SettingsDisplayAMPMFont.IsChecked = (bool)vApplicationSettings["DisplayAMPMFont"];

                //Load - Display Current Time
                cb_SettingsDisplayCurrentTime.IsChecked = (bool)vApplicationSettings["DisplayCurrentTime"];

                //Load - Display Time Splitter
                cb_SettingsDisplayTimeSplitter.SelectedIndex = (int)vApplicationSettings["DisplayTimeSplitter"];

                //Load - Display Date Year
                cb_SettingsDisplayDateYear.IsChecked = (bool)vApplicationSettings["DisplayDateYear"];

                //Load - Display Date Week number
                cb_SettingsDisplayDateWeekNumber.IsChecked = (bool)vApplicationSettings["DisplayDateWeekNumber"];

                //Load - Display Weather White Icons
                cb_SettingsDisplayWeatherWhiteIcons.IsChecked = (bool)vApplicationSettings["DisplayWeatherWhiteIcons"];

                //Load - Sleep Weather
                cb_SettingsSleepWeather.IsChecked = (bool)vApplicationSettings["SleepWeather"];

                //Load - Sleep Date
                cb_SettingsSleepDate.IsChecked = (bool)vApplicationSettings["SleepDate"];

                //Load - Sleep Analog Clock
                cb_SettingsSleepAnalogClock.IsChecked = (bool)vApplicationSettings["SleepAnalogClock"];

                //Load - Sleep Battery Level
                cb_SettingsSleepBattery.IsChecked = (bool)vApplicationSettings["SleepBattery"];

                //Load - Display Alarm Icon
                cb_SettingsDisplayAlarm.IsChecked = (bool)vApplicationSettings["DisplayAlarm"];

                //Load - Display Live Tile Font Duo Color
                cb_SettingLiveTileFontDuoColor.IsChecked = (bool)vApplicationSettings["LiveTileFontDuoColor"];

                //Load - Cutout Live Tile Time Text
                cb_SettingLiveTileTimeCutOut.IsChecked = (bool)vApplicationSettings["LiveTileTimeCutOut"];

                //Load - Display Time Custom Text
                cb_SettingsDisplayTimeCustomText.IsChecked = (bool)vApplicationSettings["DisplayTimeCustomText"];
                txtbox_SettingsDisplayTimeCustomTextString.Text = vApplicationSettings["DisplayTimeCustomTextString"].ToString();
                if ((bool)vApplicationSettings["DisplayTimeCustomText"])
                {
                    tab_SettingsDisplayTimeCustomTextString.Visibility = Visibility.Visible;
                    txtbox_SettingsDisplayTimeCustomTextString.IsEnabled = true;
                }

                //Load - Lockscreen Note Text String
                cb_SettingsLockscreenNoteText.IsChecked = (bool)vApplicationSettings["LockscreenNoteText"];
                txtbox_SettingsLockscreenNoteTextString.Text = vApplicationSettings["LockscreenNoteTextString"].ToString();
                if ((bool)vApplicationSettings["LockscreenNoteText"])
                {
                    tab_SettingsLockscreenNoteTextString.Visibility = Visibility.Visible;
                    txtbox_SettingsLockscreenNoteTextString.IsEnabled = true;
                }

                //Load - Display Weather High and Low Temp
                cb_SettingsDisplayWeatherTempHighLow.IsChecked = (bool)vApplicationSettings["DisplayWeatherTempHighLow"];

                //Load - Display Weather Tile Location
                cb_SettingsDisplayWeatherTileLocation.IsChecked = (bool)vApplicationSettings["DisplayWeatherTileLocation"];

                //Load - Display Weather Tile Provider
                cb_SettingsDisplayWeatherTileProvider.IsChecked = (bool)vApplicationSettings["DisplayWeatherTileProvider"];

                //Load - Display Weather Tile UpdateTime
                cb_SettingsDisplayWeatherTileUpdateTime.IsChecked = (bool)vApplicationSettings["DisplayWeatherTileUpdateTime"];

                //Load - Sleep Alarm Icon
                cb_SettingsSleepAlarm.IsChecked = (bool)vApplicationSettings["SleepAlarm"];

                //Load - Background Color Preview
                if (vApplicationSettings["LiveTileColorBackground"].ToString() != ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString()) { MenuSet_BackgroundColorPreview.Foreground = new SolidColorBrush(Color.FromArgb(Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(1, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(3, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(5, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(7, 2), 16))); }
                else { MenuSet_BackgroundColorPreview.Foreground = new SolidColorBrush(Colors.White); }

                //Load - Display Background Color Live
                cb_SettingsDisplayBackgroundColor.IsChecked = (bool)vApplicationSettings["DisplayBackgroundColor"];

                //Load - Display Background Color Weather
                cb_SettingsDisplayBackgroundColorWeather.IsChecked = (bool)vApplicationSettings["DisplayBackgroundColorWeather"];

                //Load - Display Background Color Battery
                cb_SettingsDisplayBackgroundColorBattery.IsChecked = (bool)vApplicationSettings["DisplayBackgroundColorBattery"];

                //Load - Display Background Photo Live
                cb_SettingsDisplayBackgroundPhoto.IsChecked = (bool)vApplicationSettings["DisplayBackgroundPhoto"];

                //Load - Display Background Photo Weather
                cb_SettingsDisplayBackgroundPhotoWeather.IsChecked = (bool)vApplicationSettings["DisplayBackgroundPhotoWeather"];

                //Load - Display Background Photo Battery
                cb_SettingsDisplayBackgroundPhotoBattery.IsChecked = (bool)vApplicationSettings["DisplayBackgroundPhotoBattery"];

                //Load - Display Background Brightness
                sldr_SettingsDisplayBackgroundBrightness.Value = Convert.ToDouble(vApplicationSettings["DisplayBackgroundBrightness"]);

                //Load - Download Bing Wallpaper
                cb_SettingsDownloadBingWallpaper.IsChecked = (bool)vApplicationSettings["DownloadBingWallpaper"];
                if (!(bool)vApplicationSettings["DownloadBingWallpaper"])
                {
                    cb_SettingsNotiBingDescription.IsEnabled = false;
                }

                //Load - Weather Download Enabled or Disabled
                cb_SettingsDownloadWeather.IsChecked = (bool)vApplicationSettings["DownloadWeather"];
                if (!(bool)vApplicationSettings["DownloadWeather"])
                {
                    cb_SettingsSleepWeather.IsEnabled = false;
                    cb_SettingsLockLocation.IsEnabled = false;
                    cb_SettingsLockWeather.IsEnabled = false;
                    cb_SettingsLockWeatherDetailed.IsEnabled = false;
                    cb_SettingsNotiWeatherCurrent.IsEnabled = false;
                    cb_SettingsWeatherGpsLocation.IsEnabled = false;
                    cb_SettingsWeatherDisplayWallpaper.IsEnabled = false;
                    txtbox_SettingsWeatherNonGpsLocation.IsEnabled = false;
                }

                //Load - Use GPS for Weather Location
                cb_SettingsWeatherGpsLocation.IsChecked = (bool)vApplicationSettings["WeatherGpsLocation"];
                if (!(bool)vApplicationSettings["WeatherGpsLocation"])
                {
                    tab_SettingsWeatherNonGpsLocation.Visibility = Visibility.Visible;
                    txtbox_SettingsWeatherNonGpsLocation.IsEnabled = true;
                }

                //Load - Display Weather Background
                cb_SettingsWeatherDisplayWallpaper.IsChecked = (bool)vApplicationSettings["WeatherDisplayWallpaper"];

                //Load - Non GPS Weather Location
                txtbox_SettingsWeatherNonGpsLocation.Text = vApplicationSettings["WeatherNonGpsLocation"].ToString();

                //Load - Weather Fahrenheit or Celsius
                lp_SettingFahrenheitCelsius.SelectedIndex = (int)vApplicationSettings["FahrenheitCelsius"];

                //Load - Lockscreen Background 
                cb_SettingsLockWallpaper.IsChecked = (bool)vApplicationSettings["LockWallpaper"];

                //Load - Device Background
                cb_SettingsDeviceWallpaper.IsChecked = (bool)vApplicationSettings["DeviceWallpaper"];

                //Load - Lockscreen Calendar
                cb_SettingsLockCalendar.IsChecked = (bool)vApplicationSettings["LockCalendar"];

                //Load - Lockscreen Weather
                cb_SettingsLockWeather.IsChecked = (bool)vApplicationSettings["LockWeather"];

                //Load - Lockscreen Weather Detailed
                cb_SettingsLockWeatherDetailed.IsChecked = (bool)vApplicationSettings["LockWeatherDetailed"];

                //Load - Lockscreen Location
                cb_SettingsLockLocation.IsChecked = (bool)vApplicationSettings["LockLocation"];

                //Load - Lockscreen Battery Level
                cb_SettingsLockBattery.IsChecked = (bool)vApplicationSettings["LockBattery"];

                //Load - Lockscreen Battery Level Detailed
                cb_SettingsLockBatteryDetailed.IsChecked = (bool)vApplicationSettings["LockBatteryDetailed"];

                //Load - Lockscreen Enter
                cb_SettingsLockEnter.IsChecked = (bool)vApplicationSettings["LockEnter"];

                //Load - Lockscreen Week Number
                cb_SettingsLockWeekNumber.IsChecked = (bool)vApplicationSettings["LockWeekNumber"];

                //Load - Lockscreen Alarm
                cb_SettingsLockAlarm.IsChecked = (bool)vApplicationSettings["LockAlarm"];

                //Load - Lockscreen Network
                cb_SettingsLockNetwork.IsChecked = (bool)vApplicationSettings["LockNetwork"];

                //Load - Lockscreen Countdown
                cb_SettingsLockCountdown.IsChecked = (bool)vApplicationSettings["LockCountdown"];

                //Load - Display current weather as notification
                cb_SettingsNotiWeatherCurrent.IsChecked = (bool)vApplicationSettings["NotiWeatherCurrent"];

                //Load - Display bing description as notification
                cb_SettingsNotiBingDescription.IsChecked = (bool)vApplicationSettings["NotiBingDescription"];

                //Load - Show battery time as notification
                cb_SettingsNotiBattery.IsChecked = (bool)vApplicationSettings["NotiBattery"];

                //Load - Show low battery warning as notification
                cb_SettingsNotiLowBattery.IsChecked = (bool)vApplicationSettings["NotiLowBattery"];

                //Load - Show next calendar time as notification
                cb_SettingsNotiCalendarTime.IsChecked = (bool)vApplicationSettings["NotiCalendarTime"];

                //Load - Show countdown event days as notification
                cb_SettingsNotiCountdownTime.IsChecked = (bool)vApplicationSettings["NotiCountdownTime"];

                //Load - Show remaining day time as notification
                cb_SettingsNotiDayTime.IsChecked = (bool)vApplicationSettings["NotiDayTime"];

                //Load - Show week number as notification
                cb_SettingsNotiWeekNumber.IsChecked = (bool)vApplicationSettings["NotiWeekNumber"];

                //Load - Show network change as notification
                cb_SettingsNotiNetworkChange.IsChecked = (bool)vApplicationSettings["NotiNetworkChange"];

                //Load - Show battery saver as notification
                cb_SettingsNotiBatterySaver.IsChecked = (bool)vApplicationSettings["NotiBatterySaver"];

                //Load - Notifications Style
                cb_SettingsNotiStyle.SelectedIndex = (int)vApplicationSettings["NotiStyle"];

                //Load - Sleepscreen Double tap to Start
                cb_SettingsScreenDoubleTapStart.IsChecked = (bool)vApplicationSettings["ScreenDoubleTapStart"];

                //Load - Sleepscreen display background
                cb_SettingsScreenWallpaper.IsChecked = (bool)vApplicationSettings["ScreenWallpaper"];

                //Load - Flashlight camera output
                cb_SettingsFlashCameraOutput.IsChecked = (bool)vApplicationSettings["FlashCameraOutput"];

                //Load - Sleepscreen display orientation
                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    lp_SettingScreenOrientation.Items.Add("Horizontal");
                    lp_SettingScreenOrientation.Items.Add("Vertical");
                }
                else
                {
                    lp_SettingScreenOrientation.Items.Add("Vertical");
                    lp_SettingScreenOrientation.Items.Add("Horizontal");
                }
                lp_SettingScreenOrientation.SelectedIndex = (int)vApplicationSettings["ScreenOrientation"];

                //Load - Sleepscreen analog clocks
                ComboBoxImageList SleepAnalog_Round = new ComboBoxImageList
                {
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Analog/Round/300.png", UriKind.Absolute)),
                    Title = "Round"
                };
                lp_SettingSleepAnalogClockStyle.Items.Add(SleepAnalog_Round);
                ComboBoxImageList SleepAnalog_Cortana = new ComboBoxImageList
                {
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Analog/Cortana/300.png", UriKind.Absolute)),
                    Title = "Cortana"
                };
                lp_SettingSleepAnalogClockStyle.Items.Add(SleepAnalog_Cortana);
                ComboBoxImageList SleepAnalog_Minimal = new ComboBoxImageList
                {
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/Analog/Minimal/300.png", UriKind.Absolute)),
                    Title = "Minimal"
                };
                lp_SettingSleepAnalogClockStyle.Items.Add(SleepAnalog_Minimal);
                lp_SettingSleepAnalogClockStyle.SelectedIndex = (int)vApplicationSettings["SleepAnalogClockStyle"];

                //Load - Background Download Enabled or Disabled
                cb_SettingsBackgroundDownload.IsChecked = (bool)vApplicationSettings["BackgroundDownload"];
                if (!(bool)vApplicationSettings["BackgroundDownload"])
                {
                    cb_SettingsDownloadWeather.IsEnabled = false;
                    cb_SettingsSleepWeather.IsEnabled = false;
                    cb_SettingsLockLocation.IsEnabled = false;
                    cb_SettingsLockWeather.IsEnabled = false;
                    cb_SettingsLockWeatherDetailed.IsEnabled = false;
                    cb_SettingsNotiWeatherCurrent.IsEnabled = false;
                    cb_SettingsNotiBingDescription.IsEnabled = false;
                    cb_SettingsWeatherGpsLocation.IsEnabled = false;
                    cb_SettingsWeatherDisplayWallpaper.IsEnabled = false;
                    txtbox_SettingsWeatherNonGpsLocation.IsEnabled = false;
                    cb_SettingsDownloadBingWallpaper.IsEnabled = false;
                    cb_SettingsDownloadWifiOnly.IsEnabled = false;
                }

                //Load - Background Download Wi-Fi only
                cb_SettingsDownloadWifiOnly.IsChecked = (bool)vApplicationSettings["DownloadWifiOnly"];

                //Load - Background Download Update Interval
                lp_SettingBackgroundDownloadInterval.SelectedIndex = (int)vApplicationSettings["BackgroundDownloadInterval"];

                //Load - Background Download Bing Region
                lp_SettingDownloadBingRegion.Items[0] = "Keyboard Region (" + vCultureInfoReg.TwoLetterISOLanguageName.ToUpper() + ")";
                lp_SettingDownloadBingRegion.SelectedIndex = (int)vApplicationSettings["DownloadBingRegion"];

                //Load - Background Download Bing Resolution
                lp_SettingDownloadBingResolution.SelectedIndex = (int)vApplicationSettings["DownloadBingResolution"];

                //Load - Font Display Weight
                lp_SettingLiveTileFontWeight.SelectedIndex = (int)vApplicationSettings["LiveTileFontWeight"];

                //Load - Other Startup Tab
                lp_SettingStartupTab.SelectedIndex = (int)vApplicationSettings["StartupTab"];
                if ((int)vApplicationSettings["StartupTab"] == 6 || (int)vApplicationSettings["StartupTab"] == 7)
                {
                    vApplicationSettings["ScreenDoubleTapStart"] = false;
                    cb_SettingsScreenDoubleTapStart.IsEnabled = false;
                    cb_SettingsScreenDoubleTapStart.IsChecked = false;
                }

                //Load - Application debugging
                cb_SettingsAppDebug.IsChecked = (bool)vApplicationSettings["AppDebug"];

                //Load - Screens Disable Back Button
                cb_SettingsScreenBackButton.IsChecked = (bool)vApplicationSettings["ScreenBackButton"];
            }
            catch (Exception Ex) { await new MessageDialog("SettingsLoadError: " + Ex.Message, "TimeMe").ShowAsync(); }
        }
    }
}