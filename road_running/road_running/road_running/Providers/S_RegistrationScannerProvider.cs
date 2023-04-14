using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using road_running.Models;

namespace road_running.Providers
{
    public class S_RegistrationScannerProvider
    {
        public class PHP
        {
            public string registration_ID { get; set; } // 報名編號
            public string staff_ID { get; set; } // 員工ID
        }

        public static async Task<S_RegistrationScanner> GetRegistrationInfoAsync(string registration_id, string sid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        PHP giftExchange = new PHP();
                        giftExchange.registration_ID = registration_id;
                        giftExchange.staff_ID = sid;
                        Console.WriteLine("registrationregistration=====" + giftExchange.registration_ID);
                        string Json = JsonConvert.SerializeObject(giftExchange, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        Console.WriteLine(strJson);
                        // 目標php檔
                        string FooUrl = $"http://running.im.ncnu.edu.tw/run_api/checkinInfo_staff.php";
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
                        S_RegistrationScanner results = JsonConvert.DeserializeObject<S_RegistrationScanner>(strResult);
                        if (results.checkin_time != null)
                        {
                            Console.WriteLine("您已報到過啦");
                        }
                        else
                        {
                            if (results.group_name != null)
                            {
                                Console.WriteLine("您是" + results.running_name + results.group_name);
                            }
                            else
                            {
                                Console.WriteLine("查無報名資訊");
                            }
                        }
                        return results;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        S_RegistrationScanner error = new S_RegistrationScanner();
                        return error;
                    }
                }
            }
        }

        public class S_PHP
        {
            public string staff_ID { get; set; }
            public string workgroup_ID { get; set; }
        }

        public static async Task<S_RegistrationScanner> S_GetRegistrationInfoAsync(string sid, string wid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        S_PHP info = new S_PHP();
                        info.staff_ID = sid;
                        info.workgroup_ID = wid;
                        string Json = JsonConvert.SerializeObject(info, Formatting.Indented);
                        string strJson = "[" + Json + "]";
                        Console.WriteLine(strJson);
                        // 目標php檔
                        string FooUrl = $"http://running.im.ncnu.edu.tw/run_api/checkStaffStatus.php";
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
                        S_RegistrationScanner results = JsonConvert.DeserializeObject<S_RegistrationScanner>(strResult);
                        if (results.checkin_time != null)
                        {
                            Console.WriteLine("您已報到過啦");
                        }
                        else
                        {
                            if (results.group_name != null)
                            {
                                Console.WriteLine("您是" + results.running_name + results.group_name);
                            }
                            else
                            {
                                Console.WriteLine("查無報名資訊");
                            }
                        }
                        return results;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        S_RegistrationScanner error = new S_RegistrationScanner();
                        return error;
                    }
                }
            }
        }
    }
}
