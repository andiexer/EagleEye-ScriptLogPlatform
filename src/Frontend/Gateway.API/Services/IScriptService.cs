using EESLP.Frontend.Gateway.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Gateway.API.Services
{
    interface IScriptService
    {
        IEnumerable<Script> GetAllScripts();
        Script GetScriptById(int id);
        bool Update(Script script);
        bool Delete(Script script);
        int Add(Script script);
    }
}
