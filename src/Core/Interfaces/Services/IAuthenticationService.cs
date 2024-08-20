using Core.Models;

namespace Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> CreateUserAsync(CreateUserRequest createUserRequest);
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
}