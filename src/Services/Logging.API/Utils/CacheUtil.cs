﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Utils
{
    public class CacheUtil
    {
        private const string DefaultKeySeparator = "-|-";
        public static string BuildCacheKey(IList<string> keyItems)
        {
            return string.Join(DefaultKeySeparator, keyItems);
        }
    }
}
