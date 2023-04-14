using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using road_running.ViewModels;
using road_running.Models;
using System.Collections.Generic;
using road_running.Providers;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace road_running.Views
{
    public partial class SupplyLocationPage : ContentPage
    {
        public string detail;
        public List<Route> GetRouteList { get; set; }
        private SupplyLocationViewModel Supply = new SupplyLocationViewModel();
        public SupplyLocationPage()
        {
            InitializeComponent();
            BindingContext = Supply;
            Get_Position();
        }

        private async void Get_Position()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var m_location = await Geolocation.GetLocationAsync(request);
            Position init_pos = new Position(m_location.Latitude, m_location.Longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(init_pos, Distance.FromMeters(100)));
        }
        public string _previousChoose;
        public async void Change_select(object sender, EventArgs e)
        {
            map.Pins.Clear();
            Route choose = (Route)pickAct.SelectedItem;
            Route route = new Route()
            {
                Running_ID = choose.Running_ID
            };
            GetRouteList = await SupplyLocationProvider.SupplyListAsync(route);
            _previousChoose = choose.Running_ID;
            detail = GetRouteList[0].Supplies;
            SetPin();
        }
        public async void SetPin()
        {
            //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            //var m_location = await Geolocation.GetLocationAsync(request);
            //Position init_pos = new Position(m_location.Latitude, m_location.Longitude);
            //map.MoveToRegion(MapSpan.FromCenterAndRadius(init_pos, Distance.FromMeters(100)));

            if (GetRouteList != null)
            {
                for (int i = 0; i < GetRouteList.Count; i++)
                {
                    Pin pinSupplyStation = new Pin()
                    {
                        Type = PinType.Place,
                        Label = GetRouteList[i].Supplies,
                        Position = new Position(GetRouteList[i].Latitude, GetRouteList[i].Longitude),
                    };
                    map.Pins.Add(pinSupplyStation);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(pinSupplyStation.Position, Distance.FromMeters(500)));

                }
            }
            else if (GetRouteList == null) { 
                map.Pins.Clear();
                var myPopup = new DisPlayMessage("設置補給站", "無補給站", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.PushAsync(new DisPlayMessage("設置補給站", "無補給站"));
            }
        }
        protected override async void OnAppearing()
        {
            if (_previousChoose != null)
            {
                Route chooseRoute = new Route()
                {
                    Running_ID = _previousChoose
                };
                GetRouteList = await SupplyLocationProvider.SupplyListAsync(chooseRoute);
            }
            base.OnAppearing();
        }
    }
}