using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.Entities
{
    public interface IAuditableEntity
    {
        DateTime CreatedDateTime { get; set; }
        DateTime LastModDateTime { get; set; }
    }
}
