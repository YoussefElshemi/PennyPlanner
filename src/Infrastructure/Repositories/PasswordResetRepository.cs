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
        passwordResetEntity.UpdatedAt = passwordResetToUpdate.UpdatedAt;

        context.PasswordResets.Update(passwordResetEntity);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(Guid passwordResetToken)
    {
        return context.PasswordResets.AnyAsync(x => x.ResetToken == passwordResetToken);
    }

    public async Task<PasswordReset?> GetAsync(Guid passwordResetToken)
    {
        var passwordResetEntity = await context.PasswordResets
            .Include(x => x.UserEntity)
            .FirstOrDefaultAsync(u => u.ResetToken == passwordResetToken);

        return passwordResetEntity == null ? null : PasswordResetMapper.MapFromEntity(passwordResetEntity);

    }
}