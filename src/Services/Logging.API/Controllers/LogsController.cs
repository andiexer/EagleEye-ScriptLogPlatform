using AutoMapper;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Services;
using EESLP.Services.Logging.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EESLP.Services.Logging.API.Controllers
{
    [Route("api/[controller]")]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;
        private readonly ILogger<LogsController> _logger;
        private readonly IMapper _mapper;

        public LogsController(ILogger<LogsController> logger, IScriptInstanceService scriptInstanceService, IMapper mapper, ILogService logService)
        {
            _logger = logger;
            _mapper = mapper;
            _logService = logService;
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
            IEnumerable<LogViewModel> logs = _mapper.Map<IEnumerable<LogViewModel>>(_logService.GetLatestLogs(amount));
            return Ok(logs);
        }
    }
}
