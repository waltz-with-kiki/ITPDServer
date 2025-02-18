using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BinanceLibrary.Service.Models
{
    public class Job
    {
        [BsonId]
        public string jobId { get; set; } = "";
        [JsonIgnore]
        public string pairs { get; set; } = "";
        [JsonIgnore]
        public string? data { get; set; }
        public string status { get; set; } = "";
        public DateTime endTime { get; set; }
    }
}
