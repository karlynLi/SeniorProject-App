using System;
using System.Collections.Generic;
using System.Text;

namespace road_running.Models
{
    public class Member
    {
        // Google登入
        //public static bool IsLogedIn;
        //public static IGoogleManager _googleManager;

        public string Member_ID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Id_card { get; set; } //身分證
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Contact_name { get; set; }
        public string Contact_phone { get; set; }
        public string Relation { get; set; }
        public string Uploadcode { get; set; }
        public string Photo_code { get; set; } //頭貼檔名.jpg
        public string Photo
        {
            get { return "http://running.im.ncnu.edu.tw/running/files/photo/member/" + Photo_code; }
        }
        public string ans { get; set; }
        public string Registration_ID { get; set; }
        public string Status { get; set; }
        public string Response { get; set; }
        public string Captcha { get; set; }
    }
}
