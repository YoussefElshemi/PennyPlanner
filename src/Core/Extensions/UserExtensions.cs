using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Configs;
using Core.Helpers;
using Core.Models;
using Core.Services;
using Core.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Core.Extensions;

public static class UserExtensions
{
    public static bool Authenticate(this User user, string password)
    {
        var salt = Convert.FromBase64String(user.PasswordSalt.ToString());
        return user.PasswordHash == AuthenticationService.HashPassword(password, salt);
    }

    public static User UpdatePassword(this User user, Password password)
    {
        var passwordSalt = SecurityTokenHelper.GenerateSalt();
        var passwordHash = AuthenticationService.HashPassword(password, passwordSalt);

        var updatedUser = user with
        {
            PasswordHash = new PasswordHash(passwordHash),
            PasswordSalt = new PasswordSalt(Convert.ToBase64String(passwordSalt))
        };

        return updatedUser;
    }


    public static JwtSecurityToken CreateJwtSecurityToken(this User user, JwtConfig config)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        return new JwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            expires: DateTime.UtcNow.AddMinutes(config.AccessTokenLifetimeInMinutes),
            notBefore: DateTime.UtcNow,
            claims: claims,
            signingCredentials: credentials
        );
    }
}