using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.ViewModel.Validations
{
    public class UpdateInstanceStatusViewModelValidator : AbstractValidator<UpdateInstanceStatusViewModel>
    {
        public UpdateInstanceStatusViewModelValidator()
        {
            RuleFor(s => s.Status).IsInEnum();
        }
    }
}
