using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; init; }
    public DbSet<PasswordResetEntity> PasswordResets { get; init; }
    public DbSet<LoginEntity> Logins { get; init; }
    public DbSet<OneTimePasscodeEntity> OneTimePasscodes { get; init; }
    public DbSet<EmailMessageOutboxEntity> Emails { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagementDbContext).Assembly);
    }
}