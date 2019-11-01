using ArnoldVinkCode;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeMe
{
    sealed partial class App : Application
    {
        //Application launch variables
        public static string vLaunchTileActivatedCommand = "";
        public static string vLaunchVoiceActivatedCommand = "";
        public static string vLaunchVoiceActivatedSpoken = "";
        public static string vApplicationLaunchArgs = "";
        public static bool vApplicationFirstLaunch = true;
        public static Rect vLaunchDisplayResolution;

        //Handle application unhandled exception
        public App()
        {
            try
            {
                UnhandledException += async (sender, e) =>
                {
                    if (e != null)
                    {
                        e.Handled = true;
                        Nullable<bool> MessageDialogResult = null;
                        MessageDialog MessageDialog = new MessageDialog("Sadly enough something went wrong in the application, if the application no longer works like it should you can try to restart the application, do you want to try to continue or close the application?\n\nThe application received the following error message: " + e.Message, "TimeMe");
                        MessageDialog.Commands.Add(new UICommand("Continue", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                        MessageDialog.Commands.Add(new UICommand("Close App", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                        await MessageDialog.ShowAsync();

                        if (MessageDialogResult == true) { return; } else { Application.Current.Exit(); }
                    }
                };
            }
            catch { }
        }

        //Handle application launching
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                //Set launch commands to string
                vApplicationLaunchArgs = args.Arguments;
                vLaunchTileActivatedCommand = args.TileId;
                vLaunchVoiceActivatedCommand = "";
                vLaunchVoiceActivatedSpoken = "";

                //Check the launch commands for close app
                if (vApplicationLaunchArgs.StartsWith("CloseApp")) { Application.Current.Exit(); }
                else if (vLaunchTileActivatedCommand.StartsWith("CloseApp")) { Application.Current.Exit(); }

                //Check if app has launched for the first time
                if (vApplicationFirstLaunch)
                {
                    //Start initializing user interface
                    Frame Frame = new Frame();
                    Frame.Navigate(typeof(MainPage));
                    Window.Current.Content = Frame;
                    Window.Current.Activate();

                    //Set current display resolution
                    if (AVFunctions.DevMobile()) { vLaunchDisplayResolution = Window.Current.Bounds; }
                    else { vLaunchDisplayResolution = ApplicationView.GetForCurrentView().VisibleBounds; }

                    //Set application launch to false
                    vApplicationFirstLaunch = false;
                }
                else { OnApplicationActivated(null, null); }
            }
            catch { }
        }

        //Handle application activation
        protected override void OnActivated(IActivatedEventArgs args)
        {
            try
            {
                //Set launch commands to string
                vApplicationLaunchArgs = args.Kind.ToString();
                vLaunchTileActivatedCommand = "";
                if (args.Kind == ActivationKind.VoiceCommand)
                {
                    vLaunchVoiceActivatedCommand = ((VoiceCommandActivatedEventArgs)args).Result.RulePath[0];
                    vLaunchVoiceActivatedSpoken = ((VoiceCommandActivatedEventArgs)args).Result.Text;
                }

                //Check the launch commands for close app
                if (vApplicationLaunchArgs.StartsWith("CloseApp")) { Application.Current.Exit(); }
                else if (vLaunchTileActivatedCommand.StartsWith("CloseApp")) { Application.Current.Exit(); }
                else if (vLaunchVoiceActivatedCommand.StartsWith("CloseApp")) { Application.Current.Exit(); }
                else if (vLaunchVoiceActivatedSpoken.StartsWith("CloseApp")) { Application.Current.Exit(); }

                //Check if app has launched for the first time
                if (vApplicationFirstLaunch)
                {
                    //Start initializing user interface
                    Frame Frame = new Frame();
                    Frame.Navigate(typeof(MainPage));
                    Window.Current.Content = Frame;
                    Window.Current.Activate();

                    //Set current display resolution
                    if (AVFunctions.DevMobile()) { vLaunchDisplayResolution = Window.Current.Bounds; }
                    else { vLaunchDisplayResolution = ApplicationView.GetForCurrentView().VisibleBounds; }

                    //Set application launch to false
                    vApplicationFirstLaunch = false;
                }
                else { OnApplicationActivated(null, null); }
            }
            catch { }
        }

        //Create application activated event
        public static event EventHandler OnApplicationActivated;
    }
}