using Auth.Models;

namespace Auth.GraphQL.Payloads;

public class CreateUserPayload
{
    public required User User { get; set; }
    public required string Password { get; set; }
}
