using EESLP.Services.Logging.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Entities
{
    [Table("ScriptInstance")]
    public class ScriptInstance : Entity, IAuditableEntity
    {
        public int Id { get; set; }
        public ScriptInstanceStatus InstanceStatus { get; set; }
        public int HostId { get; set; }
        public int ScriptId { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
