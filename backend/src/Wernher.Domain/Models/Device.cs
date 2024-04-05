using System.Text.Json.Serialization;

namespace Wernher.Domain.Models;

public class Device : Entity
{
    public Device(string description, string manufacturer, string url, List<Command> commands)
    {
        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        Commands = commands;
    }
    private Device() { }
    public override Guid Id { get; protected set; }
    [JsonIgnore]
    public Guid CustomerId { get; set; }
    public string Description { get; private set; }
    public string Manufacturer { get; private set; }
    public string Url { get; private set; }
    public List<Command> Commands { get; private set; }

    public void Update(string description, string manufacturer, string url, List<Command> commands)
    {
        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        Commands = commands;
    }
}
