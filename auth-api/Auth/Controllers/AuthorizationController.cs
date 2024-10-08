using System.Security.Claims;
using Auth.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Auth.Controllers;

public class AuthorizationController(
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictScopeManager scopeManager
) : ControllerBase
{
    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request is null)
        {
            throw new InvalidOperationException("Request cannot be found.");
        }

        if (request.IsClientCredentialsGrantType())
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.

            var application = await applicationManager.FindByClientIdAsync(request.ClientId!);

            if (application is null)
            {
                throw new InvalidOperationException("The application cannot be found.");
            }

            // Create a new ClaimsIdentity containing the claims that
            // will be used to create an id_token, a token or a code.
            var identity = new ClaimsIdentity(
                TokenValidationParameters.DefaultAuthenticationType,
                OpenIddictConstants.Claims.Name,
                OpenIddictConstants.Claims.Role
            );

            var clientId = await applicationManager.GetClientIdAsync(application);
            var name = await applicationManager.GetDisplayNameAsync(application);

            // Use the client_id as the subject identifier.
            identity.SetClaim(OpenIddictConstants.Claims.Subject, clientId);
            identity.SetClaim(OpenIddictConstants.Claims.Name, name);

            var scopes = request.GetScopes();
            identity.SetScopes(scopes);

            var resources = scopeManager
                .ListResourcesAsync(identity.GetScopes())
                .ToBlockingEnumerable();

            identity.SetResources(resources);
            identity.SetDestinations(ClaimsService.GetDestinations);

            return SignIn(
                new ClaimsPrincipal(identity),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
        }

        throw new NotImplementedException("The specified grant is not implemented.");
    }

    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    public async Task<IActionResult> Authorize()
    {
        var request =
            HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("Request cannot be found.");
        var clientId = request.ClientId!;
        var responseType = request.ResponseType!;

        var result = await HttpContext.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        if (!result.Succeeded)
        {
            var redirectUri = Uri.EscapeDataString(
                $"https://localhost:5000/connect/authorize?client_id={clientId}&response_type={responseType}"
            );

            return Redirect($"https://localhost:5200?redirect_uri={redirectUri}");
        }

        var identity = new ClaimsIdentity(
            authenticationType: "Mock",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role
        );

        identity.SetClaim(OpenIddictConstants.Claims.Subject, "mock");

        return SignIn(
            new ClaimsPrincipal(identity),
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
        );
    }
}
