using System;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePassword : ContentPage
    {
        public string memberid;
        public Member NewPassword { get; set; }
        public UpdatePassword(string id)
        {
            InitializeComponent();
            memberid = id;
        }
        public async void UpdatePass(object sender, EventArgs e)
        {
            if (pass1.Text == pass2.Text)
            {
                Member UpdatePassword = new Member
                {
                    Member_ID = memberid,
                    Password = pass2.Text
                };
                NewPassword = await UpdatePassProvider.UpdatePassAsync(UpdatePassword);
                if(NewPassword.ans == "yes")
                {
                    pass1.Text = "";
                    pass2.Text = "";
                    var myPopup = new DisPlayMessage("驗證結果", "更新密碼成功!", "返回首頁");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                    //await DisplayAlert("成功", "更新密碼成功!", "返回首頁");
                    //await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    await Navigation.PopAsync();
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else if (NewPassword.ans == "密碼需包含英文字母與數字並長度大於8")
                {
                    var myPopup = new DisPlayMessage("驗證結果", NewPassword.ans , "返回首頁");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                }
                else if(NewPassword.ans == "No data sent")
                {
                    var myPopup = new DisPlayMessage("失敗", "輸入不可為空!", "重新輸入");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                    //await DisplayAlert("失敗", "輸入不可為空", "重新輸入");
                }
                else
                {
                    var myPopup = new DisPlayMessage("失敗", "更新失敗", "確認");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                }
            }
            else
            {
                var myPopup = new DisPlayMessage("失敗", "兩次輸入密碼不相同!", "重新輸入");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await DisplayAlert("失敗", "兩次輸入密碼不相同", "重新輸入");
            }
        }
    }
}