using AutoMapper;
using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(
    PennyPlannerDbContext context,
    IMapper mapper) : PagedRepository<UserEntity>(context, mapper), IUserRepository
{
    private readonly IMapper _mapper = mapper;

    public override IDictionary<string, string> GetSortableFields()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(User.Username), nameof(UserEntity.Username) },
            { nameof(User.EmailAddress), nameof(UserEntity.EmailAddress) },
            { nameof(User.CreatedAt), nameof(UserEntity.CreatedAt) },
            { nameof(User.UpdatedAt), nameof(UserEntity.UpdatedAt) }
        };
    }

    public override IDictionary<string, string> GetSearchableFields()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(User.Username), nameof(UserEntity.Username) },
            { nameof(User.EmailAddress), nameof(UserEntity.EmailAddress) },
            { nameof(User.UserRole), nameof(UserEntity.UserRoleId) },
            { nameof(User.CreatedBy), nameof(UserEntity.CreatedBy) },
            { nameof(User.UpdatedBy), nameof(UserEntity.UpdatedBy) }
        };
    }

    public Task<bool> ExistsByIdAsync(Guid userId)
    {
        return context.Users
            .Where(x => !x.IsDeleted)
            .AnyAsync(u => u.UserId == userId);
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(u => u.UserId == userId);

        return _mapper.Map<User>(userEntity);
    }

    public Task<bool> ExistsByUsernameAsync(string username)
    {
        return context.Users
            .Where(x => !x.IsDeleted)
            .AnyAsync(u => u.Username == username);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(u => u.Username == username);

        return _mapper.Map<User>(userEntity);
    }

    public Task<bool> ExistsByEmailAddressAsync(string emailAddress)
    {
        return context.Users
            .Where(x => !x.IsDeleted)
            .AnyAsync(u => u.EmailAddress == emailAddress);
    }

    public async Task<User> GetByEmailAddressAsync(string emailAddress)
    {
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(u => u.EmailAddress == emailAddress);

        return _mapper.Map<User>(userEntity);
    }

    public async Task CreateAsync(User user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        context.Users.Add(userEntity);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var userToUpdate = _mapper.Map<UserEntity>(user);
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(x => x.UserId == userToUpdate.UserId);

        userEntity.Username = userToUpdate.Username;
        userEntity.EmailAddress = userToUpdate.EmailAddress;
        userEntity.PasswordHash = userToUpdate.PasswordHash;
        userEntity.PasswordSalt = userToUpdate.PasswordSalt;
        userEntity.UserRoleId = userToUpdate.UserRoleId;
        userEntity.UpdatedBy = userToUpdate.UpdatedBy;
        userEntity.UpdatedAt = userToUpdate.UpdatedAt;
        userEntity.IsDeleted = userToUpdate.IsDeleted;
        userEntity.DeletedBy = userToUpdate.DeletedBy;
        userEntity.DeletedAt = userToUpdate.DeletedAt;

        context.Users.Update(userEntity);
        await context.SaveChangesAsync();
    }
}