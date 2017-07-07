using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Services.Scripts.API.Entities;

namespace EESLP.Services.Scripts.API.Services
{
    public interface IHostService
    {
        IEnumerable<Host> GetAllHosts(int skipNumber, int takeNumber);
        int GetNumberOfAllHosts();
        Host GetHostById(int id);
        Host GetHostByApiKey(string apiKey);
        bool Update(Host host);
        bool Delete(Host host);
        int Add(Host host);
    }
}
