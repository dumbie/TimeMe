using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace TimeMe
{
    partial class MainPage
    {
        //Load active alarms
        void TimersLoad()
        {
            try
            {
                IReadOnlyList<ScheduledToastNotification> PlannedToasts = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();
                if (PlannedToasts.Any())
                {
                    lb_TimerListBox.Items.Clear();
                    btn_TimerRemove.IsEnabled = true;
                    txt_ActiveTimers.Text = "Current active timer alarms, tap on the\nalarm that you want to remove below:";

                    //Remove expired alarms
                    foreach (ScheduledToastNotification Alarm in PlannedToasts) { if (Alarm.DeliveryTime < DateTime.Now) { ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(Alarm); } }

                    //Add alarms to list
                    if (PlannedToasts.Any())
                    {
                        foreach (ScheduledToastNotification Alarm in PlannedToasts.OrderBy(x => x.DeliveryTime))
                        {
                            string ConvertedTime = "";
                            if ((bool)vApplicationSettings["Display24hClock"])
                            { ConvertedTime = Alarm.DeliveryTime.ToString("HH:mm"); }
                            else { ConvertedTime = Alarm.DeliveryTime.ToString("h:mm tt", vCultureInfoEng); }

                            string ConvertedDate = "";
                            if ((bool)vApplicationSettings["DisplayRegionLanguage"])
                            { ConvertedDate = AVFunctions.ToTitleCase(Alarm.DeliveryTime.ToString("d MMMM yyyy", vCultureInfoReg)); }
                            else { ConvertedDate = Alarm.DeliveryTime.ToString("d MMMM yyyy", vCultureInfoEng); }

                            lb_TimerListBox.Items.Add(new TimerList() { TimerName = Alarm.Id, TimerContent = Alarm.Content.InnerText.Replace("TimeMe", ""), TimerTime = ConvertedTime + " (" + ConvertedDate + ")" });
                        }
                    }
                    else
                    {
                        lb_TimerListBox.Items.Clear();
                        btn_TimerRemove.IsEnabled = false;
                        txt_ActiveTimers.Text = "There are currently no timer alarms active,\nadd a timer alarm as a reminder for you.";
                    }
                }
                else
                {
                    lb_TimerListBox.Items.Clear();
                    btn_TimerRemove.IsEnabled = false;
                    txt_ActiveTimers.Text = "There are currently no timer alarms active,\nadd a timer alarm as a reminder for you.";
                }
            }
            catch { txt_ActiveTimers.Text = "Failed to load the timer alarms, please try again."; }
        }

        //Add Timer Function
        async void btn_TimerAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AlarmName = "";
                DateTime BeginAlarmTime = DateTime.Now;

                //Check for empty timer time
                if (String.IsNullOrWhiteSpace(txtbox_TimerMinutes.Text)) { txtbox_TimerMinutes.Text = "0"; }
                if (String.IsNullOrWhiteSpace(txtbox_TimerHours.Text)) { txtbox_TimerHours.Text = "0"; }
                if (String.IsNullOrWhiteSpace(txtbox_TimerDays.Text)) { txtbox_TimerDays.Text = "0"; }

                //Check alarm time for 0
                if (txtbox_TimerMinutes.Text == "0" && txtbox_TimerHours.Text == "0" && txtbox_TimerDays.Text == "0")
                {
                    txtbox_TimerDays.Text = "";
                    txtbox_TimerHours.Text = "";
                    txtbox_TimerMinutes.Text = "";
                    txtbox_TimerDays.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm time, and try again.", "TimeMe").ShowAsync();
                    return;
                }

                //Check for other characters
                if (Regex.IsMatch(txtbox_TimerMinutes.Text, "(\\D+)") || Regex.IsMatch(txtbox_TimerHours.Text, "(\\D+)") || Regex.IsMatch(txtbox_TimerDays.Text, "(\\D+)"))
                {
                    txtbox_TimerDays.Text = "";
                    txtbox_TimerHours.Text = "";
                    txtbox_TimerMinutes.Text = "";
                    txtbox_TimerDays.Focus(FocusState.Programmatic);
                    await new MessageDialog("The timer alarm time can only contain numbers, please check your entered time.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if alarm time exceeds time limits
                if (Convert.ToInt32(txtbox_TimerDays.Text) > 31)
                {
                    txtbox_TimerDays.Text = "";
                    txtbox_TimerDays.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm time,\n31 days is the maximum.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if alarm time exceeds time limits
                if (Convert.ToInt32(txtbox_TimerHours.Text) > 24)
                {
                    txtbox_TimerHours.Text = "";
                    txtbox_TimerHours.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm time,\n24 hours is the maximum.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if alarm time exceeds time limits
                if (Convert.ToInt32(txtbox_TimerMinutes.Text) > 60)
                {
                    txtbox_TimerMinutes.Text = "";
                    txtbox_TimerMinutes.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm time,\n60 minutes is the maximum.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if alarm times starts with a 0
                if (txtbox_TimerMinutes.Text.Length > 1 && txtbox_TimerMinutes.Text.StartsWith("0"))
                {
                    txtbox_TimerMinutes.Text = "";
                    txtbox_TimerMinutes.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm minutes time.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if alarm times starts with a 0
                if (txtbox_TimerHours.Text.Length > 1 && txtbox_TimerHours.Text.StartsWith("0"))
                {
                    txtbox_TimerHours.Text = "";
                    txtbox_TimerHours.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm hours time.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if alarm times starts with a 0
                if (txtbox_TimerDays.Text.Length > 1 && txtbox_TimerDays.Text.StartsWith("0"))
                {
                    txtbox_TimerDays.Text = "";
                    txtbox_TimerDays.Focus(FocusState.Programmatic);
                    await new MessageDialog("Please enter a valid timer alarm days time.", "TimeMe").ShowAsync();
                    return;
                }

                BeginAlarmTime = DateTime.Now.AddSeconds(-DateTime.Now.Second).AddDays(Convert.ToInt32(txtbox_TimerDays.Text)).AddHours(Convert.ToInt32(txtbox_TimerHours.Text)).AddMinutes(Convert.ToInt32(txtbox_TimerMinutes.Text));
                AlarmName = "Timer set in " + txtbox_TimerDays.Text + "days " + txtbox_TimerHours.Text + "hours " + txtbox_TimerMinutes.Text + "mins";

                //Check date overlapping with existing alarms
                ToastNotifier CreateToastNotifier = ToastNotificationManager.CreateToastNotifier();
                if (CreateToastNotifier.GetScheduledToastNotifications().Any(x => x.DeliveryTime.LocalDateTime.ToString() == BeginAlarmTime.ToString()))
                {
                    await new MessageDialog("Timer alarm overlaps with an existing alarm please choose another time point.", "TimeMe").ShowAsync();
                    return;
                }

                //Check if date and time is in the future
                if (BeginAlarmTime < DateTime.Now)
                {
                    await new MessageDialog("The alarm timer date and time point needs to be in the future to be added as timer.", "TimeMe").ShowAsync();
                    return;
                }

                XmlDocument ToastAlarmXmlReady = new XmlDocument();
                ToastAlarmXmlReady.LoadXml("<toast duration=\"long\" launch=\"ToastTimer\"><visual><binding template=\"ToastImageAndText02\"><image id=\"1\" src=\"ms-appx:///Assets/Icons/AlarmClock.png\"/><text id=\"1\">TimeMe</text><text id=\"2\">" + AlarmName + "</text></binding></visual><audio src=\"ms-winsoundevent:Notification.Looping.Alarm\" loop=\"true\"/></toast>");
                CreateToastNotifier.AddToSchedule(new ScheduledToastNotification(ToastAlarmXmlReady, BeginAlarmTime) { Id = new Random().Next(1, 999999999).ToString() });

                txtbox_TimerDays.Text = "";
                txtbox_TimerHours.Text = "";
                txtbox_TimerMinutes.Text = "";
                TimersLoad();

                if (lb_TimerListBox.Items.Count == 1) { await new MessageDialog("Your TimeMe timer alarm has been successfully set and will alarm as entered, please note that your volume needs to be turned up and the app must have permission to show notifications.", "TimeMe").ShowAsync(); }
            }
            catch
            {
                TimersLoad();
                await new MessageDialog("Failed to add your new TimeMe timer alarm, please try again.", "TimeMe").ShowAsync();
            }
        }

        //Remove Timer Function
        async void btn_TimerRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check if alarm timer is selected
                if (!lb_TimerListBox.SelectedItems.Any())
                {
                    await new MessageDialog("Please select a timer alarm time to remove.", "TimeMe").ShowAsync();
                    return;
                }

                foreach (object SelItem in lb_TimerListBox.SelectedItems)
                {
                    TimerList SelectedItem = (TimerList)SelItem;
                    try
                    {
                        foreach (ScheduledToastNotification Alarm in ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Where(x => x.Id == SelectedItem.TimerName))
                        { ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(Alarm); }
                    }
                    catch { await new MessageDialog("Failed to remove the timer alarm, perhaps the timer has already expired?", "TimeMe").ShowAsync(); }
                }
                TimersLoad();
            }
            catch { }
        }

        //Refresh Timer Button
        void btn_TimerRefresh_Click(object sender, RoutedEventArgs e) { TimersLoad(); }
    }
}