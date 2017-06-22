using EESLP.Frontend.Gateway.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Frontend.Gateway.API.Entities;

namespace EESLP.Frontend.Gateway.API.Services
{
    public class LogService : ILogService
    {
        public IEnumerable<Log> GetLatestLogs(int amount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Log> GetLogsPerScriptInstance(int id)
        {
            throw new NotImplementedException();
        }
    }
}
