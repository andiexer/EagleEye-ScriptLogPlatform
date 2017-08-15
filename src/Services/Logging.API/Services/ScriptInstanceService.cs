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
        private readonly IHttpApiClient _http;
        private readonly ApiOptions _apiOptions;
        public ScriptInstanceService(IOptions<DatabaseOptions> databaseOptions, IHttpApiClient http, IOptions<ApiOptions> apiOptions) : base(databaseOptions)
        {
            _http = http;
            _apiOptions = apiOptions.Value;
        }

        public IEnumerable<ScriptInstance> GetAllScriptInstances(ScriptInstanceStatus[] status, string hostname, string scriptname, string transactionId, DateTime? from, DateTime? to, int skipNumber, int takeNumber)
        {
            transactionId = transactionId == null ? "" : transactionId;
            string query = $"SELECT * FROM EESLP.ScriptInstance ";
            query += getScriptInstanceWhereQuery(status, hostname, scriptname, from, to);
            query += $"ORDER BY Id DESC ";
            query += $"LIMIT {skipNumber},{takeNumber} ";
            using (var db = Connection)
            {
                db.Open();
                return db.Query<ScriptInstance>(query, new { transactionId = transactionId });
            }
        }

        public int GetNumberOfScriptInstances(ScriptInstanceStatus[] status, string hostname, string scriptname, string transactionId, DateTime? from, DateTime? to)
        {
            transactionId = transactionId == null ? "" : transactionId;
            string query = $"SELECT COUNT(*) FROM EESLP.ScriptInstance ";
            query += getScriptInstanceWhereQuery(status, hostname, scriptname, from, to);
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
                if (!checkIfScriptExists(scriptInstance.ScriptId) || !checkIfHostExists(scriptInstance.HostId))
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
                if (!checkIfScriptExists(scriptInstance.ScriptId) || !checkIfHostExists(scriptInstance.HostId)) {
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
                if (!checkIfScriptExists(scriptid))
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
                if (!checkIfHostExists(hostid))
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

        // Help functions
        private bool checkIfScriptExists(int scriptid)
        {
            var script = _http.GetStringAsync(_apiOptions.ScriptsApiUrl + "/api/Scripts/" + scriptid).Result;
            return script != null && script != "";
        }
        private bool checkIfHostExists(int hostid)
        {
            var script = _http.GetStringAsync(_apiOptions.ScriptsApiUrl + "/api/Hosts/" + hostid).Result;
            return script != null && script != "";
        }

        private string getScriptInstanceWhereQuery(ScriptInstanceStatus[] status, string hostname, string scriptname, DateTime? from, DateTime? to)
        {
            string query = $"WHERE ";
            query += "TransactionId LIKE CONCAT(\"%\",@transactionId,\"%\") ";
            if (status.Count() > 0 || hostname != null || scriptname != null || from != null || to != null)
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
                if (scriptname != null)
                {
                    var scripts = _http.GetAsync<IEnumerable<int>>(_apiOptions.ScriptsApiUrl + "/api/Scripts/IDs?scriptname=" + scriptname).Result;

                    if (scripts.Count() > 0)
                    {
                        string sqlScripts = "(";
                        scripts.ToList().ForEach((int id) => { sqlScripts += $"{id},"; });
                        sqlScripts = sqlScripts.Remove(sqlScripts.Length - 1, 1);
                        sqlScripts += ")";
                        query += $"AND ScriptId IN {sqlScripts} ";
                    }
                    else
                    {
                        query += $"AND ScriptId IS NULL ";
                    }
                }
                if (hostname != null)
                {
                    var hosts = _http.GetAsync<IEnumerable<int>>(_apiOptions.ScriptsApiUrl + "/api/Hosts/IDs?hostname=" + hostname).Result;

                    if (hosts.Count() > 0)
                    {
                        string sqlHosts = "(";
                        hosts.ToList().ForEach((int id) => { sqlHosts += $"{id},"; });
                        sqlHosts = sqlHosts.Remove(sqlHosts.Length - 1, 1);
                        sqlHosts += ")";
                        query += $"AND HostId IN {sqlHosts} ";
                    }
                    else
                    {
                        query += $"AND HostId IS NULL ";
                    }
                }
                query = query.Replace("WHERE AND", "WHERE");
            }
            return query;
        }

    }
}
