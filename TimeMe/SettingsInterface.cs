using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TimeMe
{
    partial class MainPage
    {
        //Menu Settings Functions
        async void lb_Settings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (lb_Settings.SelectedIndex >= 0 && ((StackPanel)lb_Settings.SelectedItem).IsHitTestVisible)
                {
                    StackPanel SelStackPanel = (StackPanel)lb_Settings.SelectedItem;
                    if (SelStackPanel.Name == "MenuSet_TileDisplay")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        AVAnimations.Ani_Visibility(Set_TileDisplay, true, true, 0.25);
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_LiveSizeStyle")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_LiveSizeStyle, true, true, 0.25);
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_WeatherSizeStyle")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_WeatherSizeStyle, true, true, 0.25);
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_BatterySizeStyle")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_BatterySizeStyle, true, true, 0.25);
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_FontFamily")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_FontFamily, true, true, 0.25);
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_FontColor")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_FontColor, true, true, 0.25);
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;

                        if (!lb_FontColorList.Items.Any())
                        {
                            List<ColorList> ColorList = new List<ColorList>();
                            ColorList.Add(new ColorList() { Color = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"] });
                            foreach (uint uintColor in uintColors) { ColorList.Add(new ColorList() { Color = new SolidColorBrush(Color.FromArgb((byte)(uintColor >> 24), (byte)(uintColor >> 16), (byte)(uintColor >> 8), (byte)(uintColor))) }); };
                            lb_FontColorList.ItemsSource = ColorList;
                        }
                    }
                    else if (SelStackPanel.Name == "MenuSet_Background")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_Background, true, true, 0.25);
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;

                        await CurrentBackgroundLoad();
                    }
                    else if (SelStackPanel.Name == "MenuSet_BackgroundColor")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_BackgroundColor, true, true, 0.25);

                        if (!lb_BackgroundColorList.Items.Any())
                        {
                            List<ColorList> ColorList = new List<ColorList>();
                            ColorList.Add(new ColorList() { Color = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"] });
                            foreach (uint uintColor in uintColors) { ColorList.Add(new ColorList() { Color = new SolidColorBrush(Color.FromArgb((byte)(uintColor >> 24), (byte)(uintColor >> 16), (byte)(uintColor >> 8), (byte)(uintColor))) }); };
                            lb_BackgroundColorList.ItemsSource = ColorList;
                        }
                    }
                    else if (SelStackPanel.Name == "MenuSet_BackgroundPhoto")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(0, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        await Tile_SelectBackgroundPhoto();
                    }
                    else if (SelStackPanel.Name == "MenuSet_Weather")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_Weather, true, true, 0.25);
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_Lockscreen")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_Lockscreen, true, true, 0.25);
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_Notification")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_Notification, true, true, 0.25);
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_SleepFlash")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_SleepFlash, true, true, 0.25);
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_NfcTags")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_NFCTags, true, true, 0.25);
                        Set_Download.Visibility = Visibility.Collapsed;
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_Download")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_Download, true, true, 0.25);
                        Set_Other.Visibility = Visibility.Collapsed;
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                    else if (SelStackPanel.Name == "MenuSet_Other")
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"])
                        {
                            sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                            sv_tab_Settings.UpdateLayout();
                        }
                        sv_tab_Settings_Right.ChangeView(null, 0, null);
                        sv_tab_Settings_Right.UpdateLayout();

                        Set_TileDisplay.Visibility = Visibility.Collapsed;
                        Set_Background.Visibility = Visibility.Collapsed;
                        Set_Weather.Visibility = Visibility.Collapsed;
                        Set_Lockscreen.Visibility = Visibility.Collapsed;
                        Set_Notification.Visibility = Visibility.Collapsed;
                        Set_SleepFlash.Visibility = Visibility.Collapsed;
                        Set_NFCTags.Visibility = Visibility.Collapsed;
                        Set_Download.Visibility = Visibility.Collapsed;
                        AVAnimations.Ani_Visibility(Set_Other, true, true, 0.25);
                        Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                        Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                        Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                        Set_FontFamily.Visibility = Visibility.Collapsed;
                        Set_FontColor.Visibility = Visibility.Collapsed;
                        Set_BackgroundColor.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch { }
        }

        //Handle settings back button tap
        void img_Settings_Back_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Settings.ChangeView(0, null, null);
                    sv_tab_Settings.UpdateLayout();
                }
                sv_tab_Settings_Right.ChangeView(null, 0, null);
                sv_tab_Settings_Right.UpdateLayout();
            }
            catch { }
        }

        //Save Tile Size and Style Selection
        async void lb_ListLiveTileStyle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                foreach (FrameworkElement FrameworkElement in Set_LiveSizeStyle.Children)
                {
                    if (FrameworkElement is StackPanel && !String.IsNullOrEmpty(FrameworkElement.Name)) { ((StackPanel)FrameworkElement).Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; }
                    else if (FrameworkElement is StackPanel)
                    {
                        foreach (FrameworkElement FrameworkElement2 in ((StackPanel)FrameworkElement).Children)
                        { if (!String.IsNullOrEmpty(FrameworkElement2.Name)) { ((StackPanel)FrameworkElement2).Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; } }
                    }
                }

                StackPanel Name_StackPanel = (StackPanel)sender;
                Name_StackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                vApplicationSettings["LiveTileSizeName"] = Name_StackPanel.Name;

                //Update the possible text positions tile preview
                if (await AVFunctions.AppFileExists("Assets/Preview/" + Name_StackPanel.Name + ".png")) { img_TextPositionPreview.Source = new BitmapImage(new Uri("ms-appx:///Assets/Preview/" + Name_StackPanel.Name + ".png", UriKind.Absolute)); }
                else { img_TextPositionPreview.Source = new BitmapImage(new Uri("ms-appx:///Assets/Preview/" + Name_StackPanel.Name + ".gif", UriKind.Absolute)); }

                if (Name_StackPanel.Tag != null && Name_StackPanel.Tag.ToString() == "Light") { vApplicationSettings["LiveTileSizeLight"] = true; }
                else { vApplicationSettings["LiveTileSizeLight"] = false; }

                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Settings.ChangeView(0, null, null);
                    sv_tab_Settings.UpdateLayout();
                }
            }
            catch { }
        }

        //Save Weather Size and Style Selection
        void lb_ListWeatherTileStyle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                foreach (FrameworkElement FrameworkElement in Set_WeatherSizeStyle.Children)
                {
                    if (FrameworkElement is StackPanel && !String.IsNullOrEmpty(FrameworkElement.Name)) { ((StackPanel)FrameworkElement).Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; }
                    else if (FrameworkElement is StackPanel)
                    {
                        foreach (FrameworkElement FrameworkElement2 in ((StackPanel)FrameworkElement).Children)
                        { if (!String.IsNullOrEmpty(FrameworkElement2.Name)) { ((StackPanel)FrameworkElement2).Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; } }
                    }
                }

                StackPanel Name_StackPanel = (StackPanel)sender;
                Name_StackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                vApplicationSettings["WeatherTileSizeName"] = Name_StackPanel.Name;

                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Settings.ChangeView(0, null, null);
                    sv_tab_Settings.UpdateLayout();
                }
            }
            catch { }
        }

        //Save Battery Size and Style Selection
        void lb_ListBatteryTileStyle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                foreach (FrameworkElement FrameworkElement in Set_BatterySizeStyle.Children)
                {
                    if (FrameworkElement is StackPanel && !String.IsNullOrEmpty(FrameworkElement.Name)) { ((StackPanel)FrameworkElement).Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; }
                    else if (FrameworkElement is StackPanel)
                    {
                        foreach (FrameworkElement FrameworkElement2 in ((StackPanel)FrameworkElement).Children)
                        { if (!String.IsNullOrEmpty(FrameworkElement2.Name)) { ((StackPanel)FrameworkElement2).Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]; } }
                    }
                }

                StackPanel Name_StackPanel = (StackPanel)sender;
                Name_StackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                vApplicationSettings["BatteryTileSizeName"] = Name_StackPanel.Name;

                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Settings.ChangeView(0, null, null);
                    sv_tab_Settings.UpdateLayout();
                }
            }
            catch { }
        }

        //Save Tile Font Family Selection
        void lb_FontList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                foreach (FrameworkElement FrameworkElement in Set_FontFamily.Children)
                {
                    if (FrameworkElement is StackPanel && !String.IsNullOrEmpty(FrameworkElement.Name))
                    {
                        ((StackPanel)FrameworkElement).Background = new SolidColorBrush(Colors.Transparent);
                        foreach (FrameworkElement FrameworkElement2 in ((StackPanel)FrameworkElement).Children)
                        { if (!String.IsNullOrEmpty(FrameworkElement2.Name)) { ((TextBlock)FrameworkElement2).Foreground = new SolidColorBrush(Colors.Black); } }
                    }
                }

                StackPanel Name_StackPanel = (StackPanel)sender;
                foreach (FrameworkElement FrameworkElement in Name_StackPanel.Children)
                {
                    if (FrameworkElement is TextBlock && !String.IsNullOrEmpty(FrameworkElement.Name))
                    {
                        Name_StackPanel.Background = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));
                        ((TextBlock)FrameworkElement).Foreground = new SolidColorBrush(Colors.White);
                        vApplicationSettings["LiveTileFont"] = Name_StackPanel.Tag;
                    }
                }

                if ((bool)vApplicationSettings["DevStatusMobile"])
                {
                    sv_tab_Settings.ChangeView(0, null, null);
                    sv_tab_Settings.UpdateLayout();
                }
            }
            catch { }
        }

        //Save Tile Font Color Selection
        async void lb_ColorFontList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                foreach (object SelItem in lb_FontColorList.SelectedItems)
                {
                    ColorList SelectedItem = (ColorList)SelItem;

                    if (vApplicationSettings["LiveTileColorBackground"].ToString() == SelectedItem.Color.Color.ToString())
                    { await new MessageDialog("You cannot select the same background and font color at the same time, please select a different live tile font color.", "TimeMe").ShowAsync(); return; }

                    if (!(bool)vApplicationSettings["DisplayBackgroundColor"] && !(bool)vApplicationSettings["DisplayBackgroundPhoto"] && SelectedItem.Color.Color.ToString() == ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString())
                    { await new MessageDialog("You have selected the same color as your transparent tile accent color, please select a different live tile font color.", "TimeMe").ShowAsync(); return; }

                    vApplicationSettings["LiveTileColorFont"] = SelectedItem.Color.Color.ToString();

                    if (vApplicationSettings["LiveTileColorFont"].ToString() != ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString()) { MenuSet_FontColorPreview.Foreground = new SolidColorBrush(Color.FromArgb(Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(1, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(3, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(5, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorFont"].ToString().Substring(7, 2), 16))); }
                    else { MenuSet_FontColorPreview.Foreground = new SolidColorBrush(Colors.White); }

                    if ((bool)vApplicationSettings["DevStatusMobile"])
                    {
                        sv_tab_Settings.ChangeView(0, null, null);
                        sv_tab_Settings.UpdateLayout();
                    }
                }
            }
            catch { }
        }

        //Save Tile Background Color Selection
        async void lb_ListBackgroundColor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                foreach (object SelItem in lb_BackgroundColorList.SelectedItems)
                {
                    ColorList SelectedItem = (ColorList)SelItem;

                    if (vApplicationSettings["LiveTileColorFont"].ToString() == SelectedItem.Color.Color.ToString())
                    { await new MessageDialog("You cannot select the same background and font color at the same time, please select a different live tile background color.", "TimeMe").ShowAsync(); return; }

                    if (!(bool)vApplicationSettings["DisplayBackgroundColor"] && !(bool)vApplicationSettings["DisplayBackgroundPhoto"] && SelectedItem.Color.Color.ToString() == ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString())
                    { await new MessageDialog("You have selected the same color as your transparent tile accent color, please select a different live tile background color.", "TimeMe").ShowAsync(); return; }

                    vApplicationSettings["LiveTileColorBackground"] = SelectedItem.Color.Color.ToString();

                    if (vApplicationSettings["LiveTileColorBackground"].ToString() != ((SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"]).Color.ToString()) { MenuSet_BackgroundColorPreview.Foreground = new SolidColorBrush(Color.FromArgb(Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(1, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(3, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(5, 2), 16), Convert.ToByte(vApplicationSettings["LiveTileColorBackground"].ToString().Substring(7, 2), 16))); }
                    else { MenuSet_BackgroundColorPreview.Foreground = new SolidColorBrush(Colors.White); }

                    //Render background color to small color image
                    try
                    {
                        can_BackgroundColor.Background = SelectedItem.Color;
                        RenderTargetBitmap RenderTargetBitmap = new RenderTargetBitmap();
                        await RenderTargetBitmap.RenderAsync(can_BackgroundColor);
                        StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeTileColor.png", CreationCollisionOption.ReplaceExisting);
                        using (IRandomAccessStream OpenAsync = await CreateFileAsync.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            BitmapEncoder BitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, OpenAsync);
                            BitmapEncoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)RenderTargetBitmap.PixelWidth, (uint)RenderTargetBitmap.PixelHeight, 96, 96, (await RenderTargetBitmap.GetPixelsAsync()).ToArray());
                            await BitmapEncoder.FlushAsync();
                        }
                    }
                    catch { }

                    if ((bool)vApplicationSettings["DevStatusMobile"])
                    {
                        sv_tab_Settings.ChangeView(0, null, null);
                        sv_tab_Settings.UpdateLayout();
                    }
                }
            }
            catch { }
        }

        //Show and hide the text position preview
        async void btn_TextPositionPreview_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if (grid_TextPositionPreview.Visibility == Visibility.Collapsed)
                {
                    btn_TextPositionPreview.Content = "Hide possible time tile text positions";

                    //Update the possible text positions tile preview
                    if (await AVFunctions.AppFileExists("Assets/Preview/" + vApplicationSettings["LiveTileSizeName"].ToString() + ".png")) { img_TextPositionPreview.Source = new BitmapImage(new Uri("ms-appx:///Assets/Preview/" + vApplicationSettings["LiveTileSizeName"].ToString() + ".png", UriKind.Absolute)); }
                    else { img_TextPositionPreview.Source = new BitmapImage(new Uri("ms-appx:///Assets/Preview/" + vApplicationSettings["LiveTileSizeName"].ToString() + ".gif", UriKind.Absolute)); }

                    grid_TextPositionPreview.Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"];
                    grid_TextPositionPreview.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_TextPositionPreview.Content = "Show possible time tile text positions";
                    grid_TextPositionPreview.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        //Open Project Website
        async void SettingsOpenProjectWebsite_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)vApplicationSettings["DevStatusMobile"]) { await Launcher.LaunchUriAsync(new Uri("http://m.arnoldvink.com/?p=projects")); }
            else { await Launcher.LaunchUriAsync(new Uri("http://projects.arnoldvink.com")); }
        }

        //Open Donation Page
        async void SettingsOpenDonationPage_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)vApplicationSettings["DevStatusMobile"]) { await Launcher.LaunchUriAsync(new Uri("http://m.arnoldvink.com/?p=donation")); }
            else { await Launcher.LaunchUriAsync(new Uri("http://donation.arnoldvink.com")); }
        }

        //Open Privacy Policy
        async void SettingsOpenPrivacyPolicy_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("http://privacy.arnoldvink.com")); }

        //Open NFC Settings
        async void SettingsOpenNFCSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:proximity")); }

        //Open Background Apps Settings
        async void SettingsOpenBackgroundAppsSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-backgroundapps")); }

        //Open Startscreen Settings
        async void SettingsOpenStartscreenSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:personalization-start")); }

        //Open Lockscreen Settings
        async void SettingsOpenLockscreenSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:lockscreen")); }

        //Open Calendar Settings
        async void SettingsOpenCalendarSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-calendar")); }

        //Open Location Settings
        async void SettingsOpenLocationSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location")); }

        //Open Battery Settings
        async void SettingsOpenBatterySettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:batterysaver-usagedetails")); }

        //Open WiFi Settings
        async void SettingsOpenWiFiSettings_Click(object sender, RoutedEventArgs e)
        { await Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi")); }

        //Load the current background photo
        async Task CurrentBackgroundLoad()
        {
            try
            {
                if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                {
                    btn_SaveLiveTileBackground.Visibility = Visibility.Visible;
                    txt_LiveTileBackground.Text = "Your background photo looks like this:";
                    img_LiveTileBackground.Visibility = Visibility.Visible;
                    StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMeTilePhoto.png");
                    using (IRandomAccessStream OpenAsync = await StorageFile.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapImage BitmapImage = new BitmapImage();
                        await BitmapImage.SetSourceAsync(OpenAsync);
                        OpenAsync.Dispose();
                        img_LiveTileBackground.Source = BitmapImage;
                    }
                    grid_LiveTileBackground.Background = (SolidColorBrush)Resources["SystemControlBackgroundAccentBrush"];
                }
                else
                {
                    btn_SaveLiveTileBackground.Visibility = Visibility.Collapsed;
                    txt_LiveTileBackground.Text = "No background photo preview available.";
                    img_LiveTileBackground.Visibility = Visibility.Collapsed;
                }
            }
            catch { Debug.WriteLine("Failed to load the background photo."); }
        }

        //Save the current background photo to device
        async void CurrentBackgroundSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                {
                    FileSavePicker FileSavePicker = new FileSavePicker();
                    FileSavePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    FileSavePicker.FileTypeChoices.Add("JPG Image", new List<string>() { ".jpg" });
                    FileSavePicker.FileTypeChoices.Add("PNG Image", new List<string>() { ".png" });

                    if (vApplicationSettings["BgStatusPhotoName"].ToString() == "N/A") { FileSavePicker.SuggestedFileName = "TimeMe Background Photo"; }
                    else { FileSavePicker.SuggestedFileName = vApplicationSettings["BgStatusPhotoName"].ToString(); }

                    StorageFile NewFile = await FileSavePicker.PickSaveFileAsync();
                    StorageFile BackgroundFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMeTilePhoto.png");
                    await BackgroundFile.CopyAndReplaceAsync(NewFile);
                }
            }
            catch { Debug.WriteLine("Failed to save the background photo."); }
        }

        //Background and Fonts Colors List
        public class ColorList { public SolidColorBrush Color { get; set; } }
        uint[] uintColors =
        {
            //Grayscale
            0xFF000000,0xFF222222,0xFFFFFFFF,0xFFEEEEEE,0xFFD3D3D3,0xFFA9A9A9,

            //Colors
            0xFFFFFF00,0xFFFFE135,0xFFF3DB32,0xFFF8DE7E,0xFFADFF2F,0xFFB2CF5D,0xFF9CB651,0xFFC1D881,0xFF10C96F,
            0xFF00FF00,0xFF7FFF00,0xFF32CD32,0xFF00FF7F,0xFF90EE90,0xFF42B54F,
            0xFF3CB371,0xFF2E8B57,0xFF008000,0xFF548F3B,0xFF0B8645,0xFF808000,0xFFFF0000,
            0xFFED2939,0xFF800000,0xFFA52A2A,0xFFB22222,0xFFDC143C,
            0xFFFF8C00,0xFFFFA500,0xFFEDBA47,0xFFD2691E,0xFFFF7F50,0xFFF4A460,
            0xFFD6A572,0xFFC9915F,0xFF744030,0xFF8C4A2E,0xFF572316,0xFF24160F,0xFFB2373F,0xFF9B3F53,
            0xFFCD5C5C,0xFFF08080,0xFFFFB6C1,0xFFFFA07A,0xFFFF1493,
            0xFFFF69B4,0xFFFF00FF,0xFFC71585,0xFF800080,0xFF120B23,0xFF4B0082,
            0xFF8A2BE2,0xFFDA70D6,0xFFDB7093,0xFF76608A,0xFF483D8B,
            0xFF000080,0xFF0000FF,0xFF6495ED,0xFF00C7FF,0xFF00BFFF,
            0xFF1E90FF,0xFFADD8E6,0xFF87CEFA,0xFF006FD5,0xFF7B68EE,
            0xFF6A5ACD,0xFF4169E1,0xFF708090,0xFF4682B4,
            0xFF008080,0xFF217283,0xFF40E0D0,0xFF20B2AA,0xFFA0B3C0,0xFF15B0FC
        };

        //Tile Background Photo Picker
        async Task Tile_SelectBackgroundPhoto()
        {
            try
            {
                FileOpenPicker FileOpenPicker = new FileOpenPicker();
                FileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
                FileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                FileOpenPicker.FileTypeFilter.Add(".jpg");
                FileOpenPicker.FileTypeFilter.Add(".jpeg");
                FileOpenPicker.FileTypeFilter.Add(".png");

                StorageFile StorageFile = await FileOpenPicker.PickSingleFileAsync();
                if (StorageFile != null)
                {
                    //Save background photo locally
                    await StorageFile.CopyAndReplaceAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeTilePhoto.png", CreationCollisionOption.ReplaceExisting));
                    vApplicationSettings["BgStatusPhotoName"] = StorageFile.DisplayName;

                    //Load background photo preview
                    await CurrentBackgroundLoad();
                    await WeatherLoad();

                    //Set photo as lockscreen and wallpaper
                    await UpdateDeviceWallpaper();

                    //Disable bing wallpaper after selecting a new background
                    vApplicationSettings["DownloadBingWallpaper"] = false;
                    cb_SettingsDownloadBingWallpaper.IsChecked = false;

                    //Open the Background Setting Tab
                    if ((bool)vApplicationSettings["DevStatusMobile"])
                    {
                        sv_tab_Settings.ChangeView(tab_Settings.ActualWidth, null, null);
                        sv_tab_Settings.UpdateLayout();
                    }

                    sv_tab_Settings_Right.ChangeView(null, sv_tab_Settings_Right.ScrollableHeight, null);
                    sv_tab_Settings_Right.UpdateLayout();
                    lb_Settings.SelectedIndex = 6;

                    Set_TileDisplay.Visibility = Visibility.Collapsed;
                    AVAnimations.Ani_Visibility(Set_Background, true, true, 0.25);
                    Set_Weather.Visibility = Visibility.Collapsed;
                    Set_Lockscreen.Visibility = Visibility.Collapsed;
                    Set_Notification.Visibility = Visibility.Collapsed;
                    Set_SleepFlash.Visibility = Visibility.Collapsed;
                    Set_Download.Visibility = Visibility.Collapsed;
                    Set_Other.Visibility = Visibility.Collapsed;
                    Set_LiveSizeStyle.Visibility = Visibility.Collapsed;
                    Set_WeatherSizeStyle.Visibility = Visibility.Collapsed;
                    Set_BatterySizeStyle.Visibility = Visibility.Collapsed;
                    Set_FontFamily.Visibility = Visibility.Collapsed;
                    Set_FontColor.Visibility = Visibility.Collapsed;
                    Set_BackgroundColor.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        //Set background photo as device wallpaper and lockscreen
        async Task UpdateDeviceWallpaper()
        {
            try
            {
                if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png") && ((bool)vApplicationSettings["DeviceWallpaper"] || (bool)vApplicationSettings["LockWallpaper"]))
                {
                    StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMeTilePhoto.png" + new String(' ', new Random().Next(1, 50)));
                    if ((bool)vApplicationSettings["DeviceWallpaper"]) { await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(StorageFile); }
                    if ((bool)vApplicationSettings["LockWallpaper"])
                    {
                        if ((bool)vApplicationSettings["DevStatusMobile"]) { await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Tiles/TimeMeTileColor.png"))); }
                        await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(StorageFile);
                    }
                }
            }
            catch { }
        }
    }
}