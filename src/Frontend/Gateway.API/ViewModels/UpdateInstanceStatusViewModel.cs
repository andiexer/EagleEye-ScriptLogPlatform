using EESLP.Frontend.Gateway.API.Enums;
using EESLP.Frontend.Gateway.API.ViewModel.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.ViewModel
{
    public class UpdateInstanceStatusViewModel : IValidatableObject
    {
        public ScriptInstanceStatus Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UpdateInstanceStatusViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
