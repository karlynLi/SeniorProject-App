using System;
using Com.Airbnb.Lottie;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Android;
using System.Threading.Tasks;
using Android.Util;
using Android.Content;
using static Android.Resource;


//using static Lottie.Forms.Resource;

namespace road_running.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity, Android.Animation.Animator.IAnimatorListener//global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity// global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity//Activity, Android.Animation.Animator.IAnimatorListener//global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        public void OnAnimationCancel(Android.Animation.Animator animation)
        {

        }


        public void OnAnimationEnd(Android.Animation.Animator animation)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }


        public void OnAnimationRepeat(Android.Animation.Animator animation)
        {

        }



        public void OnAnimationStart(Android.Animation.Animator animation)
        {

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.SplashLayout);

            var animation = FindViewById<Com.Airbnb.Lottie.LottieAnimationView>(Resource.Id.splashview);

            animation.AddAnimatorListener(this);


        }
        //圖片層
        //static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        //public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        //{
        //    base.OnCreate(savedInstanceState, persistentState);
        //    Log.Debug(TAG, "SplashActivity.OnCreate");
        //}

        //// Launches the startup task
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    Task startupWork = new Task(() => { SimulateStartup(); });
        //    startupWork.Start();
        //}

        //// Simulates background work that happens behind the splash screen
        //async void SimulateStartup()
        //{
        //    Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
        //    await Task.Delay(5000); // Simulate a bit of startup work.
        //    Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
        //    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        //}
    }
}