using BinanceLibrary.Service.Interface;
using ITPDServer.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ITPDServer.Controllers
{
    [ApiController]
    [Route("api/historical-data")]
    public class BinanceController : ControllerBase
    {
        private readonly IBinanceService _binanceService;
        public BinanceController(IBinanceService binanceService, ILogger<BinanceController> logger)
        {
            _binanceService = binanceService;
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadPairs([Required][FromBody] LoadPairsRequest request, [Required][FromHeader(Name = "X-Token")] string authToken)
        {
            if (authToken != Immutable.authToken)
             return Unauthorized(authToken);

            if (request.pairs.Count == 0) return BadRequest();
            if (!DateTime.TryParse(request.startDate, out var startDateTime) || !DateTime.TryParse(request.endDate, out var endDateTime)) return BadRequest("Invalid date format.");

            var result = await _binanceService.StartDataLoadAsync(request.pairs, startDateTime, endDateTime);
            return Ok(result);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetJobStatus([FromQuery] string jobId, [Required][FromHeader(Name = "X-Token")] string authToken)
        {
            if (authToken != Immutable.authToken)
            return Unauthorized(authToken);

            var jobStatus = await _binanceService.GetJobStatusAsync(jobId);
            return Ok(jobStatus);
        }

        public record LoadPairsRequest(List<string> pairs, string startDate, string endDate);
    }
}
