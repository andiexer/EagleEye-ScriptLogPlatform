using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Infrastructure.Options;
using EESLP.Frontend.Gateway.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogger<LogsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;

        public LogsController(ILogger<LogsController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions.Value;
        }

        /// <summary>
        /// gets a list of the latest log entries
        /// </summary>
        /// <param name="amount">amount of log entries</param>
        /// <returns>list of latest log entries</returns>
        /// <response code="200">returns a list of the latest log entries</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("latest/{amount}")]
        [ProducesResponseType(typeof(IEnumerable<LogViewModel>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetLatestLogs(int amount)
        {
            try
            {
                return Ok(_http.GetAsync<IEnumerable<LogViewModel>>(_apiOptions.LoggingApiUrl + "/api/Logs/latest/" + amount).Result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
