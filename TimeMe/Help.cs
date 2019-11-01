using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeMe
{
    partial class MainPage
    {
        //Load - Help text
        void HelpLoad()
        {
            try
            {
                if (!sp_Help.Children.Any())
                {
                    sp_Help.Children.Add(new TextBlock() { Text = "How to get started with TimeMe?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "After launching TimeMe, pin a live tile by clicking on the 'Pin TimeMe Live Tile' button and set the live tile to the selected tile size, after you have done this it should start automatically updating, when you unlock your device it might take a little while before the live tile gets updated.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nHelp, my new saved settings don't work.", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "The new settings will start to work on the next live tile update so it might take up to 15 minutes before it changes to your new settings, the data on the tiles and lock screen can only update every 15 minutes so the weather, battery level and timer alarm information might be inaccurate at times.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nHelp, my live tile is no longer updating.", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When the battery saver is on background tasks may not get run which will lead to a non up-to-date live tile, you can manually allow TimeMe to keep on updating while the battery saver is on in the 'Battery Saver' option in the 'All Settings' screen by adding TimeMe to your 'Always allowed' apps.\r\n\r\nWhen your device uses much resources like disk load, cpu or memory usage due to limitations background tasks might be skipped and will lead to a non up-to-date or blank live tile until the next update.\r\n\r\nWhen the application has been updated to a newer version the background task may get paused and keep the live tile from updating until you manually restart your device or open the app.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nHow can I enable the lock screen features?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Goto your device's 'All Settings' screen and click on 'Personalization' now open 'Lock screen' and set TimeMe as the detailed and quick status app and you are ready to go, the lock screen information might be outdated by 15 minutes, the 'T' in the calendar stands for Tomorrow, only 4 text lines can be displayed on the lock screen so maybe not all selected features may be displayed at all times.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nHow much bandwidth does TimeMe use?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Refreshing the weather costs around 8KB and gets refreshed every ~2 hours, so this means the application will use around 100KB a day when you are not connected to a Wi-Fi network, the daily Bing background download size is around 300KB depending on your set download resolution setting.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhen does my current location update?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Your location will automatically update to your current GPS location when the weather gets updated, if your location is not recognized the weather may show up as N/A, if your device does not support GPS it will try to use your last known IP address location or the manually set location when the usage of the GPS capability is disabled.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nHelp, the weather shows up as N/A or with a !", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Make sure that you have a working internet connection and the location service also known as GPS is enabled, once both are enabled and working the weather will be updated on the next planned live tile update when available at the weather provider.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhat is the Sleeping Screen feature?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "The Sleeping Screen is made to make your device usefull during the night in a dock, it shows the current time, weather and battery level with a black background to save your battery power, when you tap once on the screen you can adjust the brightness and when you tap twice the Sleeping Screen will be closed or close the app, swiping from left to right will switch the background color between black and white.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nFlashlight extra feature information", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "You can control the flashlights screen brightness by tapping on the screen, if your device does not support a flashlight you can swipe from left to right to change the background colors between Red, Green, Blue, Yellow and White.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhy can I only select one live tile size?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "You can only select one live tile size and style to save your device's battery life time by rendering just one live tile size and style at the time, it is also to stay within the strict applications background resource limitation.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhat sizes are used for the live tile photo?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When you are creating an image or photo for your live tile background it must be (W)300x(H)300 or (W)620x(H)300 to fit perfectly on the tiles otherwise the picture might get cropped or resized.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWhat does the 'Bing daily background' setting do?", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "This option will download the daily Bing background and set it as your live tile background every day, the background photo is a fairly big file so keep in mind that your bandwidth usage will increase when you enable this setting.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nHelp, I get a 'Failed to render live tile' error", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When you receive this error on the tiles open the application and restart the background updates by clicking on 'Stop Live Tile Background Updates' and start the updates again, if this didn't work try reinstalling the application and make sure you have atleast 10mb storage space left on your device.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nCortana voice command support", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "This application also supports various Cortana voice commands, you can check them all out by telling 'What can I say' to Cortana to see a list of capable application commands.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nSupport and bug reporting", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "When you are walking into any problems or bugs you can goto the support forum on: http://forum.arnoldvink.com so I can try to help you out and get everything working.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nDevelopment donation support", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Feel free to make a donation to support me with my developing projects, you can find a donation page on the project website or click below on the donation button to open the donation page.", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nWeather information provider", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "The weather is provided by Microsoft and their weather partner " + vApplicationSettings["BgStatusWeatherProvider"].ToString() + ".", Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });

                    PackageVersion AppVersion = Package.Current.Id.Version;
                    sp_Help.Children.Add(new TextBlock() { Text = "\r\nApplication made by Arnold Vink", Style = (Style)App.Current.Resources["TextBlockBlack"] });
                    sp_Help.Children.Add(new TextBlock() { Text = "Version: v" + AppVersion.Major + "." + AppVersion.Minor + "." + AppVersion.Build + "." + AppVersion.Revision, Style = (Style)App.Current.Resources["TextBlockLightGray"], TextWrapping = TextWrapping.Wrap });
                }
            }
            catch { }
        }
    }
}