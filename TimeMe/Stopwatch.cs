using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace TimeMe
{
    partial class MainPage
    {
        //Stopwatch Variables
        int StopWatchSplit = 0;
        DateTime StopWatchTotalTime = DateTime.MinValue;
        DateTime StopWatchSplitTime = DateTime.MinValue;
        TimeSpan CalcStopWatchTotalTime = TimeSpan.Zero;
        TimeSpan CalcStopWatchSplitTime = TimeSpan.Zero;
        DispatcherTimer StopwatchTimer = new DispatcherTimer();

        //Load stored stopwatch times
        async Task StopwatchLoad()
        {
            try
            {
                //Load Stopwatch times from XML file
                lb_StopWatchListBox.Items.Clear();
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeStopwatch.xml"))
                {
                    XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                    OpenStreamForReadAsync.Dispose();

                    foreach (XElement XElement in XDocument.Descendants("TimeMeStopwatch").Elements("Split"))
                    {
                        string ConvertedTime = "";
                        DateTime DateTimeNow = DateTime.Parse(XElement.Attribute("Date").Value);
                        if ((bool)vApplicationSettings["Display24hClock"]) { ConvertedTime = DateTimeNow.ToString("HH:mm"); }
                        else { ConvertedTime = DateTimeNow.ToString("h:mmtt", vCultureInfoEng); }
                        if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedTime = ConvertedTime + "/" + DateTimeNow.ToString("dMMMyyyy", vCultureInfoReg); }
                        else { ConvertedTime = ConvertedTime + "/" + DateTimeNow.ToString("dMMMyyyy", vCultureInfoEng); }

                        if (XElement.Attribute("SplitId").Value == "1") { lb_StopWatchListBox.Items.Insert(0, new StopWatchList() { SplitIdStart = XElement.Attribute("SplitId").Value + ",", SplitId = "", Split = XElement.Attribute("Split").Value, Total = XElement.Attribute("Total").Value, Date = ConvertedTime }); }
                        else { lb_StopWatchListBox.Items.Insert(0, new StopWatchList() { SplitId = XElement.Attribute("SplitId").Value + ",", Split = XElement.Attribute("Split").Value, Total = XElement.Attribute("Total").Value, Date = ConvertedTime }); }
                    }

                    if (lb_StopWatchListBox.Items.Any())
                    {
                        txt_StopWatchPrevTimes.Text = "Current stopwatch splitted times:";
                        btn_StopWatchClearSplits.IsEnabled = true;
                    }
                    else
                    {
                        txt_StopWatchPrevTimes.Text = "There are no stopwatch split times set.";
                        btn_StopWatchClearSplits.IsEnabled = false;
                    }
                }
            }
            catch { txt_StopWatchPrevTimes.Text = "Failed to load stopwatch split times."; }
        }

        //Clear Times from Stopwatch
        async void btn_StopWatchClearTimes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Nullable<bool> MessageDialogResult = null;
                MessageDialog MessageDialog = new MessageDialog("Do you really want to clear all the current stopwatch split times results?\nPlease note that this will also clear all your previous stored stopwatch times.", "TimeMe");
                MessageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => MessageDialogResult = true)));
                MessageDialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler((cmd) => MessageDialogResult = null)));
                await MessageDialog.ShowAsync();
                if (MessageDialogResult == true)
                {
                    StopWatchSplit = 0;
                    lb_StopWatchListBox.Items.Clear();
                    txt_StopWatchInfo.Text = "The stopwatch is currently not running.";
                    txt_StopWatchPrevTimes.Text = "There are no stopwatch split times set.";
                    btn_StopWatchClearSplits.IsEnabled = false;
                    await StopwatchResetXml();
                }
            }
            catch { }
        }

        //Reset Stopwatch Xml Times
        async Task StopwatchResetXml()
        {
            try
            {
                StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeStopwatch.xml", CreationCollisionOption.ReplaceExisting);
                using (Stream OpenStreamForWriteAsync = await CreateFileAsync.OpenStreamForWriteAsync())
                {
                    using (XmlWriter XmlWriter = XmlWriter.Create(OpenStreamForWriteAsync, new XmlWriterSettings() { Async = true }))
                    {
                        await XmlWriter.WriteStartElementAsync(null, "TimeMeStopwatch", null);
                        await XmlWriter.WriteEndElementAsync();
                    }
                }
            }
            catch { }
        }

        //Start/Resume Stopwatch
        async void btn_StopWatchStartSplit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ConvertedTime = "";
                DateTime DateTimeNow = DateTime.Now;
                if ((bool)vApplicationSettings["Display24hClock"]) { ConvertedTime = DateTimeNow.ToString("HH:mm"); }
                else { ConvertedTime = DateTimeNow.ToString("h:mmtt", vCultureInfoEng); }
                if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedTime = ConvertedTime + "/" + DateTimeNow.ToString("dMMMyyyy", vCultureInfoReg); }
                else { ConvertedTime = ConvertedTime + "/" + DateTimeNow.ToString("dMMMyyyy", vCultureInfoEng); }

                //Splittime Stopwatch
                if (btn_StopWatchStartSplit.Content.ToString() == "Split Time")
                {
                    //Add split time to listview
                    StopWatchSplit++;
                    if (StopWatchSplit == 1) { lb_StopWatchListBox.Items.Insert(0, new StopWatchList() { SplitIdStart = StopWatchSplit.ToString() + ",", SplitId = "", Split = txt_StopWatchTimeSplit.Text, Total = txt_StopWatchTimeTotal.Text, Date = ConvertedTime }); }
                    else { lb_StopWatchListBox.Items.Insert(0, new StopWatchList() { SplitId = StopWatchSplit.ToString() + ",", Split = txt_StopWatchTimeSplit.Text, Total = txt_StopWatchTimeTotal.Text, Date = ConvertedTime }); }

                    //Save split time to XML file
                    try
                    {
                        using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeStopwatch.xml"))
                        {
                            XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                            OpenStreamForReadAsync.Dispose();

                            XDocument.Element("TimeMeStopwatch").Add(new XElement("Split", new XAttribute("SplitId", StopWatchSplit.ToString()), new XAttribute("Split", txt_StopWatchTimeSplit.Text), new XAttribute("Total", txt_StopWatchTimeTotal.Text), new XAttribute("Date", DateTimeNow)));

                            StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeStopwatch.xml", CreationCollisionOption.ReplaceExisting);
                            using (Stream OpenStreamForWriteAsync = await CreateFileAsync.OpenStreamForWriteAsync()) { XDocument.Save(OpenStreamForWriteAsync); }
                        }
                    }
                    catch { }

                    txt_StopWatchPrevTimes.Text = "Current stopwatch splitted times:";
                    StopWatchSplitTime = DateTime.Now;
                    return;
                }

                //Resume Stopwatch
                if (btn_StopWatchStartSplit.Content.ToString() == "Resume")
                {
                    //Prevent application lock screen
                    try { vDisplayRequest.RequestActive(); } catch { }

                    btn_StopWatchStartSplit.Content = "Split Time";
                    btn_StopWatchPauseReset.Content = "Pause Watch";

                    StopWatchTotalTime = DateTime.Now.Subtract(CalcStopWatchTotalTime);
                    StopWatchSplitTime = DateTime.Now.Subtract(CalcStopWatchSplitTime);

                    btn_StopWatchClearSplits.IsEnabled = false;

                    txt_StopWatchInfo.Text = "The stopwatch has resumed at: " + ConvertedTime;
                    StopwatchTimer.Start();
                    return;
                }

                //Start Stopwatch
                if (btn_StopWatchStartSplit.Content.ToString() == "Start Time")
                {
                    //Prevent application lock screen
                    try { vDisplayRequest.RequestActive(); } catch { }

                    btn_StopWatchStartSplit.Content = "Split Time";
                    btn_StopWatchPauseReset.Content = "Pause Watch";

                    StopWatchTotalTime = DateTime.Now;
                    StopWatchSplitTime = DateTime.Now;

                    StopwatchTimer = new DispatcherTimer();
                    StopwatchTimer.Interval = TimeSpan.FromMilliseconds(10);
                    StopwatchTimer.Tick += delegate
                    {
                        CalcStopWatchTotalTime = DateTime.Now.Subtract(StopWatchTotalTime);
                        CalcStopWatchSplitTime = DateTime.Now.Subtract(StopWatchSplitTime);
                        txt_StopWatchTimeTotal.Text = CalcStopWatchTotalTime.ToString(@"hh\:mm\:ss\,ff");
                        txt_StopWatchTimeSplit.Text = CalcStopWatchSplitTime.ToString(@"hh\:mm\:ss\,ff");
                    };

                    btn_StopWatchPauseReset.IsEnabled = true;
                    btn_StopWatchClearSplits.IsEnabled = false;

                    txt_StopWatchInfo.Text = "The stopwatch has started at: " + ConvertedTime;
                    StopwatchTimer.Start();
                    return;
                }
            }
            catch { }
        }

        //Stop/Reset Stopwatch
        void btn_StopWatchStopReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ConvertedTime = "";
                if ((bool)vApplicationSettings["Display24hClock"]) { ConvertedTime = DateTime.Now.ToString("HH:mm"); }
                else { ConvertedTime = DateTime.Now.ToString("h:mmtt", vCultureInfoEng); }
                if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedTime = ConvertedTime + "/" + DateTime.Now.ToString("dMMMyyyy", vCultureInfoReg); }
                else { ConvertedTime = ConvertedTime + "/" + DateTime.Now.ToString("dMMMyyyy", vCultureInfoEng); }

                //Reset Stopwatch
                if (btn_StopWatchPauseReset.Content.ToString() == "Reset Watch")
                {
                    //Prevent application lock screen
                    try { vDisplayRequest.RequestActive(); } catch { }

                    btn_StopWatchStartSplit.Content = "Split Time";
                    btn_StopWatchPauseReset.Content = "Pause Watch";

                    StopWatchSplit = 0;
                    StopWatchTotalTime = DateTime.Now;
                    StopWatchSplitTime = DateTime.Now;

                    btn_StopWatchClearSplits.IsEnabled = false;
                    txt_StopWatchInfo.Text = "The stopwatch has started at: " + ConvertedTime;
                    StopwatchTimer.Start();
                    return;
                }

                //Pause Stopwatch
                if (btn_StopWatchPauseReset.Content.ToString() == "Pause Watch")
                {
                    //Allow application lock screen
                    try { vDisplayRequest.RequestRelease(); } catch { }

                    btn_StopWatchPauseReset.Content = "Reset Watch";
                    btn_StopWatchStartSplit.Content = "Resume";

                    if (lb_StopWatchListBox.Items.Any())
                    {
                        txt_StopWatchPrevTimes.Text = "Current stopwatch splitted times:";
                        btn_StopWatchClearSplits.IsEnabled = true;
                    }

                    txt_StopWatchInfo.Text = "The stopwatch has paused at: " + ConvertedTime;
                    StopwatchTimer.Stop();
                    return;
                }
            }
            catch { }
        }
    }
}