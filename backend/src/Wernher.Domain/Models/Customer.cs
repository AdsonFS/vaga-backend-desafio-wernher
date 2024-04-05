using System.Text.Json.Serialization;

namespace Wernher.Domain.Models;

public class Customer : Entity
{
    public Customer(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
        Devices = new List<Device>();
    }
    private Customer() { }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    [JsonIgnore]
    public List<Device> Devices { get; private set; }
}
