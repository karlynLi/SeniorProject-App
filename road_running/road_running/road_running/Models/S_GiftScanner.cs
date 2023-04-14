using System;
namespace road_running.Models
{
    public class S_GiftScanner
    {
        public S_GiftScanner()
        {
        }

        public string exchange_time { get; set; }
        public string[] gift { get; set; }
        public string status { get; set; }
        public string GiftStr()
        {
            string str = "";
            if (gift != null)
            {
                for (int i = 0; i < gift.Length; i=i+2)
                {
                    str += gift[i] + "(" + gift[i+1] + ")" + "\n";
                }
            }
            if (str == "")
            {
                return "無禮品";
            }
            else
            {
                return str;
            }
        }
    }
}
