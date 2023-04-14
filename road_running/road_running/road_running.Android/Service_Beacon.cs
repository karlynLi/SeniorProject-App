
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

namespace road_running.Droid
{
    [Service(Label = "Service_Beacon")]
    [IntentFilter(new String[] { "com.letrun.Service_Beacon" })]
    public class Service_Beacon : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();
            Console.WriteLine("創建Beacon服務");
            userInfo = Xamarin.Forms.Shell.Current as AppShell; // 取得登入會員資料
        }

        public static AppShell userInfo; // 登入會員資料

        // 從MainActivity取得之資訊
        public static String[] list;
        public static string running_id;


        // 藍牙掃描相關
        Thread BeaconThread;  // 創建thread
        //public static int RssiValue = 0;
        //public static AdapterConnectStatus BleStatus;
        //public static IBle ble;
        //public static IDev ConnectDevice = null;
        //public static List<IDev> ScanDevices = new List<IDev>();
        public List<BluetoothDevice> Devices;
        BluetoothManager Manager;
        private List<ScanFilter>  Filters = new List<ScanFilter>(); // 過濾藍芽清單
        private ScanSettings  Settings ;
        private LeScanCallback scanCallback = new LeScanCallback();

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            Console.WriteLine("Beacon啟動服務");
            
            list = intent?.GetStringArrayExtra("list"); // 取得MainActivity的Beacon清單
            running_id = intent?.GetStringExtra("rid"); // 取得running_ID
            //notificationManager.Notify(100, notificationBuilder.Build());
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                StartForeground(100, ReturnNotif());
            }
            //RegisterNotification();
            //ReturnNotif();
            // start your service logic here
            ScanFilter.Builder builder = new ScanFilter.Builder();
            ScanFilter filter = builder.Build();
            BeaconThread = new Thread(() =>
            {
                if (Manager == null) Manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(BluetoothService);
                if (Devices != null)
                {
                    if (Devices.Count != 0)
                    {
                        foreach (BluetoothDevice d in Devices) Console.WriteLine("BLE", "-----> " + d.Address);
                    }
                }
                Devices = new List<BluetoothDevice>();
                if (Manager.Adapter.IsDiscovering)
                {
                    Log.Debug("BLE", "ALREADY SCANNING");
                }
                else
                {
                    Log.Debug("BLE", "START SCANNING");
                    for (int i = 0; i < list.Length; i++)
                    {
                        this.Filters.Add(new ScanFilter.Builder().SetDeviceAddress(list[i]).Build());
                    }
                    this.Settings = new ScanSettings.Builder().SetScanMode(Android.Bluetooth.LE.ScanMode.LowLatency).SetCallbackType(ScanCallbackType.FirstMatch).Build();
                    Manager.Adapter.BluetoothLeScanner.StartScan(this.Filters, this.Settings, scanCallback);
                }
            });
            BeaconThread.Start();
            return StartCommandResult.Sticky;
        }

        //public override void OnTaskRemoved(Intent rootIntent)
        //{
        //    Console.WriteLine("Beacon_service just got killed....");
        //    Log.Info("SO", $"{GetType().Name} just got killed....");
        //    StopForeground(true);
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
                StopService(new Intent(this, typeof(Service_Beacon)));
                Console.WriteLine("==================二號");
            }
            StopSelf();
            base.OnTaskRemoved(rootIntent);
            //will kill threads when app is manually killed by user.
            //Java.Lang.JavaSystem.Exit(0);
        }

        // 偵測到Beacon
        public class LeScanCallback : ScanCallback
        {
            public LeScanCallback()
            {
            }
            public override void OnScanResult(ScanCallbackType cbt, ScanResult res)
            {
                //if (!m.Devices.Contains(res.Device)) m.Devices.Add(res.Device);
                double d = Math.Pow(10.0, (double)(-69 - res.Rssi) / (10 * 2));
                //設定偵測到Beacon時的通知
                var notification = new NotificationRequest
                {
                    BadgeNumber = 1,
                    Description = "距離前方補給站大約剩下5公尺！",
                    Title = $"Beacon 編號:{res.Device.Address}!",
                    ReturningData = "Beacon",
                    NotificationId = 1337, //ID:1337
                    // 設定通知的Icon (必須)
                    Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                    {
                        IconSmallName = new Plugin.LocalNotification.AndroidOption.AndroidIcon("ic_stat_icon")
                    }
                };
                NotificationCenter.Current.Show(notification); // 顯示通知
                Log.Debug("BLE", "Device found: " + res.Device.Address + " | " + res.Device.Type + " | " + res.Rssi + " | " + d + "m | " +res.Device.Name);
                UpdateLocationProvider.GetAnsAsync(userInfo.Member_ID, running_id, res.Device.Address); // 上傳位置資訊
            }
        }

        // 前景通知
        private static string foregroundChannelId = "9001";
        private static Context context = global::Android.App.Application.Context;
        public Notification ReturnNotif()
        {
            // Building intent
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notifBuilder = new Notification.Builder(context, foregroundChannelId)
                .SetContentTitle("Beacon")
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
        public override void OnDestroy() // 當服務被關閉時
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
            Manager.Adapter.BluetoothLeScanner.StopScan(scanCallback);
            Console.WriteLine("關閉Beacon服務");
        }

        public override IBinder OnBind(Intent intent)
        {
            //binder = new Service_BeaconBinder(this);
            return null;
        }
    }

    public class Service_BeaconBinder : Binder
    {
        readonly Service_Beacon service;

        public Service_BeaconBinder(Service_Beacon service)
        {
            this.service = service;
        }

        public Service_Beacon GetService_Beacon()
        {
            return service;
        }
    }
}
