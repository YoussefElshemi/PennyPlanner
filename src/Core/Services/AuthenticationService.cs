using System.IdentityModel.Tokens.Jwt;
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
    public async Task<AuthenticationResponse> CreateUserAsync(CreateUserRequest createUserRequest)
    {
        var user = await userService.CreateUserAsync(createUserRequest);
        var jwtSecurityToken = user.CreateJwtSecurityToken(config.Value.JwtConfig);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var expiresIn = Convert.ToInt32((jwtSecurityToken.ValidTo - DateTime.UtcNow).TotalSeconds);

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
       var user = await userService.GetUserAsync(authenticationRequest.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        var jwtSecurityToken = user.CreateJwtSecurityToken(config.Value.JwtConfig);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var expiresIn = Convert.ToInt32((jwtSecurityToken.ValidTo - DateTime.UtcNow).TotalSeconds);

        return new AuthenticationResponse
        {
            UserId = user.UserId,
            TokenType = TokenType.Bearer,
            AccessToken = new AccessToken(accessToken),
            ExpiresIn = new ExpiresIn(expiresIn)
        };
    }
}