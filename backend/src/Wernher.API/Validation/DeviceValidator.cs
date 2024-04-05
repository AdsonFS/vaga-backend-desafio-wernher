using System.Data;
using System.Text.RegularExpressions;
using FluentValidation;
using Wernher.Domain.Models;

namespace Wernher.API.Validation;
public class DeviceValidator : AbstractValidator<Device>
{
    public DeviceValidator()
    {
        RuleFor(x => x.Identifier).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Manufacturer).NotEmpty().Equal("PredictWeater");
        RuleFor(x => x.Url).NotEmpty().Must(BeAValidUrl).WithMessage("Invalid URL");

        RuleForEach(x => x.Commands).SetValidator(new CommandValidator());

        RuleFor(x => x.Commands)
            .NotEmpty()
            .Must(c => c.Any(y => y.TelnetCommand.Command == "get_rainfall_intensity"))
            .WithMessage("At least one command must have command 'get_rainfall_intensity'");

    }
    private bool BeAValidUrl(string url)
        => Regex.IsMatch(url, @"^[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)$");
}