using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using road_running.ViewModels;
using road_running.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http.Headers;
using road_running.Providers;
using road_running.Services;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
//using Android.Content;
//using Android.Preferences;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MLoginPage : ContentPage
    {
        public readonly IGoogleManager _googleManager; // Models裡的GoogleUsers的介面
        public static bool IsLogedIn;
        GoogleUser GoogleUser = new GoogleUser();
        public bool LogedIn { get; set; }

        public static List<Member> MLoginResult { get; set; }

        public MLoginPage()
        {
            _googleManager = DependencyService.Get<IGoogleManager>();
            //_googleManager = DependencyService.Get<IGoogleManager>();
            //CheckUserLoggedIn();
            InitializeComponent();

            email.Text = Preferences.Get("mail", string.Empty);
            password.Text = Preferences.Get("password", string.Empty);
        }

        private void CheckUserLoggedIn()
        {
            _googleManager.Login(OnLoginComplete);
        }
        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                Console.WriteLine("MLoginPage: " + GoogleUser.Name);
                Console.WriteLine("MLoginPage: " + GoogleUser.Email);
                Console.WriteLine("MLoginPage: " + GoogleUser.Picture);
                IsLogedIn = true;
                GLogin(googleUser);
            }
            else
            {
                DisplayAlert("Message", message, "Ok");
            }
        }
        private void Google_Login(object sender, EventArgs e)
        {
            _googleManager.Login(OnLoginComplete);
        }
        private void GoogleLogout()
        {
            _googleManager.Logout();
            IsLogedIn = false;
        }
        private void Google_Logout(object sender, EventArgs e)
        {
            _googleManager.Logout();
            IsLogedIn = false;
            //txtName.Text = "Name :";
            //txtEmail.Text = "Email: ";
            //imgProfile.Source = "";
        }
        void Change_mode(object sender, ToggledEventArgs e)
        {
            if(e.Value == true)
            {
                Preferences.Set("mail", email.Text);
                Preferences.Set("password", password.Text);
                Preferences.Set("log", LogedIn);
            }
            else
            {
                Preferences.Clear();
            }
        }

        private async void GLogin(GoogleUser googleUser)
        {
            Member res = await GmailCheckProvider.CheckMail(googleUser);
            if (res.ans == "yes") // 已經有此信箱
            {
                Console.WriteLine("=========有此信箱");
                // Preference記下資訊
                Preferences.Set("googleLogin", true);
                Preferences.Set("log", true);
                Preferences.Set("mail", res.Email);
                Preferences.Set("name", res.Name);
                Preferences.Set("id_card", res.Id_card);
                Preferences.Set("member_ID", res.Member_ID);
                Preferences.Set("photo_code", res.Photo_code);
                Preferences.Set("photo", res.Photo);
                
                // 將user資訊灌入AppShell
                Application.Current.MainPage = new AppShell() { Email = res.Email, Name = res.Name, Id_card = res.Id_card, Member_ID = res.Member_ID, Photo_code = res.Photo_code, Photo = res.Photo };
                // 前往主畫面
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

                MessagingCenter.Send<string>("1", "myService");
            }
            else // 會員無此信箱，前往填寫個人資料註冊
            {
                await Navigation.PushAsync(new GoogleEnrollPage(googleUser));
                Console.WriteLine("=========無信箱");
            }
        }

        private async void MLogin(object sender, EventArgs e)
        {
            //ISharedPreferences sharedPreferences = context.getSharedPreferences(“user”, Context.MODE_PRIVATE);
            Member lgn = new Member()
            {
                Email = email.Text,
                Password = password.Text
            };
            MLoginResult = await MLoginProvider.LoginAsync(lgn);
            // List<Member> Store;
            //Console.WriteLine(MLoginResult[0].Name);
            Console.WriteLine(MLoginResult[0].ans);
            if (MLoginResult[0].ans == "no")
            {
                var myPopup = new DisPlayMessage("登入失敗", "密碼錯誤", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            else if (MLoginResult[0].ans == "email doesn't exist")
            {
                var myPopup = new DisPlayMessage("登入失敗", "找不到此信箱", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            else
            {
                LogedIn = true;
                Preferences.Set("mail", email.Text);
                Preferences.Set("password", password.Text);
                Preferences.Set("log", LogedIn);

                Console.WriteLine(MLoginResult[0].Photo);
                Console.WriteLine(MLoginResult[0].Id_card);
                //await DisplayAlert("會員 : " + MLoginResult[0].Member_ID, "登入成功", "Go");
                var myPopup = new DisPlayMessage("登入成功", $"會員 : {MLoginResult[0].Member_ID}登入成功", "前往首頁");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                Preferences.Set("name", MLoginResult[0].Name);
                Preferences.Set("id_card", MLoginResult[0].Id_card);
                Preferences.Set("member_ID", MLoginResult[0].Member_ID);
                Preferences.Set("photo_code", MLoginResult[0].Photo_code);
                Preferences.Set("photo", MLoginResult[0].Photo);
                //Application.Current.MainPage = new AppShell() { Store = LoginResult };
                Application.Current.MainPage = new AppShell() { Email = MLoginResult[0].Email, Name = MLoginResult[0].Name,Id_card=MLoginResult[0].Id_card, Member_ID = MLoginResult[0].Member_ID, Photo_code = MLoginResult[0].Photo_code, Photo = MLoginResult[0].Photo };
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                // road_running.Android.MainActivity.cs 會接收到這邊傳給MessagingCenter：標題：myService 內容：1(代表開始偵測beacon)
                MessagingCenter.Send<string>("1", "myService");
            }
        }
        
        private async void MEnroll(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EnrollPage());
        }
        private async void MForget(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgetPassword());
        }
    }
}