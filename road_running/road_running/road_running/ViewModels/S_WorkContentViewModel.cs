using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using road_running.Models;
using road_running.Providers;

namespace road_running.ViewModels
{
    public class S_WorkContentViewModel : BaseViewModel
    {
        public S_WorkContentViewModel(string run_id, string workgroup_id)
        {
            LoadContent(run_id, workgroup_id);
        }

        public string content, place;
        public DateTime time;
        public ObservableCollection<S_WorkContent> Contents { get; set; }
        public static List<S_WorkContent> InitGetList { get; set; }
        public async void LoadContent(string run_id, string workgroup_id)
        {
            InitGetList = await S_WorkContentProvider.GetWorkContentAsync(run_id, workgroup_id);
            ReturnList = AddContent();
        }
        public ObservableCollection<S_WorkContent> ReturnList
        {
            get { return Contents; }
            set
            {
                Contents = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<S_WorkContent> AddContent()
        {
            Contents = new ObservableCollection<S_WorkContent>();
            for (int i = 0; i < InitGetList.Count; i++)
            {
                Contents.Add(new S_WorkContent
                {
                    Content = InitGetList[i].Content,
                    GetTime = InitGetList[i].Time.ToString("HH:mm"),
                    Place = InitGetList[i].Place
                });
                Console.WriteLine("============ S_WorkContentViewModel ============");
                Console.WriteLine("content = " + InitGetList[i].Content);
                Console.WriteLine("time = " + InitGetList[i].Time);
                Console.WriteLine("place = " + InitGetList[i].Place);
                Console.WriteLine("Contents : " + Contents);
            }
            return Contents;
        }
    }
}
