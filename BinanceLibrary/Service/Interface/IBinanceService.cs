using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceLibrary.Service.Models;

namespace BinanceLibrary.Service.Interface
{
    public interface IBinanceService
    {
        Task<string> StartDataLoadAsync(List<string> pairs, DateTime startDate, DateTime endDate);
        Task<Job> GetJobStatusAsync(string jobId);
    }
}
