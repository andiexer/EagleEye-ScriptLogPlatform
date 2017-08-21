using AutoMapper;
using EESLP.Backend.Gateway.API.Infrastructure.Options;
using EESLP.BuildingBlocks.Resilence.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    public class ScriptInstancesController : Controller
    {
        private readonly ILogger<ScriptInstancesController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;

        public ScriptInstancesController(ILogger<ScriptInstancesController> logger, IMapper mapper, IHttpApiClient http, ApiOptions apiOptions, IDistributedCache cache)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions;
        }
    }
}
