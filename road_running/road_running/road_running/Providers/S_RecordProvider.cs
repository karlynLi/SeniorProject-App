using Newtonsoft.Json;
using road_running.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class S_RecordProvider
    {
        public class transValue
        {
            public string Staff_ID;
        }
        public static async Task<List<S_Record>> GetRecordAsync(string sid)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        transValue info = new transValue();
                        info.Staff_ID = sid;
                        // 要傳的物件及網址
                        var api = $"http://running.im.ncnu.edu.tw/run_api/activeRecord_staff.php";
                        var apiUpload = $"{api}";
                        var data = JsonConvert.SerializeObject(info);
                        Console.WriteLine("data = " + data);

                        HttpContent content = new StringContent("[" + data + "]", Encoding.UTF8, "application/json");
                        Console.WriteLine("content = " + content);
                        HttpResponseMessage response = await client.PostAsync(apiUpload, content);
                        Console.WriteLine("response = " + response);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("responseMessage = " + responseMessage);
                        List<S_Record> records = JsonConvert.DeserializeObject<List<S_Record>>(responseMessage);

                        for (int i = 0; i < records.Count; i++)
                        {
                            Console.WriteLine("========= S_RecordProvider.GetRecordAsync() ================");
                            Console.WriteLine("Workgroup_ID = " + records[i].Workgroup_ID);
                            Console.WriteLine("Running_ID = " + records[i].Running_ID);
                            Console.WriteLine("Name = " + records[i].Name);
                            Console.WriteLine("Date = " + records[i].Date);
                            Console.WriteLine("status = " + records[i].Status);
                        }
                        return records;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine(ex.InnerException.Message);
                        return null;
                    }
                }
            }
        }
    }
}
