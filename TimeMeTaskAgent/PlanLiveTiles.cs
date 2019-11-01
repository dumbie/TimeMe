using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Notifications;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Plan and render future live tiles
        async Task PlanLiveTiles()
        {
            try
            {
                //Show render start debug message
                if (setAppDebug)
                {
                    Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastText02\"><text id=\"1\">Renderstart: " + taskInstanceName + "</text><text id=\"2\">" + DateTimeNow.ToString() + "</text></binding></visual><audio silent=\"true\"/></toast>");
                    Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T1", Group = "G3" });
                }

                //Remove old planned live tiles
                Debug.WriteLine("Removing " + Tile_PlannedUpdates.Count + " older live task updates.");
                foreach (ScheduledTileNotification Tile_Update in Tile_PlannedUpdates) { try { Tile_UpdateManager.RemoveFromSchedule(Tile_Update); } catch { } }

                //Render future live tile back
                if (TileLive_BackRender)
                {
                    Debug.WriteLine("Started rendering back live tile.");
                    await RenderLiveTileBack();
                }

                //Render future live tiles front
                Debug.WriteLine("Started rendering front live tiles.");
                TileTimeNow = DateTime.Now;
                TileTimeMin = TileTimeNow.AddSeconds(-TileTimeNow.Second).AddMinutes(-1);
                for (int LiveTileRenderId = 0; LiveTileRenderId < 18; LiveTileRenderId++)
                {
                    try
                    {
                        TileTimeNow = DateTime.Now;
                        TileTimeMin = TileTimeMin.AddMinutes(1);
                        TileContentId = TileTimeMin.Minute.ToString();
                        TileRenderName = LiveTileRenderId.ToString();

                        if (TileTimeNow.Minute == TileTimeMin.Minute) { Tile_UpdateManager.Update(new TileNotification(await RenderLiveTile())); } else { Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(await RenderLiveTile(), new DateTimeOffset(TileTimeMin))); }
                        if (TileLive_BackRender)
                        {
                            Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMeBack.png\"/></binding></visual></tile>");
                            if (TileTimeNow < TileTimeMin.AddSeconds(12)) { Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(Tile_XmlContent, new DateTimeOffset(TileTimeMin.AddSeconds(12)))); }
                            if (TileTimeNow < TileTimeMin.AddSeconds(32)) { Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(Tile_XmlContent, new DateTimeOffset(TileTimeMin.AddSeconds(32)))); }
                            if (TileTimeNow < TileTimeMin.AddSeconds(52)) { Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(Tile_XmlContent, new DateTimeOffset(TileTimeMin.AddSeconds(52)))); }
                            Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                            if (TileTimeNow < TileTimeMin.AddSeconds(20)) { Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(Tile_XmlContent, new DateTimeOffset(TileTimeMin.AddSeconds(20)))); }
                            if (TileTimeNow < TileTimeMin.AddSeconds(40)) { Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(Tile_XmlContent, new DateTimeOffset(TileTimeMin.AddSeconds(40)))); }
                        }

                        //Show live tile render debug message
                        if (setAppDebug)
                        {
                            Tile_XmlContent.LoadXml("<toast><visual><binding template=\"ToastText02\"><text id=\"1\">Renderedtile: " + taskInstanceName + "</text><text id=\"2\">" + TileRenderName + "/17 at " + DateTimeNow.ToString() + " Mem " + (MemoryManager.AppMemoryUsage / 1024f / 1024f).ToString() + "</text></binding></visual><audio silent=\"true\"/></toast>");
                            Toast_UpdateManager.Show(new ToastNotification(Tile_XmlContent) { SuppressPopup = true, Tag = "T2", Group = "G3" });
                        }
                    }
                    catch { }
                }

                ////Add tile will be updated shortly on the end
                //Tile_DateTimeMin = Tile_DateTimeMin.AddMinutes(1);
                //Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoUpdate.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoUpdate.png\"/></binding></visual></tile>");
                //Tile_UpdateManager.AddToSchedule(new ScheduledTileNotification(Tile_XmlContent, new DateTimeOffset(Tile_DateTimeMin)));

                Debug.WriteLine("Finished rendering live tiles batch.");
            }
            catch { Debug.WriteLine("Failed rendering live tiles batch."); }
        }
    }
}