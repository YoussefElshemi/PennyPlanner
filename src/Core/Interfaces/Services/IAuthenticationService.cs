using Core.Models;

namespace Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> CreateAsync(CreateUserRequest createUserRequest);
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
    Task RequestResetPassword(RequestResetPasswordRequest requestResetPasswordRequest);
}