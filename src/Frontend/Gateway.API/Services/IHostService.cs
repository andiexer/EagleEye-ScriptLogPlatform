using EESLP.Frontend.Gateway.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Services
{
    public interface IHostService
    {
        IEnumerable<Host> GetAllHosts();
        Host GetHostById(int id);
        bool Update(Host host);
        bool Delete(Host host);
        int Add(Host host);
    }
}
