using ArnoldVinkCode;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Update current weather tile data
        async Task<bool> LoadTileDataWeather()
        {
            try
            {
                Debug.WriteLine("Loading the weather tile data.");

                //Check if weather background updates are enabled
                if (!setBackgroundDownload || !setDownloadWeather || !await AVFunctions.LocalFileExists("TimeMeWeatherSummary.js")) { return false; }

                //Load last weather update time
                if (setDisplayWeatherTileUpdateTime)
                {
                    //Set the weather update time text
                    if (BgStatusDownloadWeatherTime != "Never")
                    {
                        DateTime WeatherTime = DateTime.Parse(BgStatusDownloadWeatherTime, vCultureInfoEng);
                        if (setDisplay24hClock)
                        {
                            if (!String.IsNullOrEmpty(TextTimeSplit)) { WeatherLastUpdate = WeatherTime.ToString("HH" + TextTimeSplit + "mm"); }
                            else { WeatherLastUpdate = WeatherTime.ToString("HH:mm"); }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(TextTimeSplit)) { WeatherLastUpdate = WeatherTime.ToString("h" + TextTimeSplit + "mm tt", vCultureInfoEng); }
                            else { WeatherLastUpdate = WeatherTime.ToString("h:mm tt", vCultureInfoEng); }
                        }
                    }
                    else { WeatherLastUpdate = "Unknown"; }
                }
                else { WeatherLastUpdate = ""; }

                //Load weather detailed information
                if (setDisplayWeatherTileLocation) { WeatherDetailed = BgStatusWeatherCurrentLocationShort; }
                else if (setDisplayWeatherTileProvider) { WeatherDetailed = BgStatusWeatherProvider; }
                else { WeatherDetailed = ""; }

                //Load weather tile background photo or color
                if (setDisplayBackgroundPhotoWeather)
                {
                    if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                    {
                        TileContentId = WebUtility.HtmlEncode(BgStatusPhotoName);
                        TileWeather_BackgroundPhotoXml = "<image src=\"ms-appdata:///local/TimeMeTilePhoto.png\" placement=\"background\" hint-overlay=\"" + setDisplayBackgroundBrightnessInt + "\"/>";
                    }
                    else { TileWeather_BackgroundPhotoXml = "<image src=\"ms-appx:///Assets/Tiles/TimeMeTilePhoto.png\" placement=\"background\" hint-overlay=\"" + setDisplayBackgroundBrightnessInt + "\"/>"; }
                }
                else if (setDisplayBackgroundColorWeather)
                {
                    if (await AVFunctions.LocalFileExists("TimeMeTileColor.png"))
                    {
                        TileContentId = WebUtility.HtmlEncode(setLiveTileColorBackground);
                        TileWeather_BackgroundPhotoXml = "<image src=\"ms-appdata:///local/TimeMeTileColor.png\" placement=\"background\" hint-overlay=\"0\"/>";
                    }
                    else { TileWeather_BackgroundPhotoXml = "<image src=\"ms-appx:///Assets/Tiles/TimeMeTileColor.png\" placement=\"background\" hint-overlay=\"0\"/>"; }
                }

                //Load weather forecast tile data
                if (setWeatherTileSizeName == "WeatherForecast" || setWeatherTileSizeName == "WeatherCombo")
                {
                    //Set not available Weather Forecast styles
                    if (setWeatherTileSizeName == "WeatherForecast")
                    {
                        WeatherTile1 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">N/A</text><image src=\"ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/0.png\" hint-removeMargin=\"true\"/><text hint-align=\"center\">N/A</text></subgroup>";
                        WeatherTile2 = WeatherTile1; WeatherTile3 = WeatherTile1; WeatherTile4 = WeatherTile1; WeatherTile5 = WeatherTile1;
                    }
                    else
                    {
                        WeatherTile1 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx:///Assets/Weather" + WeatherIconStyle + "/0.png\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">N/A</text><text hint-align=\"left\" hint-style=\"captionSubtle\">N/A</text></subgroup>";
                        WeatherTile2 = WeatherTile1; WeatherTile3 = WeatherTile1; WeatherTile4 = WeatherTile1; WeatherTile5 = WeatherTile1;
                    }

                    //Load weather from json
                    JObject WeatherJObject;
                    using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeWeatherSummary.js"))
                    {
                        using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                        {
                            WeatherJObject = JObject.Parse(await StreamReader.ReadToEndAsync());
                        }
                    }

                    //Check if there is weather forecast available
                    if (WeatherJObject["value"][0]["responses"][0]["weather"] == null || WeatherJObject["value"][0]["responses"][0]["weather"][0]["forecast"]["days"].Count() <= 1)
                    {
                        return false;
                    }
                    else
                    {
                        int ForecastCount = 1;
                        JToken ForecastJToken = WeatherJObject["value"][0]["responses"][0]["weather"][0]["forecast"]["days"];
                        foreach (JToken DayJToken in ForecastJToken)
                        {
                            //Set Weather Date
                            string WeatherDate = "";
                            if (DayJToken["daily"]["valid"] != null)
                            {
                                WeatherDate = DayJToken["daily"]["valid"].ToString();
                                if (!String.IsNullOrEmpty(WeatherDate))
                                {
                                    DateTime WeatherDateTime = DateTime.Parse(WeatherDate);

                                    //Check if the day has already passed
                                    if (WeatherDateTime.AddDays(1) < DateTimeNow) { continue; }
                                    //if (WeatherDateTime.Day == Tile_DateTimeMin.Day) { WeatherDate = "Tod"; }
                                    //else
                                    //{
                                    if (setDisplayRegionLanguage) { WeatherDate = AVFunctions.ToTitleCase(WeatherDateTime.ToString("ddd", vCultureInfoReg)); }
                                    else { WeatherDate = WeatherDateTime.ToString("ddd", vCultureInfoEng); }
                                    //}
                                }
                                else { WeatherDate = "N/A"; }
                            }
                            else { WeatherDate = "N/A"; }

                            //Set Weather Icon
                            string WeatherIcon = "";
                            string WeatherIconFormat = "WeatherSquare" + WeatherIconStyle;
                            if (setWeatherTileSizeName == "WeatherCombo") { WeatherIconFormat = "Weather" + WeatherIconStyle; }
                            if (DayJToken["daily"]["icon"] != null)
                            {
                                WeatherIcon = DayJToken["daily"]["icon"].ToString();
                                if (!String.IsNullOrEmpty(WeatherIcon))
                                {
                                    if (await AVFunctions.AppFileExists("Assets/" + WeatherIconFormat + "/" + WeatherIcon + ".png")) { WeatherIcon = "/Assets/" + WeatherIconFormat + "/" + WeatherIcon + ".png"; }
                                    else { WeatherIcon = "/Assets/" + WeatherIconFormat + "/0.png"; }
                                }
                                else { WeatherIcon = "/Assets/" + WeatherIconFormat + "/0.png"; }
                            }
                            else { WeatherIcon = "/Assets/" + WeatherIconFormat + "/0.png"; }

                            //Set Weather Highest Temperature
                            string WeatherTempHigh = "";
                            if (DayJToken["daily"]["tempHi"] != null)
                            {
                                WeatherTempHigh = DayJToken["daily"]["tempHi"].ToString();
                                if (!String.IsNullOrEmpty(WeatherTempHigh)) { WeatherTempHigh = WeatherTempHigh + "°"; }
                                else { WeatherTempHigh = "N/A"; }
                            }
                            else { WeatherTempHigh = "N/A"; }

                            //Set Weather Lowest Temperature
                            string WeatherTempLow = "";
                            if (DayJToken["daily"]["tempLo"] != null)
                            {
                                WeatherTempLow = DayJToken["daily"]["tempLo"].ToString();
                                if (!String.IsNullOrEmpty(WeatherTempLow)) { WeatherTempLow = WeatherTempLow + "°"; }
                                else { WeatherTempLow = "N/A"; }
                            }
                            else { WeatherTempLow = "N/A"; }

                            //Set Weather Forecast to XML
                            if (setWeatherTileSizeName == "WeatherForecast")
                            {
                                if (setShowMoreTiles && (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider || setDisplayWeatherTileUpdateTime))
                                {
                                    switch (ForecastCount)
                                    {
                                        case 1: { WeatherTile1 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 2: { WeatherTile2 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 3: { WeatherTile3 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 4: { WeatherTile4 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 5: { WeatherTile5 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    }
                                }
                                else
                                {
                                    switch (ForecastCount)
                                    {
                                        case 1: { WeatherTile1 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 2: { WeatherTile2 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 3: { WeatherTile3 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 4: { WeatherTile4 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 5: { WeatherTile5 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                    }
                                }
                            }
                            else
                            {
                                switch (ForecastCount)
                                {
                                    case 1: { WeatherTile1 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 2: { WeatherTile2 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 3: { WeatherTile3 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 4: { WeatherTile4 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                }
                            }
                            ForecastCount++;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed loading weather tile data: " + ex.Message);
                return false;
            }
        }
    }
}