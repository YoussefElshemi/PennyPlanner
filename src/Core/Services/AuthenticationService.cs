using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Core.Services;

public class AuthenticationService(
    IUserRepository userRepository) : IAuthenticationService
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

        return new AuthenticationResponse
        {
            UserId = user.UserId
        };
    }
}