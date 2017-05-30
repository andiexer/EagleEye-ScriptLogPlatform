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

namespace EESLP.Services.Scripts.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptsController : Controller
    {

        private readonly IScriptService _scriptService;
        private readonly ILogger<ScriptsController> _logger;
        private readonly IBusClient _busClient;
        private readonly IMapper _mapper;

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
            return Ok(_mapper.Map<IEnumerable<Script>, IEnumerable<ScriptViewModel>>(_scriptService.GetAllScripts()));
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
            var script = _scriptService.GetScriptById(id);
            if (script == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<Script, ScriptViewModel>(script));
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
                _logger.LogError($"Error while adding host to database: {e.Message}");
                return BadRequest("Error while adding host to database");
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
        /// delete a script instance
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
