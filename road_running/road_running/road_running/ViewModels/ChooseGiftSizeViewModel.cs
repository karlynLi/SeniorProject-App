using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using road_running.Views;
using Xamarin.Forms;

namespace road_running.ViewModels
{
    public class ChooseGiftSizeViewModel : INotifyPropertyChanged
    {
        public ChooseGiftSizeViewModel(Group group)
        {
            groupinfo = group;
            InitGet(group.running_ID, group.group_name);
            GotoAffidavitPageCommand = new Command(GotoAffidavitPage);
        }

        Group groupinfo;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void InitGet(string rid, string gname) // 從ActivityProvider.cs取得活動清單
        {
            //List<Size> initGetList = new List<Size>();
            SizeList = await GiftSizeProvider.GetSizeAsync(rid, gname);
            //List<GiftSize>  SizeList2 = new List<GiftSize>();
            //SizeList2.Add(new GiftSize() { id = "1", name = "衣服", sizeList = new string[] {"S", "M", "L"} });
            //SizeList2.Add(new GiftSize() { id = "2", name = "褲子", sizeList = new string[] { "1", "2", "3"} });
            //SizeList2.Add(new GiftSize() { id = "3", name = "帽子", sizeList = new string[] { "F"} });
            //SizeList = SizeList2;
        }

        public Command GotoAffidavitPageCommand { get; }

        public List<GiftSize> sizelist;
        public List<GiftSize> SizeList
        {
            get { return sizelist;}
            set
            {
                sizelist = value;
                OnPropertyChanged();
            }
        }

        private async void GotoAffidavitPage()
        {
            List<string> unselectedItem = new List<string>();
            for (int i=0; i<sizelist.Count; i++)
            {
                if (sizelist[i].SelectedSize == null)
                    unselectedItem.Add(sizelist[i].name);
                //Console.WriteLine(sizelist[i].SelectedSize);
            }
            if (unselectedItem.Count != 0)
            {
                string str = "";
                //string str2 = "";
                for (int i = 0; i < unselectedItem.Count; i++)
                    str += unselectedItem[i] + "、";
                string str2 = str.Substring(0, str.Length - 1) + "尚未選擇尺寸";
                var myPopup = new DisPlayMessage("請選擇尺寸！", str2, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("請選擇尺寸！", str2));
            }
            else
            {
                groupinfo.giftSize = sizelist;
                await Application.Current.MainPage.Navigation.PushAsync(new AffidavitPage(groupinfo));
            }
        }
    }
}
