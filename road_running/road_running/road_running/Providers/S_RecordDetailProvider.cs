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
    public class S_RecordDetailProvider
    {
        public class transValue
        {
            public string Staff_ID;
            public string workgroup_ID;
            public string running_ID;
        }
        public static async Task<List<S_RecordDetail>> GetRecordDetailAsync(string sid, string run_id, string workgroup_id)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        transValue info = new transValue();
                        info.Staff_ID = sid;
                        info.running_ID = run_id;
                        info.workgroup_ID = workgroup_id;
                        // 要傳的物件及網址
                        var api = $"http://running.im.ncnu.edu.tw/run_api/activeRecord_detail_staff.php";
                        var apiUpload = $"{api}";
                        var data = JsonConvert.SerializeObject(info);
                        Console.WriteLine("data = " + data);

                        HttpContent content = new StringContent("[" + data + "]", Encoding.UTF8, "application/json");
                        Console.WriteLine("content = " + content);
                        HttpResponseMessage response = await client.PostAsync(apiUpload, content);
                        Console.WriteLine("response = " + response);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("responseMessage = " + responseMessage);
                        List<S_RecordDetail> details = JsonConvert.DeserializeObject<List<S_RecordDetail>>(responseMessage);

                        for (int i = 0; i < details.Count; i++)
                        {
                            Console.WriteLine("========= S_RecordDetailProvider.GetRecordAsync() ================");
                            Console.WriteLine("Name = " + details[i].Name);
                            Console.WriteLine("Work_name = " + details[i].Work_name);
                            Console.WriteLine("Assembleplace = " + details[i].Assembleplace);
                            Console.WriteLine("Assembletime = " + details[i].Assembletime);
                            Console.WriteLine("Leader = " + details[i].Leader);
                            Console.WriteLine("Line = " + details[i].Line);
                            Console.WriteLine();
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