using System;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class S_UpdatePassProvider
    {
        public static async Task<Staff> S_UpdatePassAsync(Staff NewPass)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(NewPass, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/updatePwd_staff.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    Staff UpdatePass = JsonConvert.DeserializeObject<Staff>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(UpdatePass);



                    return UpdatePass;
                }
            }
        }
    }
}