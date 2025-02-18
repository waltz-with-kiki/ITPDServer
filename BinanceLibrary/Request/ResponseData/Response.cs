using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibrary.Request.ResponseData
{
    public class Response
    {
        public bool success { get; set; }
        public string errorMessage { get; set; } = "";
        public string response { get; set; } = "";
        public byte[]? byteResponse { get; set; }
        public HttpResponseHeaders? headers { get; set; }
    }
}
