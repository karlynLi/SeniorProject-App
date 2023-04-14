using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class RecordPage2 : ContentPage
    {
        RecordViewModel vm = new RecordViewModel(2);

        public RecordPage2()
        {
            InitializeComponent();
            BindingContext = vm;
        }
        protected override async void OnAppearing()
        {
            vm.LoadRecord(2);
            base.OnAppearing();
        }
    }
}