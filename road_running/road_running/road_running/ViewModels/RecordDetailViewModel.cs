using road_running.Models;
using road_running.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace road_running.ViewModels
{
    public class RecordDetailViewModel : INotifyPropertyChanged
    {
        public RecordDetailViewModel(string run_id, string regis_id)
        {
            LoadDetail(run_id, regis_id);
        }
        public string name, group_name, place, grade, complete_time;
        public DateTime time;
        public ObservableCollection<RecordDetail> Details { get; set; }
        public static List<RecordDetail> InitGetList { get; set; }
        public async void LoadDetail(string run_id, string regis_id)
        {
            InitGetList = await RecordDetailProvider.GetRecordDetailAsync(run_id, regis_id);
            Details = new ObservableCollection<RecordDetail>();
            for (int i = 0; i< InitGetList.Count; i++)
            {
                Name = InitGetList[i].Name;
                Group_name = InitGetList[i].Group_name;
                Time = InitGetList[i].Time;
                Place = InitGetList[i].Place;
                //GetGrade = InitGetList[i].GetGrade;
                //GetCompleteTime = InitGetList[i].GetCompleteTime;
                Grade = InitGetList[i].Grade;
                Complete_time = InitGetList[i].Complete_time;

            }
            Console.WriteLine("============ RecordDetailViewModel ============");
            Console.WriteLine("name = " + name);
            Console.WriteLine("group_name = " + group_name);
            Console.WriteLine("time = " + time);
            Console.WriteLine("place = " + place);
        }
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged();
            }
        }
        public string Group_name
        {
            get { return group_name; }
            set 
            { 
                group_name = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time
        {
            get { return time; }
            set 
            { 
                time = value; 
                OnPropertyChanged();
            }
        }
        public string Place
        {
            get { return place; }
            set 
            { 
                place = value;
                OnPropertyChanged();
            }
        }
        public string Grade
        {
            get { return grade; }
            set
            {
                grade = value;
                OnPropertyChanged();
            }
        }
        public string Complete_time
        {
            get { return complete_time; }
            set
            {
                complete_time = value;
                OnPropertyChanged();
            }
        }
        //public string GetGrade
        //{
        //    get { return grade; }
        //    set 
        //    { 
        //        grade = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public string GetCompleteTime
        //{
        //    get { return complete_time; }
        //    set
        //    {
        //        complete_time = value;
        //        OnPropertyChanged();
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}