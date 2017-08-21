using AutoMapper;
using EESLP.Backend.Gateway.API.Entities;
using EESLP.Backend.Gateway.API.Infrastructure.Filters;
using EESLP.Backend.Gateway.API.Infrastructure.Options;
using EESLP.Backend.Gateway.API.Utils;
using EESLP.Backend.Gateway.API.ViewModel;
using EESLP.Backend.Gateway.API.ViewModels;
using EESLP.BuildingBlocks.Resilence.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptsController : Controller
    {
        private readonly ILogger<ScriptsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;

        public ScriptsController(ILogger<ScriptsController> logger, IMapper mapper, IHttpApiClient http, ApiOptions apiOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions;
        }

        /// <summary>
        /// gets a list of all scripts
        /// </summary>
        /// <returns>list of all scripts</returns>
        /// <response code="200">returns a list of all scripts</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<Script>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Get(string scriptname)
        {
            try
            {
                var result = _http.GetAsync(_apiOptions.ScriptsApiUrl + "/api/Scripts" + Request.QueryString.Value, Request.Headers["Pagination"], null, null).Result;
                IEnumerable<string> headerValues;
                if (result.Headers.TryGetValues("Pagination", out headerValues))
                {
                    Response.Headers.Add("Pagination", headerValues.First());
                }

                return Ok(result.StatusCode != System.Net.HttpStatusCode.OK ? default(IEnumerable<Script>) : JsonConvert.DeserializeObject<IEnumerable<Script>>(result.Content.ReadAsStringAsync().Result));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// create a script
        /// </summary>
        /// <param name="model">model with script informations</param>
        /// <returns>201 created if successfull</returns>
        /// <response code="201">returns location header with actual location</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(object), 400)]
        [ValidateModelFilter]
        public IActionResult Create([FromBody]ScriptAddModel model)
        {
            try
            {
                var result = _http.PostAsync(_apiOptions.ScriptsApiUrl + "/api/Scripts", model).Result;
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

        /// <summary>
        /// Creates a new scriptinstance for this script
        /// </summary>
        /// <param name="id">ID des scriptes</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:int}/scriptInstances")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult CreateScriptInstance(int id, [FromQuery]string transactionId)
        {
            try
            {
                _logger.LogInformation($"create a scriptinstance for script with id {id}");
                StringValues headerValues;
                if (!Request.Headers.TryGetValue("ApiKey", out headerValues))
                {
                    _logger.LogInformation($"ApiKey header is not defined");
                    return BadRequest("ApiKey header is not defined");
                }
                string apiKey = headerValues.First();
                var host = _http.GetAsync<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/apikey" + apiKey).Result;
                if (host == null)
                {
                    _logger.LogInformation($"No host found with apiKey {apiKey}");
                    return NotFound("No host found with this ApiKey");
                }
                var scriptInstance = new ScriptInstanceAddModel()
                {
                    HostId = host.Id,
                    ScriptId = id,
                    TransactionId = transactionId ?? TransactionUtil.CreateTransactionId(),
                    InstanceStatus = Enums.ScriptInstanceStatus.Created
                };
                var result = _http.PostAsync(_apiOptions.LoggingApiUrl + "/api/ScriptInstances", scriptInstance).Result;
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
