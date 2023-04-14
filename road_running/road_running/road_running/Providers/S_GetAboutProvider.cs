using System;

using System.Collections.Generic;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class S_GetAboutProvider
    {
        public static async Task<List<Staff>> SGetInfoAsync(Staff mid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(mid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/getInfo_staff.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Staff> InfoResult = JsonConvert.DeserializeObject<List<Staff>>(responseMessage);
                    Console.WriteLine(InfoResult);
                    Console.WriteLine("==Provider==");
                    Console.WriteLine(InfoResult[0].Staff_ID);
                    Console.WriteLine(InfoResult[0].Photo);
                    return InfoResult;
                }
            }
        }
    }
}