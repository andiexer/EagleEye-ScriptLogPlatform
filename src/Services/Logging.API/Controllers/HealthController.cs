using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EESLP.Services.Logging.API.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            _logger.LogInformation("API Status: {ApiStatus}","up");
            return Ok("up");
        }
    }
}
