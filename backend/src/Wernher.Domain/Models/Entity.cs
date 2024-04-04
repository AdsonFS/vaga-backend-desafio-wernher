using System.Text.Json.Serialization;

namespace Wernher.Domain.Models;
public abstract class Entity()
{
    // does not show ID in the json response
    [JsonIgnore]
    public Guid Id { get; protected set; }
}
