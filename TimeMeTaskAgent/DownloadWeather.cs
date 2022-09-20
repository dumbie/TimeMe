using ArnoldVinkCode;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Download weather and forecast
        async Task DownloadWeather()
        {
            try
            {
                Debug.WriteLine("Downloading Weather update.");

                //Check for internet connection
                if (!DownloadInternetCheck())
                {
                    BackgroundStatusUpdateSettings("Never", null, null, null, "NoWifiEthernet");
                    return;
                }

                //Check if location is available
                if (String.IsNullOrEmpty(DownloadWeatherLocation))
                {
                    BackgroundStatusUpdateSettings("Never", null, null, null, "NoWeatherLocation");
                    return;
                }

                //Download and save weather summary
                string apiUrl = "https://api.msn.com/weather/overview?locale=" + DownloadWeatherLanguage + "&apiKey=OkWqHMuutahBXs3dBoygqCjgXRt6CV4i5V7SRQURrT" + DownloadWeatherUnits + DownloadWeatherLocation;
                string WeatherSummaryResult = await AVDownloader.DownloadStringAsync(5000, "TimeMe", null, new Uri(apiUrl));
                if (!String.IsNullOrEmpty(WeatherSummaryResult))
                {
                    //Update weather summary status
                    UpdateWeatherSummaryStatus(WeatherSummaryResult);

                    //Notification - Current Weather
                    ShowNotiWeatherCurrent();

                    //Save weather summary data
                    await AVFile.SaveText("TimeMeWeatherSummary.js", WeatherSummaryResult);
                }
                else
                {
                    Debug.WriteLine("Failed no weather summary found.");
                    BackgroundStatusUpdateSettings("Failed", null, null, null, "NoWeatherSummary");
                    return;
                }

                //Save Weather status
                BgStatusDownloadWeather = DateTimeNow.ToString(vCultureInfoEng);
                vApplicationSettings["BgStatusDownloadWeather"] = BgStatusDownloadWeather;
                BgStatusDownloadWeatherTime = BgStatusDownloadWeather;
                vApplicationSettings["BgStatusDownloadWeatherTime"] = BgStatusDownloadWeather;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating the weather info:" + ex.Message);
                BackgroundStatusUpdateSettings("Failed", null, null, null, "CatchDownloadWeather" + ex.Message);
            }

            //Update weather summary status
            void UpdateWeatherSummaryStatus(string WeatherSummaryResult)
            {
                try
                {
                    //Check if there is summary data available
                    JObject SummaryJObject = JObject.Parse(WeatherSummaryResult);
                    if (SummaryJObject["value"][0]["responses"][0]["weather"] != null)
                    {
                        //Set Weather Provider Information
                        JToken HttpJTokenProvider = SummaryJObject["value"][0]["responses"][0]["weather"][0]["provider"];
                        if (HttpJTokenProvider["name"] != null)
                        {
                            string Provider = HttpJTokenProvider["name"].ToString();
                            if (!String.IsNullOrEmpty(Provider)) { BgStatusWeatherProvider = Provider; vApplicationSettings["BgStatusWeatherProvider"] = BgStatusWeatherProvider; }
                            else { BgStatusWeatherProvider = "N/A"; vApplicationSettings["BgStatusWeatherProvider"] = BgStatusWeatherProvider; }
                        }

                        //Set Weather Current Conditions
                        string Icon = "";
                        string Condition = "";
                        string Temperature = "";
                        string WindSpeedDirection = "";
                        JToken UnitsJToken = SummaryJObject["value"][0]["units"];
                        JToken HttpJTokenCurrent = SummaryJObject["value"][0]["responses"][0]["weather"][0]["current"];
                        if (HttpJTokenCurrent["icon"] != null) { Icon = HttpJTokenCurrent["icon"].ToString(); }
                        if (HttpJTokenCurrent["cap"] != null) { Condition = HttpJTokenCurrent["cap"].ToString(); }
                        if (HttpJTokenCurrent["temp"] != null) { Temperature = HttpJTokenCurrent["temp"].ToString() + "°"; }
                        if (HttpJTokenCurrent["windSpd"] != null && HttpJTokenCurrent["windDir"] != null) { WindSpeedDirection = HttpJTokenCurrent["windSpd"].ToString() + " " + UnitsJToken["speed"].ToString() + " " + AVFunctions.DegreesToCardinal(Convert.ToDouble((HttpJTokenCurrent["windDir"].ToString()))); }

                        //Set Weather Forecast Conditions
                        string RainChance = "";
                        string TemperatureLow = "";
                        string TemperatureHigh = "";
                        JToken HttpJTokenForecast = SummaryJObject["value"][0]["responses"][0]["weather"][0]["forecast"]["days"][0];
                        if (HttpJTokenForecast["daily"]["precip"] != null) { RainChance = HttpJTokenForecast["daily"]["precip"].ToString() + "%"; }
                        if (HttpJTokenForecast["daily"]["tempLo"] != null) { TemperatureLow = HttpJTokenForecast["daily"]["tempLo"].ToString() + "°"; }
                        if (HttpJTokenForecast["daily"]["tempHi"] != null) { TemperatureHigh = HttpJTokenForecast["daily"]["tempHi"].ToString() + "°"; }

                        //Set Weather Icon
                        if (!String.IsNullOrEmpty(Icon))
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
                        if (!String.IsNullOrEmpty(Temperature) && !String.IsNullOrEmpty(Condition))
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
                        if (!String.IsNullOrEmpty(Temperature))
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
                        if (!String.IsNullOrEmpty(Condition))
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
                        if (!String.IsNullOrEmpty(WindSpeedDirection))
                        {
                            BgStatusWeatherCurrentWindSpeed = WindSpeedDirection;
                            vApplicationSettings["BgStatusWeatherCurrentWindSpeed"] = BgStatusWeatherCurrentWindSpeed;
                        }
                        else
                        {
                            BgStatusWeatherCurrentWindSpeed = "N/A";
                            vApplicationSettings["BgStatusWeatherCurrentWindSpeed"] = BgStatusWeatherCurrentWindSpeed;
                        }

                        //Set Weather Rain Chance
                        if (!String.IsNullOrEmpty(RainChance))
                        {
                            BgStatusWeatherCurrentRainChance = RainChance;
                            vApplicationSettings["BgStatusWeatherCurrentRainChance"] = BgStatusWeatherCurrentRainChance;
                        }
                        else
                        {
                            BgStatusWeatherCurrentRainChance = "N/A";
                            vApplicationSettings["BgStatusWeatherCurrentRainChance"] = BgStatusWeatherCurrentRainChance;
                        }

                        //Set Weather Temp Low
                        if (!String.IsNullOrEmpty(TemperatureLow))
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
                        if (!String.IsNullOrEmpty(TemperatureHigh))
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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to update weather status: " + ex.Message);
                }
            }
        }
    }
}