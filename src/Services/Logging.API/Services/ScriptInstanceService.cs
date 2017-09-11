using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EESLP.Services.Logging.API.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Dapper.Contrib.Extensions;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.ViewModel;
using Dapper;
using EESLP.BuildingBlocks.Resilence.Http;
using EESLP.Services.Logging.API.Infrastructure.Exceptions;
using EESLP.Services.Logging.API.Enums;

namespace EESLP.Services.Logging.API.Services
{
    public class ScriptInstanceService : BaseService, IScriptInstanceService
    {
        IScriptService _scriptService;
        IHostService _hostService;

        public ScriptInstanceService(IOptions<DatabaseOptions> databaseOptions, IScriptService scriptService, IHostService hostService) : base(databaseOptions)
        {
            _scriptService = scriptService;
            _hostService = hostService;
        }

        public IEnumerable<ScriptInstance> GetAllScriptInstances(ScriptInstanceStatus[] status, string hostname, string scriptname, string transactionId, DateTime? from, DateTime? to, int skipNumber, int takeNumber)
        {
            transactionId = transactionId ?? "";
            hostname = hostname ?? "";
            scriptname = scriptname?? "";
            string query = $"SELECT * FROM EESLP.ScriptInstance ";
            query += GetScriptInstanceWhereQuery(status, from, to);
            query += $"ORDER BY Id DESC ";
            query += $"LIMIT {skipNumber},{takeNumber} ";
            using (var db = Connection)
            {
                db.Open();
                return db.Query<ScriptInstance>(query, new { transactionId = transactionId, hostname = hostname, scriptname = scriptname});
            }
        }

        public int GetNumberOfScriptInstances(ScriptInstanceStatus[] status, string hostname, string scriptname, string transactionId, DateTime? from, DateTime? to)
        {
            transactionId = transactionId ?? "";
            hostname = hostname ?? "";
            scriptname = scriptname ?? "";
            string query = $"SELECT COUNT(*) FROM EESLP.ScriptInstance ";
            query += GetScriptInstanceWhereQuery(status, from, to);
            using (var db = Connection)
            {
                db.Open();
                return db.Query<int>(query, new { transactionId = transactionId }).ToArray()[0];
            }
        }

        public IEnumerable<ScriptInstance> GetLatestScriptInstances(int amount)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<ScriptInstance>($"SELECT * FROM EESLP.ScriptInstance ORDER BY Id DESC LIMIT {amount}");
            }
        }

        public ScriptInstance GetScriptInstanceById(int id)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Get<ScriptInstance>(id);
            }
        }

        public bool Update(ScriptInstance scriptInstance)
        {
            using (var db = Connection)
            {
                if (!CheckIfScriptExists(scriptInstance.ScriptId) || !CheckIfHostExists(scriptInstance.HostId))
                {
                    throw new EntityNotFoundException();
                }
                db.Open();
                UpdateAuditableFields(scriptInstance);
                return db.Update<ScriptInstance>(scriptInstance);
            }
        }

        public bool Delete(ScriptInstance scriptInstance)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Delete<ScriptInstance>(scriptInstance);
            }
        }

        public int Add(ScriptInstance scriptInstance)
        {
            using (var db = Connection)
            {
                if (!CheckIfScriptExists(scriptInstance.ScriptId) || !CheckIfHostExists(scriptInstance.HostId)) {
                    throw new EntityNotFoundException();
                }
                db.Open();
                UpdateAuditableFields(scriptInstance, true);
                return (int)db.Insert<ScriptInstance>(scriptInstance);
            }
        }

        public void DeleteByScriptId(int scriptid)
        {
            using (var db = Connection)
            {
                if (!CheckIfScriptExists(scriptid))
                {
                    throw new EntityNotFoundException();
                }
                db.Open();
                db.Execute("DELETE FROM EESLP.ScriptInstance WHERE ScriptId = @Id ", new
                {
                    Id = scriptid
                });
            }
        }

        public void DeleteByHostId(int hostid)
        {
            using (var db = Connection)
            {
                if (!CheckIfHostExists(hostid))
                {
                    throw new EntityNotFoundException();
                }
                db.Open();
                db.Execute("DELETE FROM EESLP.ScriptInstance WHERE HostId = @Id", new
                {
                    Id = hostid
                });
            }
        }

        #region private help functions
        private bool CheckIfScriptExists(int scriptid)
        {
            var script = _scriptService.GetScriptById(scriptid);
            return script != null;
        }
        private bool CheckIfHostExists(int hostid)
        {
            var host = _hostService.GetHostById(hostid);
            return host != null;
        }

        private string GetScriptInstanceWhereQuery(ScriptInstanceStatus[] status, DateTime? from, DateTime? to)
        {
            string query = $"WHERE ";
            query += "TransactionId LIKE CONCAT(\"%\",@transactionId,\"%\") ";
            query += "AND EESLP.Host.hostname LIKE CONCAT(\"%\",@hostname,\"%\") ";
            query += "AND EESLP.Script.Scriptname LIKE CONCAT(\"%\",@scriptname,\"%\") ";
            if (status.Count() > 0 || from != null || to != null)
            {
                if (status.Count() > 0)
                {
                    string queryStatus = "";
                    status.ToList().ForEach((ScriptInstanceStatus localStatus) => { queryStatus += $"{(int)localStatus},"; });
                    query += $"AND InstanceStatus IN ({queryStatus.Remove(queryStatus.Length - 1, 1)}) ";
                }

                query += from != null && to != null ? $"AND CreatedDateTime BETWEEN '{String.Format("{0:yyyy-MM-dd HH:mm:ss}", from)}' AND '{String.Format("{0:yyyy-MM-dd HH:mm:ss}", to)}' " : "";
                query += from != null && to == null ? $"AND CreatedDateTime BETWEEN '{String.Format("{0:yyyy-MM-dd HH:mm:ss}", from)}' AND '{String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.MaxValue)}' " : "";
                query += from == null && to != null ? $"AND CreatedDateTime BETWEEN '{String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.MinValue)}' AND '{String.Format("{0:yyyy-MM-dd HH:mm:ss}", to)}' " : "";
                query = query.Replace("WHERE AND", "WHERE");
            }
            return query;
        }
        #endregion
    }
}
