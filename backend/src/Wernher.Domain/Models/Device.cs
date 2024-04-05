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
    public override Guid Id { get; protected set; }
    public string Identifier { get; private set; }
    public string Description { get; private set; }
    public string Manufacturer { get; private set; }
    public string Url { get; private set; }
    public List<Command> Commands { get; private set; }

    public void Update(string identifier, string description, string manufacturer, string url, List<Command> commands)
    {
        Identifier = identifier;
        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        Commands = commands;
    }
}
