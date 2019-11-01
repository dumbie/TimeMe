﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System.Threading.Tasks;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Draw Wide Time Only Live Tile
        async Task ClockTileWideTimeOnly()
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
                    if (!setDisplayAMPMClock)
                    {
                        Win2DCanvasTextFormatTitle = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightTitle, FontSize = 185 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                        Win2DCanvasTextFormatBody = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 185 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    }
                    else
                    {
                        Win2DCanvasTextFormatTitle = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightTitle, FontSize = 170 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                        Win2DCanvasTextFormatBody = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightBody, FontSize = 170 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, HorizontalAlignment = HorizontalAlignmentTime, VerticalAlignment = CanvasVerticalAlignment.Center, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings };
                    }

                    if (setDisplayAMPMFont) { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = setLiveTileFont, FontWeight = Win2DFontWeightSub, FontSize = 45 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }
                    else { Win2DCanvasTextFormatSub = new CanvasTextFormat() { FontFamily = "Segoe UI", FontWeight = Win2DFontWeightSub, FontSize = 45 + setLiveTileFontSize, WordWrapping = CanvasWordWrapping.NoWrap, OpticalAlignment = CanvasOpticalAlignment.NoSideBearings }; }

                    //Live tile text positions
                    switch (setLiveTileFont)
                    {
                        case "Segoe UI": { TimeHeight1 = -10; break; }
                        case "/Assets/Fonts/Gothic720-Light.ttf#Gothic720 Lt BT": { TimeHeight1 = -21; break; }
                        case "/Assets/Fonts/HelveticaNeue-UltraLight.ttf#Helvetica Neue": { TimeHeight1 = -30; break; }
                        case "/Assets/Fonts/Existence-Light.ttf#Existence": { TimeHeight1 = -27; break; }
                        case "/Assets/Fonts/OneDay-Light.ttf#ONE DAY": { TimeHeight1 = -47; break; }
                        case "/Assets/Fonts/Pier-Regular.ttf#Pier Sans": { TimeHeight1 = -11; break; }
                        case "/Assets/Fonts/Panama-Light.ttf#Panama": { TimeHeight1 = -21; break; }
                        case "/Assets/Fonts/Bellota-Light.ttf#Bellota": { TimeHeight1 = -4; break; }
                        case "/Assets/Fonts/Nooa-Semiserif.ttf#Nooa Semiserif": { TimeHeight1 = -39; break; }
                        case "/Assets/Fonts/Modeka-Light.ttf#Modeka": { TimeHeight1 = -34; break; }
                        case "/Assets/Fonts/Rawengulk-Light.ttf#Rawengulk": { TimeHeight1 = -51; break; }
                        case "/Assets/Fonts/Dense-Regular.ttf#Dense": { TimeHeight1 = -43; break; }
                        case "/Assets/Fonts/DigitalDisplay.ttf#digital display tfb": { TimeHeight1 = -55; break; }
                    }

                    TileRenderVarsLoaded = true;
                }

                using (CanvasDrawingSession ds = Win2DCanvasRenderTarget.CreateDrawingSession())
                {
                    //Live tile content - Time
                    DrawTimeOnTileSolo(ds, 0, false, true);
                }
                await ExportLiveTile();
            }
            catch { }
        }
    }
}