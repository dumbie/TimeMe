using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Draw Medium More Live Tile
        async Task ClockTileMediumMore()
        {
            try
            {
                //Load Tile Render Variables
                if (!TileRenderVarsLoaded)
                {
                    CanvasHorizontalAlignment HorizontalAlignmentTime = CanvasHorizontalAlignment.Center;
                    switch (setDisplayHorizontalAlignmentTime)
                    {
                        case 1: { HorizontalAlignmentTime = CanvasHorizontalAlignment.Left; break; }
                        case 2: { HorizontalAlignmentTime = CanvasHorizontalAlignment.Center; break; }
                        case 3: { HorizontalAlignmentTime = CanvasHorizontalAlignment.Right; break; }
                    }

                    //Live tile font styles
                    Win2DCanvasTextFormatTitle = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightTitle, FontSize = 90 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    Win2DCanvasTextFormatBody = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 90 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };

                    if (setDisplayAMPMFont) { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightSub, FontSize = 38 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }
                    else { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = "Segoe UI", FontWeight = Win2DFontWeightSub, FontSize = 38 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }

                    Win2DCanvasTextFormatTextLeft = new CanvasTextFormat() { FontFamily = "Segoe UI", FontWeight = Win2DFontWeightText, FontSize = 25 + (setLiveTileFontSize / 2), WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = CanvasHorizontalAlignment.Left, VerticalAlignment = CanvasVerticalAlignment.Bottom };

                    //Live tile text positions
                    switch (setLiveTileFont)
                    {
                        case "Segoe UI": { TimeHeight1 = -75; break; }
                        case "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT": { TimeHeight1 = -82; break; }
                        case "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue": { TimeHeight1 = -86; break; }
                        case "/Assets/Fonts/Existence-Light.ttf#Existence": { TimeHeight1 = -84; break; }
                        case "/Assets/Fonts/OneDay-Light.ttf#ONE DAY": { TimeHeight1 = -96; break; }
                        case "/Assets/Fonts/Pier-Regular.ttf#Pier Sans": { TimeHeight1 = -77; break; }
                        case "/Assets/Fonts/Panama-Light.ttf#Panama": { TimeHeight1 = -80; break; }
                        case "/Assets/Fonts/Bellota-Light.ttf#Bellota": { TimeHeight1 = -70; break; }
                        case "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif": { TimeHeight1 = -85; break; }
                        case "/Assets/Fonts/Modeka-Light.ttf#Modeka": { TimeHeight1 = -87; break; }
                        case "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk": { TimeHeight1 = -95; break; }
                        case "/Assets/Fonts/Dense-Regular.ttf#Dense": { TimeHeight1 = -92; break; }
                        case "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb": { TimeHeight1 = -94; break; }
                    }

                    TileRenderVarsLoaded = true;
                }

                using (CanvasDrawingSession ds = Win2DCanvasRenderTarget.CreateDrawingSession())
                {
                    //Live tile content - Time
                    DrawTimeOnTileSolo(ds, 0, false, false);

                    //Live tile content - Left
                    ds.DrawText(DisplayPosition1Text, LiveTilePadding, BottomTextHeight1, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextLeft);
                    ds.DrawText(DisplayPosition2Text, LiveTilePadding, BottomTextHeight2, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextLeft);
                    ds.DrawText(DisplayPosition3Text, LiveTilePadding, BottomTextHeight3, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextLeft);
                    ds.DrawText(TextAlarmClock + DisplayPosition4Text, LiveTilePadding, BottomTextHeight4, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextLeft);
                }
                await ExportLiveTile();
            }
            catch { }
        }
    }
}