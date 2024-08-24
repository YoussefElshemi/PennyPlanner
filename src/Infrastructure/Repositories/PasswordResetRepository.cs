using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PasswordResetRepository(
    PennyPlannerDbContext context) : IPasswordResetRepository
{
    public async Task CreateAsync(PasswordReset passwordReset)
    {
        var passwordResetEntity = PasswordResetMapper.MapToEntity(passwordReset);
        context.PasswordResets.Add(passwordResetEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PasswordReset passwordReset)
    {
        var passwordResetToUpdate = PasswordResetMapper.MapToEntity(passwordReset);
        var passwordResetEntity = await context.PasswordResets.SingleAsync(x => x.PasswordResetId == passwordResetToUpdate.PasswordResetId);

        passwordResetEntity.IsUsed = passwordResetToUpdate.IsUsed;
        passwordResetEntity.UpdatedBy = passwordResetToUpdate.UpdatedBy;
        passwordResetEntity.UpdatedAt = passwordResetToUpdate.UpdatedAt;

        context.PasswordResets.Update(passwordResetEntity);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(string passwordResetToken)
    {
        return context.PasswordResets.AnyAsync(x => x.ResetToken == passwordResetToken);
    }

    public async Task<PasswordReset> GetAsync(string passwordResetToken)
    {
        var passwordResetEntity = await context.PasswordResets
            .Include(x => x.UserEntity)
            .FirstAsync(u => u.ResetToken == passwordResetToken);

        return PasswordResetMapper.MapFromEntity(passwordResetEntity);
    }
}