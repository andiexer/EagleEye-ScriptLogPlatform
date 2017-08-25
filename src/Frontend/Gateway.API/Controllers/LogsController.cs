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
using Microsoft.Extensions.Caching.Distributed;

namespace EESLP.Frontend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class LogsController : BaseController
    {
        public LogsController(ILogger<HostsController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache) : base(logger, mapper, http, apiOptions, cache)
        {
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
                return BaseGet<IEnumerable<LogViewModel>>(_apiOptions.LoggingApiUrl + "/api/Logs/latest/" + amount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
