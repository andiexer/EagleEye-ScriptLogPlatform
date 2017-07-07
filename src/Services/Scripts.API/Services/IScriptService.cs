using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Services.Scripts.API.Entities;

namespace EESLP.Services.Scripts.API.Services
{
    public interface IScriptService
    {
        IEnumerable<Script> GetAllScripts(string scriptname, int skipNumber, int takeNumber);
        int GetNumberOfAllScripts(string scriptname);
        Script GetScriptById(int id);
        bool Update(Script script);
        bool Delete(Script script);
        int Add(Script script);
    }
}
