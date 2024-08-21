using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<User> CreateAsync(CreateUserRequest createUserRequest);
    Task UpdateAsync(User user);
    Task<bool> ExistsAsync(UserId userId);
    Task<bool> ExistsAsync(Username username);
    Task<User?> GetAsync(Username username);
    Task<User?> GetAsync(UserId userId);
}