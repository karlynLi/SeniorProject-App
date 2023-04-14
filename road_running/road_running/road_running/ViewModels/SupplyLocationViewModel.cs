using road_running.Models;
using road_running.Providers;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace road_running.ViewModels
{
    public class SupplyLocationViewModel : BaseViewModel
    {
        public static List<Route> RouteList { get; set; } // 存路線清單
        public static List<Route> InitGetList { get; set; } // MapsProvider.cs
        public Route supply { get; set; }
        public SupplyLocationViewModel()
        {
            LoadRouteList();

        }
        // 下拉式選單
        public async void LoadRouteList()
        {
            var AppShellInstance = Shell.Current as AppShell;
            string mid = AppShellInstance.Member_ID;
            Member memberid = new Member()
            {
                Member_ID = mid
            };
            InitGetList = await SupplyLocationProvider.GetRouteListAsync(memberid);
            if (InitGetList[0].Name == "noFile")
            {
                Text_Isvisible = true;
                Picker_Map_Isvisible = false;
            }
            else{
                GetRouteList = AddRoute();
                Text_Isvisible = false;
                Picker_Map_Isvisible = true;
            }
            Console.WriteLine("Add Success");

        }
        public List<Route> AddRoute()
        {
            RouteList = new List<Route>();
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
        // Picker Binding
        public List<Route> GetRouteList
        {
            get { return RouteList; }
            set
            {
                RouteList = value;
                OnPropertyChanged();
            }
        }
        Route _selectedRoute;
        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set
            {
                if (_selectedRoute != value)
                {
                    _selectedRoute = value;
                    OnPropertyChanged();
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