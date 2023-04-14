using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace road_running.Providers
{
    public static class EnrollProvider
    {
        public static async Task<List<Member>> EnrollAsync(Member GetPass)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(GetPass, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/enroll_member.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> SEnrollResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(SEnrollResult);



                    return SEnrollResult;
                }
            }
        }
        public static async Task<List<Member>> CheckmailAsync(Member GetPass)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(GetPass, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/checkEmail.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> SEnrollResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(SEnrollResult);



                    return SEnrollResult;
                }
            }
        }
        public static async Task<List<Member>> ConfirmAsync(Member GetPass)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(GetPass, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/confirmEmail.php", content);
                    Console.WriteLine(response);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> SEnrollResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine("這邊是provider");
                    Console.WriteLine(SEnrollResult);



                    return SEnrollResult;
                }
            }
        }
    }
}