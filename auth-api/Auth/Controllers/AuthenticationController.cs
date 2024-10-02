using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace Auth.Controllers;

public class AuthenticationController : ControllerBase
{
    [HttpGet("~/callback/login/mock"), HttpPost("~/callback/login/mock"), IgnoreAntiforgeryToken]
    public async Task<ActionResult> LogInMock()
    {
        var identity = new ClaimsIdentity(authenticationType: "Mock", nameType: ClaimTypes.Name, roleType: ClaimTypes.Role);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, "mock");

        return SignIn(new ClaimsPrincipal(identity), CookieAuthenticationDefaults.AuthenticationScheme);
    }
}