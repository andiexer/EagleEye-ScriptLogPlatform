﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EESLP.Services.Logging.API.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Dapper.Contrib.Extensions;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.ViewModel;

namespace EESLP.Services.Logging.API.Services
{
    public class ScriptInstanceService : BaseService, IScriptInstanceService
    {
        public ScriptInstanceService(IOptions<DatabaseOptions> options) : base(options)
        {
        }

        public IEnumerable<ScriptInstance> GetAllScriptInstances()
        {
            using (var db = Connection)
            {
                db.Open();
                return db.GetAll<ScriptInstance>().ToList();
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
                db.Open();
                UpdateAuditableFields(scriptInstance, true);
                return (int)db.Insert<ScriptInstance>(scriptInstance);
            }
        }

        public void DeleteByScriptId(int hostId)
        {
            using (var db = Connection)
            {
                db.Open();
                db.Execute("DELETE FROM EESLP.ScriptInstance WHERE ScriptId = @Id ", new
                {
                    Id = hostId
                });
            }
        }
    }
}
