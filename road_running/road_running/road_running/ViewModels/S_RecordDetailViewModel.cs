using road_running.Models;
using road_running.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace road_running.ViewModels
{
    public class S_RecordDetailViewModel : BaseViewModel
    {
        public S_RecordDetailViewModel(string run_id, string workgroup_id)
        {
            LoadDetail(run_id, workgroup_id);
        }
        public string name, work_name, assemble_place, leader, line;
        public DateTime assemble_time;
        public ObservableCollection<S_RecordDetail> Details { get; set; }
        public static List<S_RecordDetail> InitGetList { get; set; }
        public async void LoadDetail(string run_id, string workgroup_id)
        {
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            string sid = SShellInstance.Staff_ID;
            InitGetList = await S_RecordDetailProvider.GetRecordDetailAsync(sid, run_id, workgroup_id);
            Details = new ObservableCollection<S_RecordDetail>();
            for (int i = 0; i < InitGetList.Count; i++)
            {
                Name = InitGetList[i].Name;
                Work_name = InitGetList[i].Work_name;
                Assembletime = InitGetList[i].Assembletime;
                Assembleplace = InitGetList[i].Assembleplace;
                Leader = InitGetList[i].Leader;
                Line = InitGetList[i].Line;
            }
            Console.WriteLine("============ S_RecordDetailViewModel ============");
            Console.WriteLine("name = " + name);
            Console.WriteLine("work_name = " + work_name);
            Console.WriteLine("assembleplace = " + assemble_place);
            Console.WriteLine("assembletime = " + assemble_time);
            Console.WriteLine("leader = " + leader);
            Console.WriteLine("line = " + line);
        }

        public string Name
        {
            get { return name; }
            set { name = value;
                  OnPropertyChanged();
            }
        }
        public string Work_name
        {
            get { return work_name; }
            set { work_name = value;
                  OnPropertyChanged();
            }
        }

        public DateTime Assembletime
        {
            get { return assemble_time; }
            set { assemble_time = value;
                  OnPropertyChanged();
            }
        }
        public string Assembleplace
        {
            get { return assemble_place; }
            set { assemble_place = value;
                OnPropertyChanged();
            }
        }
        public string Leader
        {
            get { return leader; }
            set { leader = value;
                  OnPropertyChanged();
            }
        }
        public string Line
        {
            get { return line; }
            set { line = value;
                  OnPropertyChanged();
            }
        }
    }
}