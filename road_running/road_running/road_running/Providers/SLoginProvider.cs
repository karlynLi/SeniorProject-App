using System;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace road_running.Providers
{
    public static class SLoginProvider
    {
        public static async Task<List<Staff>> SLoginAsync(Staff mlogin)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    using (HttpClient client = new HttpClient(handler))
                    {
                        var json = JsonConvert.SerializeObject(mlogin, Formatting.Indented);
                        var data = "[" + json + "]";
                        Console.WriteLine(data);
                        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/login_staff.php", content);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        //responseMessage = responseMessage.Replace("\uFEFF", "");
                        Console.WriteLine(responseMessage);
                        List<Staff> SLoginResult = JsonConvert.DeserializeObject<List<Staff>>(responseMessage);
                        Console.WriteLine(SLoginResult);
                        //Console.WriteLine("==Provider==");
                        //Console.WriteLine(MLoginResult);
                        //Console.WriteLine(MLoginResult[0].Member_ID);
                        //Console.WriteLine(MLoginResult[0].Photo);
                        return SLoginResult;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}