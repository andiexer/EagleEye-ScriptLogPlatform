using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;

namespace EESLP.Services.Logging.API.Infrastructure.Extensions
{
    public static class RedisCacheOptionsExtensions
    {

        public static void ResolveDns(this RedisCacheOptions options)
        {
            // Assume that the first part is host and port.
            var hostWithPort = options.Configuration;
            if (IsIpAddress(hostWithPort)) return;
            var resolved = TryResolveDns(hostWithPort);
            var replaced = options.Configuration.Replace(hostWithPort, resolved);
            options.Configuration = replaced;
        }

        private static string TryResolveDns(string redisUrl)
        {
            var config = ConfigurationOptions.Parse(redisUrl);

            foreach (DnsEndPoint addressEndpoint in config.EndPoints)
            {
                var port = addressEndpoint.Port;
                var isIp = IsIpAddress(addressEndpoint.Host);
                if (!isIp)
                {
                    var ip = Dns.GetHostEntryAsync(addressEndpoint.Host).GetAwaiter().GetResult();
                    return $"{ip.AddressList.First(x => IsIpAddress(x.ToString()))}";
                }
            }

            return redisUrl;
        }

        private static bool IsIpAddress(string host)
        {
            return Regex.IsMatch(host, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
        }

    }
}
