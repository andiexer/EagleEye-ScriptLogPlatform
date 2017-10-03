using EESLP.Services.Logging.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.ViewModel
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
