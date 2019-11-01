using System.Diagnostics;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Render weather tile front
        void RenderWeatherTile()
        {
            try
            {
                Debug.WriteLine("Started rendering the weather tile.");

                //Render lite flip weather tile
                if (setWeatherTileSizeName == "WeatherLiteFlip")
                {
                    //Set the peek image
                    string PeekImage = "<image placement=\"peek\" src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\"/>";

                    //Set Small Tile Texts
                    string SmallTileFontSize = "title";
                    if (BgStatusWeatherCurrentTemp.Length > 3) { SmallTileFontSize = "subtitle"; }
                    string WeatherSmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + PeekImage + "<group><subgroup><text hint-style=\"" + SmallTileFontSize + "\" hint-align=\"center\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group></binding>";

                    //Set Medium Tile Texts
                    string WeatherMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + PeekImage + "<group><subgroup><text hint-style=\"header\" hint-align=\"center\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group></binding>";

                    //Set Wide Tile Texts
                    string WeatherWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + PeekImage + "<group><subgroup><text hint-style=\"header\" hint-align=\"center\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherSmallTile + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                //Render grey weather tile
                else if (setWeatherTileSizeName == "WeatherGrey")
                {
                    //Set Multi Variables
                    string WeatherMultiDetails = WeatherDetailed;
                    if (setDisplayWeatherTileUpdateTime) { WeatherMultiDetails = WeatherLastUpdate; }

                    //Set Small Tile Texts
                    string WeatherSmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\"/></binding>";

                    //Set Medium Tile Texts
                    string GroupWeatherText = "<group><subgroup><text hint-style=\"captionSubtle\" hint-align=\"center\">" + WeatherMultiDetails + "</text></subgroup></group>";
                    string GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\"><image src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\"><text hint-style=\"title\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group>";
                    string WeatherMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + GroupWeatherTemps + GroupWeatherText + "</binding>";

                    //Set Wide Tile Texts
                    GroupWeatherText = "<group><subgroup><text hint-style=\"titleNumeralSubtle\">" + WeatherMultiDetails + "</text></subgroup></group>";
                    GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"35\"><image src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\"><text hint-style=\"title\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group>";
                    string WeatherWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + GroupWeatherTemps + GroupWeatherText + "</binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherSmallTile + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                //Render icon weather tile
                else if (setWeatherTileSizeName == "WeatherIcon")
                {
                    //Set Small Tile Texts
                    string SmallTileFontSize = "title";
                    if (BgStatusWeatherCurrentTemp.Length > 3) { SmallTileFontSize = "subtitle"; }
                    string WeatherSmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"" + SmallTileFontSize + "\" hint-align=\"center\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group></binding>";

                    //Set Medium Tile Texts
                    string WeatherTextCurrent = "";
                    string WeatherTextDetailed = "";
                    string WeatherTextUpdate = "";
                    string WeatherImageCurrent = "";

                    if (BgStatusWeatherCurrent.Length < 13) { WeatherTextCurrent = "<text hint-style=\"caption\" hint-align=\"center\">" + BgStatusWeatherCurrent + "</text>"; }
                    else { WeatherTextCurrent = "<text hint-style=\"caption\" hint-align=\"center\">" + BgStatusWeatherCurrentTemp.Replace("°", " degrees") + "</text>"; }
                    if (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider) { WeatherTextDetailed = "<text hint-style=\"captionSubtle\" hint-align=\"center\">" + WeatherDetailed + "</text>"; }
                    if (setDisplayWeatherTileUpdateTime) { WeatherTextUpdate = "<text hint-style=\"captionSubtle\" hint-align=\"center\">" + WeatherLastUpdate + "</text>"; }

                    if (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider || setDisplayWeatherTileUpdateTime) { WeatherImageCurrent = "<group><subgroup hint-weight=\"1\"/><subgroup hint-weight=\"2\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"1\"/></group>"; }
                    else { WeatherImageCurrent = "<group><subgroup hint-weight=\"1\"/><subgroup hint-weight=\"100\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"1\"/></group>"; }
                    string WeatherMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + WeatherImageCurrent + WeatherTextCurrent + WeatherTextDetailed + WeatherTextUpdate + "</binding>";

                    //Set Wide Tile Texts
                    WeatherTextCurrent = "<text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrent + "</text>";
                    if (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider) { WeatherTextDetailed = "<text hint-style=\"captionSubtle\" hint-align=\"left\">" + WeatherDetailed + "</text>"; }
                    if (setDisplayWeatherTileUpdateTime) { WeatherTextUpdate = "<text hint-style=\"captionSubtle\" hint-align=\"left\">" + WeatherLastUpdate + "</text>"; }
                    string WeatherWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<group><subgroup hint-weight=\"35\" hint-textStacking=\"center\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-weight=\"65\" hint-textStacking=\"center\">" + WeatherTextCurrent + WeatherTextDetailed + WeatherTextUpdate + "</subgroup></group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherSmallTile + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                //Render summary weather tile
                else if (setWeatherTileSizeName == "WeatherSumm")
                {
                    //Set Multi Variables
                    string GroupWeatherText = "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentText + "</text></subgroup></group>";

                    //Set Small Tile Texts
                    string SmallTileFontSize = "title";
                    if (BgStatusWeatherCurrentTemp.Length > 3) { SmallTileFontSize = "subtitle"; }
                    string WeatherSmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<group><subgroup><text hint-style=\"" + SmallTileFontSize + "\" hint-align=\"center\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group></binding>";

                    //Set Medium Tile Texts
                    string GroupWeatherTemps = "";
                    if (setDisplayWeatherTempHighLow) { GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"70\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BgStatusWeatherCurrentTemp + "</text></subgroup> <subgroup><text hint-align=\"right\">" + BgStatusWeatherCurrentTempHigh + "</text><text hint-style=\"captionSubtle\" hint-align=\"right\">" + BgStatusWeatherCurrentTempLow + "</text></subgroup></group>"; }
                    else { GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"1\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\" hint-weight=\"2\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group>"; }

                    string GroupWeatherPrecip = "<group><subgroup hint-weight=\"17\" hint-textStacking=\"center\"><image src=\"ms-appx:///Assets/WeatherOther/tab_Precipitation.png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentRainChance + "</text></subgroup></group>";
                    string GroupWeatherWind = "<group><subgroup hint-weight=\"17\" hint-textStacking=\"center\"><image src=\"ms-appx:///Assets/WeatherOther/tab_WindSpeed.png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentWindSpeed + "</text></subgroup></group>";
                    string WeatherMediumTile = "<binding template=\"TileMedium\">" + TileWeather_BackgroundPhotoXml + GroupWeatherTemps + GroupWeatherText + GroupWeatherWind + GroupWeatherPrecip + "</binding>";

                    //Set Wide Tile Texts
                    string WeatherTextDetailed = "";
                    string WeatherTextUpdate = "";
                    if (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider) { if (setDisplayWeatherTempHighLow) { WeatherTextDetailed = "<text hint-align=\"left\">" + WeatherDetailed + "</text>"; } else { WeatherTextDetailed = "<text hint-align=\"right\">" + WeatherDetailed + "</text>"; } }
                    if (setDisplayWeatherTileUpdateTime) { if (setDisplayWeatherTempHighLow) { WeatherTextUpdate = "<text hint-style=\"captionSubtle\" hint-align=\"left\">" + WeatherLastUpdate + "</text>"; } else { WeatherTextUpdate = "<text hint-style=\"captionSubtle\" hint-align=\"right\">" + WeatherLastUpdate + "</text>"; } }

                    if (setDisplayWeatherTempHighLow) { GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"1\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\" hint-weight=\"2\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BgStatusWeatherCurrentTemp + "</text></subgroup> <subgroup hint-weight=\"1\"><text hint-align=\"right\">" + BgStatusWeatherCurrentTempHigh + "</text><text hint-style=\"captionSubtle\" hint-align=\"right\">" + BgStatusWeatherCurrentTempLow + "</text></subgroup> <subgroup hint-weight=\"2\">" + WeatherTextDetailed + WeatherTextUpdate + "</subgroup></group>"; }
                    else { GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"15\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\" hint-weight=\"45\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BgStatusWeatherCurrentTemp + "</text></subgroup> <subgroup hint-weight=\"40\">" + WeatherTextDetailed + WeatherTextUpdate + "</subgroup></group>"; }

                    GroupWeatherPrecip = "<group><subgroup hint-weight=\"7\" hint-textStacking=\"center\"><image src=\"ms-appx:///Assets/WeatherOther/tab_Precipitation.png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentRainChance + "</text></subgroup></group>";
                    GroupWeatherWind = "<group><subgroup hint-weight=\"7\" hint-textStacking=\"center\"><image src=\"ms-appx:///Assets/WeatherOther/tab_WindSpeed.png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentWindSpeed + "</text></subgroup></group>";

                    string WeatherWideTile = "<binding template=\"TileWide\">" + TileWeather_BackgroundPhotoXml + GroupWeatherTemps + GroupWeatherText + GroupWeatherWind + GroupWeatherPrecip + "</binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherSmallTile + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                //Render forecast weather tile
                else if (setWeatherTileSizeName == "WeatherForecast")
                {
                    //Set Medium Tile Texts
                    string WeatherMediumTile = "";
                    if (setShowMoreTiles) { WeatherMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<group>" + WeatherTile1 + WeatherTile2 + WeatherTile3 + "</group><group><subgroup><text hint-style=\"captionSubtle\">" + WeatherLastUpdate + "</text><text hint-style=\"caption\">" + WeatherDetailed + "</text></subgroup></group></binding>"; }
                    else { WeatherMediumTile = "<binding template=\"TileMedium\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<group>" + WeatherTile1 + WeatherTile2 + WeatherTile3 + "</group><group><subgroup><text hint-align=\"left\" hint-style=\"caption\">" + WeatherDetailed + "</text></subgroup><subgroup><text hint-align=\"right\" hint-style=\"captionSubtle\">" + WeatherLastUpdate + "</text></subgroup></group></binding>"; }

                    //Set Wide Tile Texts
                    string WeatherWideTile = "<binding template=\"TileWide\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<group>" + WeatherTile1 + WeatherTile2 + WeatherTile3 + WeatherTile4 + WeatherTile5 + "</group><group><subgroup><text hint-align=\"left\" hint-style=\"caption\">" + WeatherDetailed + "</text></subgroup><subgroup><text hint-align=\"right\" hint-style=\"captionSubtle\">" + WeatherLastUpdate + "</text></subgroup></group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                //Render combo weather tile
                else if (setWeatherTileSizeName == "WeatherCombo")
                {
                    //Set Multi Variables
                    string GroupWeatherText = "";
                    string GroupWeatherTemps = "";
                    string GroupWeatherForecast = "";

                    //Set Small Tile Texts
                    string WeatherSmallTile = "<binding template=\"TileSmall\" hint-textStacking=\"center\">" + TileWeather_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\"/></binding>";

                    //Set Medium Tile Texts
                    GroupWeatherText = "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentText + "</text></subgroup></group>";
                    GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"1\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\" hint-weight=\"2\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BgStatusWeatherCurrentTemp + "</text></subgroup></group>";
                    if (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider) { GroupWeatherForecast = "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"captionSubtle\" hint-align=\"left\">" + WeatherDetailed + "</text><text hint-style=\"captionSubtle\" hint-align=\"left\">" + WeatherLastUpdate + "</text></subgroup></group>"; }
                    else { GroupWeatherForecast = "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"captionSubtle\" hint-align=\"left\">" + WeatherLastUpdate + "</text></subgroup></group>"; }
                    string WeatherMediumTile = "<binding template=\"TileMedium\">" + TileWeather_BackgroundPhotoXml + GroupWeatherTemps + GroupWeatherText + GroupWeatherForecast + "</binding>";

                    //Set Wide Tile Texts
                    string WeatherTextDetailed = "";
                    string WeatherTextUpdate = "";
                    if (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider) { WeatherTextDetailed = "<text hint-align=\"right\">" + WeatherDetailed + "</text>"; }
                    if (setDisplayWeatherTileUpdateTime) { WeatherTextUpdate = "<text hint-style=\"captionSubtle\" hint-align=\"right\">" + WeatherLastUpdate + "</text>"; }
                    GroupWeatherText = "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"caption\" hint-align=\"left\">" + BgStatusWeatherCurrentText + "</text></subgroup> <subgroup hint-weight=\"25\"><text hint-style=\"caption\" hint-align=\"right\">" + BgStatusWeatherCurrentRainChance + " Rain</text></subgroup></group>";
                    GroupWeatherTemps = "<group><subgroup hint-textStacking=\"center\" hint-weight=\"15\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png\" hint-removeMargin=\"true\"/></subgroup> <subgroup hint-textStacking=\"center\" hint-weight=\"45\"><text hint-style=\"titleNumeral\" hint-align=\"left\">" + BgStatusWeatherCurrentTemp + "</text></subgroup> <subgroup hint-weight=\"40\">" + WeatherTextDetailed + WeatherTextUpdate + "</subgroup></group>";
                    GroupWeatherForecast = "<group>" + WeatherTile2 + WeatherTile3 + WeatherTile4 + "</group>";
                    string WeatherWideTile = "<binding template=\"TileWide\">" + TileWeather_BackgroundPhotoXml + GroupWeatherTemps + GroupWeatherText + GroupWeatherForecast + "</binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherSmallTile + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                //Render forecast words tile
                else if (setWeatherTileSizeName == "WeatherWords")
                {
                    //Set Medium Tile Texts
                    string WeatherMediumTile = "<binding template=\"TileMedium\">" + TileWeather_BackgroundPhotoXml + "<group><subgroup><text hint-align=\"left\" hint-wrap=\"true\" hint-style=\"caption\">" + WeatherTile1 + "</text></subgroup></group><group><subgroup><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherLastUpdate + "</text><text hint-align=\"left\" hint-style=\"caption\">" + WeatherDetailed + "</text></subgroup></group></binding>";

                    //Set Wide Tile Texts
                    string WeatherWideTile = "<binding template=\"TileWide\">" + TileWeather_BackgroundPhotoXml + "<group><subgroup><text hint-align=\"left\" hint-wrap=\"true\" hint-style=\"caption\">" + WeatherTile1 + "</text><text hint-align=\"left\" hint-wrap=\"true\" hint-style=\"captionSubtle\">" + WeatherTile2 + "</text></subgroup></group><group><subgroup><text hint-align=\"left\" hint-style=\"caption\">" + WeatherDetailed + "</text></subgroup><subgroup><text hint-align=\"right\" hint-style=\"captionSubtle\">" + WeatherLastUpdate + "</text></subgroup></group></binding>";

                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\">" + WeatherMediumTile + WeatherWideTile + "</visual></tile>");
                    Tile_UpdateManager.Update(new TileNotification(Tile_XmlContent));
                }

                Debug.WriteLine("The weather tile has been updated.");
            }
            catch { }
        }
    }
}