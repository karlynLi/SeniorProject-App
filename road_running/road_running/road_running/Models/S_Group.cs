using System;
using System.Windows.Input;
using road_running.Views;
using Xamarin.Forms;
using road_running.Providers;
using Rg.Plugins.Popup.Services;

namespace road_running.Models
{
    public class S_Group
    {
        public S_Group()
        {
            SignUpCommand = new Command(SignUpAsStaff);
        }
        public Command SignUpCommand { get; set; }

        // 按我要報名前往切結書頁面
        public async void SignUpAsStaff()
        {
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            string ifsuccess = await S_SignUpActivityProvider.UpdateS_SignUpAsync(SShellInstance.Staff_ID, this);
            if (ifsuccess == "yes")
            {
                var myPopup = new DisPlayMessage("報名成功", "成功報名此活動", "確定");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                // await PopupNavigation.PushAsync(new DisPlayMessage("報名成功", "成功報名此活動"));
            }
            else if (ifsuccess == "timeLimit")
            {
                var myPopup = new DisPlayMessage("報名失敗", "活動不在報名期限內", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else if(ifsuccess == "no")
            {
                var myPopup = new DisPlayMessage("報名失敗", "", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                // await PopupNavigation.PushAsync(new DisPlayMessage("報名失敗", ""));
            }
            else if (ifsuccess == "error")
            {
                var myPopup = new DisPlayMessage("報名失敗", "請檢察網路狀態", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("報名失敗", "請檢察網路狀態"));
            }
            else if (ifsuccess == "ToTheMaximumNumber")
            {
                var myPopup = new DisPlayMessage("報名失敗", "此組別已達人數上限", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("報名失敗", "此組別已達人數上限"));
            }
            else
            {
                var myPopup = new DisPlayMessage("報名失敗", $"你已報名過此活動的{ifsuccess}", "返回主頁");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("報名失敗", "你已報名過此活動的" + ifsuccess));
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
        }

        public string workgroup_ID { get; set; } // 工作組別代碼
        public string running_ID { get; set; }
        public string name { get; set; } // 工作組別名稱
        public DateTime assemble_time { get; set; } // 集合時間
        public DateTime end_time { get; set; } // 結束時間
        public string assemble_place { get; set; } // 集合地點
        public string maximum_number { get; set; } // 人數上限

        public string time
        {
            get
            {
                return assemble_time.ToString("HH:mm") + " - " + end_time.ToString("HH:mm");
            }
        }
    //public string start_time_formated
    //{
    //    get { return start_time.ToString("HH:mm"); }
    //}
    }
}
