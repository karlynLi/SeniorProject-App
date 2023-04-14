using Newtonsoft.Json;
using road_running.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace road_running.Providers
{
    public static class MapsProvider
    {
        public class transValue
        {
            public string Member_ID;
            public string Running_ID;
        }
        // 路線清單
        public static async Task<List<Route>> GetRouteListAsync(string mid)
        {
            string responseMessage, url;
            transValue info = new transValue()
            {
                Member_ID = mid
            };
            url = "http://running.im.ncnu.edu.tw/run_api/mapRecord_m.php";
            responseMessage = await GetDataAsync(url, info);
            List<Route> routeList = JsonConvert.DeserializeObject<List<Route>>(responseMessage);
            return routeList;
        }
        // 路線資訊
        public static async Task<List<Route>> GetRouteInfoAsync(string mid, string rid)
        {
            string responseMessage, url;
            transValue info = new transValue()
            {
                Member_ID = mid,
                Running_ID = rid
            };
            url = "http://running.im.ncnu.edu.tw/run_api/mapRoutedetail.php";
            responseMessage = await GetDataAsync(url, info);
            List<Route> routeInfo = JsonConvert.DeserializeObject<List<Route>>(responseMessage);
            return routeInfo;
        }

        // 抓API Data Code
        public static async Task<string> GetDataAsync(string url, transValue info) 
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        // 要傳的物件及網址
                        var api = $"{url}";
                        var apiUpload = $"{api}";
                        var data = JsonConvert.SerializeObject(info);
                        HttpContent content = new StringContent("[" + data + "]", Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(apiUpload, content);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        return responseMessage;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
                    }
                }
            }
        }

        // 上傳位置紀錄
        public class Upload
        {
            public string Member_ID;
            public string Running_ID;
            public double Latitude;
            public double Longitude;
            public string ans { get; set; }
        }
        public static async Task<string> PostPositionAsync(string mid, string rid, double lng, double lat)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        Upload upload = new Upload()
                        {
                            Member_ID = mid,
                            Running_ID = rid,
                            Latitude = lat,
                            Longitude = lng
                        };
                        var api = $"http://running.im.ncnu.edu.tw/run_api/mapLocation_m.php";
                        var apiUpload = $"{api}";
                        var data = JsonConvert.SerializeObject(upload);
                        Console.WriteLine(data);
                        HttpContent content = new StringContent("[" + data + "]", Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(apiUpload, content);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("responseMessage"+responseMessage);
                        Upload uploadResult = JsonConvert.DeserializeObject<Upload>(responseMessage);

                        if (uploadResult.ans == "Member_ID,Running_ID,Latitude,Longitude can't empty")
                        {
                            Console.WriteLine(uploadResult.ans);
                            return uploadResult.ans;
                        }
                        else if (uploadResult.ans == "noRegistration")
                        {
                            Console.WriteLine(mid + " 未報名活動 " + rid);
                            return uploadResult.ans;
                        }
                        else if (uploadResult.ans == "unperiod")
                        {
                            Console.WriteLine("非活動期間");
                            return uploadResult.ans;
                        }
                        else if (uploadResult.ans == "no")
                        {
                            Console.WriteLine("upload location failed");
                            return uploadResult.ans;
                        }
                        else
                        {
                            Console.WriteLine("upload location success");
                            return uploadResult.ans;
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

        
    }
}