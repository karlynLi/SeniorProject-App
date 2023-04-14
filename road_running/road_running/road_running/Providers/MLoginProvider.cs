using System;

using System.Collections.Generic;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class MLoginProvider
    {
        public static async Task<List<Member>> LoginAsync(Member mlogin)
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
                        HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/login_member.php", content);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        //responseMessage = responseMessage.Replace("\uFEFF", "");
                        Console.WriteLine(responseMessage);
                        List<Member> MLoginResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                        Console.WriteLine(MLoginResult);
                        //Console.WriteLine("==Provider==");
                        //Console.WriteLine(MLoginResult);
                        //Console.WriteLine(MLoginResult[0].Member_ID);
                        //Console.WriteLine(MLoginResult[0].Photo);
                        return MLoginResult;
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