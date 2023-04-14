using System;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using road_running.Models;
using road_running.Providers;
using Rg.Plugins.Popup.Services;
using road_running.Views;

namespace road_running.ViewModels
{
    public class S_ScanViewModel : INotifyPropertyChanged
    {
        public S_ScanViewModel()
        {
            GetScan();
            LaunchGiftScanner = new Command(GiftScanner);
            LaunchSignUpScanner = new Command(SignUpScanner);
            LaunchS_CheckinScanner = new Command(S_CheckinScanner);
        }

        public void GetScan()
        {
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            staff_id = SShellInstance.Staff_ID;
            Console.WriteLine("~~~~~~~~~~" + staff_id);
        }

        public string staff_id;
        // INotifyPropertyChanged實作介面
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            // 如果PropertyChanged不是null, 去Invoke name
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Command LaunchGiftScanner { get; }
        public Command LaunchSignUpScanner { get; }
        public Command LaunchS_CheckinScanner { get; }

        // 存QR掃到的資料
        //string QRstring;

        // 建立
        public class QR
        {
            public string QRresult { get; set; }
        }
        // 接收回PHP傳值的類別
        public class PHPresult
        {
            public string result { get; set; }
        }

        public string gifttext = "掃描失敗";

        public string GiftText
        {
            get { return gifttext; }
            set
            {
                gifttext = value;
                OnPropertyChanged();
            }
        }
        public string signuptext = "掃描失敗";

        public string SignUpText
        {
            get { return signuptext; }
            set
            {
                signuptext = value;
                OnPropertyChanged();
            }
        }

        public string s_checkintext = "掃描失敗";

        public string S_CheckinText
        {
            get { return s_checkintext; }
            set
            {
                s_checkintext = value;
                OnPropertyChanged();
            }
        }

        // 兌換禮物掃描
        private async void GiftScanner()
        {
            // 建立一個掃描page
            var scan = new ZXingScannerPage();
            // 以非同步方法將page新增到堆疊頂端
            //await Navigation.PushAsync(scan);
            await Application.Current.MainPage.Navigation.PushAsync(scan);
            //App.Current.MainPage = scan;
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // 掃到後將page移除
                    await Application.Current.MainPage.Navigation.PopAsync();
                    //await Navigation.PopAsync();
                    GiftText = result.Text;
                    //QRstring = result.Text;
                    S_GiftScanner Info = new S_GiftScanner();
                    Info = await S_GiftScannerProvider.GetGiftInfoAsync(GiftText, staff_id);
                    if (Info.status == "NoRegistration")
                    {
                        var myPopup = new DisPlayMessage("禮品兌換", "工作人員無報名此活動，請找此活動的工作人員進行兌換", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                    }
                    else if (Info.status == "NotExist")
                    {
                        var myPopup = new DisPlayMessage("禮品兌換", "查無此訂單", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                        //await PopupNavigation.PushAsync(new DisPlayMessage("禮品兌換", "查無此訂單"));
                    }
                    else if (Info.status == "NotCheckInYet")
                    {
                        var myPopup = new DisPlayMessage("禮品兌換", "還沒報到，請先報到完再換禮物", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                        //await PopupNavigation.PushAsync(new DisPlayMessage("禮品兌換", "還沒報到，請先報到完再換禮物"));
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new S_ExchangeResult(Info, GiftText));
                    }
                });
            };
        }

        // 報到掃描
        private async void SignUpScanner()
        {
            var scan = new ZXingScannerPage();
            // 以非同步方法將page新增到堆疊頂端
            //await Navigation.PushAsync(scan);
            await Application.Current.MainPage.Navigation.PushAsync(scan);
            //App.Current.MainPage = scan;
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // 掃後將page移除
                    await Application.Current.MainPage.Navigation.PopAsync();
                    //await Navigation.PopAsync();
                    SignUpText = result.Text;
                    //QRstring = result.Text;
                    S_RegistrationScanner Info = new S_RegistrationScanner();
                    Info = await S_RegistrationScannerProvider.GetRegistrationInfoAsync(SignUpText, staff_id);
                    //await PopupNavigation.PushAsync(new S_CheckInScanResult(Info, SignUpText));
                    if (Info.status == "NoRegistration")
                    {
                        var myPopup = new DisPlayMessage("禮品兌換", "工作人員無報名此活動，請找此活動的工作人員進行報到", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                    }
                    else if (Info.status == "NotExist") // 沒有此訂單
                    {
                        var myPopup = new DisPlayMessage("報到", "查無此訂單", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                        //await PopupNavigation.PushAsync(new DisPlayMessage("報到", "查無此訂單"));
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new S_CheckInScanResult(Info, SignUpText));
                    }
                });
            };
        }

        // 工作人員報到掃描
        private async void S_CheckinScanner()
        {
            var scan = new ZXingScannerPage();
            // 以非同步方法將page新增到堆疊頂端
            //await Navigation.PushAsync(scan);
            await Application.Current.MainPage.Navigation.PushAsync(scan);
            //App.Current.MainPage = scan;
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // 掃後將page移除
                    await Application.Current.MainPage.Navigation.PopAsync();
                    //await Navigation.PopAsync();
                    var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
                    S_CheckinText = result.Text+ SShellInstance.Staff_ID;
                    //QRstring = result.Text;
                    S_RegistrationScanner Info = new S_RegistrationScanner();
                    Info = await S_RegistrationScannerProvider.S_GetRegistrationInfoAsync(SShellInstance.Staff_ID, result.Text);
                    //await PopupNavigation.PushAsync(new S_CheckInScanResult(Info, SignUpText));
                    if (Info.status == "NotExist") // 沒有此訂單
                    {
                        var myPopup = new DisPlayMessage("報到", "查無此活動，無法報到", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                        //await PopupNavigation.PushAsync(new DisPlayMessage("報到", "查無此訂單"));
                    }
                    else if(Info.status == "Didn'tSignUpThisGroup")
                    {
                        var myPopup = new DisPlayMessage("報到", "你沒有報名此活動，無法報到！", "返回");
                        await PopupNavigation.Instance.PushAsync(myPopup);
                        await myPopup.PopupClosedTask;
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new S_ScanToCheckinResult(Info, result.Text));
                    }
                });
            };
        }
    }
}
