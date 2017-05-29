using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.ViewModel
{
    public class ScriptInstanceAddModel
    {
        public string TransactionId { get; set; }
        public int HostId { get; set; }
        public int ScriptId { get; set; }
        public ScriptInstanceStatus? InstanceStatus { get; set; }
    }
}
