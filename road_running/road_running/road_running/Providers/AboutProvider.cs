using System;

using System.Collections.Generic;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class AboutProvider
    {
        public static async Task<List<Member>> GetAboutAsync(Member GETINFO)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(GETINFO, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/updateInfo_member.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> UpdateResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(UpdateResult);
                    
                    
   
                    return UpdateResult;
                }
            }
        }
    }
}