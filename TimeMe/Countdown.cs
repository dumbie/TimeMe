using ArnoldVinkCode;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace TimeMe
{
    partial class MainPage
    {
        //Textbox keyup enter function
        void txtbox_CountName_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == VirtualKey.Enter)
                {
                    btn_CountdownAddEvent.Focus(FocusState.Programmatic);
                    btn_CountdownAddEvent_Click(null, null);
                }
            }
            catch { }
        }

        //Refresh Countdown Button
        async void btn_CountdownRefresh_Click(object sender, RoutedEventArgs e) { await CountdownLoad(); }

        //Load stored countdown events
        async Task CountdownLoad()
        {
            try
            {
                //Check if date is in the future
                if (datepick_CountDate.Date.Date <= DateTime.Now.Date) { datepick_CountDate.Date = DateTime.Now.AddDays(1).Date; }

                //Load countdown events from XML file
                lb_CountdownListBox.Items.Clear();
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeCountdown.xml"))
                {
                    XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                    OpenStreamForReadAsync.Dispose();

                    //Remove all expired event dates
                    foreach (XElement XElement in XDocument.Descendants("TimeMeCountdown").Elements("Count").ToList())
                    {
                        DateTime LoadedDate = DateTime.Parse(XElement.Attribute("CountDate").Value);
                        if (LoadedDate.Date < DateTime.Now.Date) { XElement.Remove(); }
                    }

                    //Add new year if no events are set
                    if (!XDocument.Descendants("TimeMeCountdown").Elements("Count").Any()) { XDocument.Element("TimeMeCountdown").Add(new XElement("Count", new XAttribute("CountId", new Random().Next(1, 999999999).ToString()), new XAttribute("CountName", "New Year's Day"), new XAttribute("CountDate", new DateTime((DateTime.Now.AddYears(1).Year), 1, 1).Date))); }

                    //Save the made changes to XML file
                    StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeCountdown.xml", CreationCollisionOption.ReplaceExisting);
                    using (Stream OpenStreamForWriteAsync = await CreateFileAsync.OpenStreamForWriteAsync()) { XDocument.Save(OpenStreamForWriteAsync); }

                    //Load all set countdown events from XML
                    foreach (XElement XElement in XDocument.Descendants("TimeMeCountdown").Elements("Count"))
                    {
                        DateTime LoadedDate = DateTime.Parse(XElement.Attribute("CountDate").Value);

                        //Datetime to string for listview
                        string ConvertedDate = "";
                        if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedDate = AVFunctions.ToTitleCase(LoadedDate.Date.ToString("d MMMM yyyy", vCultureInfoReg)); }
                        else { ConvertedDate = LoadedDate.Date.ToString("d MMMM yyyy", vCultureInfoEng); }

                        //Calculate days left for listview
                        string ConvertedDays = (LoadedDate.Date.Subtract(DateTime.Now.Date).Days).ToString();

                        //Add countdown event to listview
                        if (ConvertedDays == "0") { lb_CountdownListBox.Items.Insert(0, new CountdownList() { CountId = XElement.Attribute("CountId").Value, CountName = XElement.Attribute("CountName").Value, CountDate = ConvertedDate, CountDays = "Tod" }); }
                        else { lb_CountdownListBox.Items.Insert(0, new CountdownList() { CountId = XElement.Attribute("CountId").Value, CountName = XElement.Attribute("CountName").Value, CountDate = ConvertedDate, CountDays = ConvertedDays }); }
                    }

                    //Sort countdown events by days
                    //lb_CountdownListBox.Items.Cast<CountdownList>().OrderBy(x => x.CountDays);

                    txt_CountdownDates.Text = "Currently set event countdown dates:";
                }
            }
            catch { txt_CountdownDates.Text = "Failed to load the countdown events."; }
        }

        //Add Countdown Event
        async void btn_CountdownAddEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check for empty countdown name
                if (String.IsNullOrWhiteSpace(txtbox_CountName.Text))
                {
                    txtbox_CountName.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter an event name to add it to the countdown list.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if date is in the future
                if (datepick_CountDate.Date.Date <= DateTime.Now.Date)
                {
                    datepick_CountDate.Date = DateTime.Now.AddDays(1).Date;
                    await new MessageDialog("The countdown event date needs to be set in the future to be added.", "TimeMe").ShowAsync();
                    return;
                }

                //Datetime to string for listview
                string ConvertedDate = "";
                if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                { ConvertedDate = AVFunctions.ToTitleCase(datepick_CountDate.Date.Date.ToString("d MMMM yyyy", vCultureInfoReg)); }
                else { ConvertedDate = datepick_CountDate.Date.Date.ToString("d MMMM yyyy", vCultureInfoEng); }

                //Calculate days left for listview
                string ConvertedDays = (datepick_CountDate.Date.Date.Subtract(DateTime.Now.Date).Days).ToString();

                //Set the countdown item id
                string CountdownId = new Random().Next(1, 999999999).ToString();

                //Save countdown event to XML file
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeCountdown.xml"))
                {
                    XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                    OpenStreamForReadAsync.Dispose();

                    //Check if countdown event is already set
                    foreach (XElement XElement in XDocument.Descendants("TimeMeCountdown").Elements("Count"))
                    {
                        if (XElement.Attribute("CountName").Value == txtbox_CountName.Text && DateTime.Parse(XElement.Attribute("CountDate").Value) == datepick_CountDate.Date.Date)
                        {
                            await new MessageDialog("It seems like this countdown event has already been set.", "TimeMe").ShowAsync();
                            return;
                        }
                    }

                    XDocument.Element("TimeMeCountdown").Add(new XElement("Count", new XAttribute("CountId", CountdownId), new XAttribute("CountName", txtbox_CountName.Text), new XAttribute("CountDate", datepick_CountDate.Date.Date)));

                    StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeCountdown.xml", CreationCollisionOption.ReplaceExisting);
                    using (Stream OpenStreamForWriteAsync = await CreateFileAsync.OpenStreamForWriteAsync()) { XDocument.Save(OpenStreamForWriteAsync); }
                }

                //Add countdown event to listview
                lb_CountdownListBox.Items.Insert(0, new CountdownList() { CountId = CountdownId, CountName = txtbox_CountName.Text, CountDate = ConvertedDate, CountDays = ConvertedDays });

                //Sort countdown events by days
                //lb_CountdownListBox.Items.Cast<CountdownList>().OrderBy(x => x.CountDays);
            }
            catch { await new MessageDialog("Failed to add the countdown event, please try again.", "TimeMe").ShowAsync(); }
        }

        //Remove Countdown Event
        async void btn_CountdownRemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check if countdown event is selected
                if (!lb_CountdownListBox.SelectedItems.Any())
                {
                    await new MessageDialog("Please select a countdown event to remove from the countdown list.", "TimeMe").ShowAsync();
                    return;
                }

                //Check the selected removal items
                foreach (object SelItem in lb_CountdownListBox.SelectedItems)
                {
                    CountdownList SelectedItem = (CountdownList)SelItem;
                    try
                    {
                        using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeCountdown.xml"))
                        {
                            XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                            OpenStreamForReadAsync.Dispose();

                            //Load all set countdown events from XML
                            foreach (XElement XElement in XDocument.Descendants("TimeMeCountdown").Elements("Count")) { if (XElement.Attribute("CountId").Value == SelectedItem.CountId) { XElement.Remove(); } }

                            //Save removal changes to XML file
                            StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeCountdown.xml", CreationCollisionOption.ReplaceExisting);
                            using (Stream OpenStreamForWriteAsync = await CreateFileAsync.OpenStreamForWriteAsync()) { XDocument.Save(OpenStreamForWriteAsync); }
                        }
                    }
                    catch { await new MessageDialog("Failed to remove the countdown event, please try again.", "TimeMe").ShowAsync(); }
                }

                //Reload countdown items
                await CountdownLoad();
            }
            catch { await new MessageDialog("Failed to remove the countdown event, please try again.", "TimeMe").ShowAsync(); }
        }

        //Reset Countdown Xml Events
        async Task CountdownResetXml()
        {
            try
            {
                StorageFile CreateFileAsync = await ApplicationData.Current.LocalFolder.CreateFileAsync("TimeMeCountdown.xml", CreationCollisionOption.ReplaceExisting);
                using (Stream OpenStreamForWriteAsync = await CreateFileAsync.OpenStreamForWriteAsync())
                {
                    using (XmlWriter XmlWriter = XmlWriter.Create(OpenStreamForWriteAsync, new XmlWriterSettings() { Async = true }))
                    {
                        await XmlWriter.WriteStartElementAsync(null, "TimeMeCountdown", null);
                        await XmlWriter.WriteEndElementAsync();
                    }
                }
            }
            catch { }
        }
    }
}