using System;
using System.Collections.Generic;
using road_running.Models;
using Xamarin.Forms;
using road_running.ViewModels;

namespace road_running.Views
{
    public partial class ConfirmPage : ContentPage
    {
        public ConfirmPage(Group group)
        {
            InitializeComponent();
            ConfirmViewModel vm = new ConfirmViewModel(group);
            BindingContext = vm;
        }
    }
}
