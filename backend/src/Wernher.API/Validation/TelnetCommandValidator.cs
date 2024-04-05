using FluentValidation;
using Wernher.Domain.Models;

namespace Wernher.API.Validation;
public class TelnetCommandValidator : AbstractValidator<TelnetCommand>
{
    public TelnetCommandValidator()
    {
        RuleFor(x => x.Command).NotEmpty();
        RuleFor(x => x.Parameters).NotEmpty();
        RuleForEach(x => x.Parameters).SetValidator(new ParameterValidator());
    }
}