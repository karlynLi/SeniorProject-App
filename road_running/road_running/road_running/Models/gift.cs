using System;
namespace road_running.Models
{
    public class gift
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public string ImageURL
        {
            get
            {
                Console.WriteLine("http://running.im.ncnu.edu.tw/running/files/photo/" + Image);
                return "http://running.im.ncnu.edu.tw/running/files/photo/" + Image;
            }
        }
    }
}
