using System;

namespace road_running.Models
{
    public class RecordDetail
    {
        public string Registration_ID { get; set; } // 報名編號
        public string Running_ID { get; set; } // 路跑編號
        public string Name { get; set; } // 活動名稱
        public string Group_name { get; set; } //組別名稱
        public DateTime Time { get; set; }// 報到時間
        public string Place { get; set; } // 報到地點
        public string Grade { get; set; } // 名次
        //public string GetGrade
        //{
        //    get { return WhatGrade(Grade); }
        //}
        //public string WhatGrade(string grade)
        //{
        //    if (grade == "noData")
        //        return "尚無資料";
        //    else
        //        return grade;
        //}
        public string Complete_time { get; set; } // 完賽成績
        //public string GetCompleteTime
        //{
        //    get { return WhatTime(Complete_time); }
        //}
        //public string WhatTime(string time)
        //{
        //    if (time == "noData")
        //        return "尚無資料";
        //    else
        //        return time;
        //}
    }
}