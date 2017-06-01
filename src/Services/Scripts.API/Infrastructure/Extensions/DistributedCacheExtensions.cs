using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EESLP.Services.Scripts.API.Infrastructure.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static T TryGetOrAdd<T>(this IDistributedCache cache, string key, Func<T> action)
        {
            var cacheValue = cache.GetString(key);
            if (cacheValue != null) return JsonConvert.DeserializeObject<T>(cacheValue);
            var actionResponse = action.Invoke();
            cache.SetString(key,JsonConvert.SerializeObject(actionResponse));
            return actionResponse;
        }
    }
}
