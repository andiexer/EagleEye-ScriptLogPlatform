using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Entities
{
    [Table("Host")]
    public class Host : Entity, IAuditableEntity
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string FQDN { get; set; }
        public string ApiKey { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModDateTime { get; set; }
    }
}
