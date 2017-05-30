using AutoMapper;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Enums;
using EESLP.Services.Logging.API.Infrastructure.Exceptions;
using EESLP.Services.Logging.API.Services;
using EESLP.Services.Logging.API.Utils;
using EESLP.Services.Logging.API.ViewModel;
using EESLP.Services.Logging.API.ViewModel.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using RawRabbit;
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
        private readonly ILogService _logService;
        private readonly ILogger<ScriptInstancesController> _logger;
        private readonly IMapper _mapper;
        private readonly IBusClient _busClient;

        public ScriptInstancesController(ILogger<ScriptInstancesController> logger, IScriptInstanceService scriptInstanceService, IMapper mapper, ILogService logService, IBusClient busClient)
        {
            _logger = logger;
            _scriptInstanceService = scriptInstanceService;
            _mapper = mapper;
            _logService = logService;
            _busClient = busClient;
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
            IEnumerable<ScriptInstanceViewModel> scriptInstances = _mapper.Map<IEnumerable<ScriptInstanceViewModel>>(_scriptInstanceService.GetAllScriptInstances());
            return Ok(scriptInstances);
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
        public IActionResult Create([FromBody]ScriptInstanceAddModel scriptInstance)
        {
            try
            {
                scriptInstance.TransactionId = scriptInstance.TransactionId ?? TransactionUtil.CreateTransactionId();
                scriptInstance.InstanceStatus = scriptInstance.InstanceStatus ?? ScriptInstanceStatus.Created;
                ScriptInstance newScriptInstance = _mapper.Map<ScriptInstance>(scriptInstance);
                var result = _scriptInstanceService.Add(newScriptInstance);
                _busClient.PublishAsync(new ScriptInstanceCreated(result));
                return CreatedAtRoute(routeName: "GetSingleScriptInstance", routeValues: new { id = result }, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding host to database: {e.Message}");
                return BadRequest("Error while adding host to database");
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
                var scriptInstance = _mapper.Map<ScriptInstanceViewModel>(EnsureRequestScriptInstanceAvailable(id));
                return Ok(scriptInstance);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(this.CreateErrorMessage(e, "ENTITY_NOT_FOUND"));
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
                var scriptInstance = EnsureRequestScriptInstanceAvailable(id);
                if (_scriptInstanceService.Delete(scriptInstance))
                {
                    return NoContent();
                }
                return BadRequest("Error while deleting scriptInstance");
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(this.CreateErrorMessage(e, "ENTITY_NOT_FOUND"));
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while deleting scriptInstance: {e.Message}");
                return BadRequest(CreateErrorMessage(e, "DATABASE_ERROR"));
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
        public IActionResult GetLogs(int id)
        {
            try
            {
                EnsureRequestScriptInstanceAvailable(id);
                IEnumerable<LogViewModel> logs = _mapper.Map<IEnumerable<LogViewModel>>(_logService.GetLogsPerScriptInstance(id));
                return Ok(logs);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(this.CreateErrorMessage(e, "ENTITY_NOT_FOUND"));
            }
        }

        /// <summary>
        /// add a new log to scriptinstance
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <param name="logData">logdata</param>
        /// <returns></returns>
        /// <response code="201">if log successfully created</response>
        /// <response code="404">if scriptinstance was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPost]
        [Route("{id}/logs", Name = "AddLog")]
        public IActionResult AddLog(int id, [FromBody]LogAddModel logData)
        {
            try
            {
                ScriptInstance scriptInstance = EnsureRequestScriptInstanceAvailable(id);
                if (scriptInstance.InstanceStatus != ScriptInstanceStatus.Running)
                {
                    _logger.LogError($"Scriptinstance is not started or already completed.Current status: {scriptInstance.InstanceStatus.ToString()}");
                    return BadRequest($"Scriptinstance is not started or already completed. Current status: {scriptInstance.InstanceStatus.ToString()}");
                }
                _logger.LogInformation($"Add new log entry in scriptinstance {id}");
                Log newLog = _mapper.Map<LogAddModel, Log>(logData);
                newLog.ScriptInstanceId = id;
                var result = _logService.Add(newLog);
                return CreatedAtRoute(routeName: "GetLogs", routeValues: new { id = id }, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding log to database: {e.Message}");
                return BadRequest("Error while adding log to database");
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(this.CreateErrorMessage(e, "ENTITY_NOT_FOUND"));
            }
        }

        /// <summary>
        /// changes the status of a scriptinstance 
        /// </summary>
        /// <param name="id">id of the scriptinstance</param>
        /// <param name="scriptInstanceStatus">new status of the scriptinstance</param>
        /// <returns></returns>
        /// <response code="200">successfully updated scriptinstance status</response>
        /// <response code="400">if something went really wrong</response>
        [HttpPut]
        [Route("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody]UpdateInstanceStatusViewModel scriptInstanceStatus)
        {
            try
            {
                _logger.LogInformation($"Update status of scriptinstance with id {id}");
                var scriptinstance = EnsureRequestScriptInstanceAvailable(id);
                if (scriptInstanceStatus.Status == ScriptInstanceStatus.Completed || scriptInstanceStatus.Status == ScriptInstanceStatus.CompletedWithError || scriptInstanceStatus.Status == ScriptInstanceStatus.CompletedWithWarning)
                {
                    scriptinstance.EndDateTime = DateTime.UtcNow;
                    if (_logService.GetLogsPerScriptInstance(id, Enums.LogLevel.Error) != null 
                        && _logService.GetLogsPerScriptInstance(id, Enums.LogLevel.Fatal) != null)
                    {
                        _busClient.PublishAsync(new ScriptInstanceCompleted(id, ScriptInstanceStatus.CompletedWithError.ToString()));
                        scriptinstance.InstanceStatus = ScriptInstanceStatus.CompletedWithError;
                    }
                    else if (_logService.GetLogsPerScriptInstance(id, Enums.LogLevel.Warning) != null)
                    {
                        _busClient.PublishAsync(new ScriptInstanceCompleted(id, ScriptInstanceStatus.CompletedWithWarning.ToString()));
                        scriptinstance.InstanceStatus = ScriptInstanceStatus.CompletedWithWarning;
                    }
                    else
                    {
                        _busClient.PublishAsync(new ScriptInstanceCompleted(id, ScriptInstanceStatus.Completed.ToString()));
                        scriptinstance.InstanceStatus = ScriptInstanceStatus.Completed;
                    }
                }
                else
                {
                    scriptinstance.EndDateTime = null;
                    scriptinstance.InstanceStatus = scriptInstanceStatus.Status;
                }
                if (_scriptInstanceService.Update(scriptinstance))
                {
                    return Ok();
                }
                return BadRequest($"There was an error on updating status");
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(this.CreateErrorMessage(e, "ENTITY_NOT_FOUND"));
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
            IEnumerable<ScriptInstanceViewModel> logs = _mapper.Map<IEnumerable<ScriptInstanceViewModel>>(_scriptInstanceService.GetLatestScriptInstances(amount));
            return Ok(logs);
        }

        #region private helpers

        private ScriptInstance EnsureRequestScriptInstanceAvailable(int id)
        {
            var scriptInstance = _scriptInstanceService.GetScriptInstanceById(id);
            if (scriptInstance == null)
            {
                _logger.LogWarning($"No scriptinstance found with id  {id}");
                throw new EntityNotFoundException();
            }
            return scriptInstance;
        }

        protected ErrorMessage CreateErrorMessage(Exception ex, string errorId)
        {
            return new ErrorMessage()
            {
                ErrorId = errorId,
                Message = ex.Message
            };
        }

        #endregion

    }
}
