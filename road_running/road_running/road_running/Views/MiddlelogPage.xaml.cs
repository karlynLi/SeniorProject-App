using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MiddlelogPage : ContentPage
    {
        public static List<Member> MLoginResult { get; set; }
        public MiddlelogPage()
        {
            InitializeComponent();
            Log();
        }
        private async void Log()
        {

            string email = Preferences.Get("mail", "");
            string name = Preferences.Get("name", "");
            string id_card = Preferences.Get("id_card", "");
            string member_ID = Preferences.Get("member_ID", "");
            string photo_code = Preferences.Get("photo_code", "");
            string photo = Preferences.Get("photo", "");
            //Console.WriteLine("PagePagePage:  " + Shell.Current.CurrentPage);
            Console.WriteLine("mail======"+email);
            Console.WriteLine("name======" + name);
            Console.WriteLine("id_card======" + id_card);
            Console.WriteLine("member_ID======" + member_ID);
            Console.WriteLine("photo_code======" + photo_code);
            Console.WriteLine("photo======" + photo);
            string pass = Preferences.Get("password", "");
            Member lgn = new Member()
            {
                Email = email,
                Password = pass
            };
            MLoginResult = await MLoginProvider.LoginAsync(lgn);
            Application.Current.MainPage = new AppShell() { Email = email, Name = name, Id_card = id_card, Member_ID = member_ID, Photo_code = photo_code, Photo = photo };
            //Application.Current.MainPage = new AppShell() { Email = email, Name = name, Id_card = id_card, Member_ID = member_ID, Photo_code = photo_code, Photo = photo };
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            //await Shell.Current.GoToAsync("//fly/MainPage");
            //MainPage = new NavigationPage(new MainPage());
            MessagingCenter.Send<string>("1", "myService");
        }
    }
}