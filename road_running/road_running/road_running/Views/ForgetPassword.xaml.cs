using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.Models;
using road_running.Providers;
using Rg.Plugins.Popup.Services;
namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPassword : ContentPage
    {
        public int count = 60;
        public static List<Member> ForgetResult { get; set; }
        public static List<Member> AuthResult { get; set; }
        public ForgetPassword()
        {
            InitializeComponent();
        }
        private async void GoBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void CheckMail(object sender, EventArgs e)
        {
            Member membermail = new Member()
            {
                Email = mail.Text
            };
            ForgetResult = await ForgetPwdProvider.ForgetPwdAsync(membermail);
            Console.WriteLine("==Forgetpwd.cs==");
            if(ForgetResult[0].ans == "email can't empty")
            {
                var myPopup = new DisPlayMessage("驗證失敗", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證失敗", ForgetResult[0].ans));
            }
            else if (ForgetResult[0].ans == "not exist")
            {
                var myPopup = new DisPlayMessage("驗證結果", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage(ForgetResult[0].Status, ForgetResult[0].Response));
            }
            else if(ForgetResult[0].ans == "NoSuccess")
            {
                var myPopup = new DisPlayMessage("驗證結果", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                Check.IsEnabled = false;
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    // do something every 60 seconds
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Check.Text = "倒數" + count + "秒";
                        count--;
                    });
                    if (count == 0)
                    {
                        Console.WriteLine(count);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Check.Text = "驗證信箱";
                            Check.IsEnabled = true;
                        });

                        Console.WriteLine(Check.Text);
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
                //        Check.IsEnabled = true;
                //    });
                //    return true; // runs again, or false to stop
                //});

                var myPopup = new DisPlayMessage("驗證結果", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage(ForgetResult[0].Status, ForgetResult[0].Response));
            }
        }
        private async void CheckNum(object sender, EventArgs e)
        {
            Member membercaptcha = new Member()
            {
                Email = mail.Text,
                Captcha = captcha.Text
            };
            AuthResult = await ForgetPwdProvider.AuthenticationAsync(membercaptcha);
            Console.WriteLine(AuthResult[0].ans);
            if (AuthResult[0].ans == "yes")
            {
                var myPopup = new DisPlayMessage("驗證成功", AuthResult[0].ans, "前往設定密碼");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證成功", AuthResult[0].ans));
                await Navigation.PushAsync(new ResetPassword(membercaptcha.Email));
            }
            else if (AuthResult[0].ans == "no")
            {
                var myPopup = new DisPlayMessage("驗證失敗", AuthResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證失敗", AuthResult[0].ans));
            }
            else
            {
                var myPopup = new DisPlayMessage("請輸入驗證碼", AuthResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                // await PopupNavigation.Instance.PushAsync(new DisPlayMessage("請輸入驗證碼", AuthResult[0].ans));
            }

        }
    }
}