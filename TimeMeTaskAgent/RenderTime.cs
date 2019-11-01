using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Numerics;
using Windows.UI;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Render Time to Drawing Session - Solo
        void DrawTimeOnTileSolo(CanvasDrawingSession ds, int AmPmHeightMargin, bool AmPmCenter, bool HorizontalTime)
        {
            try
            {
                //Set the text render layout
                CanvasTextLayout LayoutTimeHour = new CanvasTextLayout(ds, TextTimeHour, Win2DCanvasTextFormatTitle, LiveTileWidth, LiveTileHeight);
                CanvasTextLayout LayoutTimeSplit = new CanvasTextLayout(ds, TextTimeSplit, Win2DCanvasTextFormatBody, LiveTileWidth, LiveTileHeight);
                CanvasTextLayout LayoutTimeMin = new CanvasTextLayout(ds, TextTimeMin, Win2DCanvasTextFormatBody, LiveTileWidth, LiveTileHeight);
                CanvasTextLayout LayoutTimeAmPm = new CanvasTextLayout(ds, TextTimeAmPm, Win2DCanvasTextFormatSub, LiveTileWidth, LiveTileHeight);

                //Calculate the render positions
                int SplitWidthMargin = 15;
                int SplitHorizontal = 0;
                int HourHorizontal = 0;
                int MinHorizontal = 0;
                int AmPmHorizontal = 0;
                int AmPmVertical = 0;

                if (Win2DCanvasTextFormatTitle.HorizontalAlignment == CanvasHorizontalAlignment.Center)
                {
                    HourHorizontal = -Convert.ToInt32((LayoutTimeHour.DrawBounds.Width / 2) + (LayoutTimeSplit.DrawBounds.Width / 2) + SplitWidthMargin);
                    MinHorizontal = Convert.ToInt32((LayoutTimeMin.DrawBounds.Width / 2) + (LayoutTimeSplit.DrawBounds.Width / 2) + SplitWidthMargin);

                    if (!HorizontalTime)
                    {
                        if (setDisplayTimeCustomText)
                        {
                            //Calculate margin difference and center the text
                            if (setDisplayAMPMClock)
                            {
                                if (!AmPmCenter) { AmPmHorizontal = Convert.ToInt32(LayoutTimeSplit.DrawBounds.Right - LayoutTimeAmPm.DrawBounds.Width); }
                                AmPmVertical = Convert.ToInt32(LayoutTimeSplit.DrawBounds.Bottom + TimeHeight1 - AmPmHeightMargin);
                            }
                        }
                        else
                        {
                            //Calculate margin difference and center the text
                            Int32 MarginDifference = (Math.Abs(MinHorizontal) - Math.Abs(HourHorizontal));
                            SplitHorizontal = SplitHorizontal - MarginDifference;
                            HourHorizontal = HourHorizontal - MarginDifference;
                            MinHorizontal = MinHorizontal - MarginDifference;
                            if (setDisplayAMPMClock)
                            {
                                if (!AmPmCenter) { AmPmHorizontal = Convert.ToInt32(LayoutTimeMin.DrawBounds.Right + MinHorizontal - LayoutTimeAmPm.DrawBounds.Width); }
                                AmPmVertical = Convert.ToInt32(LayoutTimeMin.DrawBounds.Bottom + TimeHeight1 - AmPmHeightMargin);
                            }
                        }
                    }
                    else
                    {
                        if (setDisplayTimeCustomText)
                        {
                            //Calculate margin difference and center the text
                            Int32 MarginDifference = 0;
                            if (setDisplayAMPMClock) { MarginDifference = MarginDifference + Convert.ToInt32((LayoutTimeAmPm.DrawBounds.Width / 2) + (SplitWidthMargin / 2)); }
                            SplitHorizontal = SplitHorizontal - MarginDifference;

                            if (setDisplayAMPMClock)
                            {
                                AmPmHorizontal = Convert.ToInt32(LayoutTimeSplit.DrawBounds.Right + SplitWidthMargin + SplitHorizontal);
                                int TimeAmPmHeightText = Convert.ToInt32(LayoutTimeAmPm.DrawBounds.Height + LayoutTimeAmPm.DrawBounds.Top);
                                AmPmVertical = Convert.ToInt32(LayoutTimeSplit.DrawBounds.Bottom + TimeHeight1 - TimeAmPmHeightText);
                            }
                        }
                        else
                        {
                            //Calculate margin difference and center the text
                            Int32 MarginDifference = (Math.Abs(MinHorizontal) - Math.Abs(HourHorizontal));
                            if (setDisplayAMPMClock) { MarginDifference = MarginDifference + Convert.ToInt32((LayoutTimeAmPm.DrawBounds.Width / 2) + (SplitWidthMargin / 2)); }
                            SplitHorizontal = SplitHorizontal - MarginDifference;
                            HourHorizontal = HourHorizontal - MarginDifference;
                            MinHorizontal = MinHorizontal - MarginDifference;

                            if (setDisplayAMPMClock)
                            {
                                AmPmHorizontal = Convert.ToInt32(LayoutTimeMin.DrawBounds.Right + SplitWidthMargin + MinHorizontal);
                                int TimeAmPmHeightText = Convert.ToInt32(LayoutTimeAmPm.DrawBounds.Height + LayoutTimeAmPm.DrawBounds.Top);
                                AmPmVertical = Convert.ToInt32(LayoutTimeMin.DrawBounds.Bottom + TimeHeight1 - TimeAmPmHeightText);
                            }
                        }
                    }
                }
                else if (Win2DCanvasTextFormatTitle.HorizontalAlignment == CanvasHorizontalAlignment.Right)
                {
                    if (setDisplayTimeCustomText)
                    {
                        SplitHorizontal = -LiveTilePadding;
                        AmPmHorizontal = -LiveTilePadding;
                        AmPmVertical = Convert.ToInt32(LayoutTimeSplit.DrawBounds.Bottom + TimeHeight1 - AmPmHeightMargin);
                    }
                    else
                    {
                        MinHorizontal = -LiveTilePadding;
                        SplitHorizontal = -Convert.ToInt32(LiveTilePadding + LayoutTimeMin.DrawBounds.Width + SplitWidthMargin);
                        HourHorizontal = -Convert.ToInt32(LiveTilePadding + LayoutTimeMin.DrawBounds.Width + SplitWidthMargin + LayoutTimeSplit.DrawBounds.Width + SplitWidthMargin);
                        AmPmHorizontal = -LiveTilePadding;
                        AmPmVertical = Convert.ToInt32(LayoutTimeMin.DrawBounds.Bottom + TimeHeight1 - AmPmHeightMargin);
                    }
                }
                else if (Win2DCanvasTextFormatTitle.HorizontalAlignment == CanvasHorizontalAlignment.Left)
                {
                    if (setDisplayTimeCustomText)
                    {
                        SplitHorizontal = LiveTilePadding;
                        AmPmHorizontal = LiveTilePadding;
                        AmPmVertical = Convert.ToInt32(LayoutTimeSplit.DrawBounds.Bottom + TimeHeight1 - AmPmHeightMargin);
                    }
                    else
                    {
                        HourHorizontal = LiveTilePadding;
                        SplitHorizontal = Convert.ToInt32(LiveTilePadding + LayoutTimeHour.DrawBounds.Width + SplitWidthMargin);
                        MinHorizontal = Convert.ToInt32(LiveTilePadding + LayoutTimeHour.DrawBounds.Width + SplitWidthMargin + LayoutTimeSplit.DrawBounds.Width + SplitWidthMargin);
                        AmPmHorizontal = LiveTilePadding;
                        AmPmVertical = Convert.ToInt32(LayoutTimeHour.DrawBounds.Bottom + TimeHeight1 - AmPmHeightMargin);
                    }
                }

                //Check the live tile text style
                if (!setLiveTileTimeCutOut)
                {
                    //Live tile background photo or color
                    if (setDisplayBackgroundPhoto) { ds.Clear(Colors.Black); ds.DrawImage(Win2DCanvasBitmap, 0, 0, Win2DCanvasRenderTarget.Bounds, setDisplayBackgroundBrightnessFloat); } else { ds.Clear(Win2DCanvasColor); }

                    if (setLiveTileFontDuoColor)
                    {
                        ds.DrawTextLayout(LayoutTimeHour, HourHorizontal, TimeHeight1, Win2DFontColorWhite);
                        ds.DrawTextLayout(LayoutTimeSplit, SplitHorizontal, TimeHeight1, Win2DFontColorCusto);
                        ds.DrawTextLayout(LayoutTimeMin, MinHorizontal, TimeHeight1, Win2DFontColorCusto);
                        ds.DrawTextLayout(LayoutTimeAmPm, AmPmHorizontal, AmPmVertical, Win2DFontColorWhite);
                    }
                    else
                    {
                        ds.DrawTextLayout(LayoutTimeHour, HourHorizontal, TimeHeight1, Win2DFontColorCusto);
                        ds.DrawTextLayout(LayoutTimeSplit, SplitHorizontal, TimeHeight1, Win2DFontColorCusto);
                        ds.DrawTextLayout(LayoutTimeMin, MinHorizontal, TimeHeight1, Win2DFontColorCusto);
                        ds.DrawTextLayout(LayoutTimeAmPm, AmPmHorizontal, AmPmVertical, Win2DFontColorCusto);
                    }
                }
                else
                {
                    //Live tile background transparency
                    ds.Clear(Colors.Transparent);

                    CanvasGeometry GeometryBackground = CanvasGeometry.CreateRectangle(ds, 0, 0, LiveTileWidth, LiveTileHeight);
                    CanvasGeometry GeometryHour = CanvasGeometry.CreateText(LayoutTimeHour);
                    CanvasGeometry GeometrySplit = CanvasGeometry.CreateText(LayoutTimeSplit);
                    CanvasGeometry GeometryMin = CanvasGeometry.CreateText(LayoutTimeMin);
                    CanvasGeometry GeometryAmPm = CanvasGeometry.CreateText(LayoutTimeAmPm);

                    if (setDisplayBackgroundPhoto)
                    {
                        CanvasGeometry GeometryExclude1 = GeometryBackground.CombineWith(GeometryHour, Matrix3x2.CreateTranslation(HourHorizontal, TimeHeight1), CanvasGeometryCombine.Exclude);
                        GeometryExclude1 = GeometryExclude1.CombineWith(GeometrySplit, Matrix3x2.CreateTranslation(SplitHorizontal, TimeHeight1), CanvasGeometryCombine.Exclude);
                        GeometryExclude1 = GeometryExclude1.CombineWith(GeometryMin, Matrix3x2.CreateTranslation(MinHorizontal, TimeHeight1), CanvasGeometryCombine.Exclude);
                        GeometryExclude1 = GeometryExclude1.CombineWith(GeometryAmPm, Matrix3x2.CreateTranslation(AmPmHorizontal, AmPmVertical), CanvasGeometryCombine.Exclude);
                        ds.FillGeometry(GeometryExclude1, Colors.Black);
                        GeometryExclude1.Dispose();
                    }

                    CanvasGeometry GeometryExclude2 = GeometryBackground.CombineWith(GeometryHour, Matrix3x2.CreateTranslation(HourHorizontal, TimeHeight1), CanvasGeometryCombine.Exclude);
                    GeometryExclude2 = GeometryExclude2.CombineWith(GeometrySplit, Matrix3x2.CreateTranslation(SplitHorizontal, TimeHeight1), CanvasGeometryCombine.Exclude);
                    GeometryExclude2 = GeometryExclude2.CombineWith(GeometryMin, Matrix3x2.CreateTranslation(MinHorizontal, TimeHeight1), CanvasGeometryCombine.Exclude);
                    GeometryExclude2 = GeometryExclude2.CombineWith(GeometryAmPm, Matrix3x2.CreateTranslation(AmPmHorizontal, AmPmVertical), CanvasGeometryCombine.Exclude);
                    if (setDisplayBackgroundPhoto) { ds.FillGeometry(GeometryExclude2, Win2DCanvasImageBrush); } else { ds.FillGeometry(GeometryExclude2, Win2DCanvasColor); }
                    GeometryExclude2.Dispose();

                    GeometryBackground.Dispose();
                    GeometryHour.Dispose();
                    GeometrySplit.Dispose();
                    GeometryMin.Dispose();
                    GeometryAmPm.Dispose();
                }

                LayoutTimeHour.Dispose();
                LayoutTimeSplit.Dispose();
                LayoutTimeMin.Dispose();
                LayoutTimeAmPm.Dispose();
            }
            catch { }
        }

        //Render Time to Drawing Session - Duo
        void DrawTimeOnTileDuo(CanvasDrawingSession ds, string TextTop, string TextBot)
        {
            try
            {
                CanvasTextLayout LayoutTimeHour = new CanvasTextLayout(ds, TextTop, Win2DCanvasTextFormatTitle, LiveTileWidth, LiveTileHeight);
                CanvasTextLayout LayoutTimeMin = new CanvasTextLayout(ds, TextBot, Win2DCanvasTextFormatBody, LiveTileWidth, LiveTileHeight);

                //Live tile content - Time
                if (!setLiveTileTimeCutOut)
                {
                    //Live tile background photo or color
                    if (setDisplayBackgroundPhoto) { ds.Clear(Colors.Black); ds.DrawImage(Win2DCanvasBitmap, 0, 0, Win2DCanvasRenderTarget.Bounds, setDisplayBackgroundBrightnessFloat); } else { ds.Clear(Win2DCanvasColor); }

                    if (setLiveTileFontDuoColor) { ds.DrawTextLayout(LayoutTimeHour, 0, TimeHeight1, Win2DFontColorWhite); }
                    else { ds.DrawTextLayout(LayoutTimeHour, 0, TimeHeight1, Win2DFontColorCusto); }
                    ds.DrawTextLayout(LayoutTimeMin, 0, TimeHeight2, Win2DFontColorCusto);
                }
                else
                {
                    //Live tile background transparency
                    ds.Clear(Colors.Transparent);

                    CanvasGeometry GeometryBackground = CanvasGeometry.CreateRectangle(ds, 0, 0, LiveTileWidth, LiveTileHeight);
                    CanvasGeometry GeometryHour = CanvasGeometry.CreateText(LayoutTimeHour);
                    CanvasGeometry GeometryMin = CanvasGeometry.CreateText(LayoutTimeMin);

                    if (setDisplayBackgroundPhoto)
                    {
                        CanvasGeometry GeometryExclude1 = GeometryBackground.CombineWith(GeometryHour, Matrix3x2.CreateTranslation(0, TimeHeight1), CanvasGeometryCombine.Exclude);
                        GeometryExclude1 = GeometryExclude1.CombineWith(GeometryMin, Matrix3x2.CreateTranslation(0, TimeHeight2), CanvasGeometryCombine.Exclude);
                        ds.FillGeometry(GeometryExclude1, Colors.Black);
                        GeometryExclude1.Dispose();
                    }

                    CanvasGeometry GeometryExclude2 = GeometryBackground.CombineWith(GeometryHour, Matrix3x2.CreateTranslation(0, TimeHeight1), CanvasGeometryCombine.Exclude);
                    GeometryExclude2 = GeometryExclude2.CombineWith(GeometryMin, Matrix3x2.CreateTranslation(0, TimeHeight2), CanvasGeometryCombine.Exclude);
                    if (setDisplayBackgroundPhoto) { ds.FillGeometry(GeometryExclude2, Win2DCanvasImageBrush); } else { ds.FillGeometry(GeometryExclude2, Win2DCanvasColor); }
                    GeometryExclude2.Dispose();

                    GeometryBackground.Dispose();
                    GeometryHour.Dispose();
                    GeometryMin.Dispose();
                }

                LayoutTimeHour.Dispose();
                LayoutTimeMin.Dispose();
            }
            catch { }
        }

        //Render Time to Drawing Session - Trio
        void DrawTimeOnTileTrio(CanvasDrawingSession ds, string TextTop, string TextMid, string TextBot)
        {
            try
            {
                CanvasTextLayout LayoutTop = new CanvasTextLayout(ds, TextTop, Win2DCanvasTextFormatTitle, LiveTileWidth, LiveTileHeight);
                CanvasTextLayout LayoutMid = new CanvasTextLayout(ds, TextMid, Win2DCanvasTextFormatBody, LiveTileWidth, LiveTileHeight);
                CanvasTextLayout LayoutBot = new CanvasTextLayout(ds, TextBot, Win2DCanvasTextFormatBody, LiveTileWidth, LiveTileHeight);

                //Live tile content - Time
                if (!setLiveTileTimeCutOut)
                {
                    //Live tile background photo or color
                    if (setDisplayBackgroundPhoto) { ds.Clear(Colors.Black); ds.DrawImage(Win2DCanvasBitmap, 0, 0, Win2DCanvasRenderTarget.Bounds, setDisplayBackgroundBrightnessFloat); } else { ds.Clear(Win2DCanvasColor); }

                    //Live tile content - Center
                    if (setLiveTileFontDuoColor) { ds.DrawTextLayout(LayoutTop, 0, (TimeHeight1 - 66), Win2DFontColorWhite); }
                    else { ds.DrawTextLayout(LayoutTop, 0, (TimeHeight1 - 66), Win2DFontColorCusto); }
                    ds.DrawTextLayout(LayoutMid, 0, TimeHeight1, Win2DFontColorCusto);
                    ds.DrawTextLayout(LayoutBot, 0, (TimeHeight1 + 58), Win2DFontColorCusto);
                }
                else
                {
                    //Live tile background transparency
                    ds.Clear(Colors.Transparent);

                    CanvasGeometry GeometryBackground = CanvasGeometry.CreateRectangle(ds, 0, 0, LiveTileWidth, LiveTileHeight);
                    CanvasGeometry GeometryTop = CanvasGeometry.CreateText(LayoutTop);
                    CanvasGeometry GeometryMid = CanvasGeometry.CreateText(LayoutMid);
                    CanvasGeometry GeometryBot = CanvasGeometry.CreateText(LayoutBot);

                    if (setDisplayBackgroundPhoto)
                    {
                        CanvasGeometry GeometryExclude1 = GeometryBackground.CombineWith(GeometryTop, Matrix3x2.CreateTranslation(0, (TimeHeight1 - 66)), CanvasGeometryCombine.Exclude);
                        GeometryExclude1 = GeometryExclude1.CombineWith(GeometryMid, Matrix3x2.CreateTranslation(0, TimeHeight1), CanvasGeometryCombine.Exclude);
                        GeometryExclude1 = GeometryExclude1.CombineWith(GeometryBot, Matrix3x2.CreateTranslation(0, (TimeHeight1 + 58)), CanvasGeometryCombine.Exclude);
                        ds.FillGeometry(GeometryExclude1, Colors.Black);
                        GeometryExclude1.Dispose();
                    }

                    CanvasGeometry GeometryExclude2 = GeometryBackground.CombineWith(GeometryTop, Matrix3x2.CreateTranslation(0, (TimeHeight1 - 66)), CanvasGeometryCombine.Exclude);
                    GeometryExclude2 = GeometryExclude2.CombineWith(GeometryMid, Matrix3x2.CreateTranslation(0, TimeHeight1), CanvasGeometryCombine.Exclude);
                    GeometryExclude2 = GeometryExclude2.CombineWith(GeometryBot, Matrix3x2.CreateTranslation(0, (TimeHeight1 + 58)), CanvasGeometryCombine.Exclude);
                    if (setDisplayBackgroundPhoto) { ds.FillGeometry(GeometryExclude2, Win2DCanvasImageBrush); } else { ds.FillGeometry(GeometryExclude2, Win2DCanvasColor); }
                    GeometryExclude2.Dispose();

                    GeometryBackground.Dispose();
                    GeometryTop.Dispose();
                    GeometryMid.Dispose();
                    GeometryBot.Dispose();
                }

                LayoutTop.Dispose();
                LayoutMid.Dispose();
                LayoutBot.Dispose();
            }
            catch { }
        }
    }
}