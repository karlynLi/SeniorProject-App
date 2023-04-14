using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using road_running.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using road_running.Providers;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Plugin.Media;
using Rg.Plugins.Popup.Services;
//using Android.Graphics;



namespace road_running.Views
{
    //public class Edit_About
    //{
    //    public Image Image { get; set; }
    //    public string name { get; set; }
    //    public string email { get; set; }
    //    public string phone { get; set; }
    //    public string ID_number { get; set; }
    //    public DatePicker birth { get; set; }
    //    public string line_id { get; set; }
    //    public string address { get; set; }
    //}
    [QueryProperty(nameof(Member_ID), "memberid")]
    [QueryProperty(nameof(Name), "name")]
    [QueryProperty(nameof(Email), "email")]
    [QueryProperty(nameof(Id_card), "idcard")]
    [QueryProperty(nameof(Phone), "phone")]
    [QueryProperty(nameof(Address), "address")]
    //[QueryProperty(nameof(Birthday), "birthday")]
    [QueryProperty(nameof(Contact_name), "contactname")]
    [QueryProperty(nameof(Contact_phone), "contactphone")]
    [QueryProperty(nameof(Relation), "relation")]
    [QueryProperty(nameof(Photo), "photo")]
    [QueryProperty(nameof(Photo_code), "photocode")]
    [QueryProperty(nameof(Birthday), "birthday")]
    public partial class AboutPage : ContentPage
    {
        private string memberid;
        private string photo;
        private string photocode;
        private string name;
        private string email;
        private string idcard;
        private string phone;
        private string address;
        private DateTime birthday;
        private string contactname;
        private string contactphone;
        private string relation;

        public static List<Member> AlertInfo { get; set; }

        //GetInfo = AppShell.
        public string Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }
        public string Photo_code
        {
            get { return photocode; }
            set
            {
                photocode = value;
                OnPropertyChanged(nameof(Photo_code));
            }
        }
        public string Member_ID
        {
            get { return memberid; }
            set
            {
                memberid = value;
                OnPropertyChanged(nameof(Member_ID));
            }

        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Id_card
        {
            get { return idcard; }
            set
            {
                idcard = value;
                OnPropertyChanged(nameof(Id_card));
            }
        }
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public string Contact_name
        {
            get { return contactname; }
            set
            {
                contactname = value;
                OnPropertyChanged(nameof(Contact_name));
            }
        }
        public string Contact_phone
        {
            get { return contactphone; }
            set
            {
                contactphone = value;
                OnPropertyChanged(nameof(Contact_phone));
            }
        }
        public string Relation
        {
            get { return relation; }
            set
            {
                if (value != this.relation)
                {
                    this.relation = value;
                    OnPropertyChanged(nameof(Relation));
                }
            }
        }
        public string uploadcode;
        public string Photopath;
        //public List<Member> Info { get; set; }
        public AboutPage()
        {
            InitializeComponent();
            //GetInfo();
            //Info = GetAboutProvider.GetInfoAsync(Member_ID);
            //GetAboutProvider.GetInfoAsync();
            BindingContext = this;
        }

        public async void Choose_image(object sender, EventArgs e)
        {
            //開啟媒體庫選圖片
            //var profiletap = new TapGestureRecognizer();
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo",
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
            Member UpdateInfo = new Member()
            {
                Member_ID = Member_ID,
                Name = GetName.Text,
                Email = Mail.Text,
                Phone = GetPhone.Text,
                Id_card = Idcard.Text,
                Birthday = Birth.Date,
                Address = GetAddress.Text,
                Contact_name = GetContactName.Text,
                Contact_phone = GetContactPhone.Text,
                Relation = GetRelation.Text,
                Uploadcode = uploadcode
            };
            AlertInfo = await AboutProvider.GetAboutAsync(UpdateInfo);

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
                var myPopup = new DisPlayMessage("成功", "更新資料成功！", "確認");
                await PopupNavigation.Instance.PushAsync(myPopup);
                await myPopup.PopupClosedTask;
                Application.Current.MainPage = new AppShell() { Email = Mail.Text, Name = GetName.Text, Member_ID = Member_ID, Photo_code=AlertInfo[0].Photo_code, Photo = AlertInfo[0].Photo};//Photo_code = Alertinfo
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                //await Navigation.PopAsync(); //回上一頁
            }

            Console.WriteLine(AlertInfo);

        }
        private async void Cancel_edit(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

}