using MvvmHelpers.Commands;
using road_running.Models;
using road_running.Providers;
using road_running.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace road_running.ViewModels
{
    public class S_RecordViewModel : BaseViewModel
    {
        public ObservableCollection<S_Record> Records { get; set; }
        public static List<S_Record> InitGetList { get; set; }
        public AsyncCommand RefreshCommand { get; }

        public S_RecordViewModel(int pagenum)
        {
            LoadRecord(pagenum);
            RefreshCommand = new AsyncCommand(Refresh);
        }

        public async void LoadRecord(int pagenum)
        {
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            string id = SShellInstance.Staff_ID;
            InitGetList = await S_RecordProvider.GetRecordAsync(id);
            GetRecordList = AddList(pagenum);
        }
        public ObservableCollection<S_Record> GetRecordList
        {
            get { return Records; }
            set
            {
                Records = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<S_Record> AddList(int pagenum)
        {
            Records = new ObservableCollection<S_Record>();

            for (int i = 0; i < InitGetList.Count; i++)
            {
                if (pagenum == InitGetList[i].Status)
                {
                    Records.Add(new S_Record
                    {
                        Name = InitGetList[i].Name,
                        GetDate = InitGetList[i].Date.ToString("yyyy/MM/dd"),
                        Workgroup_ID = InitGetList[i].Workgroup_ID,
                        Running_ID = InitGetList[i].Running_ID
                    });
                    Console.WriteLine("=======  S_RecordViewModel.AddList("+pagenum+") =========");
                    Console.WriteLine(InitGetList[i].Status);
                    Console.WriteLine(InitGetList[i].Name);
                    Console.WriteLine(InitGetList[i].Date.ToString("yyyy/MM/dd"));
                    Console.WriteLine(InitGetList[i].Workgroup_ID);
                    Console.WriteLine(InitGetList[i].Running_ID);
                }
            }
            return Records;
        }

        private S_Record _previousSelected;
        private S_Record _selectedRecord;
        public S_Record SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                if (value != null)
                {
                    Console.WriteLine("runid = " + value.Running_ID + " workid = " + value.Workgroup_ID);
                    Application.Current.MainPage.Navigation.PushAsync(new S_RecordDetailPage(value.Running_ID, value.Workgroup_ID));
                    _previousSelected = value;
                    value = null;
                }
                _selectedRecord = value;
                OnPropertyChanged();
            }
        }
        public async Task Refresh()
        {
            IsBusy = true;
            await Task.Delay(2000);
            IsBusy = false;
        }
    }
}