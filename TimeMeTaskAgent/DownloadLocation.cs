using ArnoldVinkCode;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {

        //Download location
        async Task<bool> DownloadLocation()
        {
            try
            {
                Debug.WriteLine("Downloading Location update.");

                //Check for internet connection
                if (!DownloadInternetCheck())
                {
                    BackgroundStatusUpdateSettings(null, "Never", null, null, "NoWifiEthernet");
                    return false;
                }

                //Load and set current GPS location
                if (setWeatherGpsLocation)
                {
                    try
                    {
                        //Get current GPS position from geolocator
                        Geolocator Geolocator = new Geolocator();
                        Geolocator.DesiredAccuracy = PositionAccuracy.Default;
                        Geoposition ResGeoposition = await Geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(setBackgroundDownloadIntervalMin), TimeSpan.FromSeconds(6));
                        DownloadWeatherLocation = ResGeoposition.Coordinate.Point.Position.Latitude.ToString().Replace(",", ".") + "," + ResGeoposition.Coordinate.Point.Position.Longitude.ToString().Replace(",", ".");
                    }
                    catch { DownloadGpsUpdateFailed = true; }
                }
                else
                {
                    if (String.IsNullOrEmpty(setWeatherNonGpsLocation)) { DownloadGpsUpdateFailed = true; }
                    else { DownloadWeatherLocation = setWeatherNonGpsLocation; }
                }

                //Load and set manual location
                if (DownloadGpsUpdateFailed)
                {
                    string PreviousLocation = BgStatusWeatherCurrentLocationFull.Replace("!", "");
                    if (PreviousLocation != "N/A" && !String.IsNullOrEmpty(PreviousLocation)) { DownloadWeatherLocation = PreviousLocation; }
                    else
                    {
                        Debug.WriteLine("Failed no previous location has been set.");
                        BackgroundStatusUpdateSettings(null, "Failed", null, null, "GpsPrevUpdateFailed");
                        return false;
                    }
                }

                //Download and save the weather location
                string LocationResult = await AVDownloader.DownloadStringAsync(5000, "TimeMe", null, new Uri("https://service.weather.microsoft.com/" + DownloadWeatherLanguage + "/locations/search/" + DownloadWeatherLocation + DownloadWeatherUnits));

                //Check if there is location data available
                JObject LocationJObject = JObject.Parse(LocationResult);
                if (LocationJObject["responses"][0]["locations"] == null || !LocationJObject["responses"][0]["locations"].Any())
                {
                    Debug.WriteLine("Failed no overall info for location found.");
                    BackgroundStatusUpdateSettings(null, "Failed", null, null, "GpsNoLocationOverall");
                    return false;
                }
                else
                {
                    JToken HttpJTokenGeo = LocationJObject["responses"][0]["locations"][0];

                    //Set current location coords
                    if (HttpJTokenGeo["coordinates"]["lat"] != null && HttpJTokenGeo["coordinates"]["lon"] != null) { DownloadWeatherLocation = HttpJTokenGeo["coordinates"]["lat"].ToString().Replace(",", ".") + "," + HttpJTokenGeo["coordinates"]["lon"].ToString().Replace(",", "."); }
                    else
                    {
                        if (!setWeatherGpsLocation || DownloadGpsUpdateFailed)
                        {
                            Debug.WriteLine("Failed no gps coords for location found.");
                            BackgroundStatusUpdateSettings(null, "Failed", null, null, "GpsNoLocationCoords");
                            return false;
                        }
                    }

                    //Set weather current location
                    if (HttpJTokenGeo["displayName"] != null)
                    {
                        string LocationName = HttpJTokenGeo["displayName"].ToString();
                        if (!String.IsNullOrEmpty(LocationName))
                        {
                            BgStatusWeatherCurrentLocationFull = LocationName;
                            vApplicationSettings["BgStatusWeatherCurrentLocationFull"] = BgStatusWeatherCurrentLocationFull;

                            if (LocationName.Contains(",")) { LocationName = LocationName.Split(',')[0]; }
                            BgStatusWeatherCurrentLocation = LocationName;
                            vApplicationSettings["BgStatusWeatherCurrentLocation"] = BgStatusWeatherCurrentLocation;
                        }
                        else
                        {
                            Debug.WriteLine("Failed empty location name found.");
                            BackgroundStatusUpdateSettings(null, "Failed", null, null, "NoLocationNameEmpty");
                            return false;
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Failed no location name found.");
                        BackgroundStatusUpdateSettings(null, "Failed", null, null, "NoLocationNameFound");
                        return false;
                    }
                }

                //Save Location status
                BgStatusDownloadLocation = DateTimeNow.ToString(vCultureInfoEng);
                vApplicationSettings["BgStatusDownloadLocation"] = BgStatusDownloadLocation;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed updating the location info.");
                BackgroundStatusUpdateSettings(null, "Failed", null, null, "CatchDownloadLocation" + ex.Message);
                return false;
            }
        }
    }
}