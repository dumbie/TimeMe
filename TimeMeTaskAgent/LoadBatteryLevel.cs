using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.System.Power;

namespace TimeMeTaskAgent
{
    partial class ScheduledAgent
    {
        //Load battery level
        async Task LoadBatteryLevel()
        {
            try
            {
                Debug.WriteLine("Updating current battery level.");

                //Check if there is battery status available
                BatteryStatus BatteryStatus = PowerManager.BatteryStatus;
                if (BatteryStatus == BatteryStatus.NotPresent) { BatteryLevel = "error"; }
                else
                {
                    //Set battery remaining percent and time
                    BatteryLevel = PowerManager.RemainingChargePercent.ToString();
                    BatteryLevelInt = Convert.ToInt32(BatteryLevel);
                    TimeSpan BatteryDischargeTime = PowerManager.RemainingDischargeTime;

                    //Set the battery status
                    if (BatteryStatus == BatteryStatus.Charging || PowerManager.PowerSupplyStatus != PowerSupplyStatus.NotPresent) { BatteryCharging = true; BatteryTime = "Unknown "; }
                    else if (BatteryDischargeTime.Days > 31)
                    {
                        BatteryDischargeTime = setDevStatusBatteryTime;
                        int BatteryDays = BatteryDischargeTime.Days; int BatteryHours = BatteryDischargeTime.Hours; int BatteryMinutes = BatteryDischargeTime.Minutes;

                        if (BatteryDischargeTime == new TimeSpan()) { BatteryTime = "Unknown "; }
                        else
                        {
                            if (BatteryDays != 0) { BatteryTime = BatteryTime + BatteryDays + "d "; }
                            if (BatteryHours != 0) { BatteryTime = BatteryTime + BatteryHours + "h "; }
                            if (BatteryMinutes != 0) { BatteryTime = BatteryTime + BatteryMinutes + "m "; }
                            if (String.IsNullOrEmpty(BatteryTime)) { BatteryTime = "Unknown "; }
                        }
                    }
                    else
                    {
                        vApplicationSettings["DevStatusBatteryTime"] = BatteryDischargeTime;
                        int BatteryDays = BatteryDischargeTime.Days; int BatteryHours = BatteryDischargeTime.Hours; int BatteryMinutes = BatteryDischargeTime.Minutes;

                        if (BatteryDays != 0) { BatteryTime = BatteryTime + BatteryDays + "d "; }
                        if (BatteryHours != 0) { BatteryTime = BatteryTime + BatteryHours + "h "; }
                        if (BatteryMinutes != 0) { BatteryTime = BatteryTime + BatteryMinutes + "m "; }
                        if (String.IsNullOrEmpty(BatteryTime)) { BatteryTime = "Unknown "; }
                    }

                    //Set the used battery status icon
                    if (BatteryLevelInt <= 10) { BatteryIcon = "10"; }
                    else if (BatteryLevelInt <= 20) { BatteryIcon = "20"; }
                    else if (BatteryLevelInt <= 30) { BatteryIcon = "30"; }
                    else if (BatteryLevelInt <= 40) { BatteryIcon = "40"; }
                    else if (BatteryLevelInt <= 50) { BatteryIcon = "50"; }
                    else if (BatteryLevelInt <= 60) { BatteryIcon = "60"; }
                    else if (BatteryLevelInt <= 70) { BatteryIcon = "70"; }
                    else if (BatteryLevelInt <= 80) { BatteryIcon = "80"; }
                    else if (BatteryLevelInt <= 90) { BatteryIcon = "90"; }
                    if (BatteryCharging) { BatteryIcon = "Cha" + BatteryIcon; }
                    else { BatteryIcon = "Dis" + BatteryIcon; }
                }

                //Notification - Current Battery Level
                ShowNotiBattery();

                //Notification - Low Battery Level
                ShowNotiLowBattery();

                //Notification - Battery Saver Warning
                await ShowNotiBatterySaver();
            }
            catch
            {
                Debug.WriteLine("Updating the battery level failed.");
                BatteryLevel = "error";
            }
        }
    }
}