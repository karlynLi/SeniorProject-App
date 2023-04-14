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
    public partial class MidLog : ContentPage
    {
        public List<Staff> SLogResult { get; set; }
        public MidLog()
        {
            InitializeComponent();
            Console.WriteLine("快登入囉");
            Log();
           // Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            //Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        private async void Log()
        {
            Console.WriteLine("Log辣");
            string staff_ID = Preferences.Get("id", "");
            string name = Preferences.Get("name", "");
            string id_card = Preferences.Get("id_card", "");
            string mail = Preferences.Get("mail", "");
            string photo_code1 = Preferences.Get("photo_code", "");
            string photo1 = Preferences.Get("photo", "");
            string pass = Preferences.Get("pass", "");
            Console.WriteLine("mail======" + mail);
            Console.WriteLine("name======" + name);
            Console.WriteLine("id_card======" + id_card);
            Console.WriteLine("staff_ID======" + staff_ID);
            Console.WriteLine("photo_code======" + photo_code1);
            Console.WriteLine("photo======" + photo1);
            Staff lgn = new Staff()
            {
                Staff_ID = staff_ID,
                Password = pass
            };
            SLogResult = await SLoginProvider.SLoginAsync(lgn);
            Application.Current.MainPage = new SShell() { Email = mail, Name = name, Id_card = id_card, Staff_ID = staff_ID, Photo_code = SLogResult[0].Photo_code, Photo = SLogResult[0].Photo};
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            //Application.Current.MainPage = new AppShell() { Email = email, Name = name, Id_card = id_card, Member_ID = member_ID, Photo_code = photo_code, Photo = photo };
            //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            //await Shell.Current.GoToAsync("//fly/MainPage");
            //MainPage = new NavigationPage(new MainPage());
        }
    }
}