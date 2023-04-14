using System;
using System.Collections.Generic;

namespace road_running.Models
{
    public class GiftSize
    {
        public GiftSize()
        {
            //SelectedSize = sizeList[0];
        }
        public string id { get; set; }
        public string name { get; set; }
        public string[] sizeList { get; set; }
        public string SelectedSize { get; set; }

        public string showstring { get { return name + "(" + SelectedSize + ")"; } }

    }
}
