using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class RecordPage1 : ContentPage
    {
        RecordViewModel vm = new RecordViewModel(1);

        public RecordPage1()
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            vm.LoadRecord(1);
            base.OnAppearing();
        }
    }
}