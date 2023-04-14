using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace road_running.Droid
{
    //[Application]
    public class MainApplication : Android.App.Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug("NotificationApp", "===========aaaa=========");
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";

                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
            }

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
            FirebasePushNotificationManager.Initialize(this,false);
#endif
            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                Console.WriteLine(p.Data);
                Console.WriteLine("==========CrossFirebasePushNotification==========CrossFirebasePushNotification==========CrossFirebasePushNotification========");
                Log.Debug("NotificationApp", "===========jjj=========");
                
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    Console.WriteLine($"{data.Key} : {data.Value}");
                }

                //if (p.Data.ContainsKey("color"))
                //{
                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new MainActivity()
                //        {
                //            BackgroundColor = Color.FromHex($"{p.Data["color"]}")
                //        });
                //    });
                //}
            };

        }
    }
}