using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Text;
using road_running.Models;
using System.Net.Http;
using Newtonsoft.Json;
namespace road_running.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "修改個人資料";
            
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }


        //public ICommand OpenWebCommand { get; }
    }
}