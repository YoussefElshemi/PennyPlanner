using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Core.Configs;
using Core.Enums;
using Core.Extensions;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;

namespace Core.Services;

public class AuthenticationService(
    IUserService userService,
    IOptions<AppConfig> config) : IAuthenticationService
{
    public async Task<AuthenticationResponse> CreateAsync(CreateUserRequest createUserRequest)
    {
        var user = await userService.CreateAsync(createUserRequest);
        var jwtSecurityToken = user.CreateJwtSecurityToken(config.Value.JwtConfig);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var expiresIn = Convert.ToInt32((jwtSecurityToken.ValidTo - jwtSecurityToken.ValidFrom).TotalSeconds);

        return new AuthenticationResponse
        {
            UserId = user.UserId,
            TokenType = TokenType.Bearer,
            AccessToken = new AccessToken(accessToken),
            ExpiresIn = new ExpiresIn(expiresIn),
        };
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest)
    {
       var user = await userService.GetAsync(authenticationRequest.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var jwtSecurityToken = user.CreateJwtSecurityToken(config.Value.JwtConfig);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var expiresIn = Convert.ToInt32((jwtSecurityToken.ValidTo - jwtSecurityToken.ValidFrom).TotalSeconds);

        return new AuthenticationResponse
        {
            UserId = user.UserId,
            TokenType = TokenType.Bearer,
            AccessToken = new AccessToken(accessToken),
            ExpiresIn = new ExpiresIn(expiresIn)
        };
    }

    public static string HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        var hashedBytes = SHA256.HashData(saltedPassword);

        var hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
        Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
        Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

        return Convert.ToBase64String(hashedPasswordWithSalt);
    }

    public static byte[] GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[32];
        rng.GetBytes(salt);

        return salt;
    }
}