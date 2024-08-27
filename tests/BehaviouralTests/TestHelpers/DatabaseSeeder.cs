using Infrastructure;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BehaviouralTests.TestHelpers;

public static class DatabaseSeeder
{
    public static async Task InsertUser(IServiceProvider serviceProvider, UserEntity existingUserEntity)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.Users.AddAsync(existingUserEntity);
        await context.SaveChangesAsync();
    }
}