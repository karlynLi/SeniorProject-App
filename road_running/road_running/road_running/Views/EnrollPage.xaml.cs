using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using road_running.ViewModels;
using road_running.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using road_running.Providers;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnrollPage : ContentPage
    {
        public int count = 60;
        public List<Member> Check { get; set; }
        public List<Member> Confirm { get; set; }
        public EnrollPage()
        {
            InitializeComponent();
        }
        public static List<Member> EnrollResult { get; set; }
        int Age;
        public async void Check_mail(object sender, EventArgs e)
        {
            Member md = new Member()
            {
                Email = Email.Text
            };
            Check = await EnrollProvider.CheckmailAsync(md);
            if(Check[0].ans == "Email existed")
            {
                var myPopup = new DisPlayMessage("驗證結果", Check[0].ans, "重新輸入");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                Captcha.IsReadOnly = false;
                code1.IsEnabled = false;

                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    // do something every 60 seconds
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        code1.Text = "倒數" + count + "秒";
                        count--;
                    });
                    if (count == 0) 
                    {
                        Console.WriteLine(count);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            code1.Text = "寄送驗證碼";
                            code1.IsEnabled = true;
                        });
                        
                        Console.WriteLine(code1.Text);
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
                //     // runs again, or false to stop

                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        count = 10;
                //        code1.IsEnabled = true;
                //        code1.Text = "傳送驗證碼";
                //    });
                //    return true;
                //});
                var myPopup = new DisPlayMessage("檢查信箱", "已寄出驗證碼", "確認");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
        }
        public async void Check_captcha(object sender, EventArgs e)
        {
            Member mid = new Member()
            {
                Email = Email.Text,
                Captcha = Captcha.Text
            };
            Confirm = await EnrollProvider.ConfirmAsync(mid);
            if(Confirm[0].ans == "email can't empty")
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
            else if(Confirm[0].ans == "信箱輸入錯誤")
            {
                var myPopup = new DisPlayMessage("驗證結果", Confirm[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                Email.IsReadOnly = true;
                enroll.IsEnabled = true;
                Verify.IsEnabled = false;
                Captcha.IsReadOnly = true;
                var myPopup = new DisPlayMessage("驗證結果", Confirm[0].ans , "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
        }
        public async void EnrollClicked(object sender, EventArgs e)
        {
            if(Name.Text == null || Email.Text==null || Phone.Text == null || Pass1.Text==null || Pass2.Text==null || IdCard.Text == null || Birth.Date==null || Address.Text==null || Contact_name==null || Contact_phone.Text==null || Relation.Text==null)
            {
                var myPopup = new DisPlayMessage("失敗", "輸入不可為空", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else 
            {
                if(Pass1.Text != Pass2.Text)
                {
                    var myPopup = new DisPlayMessage("失敗", "兩次密碼不相同", "重新輸入");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                }
                else if(Age < 13)
                {
                    var myPopup = new DisPlayMessage("年紀不符合", "註冊會員需年滿13歲", "返回");
                    await PopupNavigation.Instance.PushAsync(myPopup);
                    await myPopup.PopupClosedTask;
                }
                else
                {
                    Member enroll = new Member()
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
                        Relation = Relation.Text
                    };
                    EnrollResult = await EnrollProvider.EnrollAsync(enroll);

                    if (EnrollResult[0].ans == "email can't empty")
                            {
                                var myPopup = new DisPlayMessage("失敗", "上傳失敗", "OK");
                                await PopupNavigation.Instance.PushAsync(myPopup);
                                await myPopup.PopupClosedTask;
                                //await DisplayAlert("失敗", "上傳失敗", "OK");
                                return;
                            }
                            else
                            {
                                if (EnrollResult[0].ans == "no")
                                {
                                    var myPopup = new DisPlayMessage("失敗", "註冊失敗", "OK");
                                    await PopupNavigation.Instance.PushAsync(myPopup);
                                    await myPopup.PopupClosedTask;
                                    //await DisplayAlert("失敗", "註冊失敗", "OK");
                                }
                                else
                                {
                                    var myPopup = new DisPlayMessage("你的會員編號", EnrollResult[0].ans, "確認");
                                    await PopupNavigation.Instance.PushAsync(myPopup);
                                    await myPopup.PopupClosedTask;
                                    //await DisplayAlert("你的會員編號", EnrollResult.ans, "確認");
                                    await Navigation.PopAsync(); //回上一頁
                                }
                            }

                        
                    
                }

            }

        }
        public async void Choose_birth(object sender, DateChangedEventArgs args)
        {
            Age = DateTime.Now.Year - Birth.Date.Year;
            if(Age < 13)
            {
                var myPopup = new DisPlayMessage("年紀不符合", "註冊會員需年滿13歲", "返回");
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