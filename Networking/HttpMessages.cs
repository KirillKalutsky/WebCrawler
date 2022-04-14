using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Networking
{
    
    public static class HttpMessages
    {
        private static HttpClient httpClient = new();
        public static async Task<HttpResponseMessage> TrySendAsync(string address, HttpMethod httpMethod, string content = null)
        {
            HttpResponseMessage responseRes;
            try
            {
                var request = new HttpRequestMessage(httpMethod, address);
                if(content!=null)
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                responseRes = await httpClient.SendAsync(request);
            }
            catch (HttpRequestException exp)
            {
                responseRes = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) { ReasonPhrase = exp.Message };
            }

            return responseRes;
        }

        public static async Task<Tuple<HttpResponseMessage, string>> LoadPageAsync(string url, int numberOfAttempts = 5)
        {
            HttpResponseMessage result = null;
            for (var i = 0; i < numberOfAttempts; i++)
            {
                try
                {
                    result = await TrySendAsync(url, HttpMethod.Get);
                }
                catch
                {
                    result = new HttpResponseMessage();
                    result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
                if (result.IsSuccessStatusCode && numberOfAttempts == 0)
                    break;
                    
                await Task.Delay(3);
            }
            return Tuple.Create(result, url);
        }
    }
}
