using road_running.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            //this.BindingContext = new LoginViewModel();
        }
        private async void MLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MLoginPage());
        }
        private async void SLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new S_LoginPage());
        }
    }
}