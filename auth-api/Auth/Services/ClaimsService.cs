using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Auth.Services;

public static class ClaimsService
{
    public static IEnumerable<string> GetDestinations(Claim claim)
    {
        switch (claim.Type)
        {
            case OpenIddictConstants.Claims.Name
            or OpenIddictConstants.Claims.Subject:
            {
                return
                [
                    OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken,
                ];
            }
            case OpenIddictConstants.Claims.Email:
            {
                if ((claim.Subject?.HasScope(OpenIddictConstants.Scopes.Email)).GetValueOrDefault())
                {
                    return
                    [
                        OpenIddictConstants.Destinations.IdentityToken,
                        OpenIddictConstants.Destinations.AccessToken,
                    ];
                }

                return [OpenIddictConstants.Destinations.AccessToken];
            }
            default:
                return [OpenIddictConstants.Destinations.AccessToken];
        }
    }
}
