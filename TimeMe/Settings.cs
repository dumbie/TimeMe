using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace TimeMe
{
    partial class MainPage
    {
        //Show the last set debug message in popup
        async void ShowDebugMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await new MessageDialog("Debug message: " + vApplicationSettings["AppDebugMsg"].ToString(), "TimeMe").ShowAsync();
            }
            catch { }
        }

        //Update Background Status Settings
        void BackgroundStatusUpdateSettings(string DownloadWeather, string DownloadLocation, string DownloadBing, string BingDescription, string AppDebugMsg)
        {
            try
            {
                //Always set to Failed, Never or DateTime
                if (!String.IsNullOrEmpty(DownloadWeather))
                {
                    string BgStatusWeatherProvider = vApplicationSettings["BgStatusWeatherProvider"].ToString();
                    if (!BgStatusWeatherProvider.EndsWith("!")) { vApplicationSettings["BgStatusWeatherProvider"] = BgStatusWeatherProvider + "!"; }

                    string BgStatusWeatherCurrent = vApplicationSettings["BgStatusWeatherCurrent"].ToString();
                    if (!BgStatusWeatherCurrent.EndsWith("!")) { vApplicationSettings["BgStatusWeatherCurrent"] = BgStatusWeatherCurrent + "!"; }

                    string BgStatusWeatherCurrentTemp = vApplicationSettings["BgStatusWeatherCurrentTemp"].ToString();
                    if (!BgStatusWeatherCurrentTemp.EndsWith("!")) { vApplicationSettings["BgStatusWeatherCurrentTemp"] = BgStatusWeatherCurrentTemp + "!"; }

                    vApplicationSettings["BgStatusDownloadWeatherTime"] = DownloadWeather;
                }

                //Always set to Never or DateTime
                if (!String.IsNullOrEmpty(DownloadLocation))
                {
                    string BgStatusWeatherCurrentLocationShort = vApplicationSettings["BgStatusWeatherCurrentLocationShort"].ToString();
                    if (!BgStatusWeatherCurrentLocationShort.EndsWith("!")) { vApplicationSettings["BgStatusWeatherCurrentLocationShort"] = BgStatusWeatherCurrentLocationShort + "!"; }

                    string BgStatusWeatherCurrentLocationFull = vApplicationSettings["BgStatusWeatherCurrentLocationFull"].ToString();
                    if (!BgStatusWeatherCurrentLocationFull.EndsWith("!")) { vApplicationSettings["BgStatusWeatherCurrentLocationFull"] = BgStatusWeatherCurrentLocationFull + "!"; }

                    vApplicationSettings["BgStatusDownloadLocation"] = DownloadLocation;
                }

                //Always set to Never or DateTime
                if (!String.IsNullOrEmpty(DownloadBing)) { vApplicationSettings["BgStatusDownloadBing"] = DownloadBing; }

                //Always set to current Bing Description
                if (!String.IsNullOrEmpty(BingDescription)) { vApplicationSettings["BgStatusBingDescription"] = BingDescription; }

                //Set an application debug message
                if (!String.IsNullOrEmpty(AppDebugMsg)) { vApplicationSettings["AppDebugMsg"] = AppDebugMsg; }
            }
            catch { }
        }

        //Reset TimeMe status and All Files
        async void btn_ResetTimeMeStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Nullable<bool> MessageDialogResult = null;
                MessageDialog MessageDialog = new MessageDialog("Do you really want to reset the TimeMe status and remove all files which will reset TimeMe to it's defaults? This is only recommended when TimeMe has stopped updating the live tile.\n\nAfter the application has been reset to it's defaults TimeMe will be closed and will need to be run again manually before live tile updates start to work.", "TimeMe");
                MessageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                MessageDialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                await MessageDialog.ShowAsync();
                if (MessageDialogResult == true)
                {
                    grid_Main.Opacity = 0.60;
                    grid_Main.IsHitTestVisible = false;
                    txt_StatusBar.Text = "Resetting TimeMe, please wait...";
                    sp_StatusBar.Visibility = Visibility.Visible;

                    //Stop all background tasks
                    foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> BackgroundTask in BackgroundTaskRegistration.AllTasks)
                    { BackgroundTask.Value.Unregister(true); }

                    //Reset application settings
                    foreach (KeyValuePair<string, object> AppSetting in ApplicationData.Current.LocalSettings.Values)
                    { ApplicationData.Current.LocalSettings.Values.Remove(AppSetting.Key); }

                    //Delete all files from local storage
                    foreach (IStorageItem LocalFile in await ApplicationData.Current.LocalFolder.GetItemsAsync())
                    { try { await LocalFile.DeleteAsync(StorageDeleteOption.PermanentDelete); } catch { } }

                    //Unpin all the live tiles
                    foreach (SecondaryTile SecondaryTile in await SecondaryTile.FindAllAsync())
                    { await SecondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below); }

                    //Clear all notification messages
                    ToastNotificationManager.History.Clear();

                    Application.Current.Exit();
                }
            }
            catch { Application.Current.Exit(); }
        }
    }
}