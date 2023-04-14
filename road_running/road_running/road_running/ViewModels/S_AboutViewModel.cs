using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace road_running.ViewModels
{
    public class S_AboutViewModel : BaseViewModel
    {
        public S_AboutViewModel()
        {
            Title = "修改個人資料";

            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }
    }
}