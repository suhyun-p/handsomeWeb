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

namespace Feedback.ContentsFeeder.AzureWebservice
{
	
	public class StringTable
	{
		public string[] ColumnNames { get; set; }
		public string[,] Values { get; set; }
	}

	public class AzureMLWsHelper
	{
		public static async Task InvokeRequestResponseService()
		{
			using (var client = new HttpClient())
			{
				var scoreRequest = new
				{
					Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"SiteId", "InputChannel", "ImageCount", "CountNPM", "RateOfValid", "QualityScore"},
                                Values = new string[,] {  { "1", "1", "0", "10", "75", "0" },  { "2", "2", "1", "9", "80", "0" },  }
                            }
                        },
                    },
					GlobalParameters = new Dictionary<string, string>() {
					 { "Append score columns to output", "True" },
					}
				};

				const string apiKey = "6E8XaVKt3srShK//vHXbqKbOu/MyVI5QqqI8m4jpx6jmU/Jny5UeESHJITPOfyRDMCXxDh+RCqS34FDqw4AXGQ=="; // Replace this with the API key for the web service
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

				client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/e137e7634e394e76a72808593587a3f1/services/78a9328325254827bd4e566482a92a92/execute?api-version=2.0&details=true");

				// WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
				// One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
				// For instance, replace code such as:
				//      result = await DoSomeTask()
				// with the following:
				//      result = await DoSomeTask().ConfigureAwait(false)


				HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

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
