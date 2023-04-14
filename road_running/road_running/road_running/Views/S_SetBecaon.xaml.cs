using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using road_running.Models;
using road_running.Providers;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_SetBecaon : ContentPage
    {
        public S_SetBecaon()
        {
            InitializeComponent();
        }


        // 掃描pass_point
        async void pass_point_scanner(System.Object sender, System.EventArgs e)
        {
            // 建立一個掃描page
            var scan = new ZXingScannerPage();
            // 以非同步方法將page新增到堆疊頂端
            //await Navigation.PushAsync(scan);
            await Application.Current.MainPage.Navigation.PushAsync(scan);
            //App.Current.MainPage = scan;
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // 掃到後將page移除
                    await Application.Current.MainPage.Navigation.PopAsync();
                    //await Navigation.PopAsync();
                    pass_point.Text = result.Text;
                });
            };
        }

        // 掃描beacon ID
        async void beacon_scanner(System.Object sender, System.EventArgs e)
        {
            // 建立一個掃描page
            var scan = new ZXingScannerPage();
            // 以非同步方法將page新增到堆疊頂端
            //await Navigation.PushAsync(scan);
            await Application.Current.MainPage.Navigation.PushAsync(scan);
            //App.Current.MainPage = scan;
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // 掃到後將page移除
                    await Application.Current.MainPage.Navigation.PopAsync();
                    //await Navigation.PopAsync();
                    beacon.Text = result.Text;
                });
            };
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (pass_point.Text == "")
            {
                var myPopup = new DisPlayMessage("新增失敗", "經過點沒掃描", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            else if (beacon.Text == "")
            {
                var myPopup = new DisPlayMessage("新增失敗", "Beacon沒掃描", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(request);
            S_BeaconPlace beaconplace = new S_BeaconPlace();
            beaconplace.Supply_ID = pass_point.Text;
            beaconplace.Beacon_ID = beacon.Text;
            beaconplace.Latitude = location.Latitude;
            beaconplace.Longitude = location.Longitude;

            string result = await S_SetBeaconProvider.SetBeaconAsync(beaconplace);
            if (result == "yes")
            {
                var myPopup = new DisPlayMessage("新增成功", "Beacon已放置完畢", "確定");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else if (result == "noPlacement")
            {
                var myPopup = new DisPlayMessage("新增失敗", "此補給站無設置此Beacon\n請檢查經過點與Beacon是否正確", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                var myPopup = new DisPlayMessage("新增失敗", "更新失敗", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
        }
    }
}