using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Infrastructure.Filters;
using EESLP.Frontend.Gateway.API.Infrastructure.Options;
using EESLP.Frontend.Gateway.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class HostsController : Controller
    {
        private readonly ILogger<HostsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;

        public HostsController(ILogger<HostsController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions.Value;
        }

        /// <summary>
        /// get a list of all hosts
        /// </summary>
        /// <returns>list of hosts</returns>
        /// <response code="200">if hosts are found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<Host>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Get()
        {
            try
            {
                var result = _http.GetAsync(_apiOptions.ScriptsApiUrl + "/api/Hosts", Request.Headers["Pagination"], null, null).Result;
                IEnumerable<string> headerValues;
                if (result.Headers.TryGetValues("Pagination", out headerValues))
                {
                    Response.Headers.Add("Pagination", headerValues.First());
                }
                
                return Ok(result.StatusCode != HttpStatusCode.OK ? default(IEnumerable<Host>) : JsonConvert.DeserializeObject<IEnumerable<Host>>(result.Content.ReadAsStringAsync().Result));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
                var host = _http.GetAsync<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + id).Result;
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

        /// <summary>
        /// updates a host
        /// </summary>
        /// <param name="id">id of the host</param>
        /// <param name="host">host informations</param>
        /// <returns>returns 200 ok</returns>
        /// <response code="200">if update was successfull</response>
        /// <response code="404">if host was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Update(int id, [FromBody]HostUpdateModel model)
        {
            try
            {
                var result = _http.PutAsync<HostUpdateModel>(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + id, model).Result;
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
        /// deletes a host
        /// </summary>
        /// <param name="id">id of the host</param>
        /// <returns>returns 204 no content if successfull</returns>
        /// <response code="204">if delete was successfull</response>
        /// <response code="404">if host was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), 204)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _http.DeleteAsync(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + id).Result;
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
