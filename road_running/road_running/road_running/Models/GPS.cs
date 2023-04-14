using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace road_running.Models
{
    public class GPS
    {
        public static bool ifCheckin;
        public static bool IfCheckin
        {
            get { return ifCheckin; }
            set
            {
                ifCheckin = value;
                Console.WriteLine("ifcheckin: "+ value);
            }
        }

        public static Location Location;
        public static Location location
        {
            get { return Location; }
            set
            {
                Location = value;
                Console.WriteLine(value);
            }
        }
    }
}