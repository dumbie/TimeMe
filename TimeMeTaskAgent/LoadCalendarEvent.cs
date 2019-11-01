using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Load Calendar Event
        async Task LoadCalendarEvent()
        {
            try
            {
                Debug.WriteLine("Loading current calendar event.");

                AppointmentStore AppointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);
                IReadOnlyList<Appointment> Appointments = await AppointmentStore.FindAppointmentsAsync(DateTimeNow, TimeSpan.FromDays(1));
                if (Appointments.Any())
                {
                    DateTimeOffset CalendarAppoStartTime = Appointments[0].StartTime;
                    if (!String.IsNullOrEmpty(Appointments[0].Subject)) { CalendarAppoName = Appointments[0].Subject; } else { CalendarAppoName = "Unknown event"; }
                    if (!String.IsNullOrEmpty(Appointments[0].Location)) { CalendarAppoLocation = Appointments[0].Location; }

                    if (Appointments[0].AllDay)
                    {
                        if (CalendarAppoStartTime.Date >= DateTimeNow.AddDays(1).Date)
                        {
                            if (!String.IsNullOrEmpty(CalendarAppoLocation)) { CalendarAppoSummary = "(T All day) " + CalendarAppoLocation; }
                            else { CalendarAppoSummary = "Tomorrow all the day"; }
                            CalendarAppoDateTime = "Tomorrow all day";
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(CalendarAppoLocation)) { CalendarAppoSummary = "(All day) " + CalendarAppoLocation; }
                            else { CalendarAppoSummary = "Today all the day"; }
                            CalendarAppoDateTime = "Today all day";
                        }
                    }
                    else
                    {
                        if (CalendarAppoStartTime.Date >= DateTimeNow.AddDays(1).Date)
                        {
                            if (!String.IsNullOrEmpty(CalendarAppoLocation))
                            {
                                if (setDisplay24hClock)
                                {
                                    CalendarAppoSummary = "(T " + CalendarAppoStartTime.ToString("HH:mm") + ") " + CalendarAppoLocation;
                                    CalendarAppoDateTime = "(T " + CalendarAppoStartTime.ToString("HH:mm") + ")";
                                }
                                else
                                {
                                    CalendarAppoSummary = "(T " + CalendarAppoStartTime.ToString("h:mm tt", vCultureInfoEng) + ") " + CalendarAppoLocation;
                                    CalendarAppoDateTime = "(T " + CalendarAppoStartTime.ToString("h:mm tt", vCultureInfoEng) + ")";
                                }
                            }
                            else
                            {
                                if (setDisplay24hClock)
                                {
                                    CalendarAppoSummary = "Tomorrow at " + CalendarAppoStartTime.ToString("HH:mm");
                                    CalendarAppoDateTime = "Tomorrow at " + CalendarAppoStartTime.ToString("HH:mm");
                                }
                                else
                                {
                                    CalendarAppoSummary = "Tomorrow at " + CalendarAppoStartTime.ToString("h:mm tt", vCultureInfoEng);
                                    CalendarAppoDateTime = "Tomorrow at " + CalendarAppoStartTime.ToString("h:mm tt", vCultureInfoEng);
                                }
                            }
                        }
                        else
                        {
                            //Check if event is active or needs to start
                            if (DateTimeNow >= CalendarAppoStartTime)
                            {
                                TimeSpan EventRemaining = CalendarAppoStartTime.Add(Appointments[0].Duration).Subtract(DateTimeNow);
                                int RemainDays = EventRemaining.Days; int RemainHours = EventRemaining.Hours; int RemainMinutes = EventRemaining.Minutes;

                                string EventRemainingTime = "";
                                if (RemainDays != 0) { EventRemainingTime = EventRemainingTime + RemainDays + "d "; }
                                if (RemainHours != 0) { EventRemainingTime = EventRemainingTime + RemainHours + "h "; }
                                if (RemainMinutes != 0) { EventRemainingTime = EventRemainingTime + RemainMinutes + "m "; }
                                if (String.IsNullOrEmpty(EventRemainingTime)) { EventRemainingTime = "a minute "; }

                                CalendarAppoEstimated = "Ends in " + EventRemainingTime;
                            }
                            else
                            {
                                //Set event start time
                                if (!String.IsNullOrEmpty(CalendarAppoLocation))
                                {
                                    if (setDisplay24hClock) { CalendarAppoSummary = "(" + CalendarAppoStartTime.ToString("HH:mm") + ") " + CalendarAppoLocation; }
                                    else { CalendarAppoSummary = "(" + CalendarAppoStartTime.ToString("h:mm tt", vCultureInfoEng) + ") " + CalendarAppoLocation; }
                                }
                                else
                                {
                                    if (setDisplay24hClock) { CalendarAppoSummary = "Today at " + CalendarAppoStartTime.ToString("HH:mm"); }
                                    else { CalendarAppoSummary = "Today at " + CalendarAppoStartTime.ToString("h:mm tt", vCultureInfoEng); }
                                }

                                //Set time till start
                                TimeSpan EventStart = CalendarAppoStartTime.Subtract(DateTimeNow);
                                int StartDays = EventStart.Days; int StartHours = EventStart.Hours; int StartMinutes = EventStart.Minutes;

                                string EventStartTime = "";
                                if (StartDays != 0) { EventStartTime = EventStartTime + StartDays + "d "; }
                                if (StartHours != 0) { EventStartTime = EventStartTime + StartHours + "h "; }
                                if (StartMinutes != 0) { EventStartTime = EventStartTime + StartMinutes + "m "; }
                                if (String.IsNullOrEmpty(EventStartTime)) { EventStartTime = "a minute "; }

                                CalendarAppoEstimated = "Starts in " + EventStartTime;
                            }
                        }
                    }
                }
                else { Debug.WriteLine("No calendar event has been found."); }
            }
            catch { }
        }
    }
}