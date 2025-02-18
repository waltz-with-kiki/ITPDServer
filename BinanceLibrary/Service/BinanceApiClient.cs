using BinanceLibrary.Request.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BinanceLibrary.Service
{
    public class BinanceApiClient
    {
        public async Task<Response> GetHistoricalDataAsync(string pair, DateTime startDate, DateTime endDate)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://api.binance.com/api/v3/klines?symbol={pair}&interval=1d&startTime={((DateTimeOffset)startDate).ToUnixTimeMilliseconds()}&endTime={((DateTimeOffset)endDate).ToUnixTimeMilliseconds()}");
            Response responseData = await Request.Request.Send(request);

            return responseData;
        }
    }
}
