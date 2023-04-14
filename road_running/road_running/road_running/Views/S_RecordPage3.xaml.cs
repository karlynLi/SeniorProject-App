using Xamarin.Forms;
using road_running.ViewModels;

namespace road_running.Views
{
    public partial class S_RecordPage3 : ContentPage
    {
        S_RecordViewModel vm = new S_RecordViewModel(3);

        public S_RecordPage3()
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