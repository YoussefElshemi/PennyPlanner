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
    public async Task<User> CreateAsync(CreateUserRequest createUserRequest)
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
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await repository.CreateAsync(user);
        return user;
    }

    public Task<int> GetCountAsync()
    {
        return repository.GetCountAsync();
    }

    public async Task<User> ChangePasswordAsync(User user, Password password)
    {
        var updatedUser = user.UpdatePassword(password) with
        {
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };
        await UpdateAsync(updatedUser);

        return updatedUser;
    }

    public Task<PagedResponse<User>> GetAllAsync(PagedRequest pagedRequest)
    {
        return repository.GetAllAsync(pagedRequest);
    }

    public async Task<User> UpdateAsync(User user)
    {
        await repository.UpdateAsync(user);

        return user;
    }

    public Task<bool> ExistsAsync(Username username)
    {
        return repository.ExistsByUsernameAsync(username);
    }

    public Task<bool> ExistsAsync(EmailAddress emailAddress)
    {
        return repository.ExistsByEmailAddressAsync(emailAddress);
    }

    public Task<bool> ExistsAsync(UserId userId)
    {
        return repository.ExistsByIdAsync(userId);
    }

    public Task<User> GetAsync(Username username)
    {
        return repository.GetByUsernameAsync(username);
    }

    public Task<User> GetAsync(EmailAddress emailAddress)
    {
        return repository.GetByEmailAddressAsync(emailAddress);
    }

    public Task<User> GetAsync(UserId userId)
    {
        return repository.GetByIdAsync(userId);
    }
}