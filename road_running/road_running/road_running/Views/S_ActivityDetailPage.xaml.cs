using System;
using System.Collections.Generic;
using road_running.Models;
using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class S_ActivityDetailPage : ContentPage
    {
        S_ActivityDetailViewModel vm;
        public S_ActivityDetailPage(activity Info)
        {
            InitializeComponent();
            vm = new S_ActivityDetailViewModel(Info); 
            BindingContext = vm;
        }
        protected override void OnDisappearing()
        {
            vm.Close_thread();
        }
    }
}
