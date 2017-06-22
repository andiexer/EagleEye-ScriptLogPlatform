using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Services
{
    public interface ILogService
    {
        IEnumerable<Log> GetLogsPerScriptInstance(int id);
        IEnumerable<Log> GetLatestLogs(int amount);
    }
}
