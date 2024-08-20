using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsByIdAsync(Guid userId);
    Task<User?> GetByIdAsync(Guid userId);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsByEmailAddressAsync(string emailAddress);
    Task<User?> GetByEmailAddressAsync(string emailAddress);

    Task CreateAsync(User user);
}