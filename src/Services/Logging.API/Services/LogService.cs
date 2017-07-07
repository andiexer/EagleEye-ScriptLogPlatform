﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Dapper;
using Dapper.Contrib.Extensions;
using EESLP.Services.Logging.API.Enums;

namespace EESLP.Services.Logging.API.Services
{
    public class LogService : BaseService, ILogService
    {
        public LogService(IOptions<DatabaseOptions> options) : base(options)
        {
        }

        public int Add(Log log)
        {
            using (var db = Connection)
            {
                db.Open();
                return (int)db.Insert<Log>(log);
            }
        }

        public IEnumerable<Log> GetLatestLogs(int amount)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<Log>($"SELECT LogLevel, LogText, ScriptInstanceId, LogDateTime FROM Log ORDER BY Id DESC LIMIT {amount}");
            }
        }

        public int GetNumberOfLogsPerScriptInstance(int id)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<int>($"SELECT COUNT(*) FROM Log WHERE ScriptInstanceId = {id}").ToArray()[0];
            }
        }

        public IEnumerable<Log> GetLogsPerScriptInstance(int id, LogLevel? logLevel, string text, int skipNumber, int takeNumber)
        {
            text = text == null ? "" : text;
            string query;
            if (logLevel == null)
            {
                query = $"SELECT LogLevel, LogText, ScriptInstanceId, LogDateTime FROM Log WHERE ScriptInstanceId = {id} AND LogText LIKE CONCAT(\"%\",@text,\"%\")ORDER BY Id DESC LIMIT {skipNumber},{takeNumber}";
            }
            else
            {
                query = $"SELECT LogLevel, LogText, ScriptInstanceId, LogDateTime FROM Log WHERE ScriptInstanceId = {id} AND LogLevel = {(int)logLevel} AND LogText LIKE CONCAT(\"%\",@text,\"%\") ORDER BY Id DESC LIMIT {skipNumber},{takeNumber}";
            }
            using (var db = Connection)
            {
                db.Open();
                return db.Query<Log>(query, new { text = text });
            }
        }
        
        public IEnumerable<Log> GetLogsPerScriptInstance(int id, LogLevel logLevel)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<Log>($"SELECT LogLevel, LogText, ScriptInstanceId, LogDateTime FROM Log WHERE ScriptInstanceId = {id} AND LogLevel = {(int)logLevel} ORDER BY Id DESC");
            }
        }

    }
}
