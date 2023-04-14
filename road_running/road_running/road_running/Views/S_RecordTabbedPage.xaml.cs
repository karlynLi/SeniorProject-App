using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_RecordTabbedPage : TabbedPage
    {
        public S_RecordTabbedPage()
        {
            InitializeComponent();
            var tabbedPage = new TabbedPage();
            tabbedPage.Children.Add(new S_RecordPage1());
            tabbedPage.Children.Add(new S_RecordPage2());
            tabbedPage.Children.Add(new S_RecordPage3());
        }
    }
}