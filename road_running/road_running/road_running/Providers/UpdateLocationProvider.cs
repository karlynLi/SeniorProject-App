using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace road_running.Providers
{
    public static class UpdateLocationProvider
    {
        // 上傳類型
        public class PHP
        {
            public string member_ID;
            public string running_ID;
            public string beacon_ID;
            public string time;
        }
        // 接收類型
        public class result
        {
            public string ans;
        }
        public static async void GetAnsAsync(string mID, string rID, string bID)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        PHP upload = new PHP
                        {
                            member_ID = mID,
                            running_ID = rID,
                            beacon_ID = bID,
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        string Json = JsonConvert.SerializeObject(upload, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        // 目標php檔
                        string FooUrl = $"http://running.im.ncnu.edu.tw/run_api/uploadLocation.php";
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
                        Console.WriteLine("response = " + response);
                        // PHP回傳值
                        string strResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("strResult = " + strResult);
                        // 反序列化
                        result yesorno = JsonConvert.DeserializeObject<result>(strResult);
                        Console.WriteLine("=======COunt=======" + yesorno.ans);
                        //return yesorno.ans;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("+++++++++++++exception++++++++++" + ex);
                        //return null;
                    }
                }
            }
        }
    }
}
