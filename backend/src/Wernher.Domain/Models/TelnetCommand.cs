using System.Text.Json.Serialization;

namespace Wernher.Domain.Models;

public class TelnetCommand : Entity
{
    public TelnetCommand(string command, List<Parameter> parameters)
    {
        Command = command;
        Parameters = parameters;
    }
    private TelnetCommand() { }
    [JsonIgnore]
    public Guid CommandsID { get; }
    public string Command { get; }
    public List<Parameter> Parameters { get; }

    public string GetCommandWithParameter(Parameter parameter)
        => parameter.Name != String.Empty ? $"{Command} {parameter.Name}\r\n" : $"{Command}\r\n";
}
