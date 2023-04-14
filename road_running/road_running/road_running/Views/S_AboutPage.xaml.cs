using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.Models;
using road_running.Providers;
using System.IO;
using Rg.Plugins.Popup.Services;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class S_AboutPage : ContentPage
    {
        private string photocode;
        private string photo;
        public string Photo_code
        {
            get { return photocode; }
            set
            {
                photocode = value;
                OnPropertyChanged(nameof(Photo_code));
            }
        }
        public string Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }
        public static List<Staff> Getsinfo { get; set; }
        public S_AboutPage()
        {
            InitializeComponent();
            GetSInfo();
            BindingContext = this;
        }
        private async void GetSInfo()
        {
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            string id = SShellInstance.Staff_ID;
            
            Staff Sinfo = new Staff()
            {
                Staff_ID = id
            };
            Getsinfo = await S_GetAboutProvider.SGetInfoAsync(Sinfo);
            //string Birth = Getsinfo[0].Birthday.ToString("yyyy-MM-dd");
            Console.WriteLine("這邊是CS");
            Console.WriteLine(Getsinfo);
            name.Text = Getsinfo[0].Name;
            email.Text = Getsinfo[0].Email;
            phone.Text = Getsinfo[0].Phone;
            idcard.Text = Getsinfo[0].Phone;
            birth.Date = Getsinfo[0].Birthday;
            address.Text = Getsinfo[0].Address;
            contactname.Text = Getsinfo[0].Contact_name;
            contactphone.Text = Getsinfo[0].Contact_phone;
            relation.Text = Getsinfo[0].Relation;
            line.Text = Getsinfo[0].Line_ID;
            Photo_code = Getsinfo[0].Photo_code;
            Photo = Getsinfo[0].Photo;
        }
        public string uploadcode;
        public string Photopath;
        public static List<Staff> AlertInfo { get; set; }
        public async void Choose_image(object sender, EventArgs e)
        {
            //開啟媒體庫選圖片
            //var profiletap = new TapGestureRecognizer();
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });
            if (result != null)
            {
                Console.WriteLine(result.FullPath);
                Photopath = result.FullPath;
                var stream = await result.OpenReadAsync();
                if (stream != null)
                {
                    Imageresult.Source = ImageSource.FromStream(() => stream);
                    Console.WriteLine(stream);
                }
                string code = SaveImage(result.FullPath);
                Console.WriteLine(code);
                uploadcode = code;
                //string uploadcode = System.Text.Encoding.Default.GetString(code);
                //Console.WriteLine(uploadcode);


                //Photo_code = code;
            }
            else
            {
                var myPopup = new DisPlayMessage("通知", "沒有選擇圖片", "確認");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
            }

            //Imageresult.GestureRecognizers.Add(profiletap);
        }
        public string SaveImage(String path)
        {
            FileStream fs = null;
            //fs = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
            fs = File.OpenRead(path);
            int filelength = (int)fs.Length;
            Byte[] image = new Byte[filelength];
            fs.Read(image, 0, filelength);
            fs.Close();
            string Imgbase64;
            Imgbase64 = Convert.ToBase64String(image);
            Console.WriteLine(Imgbase64);
            return Imgbase64;
        }
        private async void Store_edit(object sender, EventArgs e)
        {
            var SShellInstance = Xamarin.Forms.Shell.Current as SShell;
            string id = SShellInstance.Staff_ID;
            Staff UpdateInfo = new Staff()
            {
                Staff_ID = id,
                Name = name.Text,
                Email = email.Text,
                Phone = phone.Text,
                Id_card = idcard.Text,
                Birthday = birth.Date,
                Address = address.Text,
                Contact_name = contactname.Text,
                Contact_phone = contactphone.Text,
                Relation = relation.Text,
                Line_ID = line.Text,
                Uploadcode = uploadcode

            };
            AlertInfo = await S_AboutProvider.SAboutAsync(UpdateInfo);

            if (AlertInfo[0].ans == "photo upload failed")
            {
                var myPopup = new DisPlayMessage("失敗", "上傳失敗", "返回");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                return;
            }
            else
            {
                //Console.WriteLine(AlertInfo[0].Photo_code);
                AlertInfo[0].Photo_code = AlertInfo[0].ans;
                var myPopup = new DisPlayMessage("成功", "更新資料成功！", "確認");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                Application.Current.MainPage = new SShell() { Email = email.Text, Name = name.Text, Staff_ID = id, Photo_code = AlertInfo[0].Photo_code, Photo = AlertInfo[0].Photo };//Photo_code = Alertinfo
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                //await Navigation.PopAsync(); //回上一頁
            }

            Console.WriteLine(AlertInfo);

        }
        private async void Cancel_edit(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
            //回上一頁
            //await Shell.Current.GoToAsync("..");
        }
    }
}