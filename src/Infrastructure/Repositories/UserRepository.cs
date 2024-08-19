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
        return context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return userEntity == null ? null : UserMapper.MapFromEntity(userEntity);
    }

    public Task<bool> ExistsByUsernameAsync(string username)
    {
        return context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var userEntity = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return userEntity == null ? null : UserMapper.MapFromEntity(userEntity);
    }
}