using Xamarin.Forms;
using road_running.ViewModels;

namespace road_running.Views
{
    public partial class S_RecordPage1 : ContentPage
    {
        S_RecordViewModel vm = new S_RecordViewModel(1);
        public S_RecordPage1()
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