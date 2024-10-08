using Microsoft.AspNetCore.Identity;

namespace Auth.Exceptions;

public class UserNameTakenException() : Exception("Username already taken");

public class IdentityResultException(IEnumerable<IdentityError> errors)
    : Exception("Identity error")
{
    public IEnumerable<IdentityError> Errors { get; set; } = errors;
}
