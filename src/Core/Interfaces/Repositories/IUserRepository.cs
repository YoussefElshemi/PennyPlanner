using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<bool> ExistsByIdAsync(Guid userId);
    public Task<User?> GetUserByIdAsync(Guid userId);
    public Task<bool> ExistsByUsernameAsync(string username);
    public Task<User?> GetUserByUsernameAsync(string username);
}