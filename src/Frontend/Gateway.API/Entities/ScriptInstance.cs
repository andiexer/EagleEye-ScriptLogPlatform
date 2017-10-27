using EESLP.Frontend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Entities
{
    [Table("ScriptInstance")]
    public class ScriptInstance : Entity, IAuditableEntity
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
