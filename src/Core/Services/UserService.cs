using Core.Enums;
using Core.Helpers;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;

namespace Core.Services;

public class UserService(
    IUserRepository repository,
    TimeProvider timeProvider) : IUserService
{
    public async Task<User> CreateAsync(CreateUserRequest createUserRequest)
    {
        var passwordSalt = new PasswordSalt(Convert.ToBase64String(SecurityTokenHelper.GenerateSalt()));
        var passwordHash = AuthenticationService.HashPassword(createUserRequest.Password, passwordSalt);

        var user = new User
        {
            UserId = new UserId(Guid.NewGuid()),
            Username = createUserRequest.Username,
            EmailAddress = createUserRequest.EmailAddress,
            PasswordHash = new PasswordHash(passwordHash),
            PasswordSalt = passwordSalt,
            UserRole = UserRole.User,
            CreatedBy = createUserRequest.Username,
            CreatedAt = new CreatedAt(timeProvider.GetUtcNow().UtcDateTime),
            UpdatedBy = createUserRequest.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime),
            IsDeleted = new IsDeleted(false),
            DeletedBy = null,
            DeletedAt = null
        };

        await repository.CreateAsync(user);
        return user;
    }

    public Task<int> GetCountAsync(PagedRequest pagedRequest)
    {
        return repository.GetCountAsync(pagedRequest);
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

    public async Task DeleteAsync(UserId userId, Username deletedBy)
    {
        var user = await GetAsync(userId);
        user = user with
        {
            IsDeleted = new IsDeleted(true),
            DeletedBy = deletedBy,
            DeletedAt = new DeletedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        await UpdateAsync(user);
    }

    public Task<User> GetAsync(UserId userId)
    {
        return repository.GetByIdAsync(userId);
    }
}