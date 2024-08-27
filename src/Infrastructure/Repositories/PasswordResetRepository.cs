using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace Infrastructure.Repositories;

public class PasswordResetRepository(
    PennyPlannerDbContext context,
    IMapper mapper) : IPasswordResetRepository
{
    public async Task CreateAsync(PasswordReset passwordReset)
    {
        var passwordResetEntity = mapper.Map<PasswordResetEntity>(passwordReset);
        context.PasswordResets.Add(passwordResetEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PasswordReset passwordReset)
    {
        var passwordResetToUpdate = mapper.Map<PasswordResetEntity>(passwordReset);
        var passwordResetEntity = await context.PasswordResets.FirstAsync(x => x.PasswordResetId == passwordResetToUpdate.PasswordResetId);

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

        return mapper.Map<PasswordReset>(passwordResetEntity);
    }
}