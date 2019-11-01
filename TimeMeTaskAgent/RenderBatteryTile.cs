using System.Diagnostics;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Render Battery tile front
        void RenderBatteryTile()
        {
            try
            {
                Debug.WriteLine("Started rendering the battery tile.");

                //Render icon battery tile
                if (setBatteryTileSizeName == "BatteryIcon")
                {
                    //Set Multi Variables
                    string TileImage = "<group><subgroup><image src=\"ms-appx:///Assets/BatterySquare/BatteryVer" + BatteryIcon + ".png\"/></subgroup></group>";

                    //Set Small Tile Texts
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";

                    //Set Medium Tile Texts
                    string BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";

                    //Set Wide Tile Texts
                    string BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
                //Render text battery tile
                else if (setBatteryTileSizeName == "BatteryText")
                {
                    //Set Multi Variables
                    string TileImageText = "<subgroup><image src=\"ms-appx:///Assets/Battery/BatteryVer" + BatteryIcon + ".png\"/></subgroup>";

                    //Set Small Tile Texts
                    string SmallTileFontSize = "subtitle";
                    if (BatteryLevel.Length > 2) { SmallTileFontSize = "base"; }
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"" + SmallTileFontSize + "\" hint-align=\"center\">" + BatteryLevel + "%</text></subgroup></group></binding>";

                    //Set Medium Tile Texts
                    string BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "<subgroup hint-textStacking=\"center\"><text hint-style=\"base\" hint-align=\"left\">" + BatteryLevel + "%</text><text hint-style=\"baseSubtle\" hint-align=\"left\">Left</text></subgroup></group></binding>";

                    //Set Wide Tile Texts
                    string BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "<subgroup hint-textStacking=\"center\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BatteryLevel + "%</text><text hint-style=\"titleNumeralSubtle\" hint-align=\"left\">Left</text></subgroup></group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
                //Render text left battery tile
                else if (setBatteryTileSizeName == "BatteryTextLeft")
                {
                    //Set Multi Variables
                    string TileImageText = "<subgroup><image src=\"ms-appx:///Assets/Battery/BatteryVer" + BatteryIcon + ".png\"/></subgroup>";

                    //Set Small Tile Texts
                    string SmallTileFontSize = "subtitle";
                    if (BatteryLevel.Length > 2) { SmallTileFontSize = "base"; }
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"" + SmallTileFontSize + "\" hint-align=\"center\">" + BatteryLevel + "%</text></subgroup></group></binding>";

                    //Set Medium Tile Texts
                    string BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"base\" hint-align=\"right\">" + BatteryLevel + "%</text><text hint-style=\"baseSubtle\" hint-align=\"right\">Left</text></subgroup>" + TileImageText + "</group></binding>";

                    //Set Wide Tile Texts
                    string BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"titleNumeral\" hint-align=\"right\">" + BatteryLevel + "%</text><text hint-style=\"titleNumeralSubtle\" hint-align=\"right\">Left</text></subgroup>" + TileImageText + "</group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
                //Render text vert battery tile
                else if (setBatteryTileSizeName == "BatteryTextVert")
                {
                    //Set Multi Variables
                    string TileImage = "<group><subgroup><image src=\"ms-appx:///Assets/BatterySquare/BatteryHor" + BatteryIcon + ".png\"/></subgroup></group>";

                    //Set Small Tile Texts
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";

                    //Set Medium Tile Texts
                    string TileImageText = "<group><subgroup hint-weight=\"1\"/><subgroup><image src=\"ms-appx:///Assets/Battery/BatteryHor" + BatteryIcon + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"1\"/></group>";
                    string BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImageText + "<text hint-style=\"subtitle\" hint-align=\"center\">" + BatteryLevel + "%</text></binding>";

                    //Set Wide Tile Texts
                    TileImageText = "<group><subgroup hint-weight=\"1\"/><subgroup hint-weight=\"2\"><image src=\"ms-appx:///Assets/Battery/BatteryHor" + BatteryIcon + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"1\"/></group>";
                    string BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImageText + "<text hint-style=\"subtitle\" hint-align=\"center\">" + BatteryLevel + "%</text></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
                //Render text vert top battery tile
                else if (setBatteryTileSizeName == "BatteryTextVertTop")
                {
                    //Set Multi Variables
                    string TileImage = "<group><subgroup><image src=\"ms-appx:///Assets/BatterySquare/BatteryHor" + BatteryIcon + ".png\"/></subgroup></group>";

                    //Set Small Tile Texts
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + TileImage + "</binding>";

                    //Set Medium Tile Texts
                    string TileImageText = "<group><subgroup hint-weight=\"1\"/><subgroup><image src=\"ms-appx:///Assets/Battery/BatteryHor" + BatteryIcon + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"1\"/></group>";
                    string BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<text hint-style=\"subtitle\" hint-align=\"center\">" + BatteryLevel + "%</text>" + TileImageText + "</binding>";

                    //Set Wide Tile Texts
                    TileImageText = "<group><subgroup hint-weight=\"1\"/><subgroup hint-weight=\"2\"><image src=\"ms-appx:///Assets/Battery/BatteryHor" + BatteryIcon + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"1\"/></group>";
                    string BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<text hint-style=\"subtitle\" hint-align=\"center\">" + BatteryLevel + "%</text>" + TileImageText + "</binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
                //Render perc battery tile
                else if (setBatteryTileSizeName == "BatteryPerc")
                {
                    //Set Small Tile Texts
                    string SmallTileFontSize = "subtitle";
                    if (BatteryLevel.Length > 2) { SmallTileFontSize = "base"; }
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"" + SmallTileFontSize + "\" hint-align=\"center\">" + BatteryLevel + "%</text></subgroup></group></binding>";

                    //Set Medium Tile Texts
                    string BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"title\" hint-align=\"center\">🗲 " + BatteryLevel + "%</text></subgroup></group></binding>";

                    //Set Wide Tile Texts
                    string BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"header\" hint-align=\"center\">🗲 " + BatteryLevel + "%</text></subgroup></group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }
                //Render summ battery tile
                else if (setBatteryTileSizeName == "BatterySumm")
                {
                    //Set Multi Variables
                    string TileImageText = "<subgroup><image src=\"ms-appx:///Assets/Battery/BatteryVer" + BatteryIcon + ".png\"/></subgroup>";

                    //Set Small Tile Texts
                    string BatterySmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "</group></binding>";

                    //Set Medium and Wide Tile Texts
                    string BatteryMediumTile = "";
                    string BatteryWideTile = "";
                    if (BatteryCharging)
                    {
                        BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "<subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BatteryLevel + "%</text><text hint-style=\"captionSubtle\" hint-align=\"left\">Charg</text></subgroup></group></binding>";
                        BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "<subgroup hint-textStacking=\"center\"><text hint-style=\"body\" hint-align=\"left\">" + BatteryLevel + " Percent</text><text hint-style=\"bodySubtle\" hint-align=\"left\">Charging</text></subgroup></group></binding>";
                    }
                    else
                    {
                        BatteryMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "<subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BatteryLevel + "%</text><text hint-style=\"caption\" hint-align=\"left\">" + BatteryTime + "</text><text hint-style=\"captionSubtle\" hint-align=\"left\">Remain</text></subgroup></group></binding>";
                        BatteryWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileBattery_BackgroundPhotoXml + "<group>" + TileImageText + "<subgroup hint-textStacking=\"center\"><text hint-style=\"body\" hint-align=\"left\">" + BatteryLevel + " Percent</text><text hint-style=\"body\" hint-align=\"left\">" + BatteryTime + "</text><text hint-style=\"bodySubtle\" hint-align=\"left\">Remaining</text></subgroup></group></binding>";
                    }

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + BatterySmallTile + BatteryMediumTile + BatteryWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                Debug.WriteLine("The battery tile has been updated.");
            }
            catch { }
        }
    }
}