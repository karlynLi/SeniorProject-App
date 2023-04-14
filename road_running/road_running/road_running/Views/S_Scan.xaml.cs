using System;
using System.Collections.Generic;
using Xamarin.Forms;
using road_running.ViewModels;
using Rg.Plugins.Popup.Services;

namespace road_running.Views
{
    public partial class S_Scan : ContentPage
    {
        public S_Scan()
        {
            InitializeComponent();
            BindingContext = new S_ScanViewModel();
        }
    }
}
