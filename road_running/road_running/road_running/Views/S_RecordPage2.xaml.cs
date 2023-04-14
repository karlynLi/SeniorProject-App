using Xamarin.Forms;
using road_running.ViewModels;

namespace road_running.Views
{
    public partial class S_RecordPage2 : ContentPage
    {
        S_RecordViewModel vm = new S_RecordViewModel(2);

        public S_RecordPage2()
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