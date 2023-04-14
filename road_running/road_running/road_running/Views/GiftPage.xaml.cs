using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using road_running.Models;
using road_running.ViewModels;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms.Xaml;
using road_running.Providers;

using System.Windows;
namespace road_running.Views
{
    public partial class GiftPage : ContentPage
    {
        public IList<CheckIn> CheckGift { get; private set; }
        private CheckViewModel GiftList = new CheckViewModel();
        public GiftPage()
        {
            InitializeComponent();
            BindingContext = GiftList;
        }

        private async void Choose_act(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
            if(e.Item != null)
            {
                CheckIn checkdata = e.Item as CheckIn;
                string data = checkdata.Registration_ID;
                Console.WriteLine(checkdata.Registration_ID);
                //Generate_QR(data);
                Title = "兌換禮物";
                await Navigation.PushAsync(new ShowQrPage(data,Title));
                //await DisplayAlert(checkdata.Name, "test", "OK");
                
            }
        }

        private async void Redeem(object sender, EventArgs e)
        {
            await DisplayAlert("兌換禮物Qr code", "You have been alerted", "OK");
        }
        protected override async void OnAppearing()
        {
            GiftList.Getgift();
            base.OnAppearing();
        }
    }
}