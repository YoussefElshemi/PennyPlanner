using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LoginRepository(
    PennyPlannerDbContext context) : ILoginRepository
{
    public async Task CreateAsync(Login login)
    {
        var loginEntity = LoginMapper.MapToEntity(login);
        context.Logins.Add(loginEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Login login)
    {
        var loginToUpdate = LoginMapper.MapToEntity(login);
        var loginEntity = await context.Logins.SingleAsync(x => x.LoginId == loginToUpdate.LoginId);

        loginEntity.IsRevoked = loginToUpdate.IsRevoked;
        loginEntity.RevokedAt = loginToUpdate.RevokedAt;
        loginEntity.UpdatedAt = loginToUpdate.UpdatedAt;

        context.Logins.Update(loginEntity);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(string refreshToken)
    {
        return context.Logins.AnyAsync(x => x.RefreshToken == refreshToken);
    }

    public async Task<Login> GetAsync(string refreshToken)
    {
        var loginEntity = await context.Logins
            .Include(x => x.UserEntity)
            .FirstAsync(u => u.RefreshToken == refreshToken);

        return LoginMapper.MapFromEntity(loginEntity);
    }
}