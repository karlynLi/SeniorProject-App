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
    public static class GiftProvider
    {
        public static async Task<List<CheckIn>> GiftAsync(Member gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(gid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/redeemGift.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<CheckIn> GiftResult = JsonConvert.DeserializeObject<List<CheckIn>>(responseMessage);
                    Console.WriteLine(GiftResult);
                    Console.WriteLine("==Provider==");
                    Console.WriteLine(GiftResult[0].Name);
                    Console.WriteLine(GiftResult[0].Registration_ID);
                    //Console.WriteLine(GiftResult[0].Photo);
                    return GiftResult;
                }
            }
        }
    }
}
