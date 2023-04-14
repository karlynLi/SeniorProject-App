using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_UpdatePassword : ContentPage
    {
        public string staffid;
        public Staff NewPassword { get; set; }
        public S_UpdatePassword(string id)
        {
            InitializeComponent();
            staffid = id;
        }
        public async void UpdatePass(object sender, EventArgs e)
        {
            if (pass1.Text == pass2.Text)
            {
                Staff UpdatePassword = new Staff
                {
                    Staff_ID = staffid,
                    Password = pass2.Text
                };
                NewPassword = await S_UpdatePassProvider.S_UpdatePassAsync(UpdatePassword);
                if (NewPassword.ans == "yes")
                {
                    pass1.Text = "";
                    pass2.Text = "";
                    await DisplayAlert("成功", "更新密碼成功!", "返回首頁");
                    //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    await Navigation.PopAsync();
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    await DisplayAlert("失敗", "輸入不可為空", "重新輸入");
                }
            }
            else
            {
                await DisplayAlert("失敗", "兩次輸入密碼不相同", "重新輸入");
            }
        }
    }
}