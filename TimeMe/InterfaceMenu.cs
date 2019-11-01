using ArnoldVinkCode;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TimeMe
{
    partial class MainPage
    {
        //Handle main menu tapped
        async void lb_Menu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (lb_Menu.SelectedIndex >= 0 && ((StackPanel)lb_Menu.SelectedItem).IsHitTestVisible)
                {
                    StackPanel SelStackPanel = (StackPanel)lb_Menu.SelectedItem;
                    if (SelStackPanel.Name == "menuButtonTile")
                    {
                        WeekNumberLoad();
                        await BatteryLevelLoad();
                        await CurrentTileLoad();
                        AVAnimations.Ani_Visibility(tab_Tile, true, true, 0.25);
                        tab_Weather.Visibility = Visibility.Collapsed;
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        tab_Timer.Visibility = Visibility.Collapsed;
                        tab_World.Visibility = Visibility.Collapsed;
                        tab_Settings.Visibility = Visibility.Collapsed;
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonWeather")
                    {
                        await WeatherLoad();
                        tab_Tile.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_Weather, true, true, 0.25);
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        tab_Timer.Visibility = Visibility.Collapsed;
                        tab_World.Visibility = Visibility.Collapsed;
                        tab_Settings.Visibility = Visibility.Collapsed;
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonCountdown")
                    {
                        await CountdownLoad();
                        tab_Tile.Visibility = Visibility.Collapsed;
                        tab_Weather.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_Countdown, true, true, 0.25);
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        tab_Timer.Visibility = Visibility.Collapsed;
                        tab_World.Visibility = Visibility.Collapsed;
                        tab_Settings.Visibility = Visibility.Collapsed;
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonStopwatch")
                    {
                        await StopwatchLoad();
                        tab_Tile.Visibility = Visibility.Collapsed;
                        tab_Weather.Visibility = Visibility.Collapsed;
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_Stopwatch, true, true, 0.25);
                        tab_Timer.Visibility = Visibility.Collapsed;
                        tab_World.Visibility = Visibility.Collapsed;
                        tab_Settings.Visibility = Visibility.Collapsed;
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonTimer")
                    {
                        TimersLoad();
                        tab_Tile.Visibility = Visibility.Collapsed;
                        tab_Weather.Visibility = Visibility.Collapsed;
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_Timer, true, true, 0.25);
                        tab_World.Visibility = Visibility.Collapsed;
                        tab_Settings.Visibility = Visibility.Collapsed;
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonSleepingScreen") { Frame.Navigate(typeof(SleepingScreen)); }
                    else if (SelStackPanel.Name == "menuButtonFlashLight") { Frame.Navigate(typeof(FlashlightScreen)); }
                    else if (SelStackPanel.Name == "menuButtonWorld")
                    {
                        WorldLoad();
                        tab_Tile.Visibility = Visibility.Collapsed;
                        tab_Weather.Visibility = Visibility.Collapsed;
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        tab_Timer.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_World, true, true, 0.25);
                        tab_Settings.Visibility = Visibility.Collapsed;
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonSettings")
                    {
                        ////Reset settings scrollviewer location
                        //if ((bool)vApplicationSettings["DevStatusMobile"])
                        //{
                        //    sv_tab_Settings.ChangeView(0, null, null);
                        //    sv_tab_Settings.UpdateLayout();
                        //}
                        //sv_tab_Settings_Right.ChangeView(null, 0, null);
                        //sv_tab_Settings_Right.UpdateLayout();

                        tab_Tile.Visibility = Visibility.Collapsed;
                        tab_Weather.Visibility = Visibility.Collapsed;
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        tab_Timer.Visibility = Visibility.Collapsed;
                        tab_World.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_Settings, true, true, 0.25);
                        tab_Help.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "menuButtonHelp")
                    {
                        HelpLoad();
                        tab_Tile.Visibility = Visibility.Collapsed;
                        tab_Weather.Visibility = Visibility.Collapsed;
                        tab_Countdown.Visibility = Visibility.Collapsed;
                        tab_Stopwatch.Visibility = Visibility.Collapsed;
                        tab_Timer.Visibility = Visibility.Collapsed;
                        tab_World.Visibility = Visibility.Collapsed;
                        tab_Settings.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(tab_Help, true, true, 0.25);
                    }
                }
            }
            catch { }
        }
    }
}