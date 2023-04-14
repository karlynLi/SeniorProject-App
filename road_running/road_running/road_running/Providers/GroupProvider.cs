using System;

using System.Collections.Generic;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public class GroupProvider
    {
        public class PHP_ActivityId
        {
            public string running_ID { get; set; }
        }
        public static async Task<List<Group>> GetGroupsAsync(string gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        PHP_ActivityId aid = new PHP_ActivityId();
                        aid.running_ID = gid;
                        string Json = JsonConvert.SerializeObject(aid, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        Console.WriteLine(strJson);
                        // 目標php檔
                        string FooUrl = $"http://running.im.ncnu.edu.tw/run_api/activeDetail_member.php";
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
                        List<Group> results = JsonConvert.DeserializeObject<List<Group>>(strResult);
                        Console.WriteLine("=======COunt=======" + results.Count);
                        for (int i = 0; i < results.Count; i++)
                        {
                            //for (int j=0; j<results[i].gift.Length; i++)
                            //{
                            //    Console.WriteLine(results[i].gift[j]);
                            //}
                            Console.WriteLine(results[i].running_ID);
                        }
                        if (results[0].group_name != null)
                        {
                            Console.WriteLine("update sucess!");
                            //updateText.Text = "success";
                            //return;
                        }
                        else
                        {
                            Console.WriteLine("update fail");
                            //updateText.Text = "fail";
                            //return;
                        }
                        return results;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
                    }
                }
            }
        }
    }
}
