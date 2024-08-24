using Core.Models;

namespace Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> CreateAsync(CreateUserRequest createUserRequest);
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
    Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
    Task RequestResetPassword(RequestResetPasswordRequest requestResetPasswordRequest);
    Task ResetPassword(ResetPasswordRequest resetPasswordRequest);
}