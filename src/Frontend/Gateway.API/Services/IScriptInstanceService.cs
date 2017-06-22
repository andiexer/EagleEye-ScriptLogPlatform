using EESLP.Frontend.Gateway.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Services
{
    public interface IScriptInstanceService
    {
        IEnumerable<ScriptInstance> GetAllScriptInstances();
        IEnumerable<ScriptInstance> GetLatestScriptInstances(int amount);
        ScriptInstance GetScriptInstanceById(int id);
        bool Delete(ScriptInstance scriptInstance);
    }
}