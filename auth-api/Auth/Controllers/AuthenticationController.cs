using System.Security.Claims;
using Auth.Constants;
using Auth.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace Auth.Controllers;

public class AuthenticationController(UserManager<User> userManager) : ControllerBase
{
    [HttpGet("~/login/password"), HttpPost("~/login/password"), IgnoreAntiforgeryToken]
    public async Task<ActionResult> LogInPassword(string username, string password)
    {
        var identity = new ClaimsIdentity(
            authenticationType: AuthenticationTypeConstants.Password,
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role
        );

        return SignIn(
            new ClaimsPrincipal(identity),
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}
