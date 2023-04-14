using road_running.Models;
using road_running.Providers;
using road_running.ViewModels;
using road_running.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class ActivityDetailPage : ContentPage
    {
        public IList<gift> giftList { get; private set; }
        public ActivityDetailPage(activity Info)
        {
            InitializeComponent();
            ActivityDetailViewModel vm = new ActivityDetailViewModel(Info);
            BindingContext = vm;
        }
    }
}