using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using road_running.Models;

namespace road_running.Providers
{
    public class RegistrationProvider
    {
        public class PHP
        {
            public string member_ID { get; set; } // 會員編號
            public string running_ID { get; set; } // 活動編號
            public string group_name { get; set; } // 組別名稱
            public string registration_time { get; set; } // 報名時間
            public List<GiftSize> gift_size { get; set; }
        }
        public class result
        {
            public string ans { get; set; }
            public string group_name { get; set; }
        }

        public static async Task<string> UpdateRegistrarionAsync(string uid, Group group)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        PHP registration = new PHP();
                        registration.member_ID = uid;
                        registration.running_ID = group.running_ID;
                        registration.group_name = group.group_name;
                        registration.gift_size = group.giftSize;
                        registration.registration_time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        Console.WriteLine("TIMETIMETIMETIMETIMETIME=====" + registration.registration_time);
                        string Json = JsonConvert.SerializeObject(registration, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        Console.WriteLine(strJson);
                        // 目標php檔
                        string FooUrl = $"http://running.im.ncnu.edu.tw/run_api/confirmInformation.php";
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
                        Console.WriteLine("=======COunt=======" + results.ans);
                        if (results.ans == "yes")
                        {
                            Console.WriteLine("update sucess!");
                            return "yes";
                        }
                        else
                        {
                            Console.WriteLine("update fail");
                            //updateText.Text = "fail";
                            //return;
                            return results.group_name;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return "error";
                    }
                }
            }
        }
    }
}
