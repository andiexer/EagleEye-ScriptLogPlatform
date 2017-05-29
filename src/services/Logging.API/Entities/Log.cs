using Dapper.Contrib.Extensions;
using EESLP.Services.Logging.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Entities
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
