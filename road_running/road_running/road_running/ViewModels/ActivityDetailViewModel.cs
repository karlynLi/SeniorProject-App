using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;

namespace road_running.ViewModels
{
    public class ActivityDetailViewModel : INotifyPropertyChanged
    {
        public ActivityDetailViewModel(activity Info)
        {
            InitGet(Info);
            activityName = Info.Name;
            activityDate = Info.Date;
            activityLocation = Info.Location;
            activityKm = Info.GroupName;
            activityPhoto = Info.ImageUrl;
        }

        
        public async void InitGet(activity info) // 從ActivityProvider.cs取得活動清單
        {
            List<Group> initGetList = new List<Group>();
            initGetList = await GroupProvider.GetGroupsAsync(info.Id);
            AddGroupList(initGetList, info);
            AddGiftList(initGetList);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //public static List<Group> initGetList { get; set; } // 用於接收GroupProvider.cs
        public List<Group> groupList { get; set; }
        public List<gift> giftList { get; set; }

        string activityName;
        DateTime activityDate;
        string activityLocation;
        string activityKm;
        string activityPhoto;
        public string ActivityName
        {
            get { return activityName; }
        }
        public string ActivityLocation
        {
            get { return activityLocation; }
        }
        public string ActivityKm
        {
            get { return activityKm; }
        }
        public string ActivityDate
        {
            get { return activityDate.ToString("yyyy/MM/dd"); }
        }
        public string ActivityPhoto
        {
            get { return activityPhoto; }
        }
        public List<Group> GetGroup
        {
            get { return groupList; }
            set
            {
                groupList = value;
                OnPropertyChanged();
            }
        }
        public List<gift> GetGift
        {
            get { return giftList; }
            set
            {
                giftList = value;
                OnPropertyChanged();
            }
        }

        // 新增活動簡章
        public void AddGroupList(List<Group> groups, activity info)
        {
            groupList = new List<Group>();
            // 新增資料
            for (int i=0; i< groups.Count; i++)
            {
                Group group = new Group();
                group.running_ID = groups[i].running_ID;
                group.running_name = activityName;
                group.group_name = groups[i].group_name;
                group.amount = groups[i].amount;
                group.maximum_number = groups[i].maximum_number;
                group.start_time = groups[i].start_time;
                group.total_time = groups[i].total_time_formated;
                group.gift = groups[i].gift;
                if (info.Startdate <= DateTime.Now && DateTime.Now <= info.Enddate)
                {
                    group.Btn_Visible = true;
                }
                else
                {
                    group.Btn_Visible = false;
                }
                groupList.Add(group);
                //groupList.Add(new Group
                //{
                //    running_ID = groups[i].running_ID,
                //    group_name = groups[i].group_name,
                //    amount = groups[i].amount,
                //    maximum_number = groups[i].maximum_number,
                //    start_time = groups[i].start_time,
                //    total_time = groups[i].total_time_formated,
                //    gift = groups[i].gift
                //});

            }
            GiftHeight = 50 * groupList.Count;
            GetGroup = groupList;
        }
        // 新增禮品
        public void AddGiftList(List<Group> groups)
        {
            giftList = new List<gift>();
            // 新增資料
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].gift != null)
                {
                    for (int j = 0; j< groups[i].gift.Length; j = j + 2)
                    {
                        if (!giftList.Exists(t => t.Name == groups[i].gift[j]))
                        {
                            giftList.Add(new gift { Name = groups[i].gift[j] , Image = groups[i].gift[j+1]});
                            Console.WriteLine("ActivityDetailViewModel的AddGiftList: "+ groups[i].gift[j]+"/"+ groups[i].gift[j + 1]);
                        }
                    }
                }
            }
            Console.WriteLine("完成");
            GiftDetailHeight = (300 * giftList.Count)+10;
            GetGift = giftList;
        }
        double giftheight = 0;
        public double GiftHeight
        {
            get { return giftheight; }
            set
            {
                giftheight = value;
                OnPropertyChanged();
            }
        }
        double giftdetailheight = 0;
        public double GiftDetailHeight
        {
            get { return giftdetailheight; }
            set
            {
                giftdetailheight = value;
                OnPropertyChanged();
            }
        }
    }
}
