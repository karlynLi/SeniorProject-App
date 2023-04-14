using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace road_running.Views
{
    public partial class DisPlayMessage : Rg.Plugins.Popup.Pages.PopupPage, INotifyPropertyChanged
    {
        private TaskCompletionSource<bool> taskCompletionSource; 
        public Task PopupClosedTask { get { return taskCompletionSource.Task; } } // 接收taskCompletionSource.Task值，用於等待此PopUp頁面

        public DisPlayMessage(string title, string text, string btn)
        {
            InitializeComponent();
            BindingContext = this;
            F_One_Title = title;
            F_One_Text = text;
            Btn_Text = btn;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            taskCompletionSource.SetResult(true); // 當關閉此PopUp頁面，就會傳給taskCompletionSource，PopupClosedTask會接收到值
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string f_one_title;
        public string F_One_Title
        {
            get { return f_one_title; }
            set
            {
                f_one_title = value;
                OnPropertyChanged();
            }
        }
        string f_one_text;
        public string F_One_Text
        {
            get { return f_one_text; }
            set
            {
                f_one_text = value;
                OnPropertyChanged();
            }
        }
        string btn_Text;
        public string Btn_Text
        {
            get { return btn_Text; }
            set
            {
                btn_Text = value;
                OnPropertyChanged();
            }
        }
        [Obsolete]
        private async void CloseBtn_Clicked(object sender, EventArgs e)
        {
            //taskCompletionSource.SetResult(Title);
            //await PopupNavigation.Instance.PopAllAsync();
            await PopupNavigation.Instance.PopAsync(true);

        }
    }
}
