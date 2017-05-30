using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Scripts.API.ViewModels
{
    public class HostUpdateModel
    {
        public string Hostname { get; set; }
        public string FQDN { get; set; }
        public string ApiKey { get; set; }
    }
}
