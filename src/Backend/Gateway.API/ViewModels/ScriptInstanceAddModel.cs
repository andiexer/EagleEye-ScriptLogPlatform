using EESLP.Backend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.ViewModel
{
    public class ScriptInstanceAddModel
    {
        public string TransactionId { get; set; }
        public int HostId { get; set; }
        public int ScriptId { get; set; }
        public ScriptInstanceStatus? InstanceStatus { get; set; }
    }
}
