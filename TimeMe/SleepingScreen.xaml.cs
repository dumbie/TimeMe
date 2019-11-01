using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Power;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace TimeMe
{
    public partial class SleepingScreen : Page
    {
        //Application Variables
        IDictionary<string, object> vApplicationSettings = ApplicationData.Current.LocalSettings.Values;
        CultureInfo vCultureInfoEng = new CultureInfo("en-US");
        CultureInfo vCultureInfoReg = new CultureInfo(Windows.Globalization.Language.CurrentInputMethodLanguageTag);
        DispatcherTimer TimerUpdateSleepingscreen = new DispatcherTimer();
        DispatcherTimer TimerUpdateWallpaper = new DispatcherTimer();
        string WeatherIconStyle = "Weather";
        string ClockStyle = "Round";
        int CurrentColor = 0;
        Point StartingPoint;

        public SleepingScreen()
        {
            try
            {
                this.InitializeComponent();
                Loaded += async delegate
                {
                    //Handle Device Back Button
                    SystemNavigationManager.GetForCurrentView().BackRequested += SleepBackButtonPressed;

                    //Hide Phone StatusBar
                    await HideStatusBar();

                    //Set device specific font size
                    if ((bool)vApplicationSettings["DevStatusMobile"])
                    {
                        TimeHour.FontSize = 80;
                        TimeHour.Height = 95;
                        TimeHour.Margin = new Thickness(0, -72, 0, 0);
                        TimeSplit.FontSize = 80;
                        TimeSplit.Height = 95;
                        TimeSplit.Margin = new Thickness(0, -72, 0, 0);
                        TimeMinute.FontSize = 80;
                        TimeMinute.Height = 95;
                        TimeMinute.Margin = new Thickness(0, -72, 0, 0);
                        TimeAmPm.FontSize = 30;
                    }

                    //Load live tile font family
                    TimeHour.FontFamily = new FontFamily(vApplicationSettings["LiveTileFont"].ToString());
                    TimeSplit.FontFamily = new FontFamily(vApplicationSettings["LiveTileFont"].ToString());
                    TimeMinute.FontFamily = new FontFamily(vApplicationSettings["LiveTileFont"].ToString());
                    if ((bool)vApplicationSettings["DisplayAMPMFont"]) { TimeAmPm.FontFamily = new FontFamily(vApplicationSettings["LiveTileFont"].ToString()); }

                    //Load the screen time font weight
                    switch (Convert.ToInt32(vApplicationSettings["LiveTileFontWeight"]))
                    {
                        case 0:
                            {
                                if ((bool)vApplicationSettings["DisplayHourBold"]) { TimeHour.FontWeight = FontWeights.Normal; } else { TimeHour.FontWeight = FontWeights.Light; }
                                TimeSplit.FontWeight = FontWeights.Light;
                                TimeMinute.FontWeight = FontWeights.Light;
                                break;
                            }
                        case 1:
                            {
                                if ((bool)vApplicationSettings["DisplayHourBold"]) { TimeHour.FontWeight = FontWeights.SemiBold; } else { TimeHour.FontWeight = FontWeights.Normal; }
                                TimeSplit.FontWeight = FontWeights.Normal;
                                TimeMinute.FontWeight = FontWeights.Normal;
                                break;
                            }
                        case 2:
                            {
                                if ((bool)vApplicationSettings["DisplayHourBold"]) { TimeHour.FontWeight = FontWeights.Bold; } else { TimeHour.FontWeight = FontWeights.SemiBold; }
                                TimeSplit.FontWeight = FontWeights.SemiBold;
                                TimeMinute.FontWeight = FontWeights.SemiBold;
                                break;
                            }
                    }

                    //Set the sleep screen time layout
                    if ((bool)vApplicationSettings["SleepAnalogClock"])
                    {
                        sp_SleepDetails.Margin = new Thickness(0, 0, 0, 210);
                        sp_ClockDigital.Visibility = Visibility.Collapsed;
                        img_ClockAnalog.Visibility = Visibility.Visible;

                        //Set the selected analog clock style
                        switch ((int)vApplicationSettings["SleepAnalogClockStyle"])
                        {
                            case 1: { ClockStyle = "Cortana"; break; }
                            case 2: { ClockStyle = "Minimal"; break; }
                        }
                    }
                    else
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"]) { sp_SleepDetails.Margin = new Thickness(0, 0, 0, 95); } else { sp_SleepDetails.Margin = new Thickness(0, 0, 0, 120); }
                        sp_ClockDigital.Visibility = Visibility.Visible;
                        img_ClockAnalog.Visibility = Visibility.Collapsed;

                        //Set the time splitter character
                        switch ((int)vApplicationSettings["DisplayTimeSplitter"])
                        {
                            case 0: { TimeSplit.Text = ":"; break; }
                            case 1: { TimeSplit.Text = "."; break; }
                            case 2: { TimeSplit.Text = ","; break; }
                            case 3: { TimeSplit.Text = "_"; break; }
                            case 4: { TimeSplit.Text = "-"; break; }
                            case 5: { TimeSplit.Text = "+"; break; }
                            case 6: { TimeSplit.Text = "x"; break; }
                            case 7: { TimeSplit.Text = "|"; break; }
                            case 8: { TimeSplit.Text = "!"; break; }
                            case 9: { TimeSplit.Text = "*"; break; }
                            case 10: { TimeSplit.Text = "@"; break; }
                            case 11: { TimeSplit.Text = "#"; break; }
                            case 12: { TimeSplit.Text = "&"; break; }
                            case 13: { TimeSplit.Text = "̥"; break; }
                            case 14: { TimeSplit.Text = "͓"; break; }
                            case 15: { TimeSplit.Text = " "; break; }
                        }
                    }

                    //Load the weather icon style
                    if ((bool)vApplicationSettings["DisplayWeatherWhiteIcons"]) { WeatherIconStyle = "WeatherWhite"; }

                    //Enter fullscreen mode
                    if (!MainPage.vApplicationView.IsFullScreenMode) { MainPage.vApplicationView.TryEnterFullScreenMode(); }

                    //Prevent application lock screen
                    try { MainPage.vDisplayRequest.RequestActive(); } catch { }

                    //Monitor user touch swipes
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

                    //Set sleepingscreen orientation
                    if ((int)vApplicationSettings["ScreenOrientation"] == 0)//Hor0/Ver1
                    {
                        grid_Sleepwallpaper.RenderTransform = new RotateTransform() { Angle = 90 };
                        grid_Sleepwallpaper.Margin = new Thickness(App.vLaunchDisplayResolution.Width, 0, -App.vLaunchDisplayResolution.Height, -(App.vLaunchDisplayResolution.Width - App.vLaunchDisplayResolution.Height));
                    }

                    //Start updating the screen information
                    await UpdateSleepingscreen();
                    TimerUpdateSleepingscreen = new DispatcherTimer();
                    TimerUpdateSleepingscreen.Interval = TimeSpan.FromSeconds(10);
                    TimerUpdateSleepingscreen.Tick += async delegate { await UpdateSleepingscreen(); };
                    TimerUpdateSleepingscreen.Start();

                    //Start updating the background wallpaper
                    await UpdateWallpaper();
                    TimerUpdateWallpaper = new DispatcherTimer();
                    TimerUpdateWallpaper.Interval = TimeSpan.FromMinutes(10);
                    TimerUpdateWallpaper.Tick += async delegate { await UpdateWallpaper(); };
                    TimerUpdateWallpaper.Start();
                };
            }
            catch { }
        }

        //Set sleepingscreen wallpaper
        async Task UpdateWallpaper()
        {
            try
            {
                if ((bool)vApplicationSettings["ScreenWallpaper"] && await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                {
                    StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMeTilePhoto.png");
                    using (IRandomAccessStream OpenAsync = await StorageFile.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapImage BitmapImage = new BitmapImage();
                        await BitmapImage.SetSourceAsync(OpenAsync);
                        OpenAsync.Dispose();
                        grid_Sleepwallpaper.Background = new ImageBrush() { ImageSource = new Image() { Source = BitmapImage }.Source, Opacity = ((float)Convert.ToInt32(vApplicationSettings["DisplayBackgroundBrightness"]) / 100), Stretch = Stretch.UniformToFill, AlignmentY = AlignmentY.Center, AlignmentX = AlignmentX.Center };
                    }
                }
            }
            catch { }
        }

        //Update UpdateSleepingscreen
        async Task UpdateSleepingscreen()
        {
            try
            {
                //Set the screen time text
                DateTime DateTimeMin = DateTime.Now.AddSeconds(-DateTime.Now.Second);
                if ((bool)vApplicationSettings["SleepAnalogClock"])
                {
                    string ClockUri = "ms-appx:///Assets/Analog/" + ClockStyle + "/" + DateTimeMin.ToString("hmm") + ".png";
                    if (((BitmapImage)img_ClockAnalog.Source).UriSource.ToString() != ClockUri) { img_ClockAnalog.Source = new BitmapImage() { UriSource = new Uri(ClockUri, UriKind.Absolute) }; }
                }
                else
                {
                    string HourText = String.Empty;
                    string AmPmText = String.Empty;
                    string MinuteText = DateTime.Now.ToString("mm");
                    if ((bool)vApplicationSettings["Display24hClock"]) { HourText = DateTimeMin.ToString("HH"); } else { HourText = DateTimeMin.ToString("%h"); }
                    if ((bool)vApplicationSettings["DisplayAMPMClock"])
                    {
                        if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                        {
                            AmPmText = DateTimeMin.ToString("tt", vCultureInfoReg);
                            if (String.IsNullOrEmpty(AmPmText)) { AmPmText = DateTimeMin.ToString("tt", vCultureInfoEng); }
                        }
                        else { AmPmText = DateTimeMin.ToString("tt", vCultureInfoEng); }
                    }

                    if (TimeHour.Text != HourText) { AVAnimations.Ani_TextFadeInandOut(TimeHour, HourText); }
                    if (TimeMinute.Text != MinuteText) { AVAnimations.Ani_TextFadeInandOut(TimeMinute, MinuteText); }
                    if (TimeAmPm.Text != AmPmText) { AVAnimations.Ani_TextFadeInandOut(TimeAmPm, AmPmText); }
                }

                //Set the screen date text
                if ((bool)vApplicationSettings["SleepDate"])
                {
                    string DisplayDate = String.Empty;
                    if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                    {
                        if ((bool)vApplicationSettings["Display24hClock"]) { DisplayDate = AVFunctions.ToTitleCase(DateTimeMin.ToString("dddd d MMMM", vCultureInfoReg)); }
                        else { DisplayDate = AVFunctions.ToTitleCase(DateTimeMin.ToString("dddd MMMM d", vCultureInfoReg)); }
                    }
                    else
                    {
                        if ((bool)vApplicationSettings["Display24hClock"]) { DisplayDate = DateTimeMin.ToString("dddd d MMMM", vCultureInfoEng); }
                        else { DisplayDate = DateTimeMin.ToString("dddd MMMM d", vCultureInfoEng); }
                    }

                    if (txt_SleepDate.Text != DisplayDate) { AVAnimations.Ani_TextFadeInandOut(txt_SleepDate, DisplayDate); }
                }
                else { txt_SleepDate.Visibility = Visibility.Collapsed; }

                //Check for active alarms and timers
                if (!(bool)vApplicationSettings["SleepAlarm"] || !ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Any(x => x.DeliveryTime > DateTime.Now))
                { img_AlarmClock.Visibility = Visibility.Collapsed; }

                //Check and set Weather information
                if ((bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadWeather"] && (bool)vApplicationSettings["SleepWeather"])
                {
                    string WeatherCurrentIcon = vApplicationSettings["BgStatusWeatherCurrentIcon"].ToString();
                    string WeatherCurrentTemp = vApplicationSettings["BgStatusWeatherCurrentTemp"].ToString();

                    string WeatherIconUri = String.Empty;
                    if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherCurrentIcon + ".png")) { WeatherIconUri = "ms-appx:///Assets/" + WeatherIconStyle + "/" + WeatherCurrentIcon + ".png"; }
                    else { WeatherIconUri = "ms-appx:///Assets/" + WeatherIconStyle + "/0.png"; }

                    if (((BitmapImage)img_WeatherIcon.Source).UriSource.ToString() != WeatherIconUri) { img_WeatherIcon.Source = new BitmapImage() { UriSource = new Uri(WeatherIconUri, UriKind.Absolute) }; }
                    if (txt_WeatherIcon.Text != WeatherCurrentTemp) { AVAnimations.Ani_TextFadeInandOut(txt_WeatherIcon, WeatherCurrentTemp); }
                }
                else
                {
                    img_WeatherIcon.Visibility = Visibility.Collapsed;
                    txt_WeatherIcon.Visibility = Visibility.Collapsed;
                }

                //Load current battery level
                if ((bool)vApplicationSettings["SleepBattery"])
                {
                    //Check if there is battery status available
                    if (PowerManager.BatteryStatus == BatteryStatus.NotPresent)
                    {
                        if (txt_BatteryLevel.Text != "N/A") { AVAnimations.Ani_TextFadeInandOut(txt_BatteryLevel, "N/A"); }
                    }
                    else
                    {
                        string BatteryLevel = PowerManager.RemainingChargePercent + "%";
                        if (txt_BatteryLevel.Text != BatteryLevel) { AVAnimations.Ani_TextFadeInandOut(txt_BatteryLevel, BatteryLevel); }
                    }
                }
                else
                {
                    img_BatteryLevel.Visibility = Visibility.Collapsed;
                    txt_BatteryLevel.Visibility = Visibility.Collapsed;
                }
            }
            catch { txt_SleepDate.Text = "Failed to load sleeping screen"; }
        }

        //Hide Status Bar
        async Task HideStatusBar() { if ((bool)vApplicationSettings["DevStatusMobile"]) { await MainPage.vStatusBar.HideAsync(); } }

        //Handle user single tap
        void Page_Tap(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (sp_Sleepclock.Opacity == 1) { Ani_Opacity(sp_Sleepclock, 0.75); return; }
                if (sp_Sleepclock.Opacity == 0.75) { Ani_Opacity(sp_Sleepclock, 0.50); return; }
                if (sp_Sleepclock.Opacity == 0.50) { Ani_Opacity(sp_Sleepclock, 0.25); return; }
                if (sp_Sleepclock.Opacity == 0.25) { Ani_Opacity(sp_Sleepclock, 1); return; }
            }
            catch { }
        }

        //Storyboard - Change the Opacity
        void Ani_Opacity(FrameworkElement ObjFrameworkElement, double Opacity)
        {
            try
            {
                DoubleAnimation DoubleAnimation = new DoubleAnimation();
                DoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                DoubleAnimation.From = ObjFrameworkElement.Opacity;
                DoubleAnimation.To = Opacity;

                Storyboard sb_Opacity = new Storyboard();
                sb_Opacity.Children.Add(DoubleAnimation);

                Storyboard.SetTarget(sb_Opacity, ObjFrameworkElement);
                Storyboard.SetTargetProperty(sb_Opacity, "Opacity");

                sb_Opacity.Begin();
            }
            catch { }
        }

        //Handle user double taps
        async void Page_DoubleTap(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {
                if ((bool)vApplicationSettings["ScreenDoubleTapStart"])
                {
                    Application.Current.Exit();
                    return;
                }
                else
                {
                    TimerUpdateWallpaper.Stop();
                    TimerUpdateSleepingscreen.Stop();
                    //Close fullscreen mode
                    MainPage.vApplicationView.ExitFullScreenMode();
                    //Allow application lock screen
                    try { MainPage.vDisplayRequest.RequestRelease(); } catch { }
                    if ((bool)vApplicationSettings["DevStatusMobile"]) { await MainPage.vStatusBar.ShowAsync(); }
                    Frame.Navigate(typeof(MainPage), "tab_Tile");
                    return;
                }
            }
            catch { }
        }

        //Handle Device Back Button
        async void SleepBackButtonPressed(object sender, BackRequestedEventArgs e)
        {
            try
            {
                if (Frame.CurrentSourcePageType.Name == "SleepingScreen")
                {
                    if ((bool)vApplicationSettings["ScreenBackButton"])
                    {
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        e.Handled = true;
                        TimerUpdateWallpaper.Stop();
                        TimerUpdateSleepingscreen.Stop();
                        //Close fullscreen mode
                        MainPage.vApplicationView.ExitFullScreenMode();
                        //Allow application lock screen
                        try { MainPage.vDisplayRequest.RequestRelease(); } catch { }
                        if ((bool)vApplicationSettings["DevStatusMobile"]) { await MainPage.vStatusBar.ShowAsync(); }
                        Frame.Navigate(typeof(MainPage), "tab_Tile");
                        return;
                    }
                }
            }
            catch { }
        }

        //Handle user touch swipe
        void Page_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { StartingPoint = e.Position; }

        async void Page_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            try
            {
                Point CurrentPoint = e.Position;
                if ((CurrentPoint.X - StartingPoint.X >= 175) || (CurrentPoint.Y - StartingPoint.Y >= 175))
                {
                    SolidColorBrush LiveTileColorFont = new SolidColorBrush(Color.FromArgb(Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(1, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(3, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(5, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(7, 2), 16)));
                    if (new SolidColorBrush(Colors.White).Color == LiveTileColorFont.Color) { LiveTileColorFont = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; }
                    else if (new SolidColorBrush(Colors.Black).Color == LiveTileColorFont.Color) { LiveTileColorFont = new SolidColorBrush(Colors.DimGray); }

                    if ((bool)vApplicationSettings["ScreenWallpaper"] && await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                    {
                        if (CurrentColor == 0) { CurrentColor = 2; TimeHour.Foreground = new SolidColorBrush(Colors.Black); TimeSplit.Foreground = new SolidColorBrush(Colors.Black); TimeMinute.Foreground = new SolidColorBrush(Colors.Black); TimeAmPm.Foreground = new SolidColorBrush(Colors.Black); txt_BatteryLevel.Foreground = new SolidColorBrush(Colors.Black); img_BatteryLevel.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/BatteryLevelBlack.png", UriKind.Absolute) }; txt_WeatherIcon.Foreground = new SolidColorBrush(Colors.Black); txt_SleepDate.Foreground = new SolidColorBrush(Colors.Black); img_AlarmClock.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/AlarmClockBlack.png", UriKind.Absolute) }; return; }
                        if (CurrentColor == 1) { CurrentColor = 0; TimeHour.Foreground = new SolidColorBrush(Colors.White); TimeSplit.Foreground = new SolidColorBrush(Colors.White); TimeMinute.Foreground = new SolidColorBrush(Colors.White); TimeAmPm.Foreground = new SolidColorBrush(Colors.White); txt_BatteryLevel.Foreground = new SolidColorBrush(Colors.White); img_BatteryLevel.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/BatteryLevel.png", UriKind.Absolute) }; txt_WeatherIcon.Foreground = new SolidColorBrush(Colors.White); txt_SleepDate.Foreground = new SolidColorBrush(Colors.White); img_AlarmClock.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/AlarmClock.png", UriKind.Absolute) }; return; }
                        if (CurrentColor == 2)
                        {
                            CurrentColor = 1;

                            if ((bool)vApplicationSettings["LiveTileFontDuoColor"])
                            {
                                TimeHour.Foreground = new SolidColorBrush(Colors.White);
                                TimeSplit.Foreground = LiveTileColorFont;
                                TimeMinute.Foreground = LiveTileColorFont;
                                TimeAmPm.Foreground = new SolidColorBrush(Colors.White);
                                txt_BatteryLevel.Foreground = new SolidColorBrush(Colors.White);
                                txt_WeatherIcon.Foreground = new SolidColorBrush(Colors.White);
                                txt_SleepDate.Foreground = LiveTileColorFont;
                            }
                            else
                            {
                                TimeHour.Foreground = LiveTileColorFont;
                                TimeSplit.Foreground = LiveTileColorFont;
                                TimeMinute.Foreground = LiveTileColorFont;
                                TimeAmPm.Foreground = LiveTileColorFont;
                                txt_BatteryLevel.Foreground = LiveTileColorFont;
                                txt_WeatherIcon.Foreground = LiveTileColorFont;
                                txt_SleepDate.Foreground = LiveTileColorFont;
                            }

                            img_BatteryLevel.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/BatteryLevelYellow.png", UriKind.Absolute) }; img_AlarmClock.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/AlarmClock.png", UriKind.Absolute) }; return;
                        }
                    }
                    else
                    {
                        if (CurrentColor == 0) { CurrentColor = 2; if ((bool)vApplicationSettings["DisplayWeatherWhiteIcons"]) { grid_Sleepwallpaper.Background = new SolidColorBrush(Colors.LightGray); } else { grid_Sleepwallpaper.Background = new SolidColorBrush(Colors.White); } TimeHour.Foreground = new SolidColorBrush(Colors.Black); TimeSplit.Foreground = new SolidColorBrush(Colors.Black); TimeMinute.Foreground = new SolidColorBrush(Colors.Black); TimeAmPm.Foreground = new SolidColorBrush(Colors.Black); txt_BatteryLevel.Foreground = new SolidColorBrush(Colors.Black); img_BatteryLevel.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/BatteryLevelBlack.png", UriKind.Absolute) }; txt_WeatherIcon.Foreground = new SolidColorBrush(Colors.Black); txt_SleepDate.Foreground = new SolidColorBrush(Colors.Black); img_AlarmClock.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/AlarmClockBlack.png", UriKind.Absolute) }; return; }
                        if (CurrentColor == 1) { CurrentColor = 0; grid_Sleepwallpaper.Background = new SolidColorBrush(Colors.Black); TimeHour.Foreground = new SolidColorBrush(Colors.White); TimeSplit.Foreground = new SolidColorBrush(Colors.White); TimeMinute.Foreground = new SolidColorBrush(Colors.White); TimeAmPm.Foreground = new SolidColorBrush(Colors.White); txt_BatteryLevel.Foreground = new SolidColorBrush(Colors.White); img_BatteryLevel.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/BatteryLevel.png", UriKind.Absolute) }; txt_WeatherIcon.Foreground = new SolidColorBrush(Colors.White); txt_SleepDate.Foreground = new SolidColorBrush(Colors.White); img_AlarmClock.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/AlarmClock.png", UriKind.Absolute) }; return; }
                        if (CurrentColor == 2)
                        {
                            CurrentColor = 1;
                            grid_Sleepwallpaper.Background = new SolidColorBrush(Colors.Black);

                            if ((bool)vApplicationSettings["LiveTileFontDuoColor"])
                            {
                                TimeHour.Foreground = new SolidColorBrush(Colors.White);
                                TimeSplit.Foreground = LiveTileColorFont;
                                TimeMinute.Foreground = LiveTileColorFont;
                                TimeAmPm.Foreground = new SolidColorBrush(Colors.White);
                                txt_BatteryLevel.Foreground = new SolidColorBrush(Colors.White);
                                txt_WeatherIcon.Foreground = new SolidColorBrush(Colors.White);
                                txt_SleepDate.Foreground = LiveTileColorFont;
                            }
                            else
                            {
                                TimeHour.Foreground = LiveTileColorFont;
                                TimeSplit.Foreground = LiveTileColorFont;
                                TimeMinute.Foreground = LiveTileColorFont;
                                TimeAmPm.Foreground = LiveTileColorFont;
                                txt_BatteryLevel.Foreground = LiveTileColorFont;
                                txt_WeatherIcon.Foreground = LiveTileColorFont;
                                txt_SleepDate.Foreground = LiveTileColorFont;
                            }

                            img_BatteryLevel.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/BatteryLevelYellow.png", UriKind.Absolute) }; img_AlarmClock.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/Icons/AlarmClock.png", UriKind.Absolute) }; return;
                        }
                    }
                }
            }
            catch { }
        }
    }
}