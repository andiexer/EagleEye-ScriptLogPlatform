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
using Microsoft.Extensions.Caching.Distributed;

namespace EESLP.Frontend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptsController : BaseController
    {
        public ScriptsController(ILogger<ScriptsController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache) : base(logger, mapper, http, apiOptions, cache)
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
                return BasePost(_apiOptions.ScriptsApiUrl + "/api/Scripts", model);
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
                return BasePut(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + id, model);
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
                return BaseDelete(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
