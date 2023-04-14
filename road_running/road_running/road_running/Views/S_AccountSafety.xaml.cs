using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class S_AccountSafety : ContentPage
    {
        public Staff SaftyInfo { get; set; }
        public S_AccountSafety()
        {
            InitializeComponent();
        }
        private async void CheckPass(object sender, EventArgs e)
        {
            var AppShellInstance = Xamarin.Forms.Shell.Current as SShell;
            string id = AppShellInstance.Staff_ID;
            Staff Getstaffid = new Staff
            {
                Staff_ID = id,
                Password = Oldpass.Text
            };
            SaftyInfo = await S_SaftyProvider.S_SaftyAsync(Getstaffid);
            if (SaftyInfo.ans == "no")
            {
                var myPopup = new DisPlayMessage("驗證失敗", "重新輸入", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else if (SaftyInfo.ans == "yes")
            {
                Oldpass.Text = "";
                var myPopup = new DisPlayMessage("驗證成功", SaftyInfo.ans, "更新密碼");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                await Navigation.PushAsync(new S_UpdatePassword(id));
            }
        }
    }
}
