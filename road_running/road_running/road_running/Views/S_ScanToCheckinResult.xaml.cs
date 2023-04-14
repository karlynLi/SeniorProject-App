using System;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace road_running.Views
{
    public partial class S_ScanToCheckinResult : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        public S_ScanToCheckinResult(S_RegistrationScanner Info, string work_id)
        {
            InitializeComponent();
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            rid = SShellInstance.Staff_ID;
            wid = work_id;
            DisPlay(Info);
        }
        public string rid;
        public string wid;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void DisPlay(S_RegistrationScanner Info)
        {
            if (Info.status == "NotCheckinYet") // 有此訂單但未報到過
            {
                R_id.Text = rid;
                Status.Text = "尚未報到";
                Group_Name.Text = Info.group_name;
                CheckInBtn.IsVisible = true;
            }
            else // 已經報到過
            {
                R_id.Text = rid;
                Status.Text = Info.checkin_time + "\n已完成報到";
                Group_Name.Text = Info.group_name;
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
            bool ifsuccess = await S_CheckinAndExchangeProvider.S_UpdateInfoAsync(rid, wid, "http://running.im.ncnu.edu.tw/run_api/staffArrived.php");
            if (ifsuccess)
            {
                var myPopup = new DisPlayMessage("報到", "報到成功", "確定");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("報到", "報到成功"));
            }
            else
            {
                var myPopup = new DisPlayMessage("報到", "報到失敗", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("報到", "報到失敗"));
            }
        }
    }
}
