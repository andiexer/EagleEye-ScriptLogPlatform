using EESLP.Backend.Gateway.API.Entities;
using EESLP.Backend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.ViewModels
{
    public class ScriptInstanceViewModel
    {
        public int Id { get; set; }
        public string InstanceStatus { get; set; }
        public Host Host { get; set; }
        public Script Script { get; set; }
        public string TransactionId { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
