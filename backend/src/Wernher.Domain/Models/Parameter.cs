namespace Wernher.Domain.Models;

public class Parameter : Entity
{
    private Parameter() { }
    public Parameter(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public Guid CommandID { get; }
    public string Name { get; }
    public string Description { get; }
}
