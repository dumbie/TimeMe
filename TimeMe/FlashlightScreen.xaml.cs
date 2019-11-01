using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace TimeMe
{
    public sealed partial class FlashlightScreen : Page
    {
        //Application Variables
        IDictionary<string, object> vApplicationSettings = ApplicationData.Current.LocalSettings.Values;
        int CurrentColor = 0;
        Point StartingPoint;

        public FlashlightScreen()
        {
            try
            {
                this.InitializeComponent();
                Loaded += async delegate
                {
                    //Handle Device Back Button
                    SystemNavigationManager.GetForCurrentView().BackRequested += FlashBackButtonPressed;

                    //Hide Phone StatusBar
                    await HideStatusBar();

                    //Enter fullscreen mode
                    if (!MainPage.vApplicationView.IsFullScreenMode) { MainPage.vApplicationView.TryEnterFullScreenMode(); }

                    //Prevent application lock screen
                    try { MainPage.vDisplayRequest.RequestActive(); } catch { }

                    //Monitor user touch swipe
                    ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

                    //Enable the camera output
                    if ((bool)vApplicationSettings["FlashCameraOutput"]) { await EnableFlashlight(); }
                    else
                    {
                        grid_Flashbackground.Visibility = Visibility.Visible;
                        ce_FlashlightBox.Visibility = Visibility.Collapsed;
                    }
                };
            }
            catch { }
        }

        //Hide Status Bar
        async Task HideStatusBar() { if ((bool)vApplicationSettings["DevStatusMobile"]) { await MainPage.vStatusBar.HideAsync(); } }

        //Turn Flashlight on and off
        MediaCapture MediaCapture;
        bool FlashlightOnOff = false;
        async Task EnableFlashlight()
        {
            try
            {
                if (!FlashlightOnOff)
                {
                    FlashlightOnOff = true;
                    bool FlashlightSupport = true;
                    DeviceInformationCollection DeviceIC = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                    if (DeviceIC.Count > 0)
                    {
                        string UsedDeviceIC = "";
                        if (DeviceIC.Count > 1) { UsedDeviceIC = DeviceIC[1].Id; }
                        else { UsedDeviceIC = DeviceIC[0].Id; }

                        MediaCapture = new MediaCapture();
                        await MediaCapture.InitializeAsync(new MediaCaptureInitializationSettings() { StreamingCaptureMode = StreamingCaptureMode.Video, VideoDeviceId = UsedDeviceIC });
                        if (MediaCapture.VideoDeviceController.TorchControl.Supported)
                        {
                            MediaCapture.VideoDeviceController.TorchControl.Enabled = true;
                            if (MediaCapture.VideoDeviceController.TorchControl.PowerSupported)
                            { MediaCapture.VideoDeviceController.TorchControl.PowerPercent = 100; }
                        }
                        else
                        { FlashlightSupport = false; }

                        if (!FlashlightSupport)
                        {
                            FlashlightOnOff = false;
                            MediaCapture.Dispose();
                            grid_Flashbackground.Visibility = Visibility.Visible;
                            ce_FlashlightBox.Visibility = Visibility.Collapsed;
                            return;
                        }
                        else
                        {
                            ce_FlashlightBox.Source = MediaCapture;
                            MediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
                            await MediaCapture.StartPreviewAsync();
                        }
                    }
                    else
                    {
                        FlashlightOnOff = false;
                        grid_Flashbackground.Visibility = Visibility.Visible;
                        ce_FlashlightBox.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                else
                {
                    FlashlightOnOff = false;
                    await MediaCapture.StopPreviewAsync();
                    MediaCapture.Dispose();
                }
            }
            catch (Exception Ex)
            {
                FlashlightOnOff = false;
                MediaCapture.Dispose();
                if (Ex is UnauthorizedAccessException)
                {
                    await new MessageDialog("It seems like your device does not have permission to use your webcams flashlight please try to open the flashlight again.", "TimeMe").ShowAsync();
                    //Close fullscreen mode
                    MainPage.vApplicationView.ExitFullScreenMode();
                    //Allow application lock screen
                    try { MainPage.vDisplayRequest.RequestRelease(); } catch { }
                    Frame.Navigate(typeof(MainPage), "tab_Tile");
                }
                else
                {
                    await new MessageDialog("It seems like your device does not support a constant on flashlight or something else has gone wrong.\n\nError Message: " + Ex.Message, "TimeMe").ShowAsync();
                    //Close fullscreen mode
                    MainPage.vApplicationView.ExitFullScreenMode();
                    //Allow application lock screen
                    try { MainPage.vDisplayRequest.RequestRelease(); } catch { }
                    Frame.Navigate(typeof(MainPage), "tab_Tile");
                }
            }
        }

        //Handle user double taps
        async void Page_DoubleTap(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {
                if ((bool)vApplicationSettings["ScreenDoubleTapStart"])
                {
                    //Stop Flashlight/Camera
                    if (FlashlightOnOff)
                    {
                        FlashlightOnOff = false;
                        await MediaCapture.StopPreviewAsync();
                        MediaCapture.Dispose();
                    }
                    Application.Current.Exit();
                    return;
                }
                else
                {
                    //Stop Flashlight/Camera
                    if (FlashlightOnOff)
                    {
                        FlashlightOnOff = false;
                        await MediaCapture.StopPreviewAsync();
                        MediaCapture.Dispose();
                    }
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
        async void FlashBackButtonPressed(object sender, BackRequestedEventArgs e)
        {
            try
            {
                if (Frame.CurrentSourcePageType.Name == "FlashlightScreen")
                {
                    if ((bool)vApplicationSettings["ScreenBackButton"])
                    {
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        e.Handled = true;
                        //Stop Flashlight/Camera
                        if (FlashlightOnOff)
                        {
                            FlashlightOnOff = false;
                            await MediaCapture.StopPreviewAsync();
                            MediaCapture.Dispose();
                        }
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

        //Handle user single tap
        void Page_Tap(object sender, TappedRoutedEventArgs e)
        {
            if (grid_Flashlightscreen.Opacity == 1) { Ani_Opacity(grid_Flashlightscreen, 0.75); return; }
            if (grid_Flashlightscreen.Opacity == 0.75) { Ani_Opacity(grid_Flashlightscreen, 0.50); return; }
            if (grid_Flashlightscreen.Opacity == 0.50) { Ani_Opacity(grid_Flashlightscreen, 0.25); return; }
            if (grid_Flashlightscreen.Opacity == 0.25) { Ani_Opacity(grid_Flashlightscreen, 1); return; }
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

        //Handle user touch swipe
        void Page_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) { StartingPoint = e.Position; }

        void Page_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Point CurrentPoint = e.Position;
            if ((CurrentPoint.X - StartingPoint.X >= 175) || (CurrentPoint.Y - StartingPoint.Y >= 175))
            {
                if (CurrentColor == 0) { CurrentColor = 1; grid_Flashbackground.Background = new SolidColorBrush(Colors.Red); return; }
                if (CurrentColor == 1) { CurrentColor = 2; grid_Flashbackground.Background = new SolidColorBrush(Colors.Green); return; }
                if (CurrentColor == 2) { CurrentColor = 3; grid_Flashbackground.Background = new SolidColorBrush(Colors.Blue); return; }
                if (CurrentColor == 3) { CurrentColor = 4; grid_Flashbackground.Background = new SolidColorBrush(Colors.Yellow); return; }
                if (CurrentColor == 4) { CurrentColor = 0; grid_Flashbackground.Background = new SolidColorBrush(Colors.White); return; }
            }
        }
    }
}