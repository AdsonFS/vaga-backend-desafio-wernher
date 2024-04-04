using FluentValidation;
using Wernher.API.DTO;

namespace Wernher.API.Validation;
public class TelnetCommandValidator : AbstractValidator<TelnetCommandDto>
{
    public TelnetCommandValidator()
    {
        RuleFor(x => x.Command).NotEmpty();
        RuleFor(x => x.Parameters).NotEmpty();
        RuleForEach(x => x.Parameters).SetValidator(new ParameterValidator());
    }
}