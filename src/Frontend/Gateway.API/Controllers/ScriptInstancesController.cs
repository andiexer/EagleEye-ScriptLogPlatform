using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Infrastructure.Extensions;
using EESLP.Frontend.Gateway.API.Infrastructure.Options;
using EESLP.Frontend.Gateway.API.Utils;
using EESLP.Frontend.Gateway.API.ViewModel;
using EESLP.Frontend.Gateway.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Controllers
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
        /// gets a list of all scriptinstances
        /// </summary>
        /// <returns>list of scriptinstances</returns>
        /// <response code="200">returns a list of scriptinstances</response>
        /// <response code="404">if nothing found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<ScriptInstanceViewModel>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Get()
        {
            try
            {
                var scriptinstances = _http.GetAsync<IEnumerable<ScriptInstance>>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances").Result;
                var result = new List<ScriptInstanceViewModel>(scriptinstances.Count());
                foreach (var scriptinstance in scriptinstances)
                {
                    ScriptInstanceViewModel resultitem = _mapper.Map<ScriptInstanceViewModel>(scriptinstance);
                    var host = _cache.TryGetOrAdd(
                        CacheUtil.BuildCacheKey(new[] { "host", "id", scriptinstance.HostId.ToString() }),
                        () => _http.GetAsync<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + scriptinstance.HostId).Result);
                    var script = _cache.TryGetOrAdd(
                        CacheUtil.BuildCacheKey(new[] { "script", "id", scriptinstance.ScriptId.ToString() }),
                        () => _http.GetAsync<Script>(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + scriptinstance.ScriptId).Result);
                    resultitem.Host = host;
                    resultitem.Script = script;
                    result.Add(resultitem);
                }
                return Ok(result);
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
        /// get all logs from specific scriptinstance
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <returns>list of logs from this scriptinstance</returns>
        /// <response code="200">returns list of logs from specific scriptinstance</response>
        /// <response code="404">if scriptinstance was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("{id}/logs", Name = "GetLogs")]
        [ProducesResponseType(typeof(IEnumerable<Log>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        [ProducesResponseType(typeof(object), 404)]
        public IActionResult GetLogs(int id)
        {
            try
            {
                var logs = _http.GetAsync<IEnumerable<Log>>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id + "/logs").Result;
                return Ok(logs);
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
        [ProducesResponseType(typeof(ScriptInstanceViewModel), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetLatestScriptInstances(int amount)
        {
            try
            {
                var scriptinstances = _http.GetAsync<IEnumerable<ScriptInstance>>(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/latest/" + amount).Result;
                var result = new List<ScriptInstanceViewModel>(scriptinstances.Count());
                foreach (var scriptinstance in scriptinstances)
                {
                    ScriptInstanceViewModel resultitem = _mapper.Map<ScriptInstanceViewModel>(scriptinstance);
                    var host = _cache.TryGetOrAdd(
                        CacheUtil.BuildCacheKey(new[] { "host", "id", scriptinstance.HostId.ToString() }),
                        () => _http.GetAsync<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + scriptinstance.HostId).Result);
                    var script = _cache.TryGetOrAdd(
                        CacheUtil.BuildCacheKey(new[] { "script", "id", scriptinstance.ScriptId.ToString() }),
                        () => _http.GetAsync<Script>(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + scriptinstance.ScriptId).Result);
                    resultitem.Host = host;
                    resultitem.Script = script;
                    result.Add(resultitem);
                }
                return Ok(result);
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
                var result = _http.DeleteAsync(_apiOptions.LoggingApiUrl + "/api/ScriptInstances/" + id).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return NoContent();
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
    }
}
