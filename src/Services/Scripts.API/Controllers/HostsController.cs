using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Services.Scripts.API.Entities;
using EESLP.Services.Scripts.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace EESLP.Services.Scripts.API.Controllers
{
    [Route("api/[controller]")]
    public class HostsController : Controller
    {

        private readonly IHostService _hostService;
        private readonly ILogger<HostsController> _logger;

        public HostsController(ILogger<HostsController> logger, IHostService hostService)
        {
            _logger = logger;
            _hostService = hostService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return  Ok(_hostService.GetAllHosts());
        }

        [HttpGet]
        [Route("{id}", Name = "GetSingle")]
        public IActionResult GetSingle(int id)
        {
            var host = _hostService.GetHostById(id);
            if (host == null)
            {
                return NotFound();
            }
            return Ok(host);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Host host)
        {
            try
            {
                var result = _hostService.Add(host);
                return CreatedAtRoute(routeName: "GetSingle", routeValues: new {id = result}, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding host to database: {e.Message}");
                return BadRequest("Error while adding host to database");
            }
           
        }

        [HttpPut]
        public IActionResult Update([FromBody]Host host)
        {
            try
            {
                if (_hostService.Update(host))
                {
                    return Ok();
                }
                return BadRequest("Error while updating host");
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while updating host: {e.Message}");
                return BadRequest("Error while updating host");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var host = _hostService.GetHostById(id);
            if (host == null)
            {
                return NotFound();
            }
            try
            {
                if (_hostService.Delete(host))
                {
                    return NoContent();
                }
                return BadRequest("Error while deleting host");
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while deleting host: {e.Message}");
                return BadRequest("Error while deleting host");
            }
        }

    }
}
