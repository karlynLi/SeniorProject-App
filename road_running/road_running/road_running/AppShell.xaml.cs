using road_running.ViewModels;
using road_running.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using road_running.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using road_running.Providers;
using Xamarin.Essentials;

namespace road_running
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static IGoogleManager _googleManager;
        private string photo;
        private string email;
        private string name;
        private string photocode;
        private string idcard;
        public static List<Member> InfoResult{ get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }

        public string Photo_code
        {
            get { return photocode; }
            set
            {
                photocode = value;
                OnPropertyChanged(nameof(Photo_code));
            }
        }
        public string Id_card
        {
            get { return idcard; }
            set
            {
                idcard = value;
                OnPropertyChanged(nameof(Id_card));
            }
        }
        public static List<Member> MLoginResult { get; set; }
        public string Member_ID { get; set; }
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            bool getinfo = Preferences.Get("googleLogin",false);
            _googleManager = DependencyService.Get<IGoogleManager>();
            if (getinfo == true)
            {
                Safe.IsVisible = false;
            }
            //Console.WriteLine(Photo);
            BindingContext = this;
            //Email = Email;
            //Name = Name;
            //Photo = Photo;
            //Log();
        }

        //private async void Getpic()
        //{
        //    Member Getpic = new Member()
        //    {
        //        Member_ID = Member_ID
        //    };
        //    List<Member> getpic = await GetAboutProvider.GetInfoAsync(Getpic);
        //    photocode = getpic[0].Photo_code;
        //}
        private async void GetInfo(object sender, EventArgs e)
        {
            Member Getinfo = new Member()
            {
                Member_ID = Member_ID
            };
            InfoResult = await GetAboutProvider.GetInfoAsync(Getinfo);
            Console.WriteLine("==Shell==");
            Console.WriteLine(InfoResult);
            string Birth = InfoResult[0].Birthday.ToString("yyyy-MM-dd");
            //await Shell.Current.GoToAsync("//AboutPage");
            await Shell.Current.GoToAsync($"//AboutPage?memberid={Member_ID}&name={InfoResult[0].Name}&birthday={Birth}&email={InfoResult[0].Email}&idcard={InfoResult[0].Id_card}&phone={InfoResult[0].Phone}&address={InfoResult[0].Address}&contactname={InfoResult[0].Contact_name}&contactphone={InfoResult[0].Contact_phone}&relation={InfoResult[0].Relation}&photo_code={InfoResult[0].Photo_code}&photo={InfoResult[0].Photo}");
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}?Name = {InfoResult.Name}&Email = {InfoResult.Email}");&birthday={InfoResult[0].Birthday}



        }
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            // Google登出
            bool isGLogin = Preferences.Get("googleLogin", false);
            if (isGLogin == true)
            {
                if (_googleManager != null)
                {
                    Console.WriteLine("幹你娘幹你娘幹你娘幹你娘幹你娘幹你娘幹你娘幹你娘幹你娘幹你娘幹你娘");
                    _googleManager.Logout();
                }
            }
            // 清除Preferences
            Preferences.Clear();
            //Member._googleManager.Logout();
            //Application.Current.MainPage = new NavigationPage(new MidLog());
            //await Shell.Current.GoToAsync("//MainPage");
            //Application.Current.MainPage = new MainPage();
            await Shell.Current.GoToAsync("//MainPage1");
            //Application.Current.MainPage = new LoginPage();
            
            
            //Application.Current.MainPage = new AppShell();
            //Application.Current.MainPage = new NavigationPage(new MidLog());
            //await Application.Current.MainPage.Navigation.PopAsync();
            //Application.Current.MainPage.Navigation.PushAsync(new MidLog());
            //Application.Current.MainPage = new MidLog();
            //await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
