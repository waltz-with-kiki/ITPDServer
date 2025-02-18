using BinanceLibrary.Request.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibrary.Request
{
    public class Request
    {
        private static HttpClient httpClient = new(new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2)
        })
        { Timeout = TimeSpan.FromSeconds(200) };

        public static async Task<Response> Send(HttpRequestMessage request)
        {
            Response result = new Response();

            try
            {
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                request.Dispose();
                string content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result.success = true;
                    result.response = content;
                    result.headers = response.Headers;
                }
                else
                {
                    result.success = false;
                    result.errorMessage = $"StatusCode: {response.StatusCode}\r\nMessage: {content}";
                }
            }
            catch (HttpRequestException ex)
            {
                result.success = false;
                result.errorMessage = ex.Message;

            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMessage = ex.Message;
            }

            return result;
        }
    }
}
