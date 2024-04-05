using FluentValidation;
using Wernher.Domain.Models;

namespace Wernher.API.Validation;
public class ParameterValidator : AbstractValidator<Parameter>
{
    public ParameterValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}