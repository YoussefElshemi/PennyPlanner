using System.Security.Cryptography;
using System.Text;
using Core.Enums;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;

namespace Core.Services;

public class UserService(IUserRepository repository,
    TimeProvider timeProvider) : IUserService
{
    public async Task<User> CreateUserAsync(CreateUserRequest createUserRequest)
    {
        var passwordSalt = AuthenticationService.GenerateSalt();
        var passwordHash = AuthenticationService.HashPassword(createUserRequest.Password.ToString(), passwordSalt);

        var user = new User
        {
            UserId = new UserId(Guid.NewGuid()),
            Username = createUserRequest.Username,
            EmailAddress = createUserRequest.EmailAddress,
            PasswordHash = new PasswordHash(passwordHash),
            PasswordSalt = new PasswordSalt(Convert.ToBase64String(passwordSalt)),
            UserRole = UserRole.User,
            CreatedBy = new CreatedBy(Guid.NewGuid()), // TODO: auto create admin user to use here
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await repository.CreateAsync(user);
        return user;
    }

    public Task<bool> ExistsAsync(Username username)
    {
        return repository.ExistsByUsernameAsync(username);
    }

    public Task<User?> GetUserAsync(Username username)
    {
        return repository.GetByUsernameAsync(username);
    }
}