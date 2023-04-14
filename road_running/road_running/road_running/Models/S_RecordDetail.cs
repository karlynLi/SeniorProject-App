using System;

namespace road_running.Models
{
    public class S_RecordDetail
    {
        public string Workgroup_ID { get; set; } // 報名編號
        public string Running_ID { get; set; }
        public string Name { get; set; } // 活動名稱
        public string Work_name { get; set; } //組別名稱
        public DateTime Assembletime { get; set; }
        public string Assembleplace { get; set; } // 集合地點
        public string Leader { get; set; } // 負責人
        public string Line { get; set; } // line 群組
    }
}
