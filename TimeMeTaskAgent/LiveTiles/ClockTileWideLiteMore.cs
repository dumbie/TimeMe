using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Draw Wide Lite More Live Tile
        async Task ClockTileWideLiteMore()
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
                    Win2DCanvasTextFormatTitle = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightTitle, FontSize = 160 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    Win2DCanvasTextFormatBody = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 160 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };

                    if (setDisplayAMPMFont) { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightSub, FontSize = 42 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }
                    else { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = "Segoe UI", FontWeight = Win2DFontWeightSub, FontSize = 42 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }

                    Win2DCanvasTextFormatTextLeft = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 64 + (setLiveTileFontSize / 2), WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = CanvasHorizontalAlignment.Left, VerticalAlignment = CanvasVerticalAlignment.Bottom, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    Win2DCanvasTextFormatTextRight = new CanvasTextFormat() { FontFamily = "Segoe UI", FontWeight = Win2DFontWeightText, FontSize = 25 + (setLiveTileFontSize / 2), WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = CanvasHorizontalAlignment.Right, VerticalAlignment = CanvasVerticalAlignment.Bottom };

                    //Live tile text positions
                    switch (setLiveTileFont)
                    {
                        case "Segoe UI": { TimeHeight1 = -30; break; }
                        case "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT": { TimeHeight1 = -31; break; }
                        case "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue": { TimeHeight1 = -36; break; }
                        case "/Assets/Fonts/Existence-Light.ttf#Existence": { TimeHeight1 = -45; break; }
                        case "/Assets/Fonts/OneDay-Light.ttf#ONE DAY": { TimeHeight1 = -41; break; }
                        case "/Assets/Fonts/Pier-Regular.ttf#Pier Sans": { TimeHeight1 = -29; break; }
                        case "/Assets/Fonts/Panama-Light.ttf#Panama": { TimeHeight1 = -28; break; }
                        case "/Assets/Fonts/Bellota-Light.ttf#Bellota": { TimeHeight1 = -32; break; }
                        case "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif": { TimeHeight1 = -34; break; }
                        case "/Assets/Fonts/Modeka-Light.ttf#Modeka": { TimeHeight1 = -37; break; }
                        case "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk": { TimeHeight1 = -48; break; }
                        case "/Assets/Fonts/Dense-Regular.ttf#Dense": { TimeHeight1 = -38; break; }
                        case "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb": { TimeHeight1 = -54; break; }
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
                    DrawTimeOnTileSolo(ds, 0, false, true);

                    //Live tile content - Left and Right
                    ds.DrawText(DisplayPosition1Text, LiveTilePadding, BottomTextHeight1, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextLeft);
                    ds.DrawText(DisplayPosition2Text, -LiveTilePadding, BottomTextHeight3, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextRight);
                    ds.DrawText(TextAlarmClock + DisplayPosition3Text, -LiveTilePadding, BottomTextHeight4, LiveTileWidth, LiveTileHeight, Win2DFontColorCusto, Win2DCanvasTextFormatTextRight);
                }
                await ExportLiveTile();
            }
            catch { }
        }
    }
}