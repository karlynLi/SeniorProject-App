using System;
using Xamarin.Forms;
using road_running.ViewModels;
using road_running.Models;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;

namespace road_running.Views
{
    public partial class S_WorkContent : Rg.Plugins.Popup.Pages.PopupPage
    {
        public List<S_WorkContent> Contents { get; set; }

        public S_WorkContent(string run_id, string workgroup_id)
        {
            InitializeComponent();
            S_WorkContentViewModel ContentList = new S_WorkContentViewModel(run_id, workgroup_id);
            BindingContext = ContentList;
        }

        private async void CloseBtn_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}