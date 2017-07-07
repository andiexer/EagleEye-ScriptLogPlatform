using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Infrastructure.Filters;
using EESLP.Frontend.Gateway.API.Infrastructure.Options;
using EESLP.Frontend.Gateway.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
    public class ScriptsController : Controller
    {
        private readonly ILogger<ScriptsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;

        public ScriptsController(ILogger<ScriptsController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions.Value;
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
        public IActionResult Get()
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
                var host = _http.GetAsync<Script>(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + id).Result;
                if (host == null)
                {
                    return NotFound();
                }
                return Ok(host);
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
        /// updates a script
        /// </summary>
        /// <param name="id">id of the script</param>
        /// <param name="model">script data</param>
        /// <returns>returns 200 ok</returns>
        /// <response code="200">if update was successfull</response>
        /// <response code="404">if script was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Update(int id, [FromBody]ScriptUpdateModel model)
        {
            try
            {
                var result = _http.PutAsync(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + id, model).Result;
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
        /// delete a script
        /// this will also raise a integration event for other microservices
        /// default wil delete all script instances associated with it
        /// </summary>
        /// <param name="id">id of the script</param>
        /// <returns>returns 200 ok</returns>
        /// <response code="204">if script was successfully deleted</response>
        /// <response code="404">if script was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), 204)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _http.DeleteAsync(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + id).Result;
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
