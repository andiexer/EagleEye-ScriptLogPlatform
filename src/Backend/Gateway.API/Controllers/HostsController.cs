using AutoMapper;
using EESLP.Backend.Gateway.API.Infrastructure.Filters;
using EESLP.Backend.Gateway.API.Infrastructure.Options;
using EESLP.Backend.Gateway.API.ViewModels;
using EESLP.BuildingBlocks.Resilence.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class HostsController : Controller
    {
        private readonly ILogger<HostsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;

        public HostsController(ILogger<HostsController> logger, IMapper mapper, IHttpApiClient http, ApiOptions apiOptions, IDistributedCache cache)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions;
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
                var result = _http.PostAsync(_apiOptions.ScriptsApiUrl + "/api/Hosts", model).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return Created(result.Headers.Location, null);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    return BadRequest("Internal server error on service");
                }
                else
                {
                    return BadRequest(result.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
