namespace Wernher.Domain.Models;

public class Device : Entity
{
    public Device(string identifier, string description, string manufacturer, string url, List<Command> commands)
    {
        Identifier = identifier;
        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        Commands = commands;
    }
    private Device() { }
    public string Identifier { get; }
    public string Description { get; }
    public string Manufacturer { get; }
    public string Url { get; }
    public List<Command> Commands { get; }
}
