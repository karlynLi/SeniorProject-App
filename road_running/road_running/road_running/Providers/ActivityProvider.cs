using System;

using System.Collections.Generic;
using road_running.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class ActivityProvider
    {
		public static async Task<List<activity>> GetActivitysAsync()
		{
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        // 目標php檔
                        string FooUrl = $"http://running.im.ncnu.edu.tw/run_api/runningActive.php";
                        HttpResponseMessage response = null;

                        //設定相關網址內容
                        var fooFullUrl = $"{FooUrl}";

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        //using (var fooContent = new StringContent(StrQR, Encoding.UTF8, "application/json"))
                        //{
                        //    response = await client.PostAsync(fooFullUrl, fooContent);
                        //}

                        response = await client.GetAsync(fooFullUrl);
                        Console.WriteLine("response = " + response);
                        // PHP回傳值
                        string strResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("strResult = " + strResult);
                        // 反序列化
                        List<activity> activitys = JsonConvert.DeserializeObject<List<activity>>(strResult);
                        Console.WriteLine("=======COunt======="+activitys.Count);
                        for (int i=0; i<activitys.Count; i++)
                        {
                            Console.WriteLine(activitys[i].Name);
                            Console.WriteLine("==========="+activitys[i].ImageUrl);
                            //Console.WriteLine(activitys[i].photo);
                        }
                        if (activitys[0].Name != null)
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
                        return activitys;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("+++++++++++++exception++++++++++"+ex);
                        return null;
                    }
                }
            }
        }
    }
}
