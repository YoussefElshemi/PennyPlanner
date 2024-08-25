using Core.Configs;
using Core.Helpers;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using Microsoft.Extensions.Options;

namespace Core.Services;

public class LoginService(
    ILoginRepository loginRepository,
    IOptions<AppConfig> config,
    TimeProvider timeProvider) : ILoginService
{
    public async Task<Login> CreateAsync(User user, IpAddress ipAddress)
    {
        var login = new Login
        {
            LoginId = new LoginId(Guid.NewGuid()),
            UserId = user.UserId,
            IpAddress = ipAddress,
            RefreshToken = new RefreshToken(SecurityTokenHelper.GenerateRefreshToken()),
            ExpiresAt = new ExpiresAt(timeProvider.GetUtcNow().UtcDateTime.AddMinutes(config.Value.JwtConfig.RefreshTokenLifetimeInMinutes)),
            IsRevoked = new IsRevoked(false),
            CreatedBy = user.Username,
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedBy = user.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime),
            User = user
        };

        await loginRepository.CreateAsync(login);

        return login;
    }

    public Task UpdateAsync(Login login)
    {
        return loginRepository.UpdateAsync(login);
    }

    public Task<Login> GetAsync(RefreshToken refreshToken)
    {
        return loginRepository.GetAsync(refreshToken);
    }
}