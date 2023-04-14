using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.Models;
using road_running.Providers;
using Rg.Plugins.Popup.Services;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_ForgetPassword : ContentPage
    {
        public int count = 60;
        public static List<Staff> ForgetResult { get; set; }
        public static List<Staff> AuthResult { get; set; }
        public S_ForgetPassword()
        {
            InitializeComponent();
        }
        public async void CheckMail(object sender, EventArgs e)
        {
            Staff staffmail = new Staff()
            {
                Email = mail.Text
            };
            ForgetResult = await SForgetPwdProvider.ForgetPwdAsync(staffmail);
            Console.WriteLine("==Forgetpwd.cs==");
            //Console.WriteLine(ForgetResult[0].Status);
            //Console.WriteLine(ForgetResult[0].Response);
            if (ForgetResult[0].ans == "email can't empty")
            {
                var myPopup = new DisPlayMessage("驗證結果", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證失敗", ForgetResult[0].ans));
            }
            else if (ForgetResult[0].Status == "failed")
            {
                var myPopup = new DisPlayMessage("驗證結果", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage(ForgetResult[0].Status, ForgetResult[0].Response));
            }
            else
            {
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
                            code1.Text = "驗證信箱";
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
                var myPopup = new DisPlayMessage("驗證結果", ForgetResult[0].ans, "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage(ForgetResult[0].Status, ForgetResult[0].Response));
            }
        }
        public async void CheckNum(object sender, EventArgs e)
        {
            Staff membercaptcha = new Staff()
            {
                Email = mail.Text,
                Captcha = captcha.Text
            };
            AuthResult = await SForgetPwdProvider.AuthenticationAsync(membercaptcha);
            Console.WriteLine(AuthResult[0].ans);
            if (AuthResult[0].ans == "yes")
            {
                var myPopup = new DisPlayMessage("驗證成功", AuthResult[0].ans, "前往設定密碼");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                //await PopupNavigation.Instance.PushAsync(new DisPlayMessage("驗證成功", AuthResult[0].ans));
                await Navigation.PushAsync(new S_ResetPassword(membercaptcha.Email));
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
    
        //public void SendMail()
        //{
        //    MailMessage message = new MailMessage();
        //    message.To.Add(new MailAddress(mail.Text));
        //    message.From = new MailAddress("vickie890514@gmail.com", "yoyo", System.Text.Encoding.UTF8);
        //    message.Subject = "測試郵件";
        //    message.SubjectEncoding = System.Text.Encoding.UTF8;
        //    message.Body = "測試內容";
        //    message.BodyEncoding = System.Text.Encoding.UTF8;
        //    message.IsBodyHtml = false;
        //    message.Priority = MailPriority.High;
        //    SmtpClient client = new SmtpClient();
        //    client.Credentials = new System.Net.NetworkCredential("", "");
        //    client.Port = 587;
        //    client.Host = "smtp.gmail.com";
        //    client.EnableSsl = true;
        //    object userState = message;
        //    try
        //    {
        //        client.SendAsync(message, userState);
        //        DisplayAlert("成功","傳送成功","ok");
        //    }
        //    catch (SmtpException ex)
        //    {
        //        DisplayAlert("成功", ex.Message, "ok");
        //    }
        //}

        private async void GoBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}