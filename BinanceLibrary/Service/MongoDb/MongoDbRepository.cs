using BinanceLibrary.Service.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibrary.Service.MongoDb
{
    public class MongoDbRepository
    {
        private readonly IMongoCollection<Job> _jobsCollection;

        public MongoDbRepository(IOptions<MongoDbSettings> settings, ILogger<MongoDbRepository> logger)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _jobsCollection = database.GetCollection<Job>("jobs");
        }

        public async Task SaveJobStatusAsync(string jobId, string status)
        {
            await _jobsCollection.InsertOneAsync(new Job { jobId = jobId, status = status });
        }

        public async Task UpdateJobStatusAsync(string jobId, string pairs, string data, string status)
        {
            var filter = Builders<Job>.Filter.Eq(j => j.jobId, jobId);
            var update = Builders<Job>.Update.Set(j => j.status, status).Set(j => j.endTime, DateTime.UtcNow).Set(j => j.pairs, pairs).Set(j => j.data, data);
            await _jobsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<Job> GetJobStatusAsync(string jobId)
        {
            var job = await _jobsCollection.Find(j => j.jobId == jobId).FirstOrDefaultAsync();
            return job;
        }

    }
}
