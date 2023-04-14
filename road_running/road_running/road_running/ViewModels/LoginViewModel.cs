using road_running.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command SLoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SLoginCommand = new Command(SOnLoginClicked);
        }

        private void OnLoginClicked(object obj)
        {
            //await  (new MLoginPage());
            Application.Current.MainPage = new MLoginPage();
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(MLoginPage)}");
        }
        private void SOnLoginClicked(object obj)
        {
            Application.Current.MainPage = new S_LoginPage();
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
