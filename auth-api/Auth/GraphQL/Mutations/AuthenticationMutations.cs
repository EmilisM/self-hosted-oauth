using System.Security.Claims;
using Auth.Constants;
using Auth.Exceptions;
using Auth.Models;
using HotChocolate.Language;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Auth.GraphQL.Mutations;

[ExtendObjectType(OperationType.Mutation)]
public class AuthenticationMutations
{
    [Error(typeof(InvalidUsernameOrPasswordException))]
    [UseMutationConvention(PayloadFieldName = "outcome")]
    public async Task<bool> LogInPassword(
        string username,
        string password,
        [Service] IHttpContextAccessor httpContextAccessor,
        [Service] UserManager<User> userManager
    )
    {
        if (httpContextAccessor.HttpContext is null)
        {
            throw new NullReferenceException("HttpContext is null");
        }

        var user = await userManager.FindByNameAsync(username);

        if (user is null)
        {
            throw new InvalidUsernameOrPasswordException();
        }

        var outcome = await userManager.CheckPasswordAsync(user, password);

        if (!outcome)
        {
            throw new InvalidUsernameOrPasswordException();
        }

        var identity = new ClaimsIdentity(
            authenticationType: AuthenticationTypeConstants.Password,
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role
        );

        identity.AddClaim(OpenIddictConstants.Claims.Username, username);

        await httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
        );

        return true;
    }
}
