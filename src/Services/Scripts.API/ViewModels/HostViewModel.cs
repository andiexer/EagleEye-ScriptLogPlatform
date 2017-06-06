using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Scripts.API.ViewModels
{
    public class HostViewModel
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string FQDN { get; set; }
        public string ApiKey { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
