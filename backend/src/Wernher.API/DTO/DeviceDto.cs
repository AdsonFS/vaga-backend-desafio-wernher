namespace Wernher.API.DTO;

public record DeviceDto(string Identifier, string Description, string Manufacturer, string Url, CommandDto[] Commands);
public record CommandDto(string Operation, string Description, TelnetCommandDto TelnetCommand, string Result, string Format);
public record TelnetCommandDto(string Command, List<ParameterDto> Parameters);
public record ParameterDto(string Name, string Description);