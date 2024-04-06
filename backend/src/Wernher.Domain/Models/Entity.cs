using System.Text.Json.Serialization;

namespace Wernher.Domain.Models;
public abstract class Entity()
{
    [JsonIgnore]
    public virtual Guid Id { get; set; }
}
