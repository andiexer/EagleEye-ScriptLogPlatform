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
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptsController : BaseController
    {
        public ScriptsController(ILogger<BaseController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache) : base(logger, mapper, http, apiOptions, cache)
        {
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
                ApiKeyAuthentication();
                return BaseGetWithPaging<IEnumerable<Script>>(_apiOptions.ScriptsApiUrl + "/api/Scripts" + Request.QueryString.Value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// get a single script
        /// </summary>
        /// <param name="id">id of the script</param>
        /// <returns>single script</returns>
        /// <response code="200">returns a single script</response>
        /// <response code="404">if script was not found</response>
        [HttpGet]
        [Route("{id:int}", Name = "GetSingleScript")]
        [ProducesResponseType(typeof(Script), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetSingle(int id)
        {
            try
            {
                ApiKeyAuthentication();
                return BaseGet<Script>(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + id);
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
                ApiKeyAuthentication();
                return BasePost(_apiOptions.ScriptsApiUrl + "/api/Scripts", model);
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
                var host = ApiKeyAuthentication();
                var scriptInstance = new ScriptInstanceAddModel()
                {
                    HostId = host.Id,
                    ScriptId = id,
                    TransactionId = transactionId ?? TransactionUtil.CreateTransactionId(),
                    InstanceStatus = Enums.ScriptInstanceStatus.Created
                };
                return BasePost(_apiOptions.LoggingApiUrl + "/api/ScriptInstances", scriptInstance);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
