using System;
using System.Collections.Generic;
using road_running.ViewModels;
using road_running.Views;
using Xamarin.Forms;
using road_running.Models;
using Xamarin.Essentials;
namespace road_running
{
    public partial class SShell : Xamarin.Forms.Shell
    {
        private string email;
        private string name;
        private string photocode;
        private string photo;
        private string idcard;
        public static List<Staff> InfoResult { get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }

        public string Photo_code
        {
            get { return photocode; }
            set
            {
                photocode = value;
                OnPropertyChanged(nameof(Photo_code));
            }
        }
        public string Id_card
        {
            get { return idcard; }
            set
            {
                idcard = value;
                OnPropertyChanged(nameof(Id_card));
            }
        }

        public string Staff_ID { get; set; }
        public SShell()
        {
            InitializeComponent();
            BindingContext = this;
        }
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            await Shell.Current.GoToAsync("//MainPage1");
        }
    }
}