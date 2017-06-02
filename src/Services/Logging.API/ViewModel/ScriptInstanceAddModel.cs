using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Enums;
using EESLP.Services.Logging.API.ViewModel.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.ViewModel
{
    public class ScriptInstanceAddModel : IValidatableObject
    {
        public string TransactionId { get; set; }
        public int HostId { get; set; }
        public int ScriptId { get; set; }
        public ScriptInstanceStatus? InstanceStatus { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ScriptInstanceAddModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
