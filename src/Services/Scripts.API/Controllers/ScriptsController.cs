using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.Services.Scripts.API.Entities;
using EESLP.Services.Scripts.API.Infrastructure.Exceptions;
using EESLP.Services.Scripts.API.Infrastructure.Filters;
using EESLP.Services.Scripts.API.Services;
using EESLP.Services.Scripts.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using RawRabbit;
using EESLP.Services.Scripts.API.Infrastructure.Extensions;

namespace EESLP.Services.Scripts.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptsController : Controller
    {

        private readonly IScriptService _scriptService;
        private readonly ILogger<ScriptsController> _logger;
        private readonly IBusClient _busClient;
        private readonly IMapper _mapper;
        int page = 1;
        int pageSize = 10;

        public ScriptsController(ILogger<ScriptsController> logger, IScriptService scriptService, IBusClient busClient, IMapper mapper)
        {
            _logger = logger;
            _scriptService = scriptService;
            _busClient = busClient;
            _mapper = mapper;
        }

        /// <summary>
        /// gets a list of all scripts
        /// </summary>
        /// <returns>list of all scripts</returns>
        /// <response code="200">returns a list of all scripts</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ScriptViewModel>), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Get()
        {
            try
            {
                var pagination = Request.Headers["Pagination"];
                if (!string.IsNullOrEmpty(pagination))
                {
                    string[] vals = pagination.ToString().Split(',');
                    int.TryParse(vals[0], out page);
                    int.TryParse(vals[1], out pageSize);
                }
                int currentPage = page;
                int currentPageSize = pageSize;
                var totalScripts = _scriptService.GetNumberOfAllScripts();
                var totalPages = (int)Math.Ceiling((double)totalScripts / pageSize);

                Response.AddPagination(page, pageSize, totalScripts, totalPages);

                return Ok(
                    _mapper.Map<IEnumerable<Script>, IEnumerable<ScriptViewModel>>(_scriptService.GetAllScripts((currentPage - 1) * currentPageSize, currentPageSize)));
            }
            catch (Exception e)
            {
                return BadRequest();
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
        [Route("{id}", Name = "GetSingleScript")]
        [ProducesResponseType(typeof(ScriptViewModel),200)]
        [ProducesResponseType(typeof(object), 404)]
        public IActionResult GetSingle(int id)
        {
            try
            {
                var script = _scriptService.GetScriptById(id);
                if (script == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<Script, ScriptViewModel>(script));
            }
            catch (Exception e)
            {
                return BadRequest();
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
        [ValidateModelFilter]
        [ProducesResponseType(typeof(object),201)]
        [ProducesResponseType(typeof(object),400)]
        public IActionResult Create([FromBody]ScriptAddModel model)
        {
            try
            {
                var script = _mapper.Map<ScriptAddModel, Script>(model);
                var result = _scriptService.Add(script);
                return CreatedAtRoute(routeName: "GetSingleScript", routeValues: new { id = result }, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding script to database: {e.Message}");
                return BadRequest("Error while adding script to database");
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
        [ValidateModelFilter]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Update(int id, [FromBody]ScriptUpdateModel model)
        {
            try
            {
                var existingScript = EnsureRequestScriptAvailable(id);
                existingScript = _mapper.Map<ScriptUpdateModel, Script>(model, existingScript);
                if (_scriptService.Update(existingScript))
                {
                    return Ok();
                }
                return BadRequest("Error while updating script");
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while updating script: {e.Message}");
                return BadRequest("Error while updating script");
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
                var script = EnsureRequestScriptAvailable(id);
                if (_scriptService.Delete(script))
                {
                    _busClient.PublishAsync(new ScriptDeleted(id));
                    return NoContent();
                }
                return BadRequest("Error while deleting script");
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while deleting script: {e.Message}");
                return BadRequest("Error while deleting script");
            }
        }

        #region private helpers

        private Script EnsureRequestScriptAvailable(int id)
        {
            var script = _scriptService.GetScriptById(id);
            if (script == null)
            {
                _logger.LogWarning($"no script found with id  {id}");
                throw new EntityNotFoundException();
            }
            return script;
        }

        #endregion

    }
}
