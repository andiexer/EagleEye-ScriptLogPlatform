using EESLP.Backend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Entities
{
    [Table("Log")]
    public class Log : Entity
    {
        public LogLevel LogLevel { get; set; }
        public string LogText { get; set; }
        public int ScriptInstanceId { get; set; }
        public DateTime LogDateTime { get; set; }
    }
}
