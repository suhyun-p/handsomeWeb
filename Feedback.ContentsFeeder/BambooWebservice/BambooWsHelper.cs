using System;
using System.Collections.Generic;
using System.IO;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Feedback.ContentsFeeder.BambooWebservice
{
    public class BambooWsHelper
    {
        public static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var orderNo = "1223317364";

                client.BaseAddress = new Uri("http://bamboo.auction.co.kr/Feedback/Feedback/GetFeedbackSecondDetail?orderno=");

                HttpResponseMessage response = await client.PostAsJsonAsync("", orderNo);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}
