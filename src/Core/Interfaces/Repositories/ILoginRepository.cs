using Core.Models;

namespace Core.Interfaces.Repositories;

public interface ILoginRepository
{
    Task CreateAsync(Login login);
    Task UpdateAsync(Login login);
    Task<bool> ExistsAsync(string refreshToken);
    Task<Login> GetAsync(string refreshToken);
}