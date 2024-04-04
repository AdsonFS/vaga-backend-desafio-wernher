using FluentValidation;
using Wernher.API.DTO;

namespace Wernher.API.Validation;
public class CommandValidator : AbstractValidator<CommandDto>
{
    public CommandValidator()
    {

        RuleFor(x => x.Operation).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.TelnetCommand).NotEmpty();
        RuleFor(x => x.Result).NotEmpty();
        RuleFor(x => x.Format).NotEmpty();

        RuleFor(x => x.TelnetCommand).SetValidator(new TelnetCommandValidator());
    }
}