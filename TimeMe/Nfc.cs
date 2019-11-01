using System;
using Windows.ApplicationModel;
using Windows.Networking.Proximity;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeMe
{
    partial class MainPage
    {
        //Write to NFC Tag
        async void SettingsWriteNFCTag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProximityDevice vProximityDevice = ProximityDevice.GetDefault();
                if (vProximityDevice != null)
                {
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("After this message hold your NFC tag to this device's NFC area so the tag can be written.", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Continue", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    await MessageDialog.ShowAsync();
                    if (MessageDialogResult == true)
                    {
                        Button Button = (Button)sender;
                        string LaunchArgs = Button.Tag.ToString();
                        Set_NFCTags.Opacity = 0.60; Set_NFCTags.IsHitTestVisible = false;

                        //Create 10 seconds timeout timer
                        int TimeoutTime = 0;
                        DispatcherTimer TimeoutTimer = new DispatcherTimer();
                        TimeoutTimer.Interval = TimeSpan.FromSeconds(1);
                        TimeoutTimer.Tick += delegate
                        {
                            TimeoutTime++;
                            if (TimeoutTime >= 11) { vProximityDevice = null; TimeoutTimer.Stop(); Set_NFCTags.Opacity = 1; Set_NFCTags.IsHitTestVisible = true; sp_StatusBar.Visibility = Visibility.Collapsed; }
                            else { txt_StatusBar.Text = "Waiting on NFC tag for " + (11 - TimeoutTime).ToString() + "sec..."; sp_StatusBar.Visibility = Visibility.Visible; }
                        };
                        TimeoutTimer.Start();

                        //Start checking for NFC tag if NFC device is active
                        vProximityDevice.DeviceArrived += async delegate
                        {
                            if (vProximityDevice != null)
                            {
                                string WinAppId = Package.Current.Id.FamilyName + "!" + "App";
                                string AppMsg = LaunchArgs + "\tWindows\t" + WinAppId;

                                using (DataWriter DataWriter = new DataWriter { UnicodeEncoding = UnicodeEncoding.Utf16LE })
                                {
                                    DataWriter.WriteString(AppMsg);
                                    long PublishBinaryMessage = vProximityDevice.PublishBinaryMessage("LaunchApp:WriteTag", DataWriter.DetachBuffer());
                                    vProximityDevice.StopPublishingMessage(PublishBinaryMessage);
                                    vProximityDevice = null;

                                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async delegate
                                    {
                                        TimeoutTimer.Stop();
                                        Set_NFCTags.Opacity = 1; Set_NFCTags.IsHitTestVisible = true;
                                        sp_StatusBar.Visibility = Visibility.Collapsed;
                                        await new MessageDialog("The NFC tag has successfully been written to your tag, please note that this might not work with all NFC tags due to incompatibility issues or with some tags that are locked.", "TimeMe").ShowAsync();
                                    });
                                }
                            }
                        };
                    }
                }
                else
                {
                    vProximityDevice = null;
                    Nullable<bool> MessageDialogResult = null;
                    MessageDialog MessageDialog = new MessageDialog("It seems like your device's NFC is disabled or does not support NFC, please make sure NFC is turned on before writing a new NFC tag, do you want to enable your device's NFC functionality?", "TimeMe");
                    MessageDialog.Commands.Add(new UICommand("Enable", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                    MessageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler((cmd) => MessageDialogResult = false)));
                    await MessageDialog.ShowAsync();
                    if (MessageDialogResult == true) { await Launcher.LaunchUriAsync(new Uri("ms-settings:proximity")); }
                }
            }
            catch (Exception Ex) { await new MessageDialog("NFCError: " + Ex.Message, "TimeMe").ShowAsync(); }
        }
    }
}