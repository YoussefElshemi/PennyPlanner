using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> CreateAsync(CreateUserRequest createUserRequest);
    Task<AuthenticationResponse> AuthenticateAsync(User user, AuthenticationRequest authenticationRequest);
    Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
    Task<AuthenticationResponse>  TwoFactorAuthenticationAsync(TwoFactorRequest refreshTokenRequest);
    Task RequestTwoFactorAsync(User user, AuthenticationRequest authenticationRequest);
    Task RevokeToken(RefreshTokenRequest refreshTokenRequest);
    Task<User> ChangePasswordAsync(User user, Password password);
    Task RequestPasswordReset(RequestPasswordResetRequest requestPasswordResetRequest);
    Task ResetPassword(ResetPasswordRequest resetPasswordRequest);
    bool Authenticate(User user, Password password);
}