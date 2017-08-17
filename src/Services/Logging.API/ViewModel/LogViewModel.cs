using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.ViewModel
{
    public class LogViewModel
    {
        public string LogLevel { get; set; }
        public string LogText { get; set; }
        public DateTime LogDateTime { get; set; }
        public int ScriptInstanceId { get; set; }
    }
}
