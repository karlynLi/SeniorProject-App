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
    public static class ForgetPwdProvider
    {
        public static async Task<List<Member>> ForgetPwdAsync(Member gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(gid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/forgetPwd_member.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> PwdResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine(PwdResult);
                    Console.WriteLine("==Provider==");
                    Console.WriteLine(PwdResult);
                    //Console.WriteLine(GiftResult[1].Registraion_ID);
                    //Console.WriteLine(GiftResult[0].Photo);
                    return PwdResult;
                }
            }
        }
        public static async Task<List<Member>> AuthenticationAsync(Member gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(gid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/checkCaptcha.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> AuthResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine("==Provider==");
                    Console.WriteLine(AuthResult);
                    //Console.WriteLine(GiftResult[1].Registraion_ID);
                    //Console.WriteLine(GiftResult[0].Photo);
                    return AuthResult;
                }
            }
        }
        public static async Task<List<Member>> ResetAsync(Member gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(gid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/resetPwd.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Member> ResetResult = JsonConvert.DeserializeObject<List<Member>>(responseMessage);
                    Console.WriteLine("==Provider==");
                    Console.WriteLine(ResetResult);
                    //Console.WriteLine(GiftResult[1].Registraion_ID);
                    //Console.WriteLine(GiftResult[0].Photo);
                    return ResetResult;
                }
            }
        }
    }
}