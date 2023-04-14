using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using road_running.Models;
using Xamarin.Forms;
using road_running.Providers;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using road_running.Views;

namespace road_running.ViewModels
{
    public class ConfirmViewModel : INotifyPropertyChanged
    {
        public ConfirmViewModel(Group group)
        {
            var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
            User_ID = AppShellInstance.Member_ID;
            User_Name = AppShellInstance.Name;
            User_Identity = AppShellInstance.Id_card;
            Console.WriteLine("cccccccccc" + AppShellInstance.Id_card);
            Group_Name = group.group_name;
            Cost = "$" + group.amount;
            GiftList = group.giftSize;
            GiftListHeight = 58 * group.giftSize.Count;
            //AddGiftList(group);
            UpdateCommand = new Command(async () => await UpdateToDataBaseAsync(group));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Command UpdateCommand { get; }

        public async Task UpdateToDataBaseAsync(Group group)
        {
            string ifsuccess = await RegistrationProvider.UpdateRegistrarionAsync(User_ID, group);
            Console.WriteLine(ifsuccess);
            if (ifsuccess == "yes")
            {
                var myPopup = new DisPlayMessage("報名成功", "成功報名此活動", "返回首頁");
                MessagingCenter.Send<string>(group.running_ID, "Subscribe_FCM_Topic"); // 訂閱主題
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask; // 等待PopUp頁面回傳 （等待回傳才會繼續往下做）
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else if (ifsuccess == "error"){
                var myPopup = new DisPlayMessage("報名失敗", "請檢察網路狀態", "確定");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                var myPopup = new DisPlayMessage("報名失敗", $"你已報名過此活動的{ifsuccess}", "返回首頁");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
        }

        //public void AddGiftList(Group group)
        //{
        //    giftList = new List<GiftSize>();
        //    if (group.giftSize != null)
        //    {
        //        for (int i=0; i<group.giftSize.Count; i++)
        //        {
        //            giftList.Add(new GiftSize { Name = group.gift[i] });
        //        }
        //        GiftListHeight = 29 * giftList.Count;
        //        GiftList = giftList;
        //    }
        //}

        public List<GiftSize> giftList { get; set; }
        public List<GiftSize> GiftList
        {
            get { return giftList; }
            set
            {
                giftList = value;
                OnPropertyChanged();
            }
        }

        double giftlistheight;
        public double GiftListHeight
        {
            get { return giftlistheight; }
            set
            {
                giftlistheight = value;
                OnPropertyChanged();
            }
        }

        string user_id;
        public string User_ID
        {
            get { return user_id; }
            set
            {
                user_id = value;
                OnPropertyChanged();
            }
        }

        string user_name;
        public string User_Name
        {
            get { return user_name; }
            set
            {
                user_name = value;
                OnPropertyChanged();
            }
        }

        string user_identity;
        public string User_Identity
        {
            get { return user_identity; }
            set
            {
                user_identity = value;
                OnPropertyChanged();
            }
        }

        string group_name;
        public string Group_Name
        {
            get { return group_name; }
            set
            {
                group_name = value;
                OnPropertyChanged();
            }
        }

        string cost;
        public string Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged();
            }
        }
    }
}
