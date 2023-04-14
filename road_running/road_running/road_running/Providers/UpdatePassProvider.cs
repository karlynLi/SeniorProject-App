using System;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class UpdatePassProvider
    {
        public static async Task<Member> UpdatePassAsync(Member NewPass)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(NewPass, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/updatePwd_member.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    Member UpdatePass = JsonConvert.DeserializeObject<Member>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(UpdatePass);



                    return UpdatePass;
                }
            }
        }
    }
}