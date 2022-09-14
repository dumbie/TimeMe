using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Text;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Application Variables
        TileUpdater Tile_UpdateManager;
        ToastNotificationHistory Toast_History = ToastNotificationManager.History;
        ToastNotifier Toast_UpdateManager = ToastNotificationManager.CreateToastNotifier();
        BackgroundTaskDeferral taskInstanceDeferral;
        IReadOnlyList<ScheduledTileNotification> Tile_PlannedUpdates;
        IDictionary<string, object> vApplicationSettings = ApplicationData.Current.LocalSettings.Values;
        CultureInfo vCultureInfoEng = new CultureInfo("en-US");
        CultureInfo vCultureInfoReg = new CultureInfo(Windows.Globalization.Language.CurrentInputMethodLanguageTag);
        DateTime TileTimeMin = DateTime.Now.AddSeconds(-DateTime.Now.Second);
        DateTime TileTimeNow = DateTime.Now;
        DateTime DateTimeNow = DateTime.Now;
        bool FreshDeviceBoot = false;
        bool BatteryCharging;
        bool TimerAlarmActive = false;
        bool TileLive_NeedUpdate = true;
        bool TileLive_ForceUpdate = false;
        bool TileLive_BackRender = false;
        bool TileLive_Pinned = SecondaryTile.Exists("TimeMeLiveTile");
        bool TileWeather_Pinned = SecondaryTile.Exists("TimeMeWeatherTile");
        bool TileBattery_Pinned = SecondaryTile.Exists("TimeMeBatteryTile");
        XmlDocument Tile_XmlContent = new XmlDocument();
        int WeekNumberCurrent;
        int LiveTileWidth;
        int LiveTileHeight;
        int LiveTileWidthResize;
        int LiveTileHeightResize;
        int LiveTileWidthCropping;
        int LiveTileHeightCropping;
        int RenderRetryCount = 0;
        int DayTimeProgress = 0;
        string TileContentId = "";
        string TileRenderName = "Back";
        string TileLight_TileIcon = "";
        string TileLight_BackgroundPhotoXml = "";
        string TileWeather_BackgroundPhotoXml = "";
        string TileBattery_BackgroundPhotoXml = "";
        string taskInstanceName = "ForceRun";
        string TextCountdownEvent = "";
        string TextWeekNumber = "";
        string TextYear = "";
        string TextDay = "";
        string TextDayDateMonth = "";
        string TextDateMonth = "";
        string DateDayLength = "ddd"; //dddd
        string TextTimeFull = "";
        string TextTimeMin = "";
        string TextTimeHour = "";
        string TextTimeAmPm = "";
        string TextTimeSplit = "";
        string TextWordsDate = "";
        string BatteryIcon = "100";
        string BatteryTime = "";
        string BatteryLevel = "";
        string DisplayPosition1Text = "Not available";
        string DisplayPosition2Text = "Not available";
        string DisplayPosition3Text = "Not available";
        string DisplayPosition4Text = "Not available";
        int BatteryLevelInt = 100;
        string TextBatteryLevel = "";
        string WordsBatteryLevel = "";
        string WordsWeatherInfo = "";
        string WordsWeatherDegree = "";
        string WordsWeatherLocation = "";
        string WordsWeekNumber = "";
        string WordsAlarmClock = "";
        string TextAlarmClock = "";
        string TextNetworkName = "";
        string LockscreenText = "";
        string LockscreenEnter = "";
        string CountdownEventName = "";
        string CountdownEventDate = "";
        string CalendarAppoName = "";
        string CalendarAppoLocation = "";
        string CalendarAppoSummary = "";
        string CalendarAppoDateTime = "";
        string CalendarAppoEstimated = "";
        string WeatherIconStyle = "";
        string WeatherIconCurrent = "1000";
        string WeatherTile1 = "N/A";
        string WeatherTile2 = "N/A";
        string WeatherTile3 = "N/A";
        string WeatherTile4 = "N/A";
        string WeatherTile5 = "N/A";
        string WeatherDetailed = "N/A";
        string WeatherLastUpdate = "N/A";

        //Download Variables
        bool DownloadLocationGpsFailed = false;
        string DownloadLocationGpsCombined = string.Empty;
        string DownloadLocationGpsLatitude = string.Empty;
        string DownloadLocationGpsLongitude = string.Empty;
        string DownloadLocationTarget = string.Empty;
        string DownloadLocationLanguage = "en-US";

        //Tile Render Variables
        int TimeHeight1 = 0; int TimeHeight2 = 0; int LiveTilePadding = 0;
        int BottomTextHeight1 = 0; int BottomTextHeight2 = 0; int BottomTextHeight3 = 0; int BottomTextHeight4 = 0;
        int BottomTextCenterHeight1 = 0; int BottomTextCenterHeight2 = 0; int BottomTextCenterHeight3 = 0; int BottomTextCenterHeight4 = 0;
        bool TileRenderVarsLoaded = false;

        //Win2D Tile Render Variables
        CanvasDevice Win2DCanvasDevice;
        CanvasRenderTarget Win2DCanvasRenderTarget;
        CanvasBitmap Win2DCanvasBitmap;
        CanvasImageBrush Win2DCanvasImageBrush;
        CanvasTextFormat Win2DCanvasTextFormatTitle;
        CanvasTextFormat Win2DCanvasTextFormatBody;
        CanvasTextFormat Win2DCanvasTextFormatSub;
        CanvasTextFormat Win2DCanvasTextFormatTextLeft;
        CanvasTextFormat Win2DCanvasTextFormatTextRight;
        CanvasTextFormat Win2DCanvasTextFormatTextCenter;
        FontWeight Win2DFontWeightTitle;
        FontWeight Win2DFontWeightBody;
        FontWeight Win2DFontWeightSub;
        FontWeight Win2DFontWeightText;
        Color Win2DFontColorTrans;
        Color Win2DFontColorWhite;
        Color Win2DFontColorCusto;
        Color Win2DCanvasColor;

        //Dispose variables and clear memory
        void DisposeVariables()
        {
            try
            {
                Debug.WriteLine("Setting the last background task run date.");
                vApplicationSettings["BgStatusLastRunDate"] = DateTimeNow.ToString(vCultureInfoEng);

                Debug.WriteLine("Disposing variables and clearing memory.");
                if (Win2DCanvasDevice != null) { Win2DCanvasDevice.Dispose(); }
                if (Win2DCanvasRenderTarget != null) { Win2DCanvasRenderTarget.Dispose(); }
                if (Win2DCanvasBitmap != null) { Win2DCanvasBitmap.Dispose(); }
                if (Win2DCanvasImageBrush != null) { Win2DCanvasImageBrush.Dispose(); }
                if (Win2DCanvasTextFormatTitle != null) { Win2DCanvasTextFormatTitle.Dispose(); }
                if (Win2DCanvasTextFormatBody != null) { Win2DCanvasTextFormatBody.Dispose(); }
                if (Win2DCanvasTextFormatSub != null) { Win2DCanvasTextFormatSub.Dispose(); }
                if (Win2DCanvasTextFormatTextLeft != null) { Win2DCanvasTextFormatTextLeft.Dispose(); }
                if (Win2DCanvasTextFormatTextRight != null) { Win2DCanvasTextFormatTextRight.Dispose(); }
                if (Win2DCanvasTextFormatTextCenter != null) { Win2DCanvasTextFormatTextCenter.Dispose(); }
            }
            catch { }
        }
    }
}