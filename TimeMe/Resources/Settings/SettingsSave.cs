using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Storage;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimeMe
{
    partial class MainPage
    {
        //Save Events - Application Settings
        async Task SettingsSave()
        {
            try
            {
                //Save - Display Horizontal Alignment Time
                cb_SettingsDisplayHorizontalAlignmentTime.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DisplayHorizontalAlignmentTime"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["DisplayHorizontalAlignmentTime"] = ComboBox.SelectedIndex; }

                    //Disable the AM/PM display setting
                    if (ComboBox.SelectedIndex != 0)
                    {
                        vApplicationSettings["DisplayAMPMClock"] = false;
                        cb_SettingsDisplayAMPMClock.IsChecked = false;
                        cb_SettingsDisplayAMPMClock.IsEnabled = false;
                    }
                    else
                    {
                        cb_SettingsDisplayAMPMClock.IsEnabled = true;
                    }
                };

                //Save - Display Text Position 1
                cb_SettingsDisplayPosition1.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DisplayPosition1"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["DisplayPosition1"] = ComboBox.SelectedIndex; }
                };
                //Save - Display Text Position 2
                cb_SettingsDisplayPosition2.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DisplayPosition2"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["DisplayPosition2"] = ComboBox.SelectedIndex; }
                };
                //Save - Display Text Position 3
                cb_SettingsDisplayPosition3.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DisplayPosition3"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["DisplayPosition3"] = ComboBox.SelectedIndex; }
                };
                //Save - Display Text Position 4
                cb_SettingsDisplayPosition4.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DisplayPosition4"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["DisplayPosition4"] = ComboBox.SelectedIndex; }
                };

                //Save - Is option 'Show More Tiles' enabled?
                cb_SettingShowMoreTiles.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["ShowMoreTiles"] = true; }
                    else { vApplicationSettings["ShowMoreTiles"] = false; }
                };

                //Save - Display keyboard/region language text
                cb_SettingsDisplayRegionLanguage.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayRegionLanguage"] = true;

                        if (new Windows.Globalization.GeographicRegion().CodeTwoLetter.ToLower() != vCultureInfoReg.TwoLetterISOLanguageName.ToLower())
                        { await new MessageDialog("Your keyboard language and device's region don't seem to match and may cause language differences with the Weather and Bing background download.\n\nTo make sure the Weather and Bing background is using your region set your currently used keyboard language to your region.\n\nDetected keyboard language: " + vCultureInfoReg.TwoLetterISOLanguageName.ToUpper() + "\nDetected region language: " + new Windows.Globalization.GeographicRegion().CodeTwoLetter.ToUpper(), "TimeMe").ShowAsync(); }
                    }
                    else { vApplicationSettings["DisplayRegionLanguage"] = false; }
                    BackgroundStatusUpdateSettings("Never", "Never", null, null, "Never");
                    await WeatherLoad();
                };

                //Save - Display 24-hour Clock
                cb_SettingsDisplay24hClock.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["Display24hClock"] = true;
                        vApplicationSettings["DisplayAMPMClock"] = false;
                        cb_SettingsDisplayAMPMClock.IsChecked = false;
                    }
                    else
                    {
                        vApplicationSettings["Display24hClock"] = false;
                        if ((int)vApplicationSettings["DisplayHorizontalAlignmentTime"] == 0)
                        {
                            vApplicationSettings["DisplayAMPMClock"] = true;
                            cb_SettingsDisplayAMPMClock.IsChecked = true;
                        }
                    }
                };

                //Save - Display AM or PM Clock
                cb_SettingsDisplayAMPMClock.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayAMPMClock"] = true; }
                    else { vApplicationSettings["DisplayAMPMClock"] = false; }
                };

                //Save - Display the hour bold
                cb_SettingsDisplayHourBold.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayHourBold"] = true; }
                    else { vApplicationSettings["DisplayHourBold"] = false; }
                };

                //Save - Display AM or PM Font
                cb_SettingsDisplayAMPMFont.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayAMPMFont"] = true; }
                    else { vApplicationSettings["DisplayAMPMFont"] = false; }
                };

                //Save - Display Current Time
                cb_SettingsDisplayCurrentTime.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayCurrentTime"] = true; }
                    else { vApplicationSettings["DisplayCurrentTime"] = false; }
                };

                //Save - Display Time Splitter
                cb_SettingsDisplayTimeSplitter.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DisplayTimeSplitter"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["DisplayTimeSplitter"] = ComboBox.SelectedIndex; }
                };

                //Save - Display Date Year
                cb_SettingsDisplayDateYear.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayDateYear"] = true; }
                    else { vApplicationSettings["DisplayDateYear"] = false; }
                };

                //Save - Display Date Week number
                cb_SettingsDisplayDateWeekNumber.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayDateWeekNumber"] = true; }
                    else { vApplicationSettings["DisplayDateWeekNumber"] = false; }
                };

                //Save - Display Weather White Icons
                cb_SettingsDisplayWeatherWhiteIcons.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayWeatherWhiteIcons"] = true; }
                    else { vApplicationSettings["DisplayWeatherWhiteIcons"] = false; }
                };

                //Save - Sleep Weather
                cb_SettingsSleepWeather.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["SleepWeather"] = true; }
                    else { vApplicationSettings["SleepWeather"] = false; }
                };

                //Save - Sleep Date
                cb_SettingsSleepDate.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["SleepDate"] = true; }
                    else { vApplicationSettings["SleepDate"] = false; }
                };

                //Save - Sleep Analog Clock
                cb_SettingsSleepAnalogClock.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["SleepAnalogClock"] = true; }
                    else { vApplicationSettings["SleepAnalogClock"] = false; }
                };

                //Save - Sleep Battery Level
                cb_SettingsSleepBattery.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["SleepBattery"] = true; }
                    else { vApplicationSettings["SleepBattery"] = false; }
                };

                //Save - Display Alarm Icon
                cb_SettingsDisplayAlarm.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayAlarm"] = true; }
                    else { vApplicationSettings["DisplayAlarm"] = false; }
                };

                //Save - Increase the live tile font size
                lp_SettingLiveTileFontSize.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if (((int)vApplicationSettings["LiveTileFontSize"] + 5) != ComboBox.SelectedIndex) { vApplicationSettings["LiveTileFontSize"] = ComboBox.SelectedIndex - 5; }
                };

                //Save - Display Live Tile Font Duo Color
                cb_SettingLiveTileFontDuoColor.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LiveTileFontDuoColor"] = true; }
                    else { vApplicationSettings["LiveTileFontDuoColor"] = false; }
                };

                //Save - Cutout Live Tile Time Text
                cb_SettingLiveTileTimeCutOut.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LiveTileTimeCutOut"] = true; }
                    else { vApplicationSettings["LiveTileTimeCutOut"] = false; }
                };

                //Save - Display Time Custom Text
                cb_SettingsDisplayTimeCustomText.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayTimeCustomText"] = true;
                        tab_SettingsDisplayTimeCustomTextString.Visibility = Visibility.Visible;
                        txtbox_SettingsDisplayTimeCustomTextString.IsEnabled = true;
                        txtbox_SettingsDisplayTimeCustomTextString.Focus(FocusState.Programmatic);
                    }
                    else
                    {
                        vApplicationSettings["DisplayTimeCustomText"] = false;
                        tab_SettingsDisplayTimeCustomTextString.Visibility = Visibility.Collapsed;
                        txtbox_SettingsDisplayTimeCustomTextString.IsEnabled = false;
                    }
                };

                //Save - Display Time Custom Text String
                txtbox_SettingsDisplayTimeCustomTextString.TextChanged += (sender, e) =>
                {
                    TextBox TextBox = (TextBox)sender;
                    if (String.IsNullOrWhiteSpace(TextBox.Text)) { return; }
                    vApplicationSettings["DisplayTimeCustomTextString"] = TextBox.Text;
                };

                //Save - Lockscreen Note Text
                cb_SettingsLockscreenNoteText.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["LockscreenNoteText"] = true;
                        tab_SettingsLockscreenNoteTextString.Visibility = Visibility.Visible;
                        txtbox_SettingsLockscreenNoteTextString.IsEnabled = true;
                        txtbox_SettingsLockscreenNoteTextString.Focus(FocusState.Programmatic);

                        //Get the Windows user full name and set if empty
                        try
                        {
                            if (String.IsNullOrEmpty(vApplicationSettings["LockscreenNoteTextString"].ToString()))
                            {
                                IReadOnlyList<User> AllUsers = await User.FindAllAsync(UserType.LocalUser);
                                string FullName = await AllUsers.FirstOrDefault().GetPropertyAsync(KnownUserProperties.FirstName) + " " + await AllUsers.FirstOrDefault().GetPropertyAsync(KnownUserProperties.LastName);
                                if (!String.IsNullOrEmpty(FullName) && !String.IsNullOrWhiteSpace(FullName))
                                {
                                    vApplicationSettings["LockscreenNoteTextString"] = FullName;
                                    txtbox_SettingsLockscreenNoteTextString.Text = FullName;
                                }
                            }
                        }
                        catch { }

                        txtbox_SettingsLockscreenNoteTextString.Focus(FocusState.Programmatic);
                        txtbox_SettingsLockscreenNoteTextString.SelectionStart = txtbox_SettingsLockscreenNoteTextString.Text.Length;
                    }
                    else
                    {
                        vApplicationSettings["LockscreenNoteText"] = false;
                        tab_SettingsLockscreenNoteTextString.Visibility = Visibility.Collapsed;
                        txtbox_SettingsLockscreenNoteTextString.IsEnabled = false;
                    }
                };

                //Save - Lockscreen Note Text String
                txtbox_SettingsLockscreenNoteTextString.TextChanged += (sender, e) =>
                {
                    TextBox TextBox = (TextBox)sender;
                    if (String.IsNullOrWhiteSpace(TextBox.Text)) { return; }
                    vApplicationSettings["LockscreenNoteTextString"] = TextBox.Text;
                };

                //Save - Display Weather High and Low Temp
                cb_SettingsDisplayWeatherTempHighLow.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayWeatherTempHighLow"] = true; }
                    else { vApplicationSettings["DisplayWeatherTempHighLow"] = false; }
                };

                //Save - Display Weather Tile Location
                cb_SettingsDisplayWeatherTileLocation.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayWeatherTileLocation"] = true;
                        vApplicationSettings["DisplayWeatherTileProvider"] = false;
                        cb_SettingsDisplayWeatherTileProvider.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayWeatherTileLocation"] = false; }
                };

                //Save - Display Weather Tile Provider
                cb_SettingsDisplayWeatherTileProvider.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayWeatherTileProvider"] = true;
                        vApplicationSettings["DisplayWeatherTileLocation"] = false;
                        cb_SettingsDisplayWeatherTileLocation.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayWeatherTileProvider"] = false; }
                };

                //Save - Display Weather Tile UpdateTime
                cb_SettingsDisplayWeatherTileUpdateTime.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DisplayWeatherTileUpdateTime"] = true; }
                    else { vApplicationSettings["DisplayWeatherTileUpdateTime"] = false; }
                };

                //Save - Sleep Alarm Icon
                cb_SettingsSleepAlarm.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["SleepAlarm"] = true; }
                    else { vApplicationSettings["SleepAlarm"] = false; }
                };

                //Save - Display Background Brightness
                sldr_SettingsDisplayBackgroundBrightness.ValueChanged += (sender, e) =>
                {
                    Slider Slider = (Slider)sender;
                    vApplicationSettings["DisplayBackgroundBrightness"] = Slider.Value;
                };

                //Save - Display Background Color Live
                cb_SettingsDisplayBackgroundColor.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayBackgroundColor"] = true;
                        vApplicationSettings["DisplayBackgroundPhoto"] = false;
                        cb_SettingsDisplayBackgroundPhoto.IsChecked = false;
                    }
                    else
                    {
                        //Check if current live tile font color is accent color
                        if (vApplicationSettings["LiveTileColorFont"].ToString() == ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString())
                        {
                            await new MessageDialog("Your currently selected live tile font color is the same as your accent color, please select a different live tile font color first.", "TimeMe").ShowAsync();
                            cb_SettingsDisplayBackgroundColor.IsChecked = true;
                            return;
                        }

                        vApplicationSettings["DisplayBackgroundColor"] = false;
                    }
                };

                //Save - Display Background Color Weather
                cb_SettingsDisplayBackgroundColorWeather.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayBackgroundColorWeather"] = true;
                        vApplicationSettings["DisplayBackgroundPhotoWeather"] = false;
                        cb_SettingsDisplayBackgroundPhotoWeather.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayBackgroundColorWeather"] = false; }
                };

                //Save - Display Background Color Battery
                cb_SettingsDisplayBackgroundColorBattery.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayBackgroundColorBattery"] = true;
                        vApplicationSettings["DisplayBackgroundPhotoBattery"] = false;
                        cb_SettingsDisplayBackgroundPhotoBattery.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayBackgroundColorBattery"] = false; }
                };

                //Save - Display Background Photo Live
                cb_SettingsDisplayBackgroundPhoto.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayBackgroundPhoto"] = true;
                        vApplicationSettings["DisplayBackgroundColor"] = false;
                        cb_SettingsDisplayBackgroundColor.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayBackgroundPhoto"] = false; }
                };

                //Save - Display Background Photo Weather
                cb_SettingsDisplayBackgroundPhotoWeather.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayBackgroundPhotoWeather"] = true;
                        vApplicationSettings["DisplayBackgroundColorWeather"] = false;
                        cb_SettingsDisplayBackgroundColorWeather.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayBackgroundPhotoWeather"] = false; }
                };

                //Save - Display Background Photo Battery
                cb_SettingsDisplayBackgroundPhotoBattery.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DisplayBackgroundPhotoBattery"] = true;
                        vApplicationSettings["DisplayBackgroundColorBattery"] = false;
                        cb_SettingsDisplayBackgroundColorBattery.IsChecked = false;
                    }
                    else { vApplicationSettings["DisplayBackgroundPhotoBattery"] = false; }
                };

                //Save - Download Bing Wallpaper
                cb_SettingsDownloadBingWallpaper.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DownloadBingWallpaper"] = true;
                        cb_SettingsNotiBingDescription.IsEnabled = true;
                    }
                    else
                    {
                        vApplicationSettings["NotiBingDescription"] = false;
                        cb_SettingsNotiBingDescription.IsEnabled = false;
                        vApplicationSettings["DisplayBackgroundPhoto"] = false;
                        cb_SettingsDisplayBackgroundPhoto.IsChecked = false;
                        vApplicationSettings["DisplayBackgroundPhotoWeather"] = false;
                        cb_SettingsDisplayBackgroundPhotoWeather.IsChecked = false;
                        vApplicationSettings["DisplayBackgroundPhotoBattery"] = false;
                        cb_SettingsDisplayBackgroundPhotoBattery.IsChecked = false;
                        vApplicationSettings["LockWallpaper"] = false;
                        cb_SettingsLockWallpaper.IsChecked = false;
                        vApplicationSettings["DeviceWallpaper"] = false;
                        cb_SettingsDeviceWallpaper.IsChecked = false;
                        vApplicationSettings["ScreenWallpaper"] = false;
                        cb_SettingsScreenWallpaper.IsChecked = false;
                        vApplicationSettings["DownloadBingWallpaper"] = false;
                        //Delete TimeMeTilePhoto if exists
                        if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                        {
                            IStorageItem IStorageItem = await ApplicationData.Current.LocalFolder.GetItemAsync("TimeMeTilePhoto.png");
                            await IStorageItem.DeleteAsync(StorageDeleteOption.PermanentDelete);
                        }
                    }

                    BackgroundStatusUpdateSettings(null, null, "Never", "Never", "Never");
                    await CurrentBackgroundLoad();
                    await CurrentTileLoad();
                    await WeatherLoad();
                };

                //Save - Weather Download Enabled or Disabled
                cb_SettingsDownloadWeather.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DownloadWeather"] = true;
                        cb_SettingsSleepWeather.IsEnabled = true;
                        cb_SettingsLockLocation.IsEnabled = true;
                        cb_SettingsLockWeather.IsEnabled = true;
                        cb_SettingsLockWeatherDetailed.IsEnabled = true;
                        cb_SettingsNotiWeatherCurrent.IsEnabled = true;
                        cb_SettingsWeatherGpsLocation.IsEnabled = true;
                        cb_SettingsWeatherDisplayWallpaper.IsEnabled = true;
                        txtbox_SettingsWeatherNonGpsLocation.IsEnabled = true;

                        //Check if the application has location permission
                        await CheckLocationPermission();
                    }
                    else
                    {
                        //Disable download weather same as MainPage.cs
                        vApplicationSettings["DownloadWeather"] = false;
                        cb_SettingsDownloadWeather.IsChecked = false;
                        cb_SettingsSleepWeather.IsEnabled = false;
                        cb_SettingsLockLocation.IsEnabled = false;
                        cb_SettingsLockWeather.IsEnabled = false;
                        cb_SettingsLockWeatherDetailed.IsEnabled = false;
                        cb_SettingsNotiWeatherCurrent.IsEnabled = false;
                        cb_SettingsWeatherGpsLocation.IsEnabled = false;
                        cb_SettingsWeatherDisplayWallpaper.IsEnabled = false;
                        txtbox_SettingsWeatherNonGpsLocation.IsEnabled = false;
                    }
                    BackgroundStatusUpdateSettings("Never", "Never", null, null, "Never");
                    await WeatherLoad();
                };

                //Save - Use GPS for Weather Location
                cb_SettingsWeatherGpsLocation.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["WeatherGpsLocation"] = true;
                        tab_SettingsWeatherNonGpsLocation.Visibility = Visibility.Collapsed;
                        txtbox_SettingsWeatherNonGpsLocation.IsEnabled = false;

                        //Check if the application has location permission
                        await CheckLocationPermission();
                    }
                    else
                    {
                        vApplicationSettings["WeatherGpsLocation"] = false;
                        tab_SettingsWeatherNonGpsLocation.Visibility = Visibility.Visible;
                        txtbox_SettingsWeatherNonGpsLocation.IsEnabled = true;
                        txtbox_SettingsWeatherNonGpsLocation.Focus(FocusState.Programmatic);
                    }
                    BackgroundStatusUpdateSettings("Never", "Never", null, null, "Never");
                    await WeatherLoad();
                };

                //Save - Display Weather Background
                cb_SettingsWeatherDisplayWallpaper.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["WeatherDisplayWallpaper"] = true;
                        await WeatherLoad();
                    }
                    else
                    {
                        vApplicationSettings["WeatherDisplayWallpaper"] = false;
                        await WeatherLoad();
                    }
                };

                //Save - Non GPS Weather Location
                txtbox_SettingsWeatherNonGpsLocation.TextChanged += (sender, e) =>
                {
                    TextBox TextBox = (TextBox)sender;
                    if (String.IsNullOrWhiteSpace(TextBox.Text)) { return; }
                    vApplicationSettings["WeatherNonGpsLocation"] = TextBox.Text;
                };

                //Save - Weather Fahrenheit or Celsius
                lp_SettingFahrenheitCelsius.SelectionChanged += async (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["FahrenheitCelsius"] != ComboBox.SelectedIndex)
                    {
                        vApplicationSettings["FahrenheitCelsius"] = ComboBox.SelectedIndex;
                        BackgroundStatusUpdateSettings("Never", "Never", null, null, "Never");
                        await WeatherLoad();
                    }
                };

                //Save - Lockscreen Background 
                cb_SettingsLockWallpaper.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["LockWallpaper"] = true;

                        if (!(bool)vApplicationSettings["DeviceWallpaper"])
                        {
                            Nullable<bool> MessageDialogResultGps = null;
                            MessageDialog MessageDialogGps = new MessageDialog("Do you also want to display your background photo as your device's start screen or desktop wallpaper?", "TimeMe");
                            MessageDialogGps.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => MessageDialogResultGps = true)));
                            MessageDialogGps.Commands.Add(new UICommand("No", new UICommandInvokedHandler((cmd) => MessageDialogResultGps = false)));
                            await MessageDialogGps.ShowAsync();
                            if (MessageDialogResultGps == true)
                            {
                                vApplicationSettings["DeviceWallpaper"] = true;
                                cb_SettingsDeviceWallpaper.IsChecked = true;
                            }
                            else
                            {
                                vApplicationSettings["DeviceWallpaper"] = false;
                                cb_SettingsDeviceWallpaper.IsChecked = false;
                            }
                        }

                        await UpdateDeviceWallpaper();
                    }
                    else { vApplicationSettings["LockWallpaper"] = false; }
                };

                //Save - Device Background 
                cb_SettingsDeviceWallpaper.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["DeviceWallpaper"] = true;
                        if (!(bool)vApplicationSettings["LockWallpaper"])
                        {
                            Nullable<bool> MessageDialogResultGps = null;
                            MessageDialog MessageDialogGps = new MessageDialog("Do you also want to display your background photo on your device's lock screen as wallpaper?", "TimeMe");
                            MessageDialogGps.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => MessageDialogResultGps = true)));
                            MessageDialogGps.Commands.Add(new UICommand("No", new UICommandInvokedHandler((cmd) => MessageDialogResultGps = false)));
                            await MessageDialogGps.ShowAsync();
                            if (MessageDialogResultGps == true)
                            {
                                vApplicationSettings["LockWallpaper"] = true;
                                cb_SettingsLockWallpaper.IsChecked = true;
                            }
                            else
                            {
                                vApplicationSettings["LockWallpaper"] = false;
                                cb_SettingsLockWallpaper.IsChecked = false;
                            }
                        }

                        await UpdateDeviceWallpaper();
                    }
                    else { vApplicationSettings["DeviceWallpaper"] = false; }
                };

                //Save - Lockscreen Calendar
                cb_SettingsLockCalendar.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        //Check for Calendar permission
                        try
                        {
                            AppointmentStore AppointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
                            IReadOnlyList<Appointment> Appointments = await AppointmentStore.FindAppointmentsAsync(DateTime.Now, TimeSpan.FromMinutes(1));
                            vApplicationSettings["LockCalendar"] = true;
                        }
                        catch
                        {
                            vApplicationSettings["LockCalendar"] = false;
                            CheckBox.IsChecked = false;

                            Nullable<bool> MessageDialogResult = null;
                            MessageDialog MessageDialog = new MessageDialog("You don't seem to have calendar permission, please click on 'Enable' turn the 'Calendar' option on and scroll to TimeMe and allow it to enable calendar information on your lock screen.", "TimeMe");
                            MessageDialog.Commands.Add(new UICommand("Enable", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                            MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                            await MessageDialog.ShowAsync();
                            if (MessageDialogResult == true) { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-calendar")); }
                        }
                    }
                    else { vApplicationSettings["LockCalendar"] = false; }
                };

                //Save - Lockscreen Weather
                cb_SettingsLockWeather.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockWeather"] = true; }
                    else { vApplicationSettings["LockWeather"] = false; }
                };

                //Save - Lockscreen Weather Detailed
                cb_SettingsLockWeatherDetailed.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockWeatherDetailed"] = true; }
                    else { vApplicationSettings["LockWeatherDetailed"] = false; }
                };

                //Save - Lockscreen Location
                cb_SettingsLockLocation.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockLocation"] = true; }
                    else { vApplicationSettings["LockLocation"] = false; }
                };

                //Save - Lockscreen Battery Level
                cb_SettingsLockBattery.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockBattery"] = true; }
                    else { vApplicationSettings["LockBattery"] = false; }
                };

                //Save - Lockscreen Battery Level Detailed
                cb_SettingsLockBatteryDetailed.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockBatteryDetailed"] = true; }
                    else { vApplicationSettings["LockBatteryDetailed"] = false; }
                };

                //Save - Lockscreen Enter
                cb_SettingsLockEnter.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockEnter"] = true; }
                    else { vApplicationSettings["LockEnter"] = false; }
                };

                //Save - Lockscreen Week Number
                cb_SettingsLockWeekNumber.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockWeekNumber"] = true; }
                    else { vApplicationSettings["LockWeekNumber"] = false; }
                };

                //Save - Lockscreen Alarm
                cb_SettingsLockAlarm.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockAlarm"] = true; }
                    else { vApplicationSettings["LockAlarm"] = false; }
                };

                //Save - Lockscreen Network
                cb_SettingsLockNetwork.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockNetwork"] = true; }
                    else { vApplicationSettings["LockNetwork"] = false; }
                };

                //Save - Lockscreen Countdown
                cb_SettingsLockCountdown.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["LockCountdown"] = true; }
                    else { vApplicationSettings["LockCountdown"] = false; }
                };

                //Save - Display current weather as notification
                cb_SettingsNotiWeatherCurrent.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiWeatherCurrent"] = true; }
                    else
                    {
                        vApplicationSettings["NotiWeatherCurrent"] = false;
                        ToastNotificationManager.History.Remove("T1", "G1");
                    }
                };

                //Save - Display bing description as notification
                cb_SettingsNotiBingDescription.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiBingDescription"] = true; }
                    else
                    {
                        vApplicationSettings["NotiBingDescription"] = false;
                        ToastNotificationManager.History.Remove("T2", "G1");
                    }
                };

                //Save - Show battery time as notification
                cb_SettingsNotiBattery.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["NotiBattery"] = true;
                        vApplicationSettings["NotiLowBattery"] = false;
                        cb_SettingsNotiLowBattery.IsChecked = false;
                    }
                    else
                    {
                        vApplicationSettings["NotiBattery"] = false;
                        ToastNotificationManager.History.Remove("T3", "G1");
                    }
                };

                //Save - Show low battery warning as notification
                cb_SettingsNotiLowBattery.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["NotiLowBattery"] = true;
                        vApplicationSettings["NotiBattery"] = false;
                        cb_SettingsNotiBattery.IsChecked = false;
                    }
                    else
                    {
                        vApplicationSettings["NotiLowBattery"] = false;
                        ToastNotificationManager.History.Remove("T3", "G1");
                    }
                };

                //Save - Show next calendar time as notification
                cb_SettingsNotiCalendarTime.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiCalendarTime"] = true; }
                    else
                    {
                        vApplicationSettings["NotiCalendarTime"] = false;
                        ToastNotificationManager.History.Remove("T4", "G1");
                    }
                };

                //Save - Show countdown event days as notification
                cb_SettingsNotiCountdownTime.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiCountdownTime"] = true; }
                    else
                    {
                        vApplicationSettings["NotiCountdownTime"] = false;
                        ToastNotificationManager.History.Remove("T7", "G1");
                    }
                };

                //Save - Show remaining day time as notification
                cb_SettingsNotiDayTime.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiDayTime"] = true; }
                    else
                    {
                        vApplicationSettings["NotiDayTime"] = false;
                        ToastNotificationManager.History.Remove("T5", "G1");
                    }
                };

                //Save - Show week number as notification
                cb_SettingsNotiWeekNumber.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiWeekNumber"] = true; }
                    else
                    {
                        vApplicationSettings["NotiWeekNumber"] = false;
                        vApplicationSettings["BgStatusLastRunDate"] = "Never";
                        ToastNotificationManager.History.Remove("T6", "G1");
                    }
                };

                //Save - Show network change as notification
                cb_SettingsNotiNetworkChange.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiNetworkChange"] = true; }
                    else
                    {
                        vApplicationSettings["NotiNetworkChange"] = false;
                        ToastNotificationManager.History.Remove("T9", "G1");
                    }

                    vApplicationSettings["BgStatusNetworkName"] = "N/A";
                };

                //Save - Show battery saver as notification
                cb_SettingsNotiBatterySaver.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["NotiBatterySaver"] = true; }
                    else
                    {
                        vApplicationSettings["NotiBatterySaver"] = false;
                        ToastNotificationManager.History.Remove("T8", "G1");
                    }
                };

                //Save - Notifications Style
                cb_SettingsNotiStyle.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["NotiStyle"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["NotiStyle"] = ComboBox.SelectedIndex; }
                };

                //Save - Sleepscreen Double tap to Start
                cb_SettingsScreenDoubleTapStart.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["ScreenDoubleTapStart"] = true; }
                    else { vApplicationSettings["ScreenDoubleTapStart"] = false; }
                };

                //Save - Sleepscreen display background
                cb_SettingsScreenWallpaper.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["ScreenWallpaper"] = true; }
                    else { vApplicationSettings["ScreenWallpaper"] = false; }
                };

                //Save - Flashlight camera output
                cb_SettingsFlashCameraOutput.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["FlashCameraOutput"] = true; }
                    else { vApplicationSettings["FlashCameraOutput"] = false; }
                };

                //Save - Sleepscreen display orientation
                lp_SettingScreenOrientation.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["ScreenOrientation"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["ScreenOrientation"] = ComboBox.SelectedIndex; }
                };

                //Save - Sleepscreen analog clock style
                lp_SettingSleepAnalogClockStyle.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["SleepAnalogClockStyle"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["SleepAnalogClockStyle"] = ComboBox.SelectedIndex; }
                };

                //Save - Background Download Enabled or Disabled
                cb_SettingsBackgroundDownload.Click += async (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked)
                    {
                        vApplicationSettings["BackgroundDownload"] = true;
                        cb_SettingsDownloadBingWallpaper.IsEnabled = true;
                        cb_SettingsDownloadWifiOnly.IsEnabled = true;
                        cb_SettingsDownloadWeather.IsEnabled = true;
                        if ((bool)vApplicationSettings["DownloadBingWallpaper"])
                        {
                            cb_SettingsNotiBingDescription.IsEnabled = true;
                        }
                        if ((bool)vApplicationSettings["DownloadWeather"])
                        {
                            cb_SettingsSleepWeather.IsEnabled = true;
                            cb_SettingsLockLocation.IsEnabled = true;
                            cb_SettingsLockWeather.IsEnabled = true;
                            cb_SettingsLockWeatherDetailed.IsEnabled = true;
                            cb_SettingsNotiWeatherCurrent.IsEnabled = true;
                            cb_SettingsWeatherGpsLocation.IsEnabled = true;
                            cb_SettingsWeatherDisplayWallpaper.IsEnabled = true;
                            txtbox_SettingsWeatherNonGpsLocation.IsEnabled = true;
                        }
                    }
                    else
                    {
                        vApplicationSettings["BackgroundDownload"] = false;
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

                    BackgroundStatusUpdateSettings("Never", "Never", "Never", "Never", "Never");
                    await CurrentTileLoad();
                    await WeatherLoad();
                };

                //Save - Background Download Wi-Fi only
                cb_SettingsDownloadWifiOnly.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["DownloadWifiOnly"] = true; }
                    else { vApplicationSettings["DownloadWifiOnly"] = false; }
                };

                //Save - Background Download Update Interval
                lp_SettingBackgroundDownloadInterval.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["BackgroundDownloadInterval"] != ComboBox.SelectedIndex)
                    {
                        vApplicationSettings["BackgroundDownloadInterval"] = ComboBox.SelectedIndex;
                        if (ComboBox.SelectedIndex == 0) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 28; }
                        else if (ComboBox.SelectedIndex == 1) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 58; }
                        else if (ComboBox.SelectedIndex == 2) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 118; }
                        else if (ComboBox.SelectedIndex == 3) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 238; }
                        else if (ComboBox.SelectedIndex == 4) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 478; }
                        else if (ComboBox.SelectedIndex == 5) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 718; }
                        else if (ComboBox.SelectedIndex == 6) { vApplicationSettings["BackgroundDownloadIntervalMin"] = 1438; }
                    }
                };

                //Save - Background Download Bing Region
                lp_SettingDownloadBingRegion.SelectionChanged += async (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DownloadBingRegion"] != ComboBox.SelectedIndex)
                    {
                        vApplicationSettings["DownloadBingRegion"] = ComboBox.SelectedIndex;

                        if (ComboBox.SelectedIndex == 0)
                        {
                            if (new Windows.Globalization.GeographicRegion().CodeTwoLetter.ToLower() != vCultureInfoReg.TwoLetterISOLanguageName.ToLower())
                            { await new MessageDialog("Your keyboard language and device's region don't seem to match and may cause language differences with the Weather and Bing background download.\n\nTo make sure the Weather and Bing background is using your region set your currently used keyboard language to your region.\n\nDetected keyboard language: " + vCultureInfoReg.TwoLetterISOLanguageName.ToUpper() + "\nDetected region language: " + new Windows.Globalization.GeographicRegion().CodeTwoLetter.ToUpper(), "TimeMe").ShowAsync(); }
                        }

                        BackgroundStatusUpdateSettings(null, null, "Never", "Never", "Never");
                        await CurrentTileLoad();
                        await WeatherLoad();
                    }
                };

                //Save - Background Download Bing Resolution
                lp_SettingDownloadBingResolution.SelectionChanged += async (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["DownloadBingResolution"] != ComboBox.SelectedIndex)
                    {
                        vApplicationSettings["DownloadBingResolution"] = ComboBox.SelectedIndex;
                        BackgroundStatusUpdateSettings(null, null, "Never", "Never", "Never");
                        await CurrentTileLoad();
                        await WeatherLoad();
                    }
                };

                //Save - Font Display Weight
                lp_SettingLiveTileFontWeight.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["LiveTileFontWeight"] != ComboBox.SelectedIndex)
                    { vApplicationSettings["LiveTileFontWeight"] = ComboBox.SelectedIndex; }
                };

                //Save - Other Startup Tab
                lp_SettingStartupTab.SelectionChanged += (sender, e) =>
                {
                    ComboBox ComboBox = (ComboBox)sender;
                    if ((int)vApplicationSettings["StartupTab"] != ComboBox.SelectedIndex)
                    {
                        vApplicationSettings["StartupTab"] = ComboBox.SelectedIndex;
                        if (ComboBox.SelectedIndex == 6 || ComboBox.SelectedIndex == 7)
                        {
                            vApplicationSettings["ScreenDoubleTapStart"] = false;
                            cb_SettingsScreenDoubleTapStart.IsEnabled = false;
                            cb_SettingsScreenDoubleTapStart.IsChecked = false;
                        }
                        else { cb_SettingsScreenDoubleTapStart.IsEnabled = true; }
                    }
                };

                //Save - Application debugging
                cb_SettingsAppDebug.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["AppDebug"] = true; }
                    else { vApplicationSettings["AppDebug"] = false; }
                };

                //Save - Screens Disable Back Button
                cb_SettingsScreenBackButton.Click += (sender, e) =>
                {
                    CheckBox CheckBox = (CheckBox)sender;
                    if ((bool)CheckBox.IsChecked) { vApplicationSettings["ScreenBackButton"] = true; }
                    else { vApplicationSettings["ScreenBackButton"] = false; }
                };
            }
            catch (Exception Ex) { await new MessageDialog("SettingsSaveError: " + Ex.Message, "TimeMe").ShowAsync(); }
        }
    }
}