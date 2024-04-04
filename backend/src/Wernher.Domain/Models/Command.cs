namespace Wernher.Domain.Models;

public class Command : Entity
{
    public Command(string operation, string description, TelnetCommand telnetCommand, string result, string format)
    {
        Operation = operation;
        Description = description;
        TelnetCommand = telnetCommand;
        Result = result;
        Format = format;
    }
    private Command() { }
    public Guid DeviceID { get; init; }
    public string Operation { get; init; }
    public string Description { get; init; }
    public TelnetCommand TelnetCommand { get; init; }
    public string Result { get; init; }
    public string Format { get; init; }
}
