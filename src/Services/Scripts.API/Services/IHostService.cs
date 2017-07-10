using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.Services.Scripts.API.Entities;

namespace EESLP.Services.Scripts.API.Services
{
    public interface IHostService
    {
        IEnumerable<Host> GetAllHosts(string hostname, int skipNumber, int takeNumber);
        IEnumerable<int> GetAllHostIDs(string hostname);
        int GetNumberOfAllHosts(string hostname);
        Host GetHostById(int id);
        Host GetHostByApiKey(string apiKey);
        bool Update(Host host);
        bool Delete(Host host);
        int Add(Host host);
    }
}
