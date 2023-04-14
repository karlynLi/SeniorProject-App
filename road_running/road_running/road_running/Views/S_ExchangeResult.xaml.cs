using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class S_ExchangeResult : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        public S_ExchangeResult(S_GiftScanner Info, string SignUpText)
        {
            InitializeComponent();
            rid = SignUpText;
            DisPlay(Info);
        }
        public string rid;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void DisPlay(S_GiftScanner Info)
        {
            if (Info.status == "NotRedeemedYet") // 有此訂單但未兌換過
            {
                R_id.Text = rid;
                Status.Text = "尚未兌換";
                Gift_Content.Text = Info.GiftStr();
                CheckInBtn.IsVisible = true;
            }
            else // 已經兌換過
            {
                R_id.Text = rid;
                Status.Text = Info.exchange_time + "\n已完成兌換";
                Gift_Content.Text = Info.GiftStr();
                CheckInBtn.IsVisible = false;
            }
        }
        private async void CloseBtn_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void CheckInBtn_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            bool ifsuccess = await S_CheckinAndExchangeProvider.UpdateInfoAsync(SShellInstance.Staff_ID, rid, "http://running.im.ncnu.edu.tw/run_api/scan_gift.php");
            if (ifsuccess)
            {
                var myPopup = new DisPlayMessage("禮品兌換", "兌換成功", "確定");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("禮品兌換", "兌換成功"));
            }
            else
            {
                var myPopup = new DisPlayMessage("禮品兌換", "兌換失敗", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("禮品兌換", "兌換失敗"));
            }
        }
    }
}
