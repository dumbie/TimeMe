using ArnoldVinkCode;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TimeMe
{
    partial class MainPage
    {
        //Start loading the Weather
        async Task WeatherLoad()
        {
            try
            {
                if (!await WeatherCheck()) { return; }
                if (!await WeatherSummaryText()) { return; }
                if (!await WeatherForecastList()) { return; }
                WeatherStatus();
            }
            catch { }
        }

        //Check if the Weather is available
        async Task<bool> WeatherCheck()
        {
            try
            {
                //Check if weather background updates are enabled
                if (!(bool)vApplicationSettings["BackgroundDownload"] || !(bool)vApplicationSettings["DownloadWeather"])
                {
                    txt_ForecastInformation.Text = "The weather information is disabled, please enable\nthe download weather update setting to view the\nweather forecast and current weather on the tiles.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                    txt_ForecastInformation.Visibility = Visibility.Visible;
                    sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                    sv_WeatherForecast.Visibility = Visibility.Collapsed;
                    tab_Weather_ImgTemperaturePre.Source = null;
                    tab_Weather_ImgTemperatureCur.Source = null;
                    tab_Weather_ImgWallpaper.Source = null;
                    return false;
                }

                //Check if there are weather files available
                if (!await AVFunctions.LocalFileExists("TimeMeWeatherSummary.js") || !await AVFunctions.LocalFileExists("TimeMeWeatherForecast.js"))
                {
                    txt_ForecastInformation.Text = "The weather information is currently not available\nplease wait for the next planned live tile update\nor manually update the weather on the tile tab.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                    txt_ForecastInformation.Visibility = Visibility.Visible;
                    sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                    sv_WeatherForecast.Visibility = Visibility.Collapsed;
                    tab_Weather_ImgTemperaturePre.Source = null;
                    tab_Weather_ImgTemperatureCur.Source = null;
                    tab_Weather_ImgWallpaper.Source = null;
                    return false;
                }

                return true;
            }
            catch
            {
                txt_ForecastInformation.Text = "Failed to load the weather files\nplease check your internet connection,\nmake sure your location service is enabled\nor wait for the next planned live tile update.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                txt_ForecastInformation.Visibility = Visibility.Visible;
                sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                sv_WeatherForecast.Visibility = Visibility.Collapsed;
                tab_Weather_ImgTemperaturePre.Source = null;
                tab_Weather_ImgTemperatureCur.Source = null;
                tab_Weather_ImgWallpaper.Source = null;
                return false;
            }
        }

        //Load Weather Summary Text
        async Task<bool> WeatherSummaryText()
        {
            try
            {
                //Load Weather Location Text
                string LocationName = vApplicationSettings["BgStatusWeatherCurrentLocationFull"].ToString();
                if (LocationName != "N/A" && !String.IsNullOrEmpty(LocationName)) { txt_ForecastLocation.Text = LocationName; }
                else { txt_ForecastLocation.Text = "Unknown location"; }

                //Load summary from summary json
                JObject SummaryJObject;
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeWeatherSummary.js"))
                {
                    using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                    {
                        SummaryJObject = JObject.Parse(await StreamReader.ReadToEndAsync());
                        OpenStreamForReadAsync.Dispose();
                    }
                }
                //Check if there is weather summary available
                if (SummaryJObject["responses"][0]["weather"] == null || SummaryJObject["responses"][0]["weather"][0]["current"].Count() <= 1)
                {
                    txt_ForecastInformation.Text = "The weather information is currently not available\nplease wait for the next planned live tile update\nor manually update the weather on the tile tab.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.\n\nCause: your location might be unknown.";
                    txt_ForecastInformation.Visibility = Visibility.Visible;
                    sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                    sv_WeatherForecast.Visibility = Visibility.Collapsed;
                    tab_Weather_ImgTemperaturePre.Source = null;
                    tab_Weather_ImgTemperatureCur.Source = null;
                    tab_Weather_ImgWallpaper.Source = null;
                    return false;
                }
                else
                {
                    JToken AlertsJToken = SummaryJObject["responses"][0]["weather"][0]["alerts"];
                    JToken CurrentJToken = SummaryJObject["responses"][0]["weather"][0]["current"];
                    JToken ForecastJToken = SummaryJObject["responses"][0]["weather"][0]["forecast"]["days"][0];
                    JToken UnitsJToken = SummaryJObject["units"];

                    //Set Weather Alerts
                    if (AlertsJToken != null)
                    {
                        if (AlertsJToken.ToString() == "0") { txt_ForecastLocation.Foreground = new SolidColorBrush(Colors.White); }
                        else { txt_ForecastLocation.Foreground = new SolidColorBrush(Colors.Orange); }
                    }

                    //Set weather current temperature and color
                    if (CurrentJToken["temp"] != null)
                    {
                        string WeatherTemperature = CurrentJToken["temp"].ToString();
                        if (!String.IsNullOrEmpty(WeatherTemperature))
                        {
                            int WeatherTemp = Convert.ToInt32(WeatherTemperature);
                            int WeatherColdTemp = 18;
                            if ((int)vApplicationSettings["FahrenheitCelsius"] == 0) { WeatherColdTemp = 64; }
                            if (WeatherTemp < WeatherColdTemp)
                            {
                                txt_ForecastTemperatureLow.Text = WeatherTemp + "°";
                                txt_ForecastTemperatureHigh.Text = "";
                            }
                            else
                            {
                                txt_ForecastTemperatureLow.Text = "";
                                txt_ForecastTemperatureHigh.Text = WeatherTemp + "°";
                            }

                            WeatherTemperatureBackground(WeatherTemp);
                        }
                        else { txt_ForecastTemperatureLow.Text = "N/A"; txt_ForecastTemperatureHigh.Text = ""; }
                    }
                    else { txt_ForecastTemperatureLow.Text = "N/A"; txt_ForecastTemperatureHigh.Text = ""; }

                    //Load current wallpaper as underlayer
                    if ((bool)vApplicationSettings["WeatherDisplayWallpaper"])
                    {
                        if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                        {
                            StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMeTilePhoto.png");
                            using (IRandomAccessStream OpenAsync = await StorageFile.OpenAsync(FileAccessMode.Read))
                            {
                                BitmapImage BitmapImage = new BitmapImage();
                                await BitmapImage.SetSourceAsync(OpenAsync);
                                OpenAsync.Dispose();
                                tab_Weather_ImgWallpaper.Source = BitmapImage;
                            }
                        }
                        else { tab_Weather_ImgWallpaper.Source = null; }
                    }
                    else { tab_Weather_ImgWallpaper.Source = null; }

                    //Set Weather Icon
                    string WeatherIconStyle = "Weather";
                    if ((bool)vApplicationSettings["DisplayWeatherWhiteIcons"]) { WeatherIconStyle = "WeatherWhite"; }
                    if (CurrentJToken["icon"] != null)
                    {
                        string WeatherIcon = CurrentJToken["icon"].ToString();
                        if (!String.IsNullOrEmpty(WeatherIcon))
                        {
                            if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png")) { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png", UriKind.Absolute) }; }
                            else { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) }; }
                        }
                        else { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) }; }
                    }
                    else { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) }; }

                    //Set Weather Condition
                    if (CurrentJToken["cap"] != null)
                    {
                        string WeatherCondition = CurrentJToken["cap"].ToString();
                        if (!String.IsNullOrEmpty(WeatherCondition)) { txt_ForecastCondition.Text = WeatherCondition; }
                        else { txt_ForecastCondition.Text = "Not Available"; }
                    }
                    else { txt_ForecastCondition.Text = "Not Available"; }

                    //Set Weather Humidity
                    if (CurrentJToken["rh"] != null)
                    {
                        string WeatherHumidity = CurrentJToken["rh"].ToString();
                        if (!String.IsNullOrEmpty(WeatherHumidity)) { sp_WeatherHumidity.Visibility = Visibility.Visible; txt_WeatherHumidity.Text = WeatherHumidity + "% Humidity"; }
                        else { sp_WeatherHumidity.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherHumidity.Visibility = Visibility.Collapsed; }

                    //Set Weather Visibility
                    if (CurrentJToken["vis"] != null)
                    {
                        string WeatherVisibility = CurrentJToken["vis"].ToString();
                        if (!String.IsNullOrEmpty(WeatherVisibility)) { sp_WeatherVisibility.Visibility = Visibility.Visible; txt_WeatherVisibility.Text = WeatherVisibility + " " + UnitsJToken["distance"].ToString() + " Visibility"; }
                        else { sp_WeatherVisibility.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherVisibility.Visibility = Visibility.Collapsed; }

                    //Set Weather Wind Speed and Direction
                    if (CurrentJToken["windSpd"] != null && CurrentJToken["windDir"] != null)
                    {
                        string WeatherWindSpeed = CurrentJToken["windSpd"].ToString();
                        string WeatherWindDirection = AVFunctions.DegreesToCardinal(Convert.ToDouble((CurrentJToken["windDir"].ToString())));

                        if (!String.IsNullOrEmpty(WeatherWindSpeed) && !String.IsNullOrEmpty(WeatherWindDirection))
                        {
                            sp_WeatherWindSpeed.Visibility = Visibility.Visible;
                            txt_WeatherWindSpeed.Text = WeatherWindSpeed + " " + UnitsJToken["speed"].ToString() + " " + WeatherWindDirection + " Windspeed";
                        }
                        else { sp_WeatherWindSpeed.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherWindSpeed.Visibility = Visibility.Collapsed; }

                    //Set Weather Pressure
                    if (CurrentJToken["baro"] != null)
                    {
                        string WeatherPressure = CurrentJToken["baro"].ToString();
                        if (!String.IsNullOrEmpty(WeatherPressure)) { sp_WeatherPressure.Visibility = Visibility.Visible; txt_WeatherPressure.Text = WeatherPressure + " " + UnitsJToken["pressure"].ToString() + " Pressure"; }
                        else { sp_WeatherPressure.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherPressure.Visibility = Visibility.Collapsed; }

                    //Set Weather Precipitation
                    if (ForecastJToken["precip"] != null)
                    {
                        string WeatherPrecip = ForecastJToken["precip"].ToString();
                        if (!String.IsNullOrEmpty(WeatherPrecip)) { sp_WeatherPrecipitation.Visibility = Visibility.Visible; txt_WeatherPrecipitation.Text = WeatherPrecip + "% Rain Chance"; }
                        else { sp_WeatherPrecipitation.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherPrecipitation.Visibility = Visibility.Collapsed; }
                }
            }
            catch
            {
                txt_ForecastInformation.Text = "Failed to load the weather summary\nplease check your internet connection,\nmake sure your location service is enabled\nor wait for the next planned live tile update.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                txt_ForecastInformation.Visibility = Visibility.Visible;
                sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                sv_WeatherForecast.Visibility = Visibility.Collapsed;
                tab_Weather_ImgTemperaturePre.Source = null;
                tab_Weather_ImgTemperatureCur.Source = null;
                tab_Weather_ImgWallpaper.Source = null;
                return false;
            }

            //Load summary from forecast json
            try
            {
                JObject ForecastJObject;
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeWeatherForecast.js"))
                {
                    using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                    {
                        ForecastJObject = JObject.Parse(await StreamReader.ReadToEndAsync());
                        OpenStreamForReadAsync.Dispose();
                    }
                }
                //Check if there is weather forecast available
                if (ForecastJObject["responses"][0]["weather"] == null || ForecastJObject["responses"][0]["weather"][0]["days"].Count() <= 1)
                {
                    txt_ForecastInformation.Text = "The weather information is currently not available\nplease wait for the next planned live tile update\nor manually update the weather on the tile tab.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.\n\nCause: your location might be unknown.";
                    txt_ForecastInformation.Visibility = Visibility.Visible;
                    sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                    sv_WeatherForecast.Visibility = Visibility.Collapsed;
                    tab_Weather_ImgTemperaturePre.Source = null;
                    tab_Weather_ImgTemperatureCur.Source = null;
                    tab_Weather_ImgWallpaper.Source = null;
                    return false;
                }
                else
                {
                    JToken ForecastJToken = ForecastJObject["responses"][0]["weather"][0]["days"];

                    //Set Weather Uv
                    if (ForecastJToken[0]["daily"]["uv"] != null && ForecastJToken[0]["daily"]["uvDesc"] != null)
                    {
                        string WeatherUv = ForecastJToken[0]["daily"]["uv"].ToString();
                        string WeatherUvDesc = ForecastJToken[0]["daily"]["uvDesc"].ToString();
                        if (!String.IsNullOrEmpty(WeatherUv) && !String.IsNullOrEmpty(WeatherUvDesc)) { sp_WeatherUv.Visibility = Visibility.Visible; txt_WeatherUv.Text = WeatherUv + " (" + WeatherUvDesc + ") UV Index"; }
                        else { sp_WeatherUv.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherUv.Visibility = Visibility.Collapsed; }

                    //Set Weather Sunrise
                    sp_WeatherSunrise.Visibility = Visibility.Collapsed;

                    //Set Weather Sunset
                    if (ForecastJToken[0]["almanac"] != null && ForecastJToken[0]["almanac"]["sunset"] != null)
                    {
                        string WeatherSunset = ForecastJToken[0]["almanac"]["sunset"].ToString();
                        if (!String.IsNullOrEmpty(WeatherSunset))
                        {
                            sp_WeatherSunset.Visibility = Visibility.Visible;
                            DateTime SunsetDateTime = DateTime.Parse(WeatherSunset);
                            if ((bool)vApplicationSettings["Display24hClock"]) { txt_WeatherSunset.Text = SunsetDateTime.ToString("HH:mm") + " Sunset"; }
                            else { txt_WeatherSunset.Text = SunsetDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunset"; }
                        }
                        else { sp_WeatherSunset.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherSunset.Visibility = Visibility.Collapsed; }
                }
                return true;
            }
            catch
            {
                sp_WeatherUv.Visibility = Visibility.Collapsed;
                sp_WeatherSunrise.Visibility = Visibility.Collapsed;
                sp_WeatherSunset.Visibility = Visibility.Collapsed;
                return false;
            }
        }

        //Load Weather Forecast Text
        async Task<bool> WeatherForecastText(int ForecastId)
        {
            try
            {
                //Load forecast from forecast json
                JObject ForecastJObject;
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeWeatherForecast.js"))
                {
                    using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                    {
                        ForecastJObject = JObject.Parse(await StreamReader.ReadToEndAsync());
                        OpenStreamForReadAsync.Dispose();
                    }
                }
                //Check if there is weather forecast available
                if (ForecastJObject["responses"][0]["weather"] == null || ForecastJObject["responses"][0]["weather"][0]["days"].Count() <= 1)
                {
                    txt_ForecastInformation.Text = "The weather information is currently not available\nplease wait for the next planned live tile update\nor manually update the weather on the tile tab.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.\n\nCause: your location might be unknown.";
                    txt_ForecastInformation.Visibility = Visibility.Visible;
                    sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                    sv_WeatherForecast.Visibility = Visibility.Collapsed;
                    tab_Weather_ImgTemperaturePre.Source = null;
                    tab_Weather_ImgTemperatureCur.Source = null;
                    tab_Weather_ImgWallpaper.Source = null;
                    return false;
                }
                else
                {
                    JToken ForecastJToken = ForecastJObject["responses"][0]["weather"][0]["days"][ForecastId];
                    JToken UnitsJToken = ForecastJObject["units"];

                    //Set Weather Alerts
                    txt_ForecastLocation.Foreground = new SolidColorBrush(Colors.White);

                    //Set Weather Date
                    if (ForecastJToken["daily"]["valid"] != null)
                    {
                        string WeatherDate = ForecastJToken["daily"]["valid"].ToString();
                        if (!String.IsNullOrEmpty(WeatherDate))
                        {
                            DateTime WeatherDateTime = DateTime.Parse(WeatherDate);
                            if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { txt_ForecastLocation.Text = AVFunctions.ToTitleCase(WeatherDateTime.ToString("dddd, d MMMM", vCultureInfoReg)); }
                            else { txt_ForecastLocation.Text = WeatherDateTime.ToString("dddd, d MMMM", vCultureInfoEng); }
                        }
                        else { txt_ForecastLocation.Text = "Not available"; }
                    }
                    else { txt_ForecastLocation.Text = "Not available"; }

                    //Set Weather Highest Temperature
                    if (ForecastJToken["daily"]["tempHi"] != null)
                    {
                        string WeatherTempHigh = ForecastJToken["daily"]["tempHi"].ToString();
                        if (!String.IsNullOrEmpty(WeatherTempHigh)) { txt_ForecastTemperatureHigh.Text = WeatherTempHigh + "°"; }
                        else { txt_ForecastTemperatureHigh.Text = "N/A"; }

                        WeatherTemperatureBackground(Convert.ToInt32(WeatherTempHigh));
                    }
                    else { txt_ForecastTemperatureHigh.Text = "N/A"; }

                    //Set Weather Lowest Temperature
                    if (ForecastJToken["daily"]["tempLo"] != null)
                    {
                        string WeatherTempLow = ForecastJToken["daily"]["tempLo"].ToString();
                        if (!String.IsNullOrEmpty(WeatherTempLow)) { txt_ForecastTemperatureLow.Text = " " + WeatherTempLow + "°"; }
                        else { txt_ForecastTemperatureLow.Text = " N/A"; }
                    }
                    else { txt_ForecastTemperatureLow.Text = " N/A"; }

                    //Set Weather Icon
                    string WeatherIconStyle = "Weather";
                    if ((bool)vApplicationSettings["DisplayWeatherWhiteIcons"]) { WeatherIconStyle = "WeatherWhite"; }
                    if (ForecastJToken["daily"]["icon"] != null)
                    {
                        string WeatherIcon = ForecastJToken["daily"]["icon"].ToString();
                        if (!String.IsNullOrEmpty(WeatherIcon))
                        {
                            if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png")) { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png", UriKind.Absolute) }; }
                            else { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) }; }
                        }
                        else { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) }; }
                    }
                    else { img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) }; }

                    //Set Weather Condition
                    if (ForecastJToken["daily"]["pvdrCap"] != null)
                    {
                        string WeatherCondition = ForecastJToken["daily"]["pvdrCap"].ToString();
                        if (!String.IsNullOrEmpty(WeatherCondition)) { txt_ForecastCondition.Text = WeatherCondition; }
                        else { txt_ForecastCondition.Text = "Not Available"; }
                    }
                    else { txt_ForecastCondition.Text = "Not Available"; }

                    //Set Weather Humidity
                    if (ForecastJToken["daily"]["rhHi"] != null)
                    {
                        string WeatherHumidity = ForecastJToken["daily"]["rhHi"].ToString();
                        if (!String.IsNullOrEmpty(WeatherHumidity)) { sp_WeatherHumidity.Visibility = Visibility.Visible; txt_WeatherHumidity.Text = WeatherHumidity + "% Humidity"; }
                        else { sp_WeatherHumidity.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherHumidity.Visibility = Visibility.Collapsed; }

                    //Set Weather Visibility (Not Available)
                    sp_WeatherVisibility.Visibility = Visibility.Collapsed;

                    //Set Weather Wind speed and direction
                    if (ForecastJToken["daily"]["windMax"] != null && ForecastJToken["daily"]["windMaxDir"] != null)
                    {
                        string WeatherWindSpeed = ForecastJToken["daily"]["windMax"].ToString();
                        string WeatherWindDirection = AVFunctions.DegreesToCardinal(Convert.ToDouble((ForecastJToken["daily"]["windMaxDir"].ToString())));

                        if (!String.IsNullOrEmpty(WeatherWindSpeed) && !String.IsNullOrEmpty(WeatherWindDirection))
                        {
                            sp_WeatherWindSpeed.Visibility = Visibility.Visible;
                            txt_WeatherWindSpeed.Text = WeatherWindSpeed + " " + UnitsJToken["speed"].ToString() + " " + WeatherWindDirection + " Windspeed";
                        }
                        else { sp_WeatherWindSpeed.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherWindSpeed.Visibility = Visibility.Collapsed; }

                    //Set Weather Pressure (Not Available)
                    sp_WeatherPressure.Visibility = Visibility.Collapsed;

                    //Set Weather Precipitation
                    if (ForecastJToken["daily"]["precip"] != null)
                    {
                        string WeatherPrecipitation = ForecastJToken["daily"]["precip"].ToString();
                        if (!String.IsNullOrEmpty(WeatherPrecipitation)) { sp_WeatherPrecipitation.Visibility = Visibility.Visible; txt_WeatherPrecipitation.Text = WeatherPrecipitation + "% Rain Chance"; }
                        else { sp_WeatherPrecipitation.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherPrecipitation.Visibility = Visibility.Collapsed; }

                    //Set Weather Uv
                    if (ForecastJToken["daily"]["uv"] != null && ForecastJToken["daily"]["uvDesc"] != null)
                    {
                        string Uv = ForecastJToken["daily"]["uv"].ToString();
                        string UvDesc = ForecastJToken["daily"]["uvDesc"].ToString();
                        if (!String.IsNullOrEmpty(Uv) && !String.IsNullOrEmpty(UvDesc)) { sp_WeatherUv.Visibility = Visibility.Visible; txt_WeatherUv.Text = Uv + " (" + UvDesc + ") UV Index"; }
                        else { sp_WeatherUv.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherUv.Visibility = Visibility.Collapsed; }

                    //Set Weather Sunrise
                    if (ForecastJToken["almanac"] != null && ForecastJToken["almanac"]["sunrise"] != null)
                    {
                        string Sunrise = ForecastJToken["almanac"]["sunrise"].ToString();
                        if (!String.IsNullOrEmpty(Sunrise))
                        {
                            sp_WeatherSunrise.Visibility = Visibility.Visible;
                            DateTime SunriseDateTime = DateTime.Parse(Sunrise);
                            if ((bool)vApplicationSettings["Display24hClock"]) { txt_WeatherSunrise.Text = SunriseDateTime.ToString("HH:mm") + " Sunrise"; }
                            else { txt_WeatherSunrise.Text = SunriseDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunrise"; }
                        }
                        else { sp_WeatherSunrise.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherSunrise.Visibility = Visibility.Collapsed; }

                    //Set Weather Sunset
                    if (ForecastJToken["almanac"] != null && ForecastJToken["almanac"]["sunset"] != null)
                    {
                        string Sunset = ForecastJToken["almanac"]["sunset"].ToString();
                        if (!String.IsNullOrEmpty(Sunset))
                        {
                            sp_WeatherSunset.Visibility = Visibility.Visible;
                            DateTime SunsetDateTime = DateTime.Parse(Sunset);
                            if ((bool)vApplicationSettings["Display24hClock"]) { txt_WeatherSunset.Text = SunsetDateTime.ToString("HH:mm") + " Sunset"; }
                            else { txt_WeatherSunset.Text = SunsetDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunset"; }
                        }
                        else { sp_WeatherSunset.Visibility = Visibility.Collapsed; }
                    }
                    else { sp_WeatherSunset.Visibility = Visibility.Collapsed; }
                }

                return true;
            }
            catch
            {
                txt_ForecastInformation.Text = "Failed to load the weather forecast text\nplease check your internet connection,\nmake sure your location service is enabled\nor wait for the next planned live tile update.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                txt_ForecastInformation.Visibility = Visibility.Visible;
                sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                sv_WeatherForecast.Visibility = Visibility.Collapsed;
                tab_Weather_ImgTemperaturePre.Source = null;
                tab_Weather_ImgTemperatureCur.Source = null;
                tab_Weather_ImgWallpaper.Source = null;
                return false;
            }
        }

        //Load Weather Forecast List
        async Task<bool> WeatherForecastList()
        {
            try
            {
                //Load forecast from forecast json
                JObject ForecastJObject;
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeWeatherForecast.js"))
                {
                    using (StreamReader StreamReader = new StreamReader(OpenStreamForReadAsync))
                    {
                        ForecastJObject = JObject.Parse(await StreamReader.ReadToEndAsync());
                        OpenStreamForReadAsync.Dispose();
                    }
                }
                //Check if there is weather forecast available
                if (ForecastJObject["responses"][0]["weather"] == null || ForecastJObject["responses"][0]["weather"][0]["days"].Count() <= 1)
                {
                    txt_ForecastInformation.Text = "The weather information is currently not available\nplease wait for the next planned live tile update\nor manually update the weather on the tile tab.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.\n\nCause: your location might be unknown.";
                    txt_ForecastInformation.Visibility = Visibility.Visible;
                    sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                    sv_WeatherForecast.Visibility = Visibility.Collapsed;
                    tab_Weather_ImgTemperaturePre.Source = null;
                    tab_Weather_ImgTemperatureCur.Source = null;
                    tab_Weather_ImgWallpaper.Source = null;
                    return false;
                }
                else
                {
                    JToken ForecastJToken = ForecastJObject["responses"][0]["weather"][0]["days"];
                    JToken UnitsJToken = ForecastJObject["units"];

                    //Set Overall Weather Forecast
                    lb_ForecastListBox.Items.Clear();
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
                                if (WeatherDateTime.AddDays(1) < DateTime.Now) { continue; }
                                if (WeatherDateTime.Day == DateTime.Now.Day) { WeatherDate = "Today's Forecast"; }
                                else
                                {
                                    if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { WeatherDate = AVFunctions.ToTitleCase(WeatherDateTime.ToString("ddd, d MMMM", vCultureInfoReg)); }
                                    else { WeatherDate = WeatherDateTime.ToString("ddd, d MMMM", vCultureInfoEng); }
                                }
                            }
                            else { WeatherDate = "Unknown"; }
                        }
                        else { WeatherDate = "Unknown"; }

                        //Set Weather Icon
                        string WeatherIconStyle = "Weather";
                        if ((bool)vApplicationSettings["DisplayWeatherWhiteIcons"]) { WeatherIconStyle = "WeatherWhite"; }
                        string WeatherIcon = "";
                        if (DayJToken["daily"]["icon"] != null)
                        {
                            WeatherIcon = DayJToken["daily"]["icon"].ToString();
                            if (!String.IsNullOrEmpty(WeatherIcon))
                            {
                                if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png")) { WeatherIcon = "/Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png"; }
                                else { WeatherIcon = "/Assets/" + WeatherIconStyle + "/0.png"; }
                            }
                            else { WeatherIcon = "/Assets/" + WeatherIconStyle + "/0.png"; }
                        }
                        else { WeatherIcon = "/Assets/" + WeatherIconStyle + "/0.png"; }

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

                        //Set Weather Condition
                        string WeatherConditionDay = "";
                        if (DayJToken["daily"]["pvdrCap"] != null)
                        {
                            WeatherConditionDay = DayJToken["daily"]["pvdrCap"].ToString();
                            if (String.IsNullOrEmpty(WeatherConditionDay)) { WeatherConditionDay = "Not Available"; }
                        }
                        else { WeatherConditionDay = "Not Available"; }

                        //Set Weather Precipitation
                        string WeatherPrecipitation = "";
                        if (DayJToken["daily"]["precip"] != null)
                        {
                            WeatherPrecipitation = DayJToken["daily"]["precip"].ToString();
                            if (!String.IsNullOrEmpty(WeatherPrecipitation)) { WeatherPrecipitation = WeatherPrecipitation + "%"; }
                            else { WeatherPrecipitation = "N/A"; }
                        }
                        else { WeatherPrecipitation = "N/A"; }

                        //Set Weather Wind Speed and Direction
                        string WeatherWindSpeed = "";
                        if (DayJToken["daily"]["windMax"] != null && DayJToken["daily"]["windMaxDir"] != null)
                        {
                            WeatherWindSpeed = DayJToken["daily"]["windMax"].ToString();
                            string WeatherWindDirection = AVFunctions.DegreesToCardinal(Convert.ToDouble((DayJToken["daily"]["windMaxDir"].ToString())));

                            if (!String.IsNullOrEmpty(WeatherWindSpeed) && !String.IsNullOrEmpty(WeatherWindDirection)) { WeatherWindSpeed = WeatherWindSpeed + " " + UnitsJToken["speed"].ToString() + " " + WeatherWindDirection; }
                            else { WeatherWindSpeed = "N/A"; }
                        }
                        else { WeatherWindSpeed = "N/A"; }

                        lb_ForecastListBox.Items.Add(new WeatherList() { WeatherDate = WeatherDate, WeatherIcon = WeatherIcon, WeatherTempHigh = WeatherTempHigh, WeatherTempLow = WeatherTempLow, WeatherConditionDay = WeatherConditionDay, WeatherPrecipitation = WeatherPrecipitation, WeatherWindSpeed = WeatherWindSpeed });
                    }

                    //Select the first weather forecast
                    if (lb_ForecastListBox.Items.Any()) { lb_ForecastListBox.SelectedIndex = 0; }
                    return true;
                }
            }
            catch { return false; }
        }

        //Load the Weather Status
        void WeatherStatus()
        {
            try
            {
                //Set the weather update time text
                string ConvertedTime = "Unknown";
                string ConvertedDate = "Unknown";
                string BgStatusDownloadWeatherTime = vApplicationSettings["BgStatusDownloadWeatherTime"].ToString();
                if (BgStatusDownloadWeatherTime != "Never")
                {
                    DateTime WeatherTime = DateTime.Parse(BgStatusDownloadWeatherTime, vCultureInfoEng);

                    if ((bool)vApplicationSettings["Display24hClock"]) { ConvertedTime = WeatherTime.ToString("HH:mm"); }
                    else { ConvertedTime = WeatherTime.ToString("h:mm tt", vCultureInfoEng); }

                    if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedDate = AVFunctions.ToTitleCase(WeatherTime.ToString("d MMMM yyyy", vCultureInfoReg)); }
                    else { ConvertedDate = WeatherTime.ToString("d MMMM yyyy", vCultureInfoEng); }
                }

                string BgStatusDownloadWeather = vApplicationSettings["BgStatusDownloadWeather"].ToString();
                if (BgStatusDownloadWeather == "Never") { txt_ForecastLastupdate.Text = "Last update: Not yet updated, please update.\nWeather from: " + ConvertedDate + ", " + ConvertedTime; }
                else if (BgStatusDownloadWeather == "Failed") { txt_ForecastLastupdate.Text = "Last update: Failed, check GPS and Internet.\nWeather from: " + ConvertedDate + ", " + ConvertedTime; }
                else { txt_ForecastLastupdate.Text = "Last update: " + ConvertedDate + ", " + ConvertedTime; }

                //Hide and show weather ui elements
                txt_ForecastInformation.Visibility = Visibility.Collapsed;
                sp_WeatherCurrent.Visibility = Visibility.Visible;
                sv_WeatherForecast.Visibility = Visibility.Visible;
            }
            catch
            {
                txt_ForecastInformation.Text = "Failed to load the weather information\nplease check your internet connection,\nmake sure your location service is enabled\nor wait for the next planned live tile update.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                txt_ForecastInformation.Visibility = Visibility.Visible;
                sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                sv_WeatherForecast.Visibility = Visibility.Collapsed;
                tab_Weather_ImgTemperaturePre.Source = null;
                tab_Weather_ImgTemperatureCur.Source = null;
                tab_Weather_ImgWallpaper.Source = null;
            }
        }

        //Set weather temperature background
        void WeatherTemperatureBackground(int WeatherTemp)
        {
            try
            {
                //Set background wallpaper temperature
                int WeatherColdTemp = 18;
                if ((int)vApplicationSettings["FahrenheitCelsius"] == 0) { WeatherColdTemp = 64; }
                if (WeatherTemp < WeatherColdTemp) { AVAnimations.Ani_ImageFadeInandOut(tab_Weather_ImgTemperaturePre, tab_Weather_ImgTemperatureCur, new BitmapImage(new Uri("ms-appx:///Assets/WeatherOther/BackgroundLow.png", UriKind.Absolute))); }
                else { AVAnimations.Ani_ImageFadeInandOut(tab_Weather_ImgTemperaturePre, tab_Weather_ImgTemperatureCur, new BitmapImage(new Uri("ms-appx:///Assets/WeatherOther/BackgroundHigh.png", UriKind.Absolute))); }
            }
            catch { }
        }

        //Handle listbox item selection changed
        async void lb_ForecastListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                ListBox ListBox = (ListBox)sender;
                if (ListBox.SelectedIndex >= 0)
                {
                    if (ListBox.SelectedIndex == 0) { await WeatherSummaryText(); }
                    else
                    {
                        //Check if weather update is from today or days ago
                        string BgStatusDownloadWeather = vApplicationSettings["BgStatusDownloadWeather"].ToString();
                        if (BgStatusDownloadWeather != "Never" && BgStatusDownloadWeather != "Failed")
                        {
                            int DaysDifference = DateTime.Now.Date.Subtract(DateTime.Parse(BgStatusDownloadWeather, vCultureInfoEng).Date).Days;
                            await WeatherForecastText(ListBox.SelectedIndex + DaysDifference);
                        }
                    }
                }
            }
            catch { }
        }
    }
}