using AutoMapper;
using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OneTimePasscodeRepository(
    UserManagementDbContext context,
    IMapper mapper) : IOneTimePasscodeRepository
{
    public async Task CreateAsync(OneTimePasscode oneTimePasscode)
    {
        var oneTimePasscodeEntity = mapper.Map<OneTimePasscodeEntity>(oneTimePasscode);
        context.OneTimePasscodes.Add(oneTimePasscodeEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(OneTimePasscode oneTimePasscode)
    {
        var oneTimePasscodeToUpdate = mapper.Map<OneTimePasscodeEntity>(oneTimePasscode);
        var oneTimePasscodeEntity = await context.OneTimePasscodes.FirstAsync(x => x.OneTimePasscodeId == oneTimePasscodeToUpdate.OneTimePasscodeId);

        oneTimePasscodeEntity.IsUsed = oneTimePasscodeToUpdate.IsUsed;
        oneTimePasscodeEntity.UpdatedBy = oneTimePasscodeToUpdate.UpdatedBy;
        oneTimePasscodeEntity.UpdatedAt = oneTimePasscodeToUpdate.UpdatedAt;

        context.OneTimePasscodes.Update(oneTimePasscodeEntity);
        await context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(Guid userId, string passcode)
    {
        return context.OneTimePasscodes.AnyAsync(x => x.UserId == userId && x.Passcode == passcode);
    }

    public async Task<OneTimePasscode> GetAsync(Guid userId, string passcode)
    {
        var oneTimePasscodeEntity = await context.OneTimePasscodes
            .Include(x => x.UserEntity)
            .FirstAsync(u => u.UserId == userId && u.Passcode == passcode);

        return mapper.Map<OneTimePasscode>(oneTimePasscodeEntity);
    }
}