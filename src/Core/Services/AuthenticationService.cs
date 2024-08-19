using System.IdentityModel.Tokens.Jwt;
using Core.Configs;
using Core.Enums;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;

namespace Core.Services;

public class AuthenticationService(
    IUserRepository userRepository,
    IOptions<AppConfig> config) : IAuthenticationService
{
    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest)
    {
        var userExists = await userRepository.ExistsByUsernameAsync(authenticationRequest.Username);
        var user = await userRepository.GetUserByUsernameAsync(authenticationRequest.Username);

        if (!userExists || user == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (!user.Authenticate(authenticationRequest.Password))
        {
            throw new UnauthorizedAccessException();
        }

        var jwtSecurityToken = user.CreateJwtSecurityToken(config.Value.JwtConfig);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new AuthenticationResponse
        {
            UserId = user.UserId,
            Token = new AuthenticationToken(token),
            TokenType = TokenType.Bearer
        };
    }
}