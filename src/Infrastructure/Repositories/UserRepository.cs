using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(
    PennyPlannerDbContext context) : IUserRepository
{
    public Task<bool> ExistsByIdAsync(Guid userId)
    {
        return context.Users.AnyAsync(u => u.UserId == userId);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return userEntity == null ? null : UserMapper.MapFromEntity(userEntity);
    }

    public Task<bool> ExistsByUsernameAsync(string username)
    {
        return context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return userEntity == null ? null : UserMapper.MapFromEntity(userEntity);
    }

    public Task<bool> ExistsByEmailAddressAsync(string emailAddress)
    {
        return context.Users.AnyAsync(u => u.EmailAddress == emailAddress);
    }

    public async Task<User?> GetByEmailAddressAsync(string emailAddress)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
        return userEntity == null ? null : UserMapper.MapFromEntity(userEntity);
    }

    public async Task CreateAsync(User user)
    {
        var userEntity = UserMapper.MapToEntity(user);
        context.Users.Add(userEntity);
        await context.SaveChangesAsync();
    }
}