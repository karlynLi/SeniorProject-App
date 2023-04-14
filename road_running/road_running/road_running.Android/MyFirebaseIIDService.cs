
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Firebase.Iid;

namespace road_running.Droid
{
    [Service]
    [IntentFilter(new[] {"com.google.firebase.INSTANCE_ID_EVENT"})]
    class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token) { }
    }
}
