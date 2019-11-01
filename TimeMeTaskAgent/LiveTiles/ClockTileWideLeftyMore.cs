using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Draw Wide Lefty More Live Tile
        async Task ClockTileWideLeftyMore()
        {
            try
            {
                //Load Tile Render Variables
                if (!TileRenderVarsLoaded)
                {
                    CanvasHorizontalAlignment HorizontalAlignmentTime = CanvasHorizontalAlignment.Left;
                    switch (setDisplayHorizontalAlignmentTime)
                    {
                        case 1: { HorizontalAlignmentTime = CanvasHorizontalAlignment.Left; break; }
                        case 2: { HorizontalAlignmentTime = CanvasHorizontalAlignment.Center; break; }
                        case 3: { HorizontalAlignmentTime = CanvasHorizontalAlignment.Right; break; }
                    }

                    //Live tile font styles
                    Win2DCanvasTextFormatTitle = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightTitle, FontSize = 84 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    Win2DCanvasTextFormatBody = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 84 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };

                    if (setDisplayAMPMFont) { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightSub, FontSize = 35 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = CanvasHorizontalAlignment.Left, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }
                    else { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = "Segoe UI", FontWeight = Win2DFontWeightSub, FontSize = 35 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = CanvasHorizontalAlignment.Left, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }

                    Win2DCanvasTextFormatTextRight = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 64 + (setLiveTileFontSize / 2), WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = CanvasHorizontalAlignment.Right, VerticalAlignment = CanvasVerticalAlignment.Bottom, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };

                    //Live tile text positions
                    switch (setLiveTileFont)
                    {
                        case "Segoe UI": { TimeHeight1 = -76; break; }
                        case "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT": { TimeHeight1 = -80; break; }
                        case "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue": { TimeHeight1 = -86; break; }
                        case "/Assets/Fonts/Existence-Light.ttf#Existence": { TimeHeight1 = -85; break; }
                        case "/Assets/Fonts/OneDay-Light.ttf#ONE DAY": { TimeHeight1 = -94; break; }
                        case "/Assets/Fonts/Pier-Regular.ttf#Pier Sans": { TimeHeight1 = -75; break; }
                        case "/Assets/Fonts/Panama-Light.ttf#Panama": { TimeHeight1 = -80; break; }
                        case "/Assets/Fonts/Bellota-Light.ttf#Bellota": { TimeHeight1 = -71; break; }
                        case "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif": { TimeHeight1 = -92; break; }
                        case "/Assets/Fonts/Modeka-Light.ttf#Modeka": { TimeHeight1 = -87; break; }
                        case "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk": { TimeHeight1 = -97; break; }
                        case "/Assets/Fonts/Dense-Regular.ttf#Dense": { TimeHeight1 = -92; break; }
                        case "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb": { TimeHeight1 = -101; break; }
                    }
                    switch (setLiveTileFont)
                    {
                        case "Segoe UI": { BottomTextHeight1 = -1; break; }
                        case "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT": { BottomTextHeight1 = -8; break; }
                        case "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue": { BottomTextHeight1 = -12; break; }
                        case "/Assets/Fonts/Existence-Light.ttf#Existence": { BottomTextHeight1 = -9; break; }
                        case "/Assets/Fonts/OneDay-Light.ttf#ONE DAY": { BottomTextHeight1 = -11; break; }
                        case "/Assets/Fonts/Pier-Regular.ttf#Pier Sans": { BottomTextHeight1 = -2; break; }
                        case "/Assets/Fonts/Panama-Light.ttf#Panama": { BottomTextHeight1 = -7; break; }
                        case "/Assets/Fonts/Bellota-Light.ttf#Bellota": { BottomTextHeight1 = 8; break; }
                        case "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif": { BottomTextHeight1 = -9; break; }
                        case "/Assets/Fonts/Modeka-Light.ttf#Modeka": { BottomTextHeight1 = -14; break; }
                        case "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk": { BottomTextHeight1 = -23; break; }
                        case "/Assets/Fonts/Dense-Regular.ttf#Dense": { BottomTextHeight1 = -21; break; }
                        case "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb": { BottomTextHeight1 = -20; break; }
                    }

                    TileRenderVarsLoaded = true;
                }

                using (CanvasDrawingSession ds = Win2DCanvasRenderTarget.CreateDrawingSession())
                {
                    //Live tile content - Time
                    DrawTimeOnTileSolo(ds, 0, false, false);

                    //Live tile content - Right
                    ds.DrawText(TextAlarmClock + DisplayPosition1Text, -LiveTilePadding, BottomTextHeight1, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextRight);
                }
                await ExportLiveTile();
            }
            catch { }
        }
    }
}