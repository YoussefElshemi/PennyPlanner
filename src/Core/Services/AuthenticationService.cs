using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
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
    IPasswordResetService passwordResetService,
    ILoginService loginService,
    TimeProvider timeProvider,
    IOptions<AppConfig> config) : IAuthenticationService
{
    public async Task<AuthenticationResponse> CreateAsync(CreateUserRequest createUserRequest)
    {
        var user = await userService.CreateAsync(createUserRequest);
        var login = await loginService.CreateAsync(user, createUserRequest.IpAddress);

        return HandleAuthentication(login);
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest)
    {
        var user = await userService.GetAsync(authenticationRequest.Username);
        var login = await loginService.CreateAsync(user, authenticationRequest.IpAddress);

        return HandleAuthentication(login);
    }

    public async Task<AuthenticationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        var login = await loginService.GetAsync(refreshTokenRequest.RefreshToken);
        return HandleAuthentication(login);
    }

    public async Task RevokeToken(RefreshTokenRequest refreshTokenRequest)
    {
        var login = await loginService.GetAsync(refreshTokenRequest.RefreshToken);

        login = login with
        {
            IsRevoked = new IsRevoked(true),
            RevokedAt = new RevokedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await loginService.UpdateAsync(login);
    }

    public async Task RequestResetPassword(RequestResetPasswordRequest requestResetPasswordRequest)
    {
        var userExists = await userService.ExistsAsync(requestResetPasswordRequest.EmailAddress);
        if (!userExists)
        {
            return;
        }

        var user = await userService.GetAsync(requestResetPasswordRequest.EmailAddress);
        await passwordResetService.InitiateAsync(user);
    }

    public async Task ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        var passwordReset = await passwordResetService.GetAsync(resetPasswordRequest.PasswordResetToken);

        passwordReset = passwordReset with
        {
            IsUsed = new IsUsed(true),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };
        await passwordResetService.UpdateAsync(passwordReset);

        await userService.ChangePasswordAsync(passwordReset.User, resetPasswordRequest.Password);
    }

    public static string HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPassword = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        var hashedBytes = SHA256.HashData(saltedPassword);

        var hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
        Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
        Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

        return Convert.ToBase64String(hashedPasswordWithSalt);
    }

    private AuthenticationResponse HandleAuthentication(Login login)
    {
        var jwtSecurityToken = login.User.CreateJwtSecurityToken(config.Value.JwtConfig);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var accessTokenExpiresIn = Convert.ToInt32((jwtSecurityToken.ValidTo - jwtSecurityToken.ValidFrom).TotalSeconds);

        var refreshTokenExpiresIn = Convert.ToInt32((login.ExpiresAt - timeProvider.GetUtcNow().UtcDateTime).TotalSeconds);

        return new AuthenticationResponse
        {
            UserId = login.User.UserId,
            TokenType = TokenType.Bearer,
            AccessToken = new AccessToken(accessToken),
            AccessTokenExpiresIn = new ExpiresIn(accessTokenExpiresIn),
            RefreshToken = login.RefreshToken,
            RefreshTokenExpiresIn = new ExpiresIn(refreshTokenExpiresIn)
        };
    }
}