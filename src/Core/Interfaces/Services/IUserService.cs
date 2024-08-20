using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserRequest createUserRequest);
    Task<bool> ExistsAsync(UserId userId);
    Task<bool> ExistsAsync(Username username);
    Task<User?> GetUserAsync(Username username);
    Task<User?> GetUserAsync(UserId userId);
}