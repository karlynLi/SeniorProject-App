using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using road_running.Models;
using road_running.Providers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Timers;
namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_EnrollPage : ContentPage
    {
        public int count = 60;
        public List<Staff> Check { get; set; }
        public List<Staff> Confirm { get; set; }
        public List<Staff> SEnrollInfo { get; set; }
        private int Age;
        public S_EnrollPage()
        {
            InitializeComponent();
        }
        public async void Check_captcha(object sender, EventArgs e)
        {
            Staff mid = new Staff()
            {
                Email = Email.Text,
                Captcha = Captcha.Text
            };
            Confirm = await S_EnrollProvider.ConfirmAsync(mid);
            if (Confirm[0].ans == "信箱輸入錯誤")
            {
                var myPopup = new DisPlayMessage("失敗", "信箱輸入為空", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else if (Confirm[0].ans == "no")
            {
                var myPopup = new DisPlayMessage("失敗", "驗證碼輸入錯誤", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                senroll.IsEnabled = true;
                Email.IsReadOnly = true;
                Captcha.IsReadOnly = true;
                checkcap.IsEnabled = false;
                var myPopup = new DisPlayMessage("驗證結果", "成功", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
        }
        public async void Check_mail(object sender, EventArgs e)
        {
            Staff md = new Staff()
            {
                Email = Email.Text
            };
            Check = await S_EnrollProvider.CheckmailAsync(md);
            if (Check[0].ans == "Email existed")
            {
                var myPopup = new DisPlayMessage("錯誤", Check[0].ans, "重新輸入");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                
                Captcha.IsReadOnly = false;
                checkmail.IsEnabled = false;
                checkcap.IsEnabled = true;
                //Countdown();
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    // do something every 60 seconds
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        checkmail.Text = "倒數" + count + "秒";
                        count--;
                    });
                    if(count == 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            checkmail.Text = "傳送驗證碼";
                            checkmail.IsEnabled = true;

                        });
                        count = 60;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                });

                //Device.StartTimer(new TimeSpan(0, 0, 60), () =>
                //{
                //    // do something every 60 seconds
                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        checkmail.Text = "傳送驗證碼";
                //        checkmail.IsEnabled = true;
                //    });
                //    return true; // runs again, or false to stop
                //});
                var myPopup = new DisPlayMessage("檢查信箱", "已寄出驗證碼", "確認");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
        }
        private void Countdown()
        {
            for (int i = 60; i > 0; i--)
            {
                Timer aTimer = new System.Timers.Timer(1000);
                aTimer.Elapsed += OnTimedEvent;

            }
            //for (int i = 60; i > 0; i--)
            //{
            //    Timer aTimer = new System.Timers.Timer(1000);
            //    checkmail.Text = "倒數" + i + "秒";


            //}
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            
            Console.WriteLine(checkmail.Text);
            count--;


        }
        public async void EnrollClicked(object sender, EventArgs e)
        {
            if (Name.Text == null || Email.Text == null || Phone.Text == null || Pass1.Text == null || Pass2.Text == null || IdCard.Text == null || Birth.Date == null || Address.Text == null || Contact_name == null || Contact_phone.Text == null || Relation.Text == null || line.Text == null)
            {
                var myPopup = new DisPlayMessage("失敗", "輸入不可為空", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                if (Pass1.Text != Pass2.Text)
                {
                    var myPopup = new DisPlayMessage("失敗", "兩次密碼不相同", "重新輸入");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                }
                else if (Age < 19)
                {
                    var myPopup = new DisPlayMessage("年紀不符合", "註冊工作人員需年滿18歲", "返回");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                }
                else
                {
                    Staff SEnroll = new Staff()
                    {
                        Name = Name.Text,
                        Email = Email.Text,
                        Phone = Phone.Text,
                        Password = Pass1.Text,
                        Id_card = IdCard.Text,
                        Birthday = Birth.Date,
                        Address = Address.Text,
                        Contact_name = Contact_name.Text,
                        Contact_phone = Contact_phone.Text,
                        Relation = Relation.Text,
                        Line_ID = line.Text
                    };
                    SEnrollInfo = await S_EnrollProvider.S_EnrollAsync(SEnroll);
                    if (SEnrollInfo[0].ans == "NoEmpty")
                    {
                        var myPopup = new DisPlayMessage("失敗", "上傳失敗", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                        return;
                    }
                    else
                    {
                        if (SEnrollInfo[0].ans == "NoSuccess")
                        {
                            var myPopup = new DisPlayMessage("失敗", "註冊失敗", "返回");
                            await PopupNavigation.Instance.PushAsync(myPopup);
                            await myPopup.PopupClosedTask;
                        }
                        else
                        {
                            var myPopup = new DisPlayMessage("你的工作人員編號(請記住此編號以供日後登入使用)", SEnrollInfo[0].ans, "確認");
                            await PopupNavigation.Instance.PushAsync(myPopup);
                            await myPopup.PopupClosedTask;
                            await Navigation.PopAsync(); //回上一頁
                        }
                    }
                }

            }
        }
        public async void Choose_Birth(object e, DateChangedEventArgs eventArgs)
        {
            Age = DateTime.Now.Year - Birth.Date.Year;
            if (Age < 19)
            {
                var myPopup = new DisPlayMessage("年紀不符合", "註冊工作人員需年滿18歲", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
        }
        public async void Log(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}