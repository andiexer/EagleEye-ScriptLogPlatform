using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Services
{
    public interface ILogService
    {
        IEnumerable<Log> GetLogsPerScriptInstance(int id, LogLevel[] logLevel, string text, int skipNumber, int takeNumber);
        int GetNumberOfLogsPerScriptInstance(int id, LogLevel[] logLevel, string text);
        IEnumerable<Log> GetLogsPerScriptInstance(int id, LogLevel logLevel);
        IEnumerable<Log> GetLatestLogs(int amount);
        int Add(Log log);
    }
}
