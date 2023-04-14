using System;
using System.Collections.Generic;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using road_running.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace road_running.Models
{
    public class Group
    {
        public Group()
        {
            SignUpCommand = new Command(() => GoToAffidavitPage());
        }
        public ICommand SignUpCommand { get; set; }

        public Group GetGroup()
        {
            Group groups = new Group();
            groups.running_ID = this.running_ID;
            groups.running_name = this.running_name;
            groups.group_name = this.group_name;
            groups.amount = this.amount;
            groups.maximum_number = this.maximum_number;
            groups.start_time = this.start_time;
            groups.total_time = this.total_time;
            groups.gift = this.gift;
            groups.Btn_Visible = this.Btn_Visible;
            return groups;
        }


        // 按我要報名前往切結書頁面
        public async void GoToAffidavitPage()
        {
            string email = Preferences.Get("mail", "");
            var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
            if (email == "" || AppShellInstance == null)
            {
                var myPopup = new DisPlayMessage("報名失敗", "需要先登入才能報名活動!", "登入");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                // 前往登入畫面
                await Application.Current.MainPage.Navigation.PushAsync(new MLoginPage());
                //await PopupNavigation.PushAsync(new DisPlayMessage("報名失敗", "需要先登入才能報名活動!"));
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ChooseGiftSize(GetGroup()));
            }
        }

        // 將total_time轉換成?小時?分鐘的字串 並回傳
        public string Trans_total_time()
        {
            TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(total_time));
            string str = time.ToString("%h") + "小時" + time.ToString("%m") + "分鐘";
            return str;
        }
        public string running_ID { get; set; } // 路跑ID
        public string running_name { get; set; } // 路跑名稱
        public string group_name { get; set; } // 組別名稱
        public string amount { get; set; } // 花費
        public string maximum_number { get; set; } // 人數上限
        public DateTime start_time { get; set; } // 起跑時間
        public string total_time { get; set; } // 限制時間
        public string[] gift { get; set; } // 禮物名稱及禮物圖片檔案名稱
        public bool Btn_Visible { get; set; } // 我要報名是否顯示
        public List<GiftSize> giftSize { get; set; }


        public string giftToString()
        {
            string str = "無禮品";
            if (gift.Length > 0)
            {
                str = "";
                for (int i = 0; i < gift.Length; i = i+2)
                {
                    if (gift[i] == null)
                    {
                        str += "無資料";
                    }
                    else
                    {
                        str += gift[i] + " / ";
                    }
                }
                str = str.Substring(0, str.Length - 3);
            }
            return str;
        }

        public string gift_string
        {
            get
            {
                if (gift != null)
                    return giftToString();
                else
                    return "無禮品";
            }
        }

        public string total_time_formated // 取Trans_total_time的字串
        {
            get { return Trans_total_time(); }
        }
        public string start_time_formated
        {
            get { return start_time.ToString("HH:mm"); }
        }
    }
}
