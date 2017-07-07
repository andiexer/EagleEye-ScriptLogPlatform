using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EESLP.Frontend.Gateway.API.Infrastructure.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static T TryGetOrAdd<T>(this IDistributedCache cache, string key, Func<T> action, int expirationSeconds = 60, bool slidingExpiration = true)
        {
            var cacheValue = cache.GetString(key);
            if (cacheValue != null) return JsonConvert.DeserializeObject<T>(cacheValue);
            var actionResponse = action.Invoke();
            var cacheOptions = new DistributedCacheEntryOptions();
            SetCacheExpiration(cacheOptions, expirationSeconds, slidingExpiration);
            cache.SetString(key,JsonConvert.SerializeObject(actionResponse), cacheOptions);
            return actionResponse;
        }

        private static void SetCacheExpiration(DistributedCacheEntryOptions options, int expirationSeconds, bool slidingExpiration)
        {
            if (slidingExpiration)
            {
                options.SetSlidingExpiration(TimeSpan.FromSeconds(expirationSeconds));
            }
            else
            {
                options.SetAbsoluteExpiration(DateTime.Now.AddSeconds(expirationSeconds));
            }
        }

    }
}
