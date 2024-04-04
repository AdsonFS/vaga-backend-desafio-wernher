using FluentValidation;
using Wernher.API.DTO;

namespace Wernher.API.Validation;
public class ParameterValidator : AbstractValidator<ParameterDto>
{
    public ParameterValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}