using Infrastructure;
using Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BehaviouralTests.TestHelpers;

public static class DatabaseSeeder
{
    public static async Task InsertUser(IServiceProvider serviceProvider, UserEntity userEntity)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

        await context.Users.AddAsync(userEntity);
        await context.SaveChangesAsync();
    }

    public static async Task InsertUsers(IServiceProvider serviceProvider, List<UserEntity> userEntities)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

        await context.Users.AddRangeAsync(userEntities);
        await context.SaveChangesAsync();
    }

    public static async Task InsertEmails(IServiceProvider serviceProvider, List<EmailMessageOutboxEntity> emailEntities)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

        await context.Emails.AddRangeAsync(emailEntities);
        await context.SaveChangesAsync();
    }
}