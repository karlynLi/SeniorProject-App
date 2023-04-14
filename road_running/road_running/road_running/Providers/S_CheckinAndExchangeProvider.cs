using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using road_running.Models;

namespace road_running.Providers
{
    public class S_CheckinAndExchangeProvider
    {
        public class PHP
        {
            public string staff_ID { get; set; }
            public string registration_ID { get; set; }
            public string time { get; set; }
        }
        public class S_PHP
        {
            public string workgroup_ID { get; set; }
            public string staff_ID { get; set; }
        }
        public class result
        {
            public string ans { get; set; }
        }
        public static async Task<bool> UpdateInfoAsync(string staff_id, string registration_id, string url)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        PHP giftExchange = new PHP();
                        giftExchange.registration_ID = registration_id;
                        giftExchange.staff_ID = staff_id;
                        giftExchange.time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        Console.WriteLine("registrationregistration=====" + giftExchange.registration_ID);
                        string Json = JsonConvert.SerializeObject(giftExchange, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        Console.WriteLine(strJson);
                        // 目標php檔
                        string FooUrl = url;
                        HttpResponseMessage response = null;

                        //設定相關網址內容
                        var fooFullUrl = $"{FooUrl}";

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        using (var fooContent = new StringContent(strJson, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(fooFullUrl, fooContent);
                        }

                        //response = await client.GetAsync(fooFullUrl);
                        Console.WriteLine("response = " + response);
                        // PHP回傳值
                        string strResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("strResult = " + strResult);
                        // 反序列化
                        result results = JsonConvert.DeserializeObject<result>(strResult);

                        if (results.ans == "yes")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
        }

        public static async Task<bool> S_UpdateInfoAsync(string staff_id, string work_id, string url)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        S_PHP info = new S_PHP();
                        info.staff_ID = staff_id;
                        info.workgroup_ID = work_id;
                        //info.time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        string Json = JsonConvert.SerializeObject(info, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        Console.WriteLine(strJson);
                        // 目標php檔
                        string FooUrl = url;
                        HttpResponseMessage response = null;

                        //設定相關網址內容
                        var fooFullUrl = $"{FooUrl}";

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        using (var fooContent = new StringContent(strJson, Encoding.UTF8, "application/json"))
                        {
                            response = await client.PostAsync(fooFullUrl, fooContent);
                        }

                        //response = await client.GetAsync(fooFullUrl);
                        Console.WriteLine("response = " + response);
                        // PHP回傳值
                        string strResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("strResult = " + strResult);
                        // 反序列化
                        result results = JsonConvert.DeserializeObject<result>(strResult);

                        if (results.ans == "yes")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
        }
    }
}
