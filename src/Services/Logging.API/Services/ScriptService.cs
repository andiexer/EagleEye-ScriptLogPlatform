using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Dapper;

namespace EESLP.Services.Logging.API.Services
{
    public class ScriptService : BaseService, IScriptService
    {
        public ScriptService(IOptions<DatabaseOptions> options) : base(options)
        {
        }

        public IEnumerable<Script> GetAllScripts(string scriptname, int skipNumber, int takeNumber)
        {
            scriptname = scriptname == null ? "" : scriptname;
            using (var db = Connection)
            {
                db.Open();
                var result = db.Query<Script>($"SELECT * FROM Script WHERE Scriptname LIKE CONCAT(\"%\",@scriptname,\"%\") LIMIT {skipNumber},{takeNumber}", new { scriptname = scriptname });
                return result;
            }
        }

        public int GetNumberOfAllScripts(string scriptname)
        {
            scriptname = scriptname == null ? "" : scriptname;
            using (var db = Connection)
            {
                db.Open();
                return db.Query<int>($"SELECT COUNT(*) FROM Script WHERE Scriptname LIKE CONCAT(\"%\",@scriptname,\"%\")", new { scriptname = scriptname }).ToArray()[0];
            }
        }

        public IEnumerable<int> GetAllScriptIds(string scriptname)
        {
            scriptname = scriptname == null ? "" : scriptname;
            using (var db = Connection)
            {
                db.Open();
                var result = db.Query<int>($"SELECT Id FROM Script WHERE Scriptname LIKE CONCAT(\"%\",@scriptname,\"%\")", new { scriptname = scriptname });
                return result;
            }
        }

        public Script GetScriptById(int id)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Get<Script>(id);
            }
        }

        public bool Update(Script script)
        {
            using (var db = Connection)
            {
                db.Open();
                UpdateAuditableFields(script);
                return db.Update<Script>(script);
            }
        }

        public bool Delete(Script script)
        {
            using (var db = Connection)
            {
                db.Open();
                return db.Delete<Script>(script);
            }
        }

        public int Add(Script script)
        {
            using (var db = Connection)
            {
                db.Open();
                UpdateAuditableFields(script, true);
                return (int)db.Insert<Script>(script);
            }
        }
    }
}
