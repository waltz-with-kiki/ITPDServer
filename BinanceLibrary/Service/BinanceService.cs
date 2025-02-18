using BinanceLibrary.Service.Interface;
using BinanceLibrary.Service.Models;
using BinanceLibrary.Service.MongoDb;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibrary.Service
{
    public class BinanceService : IBinanceService
    {
        private readonly BinanceApiClient _binanceApi;
        private readonly MongoDbRepository _mongoRep;
        private readonly ILogger<BinanceService> _logger;
        public BinanceService(BinanceApiClient binanceApiClient, MongoDbRepository mongoDb, ILogger<BinanceService> logger)
        {
            _binanceApi = binanceApiClient;
            _mongoRep = mongoDb;
            _logger = logger;
        }
        public async Task<Job> GetJobStatusAsync(string jobId)
        {
            try
            {
                return await _mongoRep.GetJobStatusAsync(jobId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetJobStatus method error {jobId}", jobId);
                throw;
            }
        }

        public async Task<string> StartDataLoadAsync(List<string> pairs, DateTime startDate, DateTime endDate)
        {
            try
            {
                var jobId = Guid.NewGuid().ToString();
                await _mongoRep.SaveJobStatusAsync(jobId, "В обработке");

                Task.Run(() => RunDataLoadInBackground(jobId, pairs, startDate, endDate));

                return jobId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StartDataLoad method error");
                throw;
            }
        }

            private async Task RunDataLoadInBackground(string jobId, List<string> pairs, DateTime startDate, DateTime endDate)
            {
                try
                {
                    StringBuilder pairsinfo = new StringBuilder();
                    StringBuilder data = new StringBuilder();
                    string status = "Завершено";

                    foreach (var item in pairs)
                    {
                        var pairData = await _binanceApi.GetHistoricalDataAsync(item, startDate, endDate);
                        if (pairData.success)
                        {
                            pairsinfo.Append(item).Append(" ");
                            data.Append(pairData.response);
                        }
                        else
                        {
                            status = "Ошибка";
                        }
                    }
                    await _mongoRep.UpdateJobStatusAsync(jobId, pairsinfo.ToString(), data.ToString(), status);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "RunDataLoadInBackground error");
                    await _mongoRep.UpdateJobStatusAsync(jobId, "", "", "Ошибка");
                }
            }
    }
}

