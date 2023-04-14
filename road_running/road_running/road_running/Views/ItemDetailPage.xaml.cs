using road_running.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using road_running.Models;
using System.Collections.ObjectModel;

namespace road_running.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}