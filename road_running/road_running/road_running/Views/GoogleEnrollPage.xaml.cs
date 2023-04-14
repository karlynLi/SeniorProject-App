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
using Xamarin.Essentials;

namespace road_running.Views
{
    public partial class GoogleEnrollPage : ContentPage
    {
        public readonly IGoogleManager _googleManager; // Models裡的GoogleUsers的介面
        public GoogleEnrollPage(GoogleUser googleUser)
        {
            _googleManager = DependencyService.Get<IGoogleManager>();
            InitializeComponent();
            user = googleUser;
        }
        GoogleUser user;
        int Age;
        public async void EnrollClicked(object sender, EventArgs e)
        {
            if (Name.Text == null || Phone.Text == null || IdCard.Text == null || Birth.Date == null || Address.Text == null || Contact_name == null || Contact_phone.Text == null || Relation.Text == null)
            {
                var myPopup = new DisPlayMessage("失敗", "輸入不可為空", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }
            else
            {
                if (Age < 13)
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
                        Email = user.Email,
                        Phone = Phone.Text,
                        Id_card = IdCard.Text,
                        Birthday = Birth.Date,
                        Address = Address.Text,
                        Contact_name = Contact_name.Text,
                        Contact_phone = Contact_phone.Text,
                        Relation = Relation.Text
                    };
                    using (HttpClientHandler handler = new HttpClientHandler())
                    {
                        using (HttpClient client = new HttpClient(handler))
                        {
                            var json = JsonConvert.SerializeObject(enroll, Formatting.Indented);
                            var data = "[" + json + "]";

                            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/enroll_member.php", content);
                            string responseMessage = await response.Content.ReadAsStringAsync();
                            responseMessage = responseMessage.Replace("\uFEFF", "");
                            Console.WriteLine(responseMessage);
                            List<Member> EnrollResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);

                            if (EnrollResult[0].ans == "NoEmpty")
                            {
                                var myPopup = new DisPlayMessage("失敗", "上傳失敗", "OK");
                                await PopupNavigation.Instance.PushAsync(myPopup);
                                await myPopup.PopupClosedTask;
                                //await DisplayAlert("失敗", "上傳失敗", "OK");
                                return;
                            }
                            else
                            {
                                if (EnrollResult[0].ans == "NoSuccess")
                                {
                                    var myPopup = new DisPlayMessage("失敗", "註冊失敗", "OK");
                                    await PopupNavigation.Instance.PushAsync(myPopup);
                                    await myPopup.PopupClosedTask;
                                    //await DisplayAlert("失敗", "註冊失敗", "OK");
                                }
                                else
                                {
                                    // 註冊成功，接收user所有資料
                                    var myPopup = new DisPlayMessage("你的會員編號", EnrollResult[0].ans, "確認");
                                    await PopupNavigation.Instance.PushAsync(myPopup);
                                    await myPopup.PopupClosedTask;
                                    Member res = await GmailCheckProvider.CheckMail(user);
                                    if (res.ans == "yes") // 已經有此信箱
                                    {
                                        Console.WriteLine("=========有此信箱");
                                        // Preference記下資訊
                                        Preferences.Set("googleLogin", true);
                                        Preferences.Set("log", true);
                                        Preferences.Set("mail", res.Email);
                                        Preferences.Set("name", res.Name);
                                        Preferences.Set("id_card", res.Id_card);
                                        Preferences.Set("member_ID", res.Member_ID);
                                        Preferences.Set("photo_code", res.Photo_code);
                                        Preferences.Set("photo", res.Photo);
                                        // 將user資訊灌入AppShell
                                        Application.Current.MainPage = new AppShell() { Email = res.Email, Name = res.Name, Id_card = res.Id_card, Member_ID = res.Member_ID, Photo_code = res.Photo_code, Photo = res.Photo };
                                        // 前往主畫面
                                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                                        MessagingCenter.Send<string>("1", "myService");
                                    }
                                    //await DisplayAlert("你的會員編號", EnrollResult.ans, "確認");
                                    await Navigation.PopAsync(); //回上一頁
                                }
                            }
                        }
                    }
                }
            }
        }
        public async void Cancel(object sender, EventArgs e)
        {
            _googleManager.Logout();
            await Navigation.PopAsync(); //回上一頁
        }
        public async void Choose_birth(object sender, DateChangedEventArgs args)
        {
            Age = DateTime.Now.Year - Birth.Date.Year;
            if (Age < 13)
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
