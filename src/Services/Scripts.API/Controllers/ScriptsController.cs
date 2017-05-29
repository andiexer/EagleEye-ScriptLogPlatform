using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.Services.Scripts.API.Entities;
using EESLP.Services.Scripts.API.Infrastructure.Exceptions;
using EESLP.Services.Scripts.API.Services;
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

        public ScriptsController(ILogger<ScriptsController> logger, IScriptService scriptService, IBusClient busClient)
        {
            _logger = logger;
            _scriptService = scriptService;
            _busClient = busClient;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_scriptService.GetAllScripts());
        }

        [HttpGet]
        [Route("{id}", Name = "GetSingleScript")]
        public IActionResult GetSingle(int id)
        {
            var script = _scriptService.GetScriptById(id);
            if (script == null)
            {
                return NotFound();
            }
            return Ok(script);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Script script)
        {
            try
            {
                var result = _scriptService.Add(script);
                // TODO: remove it, test purposes only
                _busClient.PublishAsync(new ScriptInstanceCreated(result, "fancy this is working hard!"));
                return CreatedAtRoute(routeName: "GetSingleScript", routeValues: new { id = result }, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding host to database: {e.Message}");
                return BadRequest("Error while adding host to database");
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Script script)
        {
            try
            {
                var existingScript = EnsureRequestScriptAvailable(id);
                script.Id = existingScript.Id;
                if (_scriptService.Update(script))
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var script = EnsureRequestScriptAvailable(id);
                if (_scriptService.Delete(script))
                {
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
