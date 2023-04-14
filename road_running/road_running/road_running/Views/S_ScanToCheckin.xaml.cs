using System;
using System.Collections.Generic;
using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class S_ScanToCheckin : ContentPage
    {
        S_ScanViewModel vm = new S_ScanViewModel();
        public S_ScanToCheckin()
        {
            InitializeComponent();
            BindingContext = vm;
        }
        protected override async void OnAppearing()
        {
            vm.GetScan();
            base.OnAppearing();
        }
    }
}
