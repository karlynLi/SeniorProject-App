using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using road_running.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace road_running.Views
{
    public partial class MapsPage : ContentPage
    {
        MapsViewModel mapMVVM = new MapsViewModel();
        
        public static List<Route> InitGetInfo { get; set; }
        //public static ObservableCollection<Route> RouteDetail { get; set; }
        public MapsPage()
        {
            InitializeComponent();
            BindingContext = mapMVVM;
            GetPosition();
        }
        public string member_id, _previousChoose;
        public async void Index_Changed(object sender, EventArgs e)
        {
            var AppShellInstance = Shell.Current as AppShell;
            Route choose = (Route)picker.SelectedItem;
            InitGetInfo = await MapsProvider.GetRouteInfoAsync(AppShellInstance.Member_ID, choose.Running_ID);
            _previousChoose = choose.Running_ID;
            member_id = AppShellInstance.Member_ID;
            map.MapElements.Clear();
            LoadRoute();
        }
        public async void LoadRoute()
        {
            Polyline polyline = new Polyline()
            {
                StrokeColor = Color.Orange,
                StrokeWidth = 12
            };
            
            for (int i = 0; i < InitGetInfo.Count; i++)
            {
                polyline.Geopath.Add(new Position(InitGetInfo[i].Latitude, InitGetInfo[i].Longitude));
            }
            map.MapElements.Add(polyline);
        }

        public async void GetPosition()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var m_location = await Geolocation.GetLocationAsync(request);
            Position init_pos = new Position(m_location.Latitude, m_location.Longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(init_pos, Distance.FromMeters(200)));

            while (GPS.IfCheckin)
            {
                Location location = GPS.location;
                if (location != null)
                {
                    Console.WriteLine("{{{{{{{{{{{{{ location {{{{{{{{{{ "+location);
                    Position p = new Position(location.Latitude, location.Longitude);

                    // MapSpan 指定要在地圖上顯示的位置
                    // FromCenterRadius(Position, Distance) 設定顯示範圍的中心點及半徑 
                    // MoveToRegion 使地圖隨者使用者移動 
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromMeters(100));
                    map.MoveToRegion(mapSpan);


                }
                await Task.Delay(1000);
            }
        }

        protected override async void OnAppearing()
        {
            if (_previousChoose != null)
            {
                InitGetInfo = await MapsProvider.GetRouteInfoAsync(member_id, _previousChoose);
            }
            base.OnAppearing();
        }
    }
}