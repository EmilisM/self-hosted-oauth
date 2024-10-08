using Auth.Models;

namespace Auth.GraphQL.Types;

[ExtendObjectType(
    typeof(User),
    IgnoreProperties = [nameof(User.PasswordHash), nameof(User.ConcurrencyStamp)]
)]
public class UserExtensions { }
