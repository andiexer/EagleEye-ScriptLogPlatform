using AutoMapper;
using EESLP.Backend.Gateway.API.Entities;
using EESLP.Backend.Gateway.API.Infrastructure.Filters;
using EESLP.Backend.Gateway.API.Infrastructure.Options;
using EESLP.Backend.Gateway.API.ViewModels;
using EESLP.BuildingBlocks.Resilence.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class LogsController : BaseController
    {
        public LogsController(ILogger<LogsController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache) : base(logger, mapper, http, apiOptions, cache)
        {
        }

        /// <summary>
        /// returns a single host
        /// </summary>
        /// <param name="id">id of the host</param>
        /// <returns>single host</returns>
        /// <response code="200">if host was found</response>
        /// <response code="404">if host was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("{id:int}", Name = "GetSingleHost")]
        [ProducesResponseType(typeof(Host), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetSingle(int id)
        {
            try
            {
                return BaseGet<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// create a host
        /// </summary>
        /// <param name="model">host informations</param>
        /// <returns>201 created with location</returns>
        /// <response code="201">response header location with its newly location</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ValidateModelFilter]
        public IActionResult Create([FromBody]HostAddModel model)
        {
            try
            {
                return BasePost(_apiOptions.ScriptsApiUrl + "/api/Hosts", model);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
