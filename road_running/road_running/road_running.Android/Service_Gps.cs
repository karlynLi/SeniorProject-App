
using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Quick.Xamarin.BLE;
using Quick.Xamarin.BLE.Abstractions;
using road_running.Models;
using Xamarin.Forms;
using Android.Util;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Plugin.LocalNotification;
using road_running.Providers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using road_running.Views;

namespace road_running.Droid
{
    [Service(Label = "Service_Gps")]
    [IntentFilter(new String[] { "com.letrun.Service_Gps" })]
    public class Service_Gps : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();
            Console.WriteLine("GPS創建服務");
            userInfo = Xamarin.Forms.Shell.Current as AppShell; // 取得登入會員資料
        }
        //public override void OnTaskRemoved(Intent rootIntent)
        //{
        //    Console.WriteLine("GPS_service just got killed....");
        //    Log.Info("SO", $"{GetType().Name} just got killed....");
        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
        //    {
        //        StopForeground(StopForegroundFlags.Remove);
        //    }
        //    else
        //    {
        //        StopForeground(true);
        //    }
        //    StopSelf();
        //    base.OnTaskRemoved(rootIntent);
        //}
        public override void OnTaskRemoved(Intent rootIntent)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                StopForeground(true);
                Console.WriteLine("==================一號");
            }
            else
            {
                StopService(new Intent(this, typeof(Service_Gps)));
                Console.WriteLine("==================二號");
            }
            StopSelf();
            base.OnTaskRemoved(rootIntent);
            //will kill threads when app is manually killed by user.
            //Java.Lang.JavaSystem.Exit(0);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            //{
            //    StopForeground(StopForegroundFlags.Remove);
            //}
            //else
            //{
            //    StopForeground(true);
            //}
            //StopSelf();
            Console.WriteLine("GPS關閉服務");
        }

        Thread GPSThread;  // 創建thread
        public static AppShell userInfo; // 登入會員資料
        //public static string running_id; // 路跑id

        IBinder binder;

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            // start your service logic here
            Console.WriteLine("GPS啟動服務");
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                StartForeground(101, ReturnNotif()); // 啟動前景服務
            }
            string running_id = intent?.GetStringExtra("rid"); // 取得running_ID
            _ = Get_Position(running_id);
            // Return the correct StartCommandResult for the type of service you are building
            return StartCommandResult.Sticky;
        }

        private async Task Get_Position(string rid)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            //GPSThread = new Thread(async () =>
            //{
            //    while (true)
            //    {
            //        var location = await Geolocation.GetLocationAsync(request);
            //        if (location != null)
            //        {
            //            GPS.location = location;
            //            await MapsProvider.PostPositionAsync(userInfo.Member_ID, running_id, location.Longitude, location.Latitude);
            //            Console.WriteLine("====================== " + userInfo.Member_ID + "'s Real-Time GPS ============================");
            //            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
            //            await Task.Delay(5000);
            //        }
            //    }
            //});
            //GPSThread.Start();
            while (true)
            {
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    GPS.location = location;
                    await MapsProvider.PostPositionAsync(userInfo.Member_ID, rid, location.Longitude, location.Latitude);
                    Console.WriteLine("====================== " + userInfo.Member_ID + "'s Real-Time GPS ============================");
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    await Task.Delay(5000);
                }
            }
        }

        // 前景通知
        private static string foregroundChannelId = "9002";
        private static Context context = global::Android.App.Application.Context;
        public Notification ReturnNotif()
        {
            // Building intent
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notifBuilder = new Notification.Builder(context, foregroundChannelId)
                .SetContentTitle("GPS")
                .SetContentText("正在掃描中")
                .SetSmallIcon(Resource.Drawable.ic_stat_icon)
                .SetOngoing(true)
                .SetContentIntent(pendingIntent);

            // Building channel if API verion is 26 or above
            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "Title", NotificationImportance.Low);
                notificationChannel.Importance = NotificationImportance.Low;
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                var notifManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notifManager != null)
                {
                    notifBuilder.SetChannelId(foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }
            return notifBuilder.Build();
        }

        public override IBinder OnBind(Intent intent)
        {
            //binder = new Service_GpsBinder(this);
            return null;
        }
    }



    public class Service_GpsBinder : Binder
    {
        readonly Service_Gps service;

        public Service_GpsBinder(Service_Gps service)
        {
            this.service = service;
        }

        public Service_Gps GetService_Gps()
        {
            return service;
        }
    }
}
