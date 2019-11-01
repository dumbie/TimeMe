using System;
using Windows.UI.Xaml;

namespace TimeMe
{
    partial class MainPage
    {
        //Register application page events
        void ApplicationEventsRegister()
        {
            try
            {
                //Register Suspending and Resuming events
                Application.Current.Suspending += this.OnSuspending;
                Application.Current.Resuming += this.OnResuming;
                Application.Current.EnteredBackground += this.OnEnteredBackground;
                Application.Current.LeavingBackground += this.OnLeavingBackground;

                //Register application activated event
                App.OnApplicationActivated += OnApplicationActivatedEvent;
            }
            catch { }
        }

        //Disable application page events
        void ApplicationEventsDisable()
        {
            try
            {
                //Disable Suspending and Resuming events
                Application.Current.Suspending -= this.OnSuspending;
                Application.Current.Resuming -= this.OnResuming;
                Application.Current.EnteredBackground -= this.OnEnteredBackground;
                Application.Current.LeavingBackground -= this.OnLeavingBackground;

                //Disable application activated event
                App.OnApplicationActivated -= OnApplicationActivatedEvent;
            }
            catch { }
        }

        //Handle Application Activated Event
        async void OnApplicationActivatedEvent(object sender, EventArgs e) { await ApplicationNavigation(); }
    }
}