using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using road_running.Models;
using road_running.Providers;

namespace road_running.ViewModels
{
    public class S_ActivityDetailViewModel : INotifyPropertyChanged
    {
        public S_ActivityDetailViewModel(activity Info)
        {
            initGet(Info);
            activityName = Info.Name;
            activityDate = Info.Date;
            activityLocation = Info.Location;
            activityKm = Info.GroupName;
            activityPhoto = Info.ImageUrl;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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
        public DateTime ActivityDate
        {
            get { return activityDate; }
        }
        public string ActivityPhoto
        {
            get { return activityPhoto; }
        }

        public List<S_Group> s_GroupList = new List<S_Group>();

        public List<S_Group> GetS_Group
        {
            get { return s_GroupList; }
            set
            {
                s_GroupList = value;
                OnPropertyChanged();
            }
        }
        Thread Get_Thread;  // 創建thread
        private void initGet(activity Info)
        {
            Get_Thread = new Thread(async () =>
            {
                GetS_Group = await S_GroupProvider.GetS_GroupsAsync(Info.Id);
            });
            Get_Thread.Start();
            //GetS_Group = await S_GroupProvider.GetS_GroupsAsync(Info.Id);
        }
        public void Close_thread()
        {
            Get_Thread.Abort();
        }
    }
}
