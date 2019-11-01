using ArnoldVinkCode;
using System;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Update current live tile data
        void LoadTileDataTile()
        {
            try
            {
                //Load live tile text
                if (setLiveTileSizeName != "WideWords")
                {
                    //Set Current Date text
                    if (TextPositionUsed(Setting_TextPositions.DayOnly) || TextPositionUsed(Setting_TextPositions.DayDateMonth) || TextPositionUsed(Setting_TextPositions.DateMonth))
                    {
                        if (setDisplayRegionLanguage)
                        {
                            if (setDisplay24hClock)
                            {
                                TextDay = AVFunctions.ToTitleCase(TileTimeMin.ToString("dddd", vCultureInfoReg));
                                TextDayDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString(DateDayLength + ", d MMMM", vCultureInfoReg));
                                TextDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString("d MMMM", vCultureInfoReg));
                            }
                            else
                            {
                                TextDay = AVFunctions.ToTitleCase(TileTimeMin.ToString("dddd", vCultureInfoReg));
                                TextDayDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString(DateDayLength + ", MMMM d", vCultureInfoReg));
                                TextDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString("MMMM d", vCultureInfoReg));
                            }
                        }
                        else
                        {
                            if (setDisplay24hClock)
                            {
                                TextDay = AVFunctions.ToTitleCase(TileTimeMin.ToString("dddd", vCultureInfoEng));
                                TextDayDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString(DateDayLength + " d MMMM", vCultureInfoEng));
                                TextDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString("d MMMM", vCultureInfoEng));
                            }
                            else
                            {
                                TextDay = AVFunctions.ToTitleCase(TileTimeMin.ToString("dddd", vCultureInfoEng));
                                TextDayDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString(DateDayLength + "MMMM d", vCultureInfoEng));
                                TextDateMonth = AVFunctions.ToTitleCase(TileTimeMin.ToString("MMMM d", vCultureInfoEng));
                            }
                        }
                        if (setDisplayDateYear) { TextYear = TileTimeMin.ToString(" yyyy"); }
                        if (setDisplayDateWeekNumber)
                        {
                            TextPositionSet(Setting_TextPositions.DayOnly, TextDay);
                            TextPositionSet(Setting_TextPositions.DayDateMonth, TextDayDateMonth + TextYear + " " + TextWeekNumber);
                            TextPositionSet(Setting_TextPositions.DateMonth, TextDateMonth + TextYear + " " + TextWeekNumber);
                        }
                        else
                        {
                            TextPositionSet(Setting_TextPositions.DayOnly, TextDay);
                            TextPositionSet(Setting_TextPositions.DayDateMonth, TextDayDateMonth + TextYear);
                            TextPositionSet(Setting_TextPositions.DateMonth, TextDateMonth + TextYear);
                        }
                    }

                    //Set Current Time Text
                    if (setDisplayCurrentTime)
                    {
                        //Set the full time string
                        if (setDisplay24hClock)
                        {
                            TextTimeFull = TileTimeMin.ToString("HH" + TextTimeSplit + "mm");
                            TextTimeHour = TileTimeMin.ToString("HH");
                            TextTimeMin = TileTimeMin.ToString("mm");
                        }
                        else
                        {
                            if (setLiveTileSizeName != "MediumTimeOnly")
                            {
                                TextTimeFull = TileTimeMin.ToString("%h" + TextTimeSplit + "mm");
                                TextTimeHour = TileTimeMin.ToString("%h");
                                TextTimeMin = TileTimeMin.ToString("mm");
                            }
                            else
                            {
                                TextTimeFull = TileTimeMin.ToString("hh" + TextTimeSplit + "mm");
                                TextTimeHour = TileTimeMin.ToString("hh");
                                TextTimeMin = TileTimeMin.ToString("mm");
                            }
                        }

                        //Set Current Time AM/PM Text
                        if (setDisplayAMPMClock)
                        {
                            if (setDisplayRegionLanguage)
                            {
                                TextTimeAmPm = TileTimeMin.ToString("tt", vCultureInfoReg);
                                if (String.IsNullOrEmpty(TextTimeAmPm)) { TextTimeAmPm = TileTimeMin.ToString("tt", vCultureInfoEng); }
                            }
                            else { TextTimeAmPm = TileTimeMin.ToString("tt", vCultureInfoEng); }
                        }

                        //Replace custom time text if enabled
                        if (setDisplayTimeCustomText)
                        {
                            string ReplacedTimeString = setDisplayTimeCustomTextString.Replace("*time*", TextTimeFull);
                            ReplacedTimeString = ReplacedTimeString.Replace("*timett*", TextTimeAmPm);
                            ReplacedTimeString = ReplacedTimeString.Replace("*date*", TextDateMonth);
                            ReplacedTimeString = ReplacedTimeString.Replace("*battery*", TextBatteryLevel);
                            ReplacedTimeString = ReplacedTimeString.Replace("*weather*", BgStatusWeatherCurrent);
                            ReplacedTimeString = ReplacedTimeString.Replace("*location*", BgStatusWeatherCurrentLocation);
                            ReplacedTimeString = ReplacedTimeString.Replace("*network*", BgStatusNetworkName);
                            TextTimeFull = ReplacedTimeString;
                            TextTimeSplit = ReplacedTimeString;
                            TextTimeHour = String.Empty;
                            TextTimeMin = String.Empty;
                        }

                        //Tile Numm Words Text
                        if (setLiveTileSizeName == "WideNumm")
                        {
                            TextTimeHour = AVFunctions.NumberToText(TextTimeHour);
                            TextTimeMin = AVFunctions.NumberToText(TextTimeMin);
                            if (TextTimeHour == "Zero") { TextTimeHour = "Twelve"; }
                            if (TextTimeMin == "Zero") { TextTimeMin = "O'Clock"; }
                        }
                    }
                }
                else
                {
                    //Check for 24h clock and set time
                    if (setDisplay24hClock)
                    {
                        if (TileTimeMin.Minute > 30) { TextTimeHour = TileTimeMin.AddHours(1).ToString("HH").Replace("00", "12"); }
                        else { TextTimeHour = TileTimeMin.ToString("HH").Replace("00", "12"); }
                    }
                    else
                    {
                        if (TileTimeMin.Minute > 30) { TextTimeHour = TileTimeMin.AddHours(1).ToString("%h"); }
                        else { TextTimeHour = TileTimeMin.ToString("%h"); }
                    }

                    //Set current time words text
                    if (TileTimeMin.Minute != 0 && TileTimeMin.Minute != 15 && TileTimeMin.Minute != 30 && TileTimeMin.Minute != 45)
                    {
                        if (TileTimeMin.Minute > 30) { TextTimeFull = "it is " + (60 - TileTimeMin.Minute) + " to " + TextTimeHour; }
                        else { TextTimeFull = "it is " + TileTimeMin.Minute + " past " + TextTimeHour; }
                    }
                    else if (TileTimeMin.Minute == 0) { TextTimeFull = "it is " + TextTimeHour + " o'clock"; }
                    else if (TileTimeMin.Minute == 15) { TextTimeFull = "quarter past " + TextTimeHour; }
                    else if (TileTimeMin.Minute == 30) { TextTimeFull = "it is half past " + TextTimeHour; }
                    else if (TileTimeMin.Minute == 45) { TextTimeFull = "quarter to " + TextTimeHour; }

                    //Set current date words text
                    TextWordsDate = "on " + TileTimeMin.ToString("ddd", vCultureInfoEng).ToLower() + " " + TileTimeMin.Day + " of " + TileTimeMin.ToString("MMMM", vCultureInfoEng).ToLower();
                }
            }
            catch { }
        }
    }
}