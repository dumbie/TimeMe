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
                        DownloadLocationGpsLatitude = ResGeoposition.Coordinate.Point.Position.Latitude.ToString().Replace(",", ".");
                        DownloadLocationGpsLongitude = ResGeoposition.Coordinate.Point.Position.Longitude.ToString().Replace(",", ".");
                        DownloadLocationGpsCombined = DownloadLocationGpsLatitude + "," + DownloadLocationGpsLongitude;
                        DownloadLocationTarget = DownloadLocationGpsCombined;
                    }
                    catch
                    {
                        DownloadLocationGpsFailed = true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(setWeatherNonGpsLocation))
                    {
                        DownloadLocationGpsFailed = true;
                    }
                    else
                    {
                        DownloadLocationTarget = setWeatherNonGpsLocation;
                    }
                }

                //Load and set manual location
                if (DownloadLocationGpsFailed)
                {
                    string PreviousLocation = BgStatusWeatherCurrentLocationFull.Replace("!", "");
                    if (PreviousLocation != "N/A" && !string.IsNullOrEmpty(PreviousLocation))
                    {
                        DownloadLocationTarget = PreviousLocation;
                    }
                    else
                    {
                        Debug.WriteLine("Failed no previous location has been set.");
                        BackgroundStatusUpdateSettings(null, "Failed", null, null, "GpsPrevUpdateFailed");
                        return false;
                    }
                }

                //Download and save the weather location
                string LocationResult = await AVDownloader.DownloadStringAsync(5000, "TimeMe", null, new Uri("https://service.weather.microsoft.com/" + DownloadLocationLanguage + "/locations/search/" + DownloadLocationTarget));

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
                    if (HttpJTokenGeo["coordinates"]["lat"] != null && HttpJTokenGeo["coordinates"]["lon"] != null)
                    {
                        DownloadLocationGpsLatitude = HttpJTokenGeo["coordinates"]["lat"].ToString().Replace(",", ".");
                        DownloadLocationGpsLongitude = HttpJTokenGeo["coordinates"]["lon"].ToString().Replace(",", ".");
                        DownloadLocationGpsCombined = DownloadLocationGpsLatitude + "," + DownloadLocationGpsLongitude;
                    }
                    else
                    {
                        if (!setWeatherGpsLocation || DownloadLocationGpsFailed)
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
                        if (!string.IsNullOrEmpty(LocationName))
                        {
                            BgStatusWeatherCurrentLocationFull = LocationName;
                            vApplicationSettings["BgStatusWeatherCurrentLocationFull"] = BgStatusWeatherCurrentLocationFull;

                            if (LocationName.Contains(",")) { LocationName = LocationName.Split(',')[0]; }
                            BgStatusWeatherCurrentLocationShort = LocationName;
                            vApplicationSettings["BgStatusWeatherCurrentLocationShort"] = BgStatusWeatherCurrentLocationShort;
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