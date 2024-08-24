using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface ILoginService
{
    Task<Login> CreateAsync(User user, IpAddress ipAddress);
    Task<Login> GetAsync(RefreshToken refreshToken);
    Task UpdateAsync(Login login);
}