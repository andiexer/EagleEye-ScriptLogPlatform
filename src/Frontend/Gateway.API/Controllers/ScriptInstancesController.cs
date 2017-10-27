using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Enums;
using EESLP.Frontend.Gateway.API.Infrastructure.Extensions;
using EESLP.Frontend.Gateway.API.Infrastructure.Options;
using EESLP.Frontend.Gateway.API.Utils;
using EESLP.Frontend.Gateway.API.ViewModel;
using EESLP.Frontend.Gateway.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptInstancesController : BaseController
    {
        public ScriptInstancesController(ILogger<ScriptInstancesController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache) : base(logger, mapper, http, apiOptions, cache)
        {
        }

        /// <summary>
        /// gets a list of all scriptinstances
        /// </summary>
        /// <returns>list of scriptinstances</returns>
        /// <response code="200">returns a list of scriptinstances</response>
        /// <response code="404">if nothing found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<ScriptInstance>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Get(string hostname, string scriptname, string transactionId, ScriptInstanceStatus[] status, DateTime? from, DateTime? to)
        {
            try
            {
                return BaseGetWithPaging<IEnumerable<ScriptInstance>>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances" + Request.QueryString.Value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// get single scriptinstance by id
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <returns>single scriptinstance</returns>
        /// <response code="200">returns scriptinstance if found</response>
        /// <response code="404">if scriptinstance was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("{id}", Name = "GetSingleScriptInstance")]
        [ProducesResponseType(typeof(ScriptInstance), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetSingle(int id)
        {
            try
            {
                return BaseGet<ScriptInstance>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// get all logs from specific scriptinstance
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <returns>list of logs from this scriptinstance</returns>
        /// <response code="200">returns list of logs from specific scriptinstance</response>
        /// <response code="404">if scriptinstance was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("{id}/logs", Name = "GetLogs")]
        [ProducesResponseType(typeof(IEnumerable<LogViewModel>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        public IActionResult GetLogs(int id, Enums.LogLevel[] logLevel, string logText)
        {
            try
            {
                return BaseGetWithPaging<IEnumerable<LogViewModel>>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id + "/logs" + Request.QueryString.Value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// gets a list of the latest scriptinstance entries
        /// </summary>
        /// <param name="amount">amount of scriptinstance entries</param>
        /// <returns>list of latest scriptinstance entries</returns>
        /// <response code="200">returns a list of the latest scriptinstance entries</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("latest/{amount}")]
        [ProducesResponseType(typeof(ScriptInstance), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetLatestScriptInstances(int amount)
        {
            try
            {
                return BaseGetWithPaging<IEnumerable<ScriptInstance>>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances" + Request.QueryString.Value);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// delete a specific scriptinstance and all related data (included log entries)
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <returns></returns>
        /// <response code="204">scriptinstance successfully deleted</response>
        /// <response code="400">if something went really wrong</response>
        /// <response code="404">scriptinstance not found</response>
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return BaseDelete(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
