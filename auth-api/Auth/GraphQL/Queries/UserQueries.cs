using Auth.Models;
using HotChocolate.Language;
using Microsoft.AspNetCore.Identity;

namespace Auth.GraphQL.Queries;

[ExtendObjectType(OperationType.Query)]
public class UserQueries
{
    public IEnumerable<User> GetUsers([Service] UserManager<User> userManager)
    {
        return userManager.Users;
    }
}
