using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    public sealed partial class ScheduledAgent : IBackgroundTask
    {
        //Run Task Agent Update
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                //Set current background task info
                taskInstanceDeferral = taskInstance.GetDeferral();
                taskInstance.Canceled += delegate { DisposeVariables(); taskInstance.Progress = 100; taskInstanceDeferral.Complete(); return; };
                taskInstanceName = taskInstance.Task.Name;

                //Load tile and application settings
                taskInstance.Progress = 90;
                if (!LoadAppSettings())
                {
                    if (TileLive_Pinned) { RenderTileAppUpdated("TimeMeLiveTile"); }
                    if (TileWeather_Pinned) { RenderTileAppUpdated("TimeMeWeatherTile"); }
                    if (TileBattery_Pinned) { RenderTileAppUpdated("TimeMeBatteryTile"); }
                    DisposeVariables();
                    taskInstance.Progress = 100;
                    taskInstanceDeferral.Complete();
                    return;
                }

                //Show task start debug message
                if (setAppDebug)
                {
                    Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastText02\"><text id=\"1\">Taskstart: " + taskInstanceName + "</text><text id=\"2\">" + DateTimeNow.ToString() + "</text></binding></visual><audio silent=\"true\"/></toast>");
                    Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T3", Group = "G3" });
                }

                //Load other used data first
                if (!await LoadOtherDataFirst())
                {
                    if (TileLive_Pinned) { RenderTileLiveFailed("TimeMeLiveTile"); }
                    if (TileWeather_Pinned) { RenderTileLiveFailed("TimeMeWeatherTile"); }
                    if (TileBattery_Pinned) { RenderTileLiveFailed("TimeMeBatteryTile"); }
                    DisposeVariables();
                    taskInstance.Progress = 100;
                    taskInstanceDeferral.Complete();
                    return;
                }

                //Download the background updates
                taskInstance.Progress = 10;
                await DownloadBackground();

                //Update the lockscreen information
                taskInstance.Progress = 20;
                UpdateLockscreen();

                //Check if weather tile is pinned
                if (TileWeather_Pinned)
                {
                    taskInstance.Progress = 40;
                    Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeWeatherTile");
                    Tile_UpdateManager.EnableNotificationQueue(false);
                    Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();
                    if (await LoadTileDataWeather()) { RenderWeatherTile(); } else { RenderTileWeatherDisabled(); }
                }

                //Check if battery tile is pinned
                if (TileBattery_Pinned)
                {
                    taskInstance.Progress = 50;
                    Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeBatteryTile");
                    Tile_UpdateManager.EnableNotificationQueue(false);
                    Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();
                    if (await LoadTileDataBattery()) { RenderBatteryTile(); } else { RenderTileBatteryDisabled(); }
                }

                //Check if live tile is pinned
                if (TileLive_Pinned)
                {
                    taskInstance.Progress = 30;
                    Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeLiveTile");
                    Tile_UpdateManager.EnableNotificationQueue(false);
                    Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();

                    //Delay timer task to avoid collision
                    //if (TaskInstanceName.StartsWith("TimeMeTaskTimer")) { await Task.Delay(1000); }

                    //Check if there is a live tile update needed
                    if (!FreshDeviceBoot && !TileLive_ForceUpdate && taskInstanceName != "TimeMeTaskTimeZone" && BgStatusLastRunDate != "Never" && Tile_PlannedUpdates.Any())
                    {
                        //Check if the live tile has failed to update
                        if (Tile_PlannedUpdates.Last().DeliveryTime.Subtract(DateTime.Parse(BgStatusLastRunDate, vCultureInfoEng)).TotalMilliseconds <= 960000) { Debug.WriteLine("Live tile has failed to render succesfully."); }
                        else
                        {
                            if (taskInstanceName == "TimeMeTaskUser") { Debug.WriteLine("There is no user live tile update needed."); TileLive_NeedUpdate = false; }
                            else if (taskInstanceName == "TimeMeTaskTimer" && Tile_PlannedUpdates.Last().DeliveryTime.Subtract(DateTimeNow).TotalMilliseconds >= 960000) { Debug.WriteLine("There is no timer live tile update needed."); TileLive_NeedUpdate = false; }
                        }
                    }

                    //Update the live tile if needed
                    if (TileLive_NeedUpdate)
                    {
                        //Load first one time live tile data 
                        if (await LoadTileDataFirst())
                        {
                            //Plan and render future live tiles
                            await PlanLiveTiles();
                        }
                        else { RenderTileLiveFailed("TimeMeLiveTile"); }
                    }
                }
            }
            catch { }
            DisposeVariables();
            taskInstance.Progress = 100;
            taskInstanceDeferral.Complete();
            return;
        }

        //Force Task Agent Update
        public IAsyncOperation<bool> ForceRun()
        {
            return Task.Run<bool>(async delegate
            {
                try
                {
                    //Load tile and application settings
                    if (!LoadAppSettings())
                    {
                        if (TileLive_Pinned) { RenderTileAppUpdated("TimeMeLiveTile"); }
                        if (TileWeather_Pinned) { RenderTileAppUpdated("TimeMeWeatherTile"); }
                        if (TileBattery_Pinned) { RenderTileAppUpdated("TimeMeBatteryTile"); }
                        DisposeVariables();
                        return false;
                    }

                    //Load other used data first
                    if (!await LoadOtherDataFirst())
                    {
                        if (TileLive_Pinned) { RenderTileLiveFailed("TimeMeLiveTile"); }
                        if (TileWeather_Pinned) { RenderTileLiveFailed("TimeMeWeatherTile"); }
                        if (TileBattery_Pinned) { RenderTileLiveFailed("TimeMeBatteryTile"); }
                        DisposeVariables();
                        return false;
                    }

                    //Download the background updates
                    await DownloadBackground();

                    //Update the lockscreen information
                    UpdateLockscreen();

                    //Check if weather tile is pinned
                    if (TileWeather_Pinned)
                    {
                        Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeWeatherTile");
                        Tile_UpdateManager.EnableNotificationQueue(false);
                        Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();
                        if (await LoadTileDataWeather()) { RenderWeatherTile(); } else { RenderTileWeatherDisabled(); }
                    }

                    //Check if battery tile is pinned
                    if (TileBattery_Pinned)
                    {
                        Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeBatteryTile");
                        Tile_UpdateManager.EnableNotificationQueue(false);
                        Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();
                        if (await LoadTileDataBattery()) { RenderBatteryTile(); } else { RenderTileBatteryDisabled(); }
                    }

                    //Check if live tile is pinned
                    if (TileLive_Pinned)
                    {
                        Tile_UpdateManager = TileUpdateManager.CreateTileUpdaterForSecondaryTile("TimeMeLiveTile");
                        Tile_UpdateManager.EnableNotificationQueue(false);
                        Tile_PlannedUpdates = Tile_UpdateManager.GetScheduledTileNotifications();

                        //Load first one time live tile data 
                        if (await LoadTileDataFirst())
                        {
                            //Plan and render future live tiles
                            await PlanLiveTiles();
                        }
                        else { RenderTileLiveFailed("TimeMeLiveTile"); }
                    }
                }
                catch { }
                DisposeVariables();
                return true;
            }).AsAsyncOperation();
        }
    }
}