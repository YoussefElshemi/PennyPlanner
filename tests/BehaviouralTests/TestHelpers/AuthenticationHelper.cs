using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Configs;
using Core.Enums;
using Core.Models;
using Core.ValueObjects;
using Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace BehaviouralTests.TestHelpers;

public static class AuthenticationHelper
{
    public static string CreateAccessToken(User user, int tokenLifeTime)
    {
        var appConfig = new AppConfig();
        ConfigurationHelper.GetSection(nameof(AppConfig)).Bind(appConfig);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.JwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        var jwtSecurityToken = new JwtSecurityToken(
            appConfig.JwtConfig.Issuer,
            appConfig.JwtConfig.Audience,
            expires: DateTime.UtcNow.AddMinutes(tokenLifeTime),
            notBefore: DateTime.UtcNow.AddMinutes(tokenLifeTime).AddMinutes(-10),
            claims: claims,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}