using System.Security.Cryptography;

namespace Auth.Services;

public static class SecretsService
{
    public static string Generate(int length = 16)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);

        var secret = Convert.ToBase64String(bytes);

        return secret;
    }
}
