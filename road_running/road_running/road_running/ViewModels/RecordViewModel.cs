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
    public class RecordViewModel : BaseViewModel
    {
        public ObservableCollection<Record> Records { get; set; }
        public static List<Record> InitGetList { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public RecordViewModel(int pagenum)
        {
            LoadRecord(pagenum);
            RefreshCommand = new AsyncCommand(Refresh);
        }

        public async void LoadRecord(int pagenum)
        {
            var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
            string id = AppShellInstance.Member_ID;
            InitGetList = await RecordProvider.GetRecordAsync(id);
            GetRecordList = AddList(pagenum);
        }
        public ObservableCollection<Record> GetRecordList
        {
            get { return Records; }
            set
            {
                Records = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Record> AddList(int pagenum)
        {
            Records = new ObservableCollection<Record>();
            
            for (int i = 0; i < InitGetList.Count; i++)
            {
                if (pagenum == InitGetList[i].Status)
                {
                    Records.Add(new Record
                    {
                        Name = InitGetList[i].Name,
                        GetDate = InitGetList[i].Date.ToString("yyyy/MM/dd"),
                        Registration_ID = InitGetList[i].Registration_ID,
                        Running_ID = InitGetList[i].Running_ID
                    });
                    Console.WriteLine("=======  RecordViewModel.AddList(" + pagenum + ")  =========");
                    Console.WriteLine(InitGetList[i].Status);
                    Console.WriteLine(InitGetList[i].Name);
                    Console.WriteLine(InitGetList[i].Date.ToString("yyyy/MM/dd"));
                    Console.WriteLine(InitGetList[i].Registration_ID);
                    Console.WriteLine(InitGetList[i].Running_ID);
                }
            }
            return Records;
        }

        private Record _selectedRecord;
        public Record SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                if (value != null)
                {
                    Console.WriteLine("runid = " + value.Running_ID + " regisid = " + value.Registration_ID);
                    Application.Current.MainPage.Navigation.PushAsync(new RecordDetailPage(value.Running_ID, value.Registration_ID));
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