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

        public IEnumerable<ScriptInstance> GetAllScriptInstances(int skipNumber, int takeNumber)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<ScriptInstance>($"SELECT * FROM EESLP.ScriptInstance LIMIT {skipNumber},{takeNumber}");
            }
        }

        public int GetNumberOfScriptInstances()
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Query<int>($"SELECT COUNT(*) FROM EESLP.ScriptInstance").ToArray()[0];
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

    }
}
