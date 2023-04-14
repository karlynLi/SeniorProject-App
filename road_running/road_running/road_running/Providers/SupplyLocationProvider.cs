using Newtonsoft.Json;
using road_running.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace road_running.Providers
{
    public static class SupplyLocationProvider
    {
        public static async Task<List<Route>> GetRouteListAsync(Member gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(gid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/mapRecord_m.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Route> RouteList = JsonConvert.DeserializeObject<List<Route>>(responseMessage);
                    Console.WriteLine(RouteList);
                    Console.WriteLine("==SupplyLocationProvider==");
                    Console.WriteLine(RouteList[0].Name);
                    //Console.WriteLine(GiftResult[1].Registraion_ID);
                    //Console.WriteLine(GiftResult[0].Photo);
                    return RouteList;
                }
            }

        }
        public static async Task<List<Route>> SupplyListAsync(Route gid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(gid, Formatting.Indented);
                    var data = "[" + json + "]";
                    Console.WriteLine(data);
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("http://running.im.ncnu.edu.tw/run_api/mapSupply_m.php", content);
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    //responseMessage = responseMessage.Replace("\uFEFF", "");
                    Console.WriteLine(responseMessage);
                    List<Route> SupplyResult = JsonConvert.DeserializeObject<List<Route>>(responseMessage);
                    Console.WriteLine(SupplyResult);
                    Console.WriteLine("==SupplyListProvider==");

                    //Console.WriteLine(GiftResult[1].Registraion_ID);
                    //Console.WriteLine(GiftResult[0].Photo);
                    if (SupplyResult != null)
                        return SupplyResult;
                    else
                    {
                        SupplyResult = null;
                        return SupplyResult;
                    }
                        
                }
            }
        }
    }
}