using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.ViewModels;
using road_running.Models;
namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckPage : ContentPage
    {
        public IList<CheckIn> CheckGift { get; private set; }
        private CheckViewModel GiftList = new CheckViewModel();
        public CheckPage()
        {
            InitializeComponent();
            BindingContext = GiftList;
        }
        private async void Choose_Act(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
            if (e.Item != null)
            {
                CheckIn checkdata = e.Item as CheckIn;
                string data = checkdata.Registration_ID;
                Console.WriteLine(checkdata.Registration_ID);
                //Generate_QR(data);
                Title = "報到";
                await Navigation.PushAsync(new ShowQrPage(data,Title));
                //await DisplayAlert(checkdata.Name, "test", "OK");

            }
        }
        protected override async void OnAppearing()
        {
            GiftList.Getgift();
            base.OnAppearing();
        }

    }
}