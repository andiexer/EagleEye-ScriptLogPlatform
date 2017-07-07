using EESLP.Frontend.Gateway.API.Entities;
using EESLP.Frontend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.ViewModels
{
    public class ScriptInstanceViewModel
    {
        public int Id { get; set; }
        public ScriptInstanceStatus InstanceStatus { get; set; }
        public Host Host { get; set; }
        public Script Script { get; set; }
        public string TransactionId { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
