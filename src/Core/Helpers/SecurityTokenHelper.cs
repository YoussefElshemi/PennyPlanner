using System.Security.Cryptography;

namespace Core.Helpers;

public static class SecurityTokenHelper
{
    public static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(GenerateRandomValue());
    }

    public static byte[] GenerateSalt()
    {
        return GenerateRandomValue();
    }

    private static byte[] GenerateRandomValue()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);

        return bytes;
    }
}