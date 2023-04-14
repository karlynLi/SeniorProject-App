using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace road_running.Models
{
    public class Record
    {
        public string Registration_ID { get; set; }
        public string Running_ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string GetDate { get; set; }

        // 透過 date 判斷該活動為何狀態 (已結束/進行中/即將到來)
        public int Status 
        { 
            get { return WhatStatus(Date); } 
        }

        public int WhatStatus(DateTime time)
        {
            if (time > DateTime.Now.AddDays(1))
                //return "即將到來";
                return 3;

            else if (DateTime.Now.Date <= time && time < DateTime.Now.AddDays(1))
                //return "進行中";
                return 2;

            else
                //return "已結束";
                return 1;

        }

    }
}
