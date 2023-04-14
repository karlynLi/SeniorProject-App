using road_running.ViewModels;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class RecordDetailPage : ContentPage
    {
        public RecordDetailPage(string run_id, string regis_id)
        {
            InitializeComponent();
            BindingContext = new RecordDetailViewModel(run_id, regis_id);
        }
    }
}