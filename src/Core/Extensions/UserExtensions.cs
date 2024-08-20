using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Configs;
using Core.Models;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Core.Extensions;

public static class UserExtensions
{
    public static bool Authenticate(this User user, string password)
    {
        return user.PasswordHash == password.Md5Hash();
    }

    public static JwtSecurityToken CreateJwtSecurityToken(this User user, JwtConfig config)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.EmailAddress),
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        return new JwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            expires: DateTime.Now.AddMinutes(config.Lifetime),
            claims: claims,
            signingCredentials: credentials
        );
    }
}