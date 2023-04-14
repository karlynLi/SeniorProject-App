using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms.Xaml;

namespace road_running.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowQrPage : ContentPage
    {
        public string Title { get; set; }
        public ZXingBarcodeImageView img = new ZXingBarcodeImageView();
        public ShowQrPage(string code, string title)
        {
            InitializeComponent();
            BindingContext = this;
            Title = title;
            Generate_QR(code);
            //Console.WriteLine(img);
            //QR_CODE = img;
        }
        private ZXingBarcodeImageView Generate_QR(string code)
        {
            qrcode.BarcodeValue = code;
            return qrcode;
            //return img;

            //string code = 
        }
    }
}