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
    public class S_WorkContentProvider
    {
        public class transValue
        {
            public string Workgroup_ID;
            public string Running_ID;
        }
        public static async Task<List<S_WorkContent>> GetWorkContentAsync(string run_id, string workgroup_id)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        transValue info = new transValue();
                        info.Workgroup_ID = workgroup_id;
                        info.Running_ID = run_id;
                        // 要傳的物件及網址
                        var api = $"http://running.im.ncnu.edu.tw/run_api/workDetail.php";
                        var apiUpload = $"{api}";
                        var data = JsonConvert.SerializeObject(info);
                        Console.WriteLine("data = " + data);

                        HttpContent content = new StringContent("[" + data + "]", Encoding.UTF8, "application/json");
                        Console.WriteLine("content = " + content);
                        HttpResponseMessage response = await client.PostAsync(apiUpload, content);
                        Console.WriteLine("response = " + response);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("responseMessage = " + responseMessage);
                        List<S_WorkContent> workcontent = JsonConvert.DeserializeObject<List<S_WorkContent>>(responseMessage);

                        for (int i = 0; i < workcontent.Count; i++)
                        {
                            Console.WriteLine("========= S_WorkContentProvider.GetWorkContentAsync() ================");
                            Console.WriteLine("Content = " + workcontent[i].Content);
                            Console.WriteLine("Time = " + workcontent[i].Time);
                            Console.WriteLine("Place = " + workcontent[i].Place);
                            Console.WriteLine();

                        }
                        return workcontent;
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