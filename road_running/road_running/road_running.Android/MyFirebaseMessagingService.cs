using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Firebase.Messaging;
using road_running;
using System.Collections.Generic;
using Android.Support.V4.App;
using road_running.Models;
using road_running.Providers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace road_running.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public MyFirebaseMessagingService()
        {
        }
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            
            Console.WriteLine("OnMessageReceive!!");
            RemoteMessage.Notification notification = message.GetNotification();
            Log.Debug(TAG, "Message Priority: " + message.Priority);
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "data: " + message.Data);
            // 如果收到的訊息是數據消息, 開始執行Beacon、GPS的Service
            if (notification == null) {
                foreach (var key in message.Data.Keys)
                {
                    Console.WriteLine(key + "+++++++++++" + message.Data[key]);
                    if (key == "Service" && message.Data[key] == "1")
                    {
                        Start_Service("1");
                    }
                    else if (key == "Service" && message.Data[key] == "0")
                    {
                        Start_Service("0");
                    }
                }
            }
            else // 若是通知消息，顯示通知給使用者
            {
                SendNotification(notification.Title, notification.Body, message.Data);
            }
        }

        // 判斷Service是否已經在運行
        private bool isMyServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);

            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }

        // 啟動Service
        void Start_Service(string i)
        {
            if (i == "1")
            {
                MessagingCenter.Send<string>("1", "myService");
            }
            else if (i == "0")
            {
                MessagingCenter.Send<string>("0", "myService");
            }
        }

        void SendNotification(string messageTitle ,string messageBody, IDictionary<string, string> data)
        {

            Console.WriteLine("SendNotification!!!!!");

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                Console.WriteLine(key + "--------------" + data[key]);
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this,
                                                          MainActivity.NOTIFICATION_ID,
                                                          intent,
                                                          PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                                      .SetSmallIcon(Resource.Drawable.ic_stat_icon)
                                      .SetContentTitle(messageTitle)
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);

            notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());
        }
    }
}
