using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.Services.Scripts.API.Entities;
using EESLP.Services.Scripts.API.Infrastructure.Exceptions;
using EESLP.Services.Scripts.API.Infrastructure.Extensions;
using EESLP.Services.Scripts.API.Infrastructure.Filters;
using EESLP.Services.Scripts.API.Services;
using EESLP.Services.Scripts.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using RawRabbit;

namespace EESLP.Services.Scripts.API.Controllers
{
    [Route("api/[controller]")]
    public class HostsController : Controller
    {

        private readonly IHostService _hostService;
        private readonly ILogger<HostsController> _logger;
        private readonly IBusClient _busClient;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public HostsController(ILogger<HostsController> logger, IHostService hostService, IBusClient busClient, IMapper mapper, IDistributedCache cache)
        {
            _logger = logger;
            _hostService = hostService;
            _busClient = busClient;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>
        /// get a list of all hosts
        /// </summary>
        /// <returns>list of hosts</returns>
        /// <response code="200">if hosts are found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Host>),200)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Host>, IEnumerable<HostViewModel>>(_hostService.GetAllHosts()));
            }
            catch (Exception e)
            {
                return BadRequest();
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
        [ProducesResponseType(typeof(HostViewModel),200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetSingle(int id)
        {
            try
            {
                var host = _hostService.GetHostById(id);
                if (host == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<Host, HostViewModel>(host));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
              
        }

        /// <summary>
        /// get a host by its api key
        /// </summary>
        /// <param name="apiKey">api key</param>
        /// <returns>hostviewmodel</returns>
        /// <response code="200">returns hostviewmodel</response>
        /// <response code="404">if host was not found</response>
        /// <response code="400">if something went really wrong</response>
        [HttpGet("apikey/{apiKey}")]
        [ProducesResponseType(typeof(HostViewModel), 200)]
        [ProducesResponseType(typeof(object), 404)]
        [ProducesResponseType(typeof(object), 400)]
        public IActionResult GetHostByApiKey(string apiKey)
        {
            try
            {

                var host = _cache.TryGetOrAdd($"host-apikey-{apiKey}", () => _hostService.GetHostByApiKey(apiKey));
                if (host == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<Host, HostViewModel>(host));
            }
            catch (Exception e)
            {
                _logger.LogError($"EEOR while fetching data from cache: {e.Message}");
                return BadRequest();
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
                var result = _hostService.Add(_mapper.Map<HostAddModel, Host>(model));
                return CreatedAtRoute(routeName: "GetSingleHost", routeValues: new {id = result}, value: null);
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while adding host to database: {e.Message}");
                return BadRequest("Error while adding host to database");
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
                var existingHost = EnsureRequestHostAvailable(id);
                existingHost = _mapper.Map<HostUpdateModel, Host>(model, existingHost);
                if (_hostService.Update(existingHost))
                {
                    return Ok();
                }
                return BadRequest("Error while updating host");
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while updating host: {e.Message}");
                return BadRequest("Error while updating host");
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
                var host = EnsureRequestHostAvailable(id);
                if (_hostService.Delete(host))
                {
                    _busClient.PublishAsync(new HostDeleted(id));
                    return NoContent();
                }
                return BadRequest("Error while deleting host");
            }
            catch (EntityNotFoundException e)
            {
                return NotFound();
            }
            catch (MySqlException e)
            {
                _logger.LogError($"Error while deleting host: {e.Message}");
                return BadRequest("Error while deleting host");
            }
        }

        #region private helpers

        private Host EnsureRequestHostAvailable(int id)
        {
            var host = _hostService.GetHostById(id);
            if (host == null)
            {
                _logger.LogWarning($"no host found with id  {id}");
                throw new EntityNotFoundException();
            }
            return host;
        }

        #endregion

    }
}
