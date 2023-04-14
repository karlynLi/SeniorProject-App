using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using road_running.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.Providers;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_LoginPage : ContentPage
    {
        public List<Staff> SLogResult { get; set; }
        public bool SLogedIn { get; set; }
        public S_LoginPage()
        {
            InitializeComponent();
        }
        private async void SLogin(object sender, EventArgs e)
        {
            Staff Slog = new Staff()
            {
                Staff_ID = staffid.Text,
                Password = UserPassword.Text
            };
            SLogResult = await SLoginProvider.SLoginAsync(Slog);
            if (SLogResult[0].ans == "no")
            {
                var myPopup = new DisPlayMessage("登入失敗", "密碼錯誤", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            else if (SLogResult[0].ans == "staff_ID doesn't exist")
            {
                var myPopup = new DisPlayMessage("登入失敗", "找不到此會員編號", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            else
            {
                SLogedIn = true;
                Preferences.Set("login", true);
                Preferences.Set("id", staffid.Text);
                Preferences.Set("pass", UserPassword.Text);
                Preferences.Set("mail", SLogResult[0].Email);
                Preferences.Set("id_card", SLogResult[0].Id_card);
                Preferences.Set("photo_code1", SLogResult[0].Photo_code);
                Preferences.Set("photo1", SLogResult[0].Photo);
                Preferences.Set("name", SLogResult[0].Name);
                string PASS = Preferences.Get("password", "");
                Console.WriteLine(PASS);
                var myPopup = new DisPlayMessage("登入成功", $"會員 : {SLogResult[0].Staff_ID}登入成功", "前往首頁");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await DisplayAlert("會員 : " + SLogResult[0].Staff_ID, "登入成功", "Go");
                Application.Current.MainPage = new SShell() { Email = SLogResult[0].Email, Name = SLogResult[0].Name, Id_card = SLogResult[0].Id_card, Staff_ID = SLogResult[0].Staff_ID, Photo_code = SLogResult[0].Photo_code, Photo = SLogResult[0].Photo };
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            //Application.Current.MainPage = new SShell();
            //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        private async void SEnroll(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new S_EnrollPage());
        }
        private async void SForget(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new S_ForgetPassword());
        }
    }
}