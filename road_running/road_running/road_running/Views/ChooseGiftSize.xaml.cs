using System;
using System.Collections.Generic;
using road_running.Models;
using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class ChooseGiftSize : ContentPage
    {
        public ChooseGiftSize(Group group)
        {
            InitializeComponent();
            ChooseGiftSizeViewModel vm = new ChooseGiftSizeViewModel(group);
            BindingContext = vm;
        }
    }
}
