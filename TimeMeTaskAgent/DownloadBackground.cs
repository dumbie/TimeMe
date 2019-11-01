using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Download Weather Forecast and Bing Wallpaper
        async Task DownloadBackground()
        {
            try
            {
                Debug.WriteLine("Checking if background download is needed.");

                //Check if background download is enabled
                if (setBackgroundDownload)
                {
                    if (setDownloadBingWallpaper && (BgStatusDownloadBing == "Never" || DateTimeNow.Subtract(DateTime.Parse(BgStatusDownloadBing, vCultureInfoEng)).TotalMinutes >= setBackgroundDownloadIntervalMin)) { await DownloadBingWallpaper(); }
                    if (setDownloadWeather && (BgStatusDownloadLocation == "Never" || BgStatusDownloadLocation == "Failed" || BgStatusDownloadWeather == "Never" || BgStatusDownloadWeather == "Failed" || DateTimeNow.Subtract(DateTime.Parse(BgStatusDownloadWeather, vCultureInfoEng)).TotalMinutes >= setBackgroundDownloadIntervalMin))
                    {
                        //Load and set Download variables
                        if (setFahrenheitCelsius == 1) { DownloadWeatherUnits = "?units=C"; }
                        if (setDisplayRegionLanguage) { DownloadWeatherLanguage = vCultureInfoReg.Name; }

                        //Start downloading weather information
                        bool LocationResult = await DownloadLocation();
                        if (LocationResult)
                        {
                            await DownloadWeather();

                            //Force live tile update
                            TileLive_ForceUpdate = true;
                        }
                    }
                }
            }
            catch { }
        }

        //Check if internet is available
        bool DownloadInternetCheck()
        {
            try
            {
                //Check if there is an internet connection
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //Check if Wi-Fi or Ethernet is connected
                    if (setDownloadWifiOnly)
                    {
                        uint IanaInterfaceType = NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.IanaInterfaceType;
                        if (IanaInterfaceType != 71 && IanaInterfaceType != 6) { Debug.WriteLine("No Wi-Fi or Ethernet connection available."); return false; }
                    }
                    return true;
                }
                else { Debug.WriteLine("No internet connection available."); return false; }
            }
            catch { Debug.WriteLine("No internet connection available?"); return false; }
        }
    }
}