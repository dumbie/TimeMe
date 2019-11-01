using ArnoldVinkCode;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Update current battery tile data
        async Task<bool> LoadTileDataBattery()
        {
            try
            {
                Debug.WriteLine("Loading the battery tile data.");

                //Check if the device has a battery
                if (BatteryLevel == "error") { return false; }

                //Load battery tile background photo or color
                if (setDisplayBackgroundPhotoBattery)
                {
                    if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                    {
                        TileContentId = WebUtility.HtmlEncode(BgStatusPhotoName);
                        TileBattery_BackgroundPhotoXml = "<image src=\"ms-appdata:///local/TimeMeTilePhoto.png\" placement=\"background\" hint-overlay=\"" + setDisplayBackgroundBrightnessInt + "\"/>";
                    }
                    else { TileBattery_BackgroundPhotoXml = "<image src=\"ms-appx:///Assets/Tiles/TimeMeTilePhoto.png\" placement=\"background\" hint-overlay=\"" + setDisplayBackgroundBrightnessInt + "\"/>"; }
                }
                else if (setDisplayBackgroundColorBattery)
                {
                    if (await AVFunctions.LocalFileExists("TimeMeTileColor.png"))
                    {
                        TileContentId = WebUtility.HtmlEncode(setLiveTileColorBackground);
                        TileBattery_BackgroundPhotoXml = "<image src=\"ms-appdata:///local/TimeMeTileColor.png\" placement=\"background\" hint-overlay=\"0\"/>";
                    }
                    else { TileBattery_BackgroundPhotoXml = "<image src=\"ms-appx:///Assets/Tiles/TimeMeTileColor.png\" placement=\"background\" hint-overlay=\"0\"/>"; }
                }
                return true;
            }
            catch { return false; }
        }
    }
}