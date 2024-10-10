using System.Security.Claims;
using Auth.Constants;
using Auth.Exceptions;
using Auth.GraphQL.Payloads;
using Auth.Models;
using Auth.Services;
using HotChocolate.Language;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Auth.GraphQL.Mutations;

[ExtendObjectType(OperationType.Mutation)]
public class UserMutations
{
    [Error(typeof(UsernameTakenException))]
    [Error(typeof(IdentityResultException))]
    public async Task<CreateUserPayload> CreateUser(
        string username,
        [Service] UserManager<User> userManager
    )
    {
        var user = await userManager.FindByNameAsync(username);

        if (user is not null)
        {
            throw new UsernameTakenException();
        }

        var newUser = new User { UserName = username };

        var password = SecretsService.Generate();
        var result = await userManager.CreateAsync(newUser, password);

        if (!result.Succeeded)
        {
            throw new IdentityResultException(result.Errors);
        }

        return new CreateUserPayload { User = newUser, Password = password };
    }
}
