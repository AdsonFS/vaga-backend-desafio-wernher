using Wernher.Domain.Models;

namespace Wernher.API.DTO;

public static class DeviceDtoExtensions
{
    public static Device ToModel(this DeviceDto device)
    {
        return new Device(
            device.Identifier,
            device.Description,
            device.Manufacturer,
            device.Url,
            device.Commands.Select(c => c.toModel()).ToList()
        );
    }

    private static Command toModel(this CommandDto command)
        => new Command(command.Operation, command.Description, command.TelnetCommand.toModel(), command.Result, command.Format);
    private static TelnetCommand toModel(this TelnetCommandDto telnetCommand)
        => new TelnetCommand(telnetCommand.Command, telnetCommand.Parameters.Select(x => x.toModel()).ToList());
    private static Parameter toModel(this ParameterDto parameter)
        => new Parameter(parameter.Name, parameter.Description);
}
