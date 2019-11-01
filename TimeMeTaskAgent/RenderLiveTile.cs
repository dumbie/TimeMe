using Microsoft.Graphics.Canvas;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Render live tile front
        async Task<XmlDocument> RenderLiveTile()
        {
            try
            {
                //Render Medium live tile
                if (setLiveTileSizeName == "Medium")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileMediumMore(); } else { await ClockTileMedium(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoSize.png\"/></binding></visual></tile>");
                }

                //Render Medium One live tile
                else if (setLiveTileSizeName == "MediumOne")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileMediumOneMore(); } else { await ClockTileMediumOne(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoSize.png\"/></binding></visual></tile>");
                }

                //Render Medium Three live tile
                else if (setLiveTileSizeName == "MediumThree")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileMediumThreeMore(); } else { await ClockTileMediumThree(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoSize.png\"/></binding></visual></tile>");
                }

                //Render Medium Time Only live tile
                else if (setLiveTileSizeName == "MediumTimeOnly")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileMediumTimeOnlyMore(); } else { await ClockTileMediumTimeOnly(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoSize.png\"/></binding></visual></tile>");
                }

                //Render Medium Analog Minimal live tile (Light)
                else if (setLiveTileSizeName == "MediumAnalogMinimal") { Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSmall\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Minimal/" + TileTimeMin.ToString("hmm") + ".png\"/></binding><binding template=\"TileMedium\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Minimal/" + TileTimeMin.ToString("hmm") + ".png\"/></binding><binding template=\"TileWide\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Minimal/" + TileTimeMin.ToString("hmm") + ".png\"/></binding></visual></tile>"); }

                //Render Medium Analog Round live tile (Light)
                else if (setLiveTileSizeName == "MediumAnalogRound") { Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSmall\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Round/" + TileTimeMin.ToString("hmm") + ".png\"/></binding><binding template=\"TileMedium\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Round/" + TileTimeMin.ToString("hmm") + ".png\"/></binding><binding template=\"TileWide\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Round/" + TileTimeMin.ToString("hmm") + ".png\"/></binding></visual></tile>"); }

                //Render Medium Analog Cortana live tile (Light)
                else if (setLiveTileSizeName == "MediumAnalogCortana") { Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSmall\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Cortana/" + TileTimeMin.ToString("hmm") + ".png\"/></binding><binding template=\"TileMedium\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Cortana/" + TileTimeMin.ToString("hmm") + ".png\"/></binding><binding template=\"TileWide\">" + TileLight_BackgroundPhotoXml + "<image src=\"ms-appx:///Assets/Analog/Cortana/" + TileTimeMin.ToString("hmm") + ".png\"/></binding></visual></tile>"); }

                //Render Medium live tile icon (Light)
                else if (setLiveTileSizeName == "MediumIcon")
                {
                    LoadTileDataTile();
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileMedium\">" + TileLight_BackgroundPhotoXml + "<image src=\"" + TileLight_TileIcon + "\" placement=\"peek\"/><group><subgroup hint-textStacking=\"center\"><text hint-style=\"base\">" + WebUtility.HtmlEncode(TextTimeFull) + " " + TextTimeAmPm + "</text><text hint-style=\"captionSubtle\">" + WebUtility.HtmlEncode(DisplayPosition1Text) + "</text><text hint-style=\"captionSubtle\">" + WebUtility.HtmlEncode(DisplayPosition2Text) + "</text><text hint-style=\"captionSubtle\">" + TextAlarmClock + WebUtility.HtmlEncode(DisplayPosition3Text) + "</text></subgroup></group></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoSize.png\"/></binding></visual></tile>");
                }

                //Render Medium live tile text (Light)
                else if (setLiveTileSizeName == "MediumText")
                {
                    LoadTileDataTile();
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileMedium\">" + TileLight_BackgroundPhotoXml + "<group><subgroup hint-textStacking=\"center\"><text hint-style=\"base\">" + WebUtility.HtmlEncode(TextTimeFull) + " " + TextTimeAmPm + "</text><text hint-style=\"captionSubtle\">" + WebUtility.HtmlEncode(DisplayPosition1Text) + "</text><text hint-style=\"captionSubtle\">" + WebUtility.HtmlEncode(DisplayPosition2Text) + "</text><text hint-style=\"captionSubtle\">" + TextAlarmClock + WebUtility.HtmlEncode(DisplayPosition3Text) + "</text></subgroup></group></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/WideLogoSize.png\"/></binding></visual></tile>");
                }

                //Render Medium Round Image live tile (Light)
                else if (setLiveTileSizeName == "MediumRoundImage")
                {
                    LoadTileDataTile();
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSmall\"><image src=\"" + TileLight_BackgroundPhotoXml + "\" hint-crop=\"circle\"/></binding><binding template=\"TileMedium\"><group><subgroup hint-weight=\"10\"/><subgroup hint-weight=\"65\"><image src=\"" + TileLight_BackgroundPhotoXml + "\" hint-crop=\"circle\"/></subgroup><subgroup hint-weight=\"10\"/></group><text hint-style=\"title\" hint-align=\"center\">" + WebUtility.HtmlEncode(TextTimeFull) + " " + TextTimeAmPm + TextAlarmClock + "</text></binding><binding template=\"TileWide\"><group><subgroup hint-weight=\"1\"/><subgroup hint-weight=\"1\"><image src=\"" + TileLight_BackgroundPhotoXml + "\" hint-crop=\"circle\"/></subgroup><subgroup hint-weight=\"1\"/></group><text hint-style=\"base\" hint-align=\"center\">" + WebUtility.HtmlEncode(TextTimeFull) + " " + TextTimeAmPm + TextAlarmClock + "</text></binding></visual></tile>");
                }

                //Render Wide live tile icon (Light)
                else if (setLiveTileSizeName == "WideIcon")
                {
                    LoadTileDataTile();
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWide\">" + TileLight_BackgroundPhotoXml + "<group><subgroup hint-weight=\"30\"><image src=\"" + TileLight_TileIcon + "\"/></subgroup><subgroup hint-textStacking=\"center\"><text hint-style=\"base\">" + WebUtility.HtmlEncode(TextTimeFull) + " " + TextTimeAmPm + "</text><text hint-style=\"captionSubtle\">" + WebUtility.HtmlEncode(DisplayPosition1Text) + "</text><text hint-style=\"captionSubtle\">" + WebUtility.HtmlEncode(DisplayPosition2Text) + "</text><text hint-style=\"captionSubtle\">" + TextAlarmClock + WebUtility.HtmlEncode(DisplayPosition3Text) + "</text></subgroup></group></binding></visual></tile>");
                }

                //Render Wide live tile text
                else if (setLiveTileSizeName == "WideText")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideTextMore(); } else { await ClockTileWideText(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile lite
                else if (setLiveTileSizeName == "WideLite")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideLiteMore(); } else { await ClockTileWideLite(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile lefty
                else if (setLiveTileSizeName == "WideLefty")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideLeftyMore(); } else { await ClockTileWideLefty(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile photo
                else if (setLiveTileSizeName == "WidePhoto")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWidePhotoMore(); } else { await ClockTileWidePhoto(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile date
                else if (setLiveTileSizeName == "WideDate")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideDateMore(); } else { await ClockTileWideDate(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile Big
                else if (setLiveTileSizeName == "WideBig")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideBigMore(); } else { await ClockTileWideBig(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile multi / one
                else if (setLiveTileSizeName == "WideMulti" || setLiveTileSizeName == "WideOne")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideMultiMore(); } else { await ClockTileWideMulti(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile two
                else if (setLiveTileSizeName == "WideTwo")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideTwoMore(); } else { await ClockTileWideTwo(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile time only
                else if (setLiveTileSizeName == "WideTimeOnly")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideTimeOnlyMore(); } else { await ClockTileWideTimeOnly(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile numm
                else if (setLiveTileSizeName == "WideNumm")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideNummMore(); } else { await ClockTileWideNumm(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                //Render Wide live tile words
                else if (setLiveTileSizeName == "WideWords")
                {
                    LoadTileDataTile();
                    if (setShowMoreTiles) { await ClockTileWideWordsMore(); } else { await ClockTileWideWords(); }
                    Tile_XmlContent.LoadXml("<tile><visual contentId=\"" + TileContentId + "\" branding=\"none\"><binding template=\"TileSquareImage\"><image id=\"1\" src=\"ms-appx:///Assets/Tiles/SquareLogoSize.png\"/></binding><binding template=\"TileWideImage\"><image id=\"1\" src=\"ms-appdata:///local/TimeMe" + TileRenderName + ".png\"/></binding></visual></tile>");
                }

                return Tile_XmlContent;
            }
            catch { return RenderTileLiveFailed("TimeMeLiveTile"); }
        }

        //Render live tile back
        async Task RenderLiveTileBack()
        {
            try
            {
                //Render Wide live tile words back
                if (setLiveTileSizeName == "WideWords") { if (setShowMoreTiles) { await ClockTileWideWordsMoreBack(); } else { await ClockTileWideWordsBack(); } }
            }
            catch { }
        }

        //Export rendered live tile image
        async Task ExportLiveTile()
        {
            try
            {
                await Win2DCanvasRenderTarget.SaveAsync(ApplicationData.Current.LocalFolder.Path + "\\TimeMe" + TileRenderName + ".png", CanvasBitmapFileFormat.Png, 1);
                RenderRetryCount = 0;
            }
            catch (Exception ex)
            {
                if (RenderRetryCount < 3) //ex is UnauthorizedAccessException && 
                {
                    Debug.WriteLine("Failed exporting a rendered tile, retrying: " + RenderRetryCount);
                    RenderRetryCount++;
                    await ExportLiveTile();
                }
                else { BackgroundStatusUpdateSettings(null, null, null, null, "FailRenderTile" + ex.Message); }
            }
        }
    }
}