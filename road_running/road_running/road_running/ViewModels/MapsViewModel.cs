using road_running.Models;
using road_running.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;


namespace road_running.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        // 路線清單
        public static List<Route> InitGetList { get; set; }
        public static ObservableCollection<Route> RouteList { get; set; }
        //// 路線資訊
        //public static List<Route> InitGetInfo { get; set; }
        public static ObservableCollection<Route> RouteDetail { get; set; }

        public MapsViewModel()
        {
            LoadRouteList();
        }

        // 下拉式選單
        public string Mid;
        public async void LoadRouteList()
        {
            var AppShellInstance = Shell.Current as AppShell;
            Mid = AppShellInstance.Member_ID;
            InitGetList = await MapsProvider.GetRouteListAsync(Mid);
            if (InitGetList[0].Name == "noFile")
            {
                Text_Isvisible = true;
                Picker_Map_Isvisible = false;
            }
            else
            {
                GetRouteList = AddList();
                Text_Isvisible = false;
                Picker_Map_Isvisible = true;
            }
        }
        // Picker Binding
        public ObservableCollection<Route> GetRouteList
        {
            get { return RouteList; }
            set
            {
                RouteList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Route> AddList()
        {
            RouteList = new ObservableCollection<Route>();
            for (int i = 0; i < InitGetList.Count; i++)
            {
                RouteList.Add(new Route
                {
                    Name = InitGetList[i].Name,
                    Running_ID = InitGetList[i].Running_ID
                });
            }

            return RouteList;
        }

        private Route _selectedRoute;
        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set
            {
                if (_selectedRoute != value)
                {
                    _selectedRoute = value;
                }
            }
        }

        private bool text_isvisible;
        public bool Text_Isvisible
        {
            get { return text_isvisible; }
            set
            {
                text_isvisible = value;
                OnPropertyChanged();
            }
        }

        private bool picker_map_isvisible;
        public bool Picker_Map_Isvisible
        {
            get { return picker_map_isvisible; }
            set
            {
                picker_map_isvisible = value;
                OnPropertyChanged();
            }
        }

    }
}