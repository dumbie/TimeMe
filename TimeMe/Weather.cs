using ArnoldVinkCode;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimeMeShared.Classes.ApiOpenMeteo;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
                await WeatherUpdateWallpaper();
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
                if (!await AVFunctions.LocalFileExists("TimeMeWeatherSummary.js"))
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
                if (LocationName != "N/A" && !string.IsNullOrEmpty(LocationName))
                {
                    txt_ForecastLocation.Text = LocationName;
                }
                else
                {
                    txt_ForecastLocation.Text = "Unknown location";
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
                    //Set Weather Units
                    string unitsTemperature = "°";
                    string unitsSpeed = jsonForecast.daily_units.windspeed_10m_max;
                    string unitsPrecipitation = jsonForecast.daily_units.precipitation_sum;

                    //Set Weather Humidity (Not available)
                    sp_WeatherHumidity.Visibility = Visibility.Collapsed;

                    //Set Weather Visibility (Not available)
                    sp_WeatherVisibility.Visibility = Visibility.Collapsed;

                    //Set Weather Pressure (Not available)
                    sp_WeatherPressure.Visibility = Visibility.Collapsed;

                    //Set Weather Uv (Not available)
                    sp_WeatherUv.Visibility = Visibility.Collapsed;

                    //Set Weather temperature
                    float WeatherTemp = jsonForecast.current_weather.temperature;
                    int WeatherColdTemp = (int)vApplicationSettings["FahrenheitCelsius"] == 1 ? 18 : 64;
                    if (WeatherTemp < WeatherColdTemp)
                    {
                        txt_ForecastTemperatureLow.Text = WeatherTemp + unitsTemperature;
                        txt_ForecastTemperatureHigh.Text = string.Empty;
                    }
                    else
                    {
                        txt_ForecastTemperatureLow.Text = string.Empty;
                        txt_ForecastTemperatureHigh.Text = WeatherTemp + unitsTemperature;
                    }

                    //Update weather temperature background
                    WeatherTemperatureBackground(WeatherTemp);

                    //Set Weather Icon
                    string WeatherIcon = jsonForecast.current_weather.weathercode.ToString();
                    string WeatherIconStyle = (bool)vApplicationSettings["DisplayWeatherWhiteIcons"] ? "WeatherWhite" : "Weather";
                    if (!string.IsNullOrEmpty(WeatherIcon))
                    {
                        if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png"))
                        {
                            img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png", UriKind.Absolute) };
                        }
                        else
                        {
                            img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) };
                        }
                    }
                    else
                    {
                        img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) };
                    }

                    //Set Weather Condition
                    string WeatherCondition = ApiOpenMeteo.WmoCodeToString(jsonForecast.current_weather.weathercode);
                    if (!string.IsNullOrEmpty(WeatherCondition))
                    {
                        txt_ForecastCondition.Text = WeatherCondition;
                    }
                    else
                    {
                        txt_ForecastCondition.Text = "Not Available";
                    }

                    //Set Weather Wind Speed and Direction
                    string WeatherWindSpeed = jsonForecast.current_weather.windspeed.ToString() + unitsSpeed;
                    string WeatherWindDirection = AVFunctions.DegreesToCardinal(jsonForecast.current_weather.winddirection);
                    if (!string.IsNullOrEmpty(WeatherWindSpeed) && !string.IsNullOrEmpty(WeatherWindDirection))
                    {
                        sp_WeatherWindSpeed.Visibility = Visibility.Visible;
                        txt_WeatherWindSpeed.Text = WeatherWindSpeed + " " + WeatherWindDirection;
                    }
                    else
                    {
                        sp_WeatherWindSpeed.Visibility = Visibility.Collapsed;
                    }

                    //Set Weather Precipitation
                    string WeatherPrecip = jsonForecast.daily.precipitation_sum[0] + unitsPrecipitation;
                    if (!string.IsNullOrEmpty(WeatherPrecip))
                    {
                        sp_WeatherPrecipitation.Visibility = Visibility.Visible;
                        txt_WeatherPrecipitation.Text = WeatherPrecip;
                    }
                    else
                    {
                        sp_WeatherPrecipitation.Visibility = Visibility.Collapsed;
                    }

                    //Set Weather Sunrise
                    string WeatherSunrise = jsonForecast.daily.sunrise[0];
                    if (!string.IsNullOrEmpty(WeatherSunrise))
                    {
                        DateTime SunriseDateTime = DateTime.Parse(WeatherSunrise);
                        if ((bool)vApplicationSettings["Display24hClock"])
                        {
                            txt_WeatherSunrise.Text = SunriseDateTime.ToString("HH:mm") + " Sunrise";
                        }
                        else
                        {
                            txt_WeatherSunrise.Text = SunriseDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunrise";
                        }
                        sp_WeatherSunrise.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        sp_WeatherSunrise.Visibility = Visibility.Collapsed;
                    }

                    //Set Weather Sunset
                    string WeatherSunset = jsonForecast.daily.sunset[0];
                    if (!string.IsNullOrEmpty(WeatherSunset))
                    {
                        DateTime SunsetDateTime = DateTime.Parse(WeatherSunset);
                        if ((bool)vApplicationSettings["Display24hClock"])
                        {
                            txt_WeatherSunset.Text = SunsetDateTime.ToString("HH:mm") + " Sunset";
                        }
                        else
                        {
                            txt_WeatherSunset.Text = SunsetDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunset";
                        }
                        sp_WeatherSunset.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        sp_WeatherSunset.Visibility = Visibility.Collapsed;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed loading weather summary: " + ex.Message);
                txt_ForecastInformation.Text = "Failed to load the weather summary\nplease check your internet connection,\nmake sure your location service is enabled\nor wait for the next planned live tile update.\n\nPlease note: the weather refreshes itself here\nautomatically when it becomes available.";
                txt_ForecastInformation.Visibility = Visibility.Visible;
                sp_WeatherCurrent.Visibility = Visibility.Collapsed;
                sv_WeatherForecast.Visibility = Visibility.Collapsed;
                tab_Weather_ImgTemperaturePre.Source = null;
                tab_Weather_ImgTemperatureCur.Source = null;
                tab_Weather_ImgWallpaper.Source = null;
                return false;
            }
        }

        //Load current wallpaper as underlayer
        async Task WeatherUpdateWallpaper()
        {
            try
            {
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
                    else
                    {
                        tab_Weather_ImgWallpaper.Source = null;
                    }
                }
                else
                {
                    tab_Weather_ImgWallpaper.Source = null;
                }
            }
            catch { }
        }

        //Load Weather Forecast Text
        async Task<bool> WeatherForecastText(int ForecastId)
        {
            try
            {
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
                    //Set Weather Units
                    string unitsTemperature = "°";
                    string unitsSpeed = jsonForecast.daily_units.windspeed_10m_max;
                    string unitsPrecipitation = jsonForecast.daily_units.precipitation_sum;

                    //Set Weather Humidity (Not available)
                    sp_WeatherHumidity.Visibility = Visibility.Collapsed;

                    //Set Weather Visibility (Not available)
                    sp_WeatherVisibility.Visibility = Visibility.Collapsed;

                    //Set Weather Pressure (Not available)
                    sp_WeatherPressure.Visibility = Visibility.Collapsed;

                    //Set Weather Uv (Not available)
                    sp_WeatherUv.Visibility = Visibility.Collapsed;

                    //Set Weather Date
                    DateTime WeatherDateTime = DateTime.Now.AddDays(ForecastId);
                    if (WeatherDateTime.Day == DateTime.Now.Day)
                    {
                        txt_ForecastLocation.Text = "Today's Forecast";
                    }
                    else
                    {
                        if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                        {
                            txt_ForecastLocation.Text = AVFunctions.ToTitleCase(WeatherDateTime.ToString("ddd, d MMMM", vCultureInfoReg));
                        }
                        else
                        {
                            txt_ForecastLocation.Text = WeatherDateTime.ToString("ddd, d MMMM", vCultureInfoEng);
                        }
                    }

                    //Set weather temperature
                    float WeatherTemp = jsonForecast.daily.temperature_2m_max[ForecastId];
                    int WeatherColdTemp = (int)vApplicationSettings["FahrenheitCelsius"] == 1 ? 18 : 64;
                    if (WeatherTemp < WeatherColdTemp)
                    {
                        txt_ForecastTemperatureLow.Text = WeatherTemp + unitsTemperature;
                        txt_ForecastTemperatureHigh.Text = string.Empty;
                    }
                    else
                    {
                        txt_ForecastTemperatureLow.Text = string.Empty;
                        txt_ForecastTemperatureHigh.Text = WeatherTemp + unitsTemperature;
                    }

                    //Update weather temperature background
                    WeatherTemperatureBackground(WeatherTemp);

                    //Set Weather Icon
                    string WeatherIcon = jsonForecast.daily.weathercode[ForecastId].ToString();
                    string WeatherIconStyle = (bool)vApplicationSettings["DisplayWeatherWhiteIcons"] ? "WeatherWhite" : "Weather";
                    if (!string.IsNullOrEmpty(WeatherIcon))
                    {
                        if (await AVFunctions.AppFileExists("Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png"))
                        {
                            img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/" + WeatherIcon + ".png", UriKind.Absolute) };
                        }
                        else
                        {
                            img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) };
                        }
                    }
                    else
                    {
                        img_ForecastIcon.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/" + WeatherIconStyle + "/0.png", UriKind.Absolute) };
                    }

                    //Set Weather Condition
                    string WeatherCondition = ApiOpenMeteo.WmoCodeToString(jsonForecast.daily.weathercode[ForecastId]);
                    if (!string.IsNullOrEmpty(WeatherCondition))
                    {
                        txt_ForecastCondition.Text = WeatherCondition;
                    }
                    else
                    {
                        txt_ForecastCondition.Text = "Not Available";
                    }

                    //Set Weather Wind Speed and Direction
                    string WeatherWindSpeed = jsonForecast.daily.windspeed_10m_max[ForecastId].ToString() + unitsSpeed;
                    string WeatherWindDirection = AVFunctions.DegreesToCardinal(jsonForecast.daily.winddirection_10m_dominant[ForecastId]);
                    if (!string.IsNullOrEmpty(WeatherWindSpeed) && !string.IsNullOrEmpty(WeatherWindDirection))
                    {
                        sp_WeatherWindSpeed.Visibility = Visibility.Visible;
                        txt_WeatherWindSpeed.Text = WeatherWindSpeed + " " + WeatherWindDirection;
                    }
                    else
                    {
                        sp_WeatherWindSpeed.Visibility = Visibility.Collapsed;
                    }

                    //Set Weather Precipitation
                    string WeatherPrecip = jsonForecast.daily.precipitation_sum[ForecastId] + unitsPrecipitation;
                    if (!string.IsNullOrEmpty(WeatherPrecip))
                    {
                        sp_WeatherPrecipitation.Visibility = Visibility.Visible;
                        txt_WeatherPrecipitation.Text = WeatherPrecip;
                    }
                    else
                    {
                        sp_WeatherPrecipitation.Visibility = Visibility.Collapsed;
                    }

                    //Set Weather Sunrise
                    string WeatherSunrise = jsonForecast.daily.sunrise[ForecastId];
                    if (!string.IsNullOrEmpty(WeatherSunrise))
                    {
                        DateTime SunriseDateTime = DateTime.Parse(WeatherSunrise);
                        if ((bool)vApplicationSettings["Display24hClock"])
                        {
                            txt_WeatherSunrise.Text = SunriseDateTime.ToString("HH:mm") + " Sunrise";
                        }
                        else
                        {
                            txt_WeatherSunrise.Text = SunriseDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunrise";
                        }
                        sp_WeatherSunrise.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        sp_WeatherSunrise.Visibility = Visibility.Collapsed;
                    }

                    //Set Weather Sunset
                    string WeatherSunset = jsonForecast.daily.sunset[ForecastId];
                    if (!string.IsNullOrEmpty(WeatherSunset))
                    {
                        DateTime SunsetDateTime = DateTime.Parse(WeatherSunset);
                        if ((bool)vApplicationSettings["Display24hClock"])
                        {
                            txt_WeatherSunset.Text = SunsetDateTime.ToString("HH:mm") + " Sunset";
                        }
                        else
                        {
                            txt_WeatherSunset.Text = SunsetDateTime.ToString("h:mm tt", vCultureInfoEng) + " Sunset";
                        }
                        sp_WeatherSunset.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        sp_WeatherSunset.Visibility = Visibility.Collapsed;
                    }
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
                    //Clear current forecast
                    lb_ForecastListBox.Items.Clear();

                    //Set Weather Units
                    string unitsTemperature = "°";
                    string unitsSpeed = jsonForecast.daily_units.windspeed_10m_max;
                    string unitsPrecipitation = jsonForecast.daily_units.precipitation_sum;

                    //Count available days
                    int forecastDayCount = jsonForecast.daily.time.Count();

                    //Set Overall Weather Forecast
                    for (int i = 0; i < forecastDayCount; i++)
                    {
                        //Set Weather Date
                        string WeatherDate = jsonForecast.daily.time[i].ToString();
                        DateTime WeatherDateTime = DateTime.Parse(WeatherDate);
                        if (WeatherDateTime.Day == DateTime.Now.Day)
                        {
                            WeatherDate = "Today's Forecast";
                        }
                        else
                        {
                            if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                            {
                                WeatherDate = AVFunctions.ToTitleCase(WeatherDateTime.ToString("ddd, d MMMM", vCultureInfoReg));
                            }
                            else
                            {
                                WeatherDate = WeatherDateTime.ToString("ddd, d MMMM", vCultureInfoEng);
                            }
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

                        //Set Weather Condition
                        string WeatherConditionDay = ApiOpenMeteo.WmoCodeToString(jsonForecast.daily.weathercode[i]);
                        if (string.IsNullOrEmpty(WeatherConditionDay))
                        {
                            WeatherConditionDay = "Not Available";
                        }

                        //Set Weather Precipitation
                        string WeatherPrecipitation = jsonForecast.daily.precipitation_sum[i].ToString();
                        if (!string.IsNullOrEmpty(WeatherPrecipitation))
                        {
                            WeatherPrecipitation = WeatherPrecipitation + unitsPrecipitation;
                        }
                        else
                        {
                            WeatherPrecipitation = "N/A";
                        }

                        //Set Weather Wind Speed and Direction
                        string WeatherWindSpeed = jsonForecast.daily.windspeed_10m_max[i].ToString();
                        string WeatherWindDirection = AVFunctions.DegreesToCardinal(jsonForecast.daily.winddirection_10m_dominant[i]);
                        if (!string.IsNullOrEmpty(WeatherWindSpeed) && !string.IsNullOrEmpty(WeatherWindDirection))
                        {
                            WeatherWindSpeed += unitsSpeed + " " + WeatherWindDirection;
                        }
                        else
                        {
                            WeatherWindSpeed = "N/A";
                        }

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
                string BgStatusDownloadWeatherTime = vApplicationSettings["BgStatusDownloadWeatherTime"].ToString();
                if (BgStatusDownloadWeatherTime == "Never")
                {
                    txt_ForecastLastupdate.Text = "Last update: Not yet updated, please update.";
                }
                else if (BgStatusDownloadWeatherTime == "Failed")
                {
                    txt_ForecastLastupdate.Text = "Last update: Failed, check GPS and Internet.";
                }
                else
                {
                    string ConvertedTime = "Unknown";
                    string ConvertedDate = "Unknown";
                    DateTime WeatherTime = DateTime.Parse(BgStatusDownloadWeatherTime, vCultureInfoEng);
                    if ((bool)vApplicationSettings["Display24hClock"])
                    {
                        ConvertedTime = WeatherTime.ToString("HH:mm");
                    }
                    else
                    {
                        ConvertedTime = WeatherTime.ToString("h:mm tt", vCultureInfoEng);
                    }

                    if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                    {
                        ConvertedDate = AVFunctions.ToTitleCase(WeatherTime.ToString("d MMMM yyyy", vCultureInfoReg));
                    }
                    else
                    {
                        ConvertedDate = WeatherTime.ToString("d MMMM yyyy", vCultureInfoEng);
                    }

                    txt_ForecastLastupdate.Text = "Last update: " + ConvertedDate + ", " + ConvertedTime;
                }

                //Hide and show weather ui elements
                txt_ForecastInformation.Visibility = Visibility.Collapsed;
                sp_WeatherCurrent.Visibility = Visibility.Visible;
                sv_WeatherForecast.Visibility = Visibility.Visible;

                Debug.WriteLine("Weather is now visible.");
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
        void WeatherTemperatureBackground(float WeatherTemp)
        {
            try
            {
                //Set background wallpaper temperature
                int WeatherColdTemp = (int)vApplicationSettings["FahrenheitCelsius"] == 1 ? 18 : 64;
                if (WeatherTemp < WeatherColdTemp)
                {
                    AVAnimations.Ani_ImageFadeInandOut(tab_Weather_ImgTemperaturePre, tab_Weather_ImgTemperatureCur, new BitmapImage(new Uri("ms-appx:///Assets/WeatherOther/BackgroundLow.png", UriKind.Absolute)));
                }
                else
                {
                    AVAnimations.Ani_ImageFadeInandOut(tab_Weather_ImgTemperaturePre, tab_Weather_ImgTemperatureCur, new BitmapImage(new Uri("ms-appx:///Assets/WeatherOther/BackgroundHigh.png", UriKind.Absolute)));
                }
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
                    if (ListBox.SelectedIndex == 0)
                    {
                        await WeatherSummaryText();
                    }
                    else
                    {
                        //Check if weather update is from today or days ago
                        string BgStatusDownloadWeather = vApplicationSettings["BgStatusDownloadWeatherTime"].ToString();
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