using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TimeMe
{
    partial class MainPage
    {
        //Handle application page loading
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            try
            {
                Loaded += async delegate
                {
                    //Set application startup arguments
                    if (args.Parameter != null) { App.vApplicationLaunchArgs = args.Parameter.ToString(); }

                    //Handle application page startup
                    await ApplicationStartup();
                };
            }
            catch { }
        }

        //Handle application page leaving
        protected override void OnNavigatedFrom(NavigationEventArgs args)
        {
            try
            {
                //Disable application events and timers
                ApplicationTimersDisable();
                ApplicationEventsDisable();
            }
            catch { }
        }

        //Handle application suspending
        void OnSuspending(object sender, SuspendingEventArgs args)
        {
            try
            {
                SuspendingDeferral SuspendingDeferral = args.SuspendingOperation.GetDeferral();
                Debug.WriteLine("Suspending application");
                SuspendingDeferral.Complete();
            }
            catch { }
        }

        //Handle application resuming
        void OnResuming(object sender, object args)
        {
            try
            {
                Debug.WriteLine("Resuming application");
            }
            catch { }
        }

        //Handle entered background 
        void OnEnteredBackground(object sender, object args)
        {
            try
            {
                Debug.WriteLine("Entered to background");
            }
            catch { }
        }

        //Handle leaving background 
        async void OnLeavingBackground(object sender, object args)
        {
            try
            {
                Debug.WriteLine("Resuming from background");

                //Check if live tile update is needed
                await CheckForBackgroundUpdate();

                //Update the tile page information
                StackPanel SelStackPanel = (StackPanel)lb_Menu.SelectedItem;
                if (SelStackPanel.Name == "menuButtonTile")
                {
                    WeekNumberLoad();
                    await BatteryLevelLoad();
                    await CurrentTileLoad();
                }
            }
            catch { }
        }

        //Handle application page startup
        async Task ApplicationStartup()
        {
            try
            {
                //Set Application Styles
                this.RequestedTheme = ElementTheme.Light;
                if (AVFunctions.DevMobile())
                {
                    //Set Phone StatusBar
                    vStatusBar = StatusBar.GetForCurrentView();
                    vStatusBar.ForegroundColor = Colors.White;
                    vStatusBar.BackgroundColor = Color.FromArgb(255, 29, 29, 29);
                    vStatusBar.BackgroundOpacity = 1;
                    await vStatusBar.ShowAsync();

                    //Set Application Display Orientation
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped;

                    //Enable or disable device specific user interface
                    grid_TileLogo.Visibility = Visibility.Collapsed;
                    grid_CountdownLogo.Visibility = Visibility.Collapsed;
                    grid_StopwatchLogo.Visibility = Visibility.Collapsed;
                    grid_TimerLogo.Visibility = Visibility.Collapsed;
                    grid_WorldLogo.Visibility = Visibility.Collapsed;
                    grid_SettingsLogo.Visibility = Visibility.Collapsed;
                    grid_HelpLogo.Visibility = Visibility.Collapsed;

                    //Set Application Menu Orientation - Vertical
                    grid_Main.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid_Main.RowDefinitions.Add(new RowDefinition { Height = new GridLength(90, GridUnitType.Pixel) });
                    page_Menu.SetValue(Grid.RowProperty, 1);
                    page_Main.SetValue(Grid.RowProperty, 0);
                    lb_Menu.Style = (Style)App.Current.Resources["ListBoxHorizontal"];

                    //Storyboard - Show menu swipe hint animation
                    AVAnimations.Ani_SwipeHintHorizontal(lb_Menu, -25);
                }
                else
                {
                    ////Set Desktop TitleBar
                    //vTitleBar = vApplicationView.TitleBar;
                    //vTitleBar.ButtonBackgroundColor = Color.FromArgb(255, 29, 29, 29);
                    //vTitleBar.BackgroundColor = Color.FromArgb(255, 29, 29, 29);
                    //vTitleBar.ButtonForegroundColor = Colors.White;
                    //vTitleBar.ForegroundColor = Colors.White;

                    //Set Application Minimum Window Size
                    vApplicationView.SetPreferredMinSize(new Size(1024, 768));

                    //Set Application Display Orientation
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped;

                    //Enable or disable device specific user interface
                    sv_tab_Tile.HorizontalScrollMode = ScrollMode.Disabled;
                    sv_tab_Tile.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    sv_tab_Settings.HorizontalScrollMode = ScrollMode.Disabled;
                    sv_tab_Settings.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    img_Settings_Back.Visibility = Visibility.Collapsed;
                    img_Tile_Forward.Visibility = Visibility.Collapsed;
                    img_Tile_Back.Visibility = Visibility.Collapsed;

                    cb_SettingShowMoreTiles.Content = "Is option 'Start full screen' disabled?";

                    //Set Application Menu Orientation - Horizontal
                    grid_Main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90, GridUnitType.Pixel) });
                    grid_Main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    page_Menu.SetValue(Grid.ColumnProperty, 0);
                    page_Main.SetValue(Grid.ColumnProperty, 1);
                    lb_Menu.Style = (Style)App.Current.Resources["ListBoxVertical"];

                    //Storyboard - Show menu swipe hint animation
                    AVAnimations.Ani_SwipeHintVertical(lb_Menu, -25);
                }

                //Load Application Settings
                await SettingsCheck();
                await SettingsLoad();
                await SettingsSave();

                //Handle application navigation
                await ApplicationNavigation();

                //Enable or disable build specific user interface
                if (AVFunctions.DevOsVersion() < 14393)
                {
                    cb_SettingsNotiBatterySaver.Visibility = Visibility.Collapsed;
                    txt_SettingsNotiBatterySaver.Visibility = Visibility.Collapsed;
                }

                //Register application events and timers
                ApplicationTimersRegister();
                ApplicationEventsRegister();

                //Check pinned live tiles
                PinnedTileStatusLoad();

                //Load Application Background Task
                bool TaskRunning = await TaskAgentLoad();

                //Enable the user interface
                grid_Main.Opacity = 1;
                grid_Main.IsHitTestVisible = true;

                //Check if live tile update is needed
                if (TaskRunning) { await CheckForBackgroundUpdate(); }

                //Register Cortana voice commands
                await RegisterCortanaCommands();
            }
            catch { }
        }

        //Handle application navigation
        async Task ApplicationNavigation()
        {
            try
            {
                //NFC Startup
                if (App.vApplicationLaunchArgs == "Slp") { Frame.Navigate(typeof(SleepingScreen)); }
                else if (App.vApplicationLaunchArgs == "Fls") { Frame.Navigate(typeof(FlashlightScreen)); }
                //Toast/Noti Startup
                else if (App.vApplicationLaunchArgs == "ToastTimer") { lb_Menu.SelectedIndex = 4; lb_Menu_Tapped(null, null); }
                else if (App.vApplicationLaunchArgs == "NotiBatterySaver") { SettingsOpenBatterySettings_Click(null, null); lb_Menu.SelectedIndex = 0; lb_Menu_Tapped(null, null); }
                //Page Return
                else if (App.vApplicationLaunchArgs == "tab_Tile") { lb_Menu.SelectedIndex = 0; lb_Menu_Tapped(null, null); }
                else if (App.vApplicationLaunchArgs == "tab_Settings") { lb_Menu.SelectedIndex = 8; lb_Menu_Tapped(null, null); }
                //Tile Startup
                else if (App.vLaunchTileActivatedCommand == "SleepingScreen") { Frame.Navigate(typeof(SleepingScreen)); }
                else if (App.vLaunchTileActivatedCommand == "FlashLight") { Frame.Navigate(typeof(FlashlightScreen)); }
                //Voice Startup
                else if (App.vLaunchVoiceActivatedCommand == "Flashlight") { Frame.Navigate(typeof(FlashlightScreen)); }
                else if (App.vLaunchVoiceActivatedCommand == "SleepingScreen") { Frame.Navigate(typeof(SleepingScreen)); }
                else if (App.vLaunchVoiceActivatedCommand == "Weather") { lb_Menu.SelectedIndex = 1; lb_Menu_Tapped(null, null); }
                else if (App.vLaunchVoiceActivatedCommand == "Countdown") { lb_Menu.SelectedIndex = 2; lb_Menu_Tapped(null, null); }
                else if (App.vLaunchVoiceActivatedCommand == "Stopwatch") { lb_Menu.SelectedIndex = 3; lb_Menu_Tapped(null, null); }
                else if (App.vLaunchVoiceActivatedCommand == "Timer") { lb_Menu.SelectedIndex = 4; lb_Menu_Tapped(null, null); }
                else if (App.vLaunchVoiceActivatedCommand == "World") { lb_Menu.SelectedIndex = 5; lb_Menu_Tapped(null, null); }
                else if (App.vLaunchVoiceActivatedCommand == "Settings") { lb_Menu.SelectedIndex = 8; lb_Menu_Tapped(null, null); }
                else if (App.vLaunchVoiceActivatedCommand == "Info") { lb_Menu.SelectedIndex = 0; lb_Menu_Tapped(null, null); await CortanaVoiceSpeech(); }
                else if (App.vLaunchVoiceActivatedCommand == "Speech") { lb_Menu.SelectedIndex = 0; lb_Menu_Tapped(null, null); await CortanaVoiceSpeech(); }
                //Normal Startup
                else
                {
                    switch ((int)vApplicationSettings["StartupTab"])
                    {
                        case 0: { lb_Menu.SelectedIndex = 0; lb_Menu_Tapped(null, null); break; }
                        case 1: { if (await AVFunctions.LocalFileExists("TimeMeWeatherSummary.js") && (bool)vApplicationSettings["BackgroundDownload"] && (bool)vApplicationSettings["DownloadWeather"]) { lb_Menu.SelectedIndex = 1; lb_Menu_Tapped(null, null); } else { lb_Menu.SelectedIndex = 0; lb_Menu_Tapped(null, null); } break; }
                        case 2: { lb_Menu.SelectedIndex = 3; lb_Menu_Tapped(null, null); break; }
                        case 3: { lb_Menu.SelectedIndex = 4; lb_Menu_Tapped(null, null); break; }
                        case 4: { lb_Menu.SelectedIndex = 5; lb_Menu_Tapped(null, null); break; }
                        case 5: { lb_Menu.SelectedIndex = 8; lb_Menu_Tapped(null, null); break; }
                        case 6: { Frame.Navigate(typeof(SleepingScreen)); break; }
                        case 7: { Frame.Navigate(typeof(FlashlightScreen)); break; }
                        case 8: { lb_Menu.SelectedIndex = 2; lb_Menu_Tapped(null, null); break; }
                    }
                }

                //Reset the application startup variables
                App.vLaunchTileActivatedCommand = "";
                App.vLaunchVoiceActivatedCommand = "";
                App.vLaunchVoiceActivatedSpoken = "";
                App.vApplicationLaunchArgs = "";
                return;
            }
            catch { }
        }
    }
}