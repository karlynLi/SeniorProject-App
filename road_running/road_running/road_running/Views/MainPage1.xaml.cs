using road_running.Models;
using road_running.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage1 : ContentPage
    {
        protected virtual bool OnBackButtonPressed { get; }
        //ViewCell lastCell;
        public IList<activity> activitys { get; private set; }
        private MainPageViewModel vm = new MainPageViewModel();
        public MainPage1()
        {
            InitializeComponent();
            OnBackButtonPressed = false;


            OnIsVisibleChanged();

            BindingContext = vm;
        }

        public static ImageSource ToImage(byte[] byteArrayIn)
        {

            var stream = new MemoryStream(byteArrayIn);
            var retSource = ImageSource.FromStream(() => stream);

            return retSource;
        }
        public async void GoLogin()
        {
            await Navigation.PushAsync(new LoginPage());
        }
        //async void OnListViewItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        //{
        //    activity selectedItem = e.SelectedItem as activity;
        //    await Application.Current.MainPage.Navigation.PushAsync(new ActivityDetailPage(selectedItem));
        //    //Navigation.PushAsync(new ActivityDetailPage());
        //}

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tappedItem = e.Item as activity;
        }

        //private void ViewCell_Tapped(object sender, System.EventArgs e)
        //{
        //    Console.WriteLine("TapTapTapTap!");
        //    if (lastCell != null)
        //        lastCell.View.BackgroundColor = Color.Transparent;
        //    var viewCell = (ViewCell)sender;
        //    if (viewCell.View != null)
        //    {
        //        viewCell.View.BackgroundColor = Color.Red;
        //        lastCell = viewCell;
        //    }
        //}

        /// The -negative value determines how many vertical units should the panel occuply on the screen.
        /// true或falsem決定BottomSheet移動到哪裡
        private async void MoveBottomSheet(bool close)
        {
            // true打開
            if (close == true)
            {
                var y = relativelayout.Y;
                BBB.IsVisible = true;
                //double finalTranslation = close ? (Device.Idiom == TargetIdiom.Phone ? -134.0 : -144.0) : (Device.Idiom == TargetIdiom.Phone ? -389.0 : -434.0);
                await BottomSheet.TranslateTo(BottomSheet.X, -250, 450, Easing.SinIn);
            }
            // false關閉
            else
            {
                BBB.IsVisible = false;
                await BottomSheet.TranslateTo(BottomSheet.X, BottomSheet.Y, 450, Easing.SinIn);
            }
        }

        void confirmClicked(object sender, EventArgs args)
        {
            MoveBottomSheet(false);
        }
        void OnButtonClicked(object sender, EventArgs args)
        {
            MoveBottomSheet(true);
        }
        /// This is fired multiple times while the user pans the bottom sheet. This variable captures the first intention of determining whether to open (pan up) or close (pan down)
        /// 檢查使用者對篩選頁面執行的動作
        bool _panelActivated = false;
        private void OnPanelUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                //使用者滑動中
                case GestureStatus.Running:
                    if (_panelActivated)
                    {
                        return;
                    }
                    // 傳給MoveBottomSheet使用者Y分量是正或負 （如果是負就是true 代表向下滑）
                    MoveBottomSheet(e.TotalY < 0);
                    _panelActivated = true;
                    break;
                case GestureStatus.Completed:
                    _panelActivated = false;
                    break;
                case GestureStatus.Canceled:
                    break;
            }
        }

        void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            vm.RefreshListView();
        }


        // 增加登入按鈕
        private void OnIsVisibleChanged()
        {
            //var item = bindable as ToolbarItem;
            var AppShellInstance = Xamarin.Forms.Shell.Current as AppShell;
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            var toolbarItems = this.ToolbarItems; // 取得這個頁面的toolbar
            ToolbarItem item = new ToolbarItem { Text = "登入", Command = new Command(() => GoLogin()) };
            Device.BeginInvokeOnMainThread(() => { toolbarItems.Add(item); });


            // 如果沒有登入才在toolbar加登入按鈕
            //if (AppShellInstance == null && SShellInstance == null)
            //if (AppShellInstance == null && SShellInstance == null)
            //{
            //    ToolbarItem item = new ToolbarItem { Text = "登入", Command = new Command(() => GoLogin()) };
            //    Device.BeginInvokeOnMainThread(() => { toolbarItems.Add(item); });
            //}
        }
    }
}