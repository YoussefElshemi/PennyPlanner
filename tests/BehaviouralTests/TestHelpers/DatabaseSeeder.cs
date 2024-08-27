using Infrastructure;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BehaviouralTests.TestHelpers;

public static class DatabaseSeeder
{
    public static async Task InsertUser(IServiceProvider serviceProvider, UserEntity userEntity)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.Users.AddAsync(userEntity);
        await context.SaveChangesAsync();
    }

    public static async Task InsertUsers(IServiceProvider serviceProvider, List<UserEntity> userEntities)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PennyPlannerDbContext>();

        await context.Users.AddRangeAsync(userEntities);
        await context.SaveChangesAsync();
    }
}