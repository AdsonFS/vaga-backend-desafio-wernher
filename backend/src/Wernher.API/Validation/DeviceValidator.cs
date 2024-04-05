using System.Data;
using FluentValidation;
using Wernher.Domain.Models;

namespace Wernher.API.Validation;
public class DeviceValidator : AbstractValidator<Device>
{
    public DeviceValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Manufacturer).NotEmpty().Equal("PredictWeater");

        RuleForEach(x => x.Commands).SetValidator(new CommandValidator());

        RuleFor(x => x.Commands)
            .NotEmpty()
            .Must(c => c.Any(y => y.TelnetCommand.Command == "get_rainfall_intensity"))
            .WithMessage("At least one command must have command 'get_rainfall_intensity'")
            .Must(c => c.Select(x => x.TelnetCommand.Command).Distinct().Count() == c.Count)
            .WithMessage("Commands must be unique");

    }
}
