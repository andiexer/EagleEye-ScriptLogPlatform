using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Enums;
using EESLP.Services.Logging.API.Infrastructure.Exceptions;
using EESLP.Services.Logging.API.Services;
using EESLP.Services.Logging.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptInstancesController : Controller
    {
        private readonly IScriptInstanceService _scriptInstanceService;
        private readonly ILogger<ScriptInstancesController> _logger;

        public ScriptInstancesController(ILogger<ScriptInstancesController> logger, IScriptInstanceService scriptInstanceService)
        {
            _logger = logger;
            _scriptInstanceService = scriptInstanceService;
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
        public IActionResult Get()
        {
            var instances = _scriptInstanceService.GetAllScriptInstances();
            if (instances == null)
            {
                return NotFound();
            }
            return Ok(instances);
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
            var scriptInstance = _scriptInstanceService.GetScriptInstanceById(id);
            if (scriptInstance == null)
            {
                return NotFound();
            }
            return Ok(scriptInstance);
        }

        /// <summary>
        /// creates a new scriptinstance
        /// </summary>
        /// <param name="scriptInstance"></param>
        /// <returns></returns>
        /// <response code="201">scriptinstance successfully created</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPost]
        [Route("")]
        public IActionResult Create([FromBody]ScriptInstance scriptInstance)
        {
            try
            {
                var result = _scriptInstanceService.Add(scriptInstance);
                return CreatedAtRoute(routeName: "GetSingleScriptInstance", routeValues: new { id = result }, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding host to database: {e.Message}");
                return BadRequest("Error while adding host to database");
            }
        }

        /// <summary>
        /// updates a specific scriptinstance
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <param name="scriptInstance">data of the scriptinstance</param>
        /// <returns></returns>
        /// <response code="200">scriptinstance successfully updated</response>
        /// <response code="400">if something went really wrong</response>
        /// <response code="404">if scriptinstance not found</response>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]ScriptInstance scriptInstance)
        {
            try
            {
                var existingScriptInstance = EnsureRequestScriptInstanceAvailable(id);
                scriptInstance.Id = existingScriptInstance.Id;
                if (_scriptInstanceService.Update(scriptInstance))
                {
                    return Ok();
                }
                return BadRequest("Error while updating scriptInstance");
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while updating scriptInstance: {e.Message}");
                return BadRequest("Error while updating scriptInstance");
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var scriptInstance = EnsureRequestScriptInstanceAvailable(id);
                if (_scriptInstanceService.Delete(scriptInstance))
                {
                    return NoContent();
                }
                return BadRequest("Error while deleting scriptInstance");
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while deleting scriptInstance: {e.Message}");
                return BadRequest("Error while deleting scriptInstance");
            }
        }

        /// <summary>
        /// changes the status of a scriptinstance 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody]UpdateInstanceStatusViewModel model)
        {
            var scriptinstance = _scriptInstanceService.GetScriptInstanceById(id);
            _logger.LogInformation($"Update status of scriptinstance with id {id}");
            if (scriptinstance == null)
            {
                return NotFound();
            }
            if (model.Status == ScriptInstanceStatus.Completed || model.Status == ScriptInstanceStatus.CompletedWithError || model.Status == ScriptInstanceStatus.CompletedWithWarning)
            {
                scriptinstance.EndDateTime = DateTime.UtcNow;
                // TODO: Implement completed with error or warnings
            }
            else
            {
                scriptinstance.EndDateTime = null;
            }
            scriptinstance.InstanceStatus = model.Status;
            if (_scriptInstanceService.Update(scriptinstance))
            {
                return Ok();
            }
            return BadRequest($"There was an error on updating status");
        }

        #region private helpers

        private ScriptInstance EnsureRequestScriptInstanceAvailable(int id)
        {
            var host = _scriptInstanceService.GetScriptInstanceById(id);
            if (host == null)
            {
                _logger.LogWarning($"no scriptinstance found with id  {id}");
                throw new EntityNotFoundException();
            }
            return host;
        }

        #endregion

    }
}
