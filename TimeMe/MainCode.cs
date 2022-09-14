using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.System.Display;
using Windows.System.Power;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TimeMe
{
    public partial class MainPage : Page
    {
        //Application Variables
        public static uint vBackgroundTaskStatus = 100;
        public static IDictionary<string, object> vApplicationSettings = ApplicationData.Current.LocalSettings.Values;
        public static CultureInfo vCultureInfoEng = new CultureInfo("en-US");
        public static CultureInfo vCultureInfoReg = new CultureInfo(Windows.Globalization.Language.CurrentInputMethodLanguageTag);
        public static ApplicationView vApplicationView = ApplicationView.GetForCurrentView();
        public static ApplicationViewTitleBar vTitleBar;
        public static DisplayRequest vDisplayRequest = new DisplayRequest();
        public static StatusBar vStatusBar;

        //Load and check - Voice Commands for Cortana - Not awaiting to avoid slow startup
        async Task RegisterCortanaCommands()
        {
            try
            {
                VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///VoiceCommands.xml"))); //NoAwait
                Debug.WriteLine("Successfully registered the Cortana voice commands.");
            }
            catch
            {
                Debug.WriteLine("Failed to register the Cortana voice commands.");
            }
        }

        //Cortana Information Voice Speech
        async void btn_TestCortanaVoiceSpeech_Click(object sender, RoutedEventArgs e) { await CortanaVoiceSpeech(); }
        async Task CortanaVoiceSpeech()
        {
            try
            {
                DateTime DateTimeNow = DateTime.Now;
                TimeSpan MinLeftToday = new DateTime(DateTimeNow.Year, DateTimeNow.Month, DateTimeNow.AddDays(1).Day).Subtract(DateTimeNow);
                string VoiceInfoText = "Sorry, something went wrong.";
                string WordsTimeHour = "";

                //Check set time hour
                if (DateTimeNow.Minute > 30) { WordsTimeHour = DateTimeNow.AddHours(1).ToString("%h").Replace("00", "12"); }
                else { WordsTimeHour = DateTimeNow.ToString("%h").Replace("00", "12"); }

                //Set current time words text - shared with words tile
                if (DateTimeNow.Minute != 0 && DateTimeNow.Minute != 15 && DateTimeNow.Minute != 30 && DateTimeNow.Minute != 45)
                {
                    if (DateTimeNow.Minute > 30) { VoiceInfoText = "it is " + (60 - DateTimeNow.Minute) + " to " + WordsTimeHour; }
                    else { VoiceInfoText = "it is " + DateTimeNow.Minute + " past " + WordsTimeHour; }
                }
                else if (DateTimeNow.Minute == 0) { VoiceInfoText = "it is " + WordsTimeHour + " o'clock"; }
                else if (DateTimeNow.Minute == 15) { VoiceInfoText = "it is quarter past " + WordsTimeHour; }
                else if (DateTimeNow.Minute == 30) { VoiceInfoText = "it is half past " + WordsTimeHour; }
                else if (DateTimeNow.Minute == 45) { VoiceInfoText = "it is quarter to " + WordsTimeHour; }

                //Check for appointments
                try
                {
                    AppointmentStore AppointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
                    IReadOnlyList<Appointment> Appointments = await AppointmentStore.FindAppointmentsAsync(DateTimeNow, MinLeftToday);
                    if (Appointments.Any()) { VoiceInfoText = ", " + VoiceInfoText + " you have " + Appointments.Count + " appointment left for today"; }
                }
                catch { }

                //Check for weather information
                if ((bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadWeather"])
                {
                    try
                    {
                        string BgStatusWeatherCurrentTemp = vApplicationSettings["BgStatusWeatherCurrentTemp"].ToString().Replace("!", "");
                        if (BgStatusWeatherCurrentTemp != "N/A") { VoiceInfoText = VoiceInfoText + ", and it is " + BgStatusWeatherCurrentTemp + " outside"; }
                    }
                    catch { }
                }

                VoiceInfoText = VoiceInfoText + ", enjoy the rest of your day";

                //Convert text to speech and play the speech
                using (SpeechSynthesizer vSpeechSynthesizer = new SpeechSynthesizer())
                {
                    IEnumerable<VoiceInformation> FemaleVoiceList = SpeechSynthesizer.AllVoices.Where(x => x.Gender == VoiceGender.Female && !x.DisplayName.Contains("Hazel"));
                    if (FemaleVoiceList.Any()) { vSpeechSynthesizer.Voice = FemaleVoiceList.First(); } else { vSpeechSynthesizer.Voice = SpeechSynthesizer.AllVoices.First(); }

                    SpeechSynthesisStream SpeechStream = await vSpeechSynthesizer.SynthesizeTextToStreamAsync(VoiceInfoText);
                    MediaSource SpeechMediaSource = MediaSource.CreateFromStream(SpeechStream, SpeechStream.ContentType);

                    MediaPlayer MediaPlayer = new MediaPlayer();
                    MediaPlayer.AudioCategory = MediaPlayerAudioCategory.Speech;
                    MediaPlayer.MediaEnded += delegate
                    {
                        try
                        {
                            SpeechStream.Dispose();
                            SpeechMediaSource.Dispose();
                            MediaPlayer.Dispose();
                        }
                        catch { }
                        Debug.WriteLine("Disposed Cortana's voice speech.");
                    };

                    MediaPlayer.Source = SpeechMediaSource;
                    MediaPlayer.Play();
                }
            }
            catch { await new MessageDialog("Sorry, It seems like I have left my speech at home or may have lost my voice, please try again later.", "TimeMe").ShowAsync(); }
        }

        //Load Pinned Tile Status
        void PinnedTileStatusLoad()
        {
            try
            {
                if (SecondaryTile.Exists("TimeMeLiveTile"))
                {
                    txt_LiveTileStatus.Text = "The TimeMe live tile is currently pinned.";
                    btn_PinLiveTile.Content = "Unpin TimeMe Time Live Tile from Start";
                }
                if (SecondaryTile.Exists("TimeMeWeatherTile")) { btn_PinWeatherTile.Content = "Unpin TimeMe Weather Tile from Start"; }
                if (SecondaryTile.Exists("TimeMeBatteryTile")) { btn_PinBatteryTile.Content = "Unpin TimeMe Battery Tile from Start"; }
                if (SecondaryTile.Exists("SleepingScreen")) { btn_PinSleepingScreen.Content = "Unpin the Sleeping Screen tile"; }
                if (SecondaryTile.Exists("FlashLight")) { btn_PinFlashlight.Content = "Unpin the Flashlight tile"; }
            }
            catch { }
        }

        //Load Week Number and Day Percentage
        void WeekNumberLoad()
        {
            try
            {
                //Load current Week Number
                DateTime DateTimeWeekNow = DateTime.Now;
                DayOfWeek DateTimeDay = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(DateTimeWeekNow);
                if (DateTimeDay >= DayOfWeek.Monday && DateTimeDay <= DayOfWeek.Wednesday) { DateTimeWeekNow = DateTimeWeekNow.AddDays(3); }
                txt_WeekNumber.Text = "We are currently living in week number " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTimeWeekNow, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                //Load Day Progress Percentage and Day Number
                int DayTimeProgress = Convert.ToInt32(100 - (((1440 - DateTime.Now.Subtract(DateTime.Today).TotalMinutes) / 1440) * 100));
                txt_WeekNumber.Text = txt_WeekNumber.Text + "\nand day " + DateTime.Now.DayOfYear + " has progressed for " + DayTimeProgress + " percent";

                //Load Day Time Remaining
                TimeSpan TimeTomorrow = DateTime.Today.AddDays(1).Subtract(DateTime.Now);
                int TimeTomorrowHours = TimeTomorrow.Hours; int TimeTomorrowMinutes = TimeTomorrow.Minutes;

                string TimeTillTomorrow = "";
                if (TimeTomorrowHours != 0) { TimeTillTomorrow = TimeTillTomorrow + TimeTomorrowHours + "h "; }
                if (TimeTomorrowMinutes != 0) { TimeTillTomorrow = TimeTillTomorrow + TimeTomorrowMinutes + "m "; }
                if (String.IsNullOrEmpty(TimeTillTomorrow)) { TimeTillTomorrow = "a minute "; }

                txt_WeekNumber.Text = txt_WeekNumber.Text + "\nwith about " + TimeTillTomorrow + "remaining on this day.";
            }
            catch { }
        }

        //Load current live tile
        async Task CurrentTileLoad()
        {
            try
            {
                //Check if TimeMe live tile is pinned
                if (SecondaryTile.Exists("TimeMeLiveTile"))
                {
                    //Load the current live tile preview
                    if (!(bool)vApplicationSettings["LiveTileSizeLight"])
                    {
                        if (await AVFunctions.LocalFileExists("TimeMe0.png"))
                        {
                            txt_LiveTileCurrent.Text = "Your TimeMe live tile design looks like this:";
                            grid_LiveTileCurrent.Visibility = Visibility.Visible;
                            StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMe0.png");
                            using (IRandomAccessStream OpenAsync = await StorageFile.OpenAsync(FileAccessMode.Read))
                            {
                                BitmapImage BitmapImage = new BitmapImage();
                                await BitmapImage.SetSourceAsync(OpenAsync);
                                OpenAsync.Dispose();
                                img_LiveTileCurrent.Source = BitmapImage;
                            }
                            grid_LiveTileCurrent.Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"];
                        }
                        else
                        {
                            txt_LiveTileCurrent.Text = "There is no live tile preview available.";
                            grid_LiveTileCurrent.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        txt_LiveTileCurrent.Text = "Your TimeMe live tile design looks like this:";

                        if (await AVFunctions.AppFileExists("Assets/Preview/" + vApplicationSettings["LiveTileSizeName"].ToString() + ".png")) { img_LiveTileCurrent.Source = new BitmapImage(new Uri("ms-appx:///Assets/Preview/" + vApplicationSettings["LiveTileSizeName"].ToString() + ".png", UriKind.Absolute)); }
                        else { img_LiveTileCurrent.Source = new BitmapImage(new Uri("ms-appx:///Assets/Preview/" + vApplicationSettings["LiveTileSizeName"].ToString() + ".gif", UriKind.Absolute)); }

                        grid_LiveTileCurrent.Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"];
                        grid_LiveTileCurrent.Visibility = Visibility.Visible;
                    }

                    //Calculate time till next live tile update
                    try
                    {
                        IReadOnlyList<ScheduledTileNotification> Tile_PlannedUpdates = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeLiveTile").GetScheduledTileNotifications();
                        if (Tile_PlannedUpdates.Any()) { txt_LiveTileUpdateTime.Text = "Next live tile update will begin in ~" + Convert.ToInt32(Tile_PlannedUpdates.Last().DeliveryTime.Subtract(DateTime.Now).TotalMinutes) + "mins"; }
                        else { txt_LiveTileUpdateTime.Text = "There is no future live tile update planned."; }
                    }
                    catch { txt_LiveTileUpdateTime.Text = "There is no future live tile update planned."; }
                }
                else
                {
                    grid_LiveTileCurrent.Visibility = Visibility.Collapsed;
                    txt_LiveTileCurrent.Text = "There is no live tile preview available.";
                    txt_LiveTileUpdateTime.Text = "There is no future live tile update planned.";
                }

                //Get the current Bing description
                if ((bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadBingWallpaper"])
                {
                    string BgStatusBingDescription = vApplicationSettings["BgStatusBingDescription"].ToString();
                    if (BgStatusBingDescription != "Never") { txt_BingInformation.Text = "Bing description: " + BgStatusBingDescription; txt_BingInformation.Visibility = Visibility.Visible; }
                    else { txt_BingInformation.Text = "Bing description is currently not available."; txt_BingInformation.Visibility = Visibility.Visible; }
                }
                else { txt_BingInformation.Visibility = Visibility.Collapsed; }
            }
            catch { }
        }

        //Load Battery Status
        async Task BatteryLevelLoad()
        {
            try
            {
                //Check if there is battery status available
                BatteryStatus BatteryStatus = PowerManager.BatteryStatus;
                if (BatteryStatus == BatteryStatus.NotPresent) { txt_BatteryStatus.Visibility = Visibility.Collapsed; }
                else
                {
                    //Set battery remaining percent and time
                    int BatteryPercent = PowerManager.RemainingChargePercent;
                    TimeSpan BatteryDischargeTime = PowerManager.RemainingDischargeTime;

                    //Set the battery status
                    if (BatteryStatus == BatteryStatus.Charging || PowerManager.PowerSupplyStatus != PowerSupplyStatus.NotPresent) { txt_BatteryStatus.Text = "Your device's battery level is now at: " + BatteryPercent + "%\nand is currently connected to a charger."; }
                    else if (BatteryDischargeTime.Days > 31)
                    {
                        BatteryDischargeTime = (TimeSpan)vApplicationSettings["DevStatusBatteryTime"];
                        int BatteryDays = BatteryDischargeTime.Days; int BatteryHours = BatteryDischargeTime.Hours; int BatteryMinutes = BatteryDischargeTime.Minutes;

                        string BatteryTime = "";
                        if (BatteryDischargeTime == new TimeSpan()) { BatteryTime = "unknown "; }
                        else
                        {
                            if (BatteryDays != 0) { BatteryTime = BatteryTime + BatteryDays + "d "; }
                            if (BatteryHours != 0) { BatteryTime = BatteryTime + BatteryHours + "h "; }
                            if (BatteryMinutes != 0) { BatteryTime = BatteryTime + BatteryMinutes + "m "; }
                            if (String.IsNullOrEmpty(BatteryTime)) { BatteryTime = "unknown "; }
                        }

                        BatteryTime = "\nand has about " + BatteryTime + "time remaining.";
                        txt_BatteryStatus.Text = "Your device's battery level is now at: " + BatteryPercent + "%" + BatteryTime;
                    }
                    else
                    {
                        vApplicationSettings["DevStatusBatteryTime"] = BatteryDischargeTime;
                        int BatteryDays = BatteryDischargeTime.Days; int BatteryHours = BatteryDischargeTime.Hours; int BatteryMinutes = BatteryDischargeTime.Minutes;

                        string BatteryTime = "";
                        if (BatteryDays != 0) { BatteryTime = BatteryTime + BatteryDays + "d "; }
                        if (BatteryHours != 0) { BatteryTime = BatteryTime + BatteryHours + "h "; }
                        if (BatteryMinutes != 0) { BatteryTime = BatteryTime + BatteryMinutes + "m "; }
                        if (String.IsNullOrEmpty(BatteryTime)) { BatteryTime = "unknown "; }

                        BatteryTime = "\nand has " + BatteryTime + "usage time remaining.";
                        txt_BatteryStatus.Text = "Your device's battery level is now at: " + BatteryPercent + "%" + BatteryTime;
                    }

                    //Display the battery status text
                    txt_BatteryStatus.Visibility = Visibility.Visible;

                    bool BatteryWarning = false;
                    if (AVFunctions.DevOsVersion() >= 14393)
                    {
                        //Check if battery power is low and not on always allowed
                        BackgroundAccessStatus BackgroundManager = await BackgroundExecutionManager.RequestAccessAsync();
                        if (BackgroundManager != BackgroundAccessStatus.AlwaysAllowed) { BatteryWarning = true; }
                    }
                    else
                    {
                        //Check if battery power is low or energy saver is on
                        if (BatteryPercent <= 25 || PowerManager.EnergySaverStatus == EnergySaverStatus.On) { BatteryWarning = true; }
                    }

                    if (BatteryWarning)
                    {
                        txt_BatteryWarning.Visibility = Visibility.Visible;
                        btn_TileBatterySettings.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txt_BatteryWarning.Visibility = Visibility.Collapsed;
                        btn_TileBatterySettings.Visibility = Visibility.Collapsed;
                        //ToastNotificationManager.History.Remove("T8", "G1"); //Freezes a few seconds
                    }
                }
            }
            catch { txt_BatteryStatus.Text = "Your device's battery level is currently not available or there is no battery found."; }
        }

        //Load and start Task Agent
        async Task<bool> TaskAgentLoad()
        {
            try
            {
                //Check if the application has location permission
                await CheckLocationPermission();

                //Check for background permission when needed
                BackgroundAccessStatus BackgroundManager = await BackgroundExecutionManager.RequestAccessAsync();

                bool BackgroundAccessAllowed = false;
                if (AVFunctions.DevOsVersion() >= 14393) { if (BackgroundManager == BackgroundAccessStatus.AlwaysAllowed || BackgroundManager == BackgroundAccessStatus.AllowedSubjectToSystemPolicy) { BackgroundAccessAllowed = true; } }
                else { if (BackgroundManager == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity || BackgroundManager == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) { BackgroundAccessAllowed = true; } }

                if (BackgroundAccessAllowed)
                {
                    btn_LiveTileStartStop.Content = "Stop Live Tile Background Updates";
                    btn_LiveTileStartStop.IsEnabled = true;
                    btn_LiveTileForceUpdate.IsEnabled = true;
                    btn_DataForceUpdate.IsEnabled = true;
                    btn_PinLiveTile.IsEnabled = true;
                    btn_PinWeatherTile.IsEnabled = true;
                    btn_PinBatteryTile.IsEnabled = true;
                    btn_PinBlankTile.IsEnabled = true;

                    //Check running background tasks
                    bool bgTimeMeTaskUser = false;
                    bool bgTimeMeTaskTimer = false;
                    bool bgTimeMeTaskTimeZone = false;
                    foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> BackgroundTask in BackgroundTaskRegistration.AllTasks)
                    {
                        try { if (BackgroundTask.Value.Name == "TimeMeTaskUser") { bgTimeMeTaskUser = true; txt_bgTimeMeTaskUser.Text = "Ok"; BackgroundTask.Value.Progress += (sender, args) => { BackgroundTaskProgress(sender, args); }; } } catch { }
                        try { if (BackgroundTask.Value.Name == "TimeMeTaskTimer") { bgTimeMeTaskTimer = true; txt_bgTimeMeTaskTimer.Text = "Ok"; BackgroundTask.Value.Progress += (sender, args) => { BackgroundTaskProgress(sender, args); }; } } catch { }
                        try { if (BackgroundTask.Value.Name == "TimeMeTaskTimeZone") { bgTimeMeTaskTimeZone = true; txt_bgTimeMeTaskTimeZone.Text = "Ok"; BackgroundTask.Value.Progress += (sender, args) => { BackgroundTaskProgress(sender, args); }; } } catch { }
                    }

                    //Register new background tasks
                    if (!bgTimeMeTaskUser)
                    {
                        BackgroundTaskBuilder BackgroundTaskBuilder = new BackgroundTaskBuilder();
                        BackgroundTaskBuilder.Name = "TimeMeTaskUser";
                        BackgroundTaskBuilder.TaskEntryPoint = "TimeMeTaskAgent.ScheduledAgent";
                        BackgroundTaskBuilder.SetTrigger(new SystemTrigger(SystemTriggerType.UserPresent, false));
                        BackgroundTaskBuilder.Register();
                        txt_bgTimeMeTaskUser.Text = "Ok";
                    }

                    if (!bgTimeMeTaskTimer)
                    {
                        BackgroundTaskBuilder BackgroundTaskBuilder = new BackgroundTaskBuilder();
                        BackgroundTaskBuilder.Name = "TimeMeTaskTimer";
                        BackgroundTaskBuilder.TaskEntryPoint = "TimeMeTaskAgent.ScheduledAgent";
                        BackgroundTaskBuilder.SetTrigger(new TimeTrigger(15, false));
                        BackgroundTaskBuilder.Register();
                        txt_bgTimeMeTaskTimer.Text = "Ok";
                    }

                    if (!bgTimeMeTaskTimeZone)
                    {
                        BackgroundTaskBuilder BackgroundTaskBuilder = new BackgroundTaskBuilder();
                        BackgroundTaskBuilder.Name = "TimeMeTaskTimeZone";
                        BackgroundTaskBuilder.TaskEntryPoint = "TimeMeTaskAgent.ScheduledAgent";
                        BackgroundTaskBuilder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
                        BackgroundTaskBuilder.Register();
                        txt_bgTimeMeTaskTimeZone.Text = "Ok";
                    }

                    return true;
                }
                else
                {
                    btn_LiveTileStartStop.Content = "You don't have background permission";
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("You don't seem to have background permission, please click on 'Enable' and scroll to TimeMe and allow it to enable live tile updates in the background, if this option is turned off the live tile will not be updated, when you have turned the option on please restart the application to start the live tile updates.", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Enable", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    await MessageDialog.ShowAsync();
                    if (MessageDialogResult == true) { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-backgroundapps")); Application.Current.Exit(); }
                    return false;
                }
            }
            catch
            {
                btn_LiveTileStartStop.Content = "Failed to start tile background updates";
                return false;
            }
        }

        //Check if live tile update is needed
        async Task CheckForBackgroundUpdate()
        {
            try
            {
                Debug.WriteLine("Checking if live tile update is needed...");
                string TileUpdateMessage = String.Empty;
                bool TileUpdateNeeded = false;

                //Update live tile if there are only 3 minute updates or less to go
                if (SecondaryTile.Exists("TimeMeLiveTile"))
                {
                    IReadOnlyList<ScheduledTileNotification> Tile_PlannedUpdates = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeLiveTile").GetScheduledTileNotifications();
                    if (!Tile_PlannedUpdates.Any() || (Tile_PlannedUpdates.Any() && Tile_PlannedUpdates.Last().DeliveryTime.Subtract(DateTime.Now).TotalMilliseconds <= 180000))
                    {
                        TileUpdateNeeded = true;
                        TileUpdateMessage = AVFunctions.StringAdd(TileUpdateMessage, "live tile", ",");
                    }
                }

                //Update weather if it is currently not up to date
                if ((bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadWeather"])
                {
                    string BgStatusDownloadLocation = vApplicationSettings["BgStatusDownloadLocation"].ToString();
                    string BgStatusDownloadWeatherTime = vApplicationSettings["BgStatusDownloadWeatherTime"].ToString();
                    if (BgStatusDownloadLocation == "Never" || BgStatusDownloadLocation == "Failed" || BgStatusDownloadWeatherTime == "Never" || BgStatusDownloadWeatherTime == "Failed" || DateTime.Now.Subtract(DateTime.Parse(BgStatusDownloadWeatherTime, vCultureInfoEng)).TotalMinutes >= (int)vApplicationSettings["BackgroundDownloadIntervalMin"])
                    {
                        TileUpdateNeeded = true;
                        TileUpdateMessage = AVFunctions.StringAdd(TileUpdateMessage, "weather", ",");
                    }
                }

                //Update Bing if it is currently not up to date
                if ((bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadBingWallpaper"])
                {
                    string BgStatusDownloadBing = vApplicationSettings["BgStatusDownloadBing"].ToString();
                    if (BgStatusDownloadBing == "Never" || DateTime.Now.Subtract(DateTime.Parse(BgStatusDownloadBing, vCultureInfoEng)).TotalMinutes >= (int)vApplicationSettings["BackgroundDownloadIntervalMin"])
                    {
                        TileUpdateNeeded = true;
                        TileUpdateMessage = AVFunctions.StringAdd(TileUpdateMessage, "Bing", ",");
                    }
                }

                //Update the live tiles when needed
                if (TileUpdateNeeded)
                {
                    if (vBackgroundTaskStatus == 100)
                    {
                        txt_StatusBar.Text = "Updating " + TileUpdateMessage + ", please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                        await new TimeMeTaskAgent.ScheduledAgent().ForceRun().AsTask();
                        sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                        sp_StatusBar.Visibility = Visibility.Collapsed;
                        await CurrentTileLoad();
                        await WeatherLoad();
                    }
                }
            }
            catch { }
        }

        //Check if the application has location permission
        async Task CheckLocationPermission()
        {
            try
            {
                //Check for weather location permission when needed
                if ((bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadWeather"] && (bool)vApplicationSettings["WeatherGpsLocation"])
                {
                    try
                    {
                        Geolocator Geolocator = new Geolocator();
                        Geolocator.DesiredAccuracy = PositionAccuracy.Default;
                        Geoposition ResGeoposition = await Geolocator.GetGeopositionAsync(TimeSpan.FromDays(31), TimeSpan.FromSeconds(3));
                    }
                    catch (Exception Ex)
                    {
                        //Windows GPS startup error message bypass
                        if (!(Ex is TaskCanceledException))
                        {
                            Nullable<bool> MessageDialogResult = null;
                            MessageDialog MessageDialog = new MessageDialog("It seems like your device has the location service disabled, this will prevent the weather information from updating for your location click on 'Location Settings' to manually enable the location service or on 'Disable Weather' to disable the weather support, you can also use a manual location or turn the weather updates later on in the settings.", "TimeMe");
                            MessageDialog.Commands.Add(new UICommand("Location Settings", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                            MessageDialog.Commands.Add(new UICommand("Disable Weather", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                            await MessageDialog.ShowAsync();
                            if (MessageDialogResult == true)
                            {
                                Nullable<bool> MessageDialogResultGps = null;
                                MessageDialog MessageDialogGps = new MessageDialog("Please click on 'Use GPS' turn the 'Location' option on and allow TimeMe to enable weather updates for your location in the background, if this option is turned off the weather will not be updated you can also click on 'Use Manual' to use a manual set location, when you have turned one of the options on please restart the application to start the live tile updates.", "TimeMe");
                                MessageDialogGps.Commands.Add(new UICommand("Use GPS", new UICommandInvokedHandler((cmd) => MessageDialogResultGps = false)));
                                MessageDialogGps.Commands.Add(new UICommand("Use Manual", new UICommandInvokedHandler((cmd) => MessageDialogResultGps = true)));
                                await MessageDialogGps.ShowAsync();
                                if (MessageDialogResultGps == true)
                                {
                                    vApplicationSettings["WeatherGpsLocation"] = false;
                                    cb_SettingsWeatherGpsLocation.IsChecked = false;
                                    tab_SettingsWeatherNonGpsLocation.Visibility = Visibility.Visible;
                                    txtbox_SettingsWeatherNonGpsLocation.IsEnabled = true;

                                    lb_Menu.SelectedIndex = 8;
                                    lb_Menu_Tapped(null, null);
                                    await Task.Delay(250);

                                    lb_Settings.SelectedIndex = 9;
                                    lb_Settings_Tapped(null, null);
                                    await Task.Delay(250);

                                    txtbox_SettingsWeatherNonGpsLocation.Focus(FocusState.Programmatic);
                                    txtbox_SettingsWeatherNonGpsLocation.SelectionStart = txtbox_SettingsWeatherNonGpsLocation.Text.Length;
                                }
                                else
                                { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location")); }
                            }
                            else
                            {
                                //Disable download weather same as Settings.cs
                                vApplicationSettings["DownloadWeather"] = false;
                                cb_SettingsDownloadWeather.IsChecked = false;
                                cb_SettingsLockLocation.IsEnabled = false;
                                cb_SettingsLockWeather.IsEnabled = false;
                                cb_SettingsLockWeatherDetailed.IsEnabled = false;
                                cb_SettingsNotiWeatherCurrent.IsEnabled = false;
                                cb_SettingsWeatherGpsLocation.IsEnabled = false;
                                cb_SettingsWeatherDisplayWallpaper.IsEnabled = false;
                                txtbox_SettingsWeatherNonGpsLocation.IsEnabled = false;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        //Check for Background Task Progress
        async void BackgroundTaskProgress(IBackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            try
            {
                vBackgroundTaskStatus = args.Progress;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async delegate
                {
                    if (vBackgroundTaskStatus == 10)
                    {
                        txt_StatusBar.Text = "Updating download data, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                    }
                    else if (vBackgroundTaskStatus == 20)
                    {
                        txt_StatusBar.Text = "Lock screen is updating, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                    }
                    else if (vBackgroundTaskStatus == 30)
                    {
                        txt_StatusBar.Text = "Live tile is updating, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                    }
                    else if (vBackgroundTaskStatus == 40)
                    {
                        txt_StatusBar.Text = "Weather tile is updating, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                    }
                    else if (vBackgroundTaskStatus == 50)
                    {
                        txt_StatusBar.Text = "Battery tile is updating, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                    }
                    else if (vBackgroundTaskStatus == 90)
                    {
                        txt_StatusBar.Text = "Preparing background task, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                        sp_StatusBar.Visibility = Visibility.Collapsed;
                        await CurrentTileLoad();
                        await WeatherLoad();
                    }
                });
            }
            catch { }
        }

        //Stop/Start Background Task
        async void btn_LiveTileStartStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btn_LiveTileStartStop.Content.ToString() == "Stop Live Tile Background Updates")
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("Do you really want to stop the background updates? This will stop the live tiles, lock screen, Bing background, battery and weather information from updating, the next time you run TimeMe the updates will be turned on again.", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                    MessageDialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    await MessageDialog.ShowAsync();
                    if (MessageDialogResult == true)
                    {
                        txt_StatusBar.Text = "Disabling background tasks, please wait...";
                        sp_StatusBar.Visibility = Visibility.Visible;

                        btn_LiveTileStartStop.Content = "Start Live Tile Background Updates";
                        btn_LiveTileStartStop.IsEnabled = false; btn_LiveTileForceUpdate.IsEnabled = false; btn_DataForceUpdate.IsEnabled = false; btn_PinLiveTile.IsEnabled = false; btn_PinWeatherTile.IsEnabled = false; btn_PinBatteryTile.IsEnabled = false; btn_PinBlankTile.IsEnabled = false;

                        foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> BackgroundTask in BackgroundTaskRegistration.AllTasks) { BackgroundTask.Value.Unregister(true); }

                        //Reset primary tile
                        BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear();

                        //Reset secondary live tile
                        if (SecondaryTile.Exists("TimeMeLiveTile"))
                        {
                            TileUpdater Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeLiveTile");
                            foreach (ScheduledTileNotification Tile_Update in Tile_UpdateManager.GetScheduledTileNotifications()) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                            BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile("TimeMeLiveTile").Clear();

                            XmlDocument Tile_XmlContent = new XmlDocument();
                            Tile_XmlContent.LoadXml("<tile><visual branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoTaskDisabled.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoTaskDisabled.png\"/></binding></visual></tile>");
                            Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                        }

                        //Reset secondary weather tile
                        if (SecondaryTile.Exists("TimeMeWeatherTile"))
                        {
                            TileUpdater Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeWeatherTile");
                            foreach (ScheduledTileNotification Tile_Update in Tile_UpdateManager.GetScheduledTileNotifications()) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                            BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile("TimeMeWeatherTile").Clear();

                            XmlDocument Tile_XmlContent = new XmlDocument();
                            Tile_XmlContent.LoadXml("<tile><visual branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoTaskDisabled.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoTaskDisabled.png\"/></binding></visual></tile>");
                            Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                        }

                        //Reset secondary battery tile
                        if (SecondaryTile.Exists("TimeMeBatteryTile"))
                        {
                            TileUpdater Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeBatteryTile");
                            foreach (ScheduledTileNotification Tile_Update in Tile_UpdateManager.GetScheduledTileNotifications()) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                            BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile("TimeMeBatteryTile").Clear();

                            XmlDocument Tile_XmlContent = new XmlDocument();
                            Tile_XmlContent.LoadXml("<tile><visual branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoTaskDisabled.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoTaskDisabled.png\"/></binding></visual></tile>");
                            Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                        }

                        txt_bgTimeMeTaskUser.Text = "No";
                        txt_bgTimeMeTaskTimer.Text = "No";
                        txt_bgTimeMeTaskTimeZone.Text = "No";

                        btn_LiveTileStartStop.IsEnabled = true;
                        sp_StatusBar.Visibility = Visibility.Collapsed;
                    }
                }
                else { await TaskAgentLoad(); }
            }
            catch { }
        }

        //Force Background Task Update
        async void btn_LiveTileForceUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vBackgroundTaskStatus == 100)
                {
                    txt_StatusBar.Text = "Updating your live tiles, please wait...";
                    sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                    sp_StatusBar.Visibility = Visibility.Visible;
                    await new TimeMeTaskAgent.ScheduledAgent().ForceRun().AsTask();
                    sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                    sp_StatusBar.Visibility = Visibility.Collapsed;
                    await CurrentTileLoad();
                    await WeatherLoad();
                }
            }
            catch { }
        }

        //Update Download Data Information
        async void btn_DataForceUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if ((bool)vApplicationSettings["DownloadWifiOnly"])
                    {
                        try
                        {
                            uint IanaInterfaceType = NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.IanaInterfaceType;
                            if (IanaInterfaceType != 71 && IanaInterfaceType != 6)
                            {
                                await new MessageDialog("It seems like you are currently not connected to a Wi-Fi or Ethernet network, please connect to either one of those networks first and retry the download data update again.", "TimeMe").ShowAsync();
                                return;
                            }
                        }
                        catch { }
                    }

                    if (!(bool)vApplicationSettings["WeatherGpsLocation"] && String.IsNullOrEmpty(txtbox_SettingsWeatherNonGpsLocation.Text))
                    {
                        lb_Menu.SelectedIndex = 8;
                        lb_Menu_Tapped(null, null);
                        await Task.Delay(250);

                        lb_Settings.SelectedIndex = 9;
                        lb_Settings_Tapped(null, null);
                        await Task.Delay(250);

                        txtbox_SettingsWeatherNonGpsLocation.Focus(FocusState.Programmatic);
                        txtbox_SettingsWeatherNonGpsLocation.SelectionStart = txtbox_SettingsWeatherNonGpsLocation.Text.Length;
                        return;
                    }

                    if (vBackgroundTaskStatus == 100)
                    {
                        BackgroundStatusUpdateSettings("Never", "Never", "Never", "Never", "Never");
                        txt_StatusBar.Text = "Updating download data, please wait...";
                        sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;
                        sp_StatusBar.Visibility = Visibility.Visible;
                        await new TimeMeTaskAgent.ScheduledAgent().ForceRun().AsTask();
                        sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                        sp_StatusBar.Visibility = Visibility.Collapsed;
                        await CurrentTileLoad();
                        await WeatherLoad();
                    }
                }
                else { await new MessageDialog("It seems like you are currently not connected to any internet network, please connect to an internet network first and retry the download data update again.", "TimeMe").ShowAsync(); }
            }
            catch { }
        }

        //Pin the TimeMe live tile
        async void btn_PinLiveTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "TimeMeLiveTile";
                if (!SecondaryTile.Exists(SecondaryTileId))
                {
                    sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;

                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "TimeMe Tile", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/SquareLogoUpdate.png"), TileSize.Wide310x150);
                    Pin_SecondaryTile.RoamingEnabled = false;
                    Pin_SecondaryTile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Tiles/SmallLogoSize.png");
                    Pin_SecondaryTile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Tiles/WideLogoUpdate.png");
                    Pin_SecondaryTile.LockScreenDisplayBadgeAndTileText = false;
                    Pin_SecondaryTile.VisualElements.BackgroundColor = Colors.Transparent;
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned)
                    {
                        if (vBackgroundTaskStatus == 100)
                        {
                            txt_StatusBar.Text = "Updating your live tile, please wait...";
                            sp_StatusBar.Visibility = Visibility.Visible;
                            await new TimeMeTaskAgent.ScheduledAgent().ForceRun().AsTask();
                            await CurrentTileLoad();
                            await WeatherLoad();
                        }

                        txt_LiveTileStatus.Text = "The TimeMe live tile is currently pinned.";
                        btn_PinLiveTile.Content = "Unpin TimeMe Time Live Tile from Start";
                        await new MessageDialog("The TimeMe Time Live tile has successfully been pinned and updated.", "TimeMe").ShowAsync();
                    }

                    sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                    sp_StatusBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("The TimeMe Time Live tile is currently pinned, do you want to unpin the live tile?", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Unpin", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = null)));
                    await MessageDialog.ShowAsync();

                    //Unpin live tile
                    if (MessageDialogResult == false)
                    {
                        bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                        if (UnPinned)
                        {
                            txt_LiveTileStatus.Text = "Please tap on 'Pin TimeMe Time Live Tile' button below to begin your new live tile experience, you might need to set the used live tile size.";
                            btn_PinLiveTile.Content = "Pin the TimeMe Time Live Tile to Start";
                            await CurrentTileLoad();
                            await new MessageDialog("The TimeMe Time Live tile has successfully been unpinned.", "TimeMe").ShowAsync();
                        }
                    }
                }
            }
            catch { await new MessageDialog("Failed to pin, unpin or update the TimeMe Time Live Tile, please try again.", "TimeMe").ShowAsync(); }
        }

        //Pin the TimeMe Weather tile
        async void btn_PinWeatherTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "TimeMeWeatherTile";
                if (!SecondaryTile.Exists(SecondaryTileId))
                {
                    sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;

                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "TimeMe Weather", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/SquareLogoUpdate.png"), TileSize.Wide310x150);
                    Pin_SecondaryTile.RoamingEnabled = false;
                    Pin_SecondaryTile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Tiles/SmallLogoSize.png");
                    Pin_SecondaryTile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Tiles/WideLogoUpdate.png");
                    Pin_SecondaryTile.LockScreenDisplayBadgeAndTileText = false;
                    Pin_SecondaryTile.VisualElements.BackgroundColor = Colors.Transparent;
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned)
                    {
                        if (vBackgroundTaskStatus == 100)
                        {
                            txt_StatusBar.Text = "Updating your weather tile, please wait...";
                            sp_StatusBar.Visibility = Visibility.Visible;
                            await new TimeMeTaskAgent.ScheduledAgent().ForceRun().AsTask();
                            await CurrentTileLoad();
                            await WeatherLoad();
                        }

                        btn_PinWeatherTile.Content = "Unpin TimeMe Weather Tile from Start";
                        await new MessageDialog("The TimeMe Weather tile has successfully been pinned and updated.", "TimeMe").ShowAsync();
                    }

                    sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                    sp_StatusBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("The TimeMe Weather tile is currently pinned, do you want to unpin the weather tile?", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Unpin", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = null)));
                    await MessageDialog.ShowAsync();

                    //Unpin live tile
                    if (MessageDialogResult == false)
                    {
                        bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                        if (UnPinned)
                        {
                            btn_PinWeatherTile.Content = "Pin the TimeMe Weather Tile to Start";
                            await new MessageDialog("The TimeMe Weather tile has successfully been unpinned.", "TimeMe").ShowAsync();
                        }
                    }
                }
            }
            catch { await new MessageDialog("Failed to pin, unpin or update the TimeMe Weather Tile, please try again.", "TimeMe").ShowAsync(); }
        }

        //Pin the TimeMe Battery tile
        async void btn_PinBatteryTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "TimeMeBatteryTile";
                if (!SecondaryTile.Exists(SecondaryTileId))
                {
                    sp_tab_Tile.IsHitTestVisible = false; sp_tab_Tile.Opacity = 0.60;

                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "TimeMe Battery", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/SquareLogoUpdate.png"), TileSize.Square150x150);
                    Pin_SecondaryTile.RoamingEnabled = false;
                    Pin_SecondaryTile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Tiles/SmallLogoSize.png");
                    Pin_SecondaryTile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Tiles/WideLogoUpdate.png");
                    Pin_SecondaryTile.LockScreenDisplayBadgeAndTileText = false;
                    Pin_SecondaryTile.VisualElements.BackgroundColor = Colors.Transparent;
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned)
                    {
                        if (vBackgroundTaskStatus == 100)
                        {
                            txt_StatusBar.Text = "Updating your battery tile, please wait...";
                            sp_StatusBar.Visibility = Visibility.Visible;
                            await new TimeMeTaskAgent.ScheduledAgent().ForceRun().AsTask();
                            await CurrentTileLoad();
                            await WeatherLoad();
                        }

                        btn_PinBatteryTile.Content = "Unpin TimeMe Battery Tile from Start";
                        await new MessageDialog("The TimeMe Battery tile has successfully been pinned and updated.", "TimeMe").ShowAsync();
                    }

                    sp_tab_Tile.IsHitTestVisible = true; sp_tab_Tile.Opacity = 1;
                    sp_StatusBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("The TimeMe Battery tile is currently pinned, do you want to unpin the battery tile?", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Unpin", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = null)));
                    await MessageDialog.ShowAsync();

                    //Unpin live tile
                    if (MessageDialogResult == false)
                    {
                        bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                        if (UnPinned)
                        {
                            btn_PinBatteryTile.Content = "Pin the TimeMe Battery Tile to Start";
                            await new MessageDialog("The TimeMe Battery tile has successfully been unpinned.", "TimeMe").ShowAsync();
                        }
                    }
                }
            }
            catch { await new MessageDialog("Failed to pin, unpin or update the TimeMe Battery Tile, please try again.", "TimeMe").ShowAsync(); }
        }

        //Pin Blank Tile to Start
        async void btn_PinBlankTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri BlankBackground = new Uri("ms-appx:///Assets/Tiles/TimeMeTileBlank.png");
                string SecondaryTileId = "CloseApp" + new Random().Next(0, 9999999);
                if (!SecondaryTile.Exists(SecondaryTileId))
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "TimeMe Blank Tile", SecondaryTileId, BlankBackground, TileSize.Square150x150);
                    Pin_SecondaryTile.RoamingEnabled = false;
                    Pin_SecondaryTile.VisualElements.Wide310x150Logo = BlankBackground;
                    Pin_SecondaryTile.VisualElements.Square310x310Logo = BlankBackground;
                    Pin_SecondaryTile.LockScreenDisplayBadgeAndTileText = false;
                    Pin_SecondaryTile.VisualElements.BackgroundColor = Colors.Transparent;
                    await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                }
            }
            catch { await new MessageDialog("Failed to pin a new blank tile, please try again.", "TimeMe").ShowAsync(); }
        }

        //Pin Sleeping Screen Live Tile
        async void btn_PinSleepingScreen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "SleepingScreen";
                if (!SecondaryTile.Exists(SecondaryTileId))
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "TimeMe Sleeping Screen", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/SquareLogoSleepingScreen.png"), TileSize.Square150x150);
                    Pin_SecondaryTile.RoamingEnabled = false;
                    Pin_SecondaryTile.LockScreenDisplayBadgeAndTileText = false;
                    Pin_SecondaryTile.VisualElements.BackgroundColor = Colors.Transparent;
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned)
                    {
                        btn_PinSleepingScreen.Content = "Unpin the Sleeping Screen tile";
                        await new MessageDialog("The TimeMe Sleeping Screen tile has successfully been pinned.", "TimeMe").ShowAsync();
                    }
                }
                else
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("The Sleeping Screen tile is currently pinned, do you want to unpin the Sleeping Screen tile?", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Unpin", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = null)));
                    await MessageDialog.ShowAsync();

                    //Unpin live tile
                    if (MessageDialogResult == false)
                    {
                        bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                        if (UnPinned)
                        {
                            btn_PinSleepingScreen.Content = "Pin the Sleeping Screen tile";
                            await new MessageDialog("The TimeMe Sleeping Screen tile has successfully been unpinned.", "TimeMe").ShowAsync();
                        }
                    }
                }
            }
            catch { await new MessageDialog("Failed to pin, unpin or update the TimeMe Sleeping Screen, please try again.", "TimeMe").ShowAsync(); }
        }

        //Pin Flashlight Live Tile
        async void btn_PinFlashlight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string SecondaryTileId = "FlashLight";
                if (!SecondaryTile.Exists(SecondaryTileId))
                {
                    SecondaryTile Pin_SecondaryTile = new SecondaryTile(SecondaryTileId, "TimeMe Flashlight", SecondaryTileId, new Uri("ms-appx:///Assets/Tiles/SquareLogoFlashlight.png"), TileSize.Square150x150);
                    Pin_SecondaryTile.RoamingEnabled = false;
                    Pin_SecondaryTile.LockScreenDisplayBadgeAndTileText = false;
                    Pin_SecondaryTile.VisualElements.BackgroundColor = Colors.Transparent;
                    bool Pinned = await Pin_SecondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                    if (Pinned)
                    {
                        btn_PinFlashlight.Content = "Unpin the Flashlight tile";
                        await new MessageDialog("The TimeMe Flashlight tile has successfully been pinned.", "TimeMe").ShowAsync();
                    }
                }
                else
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("The Flashlight tile is currently pinned, do you want to unpin the Flashlight tile?", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Unpin", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = null)));
                    await MessageDialog.ShowAsync();

                    //Unpin live tile
                    if (MessageDialogResult == false)
                    {
                        bool UnPinned = await new SecondaryTile(SecondaryTileId).RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
                        if (UnPinned)
                        {
                            btn_PinFlashlight.Content = "Pin the Flashlight tile";
                            await new MessageDialog("The TimeMe Flashlight tile has successfully been unpinned.", "TimeMe").ShowAsync();
                        }
                    }
                }
            }
            catch { await new MessageDialog("Failed to pin, unpin or update the TimeMe Flashlight, please try again.", "TimeMe").ShowAsync(); }
        }
    }
}