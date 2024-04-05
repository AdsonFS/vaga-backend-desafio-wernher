namespace Wernher.API.DTO;

public record Login(string Email, string Password);
public record CustomerResponse(string Email, string Name);