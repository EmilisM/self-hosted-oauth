using Microsoft.AspNetCore.Identity;

namespace Auth.Exceptions;

public class UsernameTakenException() : Exception("Username already taken");

public class InvalidUsernameOrPasswordException() : Exception("Invalid username or password");

public class IdentityResultException(IEnumerable<IdentityError> errors)
    : Exception("Identity error")
{
    public IEnumerable<IdentityError> Errors { get; set; } = errors;
}
