using System.Diagnostics;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Set tile to failed update
        XmlDocument RenderTileLiveFailed(string TileId)
        {
            try
            {
                Debug.WriteLine("Set tile to failed: " + TileId);

                //Reset primary tile
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();

                Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TileId);
                Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();

                foreach (ScheduledTileNotification Tile_Update in Tile_PlannedUpdates) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TileId).Clear();

                Tile_XmlContent.LoadXml("<tile><visual branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoFailed.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoFailed.png\"/></binding></visual></tile>");
                Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
            }
            catch { }
            return Tile_XmlContent;
        }

        //Set live tile to updated launch app.
        XmlDocument RenderTileAppUpdated(string TileId)
        {
            try
            {
                Debug.WriteLine("Setting tile to app updated:" + TileId);

                //Reset primary tile
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();

                Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TileId);
                Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();

                foreach (ScheduledTileNotification Tile_Update in Tile_PlannedUpdates) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TileId).Clear();

                Tile_XmlContent.LoadXml("<tile><visual branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoVersion.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoVersion.png\"/></binding></visual></tile>");
                Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
            }
            catch { }
            return Tile_XmlContent;
        }

        //Set battery tile to disabled
        XmlDocument RenderTileBatteryDisabled()
        {
            try
            {
                //Reset secondary battery tile
                if (TileBattery_Pinned)
                {
                    Debug.WriteLine("Set battery tile to disabled.");

                    Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeBatteryTile");
                    Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();

                    foreach (ScheduledTileNotification Tile_Update in Tile_PlannedUpdates) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                    BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile("TimeMeBatteryTile").Clear();

                    string TileImage = "<group><subgroup><image src=\"ms-appx:///Assets/BatterySquare/BatteryVerNoBattery.png\"/></subgroup></group>";
                    string BatterySmallTile = "<binding template=\"TileSmall\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";
                    string BatteryMediumTile = "<binding template=\"TileMedium\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";
                    string BatteryWideTile = "<binding template=\"TileWide\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
            }
            catch { }
            return Tile_XmlContent;
        }

        //Set weather tile to disabled
        XmlDocument RenderTileWeatherDisabled()
        {
            try
            {
                //Reset secondary weather tile
                if (TileWeather_Pinned)
                {
                    Debug.WriteLine("Set weather tile to disabled.");

                    Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeWeatherTile");
                    Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();

                    foreach (ScheduledTileNotification Tile_Update in Tile_PlannedUpdates) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }
                    BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile("TimeMeWeatherTile").Clear();

                    Tile_XmlContent.LoadXml("<tile><visual branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoWeatherDisabled.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoWeatherDisabled.png\"/></binding></visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
            }
            catch { }
            return Tile_XmlContent;
        }
    }
}