using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PennyPlannerDbContext(DbContextOptions<PennyPlannerDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; init; }
    public DbSet<PasswordResetEntity> PasswordResets { get; init; }
    public DbSet<LoginEntity> Logins { get; init; }
    public DbSet<EmailMessageOutboxEntity> Emails { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PennyPlannerDbContext).Assembly);
    }
}