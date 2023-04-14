using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.ViewModels;
using road_running.Models;
using Rg.Plugins.Popup.Services;

namespace road_running.Views
{
    public partial class S_RecordDetailPage : ContentPage
    {
        public string running_id, group_id;
        public S_RecordDetailPage(string run_id, string workgroup_id)
        {
            InitializeComponent();
            BindingContext = new S_RecordDetailViewModel(run_id, workgroup_id);
            running_id = run_id;
            group_id = workgroup_id;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new S_WorkContent(running_id, group_id));
        }
    }
}