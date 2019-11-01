using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Draw Wide Numm Live Tile
        async Task ClockTileWideNumm()
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
                    Win2DCanvasTextFormatTitle = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightTitle, FontSize = 79 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    Win2DCanvasTextFormatBody = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 79 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };

                    //Live tile text positions
                    switch (setLiveTileFont)
                    {
                        case "Segoe UI": { TimeHeight1 = -54; TimeHeight2 = 40; break; }
                        case "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT": { TimeHeight1 = -60; TimeHeight2 = 35; break; }
                        case "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue": { TimeHeight1 = -64; TimeHeight2 = 30; break; }
                        case "/Assets/Fonts/Existence-Light.ttf#Existence": { TimeHeight1 = -63; TimeHeight2 = 31; break; }
                        case "/Assets/Fonts/OneDay-Light.ttf#ONE DAY": { TimeHeight1 = -71; TimeHeight2 = 25; break; }
                        case "/Assets/Fonts/Pier-Regular.ttf#Pier Sans": { TimeHeight1 = -54; TimeHeight2 = 41; break; }
                        case "/Assets/Fonts/Panama-Light.ttf#Panama": { TimeHeight1 = -60; TimeHeight2 = 36; break; }
                        case "/Assets/Fonts/Bellota-Light.ttf#Bellota": { TimeHeight1 = -48; TimeHeight2 = 47; break; }
                        case "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif": { TimeHeight1 = -68; TimeHeight2 = 20; break; }
                        case "/Assets/Fonts/Modeka-Light.ttf#Modeka": { TimeHeight1 = -65; TimeHeight2 = 31; break; }
                        case "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk": { TimeHeight1 = -74; TimeHeight2 = 21; break; }
                        case "/Assets/Fonts/Dense-Regular.ttf#Dense": { TimeHeight1 = -71; TimeHeight2 = 25; break; }
                        case "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb": { TimeHeight1 = -75; TimeHeight2 = 20; break; }
                    }

                    TileRenderVarsLoaded = true;
                }

                using (CanvasDrawingSession ds = Win2DCanvasRenderTarget.CreateDrawingSession())
                {
                    //Live tile content - Time
                    if (setDisplayTimeCustomText) { DrawTimeOnTileDuo(ds, TextTimeSplit, string.Empty); }
                    else { DrawTimeOnTileDuo(ds, TextTimeHour, TextTimeMin); }
                }
                await ExportLiveTile();
            }
            catch { }
        }
    }
}