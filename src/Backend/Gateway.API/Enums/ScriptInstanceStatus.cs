using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Enums
{
    public enum ScriptInstanceStatus
    {
        Completed,
        Running,
        Created,
        CompletedWithError,
        CompletedWithWarning,
        Aborted,
        Timeout
    }
}
