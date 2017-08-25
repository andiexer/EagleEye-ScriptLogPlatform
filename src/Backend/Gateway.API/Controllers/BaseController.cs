using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Backend.Gateway.API.Infrastructure.Options;
using EESLP.Backend.Gateway.API.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using EESLP.Backend.Gateway.API.Entities;
using EESLP.Backend.Gateway.API.Utils;

namespace EESLP.Backend.Gateway.API.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMapper _mapper;
        protected readonly IHttpApiClient _http;
        protected readonly ApiOptions _apiOptions;
        protected readonly IDistributedCache _cache;

        public BaseController(ILogger<BaseController> logger, IMapper mapper, IHttpApiClient http, IOptions<ApiOptions> apiOptions, IDistributedCache cache)
        {
            _logger = logger;
            _mapper = mapper;
            _http = http;
            _apiOptions = apiOptions.Value;
            _cache = cache;
        }

        protected IActionResult BaseDelete(string url)
        {
            try
            {
                var result = _http.DeleteAsync(url).Result;
                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    return NoContent();
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else if (result.StatusCode == HttpStatusCode.InternalServerError)
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

        protected IActionResult BaseGetWithPaging<T>(string url)
        {
            try
            {
                var result = _http.GetAsync(url, Request.Headers["Pagination"], null, null).Result;
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else if (result.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return BadRequest("Internal server error on service");
                }
                IEnumerable<string> headerValues;
                if (result.Headers.TryGetValues("Pagination", out headerValues))
                {
                    Response.Headers.Add("Pagination", headerValues.First());
                }

                return Ok(result.StatusCode != HttpStatusCode.OK ? default(T) : JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected IActionResult BaseGet<T>(string url)
        {
            try
            {
                var result = _http.GetAsync(url, null, null, null).Result;
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else if (result.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return BadRequest("Internal server error on service");
                }
                var resultObject = result.StatusCode != HttpStatusCode.OK ? default(T) : JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result);

                return Ok(resultObject);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        protected IActionResult BasePost<T>(string url, T model)
        {
            try
            {
                var result = _http.PostAsync(url, model).Result;
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    var location = Request.Scheme + "://" + Request.Host + result.Headers.Location.AbsolutePath;
                    return Created(location, null);
                }
                else if (result.StatusCode == HttpStatusCode.InternalServerError)
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

        protected IActionResult BasePut<T>(string url, T model)
        {
            try
            {
                var result = _http.PutAsync<T>(url, model).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return Ok();
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else if (result.StatusCode == HttpStatusCode.InternalServerError)
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

        protected Host ApiKeyAuthentication()
        {
            StringValues headerValues;
            if (!Request.Headers.TryGetValue("ApiKey", out headerValues))
            {
                _logger.LogInformation($"ApiKey header is not defined");
                throw new KeyNotFoundException("ApiKey header is not defined");
            }
            string apiKey = headerValues.First();
            var host = _cache.TryGetOrAdd(
                        CacheUtil.BuildCacheKey(new[] { "host", "apiKey", apiKey }),
                        () => _http.GetAsync<Host>(_apiOptions.ScriptsApiUrl + "/api/Hosts/apikey/" + apiKey).Result);
            if (host == null)
            {
                _logger.LogInformation($"No host found with apiKey {apiKey}");
                throw new KeyNotFoundException("No host found with this api key");
            }
            return host;
        }
    }
}
