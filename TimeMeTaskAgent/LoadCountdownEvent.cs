using ArnoldVinkCode;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Load countdown event from XML
        private async Task LoadCountdownEvent()
        {
            try
            {
                using (Stream OpenStreamForReadAsync = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("TimeMeCountdown.xml"))
                {
                    XDocument XDocument = XDocument.Load(OpenStreamForReadAsync);
                    OpenStreamForReadAsync.Dispose();

                    //Load set countdown event from XML
                    XElement xmlCountdownEvent = XDocument.Descendants("TimeMeCountdown").Elements("Count").OrderBy(x => x.Attribute("CountDate").Value).ThenBy(x => x.Attribute("CountName").Value).FirstOrDefault();
                    if (xmlCountdownEvent != null)
                    {
                        DateTime LoadedDate = DateTime.Parse(xmlCountdownEvent.Attribute("CountDate").Value);

                        //Datetime to string
                        string ConvertedDate = "";
                        if ((bool)vApplicationSettings["DisplayRegionLanguage"]) { ConvertedDate = AVFunctions.ToTitleCase(LoadedDate.Date.ToString("d MMMM yyyy", vCultureInfoReg)); }
                        else { ConvertedDate = LoadedDate.Date.ToString("d MMMM yyyy", vCultureInfoEng); }

                        //Calculate the days left
                        CountdownEventDate = (LoadedDate.Date.Subtract(DateTimeNow.Date).Days).ToString() + "d";
                        if (CountdownEventDate == "0d") { CountdownEventDate = "today"; }

                        //Set the countdown name
                        CountdownEventName = xmlCountdownEvent.Attribute("CountName").Value;
                    }
                }
            }
            catch { }
        }
    }
}