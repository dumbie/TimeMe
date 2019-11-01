using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.UserProfile;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {

        //Download Bing wallpaper
        async Task DownloadBingWallpaper()
        {
            try
            {
                Debug.WriteLine("Downloading Bing update.");

                //Check for internet connection
                if (!DownloadInternetCheck()) { BackgroundStatusUpdateSettings(null, null, "Never", null, "NoWifiEthernet"); return; }

                //Load and set Download Information
                string DownloadBingRegion = "en-US";
                switch (setDownloadBingRegion)
                {
                    case 0: { DownloadBingRegion = vCultureInfoReg.Name; break; }
                    case 2: { DownloadBingRegion = "en-GB"; break; }
                    case 3: { DownloadBingRegion = "en-AU"; break; }
                    case 4: { DownloadBingRegion = "de-DE"; break; }
                    case 5: { DownloadBingRegion = "en-CA"; break; }
                    case 6: { DownloadBingRegion = "ja-JP"; break; }
                    case 7: { DownloadBingRegion = "zh-CN"; break; }
                    case 8: { DownloadBingRegion = "fr-FR"; break; }
                    case 9: { DownloadBingRegion = "pt-BR"; break; }
                    case 10: { DownloadBingRegion = "nz-NZ"; break; }
                }
                string DownloadBingResolution = "1920x1080";
                switch (setDownloadBingResolution)
                {
                    case 1: { DownloadBingResolution = "1280x720"; break; }
                    case 2: { DownloadBingResolution = "1080x1920"; break; }
                    case 3: { DownloadBingResolution = "720x1280"; break; }
                    case 4: { DownloadBingResolution = "1024x768"; break; }
                }

                //Download and read Bing Wallpaper XML
                XDocument XDocumentBing = XDocument.Parse(await AVDownloader.DownloadStringAsync(5000, "TimeMe", null, new Uri("https://www.bing.com/HPImageArchive.aspx?format=xml&n=1&mkt=" + DownloadBingRegion)));

                //Read and check current Bing wallpaper
                XElement XElement = XDocumentBing.Descendants("image").First();
                string BingUrlName = XElement.Element("urlBase").Value + "_" + DownloadBingResolution + ".jpg";
                string BingDescription = XElement.Element("copyright").Value;
                if (BgStatusBingDescription != BingDescription)
                {
                    //Download and Save Bing Wallpaper image
                    IBuffer BingWallpaperFile = await AVDownloader.DownloadBufferAsync(5000, "TimeMe", new Uri("https://www.bing.com" + BingUrlName));
                    if (BingWallpaperFile != null)
                    {
                        //Save the Bing wallpaper image
                        StorageFile BingSaveFile = await AVFile.SaveBuffer("TimeMeTilePhoto.png" + new String(' ', new Random().Next(1, 50)), BingWallpaperFile);

                        //Set background photo as device wallpaper
                        try
                        {
                            if (setDeviceWallpaper)
                            {
                                await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(BingSaveFile);
                            }
                        }
                        catch { Debug.WriteLine("Failed to update Device wallpaper."); }

                        //Set background photo as lockscreen wallpaper
                        try
                        {
                            if (setLockWallpaper)
                            {
                                if (setDevStatusMobile) { await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Tiles/TimeMeTileColor.png"))); }
                                await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(BingSaveFile);
                            }
                        }
                        catch { Debug.WriteLine("Failed to update Lock screen wallpaper."); }

                        //Save Bing photo name
                        BgStatusPhotoName = BingUrlName;
                        vApplicationSettings["BgStatusPhotoName"] = BgStatusPhotoName;

                        //Save Bing description status
                        BgStatusBingDescription = BingDescription;
                        vApplicationSettings["BgStatusBingDescription"] = BgStatusBingDescription;

                        //Notification - Bing description
                        ShowNotiBingDescription();

                        //Force live tile update
                        TileLive_ForceUpdate = true;
                    }
                    else
                    {
                        Debug.WriteLine("Failed downloading the Bing wallpaper.");
                        BackgroundStatusUpdateSettings(null, null, "Never", null, "FailedDownloadBingWallpaper");
                        return;
                    }
                }

                //Save Bing update status
                BgStatusDownloadBing = DateTimeNow.ToString(vCultureInfoEng);
                vApplicationSettings["BgStatusDownloadBing"] = BgStatusDownloadBing;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating the Bing wallpaper.");
                BackgroundStatusUpdateSettings(null, null, "Never", null, "CatchDownloadBingWallpaper" + ex.Message);
            }
        }
    }
}