using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Frontend.Gateway.API.ViewModel.Validations
{
    public class ScriptInstanceAddModelValidator : AbstractValidator<ScriptInstanceAddModel>
    {
        public ScriptInstanceAddModelValidator()
        {
            RuleFor(s => s.HostId).NotEmpty();
            RuleFor(s => s.ScriptId).NotEmpty();
        }
    }
}
