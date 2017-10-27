using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.ViewModels
{
    public class ScriptUpdateModel
    {
        public string Scriptname { get; set; }
        public string Description { get; set; }
    }
}
