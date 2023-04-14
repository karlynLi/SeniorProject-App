using road_running.Services;
using road_running.Views;
using System;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.Models;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Plugin.FirebasePushNotification;
//using Android.Content;
//using Android.Preferences;
using System.Collections.Generic;
using road_running.Providers;
using Rg.Plugins.Popup.Services;

namespace road_running
{
    public partial class App : Application
    {
        public static List<Member> MLoginResult { get; set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            bool hasKey = Preferences.ContainsKey("log");
            bool shasKey = Preferences.ContainsKey("login");

            if (hasKey == true)
            {
                //Log();
                MainPage = new NavigationPage(new MiddlelogPage());
                //MainPage = new AppShell() { Email = MLoginResult[0].Email, Name = MLoginResult[0].Name, Id_card = MLoginResult[0].Id_card, Member_ID = MLoginResult[0].Member_ID, Photo_code = MLoginResult[0].Photo_code, Photo = MLoginResult[0].Photo };
                // 導入進一個空的測試業面然後透過那個葉面跳轉
            }
            else if(shasKey == true)
            {
                Console.WriteLine("工作人員登入");
                MainPage = new NavigationPage(new MidLog());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }

            //MainPage = new NavigationPage(new MainPage());

            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
        }
        private async void Log()
        {
            string email = Preferences.Get("mail", "");
            string pass = Preferences.Get("password", "");
            //var myPopup = new DisPlayMessage(pass, email , "返回");
            //await PopupNavigation.Instance.PushAsync(myPopup);
            //await myPopup.PopupClosedTask;
            Member lgn = new Member()
            {
                Email = email,
                Password = pass
            };
            MLoginResult =  await MLoginProvider.LoginAsync(lgn);
            //var myPopup = new DisPlayMessage(MLoginResult[0].Id_card, MLoginResult[0].Email, "返回");

            //await PopupNavigation.Instance.PushAsync(myPopup);
            //await myPopup.PopupClosedTask;
            MainPage = new AppShell() { Email = MLoginResult[0].Email, Name = MLoginResult[0].Name, Id_card = MLoginResult[0].Id_card, Member_ID = MLoginResult[0].Member_ID, Photo_code = MLoginResult[0].Photo_code, Photo = MLoginResult[0].Photo };
            //Xamarin.Forms.Application.Current.MainPage = new AppShell() { Email = MLoginResult[0].Email, Name = MLoginResult[0].Name, Id_card = MLoginResult[0].Id_card, Member_ID = MLoginResult[0].Member_ID, Photo_code = MLoginResult[0].Photo_code, Photo = MLoginResult[0].Photo };
            //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            //Shell.Current.Navigation.PushAsync();

            MessagingCenter.Send<string>("1", "myService");
        }
        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"==============Token==========:{e.Token}");

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
