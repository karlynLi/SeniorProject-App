using road_running.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordTabbedPage : TabbedPage
    {
        public RecordTabbedPage()
        {
            InitializeComponent();

            var tabbedPage = new TabbedPage();
            tabbedPage.Children.Add(new RecordPage1());
            tabbedPage.Children.Add(new RecordPage2());
            tabbedPage.Children.Add(new RecordPage3());

        }
    }
}