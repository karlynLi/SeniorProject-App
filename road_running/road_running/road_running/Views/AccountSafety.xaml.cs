using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class AccountSafety : ContentPage
    {
        public Member SaftyInfo { get; set; }
        public AccountSafety()
        {
            InitializeComponent();
        }
        private async void CheckPass(object sender, EventArgs e)
        {
            var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
            string id = AppShellInstance.Member_ID;
            Member Getmemberid = new Member
            {
                Member_ID = id,
                Password = OldPass.Text
            };
            SaftyInfo = await SaftyProvider.SaftyAsync(Getmemberid);
            if(SaftyInfo.ans == "no")
            {
                var myPopup = new DisPlayMessage("驗證失敗", "重新輸入", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證成功", "重新輸入"));
                //await DisplayAlert("失敗", "輸入密碼與舊密碼不符", "重新輸入");
            }
            else if(SaftyInfo.ans == "yes")
            {
                OldPass.Text = "";
                var myPopup = new DisPlayMessage("驗證成功", SaftyInfo.ans, "更新密碼");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證成功", "gogo"));
                await Navigation.PushAsync(new UpdatePassword(id));
            }
        }
    }
}
