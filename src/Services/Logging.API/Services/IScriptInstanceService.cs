using EESLP.Services.Logging.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Services
{
    public interface IScriptInstanceService
    {
        IEnumerable<ScriptInstance> GetAllScriptInstances(int skipNumber, int takeNumber);
        int GetNumberOfScriptInstances();
        IEnumerable<ScriptInstance> GetLatestScriptInstances(int amount);
        ScriptInstance GetScriptInstanceById(int id);
        bool Update(ScriptInstance scriptInstance);
        bool Delete(ScriptInstance scriptInstance);
        int Add(ScriptInstance scriptInstance);
        void DeleteByScriptId(int scriptid);
        void DeleteByHostId(int hostid);
    }
}
