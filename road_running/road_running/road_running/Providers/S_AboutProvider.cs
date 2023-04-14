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

namespace road_running.Providers
{
    public static class S_AboutProvider
    {
        public static async Task<List<Staff>> SAboutAsync(Staff GETINFO)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(GETINFO, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/updateInfo_staff.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Staff> UpdateResult = JsonConvert.DeserializeObject<List<Staff>>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(UpdateResult);



                    return UpdateResult;
                }
            }
        }
    }
}