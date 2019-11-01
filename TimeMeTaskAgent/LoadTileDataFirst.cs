using ArnoldVinkCode;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Text;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Load first live tile data
        async Task<bool> LoadTileDataFirst()
        {
            try
            {
                Debug.WriteLine("Loading data for the first live tile.");

                //Check if the location name is too long
                BgStatusWeatherCurrentLocation = AVFunctions.StringCut(BgStatusWeatherCurrentLocation, 25, String.Empty);

                //Load and set empty time tile texts
                TextPositionSet(Setting_TextPositions.NoInformation, String.Empty);

                //Load and set current battery text
                if (TextPositionUsed(Setting_TextPositions.Battery))
                {
                    if (!String.IsNullOrEmpty(BatteryLevel) && BatteryLevel != "error")
                    {
                        TextBatteryLevel = "🗲 " + BatteryLevel + "%";
                        WordsBatteryLevel = BatteryLevel + " percent battery left";
                    }
                    else
                    {
                        TextBatteryLevel = "🗲 Unknown";
                        WordsBatteryLevel = "unknown battery level";
                    }

                    TextPositionSet(Setting_TextPositions.Battery, TextBatteryLevel);
                }

                //Load and set countdown event
                if (TextPositionUsed(Setting_TextPositions.Countdown))
                {
                    if (!String.IsNullOrEmpty(CountdownEventName) && !String.IsNullOrEmpty(CountdownEventDate)) { TextCountdownEvent = CountdownEventName + " (" + AVFunctions.ToTitleCase(CountdownEventDate) + "d)"; }
                    else { TextCountdownEvent = "No countdown event"; }

                    TextPositionSet(Setting_TextPositions.Countdown, TextCountdownEvent);
                }

                //Load and set calendar event
                if (TextPositionUsed(Setting_TextPositions.CalendarName) || TextPositionUsed(Setting_TextPositions.CalendarDateTime))
                {
                    if (!String.IsNullOrEmpty(CalendarAppoName)) { TextPositionSet(Setting_TextPositions.CalendarName, CalendarAppoName); }
                    else { TextPositionSet(Setting_TextPositions.CalendarName, "No calendar event"); }

                    if (!String.IsNullOrEmpty(CalendarAppoEstimated)) { TextPositionSet(Setting_TextPositions.CalendarDateTime, CalendarAppoEstimated); }
                    else if (!String.IsNullOrEmpty(CalendarAppoDateTime)) { TextPositionSet(Setting_TextPositions.CalendarDateTime, CalendarAppoDateTime); }
                    else { TextPositionSet(Setting_TextPositions.CalendarDateTime, "No calendar date"); }
                }

                //Load and set current week number
                if (TextPositionUsed(Setting_TextPositions.WeekNumber) || setDisplayDateWeekNumber || setLiveTileSizeName == "WideWords")
                {
                    TextWeekNumber = "W" + WeekNumberCurrent;
                    WordsWeekNumber = "week number " + WeekNumberCurrent;

                    TextPositionSet(Setting_TextPositions.WeekNumber, "Week " + WeekNumberCurrent);
                }

                //Check for active alarms and timers
                if (setDisplayAlarm && TimerAlarmActive)
                {
                    TextAlarmClock = "⏰";
                    WordsAlarmClock = "the timer alarm is on";
                }

                //Set weather tile texts
                if ((TextPositionUsed(Setting_TextPositions.WeatherFull) || TextPositionUsed(Setting_TextPositions.WeatherInfo) || TextPositionUsed(Setting_TextPositions.WeatherTempTextDegrees) || TextPositionUsed(Setting_TextPositions.WeatherTempTextSymbol) || TextPositionUsed(Setting_TextPositions.WeatherTempAsciiIcon)) && setBackgroundDownload && setDownloadWeather)
                {
                    if (TextPositionUsed(Setting_TextPositions.WeatherFull)) { TextPositionSet(Setting_TextPositions.WeatherFull, BgStatusWeatherCurrent); }
                    if (TextPositionUsed(Setting_TextPositions.WeatherInfo)) { TextPositionSet(Setting_TextPositions.WeatherInfo, BgStatusWeatherCurrentText); }
                    if (TextPositionUsed(Setting_TextPositions.WeatherTempTextSymbol)) { TextPositionSet(Setting_TextPositions.WeatherTempTextSymbol, BgStatusWeatherCurrentTemp); }
                    if (TextPositionUsed(Setting_TextPositions.WeatherTempAsciiIcon)) { TextPositionSet(Setting_TextPositions.WeatherTempAsciiIcon, "☼ " + BgStatusWeatherCurrentTemp); }
                    if (TextPositionUsed(Setting_TextPositions.WeatherTempTextDegrees))
                    {
                        if (BgStatusWeatherCurrentTemp.Contains("!")) { TextPositionSet(Setting_TextPositions.WeatherTempTextDegrees, BgStatusWeatherCurrentTemp.Replace("°", "").Replace("!", "") + " degrees!"); }
                        else { TextPositionSet(Setting_TextPositions.WeatherTempTextDegrees, BgStatusWeatherCurrentTemp.Replace("°", "") + " degrees"); }
                    }
                }

                //Set wind speed and direction tile texts
                if (TextPositionUsed(Setting_TextPositions.WindSpeed) && setBackgroundDownload && setDownloadWeather)
                { TextPositionSet(Setting_TextPositions.WindSpeed, "≋ " + BgStatusWeatherCurrentWindSpeed); }

                //Set rain chance tile texts
                if (TextPositionUsed(Setting_TextPositions.RainChance) && setBackgroundDownload && setDownloadWeather)
                { TextPositionSet(Setting_TextPositions.RainChance, "☂ " + BgStatusWeatherCurrentRainChance); }

                //Set location tile texts
                if (TextPositionUsed(Setting_TextPositions.Location) && setBackgroundDownload && setDownloadWeather)
                { TextPositionSet(Setting_TextPositions.Location, BgStatusWeatherCurrentLocation); }

                //Load light Live Tile Resources
                if (setLiveTileSizeLight)
                {
                    //Set light live tiles icons style
                    TileLight_TileIcon = "ms-appx:///Assets/WeatherSquare" + WeatherIconStyle + "/" + WeatherIconCurrent + ".png";

                    //Load live tile background photo or color
                    if (setLiveTileSizeName == "MediumRoundImage")
                    {
                        if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png")) { TileLight_BackgroundPhotoXml = "ms-appdata:///local/TimeMeTilePhoto.png"; }
                        else { TileLight_BackgroundPhotoXml = "ms-appx:///Assets/Tiles/TimeMeTilePhoto.png"; }
                    }
                    else if (setDisplayBackgroundPhoto)
                    {
                        if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png")) { TileLight_BackgroundPhotoXml = "<image src=\"ms-appdata:///local/TimeMeTilePhoto.png\" placement=\"background\" hint-overlay=\"" + setDisplayBackgroundBrightnessInt + "\"/>"; }
                        else { TileLight_BackgroundPhotoXml = "<image src=\"ms-appx:///Assets/Tiles/TimeMeTilePhoto.png\" placement=\"background\" hint-overlay=\"" + setDisplayBackgroundBrightnessInt + "\"/>"; }
                    }
                    else if (setDisplayBackgroundColor)
                    {
                        if (await AVFunctions.LocalFileExists("TimeMeTileColor.png")) { TileLight_BackgroundPhotoXml = "<image src=\"ms-appdata:///local/TimeMeTileColor.png\" placement=\"background\" hint-overlay=\"0\"/>"; }
                        else { TileLight_BackgroundPhotoXml = "<image src=\"ms-appx:///Assets/Tiles/TimeMeTileColor.png\" placement=\"background\" hint-overlay=\"0\"/>"; }
                    }
                }
                //Load heavy Live Tile Resources
                else
                {
                    Debug.WriteLine("Loading live tile image and font resources.");
                    //Set current weather and battery for words back tile
                    if (setLiveTileSizeName == "WideWords")
                    {
                        if (setBackgroundDownload && setDownloadWeather)
                        {
                            //Enable Tile Background Render
                            TileLive_BackRender = true;

                            //Set Weather Degrees text
                            if (BgStatusWeatherCurrentTemp.Contains("!")) { WordsWeatherDegree = BgStatusWeatherCurrentTemp.Replace("°", "").Replace("!", "").Replace("-", "min ") + " degrees outside!"; }
                            else { WordsWeatherDegree = BgStatusWeatherCurrentTemp.Replace("°", "").Replace("-", "min ") + " degrees outside"; }

                            //Set Weather Description Text
                            if (BgStatusWeatherCurrentText.Length > 10)
                            {
                                if (BgStatusWeatherCurrentText.Length >= 13) { WordsWeatherInfo = BgStatusWeatherCurrentText.ToLower(); }
                                else { WordsWeatherInfo = BgStatusWeatherCurrentText.ToLower() + " out"; }
                            }
                            else { WordsWeatherInfo = BgStatusWeatherCurrentText.ToLower() + " outside"; }

                            //Set Weather Location Text
                            WordsWeatherLocation = BgStatusWeatherCurrentLocation;
                            if (WordsWeatherLocation.Length < 7) { WordsWeatherLocation = "near town " + WordsWeatherLocation; }
                            else if (WordsWeatherLocation.Length < 15) { WordsWeatherLocation = "near " + WordsWeatherLocation; }

                            //Check for empty strings on the back tile
                            if (String.IsNullOrEmpty(WordsBatteryLevel)) { WordsBatteryLevel = WordsWeatherLocation; }
                            if (String.IsNullOrEmpty(WordsAlarmClock)) { WordsAlarmClock = WordsWeekNumber; }
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(WordsAlarmClock)) { WordsAlarmClock = WordsWeekNumber; }
                            if (String.IsNullOrEmpty(WordsWeatherDegree))
                            {
                                if (!String.IsNullOrEmpty(WordsAlarmClock)) { WordsWeatherDegree = WordsAlarmClock; }
                                else { WordsWeatherDegree = WordsBatteryLevel; }
                            }
                        }
                    }

                    //Load live tile dimensions
                    if (setLiveTileSizeName.Contains("Medium"))
                    {
                        //Set Medium Tile Sizes
                        if (setShowMoreTiles)
                        {
                            LiveTileWidth = 230;
                            LiveTileHeight = 230;
                            LiveTileWidthResize = 409;
                            LiveTileHeightResize = 230;
                            LiveTileWidthCropping = 89;
                            LiveTileHeightCropping = 0;

                            LiveTilePadding = 14;
                            BottomTextHeight1 = -106;
                            BottomTextHeight2 = -74;
                            BottomTextHeight3 = -42;
                            BottomTextHeight4 = -10; //Bottom32
                            BottomTextCenterHeight1 = BottomTextHeight1 - 2;
                            BottomTextCenterHeight2 = BottomTextHeight2 - 2;
                            BottomTextCenterHeight3 = BottomTextHeight3 - 2;
                            BottomTextCenterHeight4 = BottomTextHeight4 - 2;
                        }
                        else
                        {
                            LiveTileWidth = 336;
                            LiveTileHeight = 336;
                            LiveTileWidthResize = 597;
                            LiveTileHeightResize = 336;
                            LiveTileWidthCropping = 130;
                            LiveTileHeightCropping = 0;

                            LiveTilePadding = 18;
                            BottomTextHeight1 = -147;
                            BottomTextHeight2 = -102;
                            BottomTextHeight3 = -57;
                            BottomTextHeight4 = -12; //Bottom45
                            BottomTextCenterHeight1 = BottomTextHeight1 - 2;
                            BottomTextCenterHeight2 = BottomTextHeight2 - 2;
                            BottomTextCenterHeight3 = BottomTextHeight3 - 2;
                            BottomTextCenterHeight4 = BottomTextHeight4 - 2;
                        }
                    }
                    else
                    {
                        //Set Wide Tile Sizes
                        if (setShowMoreTiles)
                        {
                            LiveTileWidth = 480;
                            LiveTileHeight = 235;
                            LiveTileWidthResize = 480;
                            LiveTileHeightResize = 270;
                            LiveTileWidthCropping = 0;
                            LiveTileHeightCropping = 17;

                            LiveTilePadding = 14;
                            BottomTextHeight1 = -106;
                            BottomTextHeight2 = -74;
                            BottomTextHeight3 = -42;
                            BottomTextHeight4 = -10; //Bottom32
                            BottomTextCenterHeight1 = BottomTextHeight1 - 2;
                            BottomTextCenterHeight2 = BottomTextHeight2 - 2;
                            BottomTextCenterHeight3 = BottomTextHeight3 - 2;
                            BottomTextCenterHeight4 = BottomTextHeight4 - 2;
                        }
                        else
                        {
                            LiveTileWidth = 510;
                            LiveTileHeight = 250;
                            LiveTileWidthResize = 510;
                            LiveTileHeightResize = 287;
                            LiveTileWidthCropping = 0;
                            LiveTileHeightCropping = 17;

                            LiveTilePadding = 16;
                            BottomTextHeight1 = -112;
                            BottomTextHeight2 = -78;
                            BottomTextHeight3 = -44;
                            BottomTextHeight4 = -10; //Bottom34
                            BottomTextCenterHeight1 = BottomTextHeight1 - 2;
                            BottomTextCenterHeight2 = BottomTextHeight2 - 2;
                            BottomTextCenterHeight3 = BottomTextHeight3 - 2;
                            BottomTextCenterHeight4 = BottomTextHeight4 - 2;
                        }
                    }

                    //Load and set Win2D settings
                    Win2DCanvasDevice = new CanvasDevice();
                    Win2DCanvasRenderTarget = new CanvasRenderTarget(Win2DCanvasDevice, LiveTileWidth, LiveTileHeight, 96); //96Wide-90Wide / 96Med-66Med

                    //Load live tile font colors
                    Win2DFontColorCusto = Color.FromArgb(Convert.ToByte(setLiveTileColorFont.Substring(1, 2), 16), Convert.ToByte(setLiveTileColorFont.Substring(3, 2), 16), Convert.ToByte(setLiveTileColorFont.Substring(5, 2), 16), Convert.ToByte(setLiveTileColorFont.Substring(7, 2), 16));
                    Win2DFontColorTrans = Color.FromArgb(Convert.ToByte(140), Convert.ToByte(setLiveTileColorFont.Substring(3, 2), 16), Convert.ToByte(setLiveTileColorFont.Substring(5, 2), 16), Convert.ToByte(setLiveTileColorFont.Substring(7, 2), 16));
                    Win2DFontColorWhite = Color.FromArgb(Convert.ToByte(255), Convert.ToByte(255), Convert.ToByte(255), Convert.ToByte(255));

                    //Load live tile font weights
                    Win2DFontWeightText = FontWeights.Normal;
                    switch (setLiveTileFontWeight)
                    {
                        case 0: { Win2DFontWeightTitle = FontWeights.Light; Win2DFontWeightBody = FontWeights.Light; Win2DFontWeightSub = FontWeights.Light; break; }
                        case 1: { Win2DFontWeightTitle = FontWeights.Normal; Win2DFontWeightBody = FontWeights.Normal; Win2DFontWeightSub = FontWeights.Normal; break; }
                        case 2: { Win2DFontWeightTitle = FontWeights.SemiBold; Win2DFontWeightBody = FontWeights.SemiBold; Win2DFontWeightSub = FontWeights.SemiBold; break; }
                    }
                    if (setDisplayHourBold)
                    {
                        switch (setLiveTileFontWeight)
                        {
                            case 0: { Win2DFontWeightTitle = FontWeights.Normal; break; }
                            case 1: { Win2DFontWeightTitle = FontWeights.SemiBold; break; }
                            case 2: { Win2DFontWeightTitle = FontWeights.Bold; break; }
                        }
                    }

                    //Load live tile background
                    if (setDisplayBackgroundPhoto)
                    {
                        if (await AVFunctions.LocalFileExists("TimeMeTilePhoto.png"))
                        {
                            StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("TimeMeTilePhoto.png");
                            using (IRandomAccessStream OpenAsync = await StorageFile.OpenAsync(FileAccessMode.Read))
                            {
                                using (InMemoryRandomAccessStream InMemoryRandomAccessStream = new InMemoryRandomAccessStream())
                                {
                                    BitmapEncoder BitmapEncoder = await BitmapEncoder.CreateForTranscodingAsync(InMemoryRandomAccessStream, await BitmapDecoder.CreateAsync(OpenAsync));
                                    OpenAsync.Dispose();

                                    //Resize original image 1:1
                                    BitmapEncoder.BitmapTransform.ScaledWidth = (uint)LiveTileWidthResize;
                                    BitmapEncoder.BitmapTransform.ScaledHeight = (uint)LiveTileHeightResize;

                                    //Crop image to tile size
                                    BitmapBounds BitmapBounds = new BitmapBounds();
                                    BitmapBounds.Width = (uint)LiveTileWidth;
                                    BitmapBounds.Height = (uint)LiveTileHeight;
                                    BitmapBounds.X = (uint)LiveTileWidthCropping;
                                    BitmapBounds.Y = (uint)LiveTileHeightCropping;
                                    BitmapEncoder.BitmapTransform.Bounds = BitmapBounds;

                                    await BitmapEncoder.FlushAsync();
                                    await InMemoryRandomAccessStream.FlushAsync();

                                    Win2DCanvasBitmap = await CanvasBitmap.LoadAsync(Win2DCanvasDevice, InMemoryRandomAccessStream);
                                }
                            }
                        }
                        else { Win2DCanvasBitmap = await CanvasBitmap.LoadAsync(Win2DCanvasDevice, new Uri("ms-appx:///Assets/Tiles/TimeMeTilePhoto.png", UriKind.Absolute)); }
                        if (setLiveTileTimeCutOut) { Win2DCanvasImageBrush = new CanvasImageBrush(Win2DCanvasDevice, Win2DCanvasBitmap) { Opacity = setDisplayBackgroundBrightnessFloat }; }
                    }
                    else if (setDisplayBackgroundColor || setLiveTileTimeCutOut) { Win2DCanvasColor = Color.FromArgb(Convert.ToByte(setLiveTileColorBackground.Substring(1, 2), 16), Convert.ToByte(setLiveTileColorBackground.Substring(3, 2), 16), Convert.ToByte(setLiveTileColorBackground.Substring(5, 2), 16), Convert.ToByte(setLiveTileColorBackground.Substring(7, 2), 16)); }
                    else { Win2DCanvasColor = Colors.Transparent; }
                }
                return true;
            }
            catch { return false; }
        }
    }
}