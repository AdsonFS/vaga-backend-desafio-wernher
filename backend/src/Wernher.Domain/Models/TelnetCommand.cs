namespace Wernher.Domain.Models;

public class TelnetCommand : Entity
{
    public TelnetCommand(string command, List<Parameter> parameters)
    {
        Command = command;
        Parameters = parameters;
    }
    private TelnetCommand() { }
    public Guid CommandsID { get; }
    public string Command { get; }
    public List<Parameter> Parameters { get; }
}
