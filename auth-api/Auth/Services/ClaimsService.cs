using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Auth.Services;

public static class ClaimsService
{
    public static IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            OpenIddictConstants.Claims.Name
            or OpenIddictConstants.Claims.Subject
                =>
                [
                    OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken
                ],
            _ => [OpenIddictConstants.Destinations.AccessToken]
        };
    }
}
