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
        public int HostId { get; set; }
        public int ScriptId { get; set; }
        public string TransactionId { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
