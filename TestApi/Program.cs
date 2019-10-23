using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApi
{
    internal class Program
    {
        // SET THE BASE ADDRESS TO THE SampleWebApp_1_x_x local host URL.  Similar to as seen.
        // From SampleWebApp_1_x_x PROPERTIES DEBUG TAB
        // Or Copy from browser
        private const string baseUrl = @"https://localhost:44376/"; 

        private static void Main(string[] args)
        {
            // User is defined in SampleWebApp_1_x_x
            var username = "{internal user}";
            var password = "{internal user password}";

            Console.WriteLine(string.Empty);
            Console.WriteLine(PostPublicApi().Result);
            Console.WriteLine(string.Empty);

            Console.WriteLine(string.Empty);
            Console.WriteLine(PostPrivateXXXXApi("0000", username, password).Result);
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine(PostPrivateXXXXApi("0001", username, password).Result);
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine(PostPrivateXXXXApi("0002", username, password).Result);
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine(PostPrivateXXXXApi("0003", username, password).Result);
            Console.WriteLine(string.Empty);

        }

        private static async Task<string> PostPublicApi()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            var response = await httpClient.PostAsync(@"api/PublicApi", null, CancellationToken.None);
            return await ProssessResponse("Public", response);
        }

        private static async Task<string> PostPrivateXXXXApi(string xxxx, string username, string password)
        {
            //var credentials = $"{username}{password}";
            //credentials = Convert.ToBase64String(Encoding.Default.GetBytes(credentials));

            var credentials = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}"));

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));

            var response = await httpClient.PostAsync($"api/Private{xxxx}Api", null, CancellationToken.None);
            return await ProssessResponse(xxxx, response);
        }


        private static async Task<string> ProssessResponse(string xxxx, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                return $"{xxxx} - {response.StatusCode} {response.ReasonPhrase} {str}";
            }
            else
            {
                var str = await response.Content.ReadAsStringAsync();
                return $"ERROR: {xxxx} - {response.StatusCode} {response.ReasonPhrase} {str}";
            }
        }
    }
}