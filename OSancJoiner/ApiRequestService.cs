using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OSancJoiner
{
    class ApiRequestService
    {
        public readonly HttpClient client;
        private static readonly string baseUrl = "https://api.osanc.net/";

        public ApiRequestService(HttpClient Client)
        {
            client = Client;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> MakeGenericRequest(string route, string WebMethod, string data = "")
        {
            HttpResponseMessage response = await MakeRequest(route, WebMethod, data);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> MakeRequest(string route, string WebMethod, string data = "")
        {
            string url = baseUrl + route;
            HttpResponseMessage res;
            StringContent content = null;

            if (data != null)
                content = new StringContent(data, Encoding.UTF8, "application/json");
            try
            {
                res = WebMethod switch
                {
                    "POST" => await client.PostAsync(url, content),
                    _ => await client.GetAsync(url)
                };
                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
