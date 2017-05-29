using EESLP.Services.Logging.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Services
{
    public interface IScriptInstanceService
    {
        IEnumerable<ScriptInstance> GetAllScriptInstances();
        ScriptInstance GetScriptInstanceById(int id);
        bool Update(ScriptInstance scriptInstance);
        bool Delete(ScriptInstance scriptInstance);
        int Add(ScriptInstance scriptInstance);
    }
}
