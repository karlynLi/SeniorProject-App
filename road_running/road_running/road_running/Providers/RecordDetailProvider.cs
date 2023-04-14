using Newtonsoft.Json;
using road_running.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace road_running.Providers
{
    public static class RecordDetailProvider
    {
        public class transValue
        {
            public string Registration_ID;
            public string Running_ID;
        }
        public static async Task<List<RecordDetail>> GetRecordDetailAsync(string run_id, string regis_id)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        transValue info = new transValue();
                        info.Running_ID = run_id;
                        info.Registration_ID = regis_id;
                        // 要傳的物件及網址
                        var api = $"http://running.im.ncnu.edu.tw/run_api/activeRecord_detail_member.php";
                        var apiUpload = $"{api}";
                        var data = JsonConvert.SerializeObject(info);
                        Console.WriteLine("data = " + data);

                        HttpContent content = new StringContent("[" + data + "]", Encoding.UTF8, "application/json");
                        Console.WriteLine("content = " + content);
                        HttpResponseMessage response = await client.PostAsync(apiUpload, content);
                        Console.WriteLine("response = " + response);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("responseMessage = " + responseMessage);
                        List<RecordDetail> details = JsonConvert.DeserializeObject<List<RecordDetail>>(responseMessage);

                        for (int i = 0; i < details.Count; i++)
                        {
                            Console.WriteLine("========= RecordDetailProvider.GetRecordDetailAsync() ================");
                            Console.WriteLine("Name = " + details[i].Name);
                            Console.WriteLine("Group_name = " + details[i].Group_name);
                            Console.WriteLine("Place = " + details[i].Place);
                            Console.WriteLine("Time = " + details[i].Time);
                            Console.WriteLine("Grade = " + details[i].Grade);
                            Console.WriteLine("Complete_time = " + details[i].Complete_time);
                        }
                        return details;
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
