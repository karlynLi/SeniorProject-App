using System;
using System.Collections.Generic;
using System.Text;

namespace road_running.Models
{
    public class GoogleUser
    {
        public string Google_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }
    }

    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
