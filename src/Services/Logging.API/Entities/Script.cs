using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace EESLP.Services.Logging.API.Entities
{
    [Table("Script")]
    public class Script : Entity, IAuditableEntity
    {
        public int Id { get; set; }
        public string Scriptname { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
