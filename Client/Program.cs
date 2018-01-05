using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        
        static void Main(string[] args)
        {
            RunDemoAsync().Wait();
            Console.ReadLine();
        }

        public static async Task RunDemoAsync()
        {
            var accessToken = await GetAccessTokenViaOwnerPasswordAsync();
            await CallApi(accessToken);
        }

       
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetAccessTokenViaOwnerPasswordAsync()
        {
            // discover endpoints from metadata
            var serverUrl = "http://localhost:5000";
            var disco = await DiscoveryClient.GetAsync(serverUrl);

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");


            if (tokenResponse.IsError)
            {
                Console.WriteLine("Error: ");
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);

            return tokenResponse.AccessToken;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private static async Task CallApi(string accessToken)
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            var response = await client.GetAsync("http://localhost:5001/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }

    }
}
