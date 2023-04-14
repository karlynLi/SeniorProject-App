using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Android;
using Xamarin.Forms;
using Android.Content;

using road_running.Models;
using System.Collections.Generic;
using road_running.Providers;
using Plugin.LocalNotification;
using Plugin.FirebasePushNotification;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using road_running.Droid;
using Android.Gms.Common;
using Android.Util;
using Firebase.Messaging;
using Firebase.Iid;

namespace road_running.Droid
{
    //[Activity(Label = "road_running")]
    [Activity(Label = "road_running", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // Maps 環境設定
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        protected override void OnStart()
        {
            base.OnStart();
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                    RequestPermissions(LocationPermissions, RequestLocationId);
                else
                    Console.WriteLine("Permissions already granted");
            }
        }
        public static Context context = global::Android.App.Application.Context;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Plugin.LocalNotification 創建頻道
            NotificationCenter.CreateNotificationChannel();

            
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug("Extras", "Key: {0} Value: {1}", key, value);
                }
            }

            // 檢查裝置是否可以用GoogleApi
            IsPlayServicesAvailable();
            //MyFirebaseMessagingSerevice 的頻道
            CreateNotificationChannel();

            Rg.Plugins.Popup.Popup.Init(this); // 多外觀alert視窗
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();

            // 掃QR code
            Xamarin.Essentials.Platform.Init(Application);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            //ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            var refreshedToken = FirebaseInstanceId.Instance.Token;
            var refreshedid = FirebaseInstanceId.Instance.Id;
            Console.WriteLine("Token:  " + refreshedToken);
            Console.WriteLine("ID:  " + refreshedid);
            MessagingCenter.Unsubscribe<string>(this, "myService");
            // 觸發訂閱FCM主題
            MessagingCenter.Subscribe<string>(this, "Subscribe_FCM_Topic", (string value) =>
            {
                FirebaseMessaging.Instance.SubscribeToTopic(value);
            });
            MessagingCenter.Send<string>("all", "Subscribe_FCM_Topic"); // 訂閱主題
            // 取消訂閱FCM主題
            //MessagingCenter.Subscribe<string>(this, "UnSubscribe_FCM_Topic", (string value) =>
            //{
            //    FirebaseMessaging.Instance.UnsubscribeFromTopic(value);
            //});
            // 觸發開啟Sercive_Beacon與Service_Gps
            MessagingCenter.Subscribe<string>(this , "myService", async(value) =>
            {
                var intent_Beacon = new Intent(context, typeof(Service_Beacon));
                var intent_GPS = new Intent(context, typeof(Service_Gps));
                var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
                if (value == "1" && AppShellInstance != null)
                {
                    GPS.IfCheckin = false;
                    //var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
                    List<BeaconRequest> initGetList = new List<BeaconRequest>();
                    initGetList = await BeaconListProvider.GetBeaconsAsync(AppShellInstance.Member_ID);
                    //initGetList.Add( new BeaconRequest { group_name = "暨大春健", running_ID = "A1", ifCheckin = "AlreadyChenkedIn", beaconList = new string[] { "12:3B:6A:1B:84:43", "12:3B:6A:1B:22:B1", "12:3B:6A:1B:22:08" } });
                    Console.WriteLine("lllllllllllllll"+initGetList[0].ifCheckin);
                    //Console.WriteLine("kkkkkkkkkk" + initGetList[0].beaconList);
                    if (initGetList[0].ifCheckin == "AlreadyChenkedIn" && initGetList[0].beaconList.Length > 0) //initGetList[0].ifCheckin == "AlreadyChenkedIn" && initGetList[0].beaconList.Length>0
                    {
                        GPS.IfCheckin = true;
                        //Console.WriteLine("type = "+ initGetList[0].beaconList.GetType());
                        intent_Beacon.PutExtra("list", initGetList[0].beaconList); // "list", initGetList[0].beaconList
                        intent_Beacon.PutExtra("rid", initGetList[0].running_ID);
                        intent_GPS.PutExtra("rid", initGetList[0].running_ID);

                        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                        {

                            //Service_Beacon ddd = new Service_Beacon();
                            Console.WriteLine("較高版本");
                            if (!isMyServiceRunning(typeof(Service_Beacon)))
                            {
                                StartForegroundService(intent_Beacon);
                            }
                            else
                            {
                                Console.WriteLine("已經開啟過Service_Beacon了");
                            }
                            if (!isMyServiceRunning(typeof(Service_Gps)))
                            {
                                StartForegroundService(intent_GPS);
                            }
                            else
                            {
                                Console.WriteLine("已經開啟過Service_Gps了");
                            }
                            //StartForegroundService(intent_Beacon);
                            //StartForegroundService(intent_GPS);
                            //StartService(new Intent(this, typeof(Service_Beacon)));
                            //ddd.RegisterNotification();
                        }
                        else
                        {
                            if (!isMyServiceRunning(typeof(Service_Beacon)))
                            {
                                StartService(intent_Beacon);
                            }
                            else
                            {
                                Console.WriteLine("已經開啟過Service_Beacon了");
                            }
                            if (!isMyServiceRunning(typeof(Service_Gps)))
                            {
                                StartService(intent_GPS);
                            }
                            else
                            {
                                Console.WriteLine("已經開啟過Service_Gps了");
                            }  
                        }
                    }
                }
                else
                {
                    if (isMyServiceRunning(typeof(Service_Beacon)))
                    {
                        StopService(intent_Beacon);
                    }
                    else
                    {
                        Console.WriteLine("已經關閉Service_Beacon了");
                    }
                    if (isMyServiceRunning(typeof(Service_Gps)))
                    {
                        StopService(intent_GPS);
                    }
                    else
                    {
                        Console.WriteLine("已經關閉Service_Gps了");
                    }
                }
            });
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
        // Google API 是否可以取得
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Console.WriteLine(GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Console.WriteLine("This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                Console.WriteLine("Google Play Services is available.");
                return true;
            }
        }

        // MyFirebaseMessagingSerevice Notification
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 200;
        // Android 8.0(API 26)以上，需要建立channel才能發送Notification
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        // 用於Google登入
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.Instance.OnAuthCompleted(result); // 登入完成，結果OnAuthCompleted於GoogleManger.cs中實作
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                    Console.WriteLine("Permissions granted");
                else
                    Console.WriteLine("Permissions denied");
            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
            //Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}