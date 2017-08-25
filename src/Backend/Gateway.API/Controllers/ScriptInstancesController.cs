﻿using AutoMapper;
using EESLP.Backend.Gateway.API.Entities;
using EESLP.Backend.Gateway.API.Infrastructure.Options;
using EESLP.Backend.Gateway.API.Infrastructure.Extensions;
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
using EESLP.Backend.Gateway.API.Utils;
using EESLP.Backend.Gateway.API.ViewModel;

namespace EESLP.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptInstancesController : Controller
    {
        private readonly ILogger<ScriptInstancesController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;
        private readonly IDistributedCache _cache;

        public ScriptInstancesController(ILogger<ScriptInstancesController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions.Value;
            _cache = cache;
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
        [ProducesResponseType(typeof(ScriptInstanceViewModel), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetSingle(int id)
        {
            try
            {
                var scriptinstance = _http.GetAsync<ScriptInstance>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id).Result;
                if (scriptinstance == null)
                {
                    return NotFound();
                }
                var host = _cache.TryGetOrAdd(
                    CacheUtil.BuildCacheKey(new[] { "host", "id", scriptinstance.HostId.ToString() }),
                    () => _http.GetAsync<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + scriptinstance.HostId).Result);
                var script = _cache.TryGetOrAdd(
                    CacheUtil.BuildCacheKey(new[] { "script", "id", scriptinstance.ScriptId.ToString() }),
                    () => _http.GetAsync<Script>(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + scriptinstance.ScriptId).Result);
                ScriptInstanceViewModel result = _mapper.Map<ScriptInstanceViewModel>(scriptinstance);
                result.Host = host;
                result.Script = script;
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// updates a scriptInstance status
        /// </summary>
        /// <param name="id">id of the script</param>
        /// <param name="model">scriptInstance status</param>
        /// <returns>returns 200 ok</returns>
        /// <response code="200">if update was successfull</response>
        /// <response code="404">if scriptInstance was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Update(int id, [FromBody]UpdateInstanceStatusViewModel model)
        {
            try
            {
                var result = _http.PutAsync(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id + "/status", model).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok();
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
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
        /// create a new log entry
        /// </summary>
        /// <param name="id">id of the scriptInstance where you want to create an new log entry</param>
        /// <param name="model">model with log entry informations</param>
        /// <returns>201 created if successfull</returns>
        /// <response code="201">returns location header with actual location</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPost]
        [Route("{id}/Logs")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult AddLogs(int id, [FromBody]LogAddModel model)
        {
            try
            {
                var result = _http.PostAsync(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id + "/logs", model).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var location = Request.Scheme + "://" + Request.Host + result.Headers.Location.AbsolutePath;
                    return Created(location, null);
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
        /// get all logs from a scriptinstance
        /// </summary>
        /// <param name="id">id of the scriptInstance</param>
        /// <returns>all logs of a scriptInstance</returns>
        /// <response code="200">returns all logs</response>
        /// <response code="404">if scriptInstance was not found</response>
        [HttpGet]
        [Route("{id}/Logs")]
        [ProducesResponseType(typeof(Script), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetLogs(int id)
        {
            try
            {
                var logs = _http.GetAsync<Log[]>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id + "/Logs").Result;
                if (logs == null)
                {
                    return NotFound();
                }
                return Ok(logs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
