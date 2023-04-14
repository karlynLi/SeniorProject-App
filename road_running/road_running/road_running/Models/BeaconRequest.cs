using System;
namespace road_running.Models
{
    public class BeaconRequest
    {
        public BeaconRequest()
        {
        }
        public string running_ID { get; set; }
        public string group_name { get; set; }
        public string[] beaconList { get; set; }
        public string ifCheckin { get; set; }
    }
}
