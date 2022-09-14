using ArnoldVinkCode;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TimeMeShared.Classes.ApiOpenMeteo;
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
                if (!setBackgroundDownload || !setDownloadWeather || !await AVFunctions.LocalFileExists("TimeMeWeatherSummary.js"))
                {
                    return false;
                }

                //Load last weather update time
                if (setDisplayWeatherTileUpdateTime)
                {
                    //Set the weather update time text
                    if (BgStatusDownloadWeatherTime != "Never" && BgStatusDownloadWeatherTime != "Failed")
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
                    else
                    {
                        WeatherLastUpdate = "Unknown";
                    }
                }
                else
                {
                    WeatherLastUpdate = "";
                }

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
                    Forecast jsonForecast;
                    using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeWeatherSummary.js"))
                    {
                        using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                        {
                            jsonForecast = JsonConvert.DeserializeObject<Forecast>(await StreamReader.ReadToEndAsync());
                        }
                    }

                    //Check if weather is available
                    if (jsonForecast == null)
                    {
                        return false;
                    }
                    else
                    {
                        //Set Weather Units
                        string unitsTemperature = "°";

                        //Count available days
                        int forecastDayCount = 5;

                        //Set Overall Weather Forecast
                        for (int i = 0; i < forecastDayCount; i++)
                        {
                            //Set Weather Date
                            string WeatherDate = jsonForecast.daily.time[i].ToString();
                            DateTime WeatherDateTime = DateTime.Parse(WeatherDate);
                            if (setDisplayRegionLanguage)
                            {
                                WeatherDate = AVFunctions.ToTitleCase(WeatherDateTime.ToString("ddd", vCultureInfoReg));
                            }
                            else
                            {
                                WeatherDate = WeatherDateTime.ToString("ddd", vCultureInfoEng);
                            }

                            //Set Weather Icon
                            string WeatherIcon = jsonForecast.daily.weathercode[i].ToString();
                            string WeatherIconStyle = (bool)vApplicationSettings["DisplayWeatherWhiteIcons"] ? "WeatherWhite" : "Weather";
                            if (!string.IsNullOrEmpty(WeatherIcon))
                            {
                                if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png"))
                                {
                                    WeatherIcon = "/Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png";
                                }
                                else
                                {
                                    WeatherIcon = "/Assets/" + WeatherIconStyle + "/0.png";
                                }
                            }
                            else
                            {
                                WeatherIcon = "/Assets/" + WeatherIconStyle + "/0.png";
                            }

                            //Set Weather Highest Temperature
                            string WeatherTempHigh = jsonForecast.daily.temperature_2m_max[i].ToString();
                            if (!string.IsNullOrEmpty(WeatherTempHigh))
                            {
                                WeatherTempHigh += unitsTemperature;
                            }
                            else
                            {
                                WeatherTempHigh = "N/A";
                            }

                            //Set Weather Lowest Temperature
                            string WeatherTempLow = jsonForecast.daily.temperature_2m_min[i].ToString();
                            if (!string.IsNullOrEmpty(WeatherTempLow))
                            {
                                WeatherTempLow += unitsTemperature;
                            }
                            else
                            {
                                WeatherTempLow = "N/A";
                            }

                            //Set Weather Forecast to XML
                            if (setWeatherTileSizeName == "WeatherForecast")
                            {
                                if (setShowMoreTiles && (setDisplayWeatherTileLocation || setDisplayWeatherTileProvider || setDisplayWeatherTileUpdateTime))
                                {
                                    switch (i)
                                    {
                                        case 0: { WeatherTile1 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 1: { WeatherTile2 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 2: { WeatherTile3 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 3: { WeatherTile4 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                        case 4: { WeatherTile5 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    }
                                }
                                else
                                {
                                    switch (i)
                                    {
                                        case 0: { WeatherTile1 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 1: { WeatherTile2 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 2: { WeatherTile3 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 3: { WeatherTile4 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                        case 4: { WeatherTile5 = "<subgroup hint-weight=\"1\"><text hint-align=\"center\">" + WeatherDate + "</text><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/><text hint-align=\"center\">" + WeatherTempHigh + "</text><text hint-align=\"center\" hint-style=\"captionsubtle\">" + WeatherTempLow + "</text></subgroup>"; break; }
                                    }
                                }
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 0: { WeatherTile1 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 1: { WeatherTile2 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 2: { WeatherTile3 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 3: { WeatherTile4 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                    case 4: { WeatherTile5 = "<subgroup hint-textStacking=\"center\" hint-weight=\"45\"><image src=\"ms-appx://" + WeatherIcon + "\" hint-removeMargin=\"true\"/></subgroup><subgroup hint-textStacking=\"center\" hint-weight=\"50\"><text hint-align=\"left\">" + WeatherDate + "</text><text hint-align=\"left\" hint-style=\"captionSubtle\">" + WeatherTempHigh + "</text></subgroup>"; break; }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}