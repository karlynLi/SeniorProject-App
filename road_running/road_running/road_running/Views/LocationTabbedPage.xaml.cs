using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationTabbedPage : TabbedPage
    {
        public LocationTabbedPage()
        {
            InitializeComponent();

            var tabbedPage = new TabbedPage();
            tabbedPage.Children.Add(new MapsPage());
            tabbedPage.Children.Add(new SupplyLocationPage());
        }
    }
}