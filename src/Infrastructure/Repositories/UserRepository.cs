using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(
    PennyPlannerDbContext context) : IUserRepository
{
    public async Task<PagedResponse<User>> GetAllAsync(PagedRequest pagedRequest)
    {
        var totalCount = await GetCountAsync();
        var pageCount = (totalCount + pagedRequest.PageSize - 1) / pagedRequest.PageSize;

        var users = await context.Users
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize)
            .Select(entity => UserMapper.MapFromEntity(entity))
            .ToListAsync();

        return new PagedResponse<User>
        {
            PageNumber = pagedRequest.PageNumber,
            PageSize = pagedRequest.PageSize,
            PageCount = new PageCount(pageCount),
            TotalCount = new TotalCount(totalCount),
            HasMore = new HasMore(pagedRequest.PageNumber < pageCount),
            Data = users
        };
    }

    public async Task<int> GetCountAsync()
    {
        var query = context.Users.AsQueryable();
        var totalCount = await query.CountAsync();

        return totalCount;
    }

    public Task<bool> ExistsByIdAsync(Guid userId)
    {
        return context.Users.AnyAsync(u => u.UserId == userId);
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var userEntity = await context.Users.FirstAsync(u => u.UserId == userId);
        return UserMapper.MapFromEntity(userEntity);
    }

    public Task<bool> ExistsByUsernameAsync(string username)
    {
        return context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var userEntity = await context.Users.FirstAsync(u => u.Username == username);
        return UserMapper.MapFromEntity(userEntity);
    }

    public Task<bool> ExistsByEmailAddressAsync(string emailAddress)
    {
        return context.Users.AnyAsync(u => u.EmailAddress == emailAddress);
    }

    public async Task<User> GetByEmailAddressAsync(string emailAddress)
    {
        var userEntity = await context.Users.FirstAsync(u => u.EmailAddress == emailAddress);
        return UserMapper.MapFromEntity(userEntity);
    }

    public async Task CreateAsync(User user)
    {
        var userEntity = UserMapper.MapToEntity(user);
        context.Users.Add(userEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var userToUpdate = UserMapper.MapToEntity(user);
        var userEntity = await context.Users.SingleAsync(x => x.UserId == userToUpdate.UserId);

        userEntity.EmailAddress = userToUpdate.EmailAddress;
        userEntity.PasswordHash = userToUpdate.PasswordHash;
        userEntity.PasswordSalt = userToUpdate.PasswordSalt;
        userEntity.UserRoleId = userToUpdate.UserRoleId;
        userEntity.UpdatedAt = userToUpdate.UpdatedAt;

        context.Users.Update(userEntity);
        await context.SaveChangesAsync();
    }
}