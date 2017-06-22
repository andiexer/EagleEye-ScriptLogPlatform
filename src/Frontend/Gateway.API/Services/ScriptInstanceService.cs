using EESLP.Frontend.Gateway.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Frontend.Gateway.API.Entities;

namespace EESLP.Frontend.Gateway.API.Services
{
    public class ScriptInstanceService : IScriptInstanceService
    {
        public bool Delete(ScriptInstance scriptInstance)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScriptInstance> GetAllScriptInstances()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScriptInstance> GetLatestScriptInstances(int amount)
        {
            throw new NotImplementedException();
        }

        public ScriptInstance GetScriptInstanceById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
