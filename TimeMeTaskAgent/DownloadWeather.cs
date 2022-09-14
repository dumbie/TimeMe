using ArnoldVinkCode;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TimeMeShared.Classes.ApiOpenMeteo;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Download weather information
        async Task DownloadWeather()
        {
            try
            {
                Debug.WriteLine("Downloading weather information.");

                //Check for internet connection
                if (!DownloadInternetCheck())
                {
                    BackgroundStatusUpdateSettings("Never", null, null, null, "NoWifiEthernet");
                    return;
                }

                //Check if location is available
                if (string.IsNullOrEmpty(DownloadLocationGpsCombined))
                {
                    BackgroundStatusUpdateSettings("Never", null, null, null, "NoWeatherLocation");
                    return;
                }

                //Set api url and key
                string apiUrl = "https://api.open-meteo.com/v1/forecast/";
                string apiLocation = "?latitude=" + DownloadLocationGpsLatitude + "&longitude=" + DownloadLocationGpsLongitude;
                string apiWeather = "&daily=weathercode,temperature_2m_max,temperature_2m_min,sunrise,sunset,precipitation_sum,windspeed_10m_max,winddirection_10m_dominant&current_weather=true&timezone=auto";

                //Download and save weather
                string WeatherDownloadResult = await AVDownloader.DownloadStringAsync(5000, "TimeMe", null, new Uri(apiUrl + apiLocation + apiWeather));
                if (!string.IsNullOrEmpty(WeatherDownloadResult))
                {
                    //Update weather status
                    UpdateWeatherStatus(WeatherDownloadResult);

                    //Notification - Current Weather
                    ShowNotiWeatherCurrent();

                    //Save weather summary data
                    await AVFile.SaveText("TimeMeWeatherSummary.js", WeatherDownloadResult);
                }
                else
                {
                    Debug.WriteLine("Failed no weather found.");
                    BackgroundStatusUpdateSettings("Failed", null, null, null, "NoWeatherSummary");
                    return;
                }

                //Save Weather status
                BgStatusDownloadWeatherTime = DateTimeNow.ToString(vCultureInfoEng);
                vApplicationSettings["BgStatusDownloadWeatherTime"] = BgStatusDownloadWeatherTime;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating the weather info.");
                BackgroundStatusUpdateSettings("Failed", null, null, null, "CatchDownloadWeather" + ex.Message);
            }

            //Update weather status
            void UpdateWeatherStatus(string weatherResult)
            {
                try
                {
                    Forecast jsonForecast = JsonConvert.DeserializeObject<Forecast>(weatherResult);

                    //Set Weather Provider Information
                    BgStatusWeatherProvider = "Open Meteo";
                    vApplicationSettings["BgStatusWeatherProvider"] = BgStatusWeatherProvider;

                    //Set Weather Units
                    string unitsTemperature = "°";
                    string unitsSpeed = jsonForecast.daily_units.windspeed_10m_max;
                    string unitsPrecipitation = jsonForecast.daily_units.precipitation_sum;

                    //Set Weather Information
                    string Icon = jsonForecast.current_weather.weathercode.ToString();
                    string Condition = ApiOpenMeteo.WmoCodeToString(jsonForecast.current_weather.weathercode);
                    string Temperature = jsonForecast.current_weather.temperature.ToString() + unitsTemperature;
                    string WindSpeed = jsonForecast.current_weather.windspeed.ToString() + unitsSpeed;

                    //Set Weather Forecast Conditions
                    string RainFall = jsonForecast.daily.precipitation_sum[0].ToString() + unitsPrecipitation;
                    string TemperatureLow = jsonForecast.daily.temperature_2m_min[0].ToString() + unitsTemperature;
                    string TemperatureHigh = jsonForecast.daily.temperature_2m_max[0].ToString() + unitsTemperature;

                    //Set Weather Icon
                    if (!string.IsNullOrEmpty(Icon))
                    {
                        BgStatusWeatherCurrentIcon = Icon;
                        vApplicationSettings["BgStatusWeatherCurrentIcon"] = BgStatusWeatherCurrentIcon;
                    }
                    else
                    {
                        BgStatusWeatherCurrentIcon = "0";
                        vApplicationSettings["BgStatusWeatherCurrentIcon"] = BgStatusWeatherCurrentIcon;
                    }

                    //Set Weather Temperature and Condition
                    if (!string.IsNullOrEmpty(Temperature) && !string.IsNullOrEmpty(Condition))
                    {
                        BgStatusWeatherCurrent = AVFunctions.ToTitleCase(Condition) + ", " + Temperature;
                        vApplicationSettings["BgStatusWeatherCurrent"] = BgStatusWeatherCurrent;
                    }
                    else
                    {
                        BgStatusWeatherCurrent = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrent"] = BgStatusWeatherCurrent;
                    }

                    //Set Weather Temperature
                    if (!string.IsNullOrEmpty(Temperature))
                    {
                        BgStatusWeatherCurrentTemp = Temperature;
                        vApplicationSettings["BgStatusWeatherCurrentTemp"] = BgStatusWeatherCurrentTemp;
                    }
                    else
                    {
                        BgStatusWeatherCurrentTemp = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrentTemp"] = BgStatusWeatherCurrentTemp;
                    }

                    //Set Weather Condition
                    if (!string.IsNullOrEmpty(Condition))
                    {
                        BgStatusWeatherCurrentText = AVFunctions.ToTitleCase(Condition);
                        vApplicationSettings["BgStatusWeatherCurrentText"] = BgStatusWeatherCurrentText;
                    }
                    else
                    {
                        BgStatusWeatherCurrentText = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrentText"] = BgStatusWeatherCurrentText;
                    }

                    //Set Weather Wind Speed and Direction
                    if (!string.IsNullOrEmpty(WindSpeed))
                    {
                        BgStatusWeatherCurrentWindSpeed = WindSpeed;
                        vApplicationSettings["BgStatusWeatherCurrentWindSpeed"] = BgStatusWeatherCurrentWindSpeed;
                    }
                    else
                    {
                        BgStatusWeatherCurrentWindSpeed = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrentWindSpeed"] = BgStatusWeatherCurrentWindSpeed;
                    }

                    //Set Weather Rain Fall
                    if (!string.IsNullOrEmpty(RainFall))
                    {
                        BgStatusWeatherCurrentRainChance = RainFall;
                        vApplicationSettings["BgStatusWeatherCurrentRainChance"] = BgStatusWeatherCurrentRainChance;
                    }
                    else
                    {
                        BgStatusWeatherCurrentRainChance = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrentRainChance"] = BgStatusWeatherCurrentRainChance;
                    }

                    //Set Weather Temp Low
                    if (!string.IsNullOrEmpty(TemperatureLow))
                    {
                        BgStatusWeatherCurrentTempLow = TemperatureLow;
                        vApplicationSettings["BgStatusWeatherCurrentTempLow"] = BgStatusWeatherCurrentTempLow;
                    }
                    else
                    {
                        BgStatusWeatherCurrentTempLow = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrentTempLow"] = BgStatusWeatherCurrentTempLow;
                    }

                    //Set Weather Temp High
                    if (!string.IsNullOrEmpty(TemperatureHigh))
                    {
                        BgStatusWeatherCurrentTempHigh = TemperatureHigh;
                        vApplicationSettings["BgStatusWeatherCurrentTempHigh"] = BgStatusWeatherCurrentTempHigh;
                    }
                    else
                    {
                        BgStatusWeatherCurrentTempHigh = "N/A";
                        vApplicationSettings["BgStatusWeatherCurrentTempHigh"] = BgStatusWeatherCurrentTempHigh;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to update weather status: " + ex.Message);
                }
            }
        }
    }
}