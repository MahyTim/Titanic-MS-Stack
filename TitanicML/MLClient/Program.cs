using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MLClient
{
    class Program
    {
        static void Main(string[] args)
        {
            InvokeRequestResponseService().Wait();
        }

        //True --> First,Cumings, John Bradley (Florence Briggs Thayer),Mrs,female,38,2,PC 17599,71.28,C,Cherbourg,1
        //False -->Second	Turpin	 William John Robert (Dorothy Ann Wonnacott)	Mrs	female	27	2	11668	21.00	U	Southampton	0

        static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                    {
                "input1",
                new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                {
                "Survived", "false"
                },
                {
                "Class", "Second"
                },
                {
                "Title", "Mrs"
                },
                {
                "Sex", "female"
                },
                {
                "Age", "27"
                },
                {
                "FamilySize", "2"
                },
                {
                "FarePerPerson", "21.00"
                },
                {
                "Deck", "U"
                },
                {
                "Embarked", "Southampton"
                },
                {
                "IsMother", "0"
                },
                }
                }
                },
                },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "XXXX"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/daf57c5a6b2a4489a6c98ccddabea525/services/4448ca13fc404275bafc417894ed0fea/execute?api-version=2.0&format=swagger");

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}
