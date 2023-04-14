using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class RecordPage3 : ContentPage
    {
        RecordViewModel vm = new RecordViewModel(3);

        public RecordPage3()
        {
            InitializeComponent();
            BindingContext = vm;
        }
        protected override async void OnAppearing()
        {
            vm.LoadRecord(3);
            base.OnAppearing();
        }
    }
}