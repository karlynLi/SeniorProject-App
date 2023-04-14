using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace road_running.Models
{
    public class activity
    {
        public string Id { get; set; } // 活動ID
        public string Name { get; set; } // 活動名稱
        public DateTime Date { get; set; } // 活動日期
        public DateTime Startdate { get; set; } // 報名開始時間
        public DateTime Enddate { get; set; } // 報名結束時間
        public string Location { get; set; } // 活動地點
        public string[] Group { get; set; } // 活動組別陣列
        public string ImageUrl { get; set; } // 圖片路徑
        public string ImageName { get; set; } // 圖片檔名
        // 拿ImageUrl加上前面的網址（完整Url路徑）回傳

        public bool isVisible
        {
            get { return Startdate <= DateTime.Now && DateTime.Now <= Enddate; }
        }
        public string photo
        {
            get { return ImageUrl + ImageName; }
        }
        public string GroupName
        {
            get { return GetTotalGroupName(); }
        }
        public bool LessThenTen()
        {
            for (int i = 1; i < Group.Length; i = i + 2)
            {
                if (Int32.Parse(Group[i]) < 10)
                    return true;
            }
            return false;
        }
        public bool BetweenTenAndTewnty()
        {
            for (int i = 1; i < Group.Length; i = i + 2)
            {
                if (Int32.Parse(Group[i]) >= 10 && Int32.Parse(Group[i]) <= 20)
                    return true;
            }
            return false;
        }
        public bool GreaterTewnty()
        {
            for (int i = 1; i < Group.Length; i = i + 2)
            {
                if (Int32.Parse(Group[i]) > 20)
                    return true;
            }
            return false;
        }
        public string GetTotalGroupName()
        {
            string str = "";
            for(int i=0; i<Group.Length; i = i + 2)
            {
                str += Group[i] + "(" + Group[i+1] + ")";
            }
            return str;
        }
        //public override string ToString()
        //{
        //    return Name;
        //}
    }
}
