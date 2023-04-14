using road_running.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace road_running.Models
{
    public class Route : BaseViewModel
    {
        public string Running_ID { get; set; }
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Detail { get; set; }

        private bool hasbeen;
        public bool HasBeen
        {
            get { return hasbeen; }
            set
            {
                hasbeen = value;
                OnPropertyChanged();
            }
        }
        public string Supplies { get; set; }
        public string ans { get; set; }
    }
}